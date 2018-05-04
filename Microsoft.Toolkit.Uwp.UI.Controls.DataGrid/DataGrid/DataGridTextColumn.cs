// ******************************************************************
// Copyright (c) Microsoft. All rights reserved.
// This code is licensed under the MIT License (MIT).
// THE CODE IS PROVIDED “AS IS”, WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED,
// INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.
// IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM,
// DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT,
// TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH
// THE CODE OR THE USE OR OTHER DEALINGS IN THE CODE.
// ******************************************************************

using System;
using System.ComponentModel;
using Microsoft.Toolkit.Uwp.UI.Controls.DataGridInternals;
using Windows.UI;
using Windows.UI.Text;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;

namespace Microsoft.Toolkit.Uwp.UI.Controls
{
    /// <summary>
    /// Represents a <see cref="DataGrid"/> column that hosts textual content in its cells.
    /// </summary>
    [StyleTypedProperty(Property = "ElementStyle", StyleTargetType = typeof(TextBlock))]
    [StyleTypedProperty(Property = "EditingElementStyle", StyleTargetType = typeof(TextBox))]
    public class DataGridTextColumn : DataGridBoundColumn
    {
        private const string DATAGRIDTEXTCOLUMN_fontFamilyName = "FontFamily";
        private const string DATAGRIDTEXTCOLUMN_fontSizeName = "FontSize";
        private const string DATAGRIDTEXTCOLUMN_fontStyleName = "FontStyle";
        private const string DATAGRIDTEXTCOLUMN_fontWeightName = "FontWeight";
        private const string DATAGRIDTEXTCOLUMN_foregroundName = "Foreground";

        private double? _fontSize;
        private FontStyle? _fontStyle;
        private FontWeight? _fontWeight;
        private Brush _foreground;

        /// <summary>
        /// Initializes a new instance of the <see cref="DataGridTextColumn"/> class.
        /// </summary>
        public DataGridTextColumn()
        {
            this.BindingTarget = TextBox.TextProperty;
        }

        /// <summary>
        /// Gets or sets the font name.
        /// </summary>
        public FontFamily FontFamily
        {
            get { return (FontFamily)GetValue(FontFamilyProperty); }
            set { SetValue(FontFamilyProperty, value); }
        }

        /// <summary>
        /// Identifies the FontFamily dependency property.
        /// </summary>
        public static readonly DependencyProperty FontFamilyProperty =
            DependencyProperty.Register(
                "FontFamily",
                typeof(FontFamily),
                typeof(DataGridTextColumn),
                new PropertyMetadata(null, OnFontFamilyPropertyChanged));

        private static void OnFontFamilyPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            DataGridTextColumn textColumn = (DataGridTextColumn)d;
            textColumn.NotifyPropertyChanged(DATAGRIDTEXTCOLUMN_fontFamilyName);
        }

        /// <summary>
        /// Gets or sets the font size.
        /// </summary>
        // Use DefaultValue here so undo in the Designer will set this to NaN
        [DefaultValue(double.NaN)]
        public double FontSize
        {
            get
            {
                return _fontSize ?? double.NaN;
            }

            set
            {
                if (_fontSize != value)
                {
                    _fontSize = value;
                    NotifyPropertyChanged(DATAGRIDTEXTCOLUMN_fontSizeName);
                }
            }
        }

        /// <summary>
        /// Gets or sets the font style.
        /// </summary>
        public FontStyle FontStyle
        {
            get
            {
                return _fontStyle ?? FontStyle.Normal;
            }

            set
            {
                if (_fontStyle != value)
                {
                    _fontStyle = value;
                    NotifyPropertyChanged(DATAGRIDTEXTCOLUMN_fontStyleName);
                }
            }
        }

        /// <summary>
        /// Gets or sets the font weight or thickness.
        /// </summary>
        public FontWeight FontWeight
        {
            get
            {
                return _fontWeight ?? FontWeights.Normal;
            }

            set
            {
                if (!_fontWeight.HasValue || _fontWeight.Value.Weight != value.Weight)
                {
                    _fontWeight = value;
                    NotifyPropertyChanged(DATAGRIDTEXTCOLUMN_fontWeightName);
                }
            }
        }

        /// <summary>
        /// Gets or sets a brush that describes the foreground of the column cells.
        /// </summary>
        public Brush Foreground
        {
            get
            {
                return _foreground;
            }

            set
            {
                if (_foreground != value)
                {
                    _foreground = value;
                    NotifyPropertyChanged(DATAGRIDTEXTCOLUMN_foregroundName);
                }
            }
        }

