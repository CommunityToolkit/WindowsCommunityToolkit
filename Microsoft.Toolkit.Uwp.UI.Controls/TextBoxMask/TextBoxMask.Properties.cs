using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Microsoft.Toolkit.Uwp.UI.Controls
{
    public partial class TextBoxMask
    {
        public static readonly DependencyProperty MaskProperty = DependencyProperty.RegisterAttached("Mask", typeof(string), typeof(TextBoxMask), new PropertyMetadata(null, OnMaskChanged));
        public static readonly DependencyProperty PlaceHolderProperty = DependencyProperty.RegisterAttached("PlaceHolder", typeof(char), typeof(TextBoxMask), new PropertyMetadata(DefaultPlaceHolder));
        private static readonly DependencyProperty RepresentationDictionaryProperty = DependencyProperty.RegisterAttached("RepresentationDictionary", typeof(Dictionary<char, string>), typeof(TextBoxMask), new PropertyMetadata(null));
        private static TextBox MaskedTextBox;
        private static string OldText;

        public static string GetMask(DependencyObject obj)
        {
            return (string)obj.GetValue(MaskProperty);
        }

        public static void SetMask(DependencyObject obj, string value)
        {
            obj.SetValue(MaskProperty, value);
        }

        public static string GetPlaceHolder(DependencyObject obj)
        {
            return (string)obj.GetValue(MaskProperty);
        }

        public static void SetPlaceHolder(DependencyObject obj, string value)
        {
            obj.SetValue(MaskProperty, value);
        }
    }
}
