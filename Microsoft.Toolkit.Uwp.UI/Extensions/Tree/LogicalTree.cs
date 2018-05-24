// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Markup;

namespace Microsoft.Toolkit.Uwp.UI.Extensions
{
    /// <summary>
    /// Defines a collection of extensions methods for UI.
    /// </summary>
    public static class LogicalTree
    {
        /// <summary>
        /// Find logical child control using its name.
        /// </summary>
        /// <param name="element">Parent element.</param>
        /// <param name="name">Name of the control to find.</param>
        /// <returns>Child control or null if not found.</returns>
        public static FrameworkElement FindChildByName(this FrameworkElement element, string name)
        {
            if (element == null || string.IsNullOrWhiteSpace(name))
            {
                return null;
            }

            if (name.Equals(element.Name, StringComparison.OrdinalIgnoreCase))
            {
                return element;
            }

            if (element is Panel)
            {
                foreach (var child in (element as Panel).Children)
                {
                    var result = (child as FrameworkElement)?.FindChildByName(name);
                    if (result != null)
                    {
                        return result;
                    }
                }
            }
            else if (element is ItemsControl)
            {
                foreach (var item in (element as ItemsControl).Items)
                {
                    var result = (item as FrameworkElement)?.FindChildByName(name);
                    if (result != null)
                    {
                        return result;
                    }
                }
            }
            else
            {
                var result = (element.GetContentControl() as FrameworkElement)?.FindChildByName(name);
                if (result != null)
                {
                    return result;
                }
            }

            return null;
        }

        /// <summary>
        /// Find first logical child control of a specified type.
        /// </summary>
        /// <typeparam name="T">Type to search for.</typeparam>
        /// <param name="element">Parent element.</param>
        /// <returns>Child control or null if not found.</returns>
        public static T FindChild<T>(this FrameworkElement element)
            where T : FrameworkElement
        {
            if (element == null)
            {
                return null;
            }

            if (element is Panel)
            {
                foreach (var child in (element as Panel).Children)
                {
                    if (child is T)
                    {
                        return child as T;
                    }

                    var result = (child as FrameworkElement)?.FindChild<T>();
                    if (result != null)
                    {
                        return result;
                    }
                }
            }
            else if (element is ItemsControl)
            {
                foreach (var item in (element as ItemsControl).Items)
                {
                    var result = (item as FrameworkElement)?.FindChild<T>();
                    if (result != null)
                    {
                        return result;
                    }
                }
            }
            else
            {
                var content = element.GetContentControl();

                if (content is T)
                {
                    return content as T;
                }

                var result = (content as FrameworkElement)?.FindChild<T>();
                if (result != null)
                {
                    return result;
                }
            }

            return null;
        }

        /// <summary>
        /// Find all logical child controls of the specified type.
        /// </summary>
        /// <typeparam name="T">Type to search for.</typeparam>
        /// <param name="element">Parent element.</param>
        /// <returns>Child controls or empty if not found.</returns>
        public static IEnumerable<T> FindChildren<T>(this FrameworkElement element)
            where T : FrameworkElement
        {
            if (element == null)
            {
                yield break;
            }

            if (element is Panel)
            {
                foreach (var child in (element as Panel).Children)
                {
                    if (child is T)
                    {
                        yield return child as T;
                    }

                    var childFrameworkElement = child as FrameworkElement;

                    if (childFrameworkElement != null)
                    {
                        foreach (T childOfChild in childFrameworkElement.FindChildren<T>())
                        {
                            yield return childOfChild;
                        }
                    }
                }
            }
            else if (element is ItemsControl)
            {
                foreach (var item in (element as ItemsControl).Items)
                {
                    var childFrameworkElement = item as FrameworkElement;

                    if (childFrameworkElement != null)
                    {
                        foreach (T childOfChild in childFrameworkElement.FindChildren<T>())
                        {
                            yield return childOfChild;
                        }
                    }
                }
            }
            else
            {
                var content = element.GetContentControl();

                if (content is T)
                {
                    yield return content as T;
                }

                var childFrameworkElement = content as FrameworkElement;

                if (childFrameworkElement != null)
                {
                    foreach (T childOfChild in childFrameworkElement.FindChildren<T>())
                    {
                        yield return childOfChild;
                    }
                }
            }
        }

        /// <summary>
        /// Finds the logical parent element with the given name or returns null.
        /// </summary>
        /// <param name="element">Child element.</param>
        /// <param name="name">Name of the control to find.</param>
        /// <returns>Parent control or null if not found.</returns>
        public static FrameworkElement FindParentByName(this FrameworkElement element, string name)
        {
            if (element == null || string.IsNullOrWhiteSpace(name))
            {
                return null;
            }

            if (name.Equals(element.Name, StringComparison.OrdinalIgnoreCase))
            {
                return element;
            }

            if (element.Parent == null)
            {
                return null;
            }

            return (element.Parent as FrameworkElement).FindParentByName(name);
        }

        /// <summary>
        /// Find first logical parent control of a specified type.
        /// </summary>
        /// <typeparam name="T">Type to search for.</typeparam>
        /// <param name="element">Child element.</param>
        /// <returns>Parent control or null if not found.</returns>
        public static T FindParent<T>(this FrameworkElement element)
            where T : FrameworkElement
        {
            if (element.Parent == null)
            {
                return null;
            }

            if (element.Parent is T)
            {
                return element.Parent as T;
            }

            return (element.Parent as FrameworkElement).FindParent<T>();
        }

        /// <summary>
        /// Retrieves the Content control of this element as defined by the ContentPropertyAttribute.
        /// </summary>
        /// <param name="element">Parent element.</param>
        /// <returns>Child Content control or null if not available.</returns>
        public static UIElement GetContentControl(this FrameworkElement element)
        {
            var contentpropname = ContentPropertySearch(element.GetType());
            if (contentpropname != null)
            {
                return element.GetType()?.GetProperty(contentpropname)?.GetValue(element) as UIElement;
            }

            return null;
        }

        /// <summary>
        /// Retrieves the Content Property's Name for the given type.
        /// </summary>
        /// <param name="type">UIElement based type to search for ContentProperty.</param>
        /// <returns>String name of ContentProperty for control.</returns>
        private static string ContentPropertySearch(Type type)
        {
            if (type == null)
            {
                return null;
            }

            // Using GetCustomAttribute directly isn't working for some reason, so we'll dig in ourselves: https://aka.ms/Rkzseg
            ////var attr = type.GetTypeInfo().GetCustomAttribute(typeof(ContentPropertyAttribute), true);
            var attr = type.GetTypeInfo().CustomAttributes.FirstOrDefault((element) => element.AttributeType == typeof(ContentPropertyAttribute));
            if (attr != null)
            {
                ////return attr as ContentPropertyAttribute;
                return attr.NamedArguments.First().TypedValue.Value as string;
            }

            return ContentPropertySearch(type.GetTypeInfo().BaseType);
        }
    }
}
