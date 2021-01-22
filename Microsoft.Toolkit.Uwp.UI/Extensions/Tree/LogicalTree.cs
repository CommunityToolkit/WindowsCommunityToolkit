// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Reflection;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Markup;

#nullable enable

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

        /// <summary>
        /// Find the first parent of type <see cref="FrameworkElement"/> with a given name.
        /// </summary>
        /// <param name="element">The starting element.</param>
        /// <param name="name">The name of the element to look for.</param>
        /// <param name="comparisonType">The comparison type to use to match <paramref name="name"/>.</param>
        /// <returns>The parent that was found, or <see langword="null"/>.</returns>
        public static FrameworkElement? FindParent(this FrameworkElement element, string name, StringComparison comparisonType = StringComparison.Ordinal)
        {
            return FindParent<FrameworkElement, (string Name, StringComparison ComparisonType)>(
                element,
                (name, comparisonType),
                static (e, s) => s.Name.Equals(e.Name, s.ComparisonType));
        }

        /// <summary>
        /// Find the first parent element of a given type.
        /// </summary>
        /// <typeparam name="T">The type of elements to match.</typeparam>
        /// <param name="element">The starting element.</param>
        /// <returns>The parent that was found, or <see langword="null"/>.</returns>
        public static T? FindParent<T>(this FrameworkElement element)
            where T : notnull, FrameworkElement
        {
            while (true)
            {
                if (element.Parent is not FrameworkElement parent)
                {
                    return null;
                }

                if (parent is T result)
                {
                    return result;
                }

                element = parent;
            }
        }

        /// <summary>
        /// Find the first parent element of a given type.
        /// </summary>
        /// <param name="element">The starting element.</param>
        /// <param name="type">The type of element to match.</param>
        /// <returns>The parent that was found, or <see langword="null"/>.</returns>
        public static FrameworkElement? FindParent(this FrameworkElement element, Type type)
        {
            return FindParent<FrameworkElement, Type>(element, type, static (e, t) => e.GetType() == t);
        }

        /// <summary>
        /// Find the first parent element matching a given predicate.
        /// </summary>
        /// <typeparam name="T">The type of elements to match.</typeparam>
        /// <param name="element">The starting element.</param>
        /// <param name="predicate">The predicatee to use to match the parent nodes.</param>
        /// <returns>The parent that was found, or <see langword="null"/>.</returns>
        public static T? FindParent<T>(this FrameworkElement element, Func<T, bool> predicate)
            where T : notnull, FrameworkElement
        {
            while (true)
            {
                if (element.Parent is not FrameworkElement parent)
                {
                    return null;
                }

                if (parent is T result && predicate(result))
                {
                    return result;
                }

                element = parent;
            }
        }

        /// <summary>
        /// Find the first parent element matching a given predicate.
        /// </summary>
        /// <typeparam name="T">The type of elements to match.</typeparam>
        /// <typeparam name="TState">The type of state to use when matching nodes.</typeparam>
        /// <param name="element">The starting element.</param>
        /// <param name="state">The state to give as input to <paramref name="predicate"/>.</param>
        /// <param name="predicate">The predicatee to use to match the parent nodes.</param>
        /// <returns>The parent that was found, or <see langword="null"/>.</returns>
        public static T? FindParent<T, TState>(this FrameworkElement element, TState state, Func<T, TState, bool> predicate)
            where T : notnull, FrameworkElement
        {
            while (true)
            {
                if (element.Parent is not FrameworkElement parent)
                {
                    return null;
                }

                if (parent is T result && predicate(result, state))
                {
                    return result;
                }

                element = parent;
            }
        }
    }
}
