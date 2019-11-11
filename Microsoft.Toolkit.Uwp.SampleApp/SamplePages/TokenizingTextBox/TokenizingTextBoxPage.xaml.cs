// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Toolkit.Uwp.UI.Controls;
using Microsoft.Toolkit.Uwp.UI.Extensions;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Microsoft.Toolkit.Uwp.SampleApp.SamplePages
{
    public sealed partial class TokenizingTextBoxPage : Page, IXamlRenderListener
    {
        private TokenizingTextBox _ttb;

        private List<SampleDataType> _samples = new List<SampleDataType>()
        {
            new SampleDataType() { Text = "Account", Icon = Symbol.Account },
            new SampleDataType() { Text = "Add Friend", Icon = Symbol.AddFriend },
            new SampleDataType() { Text = "Attach", Icon = Symbol.Attach },
            new SampleDataType() { Text = "Attach Camera", Icon = Symbol.AttachCamera },
            new SampleDataType() { Text = "Audio", Icon = Symbol.Audio },
            new SampleDataType() { Text = "Block Contact", Icon = Symbol.BlockContact },
            new SampleDataType() { Text = "Calculator", Icon = Symbol.Calculator },
            new SampleDataType() { Text = "Calendar", Icon = Symbol.Calendar },
            new SampleDataType() { Text = "Camera", Icon = Symbol.Camera },
            new SampleDataType() { Text = "Contact", Icon = Symbol.Contact },
            new SampleDataType() { Text = "Favorite", Icon = Symbol.Favorite },
            new SampleDataType() { Text = "Link", Icon = Symbol.Link },
            new SampleDataType() { Text = "Mail", Icon = Symbol.Mail },
            new SampleDataType() { Text = "Map", Icon = Symbol.Map },
            new SampleDataType() { Text = "Phone", Icon = Symbol.Phone },
            new SampleDataType() { Text = "Pin", Icon = Symbol.Pin },
            new SampleDataType() { Text = "Rotate", Icon = Symbol.Rotate },
            new SampleDataType() { Text = "Rotate Camera", Icon = Symbol.RotateCamera },
            new SampleDataType() { Text = "Send", Icon = Symbol.Send },
            new SampleDataType() { Text = "Tags", Icon = Symbol.Tag },
            new SampleDataType() { Text = "UnFavorite", Icon = Symbol.UnFavorite },
            new SampleDataType() { Text = "UnPin", Icon = Symbol.UnPin },
            new SampleDataType() { Text = "Zoom", Icon = Symbol.Zoom },
            new SampleDataType() { Text = "ZoomIn", Icon = Symbol.ZoomIn },
            new SampleDataType() { Text = "ZoomOut", Icon = Symbol.ZoomOut },
        };

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
                _ttb.TextChanged -= TextChanged;
                _ttb.TokenItemCreating -= TokenItemCreating;
            }

            if (control.FindChildByName("TokenBox") is TokenizingTextBox ttb)
            {
                _ttb = ttb;

                _ttb.TokenItemAdded += TokenItemAdded;
                _ttb.TokenItemRemoved += TokenItemRemoved;
                _ttb.TextChanged += TextChanged;
                _ttb.TokenItemCreating += TokenItemCreating;
            }
        }

        private void TextChanged(AutoSuggestBox sender, AutoSuggestBoxTextChangedEventArgs args)
        {
            if (args.CheckCurrent() && args.Reason == AutoSuggestionBoxTextChangeReason.UserInput)
            {
                if (string.IsNullOrWhiteSpace(sender.Text))
                {
                    _ttb.SuggestedItemsSource = Array.Empty<object>();
                }
                else
                {
                    _ttb.SuggestedItemsSource = _samples.Where((item) => item.Text.Contains(sender.Text, System.StringComparison.CurrentCultureIgnoreCase)).OrderByDescending(item => item.Text);
                }
            }
        }

        private void TokenItemCreating(object sender, TokenItemCreatingEventArgs e)
        {
            // Take the user's text and convert it to our data type.
            e.Item = _samples.FirstOrDefault((item) => item.Text.Contains(e.TokenText, System.StringComparison.CurrentCultureIgnoreCase));
        }

        private void TokenItemAdded(TokenizingTextBox sender, TokenizingTextBoxItem args)
        {
            // TODO: Add InApp Notification?
            if (args.Content is SampleDataType sample)
            {
                Debug.WriteLine("Added Token: " + sample.Text);
            }
            else
            {
                Debug.WriteLine("Added Token: " + args.Content);
            }
        }

        private void TokenItemRemoved(TokenizingTextBox sender, TokenItemRemovedEventArgs args)
        {
            if (args.Item is SampleDataType sample)
            {
                Debug.WriteLine("Removed Token: " + sample.Text);
            }
            else
            {
                Debug.WriteLine("Removed Token: " + args.Item);
            }
        }
    }
}
