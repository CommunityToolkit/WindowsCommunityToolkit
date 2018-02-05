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
using Microsoft.Toolkit.Uwp.SampleApp.SamplePages.TextToolbarSamples;
using Microsoft.Toolkit.Uwp.UI.Controls;
using Microsoft.Toolkit.Uwp.UI.Controls.TextToolbarButtons;
using Microsoft.Toolkit.Uwp.UI.Controls.TextToolbarFormats.MarkDown;
using Microsoft.Toolkit.Uwp.UI.Extensions;
using Windows.System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace Microsoft.Toolkit.Uwp.SampleApp.SamplePages
{
    public sealed partial class TextToolbarPage : IXamlRenderListener
    {
        private TextToolbar _toolbar;
        private MarkdownTextBlock _previewer;

        public TextToolbarPage()
        {
            InitializeComponent();
        }

        public void OnXamlRendered(FrameworkElement control)
        {
            _toolbar = control.FindChildByName("Toolbar") as TextToolbar;

            var editZone = control.FindChildByName("EditZone") as RichEditBox;
            if (editZone != null)
            {
                editZone.TextChanged += EditZone_TextChanged;
            }

            _previewer = control.FindChildByName("Previewer") as MarkdownTextBlock;
            if (_previewer != null)
            {
                _previewer.LinkClicked += Previewer_LinkClicked;
            }
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            Shell.Current.RegisterNewCommand("Add/Remove Bold Button", (sender, args) =>
            {
                var button = _toolbar?.GetDefaultButton(ButtonType.Bold);
                if (button != null)
                {
                    button.Visibility = button.Visibility == Windows.UI.Xaml.Visibility.Visible ? Windows.UI.Xaml.Visibility.Collapsed : Windows.UI.Xaml.Visibility.Visible;
                }
            });

            Shell.Current.RegisterNewCommand("Add Custom Button", (sender, args) =>
            {
                AddCustomButton();
            });

            Shell.Current.RegisterNewCommand("Use Custom Formatter", (sender, args) =>
            {
                UseCustomFormatter();
            });
        }

        private void UseCustomFormatter()
        {
            if (_toolbar == null)
            {
                return;
            }

            var formatter = new SampleFormatter(_toolbar);
            _toolbar.Format = UI.Controls.TextToolbarFormats.Format.Custom;
            _toolbar.Formatter = formatter;
        }

        private int DemoCounter { get; set; } = 0;

        private void AddCustomButton()
        {
            if (_toolbar == null)
            {
                return;
            }

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
                    var md = _toolbar.Formatter as MarkDownFormatter;
                    if (md != null)
                    {
                        md.SetSelection($"[{demoText}]", $"[/{demoText}]");
                    }
                    else
                    {
                        _toolbar.Formatter.Selected.Text = $"This was filled by {demoText} button ";
                    }
                }
            };

            _toolbar.CustomButtons.Add(demoButton);
        }

        private void Previewer_LinkClicked(object sender, LinkClickedEventArgs e)
        {
            try
            {
                var link = new Uri(e.Link);
                var linkOpen = Task.Run(() => Launcher.LaunchUriAsync(link));
            }
            catch
            {
            }
        }

        private void EditZone_TextChanged(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            if (_toolbar == null || _previewer == null)
            {
                return;
            }

            var md = _toolbar.Formatter as MarkDownFormatter;
            if (md != null)
            {
                string text = md.Text;
                _previewer.Text = string.IsNullOrWhiteSpace(text) ? "Nothing to Preview" : text;
            }
        }
    }
}