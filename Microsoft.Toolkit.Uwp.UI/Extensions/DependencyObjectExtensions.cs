// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using Microsoft.Toolkit.Uwp.UI.Helpers.Internals;
using Microsoft.Toolkit.Uwp.UI.Predicates;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media;

#nullable enable

namespace Microsoft.Toolkit.Uwp.UI
{
    /// <summary>
    /// Provides attached dependency properties for the <see cref="DependencyObject"/> type.
    /// </summary>
    public static class DependencyObjectExtensions
    {
        /// <summary>
        /// Find the first descendant of type <see cref="FrameworkElement"/> with a given name, using a depth-first search.
        /// </summary>
        /// <param name="element">The root element.</param>
        /// <param name="name">The name of the element to look for.</param>
        /// <param name="comparisonType">The comparison type to use to match <paramref name="name"/>.</param>
        /// <returns>The descendant that was found, or <see langword="null"/>.</returns>
        public static FrameworkElement? FindDescendant(this DependencyObject element, string name, StringComparison comparisonType = StringComparison.Ordinal)
        {
            PredicateByName predicateByName = new(name, comparisonType);

            return FindDescendant<FrameworkElement, PredicateByName>(element, ref predicateByName, SearchType.DepthFirst);
        }

        /// <summary>
        /// Find the first descendant of type <see cref="FrameworkElement"/> with a given name.
        /// </summary>
        /// <param name="element">The root element.</param>
        /// <param name="name">The name of the element to look for.</param>
        /// <param name="comparisonType">The comparison type to use to match <paramref name="name"/>.</param>
        /// <param name="searchType">The search type to use to explore the visual tree.</param>
        /// <returns>The descendant that was found, or <see langword="null"/>.</returns>
        public static FrameworkElement? FindDescendant(this DependencyObject element, string name, StringComparison comparisonType, SearchType searchType)
        {
            PredicateByName predicateByName = new(name, comparisonType);

            return FindDescendant<FrameworkElement, PredicateByName>(element, ref predicateByName, searchType);
        }

        /// <summary>
        /// Find the first descendant element of a given type, using a depth-first search.
        /// </summary>
        /// <typeparam name="T">The type of elements to match.</typeparam>
        /// <param name="element">The root element.</param>
        /// <returns>The descendant that was found, or <see langword="null"/>.</returns>
        public static T? FindDescendant<T>(this DependencyObject element)
            where T : notnull, DependencyObject
        {
            PredicateByAny<T> predicateByAny = default;

            return FindDescendant<T, PredicateByAny<T>>(element, ref predicateByAny, SearchType.DepthFirst);
        }

        /// <summary>
        /// Find the first descendant element of a given type.
        /// </summary>
        /// <typeparam name="T">The type of elements to match.</typeparam>
        /// <param name="element">The root element.</param>
        /// <param name="searchType">The search type to use to explore the visual tree.</param>
        /// <returns>The descendant that was found, or <see langword="null"/>.</returns>
        public static T? FindDescendant<T>(this DependencyObject element, SearchType searchType)
            where T : notnull, DependencyObject
        {
            PredicateByAny<T> predicateByAny = default;

            return FindDescendant<T, PredicateByAny<T>>(element, ref predicateByAny, searchType);
        }

        /// <summary>
        /// Find the first descendant element of a given type, using a depth-first search.
        /// </summary>
        /// <param name="element">The root element.</param>
        /// <param name="type">The type of element to match.</param>
        /// <returns>The descendant that was found, or <see langword="null"/>.</returns>
        public static DependencyObject? FindDescendant(this DependencyObject element, Type type)
        {
            PredicateByType predicateByType = new(type);

            return FindDescendant<DependencyObject, PredicateByType>(element, ref predicateByType, SearchType.DepthFirst);
        }

        /// <summary>
        /// Find the first descendant element of a given type.
        /// </summary>
        /// <param name="element">The root element.</param>
        /// <param name="type">The type of element to match.</param>
        /// <param name="searchType">The search type to use to explore the visual tree.</param>
        /// <returns>The descendant that was found, or <see langword="null"/>.</returns>
        public static DependencyObject? FindDescendant(this DependencyObject element, Type type, SearchType searchType)
        {
            PredicateByType predicateByType = new(type);

            return FindDescendant<DependencyObject, PredicateByType>(element, ref predicateByType, searchType);
        }

