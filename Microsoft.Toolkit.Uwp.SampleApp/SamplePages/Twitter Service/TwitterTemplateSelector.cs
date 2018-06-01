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

using Microsoft.Toolkit.Uwp.Services.Twitter;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Microsoft.Toolkit.Uwp.SampleApp.SamplePages.Twitter_Service
{
    public class TwitterTemplateSelector : DataTemplateSelector
    {
        public DataTemplate TweetTemplate { get; set; }

        public DataTemplate DirectMessageTemplate { get; set; }

        public DataTemplate EventTemplate { get; set; }

        protected override DataTemplate SelectTemplateCore(object item, DependencyObject container)
        {
            var currentFrame = Window.Current.Content as Frame;

            if (currentFrame != null)
            {
                var currentPage = currentFrame.Content as Page;

                if (item != null && currentPage != null)
                {
                    if (item is Tweet)
                    {
                        return TweetTemplate;
                    }

                    if (item is TwitterDirectMessage)
                    {
                        return DirectMessageTemplate;
                    }

                    if (item is TwitterStreamEvent)
                    {
                        return EventTemplate;
                    }
                }
            }

            return base.SelectTemplateCore(item, container);
        }
    }
}
