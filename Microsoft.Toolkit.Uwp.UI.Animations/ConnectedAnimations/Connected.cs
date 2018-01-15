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
using Microsoft.Toolkit.Uwp.UI.Extensions;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Animation;

namespace Microsoft.Toolkit.Uwp.UI.Animations
{
    /// <summary>
    /// A helper class that allows Connected Animations to be enabled through XAML
    /// </summary>
    /// <seealso cref="ConnectedAnimationService"/>
    public static class Connected
    {
        /// <summary>
        /// Get the connected animation key associated with the <see cref="DependencyObject"/>
        /// </summary>
        /// <param name="obj">The <see cref="DependencyObject"/></param>
        /// <returns>the connected animation key</returns>
        public static string GetKey(DependencyObject obj)
        {
            return (string)obj.GetValue(KeyProperty);
        }

        /// <summary>
        /// Sets the connected animation key associated with the <see cref="DependencyObject"/>
        /// </summary>
        /// <param name="obj">The <see cref="DependencyObject"/></param>
        /// <param name="value">The key to set</param>
        public static void SetKey(DependencyObject obj, string value)
        {
            obj.SetValue(KeyProperty, value);
        }

        /// <summary>
        /// Gets the <see cref="UIElement"/> that is the anchor for the coordinated connected animation
        /// </summary>
        /// <param name="obj">The <see cref="UIElement"/></param>
        /// <returns>The anchor <see cref="UIElement"/></returns>
        public static UIElement GetAnchorElement(DependencyObject obj)
        {
            return (UIElement)obj.GetValue(AnchorElementProperty);
        }

        /// <summary>
        /// Sets the <see cref="UIElement"/> that is the anchor for the coordinated connected animation
        /// </summary>
        /// <param name="obj">The <see cref="UIElement"/> that will follow the anchor element</param>
        /// <param name="value">The <see cref="UIElement"/> that should be followed</param>
        public static void SetAnchorElement(DependencyObject obj, UIElement value)
        {
            obj.SetValue(AnchorElementProperty, value);
        }

        /// <summary>
        /// Gets the connected animation key associated with the ListViewBase item being animated
        /// </summary>
        /// <param name="obj">The <see cref="Windows.UI.Xaml.Controls.ListViewBase"/></param>
        /// <returns>The connected animation key</returns>
        public static string GetListItemKey(DependencyObject obj)
        {
            return (string)obj.GetValue(ListItemKeyProperty);
        }

        /// <summary>
        /// Sets the connected animation key for the <see cref="Windows.UI.Xaml.Controls.ListViewBase"/> item being animated
        /// </summary>
        /// <param name="obj">The <see cref="Windows.UI.Xaml.Controls.ListViewBase"/></param>
        /// <param name="value">The connected animation key</param>
        public static void SetListItemKey(DependencyObject obj, string value)
        {
            obj.SetValue(ListItemKeyProperty, value);
        }

        /// <summary>
        /// Gets the name of the element in the <see cref="DataTemplate"/> that is animated
        /// </summary>
        /// <param name="obj">The <see cref="Windows.UI.Xaml.Controls.ListViewBase"/></param>
        /// <returns>The name of the element being animated</returns>
        public static string GetListItemElementName(DependencyObject obj)
        {
            return (string)obj.GetValue(ListItemElementNameProperty);
        }

        /// <summary>
        /// Sets the name of the element in the <see cref="DataTemplate"/> that is animated
        /// </summary>
        /// <param name="obj">The <see cref="Windows.UI.Xaml.Controls.ListViewBase"/></param>
        /// <param name="value">The name of the element to animate</param>
        public static void SetListItemElementName(DependencyObject obj, string value)
        {
            obj.SetValue(ListItemElementNameProperty, value);
        }

        /// <summary>
        /// Gets the <see cref="ConnectedAnimationHelper"/> attached to a <see cref="Frame"/>
        /// Creates a new <see cref="ConnectedAnimationHelper"/> if one is not attached
        /// </summary>
        /// <param name="frame">The <see cref="Frame"/></param>
        /// <returns><see cref="ConnectedAnimationHelper"/> attached to the Frame</returns>
        private static ConnectedAnimationHelper GetConnectedAnimationHelper(Frame frame)
        {
            var helper = (ConnectedAnimationHelper)frame.GetValue(ConnectedAnimationHelperProperty);

            if (helper == null)
            {
                helper = new ConnectedAnimationHelper(frame);
                frame.SetValue(ConnectedAnimationHelperProperty, helper);
            }

            return helper;
        }

        /// <summary>
        /// Sets the <see cref="ConnectedAnimationHelper"/> to a <see cref="Frame"/>
        /// </summary>
        /// <param name="frame">The Frame to attach the <see cref="ConnectedAnimationHelper"/></param>
        /// <param name="value"><see cref="ConnectedAnimationHelper"/> to attach</param>
        private static void SetConnectedAnimationHelper(Frame frame, ConnectedAnimationHelper value)
        {
            frame.SetValue(ConnectedAnimationHelperProperty, value);
        }

