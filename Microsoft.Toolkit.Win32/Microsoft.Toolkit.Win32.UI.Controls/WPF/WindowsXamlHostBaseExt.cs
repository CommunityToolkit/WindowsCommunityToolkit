using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Data;
using Microsoft.Windows.Interop;
using Windows.UI.Xaml.Media;

namespace Microsoft.Toolkit.Win32.UI.Controls.WPF
{
    public class WindowsXamlHostBaseExt : WindowsXamlHostBase
    {
        public WindowsXamlHostBaseExt(string typeName)
        {
            this.XamlRootInternal = UWPTypeFactory.CreateXamlContentByType(typeName);
            this.XamlRootInternal.SetWrapper(this);
        }

        protected override void OnInitialized(EventArgs e)
        {
            base.OnInitialized(e);

            SetHost();
        }

        protected virtual void SetHost()
        {
            // Set DesktopWindowXamlSource
            this.desktopWindowXamlSource.Content = this.XamlRootInternal;
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
        public void Bind(string propertyName, DependencyProperty wpfProperty, global::Windows.UI.Xaml.DependencyProperty uwpProperty, object converter = null, BindingDirection direction = BindingDirection.TwoWay)
        {
            if (direction == BindingDirection.TwoWay)
            {
                var binder = new global::Windows.UI.Xaml.Data.Binding()
                {
                    Source = this,
                    Path = new global::Windows.UI.Xaml.PropertyPath(propertyName),
                    Converter = (global::Windows.UI.Xaml.Data.IValueConverter)converter
                };
                global::Windows.UI.Xaml.Data.BindingOperations.SetBinding(XamlRootInternal, uwpProperty, binder);
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
