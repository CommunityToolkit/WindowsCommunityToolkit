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
using System.Linq;
using System.Reflection;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Markup;
using Windows.UI.Xaml.Media;

namespace Microsoft.Toolkit.Uwp.UI
{
    /// <summary>
    /// Defines a collection of extensions methods for UI.
    /// </summary>
    public static class LogicalTreeExtensions
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
        /// <param name="element">Child element.</param>
        /// <returns>Parent control or null if not found.</returns>
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
        /// Obsolete: Find first logical ascendant control of a specified type.
        /// Same as <see cref="FindParent{T}(FrameworkElement)"/>
        /// </summary>
        /// <typeparam name="T">Type to search for.</typeparam>
        /// <param name="element">Child element.</param>
        /// <returns>Ascendant control or null if not found.</returns>
        [Obsolete("This extension method is being deprecated.  Please use FindParent<T> instead.")]
        public static T FindAscendant<T>(this FrameworkElement element)
            where T : FrameworkElement
        {
            // Moved from VisualTreeExtensions to here, also created new alias to match FindParentByName.
            if (element.Parent == null)
            {
                return null;
            }

            if (element.Parent is T)
            {
                return element.Parent as T;
            }

            return (element.Parent as FrameworkElement).FindAscendant<T>();
        }

        /// <summary>
        /// Find first logical parent control of a specified type.
        /// Same as <see cref="FindAscendant{T}(FrameworkElement)"/>
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
                return element.GetType().GetProperty(contentpropname).GetValue(element) as UIElement;
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
