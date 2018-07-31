// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Data;
using Microsoft.Toolkit.Win32.UI.Interop;
using Microsoft.Toolkit.Win32.UI.Interop.WPF;

namespace Microsoft.Toolkit.Win32.UI.Controls.WPF
{
    /// <inheritdoc />
    public class WindowsXamlHostBaseExt : WindowsXamlHostBase
    {
        public WindowsXamlHostBaseExt(string typeName)
        {
            XamlRootInternal = UWPTypeFactory.CreateXamlContentByType(typeName);
            XamlRootInternal.SetWrapper(this);
        }

        /// <inheritdoc />
        protected override void OnInitialized(EventArgs e)
        {
            base.OnInitialized(e);

            SetContent();
        }

        /// <summary>
        /// Binds this wrapper object's exposed WPF DependencyProperty with the wrapped UWP object's DependencyProperty
        /// for what effectively works as a regular one- or two-way binding.
        /// </summary>
        /// <param name="propertyName">the registered name of the dependency property</param>
        /// <param name="wpfProperty">the DependencyProperty of the wrapper</param>
        /// <param name="uwpProperty">the related DependencyProperty of the UWP control</param>
        /// <param name="converter">a converter, if one's needed</param>
        /// <param name="direction">indicates that the binding should be one or two directional.  If one way, the Uwp control is only updated from the wrapper.</param>
        public void Bind(string propertyName, DependencyProperty wpfProperty, Windows.UI.Xaml.DependencyProperty uwpProperty, object converter = null, BindingDirection direction = BindingDirection.TwoWay)
        {
            if (direction == BindingDirection.TwoWay)
            {
                var binder = new Windows.UI.Xaml.Data.Binding()
                {
                    Source = this,
                    Path = new Windows.UI.Xaml.PropertyPath(propertyName),
                    Converter = (Windows.UI.Xaml.Data.IValueConverter)converter
                };
                Windows.UI.Xaml.Data.BindingOperations.SetBinding(XamlRootInternal, uwpProperty, binder);
            }

            var rebinder = new Binding()
            {
                Source = XamlRootInternal,
                Path = new PropertyPath(propertyName),
                Converter = (IValueConverter)converter
            };
            BindingOperations.SetBinding(this, wpfProperty, rebinder);
        }
    }
}
