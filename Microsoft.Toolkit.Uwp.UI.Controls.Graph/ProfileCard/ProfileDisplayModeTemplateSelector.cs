// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Microsoft.Toolkit.Uwp.UI.Controls.Graph
{
    internal class ProfileDisplayModeTemplateSelector : DataTemplateSelector
    {
        private ContentControl _contentPresenter;

        internal ProfileDisplayModeTemplateSelector(ContentControl contentPresenter)
        {
            _contentPresenter = contentPresenter;
        }

        private DataTemplate SelectItemTemplate(object item)
        {
            DataTemplate dataTemplate = null;

            if (item != null && item is ProfileCardItem profileItem)
            {
                dataTemplate = _contentPresenter.Resources[profileItem.DisplayMode.ToString()] as DataTemplate;
            }

            return dataTemplate;
        }

        protected override DataTemplate SelectTemplateCore(object item)
        {
            return SelectItemTemplate(item);
        }

        protected override DataTemplate SelectTemplateCore(object item, DependencyObject container)
        {
            return SelectItemTemplate(item);
        }
    }
}
