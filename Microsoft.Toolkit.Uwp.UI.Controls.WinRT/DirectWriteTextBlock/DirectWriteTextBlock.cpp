// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

#include "pch.h"
#include "DirectWriteTextBlock.h"
#include <windows.ui.xaml.media.dxinterop.h>

BEGIN_NAMESPACE_CONTROLS_WINRT

using namespace Platform;
using namespace Windows::Foundation;
using namespace Windows::Foundation::Collections;
using namespace Windows::Graphics::Display;
using namespace Windows::UI::Xaml;
using namespace Windows::UI::Xaml::Controls;
using namespace Windows::UI::Xaml::Data;
using namespace Windows::UI::Xaml::Documents;
using namespace Windows::UI::Xaml::Input;
using namespace Windows::UI::Xaml::Interop;
using namespace Windows::UI::Xaml::Media;
using namespace Windows::UI::Xaml::Media::Imaging;
using namespace Windows::UI::ViewManagement;

DependencyProperty^ DirectWriteTextBlock::m_textProperty = DependencyProperty::Register(
    L"Text",
    Platform::String::typeid,
    DirectWriteTextBlock::typeid,
    ref new PropertyMetadata(L"", ref new PropertyChangedCallback(&DirectWriteTextBlock::OnDependencyPropertyChanged)));

DependencyProperty^ DirectWriteTextBlock::m_textLocaleProperty = DependencyProperty::Register(
    L"TextLocale",
    Platform::String::typeid,
    DirectWriteTextBlock::typeid,
    ref new PropertyMetadata(L"en-US", ref new PropertyChangedCallback(&DirectWriteTextBlock::OnDependencyPropertyChanged)));

DependencyProperty^ DirectWriteTextBlock::m_textReadingDirectionProperty = DependencyProperty::Register(
    L"TextReadingDirection",
    DirectWriteReadingDirection::typeid,
    DirectWriteTextBlock::typeid,
    ref new PropertyMetadata(ref new Platform::Box<DirectWriteReadingDirection>(DirectWriteReadingDirection::TopToBottom), ref new PropertyChangedCallback(&DirectWriteTextBlock::OnDependencyPropertyChanged)));

DependencyProperty^ DirectWriteTextBlock::m_textWrapProperty = DependencyProperty::Register(
    L"TextWrap",
    DirectWriteWordWrapping::typeid,
    DirectWriteTextBlock::typeid,
    ref new PropertyMetadata(ref new Platform::Box<DirectWriteWordWrapping>(DirectWriteWordWrapping::NoWrap), ref new PropertyChangedCallback(&DirectWriteTextBlock::OnDependencyPropertyChanged)));

DependencyProperty^ DirectWriteTextBlock::m_textAlignmentProperty = DependencyProperty::Register(
    L"TextAlign",
    DirectWriteTextAlignment::typeid,
    DirectWriteTextBlock::typeid,
    ref new PropertyMetadata(ref new Platform::Box<DirectWriteTextAlignment>(DirectWriteTextAlignment::Leading), ref new PropertyChangedCallback(&DirectWriteTextBlock::OnDependencyPropertyChanged)));


DirectWriteTextBlock::DirectWriteTextBlock()
{
	DefaultStyleKey = "Microsoft.Toolkit.Uwp.UI.Controls.WinRT.DirectWriteTextBlock";

    auto displayInfo = DisplayInformation::GetForCurrentView();
    m_dpiChangedToken = displayInfo->DpiChanged += ref new TypedEventHandler<DisplayInformation^, Object^>(this, &DirectWriteTextBlock::OnDpiChanged);

    m_accessibilitySettings = ref new AccessibilitySettings();
    m_highContrastChangedToken = m_accessibilitySettings->HighContrastChanged += ref new TypedEventHandler<AccessibilitySettings^, Platform::Object^>(this, &DirectWriteTextBlock::OnHighContrastSettingsChanged);
    m_isHighContrast = m_accessibilitySettings->HighContrast;

#define REGISTER_INHERITED_PROPERTY_CALLBACK(token, inheritedProperty) \
    token = RegisterPropertyChangedCallback(inheritedProperty, ref new DependencyPropertyChangedCallback(&DirectWriteTextBlock::OnInheritedDependencyPropertyChanged))

    REGISTER_INHERITED_PROPERTY_CALLBACK(m_flowDirectionChangedToken, FlowDirectionProperty);
    REGISTER_INHERITED_PROPERTY_CALLBACK(m_foregroundChangedToken, ForegroundProperty);
    REGISTER_INHERITED_PROPERTY_CALLBACK(m_fontSizeChangedToken, FontSizeProperty);
    REGISTER_INHERITED_PROPERTY_CALLBACK(m_fontFamilyChangedToken, FontFamilyProperty);
    REGISTER_INHERITED_PROPERTY_CALLBACK(m_fontStretchChangedToken, FontStretchProperty);
    REGISTER_INHERITED_PROPERTY_CALLBACK(m_fontStyleChangedToken, FontStyleProperty);
    REGISTER_INHERITED_PROPERTY_CALLBACK(m_fontWeightChangedToken, FontWeightProperty);
#undef REGISTER_INHERITED_PROPERTY_CALLBACK
}

