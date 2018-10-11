#include "pch.h"
#include "DirectWriteRenderArgBuilder.h"
#include "DirectWriteResourceManager.h"
#include "UniversalWindowsAppPackageFontLoader\FontCollectionLoader.h"

BEGIN_NAMESPACE_CONTROLS_WINRT

using namespace Platform;
using namespace Windows::UI;
using namespace Windows::UI::Text;
using namespace Windows::UI::Xaml;
using namespace Windows::UI::Xaml::Controls;
using namespace Windows::UI::Xaml::Media;
using namespace Windows::Globalization;
using namespace Windows::Graphics::Display;
using namespace Windows::Foundation;

DirectWriteRenderArgBuilder::DirectWriteRenderArgBuilder()
{
    // default values
    m_builtArgs.availableHeight = 0;
    m_builtArgs.availableWidth = 0;
    m_builtArgs.flowDirection = DWRITE_FLOW_DIRECTION::DWRITE_FLOW_DIRECTION_LEFT_TO_RIGHT;
    m_builtArgs.fontFamily = ref new String(L"Segoe UI");
    m_builtArgs.fontSize = 15;
    m_builtArgs.fontStretch = DWRITE_FONT_STRETCH::DWRITE_FONT_STRETCH_NORMAL;
    m_builtArgs.fontStyle = DWRITE_FONT_STYLE::DWRITE_FONT_STYLE_NORMAL;
    m_builtArgs.fontWeight = DWRITE_FONT_WEIGHT::DWRITE_FONT_WEIGHT_NORMAL;
    m_builtArgs.foregroundColor = Windows::UI::Colors::Black;
    m_builtArgs.rawPixelsPerViewPixel = 1;
    m_builtArgs.readingDirection = DWRITE_READING_DIRECTION::DWRITE_READING_DIRECTION_LEFT_TO_RIGHT;
    m_builtArgs.textLocale = ref new String(L"en-US");
    m_builtArgs.textWrapping = DWRITE_WORD_WRAPPING::DWRITE_WORD_WRAPPING_NO_WRAP;
    m_builtArgs.fontCollection = nullptr;
}

void DirectWriteRenderArgBuilder::SetFontFamily(FontFamily^ fontFamily)
{
    String^ resultFontFamily;

    // try to get the FontFamily property first.
    auto controlFontFamily = fontFamily;
    if ((controlFontFamily != nullptr) && !controlFontFamily->Source->IsEmpty())
    {
        // if there's something in the font family, use it.
        resultFontFamily = controlFontFamily->Source;
    }

    // if nothing was in the font family, try the XAML default value.
    if (resultFontFamily->IsEmpty() && (controlFontFamily != nullptr))
    {
        auto xamlDefault = FontFamily::XamlAutoFontFamily;
        if ((xamlDefault != nullptr) && !xamlDefault->Source->IsEmpty())
        {
            resultFontFamily = xamlDefault->Source;
        }
    }

    // if the xaml default failed for some reason, hardcode to Segoe UI as last fallback.
    if (resultFontFamily->IsEmpty())
    {
        resultFontFamily = L"Segoe UI";
    }

    m_builtArgs.fontFamily = resultFontFamily;
    BuildFontCollection(resultFontFamily);
}

void DirectWriteRenderArgBuilder::SetText(Platform::String^ text)
{
    m_builtArgs.text = text;
}

void DirectWriteRenderArgBuilder::SetTextLocale(Platform::String^ textLocale)
{
    if (Language::IsWellFormed(textLocale))
    {
        m_builtArgs.textLocale = textLocale;
    }
    else
    {
        // default to en-US.
        m_builtArgs.textLocale = ref new String(L"en-US");
    }
}

void DirectWriteRenderArgBuilder::SetForegroundBrush(Brush^ brush)
{
    auto solidColorBrush = dynamic_cast<SolidColorBrush^>(brush);
    if (solidColorBrush != nullptr)
    {
        m_builtArgs.foregroundColor = solidColorBrush->Color;
    }
    else
    {
        m_builtArgs.foregroundColor = Colors::Black;
    }
}

void DirectWriteRenderArgBuilder::SetFontStyle(FontStyle fontStyle)
{
    switch (fontStyle)
    {
    case FontStyle::Normal: __fallthrough;
    default:
        m_builtArgs.fontStyle = DWRITE_FONT_STYLE::DWRITE_FONT_STYLE_NORMAL;
        break;
    case FontStyle::Italic:
        m_builtArgs.fontStyle = DWRITE_FONT_STYLE::DWRITE_FONT_STYLE_ITALIC;
        break;
    case FontStyle::Oblique:
        m_builtArgs.fontStyle = DWRITE_FONT_STYLE::DWRITE_FONT_STYLE_OBLIQUE;
        break;
    }
}

