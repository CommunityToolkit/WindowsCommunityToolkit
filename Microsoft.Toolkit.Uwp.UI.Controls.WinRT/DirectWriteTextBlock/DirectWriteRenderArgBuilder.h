#pragma once

BEGIN_NAMESPACE_CONTROLS_WINRT

/// <summary>
/// These are arguments which are used to render the DWrite string.
/// <summary>
struct DirectWriteTextRenderArgs
{
    Platform::String^ fontFamily;
    Platform::String^ text;
    Platform::String^ textLocale;
    Windows::UI::Color foregroundColor;
    DWRITE_FONT_STYLE fontStyle;
    DWRITE_FONT_STRETCH fontStretch;
    DWRITE_READING_DIRECTION readingDirection;
    DWRITE_FLOW_DIRECTION flowDirection;
    DWRITE_FONT_WEIGHT fontWeight;
    DWRITE_WORD_WRAPPING textWrapping;
    float fontSize;
    float rawPixelsPerViewPixel;
    float availableWidth;
    float availableHeight;
    winrt::com_ptr<IDWriteFontCollection> fontCollection;
};

/// <summary>
/// This class translates input from the XAML world into a DirectWriteTextRenderArgs struct
/// meant to be used with the DWrite world. If the user passes invalid values in, it attempts
/// to correct by setting them back to a known default value. This is because in case the user
/// is using the text block from C# (most users are expected to do this), a crash ultimately
/// just gives them a "something went wrong and a native exception was thrown" which is not
/// super helpful. Recovery will at least render something that looks incorrect on the screen.
/// <summary>
class DirectWriteRenderArgBuilder
{
public:
    DirectWriteRenderArgBuilder();

    /// <summary>
    /// The font family, by default Segoe UI
    /// <summary>
    void SetFontFamily(Windows::UI::Xaml::Media::FontFamily^ fontFamily);

    /// <summary>
    /// The text to render, empty string by default.
    /// <summary>
    void SetText(Platform::String^ text);

    /// <summary>
    /// The text locale for DWrite, en-US by default.
    /// <summary>
    void SetTextLocale(Platform::String^ textLocale);

    /// <summary>
    /// The foreground brush, Black by default
    /// <summary>
    void SetForegroundBrush(Windows::UI::Xaml::Media::Brush^ brush);

    /// <summary>
    /// The font style, Normal by default
    /// <summary>
    void SetFontStyle(Windows::UI::Text::FontStyle fontStyle);

    /// <summary>
    /// The font stretch, normal by default
    /// <summary>
    void SetFontStretch(Windows::UI::Text::FontStretch fontStretch);

    /// <summary>
    /// The text orientation, vertical by default since if you're going to use horizontal, you should use a XAML TextBlock
    /// <summary>
    void SetTextOrientation(Windows::UI::Xaml::Controls::Orientation textOrientation);

    /// <summary>
    /// The flow direction, LeftToRight by default
    /// <summary>
    void SetFlowDirection(Windows::UI::Xaml::FlowDirection flowDirection);

    /// <summary>
    /// The font weight, Normal by default
    /// <summary>
    void SetFontWeight(Windows::UI::Text::FontWeight fontWeight);

    /// <summary>
    /// The font size, 15 by default.
    /// <summary>
    void SetFontSize(double fontSize);

    /// <summary>
    /// The DPI to render at, 1 by default
    /// <summary>
    void SetDPI(double rawPixelsPerViewPixel);

    /// <summary>
    /// The available width to render at
    /// <summary>
    void SetAvailableWidth(float availableWidth);

    /// <summary>
    /// The available height to render at.
    /// <summary>
    void SetAvailableHeight(float availableHeight);

    /// <summary>
    /// The text wrap mode, None by default.
    /// <summary>
    void SetTextWrapping(Windows::UI::Xaml::TextWrapping textWrapping);

    /// <summary>
    /// The render arguments.
    /// <summary>
    DirectWriteTextRenderArgs& BuildRenderArgs();

private:
    void BuildFontCollection(Platform::String^ fontFamily);
    DirectWriteTextRenderArgs m_builtArgs = {};
};

END_NAMESPACE_CONTROLS_WINRT