// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Windows.UI.Xaml;

namespace Microsoft.Toolkit.Uwp.UI
{
    /// <summary>
    /// Helper class for attaching <see cref="AttachedShadowBase"/> shadows to <see cref="FrameworkElement"/>s.
    /// </summary>
    public static class Effects
    {
        /// <summary>
        /// Gets the shadow attached to a <see cref="FrameworkElement"/> by getting the value of the <see cref="ShadowProperty"/> property.
        /// </summary>
        /// <param name="obj">The <see cref="FrameworkElement"/> the <see cref="AttachedShadowBase"/> is attached to.</param>
        /// <returns>The <see cref="AttachedShadowBase"/> that is attached to the <paramref name="obj">FrameworkElement.</paramref></returns>
        public static AttachedShadowBase GetShadow(FrameworkElement obj)
        {
            return (AttachedShadowBase)obj.GetValue(ShadowProperty);
        }

        /// <summary>
        /// Attaches a shadow to an element by setting the <see cref="ShadowProperty"/> property.
        /// </summary>
        /// <param name="obj">The <see cref="FrameworkElement"/> to attach the shadow to.</param>
        /// <param name="value">The <see cref="AttachedShadowBase"/> that will be attached to the element</param>
        public static void SetShadow(FrameworkElement obj, AttachedShadowBase value)
        {
            obj.SetValue(ShadowProperty, value);
        }

        /// <summary>
        /// Attached <see cref="DependencyProperty"/> for setting an <see cref="AttachedShadowBase"/> to a <see cref="FrameworkElement"/>.
        /// </summary>
        public static readonly DependencyProperty ShadowProperty =
            DependencyProperty.RegisterAttached("Shadow", typeof(AttachedShadowBase), typeof(Effects), new PropertyMetadata(null, OnShadowChanged));

        private static void OnShadowChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (!(d is FrameworkElement element))
            {
                return;
            }

            if (e.OldValue is AttachedShadowBase oldShadow)
            {
                oldShadow.DisconnectElement(element);
            }

            if (e.NewValue is AttachedShadowBase newShadow)
            {
                newShadow.ConnectElement(element);
            }
        }
    }
}
