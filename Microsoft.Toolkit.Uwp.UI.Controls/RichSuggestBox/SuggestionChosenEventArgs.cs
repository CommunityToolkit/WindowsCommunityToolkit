// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using Microsoft.Toolkit.Uwp.Deferred;

namespace Microsoft.Toolkit.Uwp.UI.Controls
{
    public class SuggestionChosenEventArgs : DeferredEventArgs
    {
        public string Query { get; internal set; }

        public string Prefix { get; internal set; }

        public string Text { get; set; }

        public object SelectedItem { get; internal set; }

        public Guid Id { get; internal set; }

        public SuggestionTokenFormat Format { get; internal set; }
    }
}
