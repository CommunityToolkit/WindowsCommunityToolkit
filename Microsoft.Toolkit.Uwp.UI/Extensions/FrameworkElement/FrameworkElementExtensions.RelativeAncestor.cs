using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;

namespace Microsoft.Toolkit.Uwp.UI.Extensions
{
    /// <summary>
    /// Provides attached dependency properties for the <see cref="Windows.UI.Xaml.FrameworkElement"/>
    /// </summary>
    public static partial class FrameworkElementExtensions
    {
        public static object GetAncestor(DependencyObject obj)
        {
            return (object)obj.GetValue(AncestorProperty);
        }

        public static void SetAncestor(DependencyObject obj, object value)
        {
            obj.SetValue(AncestorProperty, value);
        }

        public static readonly DependencyProperty AncestorProperty =
            DependencyProperty.RegisterAttached("Ancestor", typeof(object), typeof(FrameworkElementExtensions), new PropertyMetadata(null));


        public static Type GetAncestorType(DependencyObject obj)
        {
            return (Type)obj.GetValue(AncestorTypeProperty);
        }

        public static void SetAncestorType(DependencyObject obj, Type value)
        {
            obj.SetValue(AncestorTypeProperty, value);
        }

        public static readonly DependencyProperty AncestorTypeProperty =
            DependencyProperty.RegisterAttached("AncestorType", typeof(Type), typeof(FrameworkElementExtensions), new PropertyMetadata(null, AncestorType_PropertyChanged));

        private static void AncestorType_PropertyChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
        {
            if (obj is FrameworkElement fe)
            {
                fe.Loaded -= FrameworkElement_Loaded;

                if (args.NewValue != null)
                {
                    fe.Loaded += FrameworkElement_Loaded;
                    if (fe.Parent != null)
                    {
                        FrameworkElement_Loaded(fe, null);
                    }
                }
            }
        }

        private static void FrameworkElement_Loaded(object sender, RoutedEventArgs e)
        {
            if (sender is FrameworkElement fe)
            {
                var at = GetAncestorType(fe);

                // TODO: Add non-generic version in VisualTree extensions.
                MethodInfo method = typeof(VisualTree).GetMethod("FindAscendant")
                                    .MakeGenericMethod(new Type[] { at });
                var ancestor = method.Invoke(fe, new object[] { fe });

                SetAncestor(fe, ancestor);
            }
        }
    }
}
