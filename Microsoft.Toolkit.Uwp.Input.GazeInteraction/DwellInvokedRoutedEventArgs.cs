// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Windows.UI.Xaml;

namespace Microsoft.Toolkit.Uwp.Input.GazeInteraction
{
    /// <summary>
    /// This parameter is passed to the GazeElement::Invoked event and allows
    /// the application to prevent default invocation when the user dwells on a control
    /// </summary>
    public class DwellInvokedRoutedEventArgs : RoutedEventArgs
    {
        /// <summary>
        /// Gets or sets a value indicating whether the application handled the event. If this parameter is set to true, the library prevents invoking the control when the user dwells on a control
        /// </summary>
        public bool Handled { get; set; }

        internal DwellInvokedRoutedEventArgs()
        {
        }
    }
}