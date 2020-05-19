// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Microsoft.UI.Xaml;

namespace Microsoft.Toolkit.Uwp.UI.Controls
{
    internal class PretokenStringContainer : DependencyObject
    {
        public string Text
        {
            get { return (string)GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Text.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty TextProperty =
            DependencyProperty.Register(nameof(Text), typeof(string), typeof(PretokenStringContainer), new PropertyMetadata(string.Empty));

        public PretokenStringContainer()
        {
        }

        public PretokenStringContainer(string text)
        {
            Text = text;
        }
    }
}