        /// <summary>
        /// Find the first descendant element matching a given predicate, using a depth-first search.
        /// </summary>
        /// <typeparam name="T">The type of elements to match.</typeparam>
        /// <param name="element">The root element.</param>
        /// <param name="predicate">The predicatee to use to match the descendant nodes.</param>
        /// <returns>The descendant that was found, or <see langword="null"/>.</returns>
        public static T? FindDescendant<T>(this DependencyObject element, Func<T, bool> predicate)
            where T : notnull, DependencyObject
        {
            PredicateByFunc<T> predicateByFunc = new(predicate);

            return FindDescendant<T, PredicateByFunc<T>>(element, ref predicateByFunc, SearchType.DepthFirst);
        }

        /// <summary>
        /// Find the first descendant element matching a given predicate.
        /// </summary>
        /// <typeparam name="T">The type of elements to match.</typeparam>
        /// <param name="element">The root element.</param>
        /// <param name="predicate">The predicatee to use to match the descendant nodes.</param>
        /// <param name="searchType">The search type to use to explore the visual tree.</param>
        /// <returns>The descendant that was found, or <see langword="null"/>.</returns>
        public static T? FindDescendant<T>(this DependencyObject element, Func<T, bool> predicate, SearchType searchType)
            where T : notnull, DependencyObject
        {
            PredicateByFunc<T> predicateByFunc = new(predicate);

            return FindDescendant<T, PredicateByFunc<T>>(element, ref predicateByFunc, searchType);
        }

        /// <summary>
        /// Find the first descendant element matching a given predicate, using a depth-first search.
        /// </summary>
        /// <typeparam name="T">The type of elements to match.</typeparam>
        /// <typeparam name="TState">The type of state to use when matching nodes.</typeparam>
        /// <param name="element">The root element.</param>
        /// <param name="state">The state to give as input to <paramref name="predicate"/>.</param>
        /// <param name="predicate">The predicatee to use to match the descendant nodes.</param>
        /// <returns>The descendant that was found, or <see langword="null"/>.</returns>
        public static T? FindDescendant<T, TState>(this DependencyObject element, TState state, Func<T, TState, bool> predicate)
            where T : notnull, DependencyObject
        {
            PredicateByFunc<T, TState> predicateByFunc = new(state, predicate);

            return FindDescendant<T, PredicateByFunc<T, TState>>(element, ref predicateByFunc, SearchType.DepthFirst);
        }

        /// <summary>
        /// Find the first descendant element matching a given predicate.
        /// </summary>
        /// <typeparam name="T">The type of elements to match.</typeparam>
        /// <typeparam name="TState">The type of state to use when matching nodes.</typeparam>
        /// <param name="element">The root element.</param>
        /// <param name="state">The state to give as input to <paramref name="predicate"/>.</param>
        /// <param name="predicate">The predicatee to use to match the descendant nodes.</param>
        /// <param name="searchType">The search type to use to explore the visual tree.</param>
        /// <returns>The descendant that was found, or <see langword="null"/>.</returns>
        public static T? FindDescendant<T, TState>(this DependencyObject element, TState state, Func<T, TState, bool> predicate, SearchType searchType)
            where T : notnull, DependencyObject
        {
            PredicateByFunc<T, TState> predicateByFunc = new(state, predicate);

            return FindDescendant<T, PredicateByFunc<T, TState>>(element, ref predicateByFunc, searchType);
        }

