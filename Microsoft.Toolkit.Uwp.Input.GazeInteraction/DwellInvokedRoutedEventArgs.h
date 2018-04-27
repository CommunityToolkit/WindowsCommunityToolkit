//Copyright (c) Microsoft. All rights reserved. Licensed under the MIT license.
//See LICENSE in the project root for license information.

#pragma once

BEGIN_NAMESPACE_GAZE_INPUT

/// <summary>
/// TODO: harishsk
/// </summary>
public ref class DwellInvokedRoutedEventArgs : public RoutedEventArgs
{
public:

    /// <summary>
    /// TODO: harishsk
    /// </summary>
    property bool Handled;

internal:

    DwellInvokedRoutedEventArgs()
    {
    }
};

END_NAMESPACE_GAZE_INPUT