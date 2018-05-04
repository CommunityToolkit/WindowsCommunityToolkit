//Copyright (c) Microsoft. All rights reserved. Licensed under the MIT license.
//See LICENSE in the project root for license information.

#pragma once

BEGIN_NAMESPACE_GAZE_INPUT

/// <summary>
/// This parameter is passed to the GazeElement::Invoked event and allows 
/// the application to prevent default invocation when the user dwells on a control
/// </summary>
public ref class DwellInvokedRoutedEventArgs : public RoutedEventArgs
{
public:

    /// <summary>
    /// The application should set this value to true to prevent invoking the control when the user dwells on a control
    /// </summary>
    property bool Handled;

internal:

    DwellInvokedRoutedEventArgs()
    {
    }
};

END_NAMESPACE_GAZE_INPUT