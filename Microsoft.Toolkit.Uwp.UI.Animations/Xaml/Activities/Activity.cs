// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Threading.Tasks;
using Windows.UI.Xaml;

namespace Microsoft.Toolkit.Uwp.UI.Animations
{
    /// <summary>
    /// Base class to use when creating activities which accept a <see cref="Delay"/>.
    /// </summary>
    public abstract class Activity : DependencyObject, IActivity
    {
        /// <summary>
        /// Gets or sets the <see cref="TimeSpan"/> to wait before running the activity.
        /// </summary>
        public TimeSpan? Delay { get; set; }

        /// <inheritdoc/>
        public virtual Task InvokeAsync(UIElement element)
        {
            if (Delay is not null)
            {
                return Task.Delay(Delay.Value);
            }

            return Task.CompletedTask;
        }
    }
}
