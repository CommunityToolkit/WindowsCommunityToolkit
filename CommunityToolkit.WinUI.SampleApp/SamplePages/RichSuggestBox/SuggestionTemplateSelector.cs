// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;

namespace CommunityToolkit.WinUI.SampleApp.SamplePages
{
    public class SuggestionTemplateSelector : DataTemplateSelector
    {
        public DataTemplate Person { get; set; }

        public DataTemplate Data { get; set; }

        protected override DataTemplate SelectTemplateCore(object item)
        {
            return item is SampleEmailDataType ? this.Person : this.Data;
        }
    }
}
