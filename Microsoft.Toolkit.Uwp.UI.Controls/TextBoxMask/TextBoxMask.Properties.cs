using System.Collections.Generic;
using Windows.UI.Xaml;

namespace Microsoft.Toolkit.Uwp.UI.Controls
{
    /// <summary>
    /// TextBox Mask property allows a user to more easily enter fixed width text in TextBox control
    /// where you would like them to enter the data in a certain format
    /// </summary>
    public partial class TextBoxMask
    {
        /// <summary>
        /// Represents the mask for the textbox
        /// </summary>
        public static readonly DependencyProperty MaskProperty = DependencyProperty.RegisterAttached("Mask", typeof(string), typeof(TextBoxMask), new PropertyMetadata(null, OnMaskChanged));

        /// <summary>
        /// Represents the mask place holder
        /// </summary>
        public static readonly DependencyProperty PlaceHolderProperty = DependencyProperty.RegisterAttached("PlaceHolder", typeof(string), typeof(TextBoxMask), new PropertyMetadata(DefaultPlaceHolder, OnPlaceHolderChanged));
        private static readonly DependencyProperty RepresentationDictionaryProperty = DependencyProperty.RegisterAttached("RepresentationDictionary", typeof(Dictionary<char, string>), typeof(TextBoxMask), new PropertyMetadata(null));
        private static readonly DependencyProperty OldTextProperty = DependencyProperty.RegisterAttached("OldText", typeof(string), typeof(TextBoxMask), new PropertyMetadata(null));
        private static readonly DependencyProperty OldSelectionLengthProperty = DependencyProperty.RegisterAttached("OldSelectionLength", typeof(int), typeof(TextBoxMask), new PropertyMetadata(0));
        private static readonly DependencyProperty OldSelectionStartProperty = DependencyProperty.RegisterAttached("OldSelectionStart", typeof(int), typeof(TextBoxMask), new PropertyMetadata(0));

        /// <summary>
        /// Gets mask value
        /// </summary>
        /// <param name="obj">TextBox control</param>
        /// <returns>mask value</returns>
        public static string GetMask(DependencyObject obj)
        {
            return (string)obj.GetValue(MaskProperty);
        }

        /// <summary>
        /// Sets mask property
        /// </summary>
        /// <param name="obj">TextBox Control</param>
        /// <param name="value">Mask Value</param>
        public static void SetMask(DependencyObject obj, string value)
        {
            obj.SetValue(MaskProperty, value);
        }

        /// <summary>
        /// Gets placeholder value
        /// </summary>
        /// <param name="obj">TextBox control</param>
        /// <returns>placeholder value</returns>
        public static string GetPlaceHolder(DependencyObject obj)
        {
            return (string)obj.GetValue(PlaceHolderProperty);
        }

        /// <summary>
        /// Sets placeholder property
        /// </summary>
        /// <param name="obj">TextBox Control</param>
        /// <param name="value">placeholder Value</param>
        public static void SetPlaceHolder(DependencyObject obj, string value)
        {
            obj.SetValue(PlaceHolderProperty, value);
        }
    }
}
