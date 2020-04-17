//Copyright (c) Microsoft. All rights reserved. Licensed under the MIT license.
//See LICENSE in the project root for license information.

#include "pch.h"
#include "DwellInvokedRoutedEventArgs.h"
#include "DwellInvokedRoutedEventArgs.g.cpp"

namespace winrt::Microsoft::Toolkit::Uwp::Input::GazeInteraction::implementation
{
    bool DwellInvokedRoutedEventArgs::Handled()
    {
        return _handled;
    }
    void DwellInvokedRoutedEventArgs::Handled(bool value)
    {
        _handled = value;
    }
}
