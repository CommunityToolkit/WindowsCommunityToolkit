// Copyright (c) Microsoft. All rights reserved. Licensed under the MIT license.
// See LICENSE in the project root for license information.

#pragma once

#include "DirectWriteResourceManager.h"
#include "DirectWriteRenderArgBuilder.h"

BEGIN_NAMESPACE_CONTROLS_WINRT

#define DEFINE_XAML_DEPENDENCY_PROPERTY(type, name, membername) \
    public: \
        property type name \
        { \
            type get() \
            { \
                return (type)GetValue(membername); \
            } \
            void set(type value) \
            { \
                SetValue(membername, value); \
            } \
        } \
        static property Windows::UI::Xaml::DependencyProperty^ name ## Property \
        { \
            Windows::UI::Xaml::DependencyProperty^ get() \
            { \
                return membername; \
            } \
        } \
    private: \
        static Windows::UI::Xaml::DependencyProperty^ membername; \
    public:

/// <summary>
/// This is a text block which uses DirectWrite to draw text onto a bitmap image, allowing the user to orient text vertically for
/// East Asian languages and to support text rendering modes which aren't supported by XAML yet, but are supported by DirectWrite.
///
/// Parts of this are built using C++/WinRT and will require the 1803 Windows SDK to build and use at minumum.
/// </summary>
/// <remarks>
/// The current XAML code gen for C++/WinRT is in preview and results in a compile error so the main project class is C++/CX to get around
/// that limitation.
///
/// Throwing out of this class will cause significant debuggability issues in C# consumers as they just get a "native exception was thrown."
/// Therefore, this class will attempt to recover from poor input like setting font size = 0. If something happens like a DWrite
/// method fails, that will throw an exception.
/// </remarks>
[Windows::Foundation::Metadata::WebHostHidden]
public ref class DirectWriteTextBlock sealed : public Windows::UI::Xaml::Controls::Control
{
public:
    DirectWriteTextBlock();
    virtual ~DirectWriteTextBlock();

    // Note: DWrite actually supports a lot more rendering modes than XAML, but we're able to get most of what we want
    // with just the XAML built in types. In the event we need more of the DWrite API surface, we would need
    // to define our own enums which map to the DWrite enums.

    /// <summary>
    /// The text to render
    /// </summary>
    DEFINE_XAML_DEPENDENCY_PROPERTY(Platform::String^, Text, m_textProperty);

    /// <summary>
    /// The locale of the text to show
    /// </summary>
    DEFINE_XAML_DEPENDENCY_PROPERTY(Platform::String^, TextLocale, m_textLocaleProperty);

    /// <summary>
    /// The orientation of the text.
    /// </summary>
    DEFINE_XAML_DEPENDENCY_PROPERTY(Windows::UI::Xaml::Controls::Orientation, TextOrientation, m_textOrientationProperty);

    /// <summary>
    /// How the text is wrapped. To Wrap text, just set the Height or Width of this control.
    /// </summary>
    DEFINE_XAML_DEPENDENCY_PROPERTY(Windows::UI::Xaml::TextWrapping, TextWrap, m_textWrapProperty);

protected:
    // These are the methods we're overriding from IFrameworkElementOverrides.
    void OnApplyTemplate() override;
    Windows::Foundation::Size MeasureOverride(Windows::Foundation::Size availableSize) override;

private:
    static void OnDependencyPropertyChanged(_In_ Windows::UI::Xaml::DependencyObject^ d, _In_ Windows::UI::Xaml::DependencyPropertyChangedEventArgs^ e);
    static void OnInheritedDependencyPropertyChanged(_In_ Windows::UI::Xaml::DependencyObject^ d, _In_ Windows::UI::Xaml::DependencyProperty^ e);
    void OnDpiChanged(_In_ Windows::Graphics::Display::DisplayInformation^ displayInfo, _In_ Platform::Object^ obj);
    void OnHighContrastSettingsChanged(_In_ Windows::UI::ViewManagement::AccessibilitySettings^ accessibilitySettings, _In_ Platform::Object^ obj);
    Windows::Foundation::Size RenderText(DirectWriteTextRenderArgs const& args);
    void Close();
    void UpdateTextBrushesForHighContrast();
    void UpdateElementsForHighContrast();

    Windows::Foundation::EventRegistrationToken m_dpiChangedToken = {};
    Windows::Foundation::EventRegistrationToken m_highContrastChangedToken = {};

    long long m_foregroundChangedToken = 0;
    long long m_fontSizeChangedToken = 0;
    long long m_fontFamilyChangedToken = 0;
    long long m_flowDirectionChangedToken = 0;
    long long m_fontStretchChangedToken = 0;
    long long m_fontWeightChangedToken = 0;
    long long m_fontStyleChangedToken = 0;

    Windows::UI::ViewManagement::AccessibilitySettings^ m_accessibilitySettings;
    Windows::UI::Xaml::Controls::Image^ m_image;
    Windows::UI::Xaml::Controls::Border^ m_textBackground;
    Windows::UI::Xaml::Media::Brush^ m_textForegroundBrush;
    Windows::UI::Xaml::Media::Brush^ m_textBackgroundBrush;
    bool m_isHighContrast;
};

END_NAMESPACE_CONTROLS_WINRT
