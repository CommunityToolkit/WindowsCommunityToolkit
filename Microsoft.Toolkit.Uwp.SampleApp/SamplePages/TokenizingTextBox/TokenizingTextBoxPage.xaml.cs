using Microsoft.Toolkit.Uwp.UI.Controls;
using Microsoft.Toolkit.Uwp.UI.Extensions;
using System;
using System.Diagnostics;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Microsoft.Toolkit.Uwp.SampleApp.SamplePages
{
    public sealed partial class TokenizingTextBoxPage : Page, IXamlRenderListener
    {
        private TokenizingTextBox _ttb;

        public TokenizingTextBoxPage()
        {
            InitializeComponent();
        }

        public void OnXamlRendered(FrameworkElement control)
        {
            if (_ttb != null)
            {
                _ttb.TokenItemAdded -= TokenItemAdded;
                _ttb.TokenItemRemoved -= TokenItemRemoved;
            }

            if (control.FindChildByName("TokenBox") is TokenizingTextBox ttb)
            {
                _ttb = ttb;

                _ttb.TokenItemAdded += TokenItemAdded;
                _ttb.TokenItemRemoved += TokenItemRemoved;
            }
        }

        private void TokenItemAdded(TokenizingTextBox sender, TokenizingTextBoxItem args)
        {
            // TODO: Add InApp Notification?
            Debug.WriteLine("Added Token: " + args.Content);
        }

        private void TokenItemRemoved(TokenizingTextBox sender, TokenItemRemovedEventArgs args)
        {
            Debug.WriteLine("Removed Token: " + args.Item);
        }
    }
}