void DirectWriteRenderArgBuilder::SetFontStretch(FontStretch fontStretch)
{
    switch (fontStretch)
    {
    case FontStretch::Normal: __fallthrough;
    default:
        m_builtArgs.fontStretch = DWRITE_FONT_STRETCH::DWRITE_FONT_STRETCH_NORMAL;
        break;
    case FontStretch::Condensed:
        m_builtArgs.fontStretch = DWRITE_FONT_STRETCH::DWRITE_FONT_STRETCH_CONDENSED;
        break;
    case FontStretch::Expanded:
        m_builtArgs.fontStretch = DWRITE_FONT_STRETCH::DWRITE_FONT_STRETCH_EXPANDED;
        break;
    case FontStretch::ExtraCondensed:
        m_builtArgs.fontStretch = DWRITE_FONT_STRETCH::DWRITE_FONT_STRETCH_EXTRA_CONDENSED;
        break;
    case FontStretch::ExtraExpanded:
        m_builtArgs.fontStretch = DWRITE_FONT_STRETCH::DWRITE_FONT_STRETCH_EXTRA_EXPANDED;
        break;
    case FontStretch::SemiCondensed:
        m_builtArgs.fontStretch = DWRITE_FONT_STRETCH::DWRITE_FONT_STRETCH_SEMI_CONDENSED;
        break;
    case FontStretch::SemiExpanded:
        m_builtArgs.fontStretch = DWRITE_FONT_STRETCH::DWRITE_FONT_STRETCH_SEMI_EXPANDED;
        break;
    case FontStretch::UltraCondensed:
        m_builtArgs.fontStretch = DWRITE_FONT_STRETCH::DWRITE_FONT_STRETCH_ULTRA_CONDENSED;
        break;
    case FontStretch::UltraExpanded:
        m_builtArgs.fontStretch = DWRITE_FONT_STRETCH::DWRITE_FONT_STRETCH_ULTRA_EXPANDED;
        break;
    }
}

void DirectWriteRenderArgBuilder::SetTextOrientation(Orientation textOrientation)
{
    switch (textOrientation)
    {
    case Orientation::Vertical: __fallthrough;
    default:
        m_builtArgs.readingDirection = DWRITE_READING_DIRECTION::DWRITE_READING_DIRECTION_TOP_TO_BOTTOM;
        break;
    case Orientation::Horizontal:
        m_builtArgs.readingDirection = DWRITE_READING_DIRECTION::DWRITE_READING_DIRECTION_LEFT_TO_RIGHT;
        break;
    }
}

void DirectWriteRenderArgBuilder::SetFlowDirection(FlowDirection flowDirection)
{
    switch (flowDirection)
    {
    case FlowDirection::LeftToRight: __fallthrough;
    default:
        m_builtArgs.flowDirection = DWRITE_FLOW_DIRECTION::DWRITE_FLOW_DIRECTION_LEFT_TO_RIGHT;
        break;
    case FlowDirection::RightToLeft:
        m_builtArgs.flowDirection = DWRITE_FLOW_DIRECTION::DWRITE_FLOW_DIRECTION_RIGHT_TO_LEFT;
        break;
    }
}

void DirectWriteRenderArgBuilder::SetFontWeight(FontWeight fontWeight)
{
    auto weight = fontWeight.Weight;
    if (weight == FontWeights::Black.Weight)
    {
        m_builtArgs.fontWeight = DWRITE_FONT_WEIGHT::DWRITE_FONT_WEIGHT_BLACK;
    }
    else if (weight == FontWeights::Bold.Weight)
    {
        m_builtArgs.fontWeight = DWRITE_FONT_WEIGHT::DWRITE_FONT_WEIGHT_BOLD;
    }
    else if (weight == FontWeights::ExtraBlack.Weight)
    {
        m_builtArgs.fontWeight = DWRITE_FONT_WEIGHT::DWRITE_FONT_WEIGHT_EXTRA_BLACK;
    }
    else if (weight == FontWeights::ExtraBold.Weight)
    {
        m_builtArgs.fontWeight = DWRITE_FONT_WEIGHT::DWRITE_FONT_WEIGHT_EXTRA_BOLD;
    }
    else if (weight == FontWeights::ExtraLight.Weight)
    {
        m_builtArgs.fontWeight = DWRITE_FONT_WEIGHT::DWRITE_FONT_WEIGHT_EXTRA_LIGHT;
    }
    else if (weight == FontWeights::Light.Weight)
    {
        m_builtArgs.fontWeight = DWRITE_FONT_WEIGHT::DWRITE_FONT_WEIGHT_LIGHT;
    }
    else if (weight == FontWeights::SemiBold.Weight)
    {
        m_builtArgs.fontWeight = DWRITE_FONT_WEIGHT::DWRITE_FONT_WEIGHT_SEMI_BOLD;
    }
    else if (weight == FontWeights::SemiLight.Weight)
    {
        m_builtArgs.fontWeight = DWRITE_FONT_WEIGHT::DWRITE_FONT_WEIGHT_SEMI_LIGHT;
    }
    else
    {
        m_builtArgs.fontWeight = DWRITE_FONT_WEIGHT::DWRITE_FONT_WEIGHT_NORMAL;
    }
}

