// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

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
                return (type)(GetValue(membername)); \
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

#define DEFINE_XAML_DEPENDENCY_PROPERTY_VALUE_TYPE(type, name, membername) \
    public: \
        property type name \
        { \
            type get() \
            { \
                auto boxedType = dynamic_cast<Platform::IBox<type>^>(GetValue(membername)); \
                if (boxedType != nullptr) \
                { \
                    return boxedType->Value; \
                } \
                else \
                { \
                    return static_cast<type>((int)(GetValue(membername))); \
                } \
            } \
            void set(type value) \
            { \
                SetValue(membername, ref new Platform::Box<type>(value)); \
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

    /// <summary>
    /// The text to render
    /// </summary>
    DEFINE_XAML_DEPENDENCY_PROPERTY(Platform::String^, Text, m_textProperty);

    /// <summary>
    /// The locale of the text to show
    /// </summary>
    DEFINE_XAML_DEPENDENCY_PROPERTY(Platform::String^, TextLocale, m_textLocaleProperty);

    /// <summary>
    /// The reading direction of the text.
    /// </summary>
    DEFINE_XAML_DEPENDENCY_PROPERTY_VALUE_TYPE(DirectWriteReadingDirection, TextReadingDirection, m_textReadingDirectionProperty);

    /// <summary>
    /// How the text is wrapped. To Wrap text, just set the Height or Width of this control.
    /// </summary>
    DEFINE_XAML_DEPENDENCY_PROPERTY_VALUE_TYPE(DirectWriteWordWrapping, TextWrap, m_textWrapProperty);

    /// <summary>
    /// The direct write based alignment of the text. This is agnostic of XAML and only supports DirectWrite values.
    /// </summary>
    DEFINE_XAML_DEPENDENCY_PROPERTY_VALUE_TYPE(DirectWriteTextAlignment, TextAlign, m_textAlignmentProperty);

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
    Windows::Graphics::Display::DisplayInformation^ m_displayInfo;
    bool m_isHighContrast = false;
    HRESULT m_lastDrawError = S_OK;
    unsigned int m_drawRetries = 0;
};

END_NAMESPACE_CONTROLS_WINRT
