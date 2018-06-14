// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Microsoft.Toolkit.Services.OneDrive;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Microsoft.Toolkit.Uwp.SampleApp.SamplePages
{
    /// <summary>
    /// FileFolderDataTemplateSelector class
    /// </summary>
    public class OneDriveDataTemplateSelector : DataTemplateSelector
    {
        /// <summary>
        /// Gets or sets the File DataTemplate
        /// </summary>
        public DataTemplate FileTemplate { get; set; }

        /// <summary>
        /// Gets or sets the Folder DataTemplate
        /// </summary>
        public DataTemplate FolderTemplate { get; set; }

        /// <summary>
        /// Gets or sets the OneNote DataTemplate
        /// </summary>
        public DataTemplate NoteTemplate { get; set; }

        protected override DataTemplate SelectTemplateCore(object item, DependencyObject container)
        {
            if (item is OneDriveStorageItem oneDriveItem)
            {
                if (oneDriveItem.IsFile())
                {
                    return FileTemplate;
                }

                if (oneDriveItem.IsFolder())
                {
                    return FolderTemplate;
                }

                if (oneDriveItem.IsOneNote())
                {
                    return NoteTemplate;
                }
            }

            return FolderTemplate;
        }
    }
}
