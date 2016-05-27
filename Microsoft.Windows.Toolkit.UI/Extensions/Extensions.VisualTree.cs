using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media;

namespace Microsoft.Windows.Toolkit.UI
{
    public static partial class Extensions
    {
        /// <summary>
        /// Find descendant control using its name.
        /// </summary>
        /// <param name="element">Parent element.</param>
        /// <param name="name">Name of the control to find</param>
        /// <returns>Descendant control or null if not found.</returns>
        public static FrameworkElement FindDescendantByName(this FrameworkElement element, string name)
        {
            if (element == null || string.IsNullOrWhiteSpace(name))
            {
                return null;
            }

            if (name.Equals(element.Name, StringComparison.OrdinalIgnoreCase))
            {
                return element;
            }

            var childCount = VisualTreeHelper.GetChildrenCount(element);
            for (int i = 0; i < childCount; i++)
            {
                var result = (VisualTreeHelper.GetChild(element, i) as FrameworkElement).FindDescendantByName(name);
                if (result != null)
                {
                    return result;
                }
            }
            return null;
        }

        /// <summary>
        /// Find first descendant control of a specified type.
        /// </summary>
        /// <typeparam name="tType">Type to search for.</typeparam>
        /// <param name="element">Parent element.</param>
        /// <returns>Descendant control or null if not found.</returns>
        public static tType FindDescendant<tType>(this DependencyObject element) where tType : DependencyObject
        {
            tType retValue = null;
            var childrenCount = VisualTreeHelper.GetChildrenCount(element);

            for (var i = 0; i < childrenCount; i++)
            {
                var child = VisualTreeHelper.GetChild(element, i);
                var type = child as tType;
                if (type != null)
                {
                    retValue = type;
                    break;
                }

                retValue = FindDescendant<tType>(child);

                if (retValue != null)
                {
                    break;
                }
            }

            return retValue;
        }

        /// <summary>
        /// Find first ascendant control of a specified type.
        /// </summary>
        /// <typeparam name="tType">Type to search for.</typeparam>
        /// <param name="element">Child element.</param>
        /// <returns>Ascendant control or null if not found.</returns>
        public static tType FindAscendant<tType>(this FrameworkElement element) where tType : FrameworkElement
        {
            if (element.Parent == null)
            {
                return null;
            }

            if (element.Parent is tType)
            {
                return element.Parent as tType;
            }

            return (element.Parent as FrameworkElement).FindAscendant<tType>();
        }

        /// <summary>
        /// Find first visual ascendant control of a specified type.
        /// </summary>
        /// <typeparam name="tType">Type to search for.</typeparam>
        /// <param name="element">Child element.</param>
        /// <returns>Ascendant control or null if not found.</returns>
        public static tType FindVisualAscendant<tType>(this FrameworkElement element) where tType : FrameworkElement
        {
            var parent = VisualTreeHelper.GetParent(element);

            if (parent == null)
            {
                return null;
            }

            if (parent is tType)
            {
                return parent as tType;
            }

            return (parent as FrameworkElement).FindVisualAscendant<tType>();
        }
    }
}
