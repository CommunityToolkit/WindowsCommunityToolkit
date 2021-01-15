// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Windows.Foundation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Microsoft.Toolkit.Uwp.UI.Controls
{
    public partial class RichSuggestBox
    {
        public event TypedEventHandler<RichSuggestBox, SuggestionsRequestedEventArgs> SuggestionsRequested;

        public event TypedEventHandler<RichSuggestBox, SuggestionChosenEventArgs> SuggestionChosen;

        public event TypedEventHandler<RichEditBox, RoutedEventArgs> TextChanged;
    }
}
