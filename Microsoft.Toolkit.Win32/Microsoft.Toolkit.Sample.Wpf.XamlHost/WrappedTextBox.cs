// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.ComponentModel;
using System.Windows.Media;
using Microsoft.Toolkit.Win32.UI.XamlHost;
using Microsoft.Toolkit.Wpf.UI.XamlHost;

namespace Microsoft.Toolkit.Sample.Wpf.XamlHost
{
    public class WrappedTextBox : WindowsXamlHostBase
    {

        public WrappedTextBox() : base()
        {
        }

        protected override void OnInitialized(EventArgs e)
        {
            base.OnInitialized(e);

            this.ChildInternal = UWPTypeFactory.CreateXamlContentByType("Windows.UI.Xaml.Controls.TextBox");

            // Set DesktopWindowXamlSource
            SetContent();

            PlaceholderText = placeholderText;
        }

        [Category("UWP XAML TextBox")]
        string placeholderText;
        public string PlaceholderText
        {
            get
            {
                return placeholderText;
            }
            set
            {
                placeholderText = value;

                // UWP XAML content is not created until base.OnInitialized
                if (this.ChildInternal != null)
                {
                    global::Windows.UI.Xaml.Controls.TextBox textBox = this.ChildInternal as global::Windows.UI.Xaml.Controls.TextBox;

                    textBox.PlaceholderText = value;
                }
            }
        }
    }
}
