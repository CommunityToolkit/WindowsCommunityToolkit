// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Linq;
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

        internal static Dictionary<string, ConnectedAnimationProperties> GetPageConnectedAnimationProperties(Page page)
        {
            var props = (Dictionary<string, ConnectedAnimationProperties>)page.GetValue(PageConnectedAnimationPropertiesProperty);

            if (props == null)
            {
                props = new Dictionary<string, ConnectedAnimationProperties>();
                page.SetValue(PageConnectedAnimationPropertiesProperty, props);
            }

            return props;
        }

        internal static void SetPageConnectedAnimationProperties(Page page, Dictionary<string, ConnectedAnimationProperties> value)
        {
            page.SetValue(PageConnectedAnimationPropertiesProperty, value);
        }

        internal static Dictionary<UIElement, List<UIElement>> GetPageCoordinatedAnimationElements(Page page)
        {
            var elements = (Dictionary<UIElement, List<UIElement>>)page.GetValue(PageCoordinatedAnimationElementsProperty);

            if (elements == null)
            {
                elements = new Dictionary<UIElement, List<UIElement>>();
                page.SetValue(PageCoordinatedAnimationElementsProperty, elements);
            }

            return elements;
        }

        internal static void SetPageCoordinatedAnimationElements(Page page, Dictionary<UIElement, List<UIElement>> value)
        {
            page.SetValue(PageCoordinatedAnimationElementsProperty, value);
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

        /// <summary>
        /// Identifies the Connected.PageConnectedAnimationProperties XAML attached property
        /// </summary>
        private static readonly DependencyProperty PageConnectedAnimationPropertiesProperty =
            DependencyProperty.RegisterAttached("PageConnectedAnimationProperties", typeof(Dictionary<string, ConnectedAnimationProperties>), typeof(Connected), new PropertyMetadata(null));

        /// <summary>
        /// Identifies the Connected.PageCoordinatedAnimationElements XAML attached property
        /// </summary>
        private static readonly DependencyProperty PageCoordinatedAnimationElementsProperty =
            DependencyProperty.RegisterAttached("PageCoordinatedAnimationElements", typeof(Dictionary<UIElement, List<UIElement>>), typeof(Connected), new PropertyMetadata(null));

        private static void RegisterKey(this Page page, string key, UIElement element)
        {
            if (key != null)
            {
                var animation = new ConnectedAnimationProperties()
                {
                    Key = key,
                    Element = element,
                };

                var props = GetPageConnectedAnimationProperties(page);
                props[key] = animation;
            }
        }

        private static void RemoveKey(this Page page, string key)
        {
            if (key != null)
            {
                var props = GetPageConnectedAnimationProperties(page);
                props.Remove(key);
            }
        }

        private static void AttachElementToAnimatingElement(this Page page, UIElement element, UIElement anchor)
        {
            if (anchor != null)
            {
                var coordinatedElements = GetPageCoordinatedAnimationElements(page);
                if (!coordinatedElements.TryGetValue(anchor, out var list))
                {
                    list = new List<UIElement>();
                    coordinatedElements[anchor] = list;
                }

                list.Add(element);
            }
        }

        private static void RemoveAnchoredElement(this Page page, UIElement element, UIElement anchor)
        {
            if (anchor != null)
            {
                var coordinatedElements = GetPageCoordinatedAnimationElements(page);
                if (coordinatedElements.TryGetValue(anchor, out var oldElementList))
                {
                    oldElementList.Remove(element);
                }
            }
        }

        private static void RegisterListItem(this Page page, Windows.UI.Xaml.Controls.ListViewBase listViewBase, string key, string elementName)
        {
            if (listViewBase == null || string.IsNullOrWhiteSpace(key) || string.IsNullOrWhiteSpace(elementName))
            {
                return;
            }

            var props = GetPageConnectedAnimationProperties(page);

            if (!props.TryGetValue(key, out var prop))
            {
                prop = new ConnectedAnimationProperties
                {
                    Key = key,
                    ListAnimProperties = new List<ConnectedAnimationListProperty>()
                };
                props[key] = prop;
            }

            if (!prop.ListAnimProperties.Any(lap => lap.ListViewBase == listViewBase && lap.ElementName == elementName))
            {
                prop.ListAnimProperties.Add(new ConnectedAnimationListProperty
                {
                    ElementName = elementName,
                    ListViewBase = listViewBase
                });
            }
        }

        private static void RemoveListItem(this Page page, Windows.UI.Xaml.Controls.ListViewBase listViewBase, string key)
        {
            if (listViewBase == null || string.IsNullOrWhiteSpace(key))
            {
                return;
            }

            var props = GetPageConnectedAnimationProperties(page);

            if (props.TryGetValue(key, out var prop))
            {
                if (!prop.IsListAnimation)
                {
                    props.Remove(key);
                }
                else
                {
                    var listAnimProperty = prop.ListAnimProperties.FirstOrDefault(lap => lap.ListViewBase == listViewBase);
                    if (listAnimProperty != null)
                    {
                        prop.ListAnimProperties.Remove(listAnimProperty);
                        if (prop.ListAnimProperties.Count == 0)
                        {
                            props.Remove(key);
                        }
                    }
                }
            }
        }

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
                GetConnectedAnimationHelper(frame);
                if (e.OldValue is string oldKey)
                {
                    (frame.Content as Page).RemoveKey(oldKey);
                }
                if (e.NewValue is string newKey)
                {
                    (frame.Content as Page).RegisterKey(newKey, element);
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
                GetConnectedAnimationHelper(frame);
                if (e.OldValue is UIElement oldAnchor)
                {
                    (frame.Content as Page).RemoveAnchoredElement(element, oldAnchor);
                }

                if (e.NewValue is UIElement newAnchor)
                {
                    (frame.Content as Page).AttachElementToAnimatingElement(element, newAnchor);
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
                GetConnectedAnimationHelper(frame);

                if (e.OldValue is string oldKey)
                {
                    (frame.Content as Page).RemoveListItem(element, oldKey);
                }

                AddListViewBaseItemAnimationDetails(frame.Content as Page, element);
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
                GetConnectedAnimationHelper(frame);

                if (e.OldValue is string oldElementName)
                {
                    var elementKey = GetListItemKey(element);
                    if (elementKey != null)
                    {
                        (frame.Content as Page).RemoveListItem(element, elementKey);
                    }
                }

                AddListViewBaseItemAnimationDetails(frame.Content as Page, element);
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

        private static void AddListViewBaseItemAnimationDetails(Page page, Windows.UI.Xaml.Controls.ListViewBase listViewBase)
        {
            if (ApiInformationHelper.IsCreatorsUpdateOrAbove && listViewBase != null)
            {
                var elementName = GetListItemElementName(listViewBase);
                var key = GetListItemKey(listViewBase);

                if (string.IsNullOrWhiteSpace(elementName) ||
                    string.IsNullOrWhiteSpace(key))
                {
                    return;
                }

                page.RegisterListItem(listViewBase, key, elementName);
            }
        }
    }
}
