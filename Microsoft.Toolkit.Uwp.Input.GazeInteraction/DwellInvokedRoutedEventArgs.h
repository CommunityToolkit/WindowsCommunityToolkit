//Copyright (c) Microsoft. All rights reserved. Licensed under the MIT license.
//See LICENSE in the project root for license information.

#pragma once
#include "DwellInvokedRoutedEventArgs.g.h"

namespace winrt::Microsoft::Toolkit::Uwp::Input::GazeInteraction::implementation
{
    /// <summary>
    /// This parameter is passed to the GazeElement::Invoked event and allows 
    /// the application to prevent default invocation when the user dwells on a control
    /// </summary>
    struct DwellInvokedRoutedEventArgs : DwellInvokedRoutedEventArgsT<DwellInvokedRoutedEventArgs>
    {
    public:
        DwellInvokedRoutedEventArgs() = default;

        /// <summary>
        /// The application should set this value to true to prevent invoking the control when the user dwells on a control
        /// </summary>
        bool Handled();
        void Handled(bool value);

    private:
        bool _handled{ false };
    };
}
namespace winrt::Microsoft::Toolkit::Uwp::Input::GazeInteraction::factory_implementation
{
    struct DwellInvokedRoutedEventArgs : DwellInvokedRoutedEventArgsT<DwellInvokedRoutedEventArgs, implementation::DwellInvokedRoutedEventArgs>
    {
    };
}