        /// <summary>
        /// Find the first descendant element matching a given predicate.
        /// </summary>
        /// <typeparam name="T">The type of elements to match.</typeparam>
        /// <typeparam name="TPredicate">The type of predicate in use.</typeparam>
        /// <param name="element">The root element.</param>
        /// <param name="predicate">The predicate to use to match the descendant nodes.</param>
        /// <param name="searchType">The search type to use to explore the visual tree.</param>
        /// <returns>The descendant that was found, or <see langword="null"/>.</returns>
        private static T? FindDescendant<T, TPredicate>(this DependencyObject element, ref TPredicate predicate, SearchType searchType)
            where T : notnull, DependencyObject
            where TPredicate : struct, IPredicate<T>
        {
            // Depth-first search, with recursive implementation
            static T? FindDescendantWithDepthFirstSearch(DependencyObject element, ref TPredicate predicate)
            {
                int childrenCount = VisualTreeHelper.GetChildrenCount(element);

                for (int i = 0; i < childrenCount; i++)
                {
                    DependencyObject child = VisualTreeHelper.GetChild(element, i);

                    if (child is T result && predicate.Match(result))
                    {
                        return result;
                    }

                    T? descendant = FindDescendantWithDepthFirstSearch(child, ref predicate);

                    if (descendant is not null)
                    {
                        return descendant;
                    }
                }

                return null;
            }

            // Breadth-first search, with iterative implementation and pooled local stack
            static T? FindDescendantWithBreadthFirstSearch(DependencyObject element, ref TPredicate predicate)
            {
                // We're using a pooled buffer writer to amortize allocations for the temporary collection of children
                // to visit for each level. The underlying array is deliberately just of type object and not DependencyObject
                // to reduce the number of generic instantiations and allow the rented arrays to be reused more.
                using ArrayPoolBufferWriter<object> bufferWriter = ArrayPoolBufferWriter<object>.Create();

                int childrenCount = VisualTreeHelper.GetChildrenCount(element);

                // Add the top level children
                for (int i = 0; i < childrenCount; i++)
                {
                    DependencyObject child = VisualTreeHelper.GetChild(element, i);

                    if (child is T result && predicate.Match(result))
                    {
                        return result;
                    }

                    bufferWriter.Add(child);
                }

                // Explore each depth level
                for (int i = 0; i < bufferWriter.Count; i++)
                {
                    DependencyObject parent = (DependencyObject)bufferWriter[i];

                    childrenCount = VisualTreeHelper.GetChildrenCount(parent);

                    for (int j = 0; j < childrenCount; j++)
                    {
                        DependencyObject child = VisualTreeHelper.GetChild(parent, j);

                        if (child is T result && predicate.Match(result))
                        {
                            return result;
                        }

                        bufferWriter.Add(child);
                    }
                }

                return null;
            }

            static T? ThrowArgumentOutOfRangeExceptionForInvalidSearchType()
            {
                throw new ArgumentOutOfRangeException(nameof(searchType), "The input search type is not valid");
            }

            return searchType switch
            {
                SearchType.DepthFirst => FindDescendantWithDepthFirstSearch(element, ref predicate),
                SearchType.BreadthFirst => FindDescendantWithBreadthFirstSearch(element, ref predicate),
                _ => ThrowArgumentOutOfRangeExceptionForInvalidSearchType()
            };
        }

        /// <summary>
        /// Find the first descendant (or self) of type <see cref="FrameworkElement"/> with a given name, using a depth-first search.
        /// </summary>
        /// <param name="element">The root element.</param>
        /// <param name="name">The name of the element to look for.</param>
        /// <param name="comparisonType">The comparison type to use to match <paramref name="name"/>.</param>
        /// <returns>The descendant (or self) that was found, or <see langword="null"/>.</returns>
        public static FrameworkElement? FindDescendantOrSelf(this DependencyObject element, string name, StringComparison comparisonType = StringComparison.Ordinal)
        {
            if (element is FrameworkElement result && name.Equals(result.Name, comparisonType))
            {
                return result;
            }

            return FindDescendant(element, name, comparisonType);
        }

        /// <summary>
        /// Find the first descendant (or self) of type <see cref="FrameworkElement"/> with a given name.
        /// </summary>
        /// <param name="element">The root element.</param>
        /// <param name="name">The name of the element to look for.</param>
        /// <param name="comparisonType">The comparison type to use to match <paramref name="name"/>.</param>
        /// <param name="searchType">The search type to use to explore the visual tree.</param>
        /// <returns>The descendant (or self) that was found, or <see langword="null"/>.</returns>
        public static FrameworkElement? FindDescendantOrSelf(this DependencyObject element, string name, StringComparison comparisonType, SearchType searchType)
        {
            if (element is FrameworkElement result && name.Equals(result.Name, comparisonType))
            {
                return result;
            }

            return FindDescendant(element, name, comparisonType, searchType);
        }

        /// <summary>
        /// Find the first descendant (or self) element of a given type, using a depth-first search.
        /// </summary>
        /// <typeparam name="T">The type of elements to match.</typeparam>
        /// <param name="element">The root element.</param>
        /// <returns>The descendant (or self) that was found, or <see langword="null"/>.</returns>
        public static T? FindDescendantOrSelf<T>(this DependencyObject element)
            where T : notnull, DependencyObject
        {
            if (element is T result)
            {
                return result;
            }

            return FindDescendant<T>(element);
        }

        /// <summary>
        /// Find the first descendant (or self) element of a given type.
        /// </summary>
        /// <typeparam name="T">The type of elements to match.</typeparam>
        /// <param name="element">The root element.</param>
        /// <param name="searchType">The search type to use to explore the visual tree.</param>
        /// <returns>The descendant (or self) that was found, or <see langword="null"/>.</returns>
        public static T? FindDescendantOrSelf<T>(this DependencyObject element, SearchType searchType)
            where T : notnull, DependencyObject
        {
            if (element is T result)
            {
                return result;
            }

            return FindDescendant<T>(element, searchType);
        }

