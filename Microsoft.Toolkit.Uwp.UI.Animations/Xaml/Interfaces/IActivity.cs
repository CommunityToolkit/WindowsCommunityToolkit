// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Threading.Tasks;
using Windows.UI.Xaml;

namespace Microsoft.Toolkit.Uwp.UI.Animations
{
    /// <summary>
    /// An interface representing a XAML model for a custom activity or action within an <see cref="AnimationSet"/> 'Timeline'.
    /// </summary>
    public interface IActivity : AnimationSet.INode
    {
        /// <summary>
        /// Invokes the current activity.
        /// </summary>
        /// <returns>A <see cref="Task"/> that indicates when the activity has completed its execution.</returns>
        Task InvokeAsync(UIElement element);
    }
}