void DirectWriteRenderArgBuilder::SetTextWrapping(TextWrapping textWrapping)
{
    switch (textWrapping)
    {
    case TextWrapping::NoWrap:
    default:
        m_builtArgs.textWrapping = DWRITE_WORD_WRAPPING::DWRITE_WORD_WRAPPING_NO_WRAP;
        break;
    case TextWrapping::Wrap:
        m_builtArgs.textWrapping = DWRITE_WORD_WRAPPING::DWRITE_WORD_WRAPPING_WRAP;
        break;
    case TextWrapping::WrapWholeWords:
        m_builtArgs.textWrapping = DWRITE_WORD_WRAPPING::DWRITE_WORD_WRAPPING_WHOLE_WORD;
        break;
    }
}

void DirectWriteRenderArgBuilder::SetFontSize(double fontSize)
{
    m_builtArgs.fontSize = static_cast<float>(fontSize);
    if (m_builtArgs.fontSize <= 0)
    {
        m_builtArgs.fontSize = 15.0f;
    }
}

void DirectWriteRenderArgBuilder::SetDPI(double rawPixelsPerViewPixel)
{
    m_builtArgs.rawPixelsPerViewPixel = static_cast<float>(rawPixelsPerViewPixel);
    if (m_builtArgs.rawPixelsPerViewPixel <= 0)
    {
        m_builtArgs.rawPixelsPerViewPixel = 1.0f;
    }
}

void DirectWriteRenderArgBuilder::SetAvailableWidth(float availableWidth)
{
    m_builtArgs.availableWidth = availableWidth;
}

void DirectWriteRenderArgBuilder::SetAvailableHeight(float availableHeight)
{
    m_builtArgs.availableHeight = availableHeight;
}

DirectWriteTextRenderArgs& DirectWriteRenderArgBuilder::BuildRenderArgs()
{
    return m_builtArgs;
}

void DirectWriteRenderArgBuilder::BuildFontCollection(Platform::String^ fontFamily)
{
    // default arg is system font collection, what we're looking for is if we're trying to
    // find a local custom ttf file.
    if (UniversalWindowsAppPackageFontLoader::FontCollectionLoader::HasCustomFontFamily(fontFamily))
    {
        auto resourceManager = DirectWriteResourceManager::GetInstance();
        auto dwriteFactory = resourceManager->GetDirectWriteFactoryNoRef();
        UniversalWindowsAppPackageFontLoader::UniversalPackageFontData parseData = {};
        UniversalWindowsAppPackageFontLoader::FontCollectionLoader::ParseXamlFontFamily(fontFamily, parseData);
        auto customLoader = UniversalWindowsAppPackageFontLoader::FontCollectionLoader::GetInstance();

        // the key is a void* meaning the size is actual size.
        auto fontFamilyString = fontFamily->Data();
        auto fontFamilySize = fontFamily->Length() * sizeof(wchar_t);
        dwriteFactory->CreateCustomFontCollection(customLoader.get(), fontFamilyString, static_cast<unsigned int>(fontFamilySize), m_builtArgs.fontCollection.put());

        // set font family to the parsed font family.
        m_builtArgs.fontFamily = ref new String(parseData.customFontName.data(), static_cast<unsigned int>(parseData.customFontName.size()));
    }
    else
    {
        // else assume it's a system font for now. We could do a lookup, but that would actually slow things down.
        m_builtArgs.fontCollection = nullptr;
    }
}

END_NAMESPACE_CONTROLS_WINRT