        /// <summary>
        /// Causes the column cell being edited to revert to the specified value.
        /// </summary>
        /// <param name="editingElement">The element that the column displays for a cell in editing mode.</param>
        /// <param name="uneditedValue">The previous, unedited value in the cell being edited.</param>
        protected override void CancelCellEdit(FrameworkElement editingElement, object uneditedValue)
        {
            TextBox textBox = editingElement as TextBox;
            if (textBox != null)
            {
                string uneditedString = uneditedValue as string;
                textBox.Text = uneditedString ?? string.Empty;
            }
        }

        /// <summary>
        /// Gets a <see cref="T:System.Windows.Controls.TextBox"/> control that is bound to the column's <see cref="P:Microsoft.Toolkit.Uwp.UI.Controls.DataGridBoundColumn.Binding"/> property value.
        /// </summary>
        /// <param name="cell">The cell that will contain the generated element.</param>
        /// <param name="dataItem">The data item represented by the row that contains the intended cell.</param>
        /// <returns>A new <see cref="T:System.Windows.Controls.TextBox"/> control that is bound to the column's <see cref="P:Microsoft.Toolkit.Uwp.UI.Controls.DataGridBoundColumn.Binding"/> property value.</returns>
        protected override FrameworkElement GenerateEditingElement(DataGridCell cell, object dataItem)
        {
            TextBox textBox = new TextBox();
            textBox.VerticalAlignment = VerticalAlignment.Stretch;
            textBox.Background = new SolidColorBrush(Colors.Transparent);

            if (DependencyProperty.UnsetValue != ReadLocalValue(DataGridTextColumn.FontFamilyProperty))
            {
                textBox.FontFamily = this.FontFamily;
            }

            if (_fontSize.HasValue)
            {
                textBox.FontSize = _fontSize.Value;
            }

            if (_fontStyle.HasValue)
            {
                textBox.FontStyle = _fontStyle.Value;
            }

            if (_fontWeight.HasValue)
            {
                textBox.FontWeight = _fontWeight.Value;
            }

            if (_foreground != null)
            {
                textBox.Foreground = _foreground;
            }

            bool isDesignMode = Windows.ApplicationModel.DesignMode.DesignModeEnabled;

            if (this.Binding != null || !isDesignMode)
            {
                textBox.SetBinding(this.BindingTarget, this.Binding);
            }

            return textBox;
        }

        /// <summary>
        /// Gets a read-only <see cref="T:System.Windows.Controls.TextBlock"/> element that is bound to the column's <see cref="P:Microsoft.Toolkit.Uwp.UI.Controls.DataGridBoundColumn.Binding"/> property value.
        /// </summary>
        /// <param name="cell">The cell that will contain the generated element.</param>
        /// <param name="dataItem">The data item represented by the row that contains the intended cell.</param>
        /// <returns>A new, read-only <see cref="T:System.Windows.Controls.TextBlock"/> element that is bound to the column's <see cref="P:Microsoft.Toolkit.Uwp.UI.Controls.DataGridBoundColumn.Binding"/> property value.</returns>
        protected override FrameworkElement GenerateElement(DataGridCell cell, object dataItem)
        {
            TextBlock textBlockElement = new TextBlock();
            textBlockElement.Margin = new Thickness(12, 0, 12, 0);
            textBlockElement.VerticalAlignment = VerticalAlignment.Center;
            if (DependencyProperty.UnsetValue != ReadLocalValue(DataGridTextColumn.FontFamilyProperty))
            {
                textBlockElement.FontFamily = this.FontFamily;
            }

            if (_fontSize.HasValue)
            {
                textBlockElement.FontSize = _fontSize.Value;
            }

            if (_fontStyle.HasValue)
            {
                textBlockElement.FontStyle = _fontStyle.Value;
            }

            if (_fontWeight.HasValue)
            {
                textBlockElement.FontWeight = _fontWeight.Value;
            }

            if (_foreground != null)
            {
                textBlockElement.Foreground = _foreground;
            }

            bool isDesignMode = Windows.ApplicationModel.DesignMode.DesignModeEnabled;

            if (this.Binding != null || !isDesignMode)
            {
                textBlockElement.SetBinding(TextBlock.TextProperty, this.Binding);
            }

            return textBlockElement;
        }

