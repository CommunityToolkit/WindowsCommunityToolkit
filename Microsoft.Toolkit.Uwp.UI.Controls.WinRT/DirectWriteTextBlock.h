// Copyright (c) Microsoft. All rights reserved. Licensed under the MIT license.
// See LICENSE in the project root for license information.

#pragma once
#include <winrt\windows.graphics.display.h>
#include "DirectWriteTextBlock.g.h"
#include "DirectWriteResourceManager.h"
#include "DirectWriteRenderArgBuilder.h"

namespace winrt::Microsoft_Toolkit_Uwp_UI_Controls_WinRT::implementation
{
#define DEFINE_XAML_DEPENDENCY_PROPERTY(type, name, membername) \
    public: \
        type name() \
        { \
            return winrt::unbox_value<type>(GetValue(membername)); \
        } \
        void name(type const& value) \
        { \
            SetValue(membername, winrt::box_value(value)); \
        } \
        static Windows::UI::Xaml::DependencyProperty name ## Property() \
        { \
            return membername; \
        } \
    private: \
        static Windows::UI::Xaml::DependencyProperty membername; \
    public:

    /// <summary>
    /// This is a text block which uses DirectWrite to draw text onto a bitmap image, allowing the user to orient text vertically for
    /// East Asian languages and to support text rendering modes which aren't supported by XAML yet, but are supported by DirectWrite.
    ///
    /// This is built using C++/WinRT and will require the 1803 Windows SDK to build and use at minumum.
    /// </summary>
    /// <remarks>
    /// The current XAML code gen for C++/WinRT is in preview and results in a compile error. Users will need to define their own
    /// default style for this thing in their apps that use this. This currently only supports SolidColorBrush based Foregrounds
    ///
    /// Throwing out of this class will cause significant debuggability issues in C# consumers as they just get a "native exception was thrown."
    /// Therefore, this class will attempt to recover from poor input like setting font size = 0. If something happens like a DWrite
    /// method fails, that will throw an exception.
    /// </remarks>
    struct DirectWriteTextBlock : DirectWriteTextBlockT<DirectWriteTextBlock>
    {
    public:
        DirectWriteTextBlock();
        virtual ~DirectWriteTextBlock();

        // IClosable is here so that we can unsub from the inherited dependency properties.
        virtual void Close();

        // Note: DWrite actually supports a lot more rendering modes than XAML, but we're able to get most of what we want
        // with just the XAML built in types. In the event we need more of the DWrite API surface, we would need
        // to define our own enums which map to the DWrite enums.

        /// <summary>
        /// The text to render
        /// </summary>
        DEFINE_XAML_DEPENDENCY_PROPERTY(winrt::hstring, Text, m_textProperty);

        /// <summary>
        /// The locale of the text to show
        /// </summary>
        DEFINE_XAML_DEPENDENCY_PROPERTY(winrt::hstring, TextLocale, m_textLocaleProperty);

        /// <summary>
        /// The orientation of the text.
        /// </summary>
        DEFINE_XAML_DEPENDENCY_PROPERTY(Windows::UI::Xaml::Controls::Orientation, TextOrientation, m_textOrientationProperty);

        /// <summary>
        /// How the text is wrapped. To Wrap text, just set the Height or Width of this control.
        /// </summary>
        DEFINE_XAML_DEPENDENCY_PROPERTY(Windows::UI::Xaml::TextWrapping, TextWrap, m_textWrapProperty);

    public:
        // These are the methods we're overriding from IFrameworkElementOverrides.
        void OnApplyTemplate();
        Windows::Foundation::Size MeasureOverride(Windows::Foundation::Size const& availableSize);

    private:
        static void OnDependencyPropertyChanged(Windows::UI::Xaml::DependencyObject const& d, Windows::UI::Xaml::DependencyPropertyChangedEventArgs const& e);
        static void OnInheritedDependencyPropertyChanged(Windows::UI::Xaml::DependencyObject const& d, Windows::UI::Xaml::DependencyProperty const& e);
        void OnDpiChanged(Windows::Graphics::Display::DisplayInformation const& displayInfo, Windows::Foundation::IInspectable const& obj);
        Windows::Foundation::Size RenderText(DirectWriteTextRenderArgs const& args);

        winrt::event_token m_dpiChangedToken;
        long long m_foregroundChangedToken = 0;
        long long m_fontSizeChangedToken = 0;
        long long m_fontFamilyChangedToken = 0;
        long long m_flowDirectionChangedToken = 0;
        long long m_fontStretchChangedToken = 0;
        long long m_fontWeightChangedToken = 0;
        long long m_fontStyleChangedToken = 0;

        Windows::UI::Xaml::Controls::Image m_image;
    };
}

namespace  winrt::Microsoft_Toolkit_Uwp_UI_Controls_WinRT::factory_implementation
{
    struct DirectWriteTextBlock : DirectWriteTextBlockT<DirectWriteTextBlock, implementation::DirectWriteTextBlock>
    {
    };
}
