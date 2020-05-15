// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Microsoft.Toolkit.Uwp.UI.Controls
{
    public class TokenizingTextBoxTemplateSelector : DataTemplateSelector
    {
        public DataTemplate TokenTemplate { get; set; }

        public DataTemplate TextTemplate { get; set; }

        public TokenizingTextBoxTemplateSelector()
        {
        }

        protected override DataTemplate SelectTemplateCore(object item)
        {
            // Return the correct data template based on the item's type.
            if (item.GetType() == typeof(string))
            {
                return TextTemplate;
            }
            else
            {
                return TokenTemplate;
            }
        }
    }
}