        /// <summary>
        /// Find the first descendant (or self) element of a given type, using a depth-first search.
        /// </summary>
        /// <param name="element">The root element.</param>
        /// <param name="type">The type of element to match.</param>
        /// <returns>The descendant (or self) that was found, or <see langword="null"/>.</returns>
        public static DependencyObject? FindDescendantOrSelf(this DependencyObject element, Type type)
        {
            if (element.GetType() == type)
            {
                return element;
            }

            return FindDescendant(element, type);
        }

        /// <summary>
        /// Find the first descendant (or self) element of a given type.
        /// </summary>
        /// <param name="element">The root element.</param>
        /// <param name="type">The type of element to match.</param>
        /// <param name="searchType">The search type to use to explore the visual tree.</param>
        /// <returns>The descendant (or self) that was found, or <see langword="null"/>.</returns>
        public static DependencyObject? FindDescendantOrSelf(this DependencyObject element, Type type, SearchType searchType)
        {
            if (element.GetType() == type)
            {
                return element;
            }

            return FindDescendant(element, type, searchType);
        }

        /// <summary>
        /// Find the first descendant (or self) element matching a given predicate, using a depth-first search.
        /// </summary>
        /// <typeparam name="T">The type of elements to match.</typeparam>
        /// <param name="element">The root element.</param>
        /// <param name="predicate">The predicatee to use to match the descendant nodes.</param>
        /// <returns>The descendant (or self) that was found, or <see langword="null"/>.</returns>
        public static T? FindDescendantOrSelf<T>(this DependencyObject element, Func<T, bool> predicate)
            where T : notnull, DependencyObject
        {
            if (element is T result && predicate(result))
            {
                return result;
            }

            return FindDescendant(element, predicate);
        }

        /// <summary>
        /// Find the first descendant (or self) element matching a given predicate.
        /// </summary>
        /// <typeparam name="T">The type of elements to match.</typeparam>
        /// <param name="element">The root element.</param>
        /// <param name="predicate">The predicatee to use to match the descendant nodes.</param>
        /// <param name="searchType">The search type to use to explore the visual tree.</param>
        /// <returns>The descendant (or self) that was found, or <see langword="null"/>.</returns>
        public static T? FindDescendantOrSelf<T>(this DependencyObject element, Func<T, bool> predicate, SearchType searchType)
            where T : notnull, DependencyObject
        {
            if (element is T result && predicate(result))
            {
                return result;
            }

            return FindDescendant(element, predicate, searchType);
        }

        /// <summary>
        /// Find the first descendant (or self) element matching a given predicate, using a depth-first search.
        /// </summary>
        /// <typeparam name="T">The type of elements to match.</typeparam>
        /// <typeparam name="TState">The type of state to use when matching nodes.</typeparam>
        /// <param name="element">The root element.</param>
        /// <param name="state">The state to give as input to <paramref name="predicate"/>.</param>
        /// <param name="predicate">The predicatee to use to match the descendant nodes.</param>
        /// <returns>The descendant (or self) that was found, or <see langword="null"/>.</returns>
        public static T? FindDescendantOrSelf<T, TState>(this DependencyObject element, TState state, Func<T, TState, bool> predicate)
            where T : notnull, DependencyObject
        {
            if (element is T result && predicate(result, state))
            {
                return result;
            }

            return FindDescendant(element, state, predicate);
        }

        /// <summary>
        /// Find the first descendant (or self) element matching a given predicate.
        /// </summary>
        /// <typeparam name="T">The type of elements to match.</typeparam>
        /// <typeparam name="TState">The type of state to use when matching nodes.</typeparam>
        /// <param name="element">The root element.</param>
        /// <param name="state">The state to give as input to <paramref name="predicate"/>.</param>
        /// <param name="predicate">The predicatee to use to match the descendant nodes.</param>
        /// <param name="searchType">The search type to use to explore the visual tree.</param>
        /// <returns>The descendant (or self) that was found, or <see langword="null"/>.</returns>
        public static T? FindDescendantOrSelf<T, TState>(this DependencyObject element, TState state, Func<T, TState, bool> predicate, SearchType searchType)
            where T : notnull, DependencyObject
        {
            if (element is T result && predicate(result, state))
            {
                return result;
            }

            return FindDescendant(element, state, predicate, searchType);
        }

