// ******************************************************************
// Copyright (c) Microsoft. All rights reserved.
// This code is licensed under the MIT License (MIT).
// THE CODE IS PROVIDED “AS IS”, WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED,
// INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.
// IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM,
// DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT,
// TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH
// THE CODE OR THE USE OR OTHER DEALINGS IN THE CODE.
// ******************************************************************

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
