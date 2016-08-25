﻿// ******************************************************************
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
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Microsoft.Toolkit.Uwp.UI.Controls
{
    /// <summary>
    /// Represents a control that displays a header that has a collapsible window that displays content.
    /// </summary>
    public partial class Expander : ContentControl
    {
        /// <summary>
        /// Identifies the <see cref="IsExpanded"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty IsExpandedProperty = DependencyProperty.Register(nameof(IsExpanded), typeof(bool), typeof(Expander), new PropertyMetadata(false, new PropertyChangedCallback(IsExpanded_Changed)));

        /// <summary>
        /// Identifies the <see cref="ExpandDirection"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty ExpandDirectionProperty = DependencyProperty.Register(nameof(ExpandDirection), typeof(ExpandDirection), typeof(Expander), new PropertyMetadata(ExpandDirection.Down, new PropertyChangedCallback(ExpandDirection_Changed)));

        /// <summary>
        /// Identifies the <see cref="Header"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty HeaderProperty = DependencyProperty.Register(nameof(Header), typeof(object), typeof(Expander), new PropertyMetadata(null, new PropertyChangedCallback(Header_Changed)));

        /// <summary>
        /// Identifies the <see cref="HeaderTemplate"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty HeaderTemplateProperty = DependencyProperty.Register(nameof(HeaderTemplate), typeof(DataTemplate), typeof(Expander), new PropertyMetadata(null, new PropertyChangedCallback(Header_Changed)));

        private static void Header_Changed(DependencyObject obj, DependencyPropertyChangedEventArgs args)
        {
            var expander = obj as Expander;
            expander.UpdateVisualState();
        }

        private static void ExpandDirection_Changed(DependencyObject obj, DependencyPropertyChangedEventArgs args)
        {
            var expander = obj as Expander;
            expander.UpdateVisualState();
        }

        private static void IsExpanded_Changed(DependencyObject obj, DependencyPropertyChangedEventArgs args)
        {
            var expander = obj as Expander;
            expander.UpdateVisualState();
        }

        /// <summary>
        /// Gets or sets a value indicating whether gets or sets a value that specifies whether the expander is expanded to its full size.
        /// </summary>
        public bool IsExpanded
        {
            get
            {
                return (bool)GetValue(IsExpandedProperty);
            }

            set
            {
                SetValue(IsExpandedProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets a value indicating the direction the Expander control will expand.
        /// </summary>
        public ExpandDirection ExpandDirection
        {
            get
            {
                return (ExpandDirection)GetValue(ExpandDirectionProperty);
            }

            set
            {
                SetValue(ExpandDirectionProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets the content for the control's header.
        /// </summary>
        public object Header
        {
            get
            {
                return (object)GetValue(HeaderProperty);
            }

            set
            {
                SetValue(HeaderProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets the template for the control's header.
        /// </summary>
        public DataTemplate HeaderTemplate
        {
            get { return (DataTemplate)GetValue(HeaderTemplateProperty); }
            set { SetValue(HeaderTemplateProperty, value); }
        }
    }
}
