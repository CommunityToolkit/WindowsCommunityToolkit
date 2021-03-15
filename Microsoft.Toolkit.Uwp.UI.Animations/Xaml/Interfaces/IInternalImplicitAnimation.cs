// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;

#nullable enable

namespace Microsoft.Toolkit.Uwp.UI.Animations
{
    /// <summary>
    /// An interface adding notification support to implicit animations. This is needed to
    /// avoid the type parameters when the event is subscribed to from <see cref="ImplicitAnimationSet"/>.
    /// </summary>
    internal interface IInternalImplicitAnimation
    {
        /// <summary>
        /// Raised whenever a property that influences the animation changes.
        /// </summary>
        event EventHandler? AnimationPropertyChanged;
    }
}
