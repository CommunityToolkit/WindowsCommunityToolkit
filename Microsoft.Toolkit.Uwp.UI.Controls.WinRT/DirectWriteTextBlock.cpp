// Copyright (c) Microsoft. All rights reserved. Licensed under the MIT license.
// See LICENSE in the project root for license information.

#include "pch.h"
#include <winrt\windows.ui.xaml.interop.h>
#include <winrt\Windows.UI.Xaml.Media.Imaging.h>
#include <windows.ui.xaml.media.dxinterop.h>
#include "DirectWriteTextBlock.h"

namespace winrt::Microsoft_Toolkit_Uwp_UI_Controls_WinRT::implementation
{
    using namespace Windows::UI::Xaml;
    using namespace Windows::UI::Xaml::Controls;
    using namespace Windows::UI::Xaml::Media;
    using namespace Windows::UI::Xaml::Media::Imaging;
    using namespace Windows::Graphics::Display;
    using namespace Windows::Foundation;

    // note: if you pass nullptr to winrt::to_hstring you will get a crash.
    DependencyProperty DirectWriteTextBlock::m_textProperty = DependencyProperty::Register(
        winrt::param::hstring(L"Text"),
        winrt::xaml_typename<winrt::hstring>(),
        winrt::xaml_typename<Microsoft_Toolkit_Uwp_UI_Controls_WinRT::DirectWriteTextBlock>(),
        PropertyMetadata{ winrt::box_value(winrt::to_hstring(L"")), PropertyChangedCallback{ &DirectWriteTextBlock::OnDependencyPropertyChanged } });

    DependencyProperty DirectWriteTextBlock::m_textLocaleProperty = DependencyProperty::Register(
        winrt::param::hstring(L"TextLocale"),
        winrt::xaml_typename<winrt::hstring>(),
        winrt::xaml_typename<Microsoft_Toolkit_Uwp_UI_Controls_WinRT::DirectWriteTextBlock>(),
        PropertyMetadata{ winrt::box_value(L"en-US"), PropertyChangedCallback{ &DirectWriteTextBlock::OnDependencyPropertyChanged } });

    DependencyProperty DirectWriteTextBlock::m_textOrientationProperty = DependencyProperty::Register(
        winrt::param::hstring(L"TextOrientation"),
        winrt::xaml_typename<Windows::UI::Xaml::Controls::Orientation>(),
        winrt::xaml_typename<Microsoft_Toolkit_Uwp_UI_Controls_WinRT::DirectWriteTextBlock>(),
        PropertyMetadata{ winrt::box_value(Windows::UI::Xaml::Controls::Orientation::Vertical), PropertyChangedCallback{ &DirectWriteTextBlock::OnDependencyPropertyChanged } });

    DependencyProperty DirectWriteTextBlock::m_textWrapProperty = DependencyProperty::Register(
        winrt::param::hstring(L"TextWrap"),
        winrt::xaml_typename<Windows::UI::Xaml::TextWrapping>(),
        winrt::xaml_typename<Microsoft_Toolkit_Uwp_UI_Controls_WinRT::DirectWriteTextBlock>(),
        PropertyMetadata{ winrt::box_value(Windows::UI::Xaml::TextWrapping::NoWrap), PropertyChangedCallback{ &DirectWriteTextBlock::OnDependencyPropertyChanged } });

