//Copyright (c) Microsoft. All rights reserved. Licensed under the MIT license.
//See LICENSE in the project root for license information.

#pragma once

#include "Interaction.h"

BEGIN_NAMESPACE_GAZE_INPUT

/// <summary>
/// Helper class that helps track which UIElements in the visual tree are enabled.
///
/// The GazePointer is enabled when one or more UIElements in the visual tree have
/// their GazeInput.InteractionProperty value set to Enabled. Notice that there are
/// two conditions for enablement: that attached property is Enabled; that the UIElement
/// is in the visual tree.
/// </summary>
private ref class GazePointerProxy sealed
{
public:

    /// <summary>
    /// A private attached property for associating an instance of this class with the UIElement
    /// to which it refers.
    /// </summary>
    static property DependencyProperty^ GazePointerProxyProperty { DependencyProperty^ get(); };

internal:

    /// <summary>
    /// Method called when the GazeInput.Interaction attached property is set to a new value.
    /// </summary>
    /// <param name="element">The element being set. May be null to indicate whole user interface.</param>
    /// <param name="value">The interaction enablement value being set.</param>
    static void SetInteraction(FrameworkElement^ element, Interaction value);

private:

    /// <summary>
    /// Constructor.
    /// </summary>
    /// <param name="element">The element proxy is attaching to.</param>
    GazePointerProxy(FrameworkElement^ element);

    /// <summary>
    /// Set the enablement of this proxy.
    /// </summary>
    /// <param name="sender">The object setting the enable value.</param>
    /// <param name="value">The new enable value.</param>
    void SetIsEnabled(Object^ sender, bool value);

    /// <summary>
    /// The handler to be called when the corresponding element joins the visual tree.
    /// </summary>
    void OnLoaded(Object^ sender, RoutedEventArgs^ args);

    /// <summary>
    /// The handler to be called when the corresponding element leaves the visual tree.
    /// </summary>
    void OnUnloaded(Object^ sender, RoutedEventArgs^ args);

private:

    /// <summary>
    /// Non-zero ID associated with this instance.
    /// </summary>
    int _uniqueId;

    /// <summary>
    /// Indicator that the corresponding element is part of the visual tree.
    /// </summary>
    bool _isLoaded;

    /// <summary>
    /// Boolean representing whether gaze is enabled for the corresponding element and its subtree.
    /// </summary>
    bool _isEnabled;
};

END_NAMESPACE_GAZE_INPUT


