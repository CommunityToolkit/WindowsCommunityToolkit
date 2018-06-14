// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Windows.UI.Xaml;

namespace Microsoft.Toolkit.Uwp.UI.Animations.Behaviors
{
    /// <summary>
    /// Non-generic convenience implementation to provide backwards compatibility.
    /// </summary>
    /// <seealso cref="Microsoft.Toolkit.Uwp.UI.Animations.Behaviors.CompositionBehaviorBase{T}" />
    public abstract class CompositionBehaviorBase : CompositionBehaviorBase<UIElement>
    {
    }
}
