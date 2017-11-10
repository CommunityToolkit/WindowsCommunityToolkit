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
<<<<<<< HEAD
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Toolkit.Uwp.SampleApp.Models;
using Microsoft.Toolkit.Uwp.SampleApp.SamplePages.TextToolbarSamples;
using Microsoft.Toolkit.Uwp.UI.Controls;
using Microsoft.Toolkit.Uwp.UI.Controls.TextToolbarButtons;
using Microsoft.Toolkit.Uwp.UI.Controls.TextToolbarFormats;
=======
using System.Threading.Tasks;
using Microsoft.Toolkit.Uwp.SampleApp.SamplePages.TextToolbarSamples;
using Microsoft.Toolkit.Uwp.UI.Controls;
using Microsoft.Toolkit.Uwp.UI.Controls.TextToolbarButtons;
>>>>>>> fb2912293936b8803e6224af5086e6d0c8780bcd
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

<<<<<<< HEAD
            if (control.FindChildByName("EditZone") is RichEditBox editZone)
=======
            var editZone = control.FindChildByName("EditZone") as RichEditBox;
            if (editZone != null)
>>>>>>> fb2912293936b8803e6224af5086e6d0c8780bcd
            {
                editZone.TextChanged += EditZone_TextChanged;
            }

<<<<<<< HEAD
            if (control.FindChildByName("Previewer") is MarkdownTextBlock previewer)
            {
                _previewer = previewer;
                _previewer.LinkClicked += Previewer_LinkClicked;
            }

            if (ToolbarFormat != null && (Format)ToolbarFormat.Value == Format.Custom)
            {
                UseCustomFormatter();
            }
=======
            _previewer = control.FindChildByName("Previewer") as MarkdownTextBlock;
            if (_previewer != null)
            {
                _previewer.LinkClicked += Previewer_LinkClicked;
            }
>>>>>>> fb2912293936b8803e6224af5086e6d0c8780bcd
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            Shell.Current.RegisterNewCommand("Add/Remove Bold Button", (sender, args) =>
            {
                var button = _toolbar?.GetDefaultButton(ButtonType.Bold);
                if (button != null)
                {
<<<<<<< HEAD
                    button.Visibility = button.Visibility == Visibility.Visible ? Visibility.Collapsed : Visibility.Visible;
=======
                    button.Visibility = button.Visibility == Windows.UI.Xaml.Visibility.Visible ? Windows.UI.Xaml.Visibility.Collapsed : Windows.UI.Xaml.Visibility.Visible;
>>>>>>> fb2912293936b8803e6224af5086e6d0c8780bcd
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
<<<<<<< HEAD

            Shell.Current.RegisterNewCommand("Reset Layout", (sender, args) =>
            {
                ResetLayout();
            });
        }

        private void ResetLayout()
        {
            if (_toolbar == null)
            {
                return;
            }

            _toolbar.CustomButtons.Clear();
            if (_toolbar.DefaultButtons != null)
            {
                foreach (var item in _toolbar.DefaultButtons)
                {
                    if (item is ToolbarButton button)
                    {
                        button.Visibility = Visibility.Visible;
                    }
                }
            }
=======
>>>>>>> fb2912293936b8803e6224af5086e6d0c8780bcd
        }

        private void UseCustomFormatter()
        {
<<<<<<< HEAD
            if (_toolbar == null || ToolbarFormat == null)
=======
            if (_toolbar == null)
>>>>>>> fb2912293936b8803e6224af5086e6d0c8780bcd
            {
                return;
            }

            var formatter = new SampleFormatter(_toolbar);
<<<<<<< HEAD
            ToolbarFormat.Value = Format.Custom;
            _toolbar.Formatter = formatter;
        }

=======
            _toolbar.Format = UI.Controls.TextToolbarFormats.Format.Custom;
            _toolbar.Formatter = formatter;
        }

        private int DemoCounter { get; set; } = 0;

>>>>>>> fb2912293936b8803e6224af5086e6d0c8780bcd
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
<<<<<<< HEAD
                    if (_toolbar.Formatter is MarkDownFormatter md)
=======
                    var md = _toolbar.Formatter as MarkDownFormatter;
                    if (md != null)
>>>>>>> fb2912293936b8803e6224af5086e6d0c8780bcd
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

<<<<<<< HEAD
        private void EditZone_TextChanged(object sender, RoutedEventArgs e)
=======
        private void EditZone_TextChanged(object sender, Windows.UI.Xaml.RoutedEventArgs e)
>>>>>>> fb2912293936b8803e6224af5086e6d0c8780bcd
        {
            if (_toolbar == null || _previewer == null)
            {
                return;
            }

<<<<<<< HEAD
            if (_toolbar.Formatter is MarkDownFormatter md)
=======
            var md = _toolbar.Formatter as MarkDownFormatter;
            if (md != null)
>>>>>>> fb2912293936b8803e6224af5086e6d0c8780bcd
            {
                string text = md.Text;
                _previewer.Text = string.IsNullOrWhiteSpace(text) ? "Nothing to Preview" : text;
            }
        }
<<<<<<< HEAD

        private int DemoCounter { get; set; } = 0;

        private ValueHolder ToolbarFormat
        {
            get
            {
                if (DataContext is Sample sample)
                {
                    if (sample.PropertyDescriptor.Expando is IDictionary<string, object> properties && properties.TryGetValue("Format", out var format))
                    {
                        return format as ValueHolder;
                    }
                }

                return null;
            }
        }
=======
>>>>>>> fb2912293936b8803e6224af5086e6d0c8780bcd
    }
}