        /// <summary>
        /// Called when the cell in the column enters editing mode.
        /// </summary>
        /// <param name="editingElement">The element that the column displays for a cell in editing mode.</param>
        /// <param name="editingEventArgs">Information about the user gesture that is causing a cell to enter editing mode.</param>
        /// <returns>The unedited value. </returns>
        protected override object PrepareCellForEdit(FrameworkElement editingElement, RoutedEventArgs editingEventArgs)
        {
            TextBox textBox = editingElement as TextBox;
            if (textBox != null)
            {
                string uneditedText = textBox.Text;
                int len = uneditedText.Length;
                KeyRoutedEventArgs keyEventArgs = editingEventArgs as KeyRoutedEventArgs;
                if (keyEventArgs != null && keyEventArgs.Key == Windows.System.VirtualKey.F2)
                {
                    // Put caret at the end of the text
                    textBox.Select(len, len);
                }
                else
                {
                    // Select all text
                    textBox.Select(0, len);
                }

                return uneditedText;
            }

            return string.Empty;
        }

        /// <summary>
        /// Called by the DataGrid control when this column asks for its elements to be
        /// updated, because a property changed.
        /// </summary>
        protected internal override void RefreshCellContent(FrameworkElement element, string propertyName)
        {
            if (element == null)
            {
                throw new ArgumentNullException("element");
            }

            TextBox textBox = element as TextBox;
            if (textBox == null)
            {
                TextBlock textBlock = element as TextBlock;
                if (textBlock == null)
                {
                    throw DataGridError.DataGrid.ValueIsNotAnInstanceOfEitherOr("element", typeof(TextBox), typeof(TextBlock));
                }

                if (propertyName == DATAGRIDTEXTCOLUMN_fontFamilyName)
                {
                    textBlock.FontFamily = this.FontFamily;
                }
                else if (propertyName == DATAGRIDTEXTCOLUMN_fontSizeName)
                {
                    SetTextFontSize(textBlock, TextBlock.FontSizeProperty);
                }
                else if (propertyName == DATAGRIDTEXTCOLUMN_fontStyleName)
                {
                    textBlock.FontStyle = this.FontStyle;
                }
                else if (propertyName == DATAGRIDTEXTCOLUMN_fontWeightName)
                {
                    textBlock.FontWeight = this.FontWeight;
                }
                else if (propertyName == DATAGRIDTEXTCOLUMN_foregroundName)
                {
                    textBlock.Foreground = this.Foreground;
                }
                else
                {
                    if (this.FontFamily != null)
                    {
                        textBlock.FontFamily = this.FontFamily;
                    }

                    SetTextFontSize(textBlock, TextBlock.FontSizeProperty);
                    textBlock.FontStyle = this.FontStyle;
                    textBlock.FontWeight = this.FontWeight;
                    if (this.Foreground != null)
                    {
                        textBlock.Foreground = this.Foreground;
                    }
                }

                return;
            }

            if (propertyName == DATAGRIDTEXTCOLUMN_fontFamilyName)
            {
                textBox.FontFamily = this.FontFamily;
            }
            else if (propertyName == DATAGRIDTEXTCOLUMN_fontSizeName)
            {
                SetTextFontSize(textBox, TextBox.FontSizeProperty);
            }
            else if (propertyName == DATAGRIDTEXTCOLUMN_fontStyleName)
            {
                textBox.FontStyle = this.FontStyle;
            }
            else if (propertyName == DATAGRIDTEXTCOLUMN_fontWeightName)
            {
                textBox.FontWeight = this.FontWeight;
            }
            else if (propertyName == DATAGRIDTEXTCOLUMN_foregroundName)
            {
                textBox.Foreground = this.Foreground;
            }
            else
            {
                if (this.FontFamily != null)
                {
                    textBox.FontFamily = this.FontFamily;
                }

                SetTextFontSize(textBox, TextBox.FontSizeProperty);
                textBox.FontStyle = this.FontStyle;
                textBox.FontWeight = this.FontWeight;
                if (this.Foreground != null)
                {
                    textBox.Foreground = this.Foreground;
                }
            }
        }

        private void SetTextFontSize(DependencyObject textElement, DependencyProperty fontSizeProperty)
        {
            double newFontSize = this.FontSize;
            if (double.IsNaN(newFontSize))
            {
                textElement.ClearValue(fontSizeProperty);
            }
            else
            {
                textElement.SetValue(fontSizeProperty, newFontSize);
            }
        }
    }
}