DirectWriteTextBlock::~DirectWriteTextBlock()
{
    Close();
}

void DirectWriteTextBlock::OnApplyTemplate()
{
    // make XAML apply the template first
    __super::OnApplyTemplate();

    auto maybeImage = dynamic_cast<Image^>(GetTemplateChild(L"Image"));
    if (!maybeImage)
    {
        winrt::throw_hresult(E_NOT_VALID_STATE);
    }

    // this border is essentially just used to emulate the XAML text high contrast background.
    // the consumer can use it to set background to the text block, but normally, it should just be
    // left null.
    auto maybeBorder = dynamic_cast<Border^>(GetTemplateChild(L"TextBackground"));
    if (!maybeBorder)
    {
        winrt::throw_hresult(E_NOT_VALID_STATE);
    }

    m_image = maybeImage;
    m_textBackground = maybeBorder;
}

Size DirectWriteTextBlock::MeasureOverride(Size availableSize)
{
    UpdateTextBrushesForHighContrast();
    auto displayInfo = DisplayInformation::GetForCurrentView();
    DirectWriteRenderArgBuilder builder;
    builder.SetAvailableHeight(availableSize.Height);
    builder.SetAvailableWidth(availableSize.Width);
    builder.SetDPI(displayInfo->RawPixelsPerViewPixel);
    builder.SetFlowDirection(FlowDirection);
    builder.SetFontFamily(FontFamily);
    builder.SetFontSize(FontSize);
    builder.SetFontStretch(FontStretch);
    builder.SetFontStyle(FontStyle);
    builder.SetFontWeight(FontWeight);
    builder.SetForegroundBrush(m_textForegroundBrush);
    builder.SetText(Text);
    builder.SetTextLocale(TextLocale);
    builder.SetTextReadingDirection(TextReadingDirection);
    builder.SetTextWrapping(TextWrap);
    builder.SetTextAlignment(TextAlign);

    auto args = builder.BuildRenderArgs();
    UpdateElementsForHighContrast();

    auto resultSize = RenderText(args);

    // call __super::Measure after we've already set the source to the new image.
    __super::MeasureOverride(availableSize);
    return resultSize;
}

void DirectWriteTextBlock::UpdateTextBrushesForHighContrast()
{
    if (m_isHighContrast)
    {
        // XAML High Contrast TextBlock behavior emulation: XAML on high contrast basically sets
        // a background to the TextBlock in order to get text to always appear in High Contrast.
        // To emulate this, we basically look up the applicable text brushes from the system
        // resource dictionary and override any foreground/background the user may have set like
        // standard textblock would.
        
        auto resources = Application::Current->Resources;
        auto highContrastForeground = static_cast<Windows::UI::Color>(resources->Lookup(L"SystemColorWindowTextColor"));
        auto highContrastBackground = static_cast<Windows::UI::Color>(resources->Lookup(L"SystemColorWindowColor"));
        m_textForegroundBrush = ref new SolidColorBrush(highContrastForeground);
        m_textBackgroundBrush = ref new SolidColorBrush(highContrastBackground);
    }
    else
    {
        m_textForegroundBrush = this->Foreground;
        m_textBackgroundBrush = this->Background;
    }
}

void DirectWriteTextBlock::UpdateElementsForHighContrast()
{
    m_textBackground->Background = m_textBackgroundBrush;
}