        /// <summary>
        /// Find all descendant elements of the specified element. This method can be chained with
        /// LINQ calls to add additional filters or projections on top of the returned results.
        /// <para>
        /// This method is meant to provide extra flexibility in specific scenarios and it should not
        /// be used when only the first item is being looked for. In those cases, use one of the
        /// available <see cref="FindDescendant{T}(DependencyObject)"/> overloads instead,
        /// which will offer a more compact syntax as well as better performance in those cases.
        /// </para>
        /// </summary>
        /// <param name="element">The root element.</param>
        /// <returns>All the descendant <see cref="DependencyObject"/> instance from <paramref name="element"/>.</returns>
        public static IEnumerable<DependencyObject> FindDescendants(this DependencyObject element)
        {
            return FindDescendants(element, SearchType.DepthFirst);
        }

        /// <summary>
        /// Find all descendant elements of the specified element. This method can be chained with
        /// LINQ calls to add additional filters or projections on top of the returned results.
        /// <para>
        /// This method is meant to provide extra flexibility in specific scenarios and it should not
        /// be used when only the first item is being looked for. In those cases, use one of the
        /// available <see cref="FindDescendant{T}(DependencyObject)"/> overloads instead,
        /// which will offer a more compact syntax as well as better performance in those cases.
        /// </para>
        /// </summary>
        /// <param name="element">The root element.</param>
        /// <param name="searchType">The search type to use to explore the visual tree.</param>
        /// <returns>All the descendant <see cref="DependencyObject"/> instance from <paramref name="element"/>.</returns>
        public static IEnumerable<DependencyObject> FindDescendants(this DependencyObject element, SearchType searchType)
        {
            // Depth-first traversal, with recursion
            static IEnumerable<DependencyObject> FindDescendantsWithDepthFirstSearch(DependencyObject element)
            {
                int childrenCount = VisualTreeHelper.GetChildrenCount(element);

                for (var i = 0; i < childrenCount; i++)
                {
                    DependencyObject child = VisualTreeHelper.GetChild(element, i);

                    yield return child;

                    foreach (DependencyObject childOfChild in FindDescendants(child))
                    {
                        yield return childOfChild;
                    }
                }
            }

            // Breadth-first traversal, with pooled local stack
            static IEnumerable<DependencyObject> FindDescendantsWithBreadthFirstSearch(DependencyObject element)
            {
                using ArrayPoolBufferWriter<object> bufferWriter = ArrayPoolBufferWriter<object>.Create();

                int childrenCount = VisualTreeHelper.GetChildrenCount(element);

                for (int i = 0; i < childrenCount; i++)
                {
                    DependencyObject child = VisualTreeHelper.GetChild(element, i);

                    yield return child;

                    bufferWriter.Add(child);
                }

                for (int i = 0; i < bufferWriter.Count; i++)
                {
                    DependencyObject parent = (DependencyObject)bufferWriter[i];

                    childrenCount = VisualTreeHelper.GetChildrenCount(parent);

                    for (int j = 0; j < childrenCount; j++)
                    {
                        DependencyObject child = VisualTreeHelper.GetChild(parent, j);

                        yield return child;

                        bufferWriter.Add(child);
                    }
                }
            }

            static IEnumerable<DependencyObject> ThrowArgumentOutOfRangeExceptionForInvalidSearchType()
            {
                throw new ArgumentOutOfRangeException(nameof(searchType), "The input search type is not valid");
            }

            return searchType switch
            {
                SearchType.DepthFirst => FindDescendantsWithDepthFirstSearch(element),
                SearchType.BreadthFirst => FindDescendantsWithBreadthFirstSearch(element),
                _ => ThrowArgumentOutOfRangeExceptionForInvalidSearchType()
            };
        }

        /// <summary>
        /// Find all descendant elements of the specified element (or self). This method can be chained
        /// with LINQ calls to add additional filters or projections on top of the returned results.
        /// <para>
        /// This method is meant to provide extra flexibility in specific scenarios and it should not
        /// be used when only the first item is being looked for. In those cases, use one of the
        /// available <see cref="FindDescendantOrSelf{T}(DependencyObject)"/> overloads instead,
        /// which will offer a more compact syntax as well as better performance in those cases.
        /// </para>
        /// </summary>
        /// <param name="element">The root element.</param>
        /// <returns>All the descendant <see cref="DependencyObject"/> instance from <paramref name="element"/>.</returns>
        public static IEnumerable<DependencyObject> FindDescendantsOrSelf(this DependencyObject element)
        {
            return FindDescendantsOrSelf(element, SearchType.DepthFirst);
        }

