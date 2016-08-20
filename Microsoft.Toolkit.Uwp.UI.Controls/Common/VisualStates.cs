// (c) Copyright Microsoft Corporation.
// This source is subject to the Microsoft Public License (Ms-PL).
// Please see http://go.microsoft.com/fwlink/?LinkID=131993 for details.
// All other rights reserved.

using System.Diagnostics;
using System.Linq;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;

namespace Microsoft.Toolkit.Uwp.UI.Controls
{
    /// <summary>
    /// Names and helpers for visual states in the controls.
    /// </summary>
    internal static class VisualStates
    {
        /// <summary>
        /// Use VisualStateManager to change the visual state of the control.
        /// </summary>
        /// <param name="control">
        /// Control whose visual state is being changed.
        /// </param>
        /// <param name="useTransitions">
        /// A value indicating whether to use transitions when updating the
        /// visual state, or to snap directly to the new visual state.
        /// </param>
        /// <param name="stateNames">
        /// Ordered list of state names and fallback states to transition into.
        /// Only the first state to be found will be used.
        /// </param>
        public static void GoToState(Control control, bool useTransitions, params string[] stateNames)
        {
            Debug.Assert(control != null, "control should not be null!");
            Debug.Assert(stateNames != null, "stateNames should not be null!");
            Debug.Assert(stateNames.Length > 0, "stateNames should not be empty!");

            foreach (string name in stateNames)
            {
                if (VisualStateManager.GoToState(control, name, useTransitions))
                {
                    break;
                }
            }
        }

        /// <summary>
        /// Gets the implementation root of the Control.
        /// </summary>
        /// <param name="dependencyObject">The DependencyObject.</param>
        /// <remarks>
        /// Implements Silverlight's corresponding internal property on Control.
        /// </remarks>
        /// <returns>Returns the implementation root or null.</returns>
        public static FrameworkElement GetImplementationRoot(DependencyObject dependencyObject)
        {
            Debug.Assert(dependencyObject != null, "DependencyObject should not be null.");
            return VisualTreeHelper.GetChildrenCount(dependencyObject) == 1 ?
                VisualTreeHelper.GetChild(dependencyObject, 0) as FrameworkElement :
                null;
        }

        /// <summary>
        /// This method tries to get the named VisualStateGroup for the
        /// dependency object. The provided object's ImplementationRoot will be
        /// looked up in this call.
        /// </summary>
        /// <param name="dependencyObject">The dependency object.</param>
        /// <param name="groupName">The visual state group's name.</param>
        /// <returns>Returns null or the VisualStateGroup object.</returns>
        public static VisualStateGroup TryGetVisualStateGroup(DependencyObject dependencyObject, string groupName)
        {
            FrameworkElement root = GetImplementationRoot(dependencyObject);
            if (root == null)
            {
                return null;
            }

            return VisualStateManager
                .GetVisualStateGroups(root)
                .FirstOrDefault(group => string.CompareOrdinal(groupName, group.Name) == 0);
        }
    }
}