    DirectWriteTextBlock::DirectWriteTextBlock()
    {
        // note: while this is what we should do, there's current a crash if you add a .xaml file to a C++/WinRT project. Once that bug is fixed, we should
        // keep a default style with this dll so consumers can know what to do.
        DefaultStyleKey(winrt::box_value(L"Microsoft.Toolkit.Uwp.UI.Controls.WinRT.DirectWriteTextBlock"));

        auto displayInfo = DisplayInformation::GetForCurrentView();
        m_dpiChangedToken = displayInfo.DpiChanged({ this, &DirectWriteTextBlock::OnDpiChanged });

#define REGISTER_INHERITED_PROPERTY_CALLBACK(token, inheritedProperty) \
    token = RegisterPropertyChangedCallback(inheritedProperty, DependencyPropertyChangedCallback{ &DirectWriteTextBlock::OnInheritedDependencyPropertyChanged })

        REGISTER_INHERITED_PROPERTY_CALLBACK(m_flowDirectionChangedToken, FrameworkElement::FlowDirectionProperty());
        REGISTER_INHERITED_PROPERTY_CALLBACK(m_foregroundChangedToken, Control::ForegroundProperty());
        REGISTER_INHERITED_PROPERTY_CALLBACK(m_fontSizeChangedToken, Control::FontSizeProperty());
        REGISTER_INHERITED_PROPERTY_CALLBACK(m_fontFamilyChangedToken, Control::FontFamilyProperty());
        REGISTER_INHERITED_PROPERTY_CALLBACK(m_fontStretchChangedToken, Control::FontStretchProperty());
        REGISTER_INHERITED_PROPERTY_CALLBACK(m_fontStyleChangedToken, Control::FontStyleProperty());
        REGISTER_INHERITED_PROPERTY_CALLBACK(m_fontWeightChangedToken, Control::FontWeightProperty());
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

        auto maybeImage = GetTemplateChild(L"Image").try_as<Image>();
        if (maybeImage == nullptr)
        {
            winrt::throw_hresult(E_NOT_VALID_STATE);
        }

        m_image = maybeImage;
    }

    Size DirectWriteTextBlock::MeasureOverride(Size const& availableSize)
    {
        auto displayInfo = DisplayInformation::GetForCurrentView();
        DirectWriteRenderArgBuilder builder;
        builder.SetAvailableHeight(availableSize.Height);
        builder.SetAvailableWidth(availableSize.Width);
        builder.SetDPI(displayInfo.RawPixelsPerViewPixel());
        builder.SetFlowDirection(FlowDirection());
        builder.SetFontFamily(FontFamily());
        builder.SetFontSize(FontSize());
        builder.SetFontStretch(FontStretch());
        builder.SetFontStyle(FontStyle());
        builder.SetFontWeight(FontWeight());
        builder.SetForegroundBrush(Foreground());
        builder.SetText(Text());
        builder.SetTextLocale(TextLocale());
        builder.SetTextOrientation(TextOrientation());
        builder.SetTextWrapping(TextWrap());

        auto args = builder.BuildRenderArgs();
        return RenderText(args);
    }

    void DirectWriteTextBlock::Close()
    {

#define UNREGISTER_INHERITED_PROPERTY_CALLBACK(token, inheritedProperty) \
    if (token != 0) \
    { \
        UnregisterPropertyChangedCallback(inheritedProperty, token); \
        token = 0; \
    }

        UNREGISTER_INHERITED_PROPERTY_CALLBACK(m_flowDirectionChangedToken, FrameworkElement::FlowDirectionProperty());
        UNREGISTER_INHERITED_PROPERTY_CALLBACK(m_foregroundChangedToken, Control::ForegroundProperty());
        UNREGISTER_INHERITED_PROPERTY_CALLBACK(m_fontSizeChangedToken, Control::FontSizeProperty());
        UNREGISTER_INHERITED_PROPERTY_CALLBACK(m_fontFamilyChangedToken, Control::FontFamilyProperty());
        UNREGISTER_INHERITED_PROPERTY_CALLBACK(m_fontStretchChangedToken, Control::FontStretchProperty());
        UNREGISTER_INHERITED_PROPERTY_CALLBACK(m_fontStyleChangedToken, Control::FontStyleProperty());
        UNREGISTER_INHERITED_PROPERTY_CALLBACK(m_fontWeightChangedToken, Control::FontWeightProperty());

        if (m_dpiChangedToken.value != 0)
        {
            auto displayInfo = DisplayInformation::GetForCurrentView();
            displayInfo.DpiChanged(m_dpiChangedToken);
            m_dpiChangedToken = {};
        }

#undef UNREGISTER_INHERITED_PROPERTY_CALLBACK
    }

