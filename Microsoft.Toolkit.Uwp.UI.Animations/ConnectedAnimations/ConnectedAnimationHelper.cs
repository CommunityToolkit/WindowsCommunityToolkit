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

using System;
using System.Collections.Generic;
using System.Linq;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Animation;

namespace Microsoft.Toolkit.Uwp.UI.Animations
{
    /// <summary>
    /// Connected Animation Helper used with the <see cref="Connected"/> class
    /// Attaches to a <see cref="Frame"/> navigation events to handle connected animations
    /// <seealso cref="Connected"/>
    /// </summary>
    internal class ConnectedAnimationHelper
    {
        private readonly Dictionary<string, ConnectedAnimationProperties> _previousPageConnectedAnimationProps = new Dictionary<string, ConnectedAnimationProperties>();

        /// <summary>
        /// Initializes a new instance of the <see cref="ConnectedAnimationHelper"/> class.
        /// </summary>
        /// <param name="frame">The <see cref="Frame"/> hosting the content</param>
        public ConnectedAnimationHelper(Frame frame)
        {
            if (frame == null)
            {
                throw new ArgumentNullException(nameof(frame));
            }

            if (!ApiInformationHelper.IsCreatorsUpdateOrAbove)
            {
                return;
            }

            frame.Navigating += Frame_Navigating;
            frame.Navigated += Frame_Navigated;
        }

        private void Frame_Navigating(object sender, Windows.UI.Xaml.Navigation.NavigatingCancelEventArgs e)
        {
            var parameter = e.Parameter != null && !(e.Parameter is string str && string.IsNullOrEmpty(str)) ? e.Parameter : null;

            var cas = ConnectedAnimationService.GetForCurrentView();

            var page = (sender as Frame).Content as Page;
            var connectedAnimationsProps = Connected.GetPageConnectedAnimationProperties(page);

            foreach (var props in connectedAnimationsProps.Values)
            {
                if (props.IsListAnimation && parameter != null && ApiInformationHelper.IsCreatorsUpdateOrAbove)
                {
                    props.ListViewBase.PrepareConnectedAnimation(props.Key, e.Parameter, props.ElementName);
                }
                else if (!props.IsListAnimation)
                {
                    cas.PrepareToAnimate(props.Key, props.Element);
                }
                else
                {
                    continue;
                }

                _previousPageConnectedAnimationProps[props.Key] = props;
            }
        }

        private void Frame_Navigated(object sender, Windows.UI.Xaml.Navigation.NavigationEventArgs e)
        {
            var navigatedPage = (sender as Frame).Content as Page;

            if (navigatedPage == null)
            {
                return;
            }

            void loadedHandler(object s, RoutedEventArgs args)
            {
                var page = s as Page;
                page.Loaded -= loadedHandler;

                object parameter;
                if (e.NavigationMode == Windows.UI.Xaml.Navigation.NavigationMode.Back)
                {
                    var sourcePage = (sender as Frame).ForwardStack.LastOrDefault();
                    parameter = sourcePage?.Parameter;
                }
                else
                {
                    parameter = e.Parameter;
                }

                var cas = ConnectedAnimationService.GetForCurrentView();

                var connectedAnimationsProps = Connected.GetPageConnectedAnimationProperties(page);
                var coordinatedAnimationElements = Connected.GetPageCoordinatedAnimationElements(page);

                foreach (var props in connectedAnimationsProps.Values)
                {
                    var connectedAnimation = cas.GetAnimation(props.Key);
                    var animationHandled = false;
                    if (connectedAnimation != null)
                    {
                        if (props.IsListAnimation && parameter != null && ApiInformationHelper.IsCreatorsUpdateOrAbove)
                        {
                            props.ListViewBase.ScrollIntoView(parameter);

                            // give time to the UI thread to scroll the list
                            var t = props.ListViewBase.Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, async () =>
                            {
                                try
                                {
                                    var success = await props.ListViewBase.TryStartConnectedAnimationAsync(connectedAnimation, parameter, props.ElementName);
                                }
                                catch (Exception)
                                {
                                    connectedAnimation.Cancel();
                                }
                            });

                            animationHandled = true;
                        }
                        else if (!props.IsListAnimation)
                        {
                            if (ApiInformationHelper.IsCreatorsUpdateOrAbove && coordinatedAnimationElements.TryGetValue(props.Element, out var coordinatedElements))
                            {
                                connectedAnimation.TryStart(props.Element, coordinatedElements);
                            }
                            else
                            {
                                connectedAnimation.TryStart(props.Element);
                            }

                            animationHandled = true;
                        }
                    }

                    if (_previousPageConnectedAnimationProps.ContainsKey(props.Key) && animationHandled)
                    {
                        _previousPageConnectedAnimationProps.Remove(props.Key);
                    }
                }

                // if there are animations that were prepared on previous page but no elements on this page have the same key - cancel
                foreach (var previousProps in _previousPageConnectedAnimationProps)
                {
                    var connectedAnimation = cas.GetAnimation(previousProps.Key);
                    connectedAnimation?.Cancel();
                }

                _previousPageConnectedAnimationProps.Clear();
            }

            navigatedPage.Loaded += loadedHandler;
        }
    }
}
