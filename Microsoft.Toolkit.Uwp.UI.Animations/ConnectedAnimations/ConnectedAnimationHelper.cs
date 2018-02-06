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
        private readonly Dictionary<string, ConnectedAnimationProperties> _connectedAnimationsProps = new Dictionary<string, ConnectedAnimationProperties>();
        private readonly Dictionary<string, ConnectedAnimationProperties> _previousPageConnectedAnimationProps = new Dictionary<string, ConnectedAnimationProperties>();
        private readonly Dictionary<UIElement, List<UIElement>> _coordinatedAnimationElements = new Dictionary<UIElement, List<UIElement>>();

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

        /// <summary>
        /// Registers an element with the given key with the <see cref="ConnectedAnimationService"/>
        /// </summary>
        /// <param name="key">The key to be used with the <see cref="ConnectedAnimationService"/></param>
        /// <param name="element">The element to be animated</param>
        public void RegisterKey(string key, UIElement element)
        {
            if (key != null)
            {
                var animation = new ConnectedAnimationProperties()
                {
                    Key = key,
                    Element = element,
                };

                _connectedAnimationsProps[key] = animation;
            }
        }

        /// <summary>
        /// Unregisters a key from participating in the connected animation
        /// </summary>
        /// <param name="key">The connected animation key to unregister</param>
        public void RemoveKey(string key)
        {
            if (key != null)
            {
                _connectedAnimationsProps.Remove(key);
            }
        }

        /// <summary>
        /// Anchors an element to another element (anchor) that is registered to element
        /// </summary>
        /// <param name="element">The element that should animate together with the anchor</param>
        /// <param name="anchor">An element that is already registered with a key to animate</param>
        public void AttachElementToAnimatingElement(UIElement element, UIElement anchor)
        {
            if (anchor != null)
            {
                if (!_coordinatedAnimationElements.TryGetValue(anchor, out var list))
                {
                    list = new List<UIElement>();
                    _coordinatedAnimationElements[anchor] = list;
                }

                list.Add(element);
            }
        }

        /// <summary>
        /// Removes an element from animating alongside the anchor element
        /// </summary>
        /// <param name="element">The element that was previously anchored to the anchor element</param>
        /// <param name="anchor">An element that is registered with a key to animate</param>
        public void RemoveAnchoredElement(UIElement element, UIElement anchor)
        {
            if (anchor != null)
            {
                if (_coordinatedAnimationElements.TryGetValue(anchor, out var oldElementList))
                {
                    oldElementList.Remove(element);
                }
            }
        }

        /// <summary>
        /// Registers the ListViewBase element items to participate in a connected animation
        /// When page is navigated, the parameter used in the navigation is used to decide
        /// which item to animate
        /// </summary>
        /// <param name="listViewBase">The <see cref="ListViewBase"/></param>
        /// <param name="key">The connected animation key to register with the <see cref="ConnectedAnimationService"/></param>
        /// <param name="elementName">The name of the element in the <see cref="DataTemplate"/> to animate</param>
        public void RegisterListItem(ListViewBase listViewBase, string key, string elementName)
        {
            if (listViewBase == null || string.IsNullOrWhiteSpace(key) || string.IsNullOrWhiteSpace(elementName))
            {
                return;
            }

            var props = new ConnectedAnimationProperties()
            {
                Key = key,
                IsListAnimation = true,
                ElementName = elementName,
                ListViewBase = listViewBase
            };

            _connectedAnimationsProps[key] = props;
        }

        /// <summary>
        /// Unregisters the ListViewBase element items from participating in the connected animation
        /// <seealso cref="RegisterListItem(ListViewBase, string, string)"/>
        /// </summary>
        /// <param name="listViewBase">The <see cref="ListViewBase"/></param>
        /// <param name="key">The connected animation key to unregister</param>
        public void RemoveListItem(ListViewBase listViewBase, string key)
        {
            if (listViewBase == null || string.IsNullOrWhiteSpace(key))
            {
                return;
            }

            _connectedAnimationsProps.Remove(key);
        }

        private void Frame_Navigating(object sender, Windows.UI.Xaml.Navigation.NavigatingCancelEventArgs e)
        {
            var parameter = e.Parameter != null && !(e.Parameter is string str && string.IsNullOrEmpty(str)) ? e.Parameter : null;

            var cas = ConnectedAnimationService.GetForCurrentView();
            foreach (var props in _connectedAnimationsProps.Values)
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

            _connectedAnimationsProps.Clear();
            _coordinatedAnimationElements.Clear();
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

                foreach (var props in _connectedAnimationsProps.Values)
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
                            if (ApiInformationHelper.IsCreatorsUpdateOrAbove && _coordinatedAnimationElements.TryGetValue(props.Element, out var coordinatedElements))
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
