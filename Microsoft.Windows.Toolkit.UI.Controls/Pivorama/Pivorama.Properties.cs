using System;

using Microsoft.Windows.Toolkit.UI.Controls.Extensions;

using Windows.UI.Xaml;

namespace Microsoft.Windows.Toolkit.UI.Controls
{
    /// <summary>
    /// Defines the properties for the <see cref="Pivorama"/> control.
    /// </summary>
    public partial class Pivorama
    {
        /// <summary>
        /// Defines the <see cref="ItemsSource"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty ItemsSourceProperty = DependencyProperty.Register(
            nameof(ItemsSource),
            typeof(object),
            typeof(Pivorama),
            new PropertyMetadata(null));

        /// <summary>
        /// Defines the <see cref="ContentTemplate"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty ContentTemplateProperty =
            DependencyProperty.Register(
                nameof(ContentTemplate),
                typeof(DataTemplate),
                typeof(Pivorama),
                new PropertyMetadata(null));

        /// <summary>
        /// Defines the <see cref="HeaderTemplate"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty HeaderTemplateProperty =
            DependencyProperty.Register(
                nameof(HeaderTemplate),
                typeof(DataTemplate),
                typeof(Pivorama),
                new PropertyMetadata(null));

        /// <summary>
        /// Defines the <see cref="TabTemplate"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty TabTemplateProperty = DependencyProperty.Register(
            nameof(TabTemplate),
            typeof(DataTemplate),
            typeof(Pivorama),
            new PropertyMetadata(null));

        /// <summary>
        /// Defines the <see cref="FitToScreen"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty FitToScreenProperty = DependencyProperty.Register(
            nameof(FitToScreen),
            typeof(bool),
            typeof(Pivorama),
            new PropertyMetadata(
                false,
                (d, e) =>
                    {
                        ((Pivorama)d).SetFitToScreen((bool)e.NewValue);
                    }));

        /// <summary>
        /// Defines the <see cref="ItemWidth"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty ItemWidthProperty = DependencyProperty.Register(
            nameof(ItemWidth),
            typeof(double),
            typeof(Pivorama),
            new PropertyMetadata(
                440.0,
                (d, e) => { ((Pivorama)d).SetItemWidth((double)e.NewValue); }));

        /// <summary>
        /// Defines the <see cref="ItemWidthEx"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty ItemWidthExProperty = DependencyProperty.Register(
            nameof(ItemWidthEx),
            typeof(double),
            typeof(Pivorama),
            new PropertyMetadata(
                440.0,
                (d, e) => { ((Pivorama)d).SetItemWidthEx((double)e.NewValue, (double)e.OldValue); }));

        /// <summary>
        /// Defines the <see cref="Index"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty IndexProperty = DependencyProperty.Register(
            nameof(Index),
            typeof(int),
            typeof(Pivorama),
            new PropertyMetadata(0, (d, e) => { ((Pivorama)d).SetIndex((int)e.NewValue); }));

        /// <summary>
        /// Gets or sets the position.
        /// </summary>
        public double Position
        {
            get
            {
                return _panelContainer.GetTranslateX();
            }
            set
            {
                _headerContainer.TranslateX(value);
                _panelContainer.TranslateX(value);
            }
        }

        /// <summary>
        /// Gets the offset for manipulation.
        /// </summary>
        public double Offset
        {
            get
            {
                double position = Position % ItemWidthEx;
                if (Math.Sign(position) > 0)
                {
                    return ItemWidthEx - position;
                }
                return -position;
            }
        }

        /// <summary>
        /// Gets or sets the items source.
        /// </summary>
        public object ItemsSource
        {
            get
            {
                return GetValue(ItemsSourceProperty);
            }
            set
            {
                SetValue(ItemsSourceProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets the content's data template.
        /// </summary>
        public DataTemplate ContentTemplate
        {
            get
            {
                return (DataTemplate)GetValue(ContentTemplateProperty);
            }
            set
            {
                SetValue(ContentTemplateProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets the header's data template.
        /// </summary>
        public DataTemplate HeaderTemplate
        {
            get
            {
                return (DataTemplate)GetValue(HeaderTemplateProperty);
            }
            set
            {
                SetValue(HeaderTemplateProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets the tab's data template.
        /// </summary>
        public DataTemplate TabTemplate
        {
            get
            {
                return (DataTemplate)GetValue(TabTemplateProperty);
            }
            set
            {
                SetValue(TabTemplateProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether to fit the control to the screen.
        /// </summary>
        public bool FitToScreen
        {
            get
            {
                return (bool)GetValue(FitToScreenProperty);
            }
            set
            {
                SetValue(FitToScreenProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets the item width.
        /// </summary>
        public double ItemWidth
        {
            get
            {
                return (double)GetValue(ItemWidthProperty);
            }
            set
            {
                SetValue(ItemWidthProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets the extended item width.
        /// </summary>
        public double ItemWidthEx
        {
            get
            {
                return (double)GetValue(ItemWidthExProperty);
            }
            set
            {
                SetValue(ItemWidthExProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets the current selected index.
        /// </summary>
        public int Index
        {
            get
            {
                return (int)GetValue(IndexProperty);
            }
            set
            {
                SetValue(IndexProperty, value);
            }
        }
    }
}