    Size DirectWriteTextBlock::RenderText(DirectWriteTextRenderArgs const& args)
    {
        if (args.text.empty())
        {
            return Size{ 0, 0 };
        }

        auto resourceManager = DirectWriteResourceManager::GetInstance();
        auto scale = args.rawPixelsPerViewPixel;
        auto scaledFontSize = args.fontSize * scale;
        auto dwriteFactory = resourceManager->GetDirectWriteFactoryNoRef();

        winrt::com_ptr<IDWriteTextFormat> textFormat;
        winrt::check_hresult(dwriteFactory->CreateTextFormat(
            args.fontFamily.data(),
            args.fontCollection.get(),
            args.fontWeight,
            args.fontStyle,
            args.fontStretch,
            scaledFontSize,
            args.textLocale.data(),
            textFormat.put()));

        textFormat->SetWordWrapping(args.textWrapping);

        // Trying to set readingDirection + FlowDirection to LEFT_TO_RIGHT will result in
        // a failed HRESULT From DWRITE. Since the defaults work fine for horizontal, only
        // set these values for text orientation = vertical.
        if (this->TextOrientation() == Orientation::Vertical)
        {
            textFormat->SetReadingDirection(args.readingDirection);
            textFormat->SetFlowDirection(args.flowDirection);
        }

        winrt::com_ptr<IDWriteTextLayout> textLayout;
        winrt::check_hresult(dwriteFactory->CreateTextLayout(
            args.text.data(),
            args.text.size(),
            textFormat.get(),
            args.availableWidth,
            args.availableHeight,
            textLayout.put()));

        DWRITE_TEXT_METRICS textMetrics = {};
        winrt::check_hresult(textLayout->GetMetrics(&textMetrics));

        auto sisWidth = static_cast<UINT32>(std::ceil(textMetrics.width));
        auto sisHeight = static_cast<UINT32>(std::ceil(textMetrics.height));
        auto imageSource = SurfaceImageSource(sisWidth, sisHeight);
        auto sisNative{ imageSource.as<ISurfaceImageSourceNative>() };
        sisNative->SetDevice(resourceManager->GetDXGIDeviceNoRef());

        winrt::com_ptr<IDXGISurface> surface;
        RECT updateRect = { 0, 0, static_cast<LONG>(sisWidth), static_cast<LONG>(sisHeight) };
        POINT offset = { 0, 0 };
        if (SUCCEEDED(sisNative->BeginDraw(updateRect, surface.put(), &offset)))
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
                args.text.data(),
                args.text.size(),
                textFormat.get(),
                D2D1::RectF(0, 0, static_cast<float>(sisWidth), static_cast<float>(sisHeight)),
                brush.get(),
                D2D1_DRAW_TEXT_OPTIONS_ENABLE_COLOR_FONT);

            d2dContext->EndDraw();
            sisNative->EndDraw();
        }

        m_image.Source(imageSource);
        // XAML will rescale, so we divide by scale here.
        return Size{ static_cast<float>(sisWidth / scale), static_cast<float>(sisHeight / scale) };
    }

    void DirectWriteTextBlock::OnDependencyPropertyChanged(DependencyObject const& d, DependencyPropertyChangedEventArgs const& /* e */)
    {
        auto textBlockInstance{ d.try_as<DirectWriteTextBlock>() };
        if (textBlockInstance)
        {
            textBlockInstance->InvalidateMeasure();
        }
    }

    void DirectWriteTextBlock::OnInheritedDependencyPropertyChanged(DependencyObject const& d, DependencyProperty const& /* e */)
    {
        auto textBlockInstance{ d.try_as<DirectWriteTextBlock>() };
        if (textBlockInstance)
        {
            textBlockInstance->InvalidateMeasure();
        }
    }

    void DirectWriteTextBlock::OnDpiChanged(DisplayInformation const& /* displayInfo */, IInspectable const& /* obj */)
    {
        InvalidateMeasure();
    }
}
