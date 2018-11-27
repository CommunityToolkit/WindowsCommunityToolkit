// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Microsoft.Toolkit.Uwp.SampleApp.Models;
using Microsoft.Toolkit.Uwp.UI.Controls;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace Microsoft.Toolkit.Uwp.SampleApp.SamplePages
{
    /// <summary>
    /// A page that shows how to use the DropShadowPanel control.
    /// </summary>
    public sealed partial class DropShadowPanelPage : Page
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DropShadowPanelPage"/> class.
        /// </summary>
        public DropShadowPanelPage()
        {
            InitializeComponent();
            Load();
        }

        private void Load()
        {
            if (!DropShadowPanel.IsSupported)
            {
                WarningText.Visibility = Visibility.Visible;
            }
        }
    }
}
