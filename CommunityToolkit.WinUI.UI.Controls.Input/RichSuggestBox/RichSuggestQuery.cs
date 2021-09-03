// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Threading;
using Microsoft.UI.Text;

namespace CommunityToolkit.WinUI.UI.Controls
{
    /// <summary>
    /// A structure for <see cref="RichSuggestBox"/> to keep track of the current query internally.
    /// </summary>
    internal class RichSuggestQuery
    {
        public string Prefix { get; set; }

        public string QueryText { get; set; }

        public ITextRange Range { get; set; }

        public CancellationTokenSource CancellationTokenSource { get; set; }
    }
}