void DirectWriteTextBlock::Close()
{

#define UNREGISTER_INHERITED_PROPERTY_CALLBACK(token, inheritedProperty) \
    if (token != 0) \
    { \
        UnregisterPropertyChangedCallback(inheritedProperty, token); \
        token = 0; \
    }

    UNREGISTER_INHERITED_PROPERTY_CALLBACK(m_flowDirectionChangedToken, FlowDirectionProperty);
    UNREGISTER_INHERITED_PROPERTY_CALLBACK(m_foregroundChangedToken, ForegroundProperty);
    UNREGISTER_INHERITED_PROPERTY_CALLBACK(m_fontSizeChangedToken, FontSizeProperty);
    UNREGISTER_INHERITED_PROPERTY_CALLBACK(m_fontFamilyChangedToken, FontFamilyProperty);
    UNREGISTER_INHERITED_PROPERTY_CALLBACK(m_fontStretchChangedToken, FontStretchProperty);
    UNREGISTER_INHERITED_PROPERTY_CALLBACK(m_fontStyleChangedToken, FontStyleProperty);
    UNREGISTER_INHERITED_PROPERTY_CALLBACK(m_fontWeightChangedToken, FontWeightProperty);

    if (m_dpiChangedToken.Value != 0)
    {
        auto displayInfo = DisplayInformation::GetForCurrentView();
        displayInfo->DpiChanged -= m_dpiChangedToken;
        m_dpiChangedToken = {};
    }

    if (m_highContrastChangedToken.Value != 0)
    {
        m_accessibilitySettings->HighContrastChanged -= m_highContrastChangedToken;
        m_highContrastChangedToken = {};
    }

#undef UNREGISTER_INHERITED_PROPERTY_CALLBACK
}

