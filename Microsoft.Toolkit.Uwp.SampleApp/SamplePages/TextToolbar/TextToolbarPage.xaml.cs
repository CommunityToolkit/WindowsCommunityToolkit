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

namespace Microsoft.Toolkit.Uwp.SampleApp.SamplePages
{
    using System;
    using System.Threading.Tasks;
    using Microsoft.Toolkit.Uwp.UI.Controls;
    using Microsoft.Toolkit.Uwp.UI.Controls.TextToolbarButtons;
    using Microsoft.Toolkit.Uwp.UI.Controls.TextToolbarFormats;
    using Windows.System;
    using Windows.UI.Xaml;
    using Windows.UI.Xaml.Controls;
    using Windows.UI.Xaml.Navigation;

    public sealed partial class TextToolbarPage
    {
        public TextToolbarPage()
        {
            InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            Shell.Current.RegisterNewCommand("Switch Theme", (sender, args) =>
            {
                MainGrid.RequestedTheme = MainGrid.RequestedTheme == ElementTheme.Light ? ElementTheme.Dark : ElementTheme.Light;
            });

            Shell.Current.RegisterNewCommand("Remove Code Button", (sender, args) =>
            {
                Toolbar.RemoveDefaultButton(DefaultButton.Code);
            });

            Shell.Current.RegisterNewCommand("Add Custom Button", (sender, args) =>
            {
                AddCustomButton();
            });
        }

        private int DemoCounter { get; set; } = 0;

        private void AddCustomButton()
        {
            string demoText = "Demo";
            demoText = DemoCounter > 0 ? demoText + DemoCounter : demoText;
            DemoCounter++;

            Toolbar.CustomItems.Add(new CustomToolbarButton
            {
                Icon = new SymbolIcon { Symbol = Symbol.ReportHacked },
                Label = demoText,
                Action = new Action(() =>
                {
                    if (Toolbar.Formatter is MarkDownFormatter md)
                    {
                        md.SetSelection($"[{demoText}]", $"[/{demoText}]");
                    }
                })
            });
        }

        private void EditZone_TextChanged(object sender, RoutedEventArgs e)
        {
            string text = Toolbar.Formatter.Text;
            Previewer.Text = string.IsNullOrWhiteSpace(text) ? "Nothing to Preview" : text;
        }

        private void Previewer_LinkClicked(object sender, LinkClickedEventArgs e)
        {
            try
            {
                var linkOpen = Task.Run(() => Launcher.LaunchUriAsync(new Uri(e.Link)));
            }
            catch
            {
            }
        }
    }
}