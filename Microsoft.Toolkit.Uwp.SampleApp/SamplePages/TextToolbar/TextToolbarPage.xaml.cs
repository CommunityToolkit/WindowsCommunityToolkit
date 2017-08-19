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

using System;
using System.Threading.Tasks;
using Microsoft.Toolkit.Uwp.UI.Controls;
using Microsoft.Toolkit.Uwp.UI.Controls.TextToolbarButtons;
using Microsoft.Toolkit.Uwp.UI.Controls.TextToolbarFormats.MarkDown;
using Windows.System;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace Microsoft.Toolkit.Uwp.SampleApp.SamplePages
{
    public sealed partial class TextToolbarPage
    {
        public TextToolbarPage()
        {
            InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            Shell.Current.RegisterNewCommand("Add/Remove Bold Button", (sender, args) =>
            {
                var button = DefaultButton.OfType(ButtonType.Bold);
                if (!codeRemoved)
                {
                    Toolbar.RemoveDefaultButtons.Add(button);
                    codeRemoved = true;
                }
                else
                {
                    Toolbar.RemoveDefaultButtons.Remove(button);
                    codeRemoved = false;
                }
            });

            Shell.Current.RegisterNewCommand("Add Custom Button", (sender, args) =>
            {
                AddCustomButton();
            });
        }

        private bool codeRemoved = false;

        private int DemoCounter { get; set; } = 0;

        private void AddCustomButton()
        {
            string demoText = "Demo";
            demoText = DemoCounter > 0 ? demoText + DemoCounter : demoText;

            int keycode = (int)VirtualKey.Number0 + DemoCounter;
            VirtualKey? shortcut = null;

            if (keycode <= (int)VirtualKey.Number9)
            {
                shortcut = (VirtualKey)keycode;
            }

            DemoCounter++;

            var demoButton = new ToolbarButton
            {
                Icon = new SymbolIcon { Symbol = Symbol.ReportHacked },
                ToolTip = demoText,
                ShortcutKey = shortcut,
                Activation = (b) =>
                {
                    var md = Toolbar.Formatter as MarkDownFormatter;
                    if (md != null)
                    {
                        md.SetSelection($"[{demoText}]", $"[/{demoText}]");
                    }
                    else
                    {
                        Toolbar.Formatter.Selected.Text = $"This was filled by {demoText} button ";
                    }
                }
            };

            Toolbar.CustomButtons.Add(demoButton);
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

        private void EditZone_TextChanged(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            var md = Toolbar.Formatter as MarkDownFormatter;
            if (md != null)
            {
                string text = md.Text;
                Previewer.Text = string.IsNullOrWhiteSpace(text) ? "Nothing to Preview" : text;
            }
        }
    }
}