        /// <summary>
        /// Find all descendant elements of the specified element (or self). This method can be chained
        /// with LINQ calls to add additional filters or projections on top of the returned results.
        /// <para>
        /// This method is meant to provide extra flexibility in specific scenarios and it should not
        /// be used when only the first item is being looked for. In those cases, use one of the
        /// available <see cref="FindDescendantOrSelf{T}(DependencyObject)"/> overloads instead,
        /// which will offer a more compact syntax as well as better performance in those cases.
        /// </para>
        /// </summary>
        /// <param name="element">The root element.</param>
        /// <param name="searchType">The search type to use to explore the visual tree.</param>
        /// <returns>All the descendant <see cref="DependencyObject"/> instance from <paramref name="element"/>.</returns>
        public static IEnumerable<DependencyObject> FindDescendantsOrSelf(this DependencyObject element, SearchType searchType)
        {
            // Depth-first traversal, with recursion
            static IEnumerable<DependencyObject> FindDescendantsWithDepthFirstSearch(DependencyObject element)
            {
                yield return element;

                int childrenCount = VisualTreeHelper.GetChildrenCount(element);

                for (var i = 0; i < childrenCount; i++)
                {
                    DependencyObject child = VisualTreeHelper.GetChild(element, i);

                    yield return child;

                    foreach (DependencyObject childOfChild in FindDescendants(child))
                    {
                        yield return childOfChild;
                    }
                }
            }

            // Breadth-first traversal, with pooled local stack
            static IEnumerable<DependencyObject> FindDescendantsWithBreadthFirstSearch(DependencyObject element)
            {
                yield return element;

                using ArrayPoolBufferWriter<object> bufferWriter = ArrayPoolBufferWriter<object>.Create();

                int childrenCount = VisualTreeHelper.GetChildrenCount(element);

                for (int i = 0; i < childrenCount; i++)
                {
                    DependencyObject child = VisualTreeHelper.GetChild(element, i);

                    yield return child;

                    bufferWriter.Add(child);
                }

                for (int i = 0; i < bufferWriter.Count; i++)
                {
                    DependencyObject parent = (DependencyObject)bufferWriter[i];

                    childrenCount = VisualTreeHelper.GetChildrenCount(parent);

                    for (int j = 0; j < childrenCount; j++)
                    {
                        DependencyObject child = VisualTreeHelper.GetChild(parent, j);

                        yield return child;

                        bufferWriter.Add(child);
                    }
                }
            }

            static IEnumerable<DependencyObject> ThrowArgumentOutOfRangeExceptionForInvalidSearchType()
            {
                throw new ArgumentOutOfRangeException(nameof(searchType), "The input search type is not valid");
            }

            return searchType switch
            {
                SearchType.DepthFirst => FindDescendantsWithDepthFirstSearch(element),
                SearchType.BreadthFirst => FindDescendantsWithBreadthFirstSearch(element),
                _ => ThrowArgumentOutOfRangeExceptionForInvalidSearchType()
            };
        }

        /// <summary>
        /// Find the first ascendant of type <see cref="FrameworkElement"/> with a given name.
        /// </summary>
        /// <param name="element">The starting element.</param>
        /// <param name="name">The name of the element to look for.</param>
        /// <param name="comparisonType">The comparison type to use to match <paramref name="name"/>.</param>
        /// <returns>The ascendant that was found, or <see langword="null"/>.</returns>
        public static FrameworkElement? FindAscendant(this DependencyObject element, string name, StringComparison comparisonType = StringComparison.Ordinal)
        {
            PredicateByName predicateByName = new(name, comparisonType);

            return FindAscendant<FrameworkElement, PredicateByName>(element, ref predicateByName);
        }

        /// <summary>
        /// Find the first ascendant element of a given type.
        /// </summary>
        /// <typeparam name="T">The type of elements to match.</typeparam>
        /// <param name="element">The starting element.</param>
        /// <returns>The ascendant that was found, or <see langword="null"/>.</returns>
        public static T? FindAscendant<T>(this DependencyObject element)
            where T : notnull, DependencyObject
        {
            PredicateByAny<T> predicateByAny = default;

            return FindAscendant<T, PredicateByAny<T>>(element, ref predicateByAny);
        }

        /// <summary>
        /// Find the first ascendant element of a given type.
        /// </summary>
        /// <param name="element">The starting element.</param>
        /// <param name="type">The type of element to match.</param>
        /// <returns>The ascendant that was found, or <see langword="null"/>.</returns>
        public static DependencyObject? FindAscendant(this DependencyObject element, Type type)
        {
            PredicateByType predicateByType = new(type);

            return FindAscendant<DependencyObject, PredicateByType>(element, ref predicateByType);
        }

