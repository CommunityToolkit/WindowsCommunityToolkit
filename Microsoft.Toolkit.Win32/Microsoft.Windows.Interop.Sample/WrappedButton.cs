// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

namespace Microsoft.Windows.Interop.Sample
{
    using System;
    using System.ComponentModel;
    using System.Windows.Media;

    using Microsoft.Windows.Interop;

    public class WrappedButton : WindowsXamlHost
    {

        #region Constructors and Initialization
        public WrappedButton() : base()
        {
            base.TypeName = "Windows.UI.Xaml.Controls.Button";
        }

        protected override void OnInitialized(EventArgs e)
        {
            base.OnInitialized(e);

            // Properties set in markup need to be re-applied in OnInitialized.  
            Background = background;
            Content = content;
        }

        #endregion

        #region Properties 

        private string background;

        [Category("UWP XAML Button")]
        public string Background
        {
            set
            {
                background = value;

                // UWP XAML content is not created until base.OnInitialized
                if (value != null && this.XamlRoot != null)
                {
                    global::Windows.UI.Xaml.Controls.Button button = this.XamlRoot as global::Windows.UI.Xaml.Controls.Button;

                    Color wpfColor = (Color)ColorConverter.ConvertFromString(value);

                    button.Background = new global::Windows.UI.Xaml.Media.SolidColorBrush(ConvertWPFColorToUWPColor(wpfColor));
                }

            }
        }

        [Category("UWP XAML Button")]
        string content;
        public string Content
        {
            get
            {
                return content;
            }
            set
            {
                content = value;

                // UWP XAML content is not created until base.OnInitialized
                if (this.XamlRoot != null)
                {
                    global::Windows.UI.Xaml.Controls.Button button = this.XamlRoot as global::Windows.UI.Xaml.Controls.Button;

                    button.Content = value;
                }
            }
        }
            

        [Browsable(false)]
        public override string TypeName
        {
            get
            {
                return base.TypeName;
            }
            set
            {
                // Don't allow setting TypeName
            }
        }

        #endregion

        #region Helpers
        private global::Windows.UI.Color ConvertWPFColorToUWPColor(Color wpfColor)
        {
            return global::Windows.UI.Color.FromArgb(wpfColor.A, wpfColor.R, wpfColor.G, wpfColor.B);
        }

        #endregion
    }
}
