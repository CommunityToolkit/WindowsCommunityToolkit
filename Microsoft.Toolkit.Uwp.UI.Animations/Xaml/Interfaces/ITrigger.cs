// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Threading.Tasks;

namespace Microsoft.Toolkit.Uwp.UI.Animations
{
    /// <summary>
    /// An interface representing a XAML model for a custom trigger or action.
    /// </summary>
    public interface ITrigger : AnimationSet.INode
    {
        /// <summary>
        /// Invokes the current trigger.
        /// </summary>
        /// <returns>A <see cref="Task"/> that indicates when the trigger has completed its execution.</returns>
        Task InvokeAsync();
    }
}
