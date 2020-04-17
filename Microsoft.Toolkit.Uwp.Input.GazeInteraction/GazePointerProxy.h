//Copyright (c) Microsoft. All rights reserved. Licensed under the MIT license.
//See LICENSE in the project root for license information.

#pragma once

#include "GazePointerProxy.g.h"

#include "Interaction.h"
#include <winrt/Microsoft.UI.Xaml.Media.h>
#include <winrt/Microsoft.UI.Xaml.Interop.h>

using namespace winrt::Microsoft::UI::Xaml::Media;

namespace winrt::Microsoft::Toolkit::Uwp::Input::GazeInteraction::implementation
{
    /// <summary>
    /// Helper class that helps track which UIElements in the visual tree are enabled.
    ///
    /// The GazePointer is enabled when one or more UIElements in the visual tree have
    /// their GazeInput.InteractionProperty value set to Enabled. Notice that there are
    /// two conditions for enablement: that attached property is Enabled; that the UIElement
    /// is in the visual tree.
    /// </summary>
    struct GazePointerProxy : GazePointerProxyT<GazePointerProxy>
    {
    public:

        GazePointerProxy() = default;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="element">The element proxy is attaching to.</param>
        GazePointerProxy(Microsoft::UI::Xaml::FrameworkElement const& element);

        /// <summary>
        /// A private attached property for associating an instance of this class with the UIElement
        /// to which it refers.
        /// </summary>
        static Microsoft::UI::Xaml::DependencyProperty GazePointerProxyProperty();

        /// <summary>
        /// Method called when the GazeInput.Interaction attached property is set to a new value.
        /// </summary>
        /// <param name="element">The element being set. May be null to indicate whole user interface.</param>
        /// <param name="value">The interaction enablement value being set.</param>
        static void SetInteraction(Microsoft::UI::Xaml::FrameworkElement const& element, Microsoft::Toolkit::Uwp::Input::GazeInteraction::Interaction const& value);

        /// <summary>
        /// Set the enablement of this proxy.
        /// </summary>
        /// <param name="sender">The object setting the enable value.</param>
        /// <param name="value">The new enable value.</param>
        void SetIsEnabled(Windows::Foundation::IInspectable const& sender, bool value);
    private:

        /// <summary>
        /// The handler to be called when the corresponding element joins the visual tree.
        /// </summary>
        void OnLoaded(Windows::Foundation::IInspectable const& sender, Microsoft::UI::Xaml::RoutedEventArgs const& e);

        /// <summary>
        /// The handler to be called when the corresponding element leaves the visual tree.
        /// </summary>
        void OnUnloaded(Windows::Foundation::IInspectable const& sender, Microsoft::UI::Xaml::RoutedEventArgs const& e);

        /// <summary>
        /// Non-zero ID associated with this instance.
        /// </summary>
        int _uniqueId;

        /// <summary>
        /// Indicator that the corresponding element is part of the visual tree.
        /// </summary>
        bool _isLoaded{ false };

        /// <summary>
        /// Boolean representing whether gaze is enabled for the corresponding element and its subtree.
        /// </summary>
        bool _isEnabled{ false };

        static Microsoft::UI::Xaml::DependencyProperty m_gazePointerProxyProperty;
    };
}
namespace winrt::Microsoft::Toolkit::Uwp::Input::GazeInteraction::factory_implementation
{
    struct GazePointerProxy : GazePointerProxyT<GazePointerProxy, implementation::GazePointerProxy>
    {
    };
}
