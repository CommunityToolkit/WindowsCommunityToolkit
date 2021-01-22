// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
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
        /// Finds the logical parent element with the given name or returns null. Note: Parent may only be set when the control is added to the VisualTree.
        /// <seealso href="https://docs.microsoft.com/uwp/api/windows.ui.xaml.frameworkelement.parent#remarks"/>
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
        /// Find first logical parent control of a specified type. Note: Parent may only be set when the control is added to the VisualTree.
        /// <seealso href="https://docs.microsoft.com/uwp/api/windows.ui.xaml.frameworkelement.parent#remarks"/>
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
            Type type = element.GetType();

            if (type.GetCustomAttribute<ContentPropertyAttribute>(true) is ContentPropertyAttribute attribute &&
                type.GetProperty(attribute.Name) is PropertyInfo propertyInfo &&
                propertyInfo.GetValue(element) is UIElement content)
            {
                return content;
            }

            return null;
        }

        /// <summary>
        /// Provides a WPF compatible version of TryFindResource to provide a static resource lookup.
        /// If the key is not found in the current element's resources, the logical tree is then searched element-by-element to look for the resource in each element's resources.
        /// If none of the elements contain the resource, the Application's resources are then searched.
        /// <seealso href="https://docs.microsoft.com/en-us/dotnet/api/system.windows.frameworkelement.tryfindresource"/>
        /// <seealso href="https://docs.microsoft.com/en-us/dotnet/desktop-wpf/fundamentals/xaml-resources-define#static-resource-lookup-behavior"/>
        /// </summary>
        /// <param name="start"><see cref="FrameworkElement"/> to start searching for Resource.</param>
        /// <param name="resourceKey">Key to search for.</param>
        /// <returns>Requested resource or null.</returns>
        public static object TryFindResource(this FrameworkElement start, object resourceKey)
        {
            object value = null;
            var current = start;

            // Look in our dictionary and then walk-up parents
            while (current != null)
            {
                if (current.Resources?.TryGetValue(resourceKey, out value) == true)
                {
                    return value;
                }

                current = current.Parent as FrameworkElement;
            }

            // Finally try application resources.
            Application.Current?.Resources?.TryGetValue(resourceKey, out value);

            return value;
        }
    }
}
