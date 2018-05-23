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
                switch (profileItem.DisplayMode)
                {
                    case ViewType.EmailOnly:
                        dataTemplate = _contentPresenter.Resources["EmailOnly"] as DataTemplate;
                        break;
                    case ViewType.PictureOnly:
                        dataTemplate = _contentPresenter.Resources["PictureOnly"] as DataTemplate;
                        break;
                    case ViewType.LargeProfilePhotoLeft:
                        dataTemplate = _contentPresenter.Resources["LargeProfilePhotoLeft"] as DataTemplate;
                        break;
                    case ViewType.LargeProfilePhotoRight:
                        dataTemplate = _contentPresenter.Resources["LargeProfilePhotoRight"] as DataTemplate;
                        break;
                }
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