        /// <summary>
        /// Find the first ascendant element matching a given predicate.
        /// </summary>
        /// <typeparam name="T">The type of elements to match.</typeparam>
        /// <param name="element">The starting element.</param>
        /// <param name="predicate">The predicatee to use to match the ascendant nodes.</param>
        /// <returns>The ascendant that was found, or <see langword="null"/>.</returns>
        public static T? FindAscendant<T>(this DependencyObject element, Func<T, bool> predicate)
            where T : notnull, DependencyObject
        {
            PredicateByFunc<T> predicateByFunc = new(predicate);

            return FindAscendant<T, PredicateByFunc<T>>(element, ref predicateByFunc);
        }

        /// <summary>
        /// Find the first ascendant element matching a given predicate.
        /// </summary>
        /// <typeparam name="T">The type of elements to match.</typeparam>
        /// <typeparam name="TState">The type of state to use when matching nodes.</typeparam>
        /// <param name="element">The starting element.</param>
        /// <param name="state">The state to give as input to <paramref name="predicate"/>.</param>
        /// <param name="predicate">The predicatee to use to match the ascendant nodes.</param>
        /// <returns>The ascendant that was found, or <see langword="null"/>.</returns>
        public static T? FindAscendant<T, TState>(this DependencyObject element, TState state, Func<T, TState, bool> predicate)
            where T : notnull, DependencyObject
        {
            PredicateByFunc<T, TState> predicateByFunc = new(state, predicate);

            return FindAscendant<T, PredicateByFunc<T, TState>>(element, ref predicateByFunc);
        }

        /// <summary>
        /// Find the first ascendant element matching a given predicate.
        /// </summary>
        /// <typeparam name="T">The type of elements to match.</typeparam>
        /// <typeparam name="TPredicate">The type of predicate in use.</typeparam>
        /// <param name="element">The starting element.</param>
        /// <param name="predicate">The predicatee to use to match the ascendant nodes.</param>
        /// <returns>The ascendant that was found, or <see langword="null"/>.</returns>
        private static T? FindAscendant<T, TPredicate>(this DependencyObject element, ref TPredicate predicate)
            where T : notnull, DependencyObject
            where TPredicate : struct, IPredicate<T>
        {
            while (true)
            {
                DependencyObject? parent = VisualTreeHelper.GetParent(element);

                if (parent is null)
                {
                    return null;
                }

                if (parent is T result && predicate.Match(result))
                {
                    return result;
                }

                element = parent;
            }
        }

        /// <summary>
        /// Find the first ascendant (or self) of type <see cref="FrameworkElement"/> with a given name.
        /// </summary>
        /// <param name="element">The starting element.</param>
        /// <param name="name">The name of the element to look for.</param>
        /// <param name="comparisonType">The comparison type to use to match <paramref name="name"/>.</param>
        /// <returns>The ascendant (or self) that was found, or <see langword="null"/>.</returns>
        public static FrameworkElement? FindAscendantOrSelf(this DependencyObject element, string name, StringComparison comparisonType = StringComparison.Ordinal)
        {
            if (element is FrameworkElement result && name.Equals(result.Name, comparisonType))
            {
                return result;
            }

            return FindAscendant(element, name, comparisonType);
        }

        /// <summary>
        /// Find the first ascendant (or self) element of a given type.
        /// </summary>
        /// <typeparam name="T">The type of elements to match.</typeparam>
        /// <param name="element">The starting element.</param>
        /// <returns>The ascendant (or self) that was found, or <see langword="null"/>.</returns>
        public static T? FindAscendantOrSelf<T>(this DependencyObject element)
            where T : notnull, DependencyObject
        {
            if (element is T result)
            {
                return result;
            }

            return FindAscendant<T>(element);
        }

        /// <summary>
        /// Find the first ascendant (or self) element of a given type.
        /// </summary>
        /// <param name="element">The starting element.</param>
        /// <param name="type">The type of element to match.</param>
        /// <returns>The ascendant (or self) that was found, or <see langword="null"/>.</returns>
        public static DependencyObject? FindAscendantOrSelf(this DependencyObject element, Type type)
        {
            if (element.GetType() == type)
            {
                return element;
            }

            return FindAscendant(element, type);
        }

