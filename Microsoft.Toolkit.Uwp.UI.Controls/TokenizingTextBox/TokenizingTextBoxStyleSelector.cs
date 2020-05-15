// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Microsoft.Toolkit.Uwp.UI.Controls
{
    public class TokenizingTextBoxStyleSelector : StyleSelector
    {
        public Style TokenStyle { get; set; }

        public Style TextStyle { get; set; }

        public TokenizingTextBoxStyleSelector()
        {
        }

        protected override Style SelectStyleCore(object item, DependencyObject container)
        {
            if (item is PretokenStringContainer)
            {
                return TextStyle;
            }

            return TokenStyle;

            ////return base.SelectStyleCore(item, container);
        }
    }
}
