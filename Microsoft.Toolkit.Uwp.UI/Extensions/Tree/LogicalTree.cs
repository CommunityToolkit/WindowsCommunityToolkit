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
        /// Find the first child of type <see cref="FrameworkElement"/> with a given name, using a depth-first search.
        /// </summary>
        /// <param name="element">The root element.</param>
        /// <param name="name">The name of the element to look for.</param>
        /// <param name="comparisonType">The comparison type to use to match <paramref name="name"/>.</param>
        /// <returns>The child that was found, or <see langword="null"/>.</returns>
        public static FrameworkElement? FindChild(this FrameworkElement element, string name, StringComparison comparisonType = StringComparison.Ordinal)
        {
            return FindChild<FrameworkElement, (string Name, StringComparison ComparisonType)>(
                element,
                (name, comparisonType),
                static (e, s) => s.Name.Equals(e.Name, s.ComparisonType));
        }

        /// <summary>
        /// Find the first child element of a given type, using a depth-first search.
        /// </summary>
        /// <typeparam name="T">The type of elements to match.</typeparam>
        /// <param name="element">The root element.</param>
        /// <returns>The child that was found, or <see langword="null"/>.</returns>
        public static T? FindChild<T>(this FrameworkElement element)
            where T : notnull, FrameworkElement
        {
            return FindChild<T>(element, static _ => true);
        }

        /// <summary>
        /// Find the first child element of a given type, using a depth-first search.
        /// </summary>
        /// <param name="element">The root element.</param>
        /// <param name="type">The type of element to match.</param>
        /// <returns>The child that was found, or <see langword="null"/>.</returns>
        public static FrameworkElement? FindChild(this FrameworkElement element, Type type)
        {
            return FindChild<FrameworkElement, Type>(element, type, static (e, t) => e.GetType() == t);
        }

        /// <summary>
        /// Find the first child element matching a given predicate, using a depth-first search.
        /// </summary>
        /// <typeparam name="T">The type of elements to match.</typeparam>
        /// <param name="element">The root element.</param>
        /// <param name="predicate">The predicatee to use to match the child nodes.</param>
        /// <returns>The child that was found, or <see langword="null"/>.</returns>
        public static T? FindChild<T>(this FrameworkElement element, Func<T, bool> predicate)
            where T : notnull, FrameworkElement
        {
            if (element is Panel panel)
            {
                foreach (UIElement child in panel.Children)
                {
                    if (child is not FrameworkElement current)
                    {
                        continue;
                    }

                    if (child is T result && predicate(result))
                    {
                        return result;
                    }

                    T? descendant = FindChild(current, predicate);

                    if (descendant is not null)
                    {
                        return descendant;
                    }
                }
            }
            else if (element is ItemsControl itemsControl)
            {
                foreach (object item in itemsControl.Items)
                {
                    if (item is not FrameworkElement current)
                    {
                        continue;
                    }

                    if (item is T result && predicate(result))
                    {
                        return result;
                    }

                    T? descendant = FindChild(current, predicate);

                    if (descendant is not null)
                    {
                        return descendant;
                    }
                }
            }
            else if (element.TryGetContentControl() is FrameworkElement contentControl)
            {
                if (contentControl is T result && predicate(result))
                {
                    return result;
                }

                return FindChild(contentControl, predicate);
            }

            return null;
        }

        /// <summary>
        /// Find the first child element matching a given predicate, using a depth-first search.
        /// </summary>
        /// <typeparam name="T">The type of elements to match.</typeparam>
        /// <typeparam name="TState">The type of state to use when matching nodes.</typeparam>
        /// <param name="element">The root element.</param>
        /// <param name="state">The state to give as input to <paramref name="predicate"/>.</param>
        /// <param name="predicate">The predicatee to use to match the child nodes.</param>
        /// <returns>The child that was found, or <see langword="null"/>.</returns>
        public static T? FindChild<T, TState>(this FrameworkElement element, TState state, Func<T, TState, bool> predicate)
            where T : notnull, FrameworkElement
        {
            if (element is Panel panel)
            {
                foreach (UIElement child in panel.Children)
                {
                    if (child is not FrameworkElement current)
                    {
                        continue;
                    }

                    if (child is T result && predicate(result, state))
                    {
                        return result;
                    }

                    T? descendant = FindChild(current, state, predicate);

                    if (descendant is not null)
                    {
                        return descendant;
                    }
                }
            }
            else if (element is ItemsControl itemsControl)
            {
                foreach (object item in itemsControl.Items)
                {
                    if (item is not FrameworkElement current)
                    {
                        continue;
                    }

                    if (item is T result && predicate(result, state))
                    {
                        return result;
                    }

                    T? descendant = FindChild(current, state, predicate);

                    if (descendant is not null)
                    {
                        return descendant;
                    }
                }
            }
            else if (element.TryGetContentControl() is FrameworkElement contentControl)
            {
                if (contentControl is T result && predicate(result, state))
                {
                    return result;
                }

                return FindChild(contentControl, state, predicate);
            }

            return null;
        }

        /// <summary>
        /// Find the first child (or self) of type <see cref="FrameworkElement"/> with a given name, using a depth-first search.
        /// </summary>
        /// <param name="element">The root element.</param>
        /// <param name="name">The name of the element to look for.</param>
        /// <param name="comparisonType">The comparison type to use to match <paramref name="name"/>.</param>
        /// <returns>The child (or self) that was found, or <see langword="null"/>.</returns>
        public static FrameworkElement? FindChildOrSelf(this FrameworkElement element, string name, StringComparison comparisonType = StringComparison.Ordinal)
        {
            if (name.Equals(element.Name, comparisonType))
            {
                return element;
            }

            return FindChild(element, name, comparisonType);
        }

        /// <summary>
        /// Find the first child (or self) element of a given type, using a depth-first search.
        /// </summary>
        /// <typeparam name="T">The type of elements to match.</typeparam>
        /// <param name="element">The root element.</param>
        /// <returns>The child (or self) that was found, or <see langword="null"/>.</returns>
        public static T? FindChildOrSelf<T>(this FrameworkElement element)
            where T : notnull, FrameworkElement
        {
            if (element is T result)
            {
                return result;
            }

            return FindChild<T>(element);
        }

        /// <summary>
        /// Find the first child (or self) element of a given type, using a depth-first search.
        /// </summary>
        /// <param name="element">The root element.</param>
        /// <param name="type">The type of element to match.</param>
        /// <returns>The child (or self) that was found, or <see langword="null"/>.</returns>
        public static FrameworkElement? FindChildOrSelf(this FrameworkElement element, Type type)
        {
            if (element.GetType() == type)
            {
                return element;
            }

            return FindChild(element, type);
        }

        /// <summary>
        /// Find the first child (or self) element matching a given predicate, using a depth-first search.
        /// </summary>
        /// <typeparam name="T">The type of elements to match.</typeparam>
        /// <param name="element">The root element.</param>
        /// <param name="predicate">The predicatee to use to match the child nodes.</param>
        /// <returns>The child (or self) that was found, or <see langword="null"/>.</returns>
        public static T? FindChildOrSelf<T>(this FrameworkElement element, Func<T, bool> predicate)
            where T : notnull, FrameworkElement
        {
            if (element is T result && predicate(result))
            {
                return result;
            }

            return FindChild(element, predicate);
        }

        /// <summary>
        /// Find the first child (or self) element matching a given predicate, using a depth-first search.
        /// </summary>
        /// <typeparam name="T">The type of elements to match.</typeparam>
        /// <typeparam name="TState">The type of state to use when matching nodes.</typeparam>
        /// <param name="element">The root element.</param>
        /// <param name="state">The state to give as input to <paramref name="predicate"/>.</param>
        /// <param name="predicate">The predicatee to use to match the child nodes.</param>
        /// <returns>The child (or self) that was found, or <see langword="null"/>.</returns>
        public static T? FindChildOrSelf<T, TState>(this FrameworkElement element, TState state, Func<T, TState, bool> predicate)
            where T : notnull, FrameworkElement
        {
            if (element is T result && predicate(result, state))
            {
                return result;
            }

            return FindChild(element, state, predicate);
        }

        /// <summary>
        /// Find all logical child elements of the specified element. This method can be chained with
        /// LINQ calls to add additional filters or projections on top of the returned results.
        /// <para>
        /// This method is meant to provide extra flexibility in specific scenarios and it should not
        /// be used when only the first item is being looked for. In those cases, use one of the
        /// available <see cref="FindChild{T}(FrameworkElement)"/> overloads instead, which will
        /// offer a more compact syntax as well as better performance in those cases.
        /// </para>
        /// </summary>
        /// <param name="element">The root element.</param>
        /// <returns>All the child <see cref="FrameworkElement"/> instance from <paramref name="element"/>.</returns>
        public static IEnumerable<FrameworkElement> FindChildren(this FrameworkElement element)
        {
            if (element is Panel panel)
            {
                foreach (UIElement child in panel.Children)
                {
                    if (child is not FrameworkElement current)
                    {
                        continue;
                    }

                    yield return current;

                    foreach (FrameworkElement childOfChild in FindChildren(current))
                    {
                        yield return childOfChild;
                    }
                }
            }
            else if (element is ItemsControl itemsControl)
            {
                foreach (object item in itemsControl.Items)
                {
                    if (item is not FrameworkElement current)
                    {
                        continue;
                    }

                    yield return current;

                    foreach (FrameworkElement childOfChild in FindChildren(current))
                    {
                        yield return childOfChild;
                    }
                }
            }
            else if (element.TryGetContentControl() is FrameworkElement contentControl)
            {
                yield return contentControl;

                foreach (FrameworkElement childOfChild in FindChildren(contentControl))
                {
                    yield return childOfChild;
                }
            }
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

        /// <summary>
        /// Find the first parent (or self) of type <see cref="FrameworkElement"/> with a given name.
        /// </summary>
        /// <param name="element">The starting element.</param>
        /// <param name="name">The name of the element to look for.</param>
        /// <param name="comparisonType">The comparison type to use to match <paramref name="name"/>.</param>
        /// <returns>The parent (or self) that was found, or <see langword="null"/>.</returns>
        public static FrameworkElement? FindParentOrSelf(this FrameworkElement element, string name, StringComparison comparisonType = StringComparison.Ordinal)
        {
            if (name.Equals(element.Name, comparisonType))
            {
                return element;
            }

            return FindParent(element, name, comparisonType);
        }

        /// <summary>
        /// Find the first parent (or self) element of a given type.
        /// </summary>
        /// <typeparam name="T">The type of elements to match.</typeparam>
        /// <param name="element">The starting element.</param>
        /// <returns>The parent (or self) that was found, or <see langword="null"/>.</returns>
        public static T? FindParentOrSelf<T>(this FrameworkElement element)
            where T : notnull, FrameworkElement
        {
            if (element is T result)
            {
                return result;
            }

            return FindParent<T>(element);
        }

        /// <summary>
        /// Find the first parent (or self) element of a given type.
        /// </summary>
        /// <param name="element">The starting element.</param>
        /// <param name="type">The type of element to match.</param>
        /// <returns>The parent (or self) that was found, or <see langword="null"/>.</returns>
        public static FrameworkElement? FindParentOrSelf(this FrameworkElement element, Type type)
        {
            if (element.GetType() == type)
            {
                return element;
            }

            return FindParent(element, type);
        }

        /// <summary>
        /// Find the first parent (or self) element matching a given predicate.
        /// </summary>
        /// <typeparam name="T">The type of elements to match.</typeparam>
        /// <param name="element">The starting element.</param>
        /// <param name="predicate">The predicatee to use to match the parent nodes.</param>
        /// <returns>The parent (or self) that was found, or <see langword="null"/>.</returns>
        public static T? FindParentOrSelf<T>(this FrameworkElement element, Func<T, bool> predicate)
            where T : notnull, FrameworkElement
        {
            if (element is T result && predicate(result))
            {
                return result;
            }

            return FindParent(element, predicate);
        }

        /// <summary>
        /// Find the first parent (or self) element matching a given predicate.
        /// </summary>
        /// <typeparam name="T">The type of elements to match.</typeparam>
        /// <typeparam name="TState">The type of state to use when matching nodes.</typeparam>
        /// <param name="element">The starting element.</param>
        /// <param name="state">The state to give as input to <paramref name="predicate"/>.</param>
        /// <param name="predicate">The predicatee to use to match the parent nodes.</param>
        /// <returns>The parent (or self) that was found, or <see langword="null"/>.</returns>
        public static T? FindParentOrSelf<T, TState>(this FrameworkElement element, TState state, Func<T, TState, bool> predicate)
            where T : notnull, FrameworkElement
        {
            if (element is T result && predicate(result, state))
            {
                return result;
            }

            return FindParent(element, state, predicate);
        }

        /// <summary>
        /// Find all parent elements of the specified element. This method can be chained with
        /// LINQ calls to add additional filters or projections on top of the returned results.
        /// <para>
        /// This method is meant to provide extra flexibility in specific scenarios and it should not
        /// be used when only the first item is being looked for. In those cases, use one of the
        /// available <see cref="FindParent{T}(FrameworkElement)"/> overloads instead, which will
        /// offer a more compact syntax as well as better performance in those cases.
        /// </para>
        /// </summary>
        /// <param name="element">The root element.</param>
        /// <returns>All the parent <see cref="FrameworkElement"/> instance from <paramref name="element"/>.</returns>
        public static IEnumerable<FrameworkElement> FindParents(this FrameworkElement element)
        {
            while (true)
            {
                if (element.Parent is not FrameworkElement parent)
                {
                    yield break;
                }

                yield return parent;

                element = parent;
            }
        }

        /// <summary>
        /// Tries to retrieve the content property of this element as defined by <see cref="ContentPropertyAttribute"/>.
        /// </summary>
        /// <param name="element">The parent element.</param>
        /// <returns>The retrieved content control, or <see langword="null"/> if not available.</returns>
        public static UIElement? TryGetContentControl(this FrameworkElement element)
        {
            Type type = element.GetType();
            TypeInfo typeInfo = type.GetTypeInfo();

            while (typeInfo is not null)
            {
                // We need to manually explore the custom attributes this way as the target one
                // is not returned by any of the other available GetCustomAttribute<T> APIs.
                foreach (CustomAttributeData attribute in typeInfo.CustomAttributes)
                {
                    if (attribute.AttributeType == typeof(ContentPropertyAttribute))
                    {
                        string propertyName = (string)attribute.NamedArguments[0].TypedValue.Value;
                        PropertyInfo? propertyInfo = type.GetProperty(propertyName);

                        return propertyInfo?.GetValue(element) as UIElement;
                    }
                }

                typeInfo = typeInfo.BaseType.GetTypeInfo();
            }

            return null;
        }

        /// <summary>
        /// Provides a WPF compatible version of TryFindResource to provide a static resource lookup.
        /// If the key is not found in the current element's resources, the logical tree is then
        /// searched element-by-element to look for the resource in each element's resources.
        /// If none of the elements contain the resource, the Application's resources are then searched.
        /// <para>See: <seealso href="https://docs.microsoft.com/dotnet/api/system.windows.frameworkelement.tryfindresource"/></para>
        /// <para>And also: <seealso href="https://docs.microsoft.com/dotnet/desktop-wpf/fundamentals/xaml-resources-define#static-resource-lookup-behavior"/></para>
        /// </summary>
        /// <param name="element">The <see cref="FrameworkElement"/> to start searching for the target resource.</param>
        /// <param name="resourceKey">The resource key to search for.</param>
        /// <returns>The requested resource, or <see langword="null"/>.</returns>
        public static object? TryFindResource(this FrameworkElement element, object resourceKey)
        {
            object? value = null;
            FrameworkElement? current = element;

            // Look in our dictionary and then walk-up parents. We use a do-while loop here
            // so that an implicit NRE will be thrown at the first iteration in case the
            // input element is null. This is consistent with the other extensions.
            do
            {
                if (current.Resources?.TryGetValue(resourceKey, out value) == true)
                {
                    return value!;
                }

                current = current.Parent as FrameworkElement;
            }
            while (current is not null);

            // Finally try application resources
            Application.Current?.Resources?.TryGetValue(resourceKey, out value);

            return value;
        }
    }
}