Size DirectWriteTextBlock::RenderText(DirectWriteTextRenderArgs const& args)
{
    if (args.text->IsEmpty())
    {
        return Size(0.0f, 0.0f);
    }

    auto resourceManager = DirectWriteResourceManager::GetInstance();
    auto scale = args.rawPixelsPerViewPixel;
    auto scaledFontSize = args.fontSize * scale;
    auto dwriteFactory = resourceManager->GetDirectWriteFactoryNoRef();

    winrt::com_ptr<IDWriteTextFormat> textFormat;
    winrt::check_hresult(dwriteFactory->CreateTextFormat(
        args.fontFamily->Data(),
        args.fontCollection.get(),
        args.fontWeight,
        args.fontStyle,
        args.fontStretch,
        scaledFontSize,
        args.textLocale->Data(),
        textFormat.put()));

    textFormat->SetWordWrapping(args.textWrapping);

    // Trying to set readingDirection + FlowDirection to LEFT_TO_RIGHT will result in
    // a failed HRESULT From DWRITE. Since the defaults work fine for horizontal, only
    // set these values for text orientation = vertical.
    auto textReadingDirection = this->TextReadingDirection;
    if (textReadingDirection != DirectWriteReadingDirection::LeftToRight)
    {
        textFormat->SetReadingDirection(args.readingDirection);
        textFormat->SetFlowDirection(args.flowDirection);
    }

    textFormat->SetTextAlignment(args.textAlignment);

    winrt::com_ptr<IDWriteTextLayout> textLayout;
    winrt::check_hresult(dwriteFactory->CreateTextLayout(
        args.text->Data(),
        args.text->Length(),
        textFormat.get(),
        args.availableWidth,
        args.availableHeight,
        textLayout.put()));

    DWRITE_TEXT_METRICS textMetrics = {};
    winrt::check_hresult(textLayout->GetMetrics(&textMetrics));

    auto sisWidth = static_cast<UINT32>(std::ceil(textMetrics.width));
    auto sisHeight = static_cast<UINT32>(std::ceil(textMetrics.height));
    auto imageSource = ref new SurfaceImageSource(sisWidth, sisHeight);
    auto sisUnknown = reinterpret_cast<IUnknown*>(imageSource);
    winrt::com_ptr<ISurfaceImageSourceNative> sisNative;
    sisUnknown->QueryInterface(__uuidof(ISurfaceImageSourceNative), sisNative.put_void());
    sisNative->SetDevice(resourceManager->GetDXGIDeviceNoRef());

    winrt::com_ptr<IDXGISurface> surface;
    RECT updateRect = { 0, 0, static_cast<LONG>(sisWidth), static_cast<LONG>(sisHeight) };
    POINT offset = { 0, 0 };
    m_lastDrawError = sisNative->BeginDraw(updateRect, surface.put(), &offset);
    if (SUCCEEDED(m_lastDrawError))
    {
        auto d2dContext = resourceManager->GetD2dDCNoRef();

        // set the translation to the section of the bitmap that we want to render.
        auto translate = D2D1::Matrix3x2F::Translation(static_cast<float>(offset.x), static_cast<float>(offset.y));
        d2dContext->SetTransform(translate);

        // this basically ensures the text background is transparent.
        D2D1_BITMAP_PROPERTIES1 bitmapProperties =
        {
            { DXGI_FORMAT_UNKNOWN, D2D1_ALPHA_MODE_PREMULTIPLIED },
            96.f * scale,
            96.f * scale,
            D2D1_BITMAP_OPTIONS_TARGET | D2D1_BITMAP_OPTIONS_CANNOT_DRAW
        };

        winrt::com_ptr<ID2D1Bitmap1> bitmap;
        winrt::check_hresult(d2dContext->CreateBitmapFromDxgiSurface(
            surface.get(),
            &bitmapProperties,
            bitmap.put()));

        d2dContext->SetTarget(bitmap.get());
        d2dContext->BeginDraw();
        d2dContext->Clear();

        winrt::com_ptr<ID2D1SolidColorBrush> brush;
        auto color = args.foregroundColor;
        D2D1_COLOR_F d2dColor = D2D1::ColorF(
            static_cast<float>(color.R) / 255.0f,
            static_cast<float>(color.G) / 255.0f,
            static_cast<float>(color.B) / 255.0f,
            static_cast<float>(color.A) / 255.0f);
        winrt::check_hresult(d2dContext->CreateSolidColorBrush(d2dColor, brush.put()));

        d2dContext->DrawText(
            args.text->Data(),
            args.text->Length(),
            textFormat.get(),
            D2D1::RectF(0, 0, static_cast<float>(sisWidth), static_cast<float>(sisHeight)),
            brush.get(),
            D2D1_DRAW_TEXT_OPTIONS_ENABLE_COLOR_FONT);

        HRESULT d2dEndDrawResult = d2dContext->EndDraw();
        sisNative->EndDraw();

        if (d2dEndDrawResult == D2DERR_RECREATE_TARGET)
        {
            winrt::check_hresult(resourceManager->RebuildDeviceResources());
        }

        m_image->Source = imageSource;
        m_drawRetries = 0;

        // XAML will rescale, so we divide by scale here.
        return Size{ static_cast<float>(sisWidth / scale), static_cast<float>(sisHeight / scale) };
    }
    else if (m_drawRetries == 0)
    {
        // D2D draw can fail for multiple reasons where rebuilding the D3D device might be necessary.
        winrt::check_hresult(resourceManager->RebuildDeviceResources());
        m_drawRetries++;
        return RenderText(args);
    }
    else
    {
        // if we fail to draw 2x a retry, just bail.
        return Size{};
    }
}

void DirectWriteTextBlock::OnDependencyPropertyChanged(_In_ DependencyObject^ d, _In_ DependencyPropertyChangedEventArgs^ /* e */)
{
    auto textBlockInstance = dynamic_cast<DirectWriteTextBlock^>(d);
    if (textBlockInstance)
    {
        textBlockInstance->InvalidateMeasure();
    }
}

void DirectWriteTextBlock::OnInheritedDependencyPropertyChanged(_In_ DependencyObject^ d, _In_ DependencyProperty^ /* e */)
{
    auto textBlockInstance = dynamic_cast<DirectWriteTextBlock^>(d);
    if (textBlockInstance)
    {
        textBlockInstance->InvalidateMeasure();
    }
}

void DirectWriteTextBlock::OnDpiChanged(_In_ DisplayInformation^ /* displayInfo */, _In_ Object^ /* obj */)
{
    InvalidateMeasure();
}

void DirectWriteTextBlock::OnHighContrastSettingsChanged(_In_ AccessibilitySettings^ accessibilitySettings, _In_ Object^ /* obj */)
{
    m_isHighContrast = accessibilitySettings->HighContrast;
    InvalidateMeasure();
}

END_NAMESPACE_CONTROLS_WINRT