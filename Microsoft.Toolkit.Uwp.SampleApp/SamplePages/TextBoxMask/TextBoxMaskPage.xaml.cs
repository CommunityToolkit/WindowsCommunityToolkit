// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Microsoft.Toolkit.Uwp.UI.Extensions;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace Microsoft.Toolkit.Uwp.SampleApp.SamplePages
{
    /// <summary>
    /// Textbox Mask sample page
    /// </summary>
    public sealed partial class TextBoxMaskPage : Page, IXamlRenderListener
    {
        private TextBox alphaTextBox;

        public TextBoxMaskPage()
        {
            InitializeComponent();
        }

        public void OnXamlRendered(FrameworkElement control)
        {
            alphaTextBox = control.FindChildByName("AlphaTextBox") as TextBox;
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            Shell.Current.RegisterNewCommand("Apply Full Mask", (s, e2) =>
            {
                if (alphaTextBox != null)
                {
                    alphaTextBox.Text = "7b1y--x4a5";
                }
            });

            Shell.Current.RegisterNewCommand("Apply Partial Mask", (s, e2) =>
            {
                if (alphaTextBox != null)
                {
                    alphaTextBox.Text = "7b1yZW";
                }
            });
        }
    }
}