        /// <summary>
        /// Find the first ascendant (or self) element matching a given predicate.
        /// </summary>
        /// <typeparam name="T">The type of elements to match.</typeparam>
        /// <param name="element">The starting element.</param>
        /// <param name="predicate">The predicatee to use to match the ascendant nodes.</param>
        /// <returns>The ascendant (or self) that was found, or <see langword="null"/>.</returns>
        public static T? FindAscendantOrSelf<T>(this DependencyObject element, Func<T, bool> predicate)
            where T : notnull, DependencyObject
        {
            if (element is T result && predicate(result))
            {
                return result;
            }

            return FindAscendant(element, predicate);
        }

        /// <summary>
        /// Find the first ascendant (or self) element matching a given predicate.
        /// </summary>
        /// <typeparam name="T">The type of elements to match.</typeparam>
        /// <typeparam name="TState">The type of state to use when matching nodes.</typeparam>
        /// <param name="element">The starting element.</param>
        /// <param name="state">The state to give as input to <paramref name="predicate"/>.</param>
        /// <param name="predicate">The predicatee to use to match the ascendant nodes.</param>
        /// <returns>The ascendant (or self) that was found, or <see langword="null"/>.</returns>
        public static T? FindAscendantOrSelf<T, TState>(this DependencyObject element, TState state, Func<T, TState, bool> predicate)
            where T : notnull, DependencyObject
        {
            if (element is T result && predicate(result, state))
            {
                return result;
            }

            return FindAscendant(element, state, predicate);
        }

        /// <summary>
        /// Find all ascendant elements of the specified element. This method can be chained with
        /// LINQ calls to add additional filters or projections on top of the returned results.
        /// <para>
        /// This method is meant to provide extra flexibility in specific scenarios and it should not
        /// be used when only the first item is being looked for. In those cases, use one of the
        /// available <see cref="FindAscendant{T}(DependencyObject)"/> overloads instead,
        /// which will offer a more compact syntax as well as better performance in those cases.
        /// </para>
        /// </summary>
        /// <param name="element">The root element.</param>
        /// <returns>All the descendant <see cref="DependencyObject"/> instance from <paramref name="element"/>.</returns>
        public static IEnumerable<DependencyObject> FindAscendants(this DependencyObject element)
        {
            while (true)
            {
                DependencyObject? parent = VisualTreeHelper.GetParent(element);

                if (parent is null)
                {
                    yield break;
                }

                yield return parent;

                element = parent;
            }
        }

        /// <summary>
        /// Find all ascendant elements of the specified element (or self). This method can be chained
        /// with LINQ calls to add additional filters or projections on top of the returned results.
        /// <para>
        /// This method is meant to provide extra flexibility in specific scenarios and it should not
        /// be used when only the first item is being looked for. In those cases, use one of the
        /// available <see cref="FindAscendantOrSelf{T}(DependencyObject)"/> overloads instead,
        /// which will offer a more compact syntax as well as better performance in those cases.
        /// </para>
        /// </summary>
        /// <param name="element">The root element.</param>
        /// <returns>All the descendant <see cref="DependencyObject"/> instance from <paramref name="element"/>.</returns>
        public static IEnumerable<DependencyObject> FindAscendantsOrSelf(this DependencyObject element)
        {
            yield return element;

            while (true)
            {
                DependencyObject? parent = VisualTreeHelper.GetParent(element);

                if (parent is null)
                {
                    yield break;
                }

                yield return parent;

                element = parent;
            }
        }

        /// <summary>
        /// Checks whether or not a given <see cref="DependencyObject"/> instance is a descendant of another one.
        /// </summary>
        /// <param name="child">The input child element.</param>
        /// <param name="element">The element to look for in the ascendants hierarchy for <paramref name="child"/>.</param>
        /// <returns>Whether or not <paramref name="child"/> is a descendant of <paramref name="element"/>.</returns>
        public static bool IsDescendantOf(this DependencyObject child, DependencyObject element)
        {
            while (true)
            {
                DependencyObject? parent = VisualTreeHelper.GetParent(child);

                if (parent is null)
                {
                    return false;
                }

                if (parent == element)
                {
                    return true;
                }

                child = parent;
            }
        }

        /// <summary>
        /// Checks whether or not a given <see cref="DependencyObject"/> instance is an ascendant of another one.
        /// </summary>
        /// <param name="parent">The input parent element.</param>
        /// <param name="element">The element to look for in the descendants hierarchy for <paramref name="parent"/>.</param>
        /// <returns>Whether or not <paramref name="parent"/> is an ascendant of <paramref name="element"/>.</returns>
        public static bool IsAscendantOf(this DependencyObject parent, DependencyObject element)
        {
            return IsDescendantOf(element, parent);
        }
    }
}