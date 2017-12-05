// ******************************************************************
// Copyright (c) Microsoft. All rights reserved.
// This code is licensed under the MIT License (MIT).
// THE CODE IS PROVIDED “AS IS”, WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED,
// INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.
// IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM,
// DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT,
// TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH
// THE CODE OR THE USE OR OTHER DEALINGS IN THE CODE.
// ******************************************************************

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