        /// <summary>
        /// Identifies the Connected.Key XAML attached property
        /// </summary>
        public static readonly DependencyProperty KeyProperty =
            DependencyProperty.RegisterAttached("Key", typeof(string), typeof(Connected), new PropertyMetadata(null, OnKeyChanged));

        /// <summary>
        /// Identifies the Connected.AnchorElement XAML attached property
        /// </summary>
        public static readonly DependencyProperty AnchorElementProperty =
            DependencyProperty.RegisterAttached("AnchorElement", typeof(UIElement), typeof(Connected), new PropertyMetadata(null, OnAnchorElementChanged));

        /// <summary>
        /// Identifies the Connected.ListItemKey XAML attached property
        /// </summary>
        public static readonly DependencyProperty ListItemKeyProperty =
            DependencyProperty.RegisterAttached("ListItemKey", typeof(string), typeof(Connected), new PropertyMetadata(null, OnListItemKeyChanged));

        /// <summary>
        /// Identifies the Connected.ListItemElementName XAML attached property
        /// </summary>
        public static readonly DependencyProperty ListItemElementNameProperty =
            DependencyProperty.RegisterAttached("ListItemElementName", typeof(string), typeof(Connected), new PropertyMetadata(null, OnListItemElementNameChanged));

        /// <summary>
        /// Identifies the Connected.ConnectedAnimationHelper XAML attached property
        /// </summary>
        private static readonly DependencyProperty ConnectedAnimationHelperProperty =
            DependencyProperty.RegisterAttached("ConnectedAnimationHelper", typeof(ConnectedAnimationHelper), typeof(Connected), new PropertyMetadata(null));

        private static void OnKeyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (!ApiInformationHelper.IsCreatorsUpdateOrAbove)
            {
                return;
            }

            var element = d as FrameworkElement;
            if (element == null)
            {
                return;
            }

            GetParentFrameAndExecuteAction(element, (frame) =>
            {
                var helper = GetConnectedAnimationHelper(frame);
                if (e.OldValue is string oldKey)
                {
                    helper?.RemoveKey(oldKey);
                }
                if (e.NewValue is string newKey)
                {
                    helper?.RegisterKey(newKey, element);
                }
            });
        }

        private static void OnAnchorElementChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (!ApiInformationHelper.IsCreatorsUpdateOrAbove)
            {
                return;
            }

            var element = d as FrameworkElement;
            if (element == null)
            {
                return;
            }

            GetParentFrameAndExecuteAction(element, (frame) =>
            {
                var helper = GetConnectedAnimationHelper(frame);
                if (e.OldValue is UIElement oldAnchor)
                {
                    helper?.RemoveAnchoredElement(element, oldAnchor);
                }

                if (e.NewValue is UIElement newAnchor)
                {
                    helper?.AttachElementToAnimatingElement(element, newAnchor);
                }
            });
        }

        private static void OnListItemKeyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (!ApiInformationHelper.IsCreatorsUpdateOrAbove)
            {
                return;
            }

            var element = d as Windows.UI.Xaml.Controls.ListViewBase;
            if (element == null)
            {
                return;
            }

            GetParentFrameAndExecuteAction(element, (frame) =>
            {
                var helper = GetConnectedAnimationHelper(frame);

                if (e.OldValue is string oldKey)
                {
                    helper?.RemoveListItem(element, oldKey);
                }

                AddListViewBaseItemAnimationDetails(helper, element);
            });
        }

        private static void OnListItemElementNameChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (!ApiInformationHelper.IsCreatorsUpdateOrAbove)
            {
                return;
            }

            var element = d as Windows.UI.Xaml.Controls.ListViewBase;
            if (element == null)
            {
                return;
            }

            GetParentFrameAndExecuteAction(element, (frame) =>
            {
                var helper = GetConnectedAnimationHelper(frame);

                if (e.OldValue is string oldElementName)
                {
                    var elementKey = GetListItemKey(element);
                    if (elementKey != null)
                    {
                        helper?.RemoveListItem(element, elementKey);
                    }
                }

                AddListViewBaseItemAnimationDetails(helper, element);
            });
        }

        private static void GetParentFrameAndExecuteAction(FrameworkElement element, Action<Frame> action)
        {
            // get parent Frame
            var frame = element.FindAscendant<Frame>();

            if (frame == null)
            {
                RoutedEventHandler handler = null;
                handler = (s, args) =>
                {
                    element.Loaded -= handler;
                    frame = element.FindAscendant<Frame>();
                    if (frame != null)
                    {
                        action(frame);
                    }
                };

                element.Loaded += handler;
            }
            else
            {
                action(frame);
            }
        }

        private static void AddListViewBaseItemAnimationDetails(ConnectedAnimationHelper helper, Windows.UI.Xaml.Controls.ListViewBase listViewBase)
        {
            if (ApiInformationHelper.IsCreatorsUpdateOrAbove && listViewBase != null && helper != null)
            {
                var elementName = GetListItemElementName(listViewBase);
                var key = GetListItemKey(listViewBase);

                if (string.IsNullOrWhiteSpace(elementName) ||
                    string.IsNullOrWhiteSpace(key))
                {
                    return;
                }

                helper.RegisterListItem(listViewBase, key, elementName);
            }
        }
    }
}
