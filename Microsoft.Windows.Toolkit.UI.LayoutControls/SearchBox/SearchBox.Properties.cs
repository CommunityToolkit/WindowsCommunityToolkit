using System; 
using System.Windows.Input; 
using Windows.UI.Xaml;

namespace Microsoft.Windows.Toolkit.UI.Controls
{
    /// <summary>
    /// Defines the properties for the <see cref="SearchBox"/> control.
    /// </summary>
    public partial class SearchBox
    {
        /// <summary>
        /// Defines the display mode states for the <see cref="SearchBox"/> control.
        /// </summary>
        public enum DisplayModeValue
        {
            Visible,

            Expand,

            FadeIn
        }

        /// <summary>
        /// Defines the <see cref="PlaceholderText"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty PlaceholderTextProperty =
            DependencyProperty.Register(
                nameof(PlaceholderText),
                typeof(string),
                typeof(SearchBox),
                new PropertyMetadata("search"));

        /// <summary>
        /// Defines the <see cref="Text"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty TextProperty = DependencyProperty.Register(
            nameof(Text),
            typeof(string),
            typeof(SearchBox),
            new PropertyMetadata(
                string.Empty,
                (d, e) => { ((SearchBox)d).UpdatePlaceholderTextVisibility(e.NewValue); }));

        /// <summary>
        /// Defines the <see cref="MaxLength"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty MaxLengthProperty = DependencyProperty.Register(
            nameof(MaxLength),
            typeof(int),
            typeof(SearchBox),
            new PropertyMetadata(int.MaxValue));

        /// <summary>
        /// Defines the <see cref="TextAlignment"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty TextAlignmentProperty =
            DependencyProperty.Register(
                nameof(TextAlignment),
                typeof(TextAlignment),
                typeof(SearchBox),
                new PropertyMetadata(TextAlignment.Left));

        /// <summary>
        /// Defines the <see cref="SearchCommand"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty SearchCommandProperty =
            DependencyProperty.Register(
                nameof(SearchCommand),
                typeof(ICommand),
                typeof(SearchBox),
                new PropertyMetadata(null));

        /// <summary>
        /// Defines the <see cref="SearchWidth"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty SearchWidthProperty = DependencyProperty.Register(
            nameof(SearchWidth),
            typeof(double),
            typeof(SearchBox),
            new PropertyMetadata(250.0));

        /// <summary>
        /// Defines the <see cref="SearchButtonSize"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty SearchButtonSizeProperty =
            DependencyProperty.Register(
                nameof(SearchButtonSize),
                typeof(double),
                typeof(SearchBox),
                new PropertyMetadata(20.0));

        /// <summary>
        /// Defines the <see cref="DisplayMode"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty DisplayModeProperty = DependencyProperty.Register(
            nameof(DisplayMode),
            typeof(DisplayModeValue),
            typeof(SearchBox),
            new PropertyMetadata(
                DisplayModeValue.Visible,
                (d, e) => { ((SearchBox)d).UpdateSearchTextGridVisibility(); }));

        /// <summary>
        /// Defines the <see cref="IsTextVisible"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty IsTextVisibleProperty =
            DependencyProperty.Register(
                nameof(IsTextVisible),
                typeof(bool),
                typeof(SearchBox),
                new PropertyMetadata(false));

        /// <summary>
        /// Defines the <see cref="PlaceholderTextVisibility"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty PlaceholderTextVisibilityProperty =
            DependencyProperty.Register(
                nameof(PlaceholderTextVisibility),
                typeof(Visibility),
                typeof(SearchBox),
                new PropertyMetadata(Visibility.Visible));

        /// <summary>
        /// Defines the <see cref="SearchTextGridVisibility"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty SearchTextGridVisibilityProperty =
            DependencyProperty.Register(
                nameof(SearchTextGridVisibility),
                typeof(Visibility),
                typeof(SearchBox),
                new PropertyMetadata(Visibility.Collapsed));

        /// <summary>
        /// Defines the <see cref="SearchTextGridOpacity"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty SearchTextGridOpacityProperty =
            DependencyProperty.Register(
                nameof(SearchTextGridOpacity),
                typeof(double),
                typeof(SearchBox),
                new PropertyMetadata(1.0));

        /// <summary>
        /// Defines the <see cref="ShadowOpacity"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty ShadowOpacityProperty =
            DependencyProperty.Register(
                nameof(ShadowOpacity),
                typeof(double),
                typeof(SearchBox),
                new PropertyMetadata(0.0));

        /// <summary>
        /// Defines the event called when the visibility of the search text changes.
        /// </summary>
        public event EventHandler<bool> IsTextVisibleChanged;

        /// <summary>
        /// Gets or sets the placeholder text.
        /// </summary>
        public string PlaceholderText
        {
            get
            {
                return (string)GetValue(PlaceholderTextProperty);
            }
            set
            {
                SetValue(PlaceholderTextProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets the text.
        /// </summary>
        public string Text
        {
            get
            {
                return (string)GetValue(TextProperty);
            }
            set
            {
                SetValue(TextProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets the max length the text can be.
        /// </summary>
        public int MaxLength
        {
            get
            {
                return (int)GetValue(MaxLengthProperty);
            }
            set
            {
                SetValue(MaxLengthProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets the alignment of the text within the control.
        /// </summary>
        public TextAlignment TextAlignment
        {
            get
            {
                return (TextAlignment)GetValue(TextAlignmentProperty);
            }
            set
            {
                SetValue(TextAlignmentProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets the command invoked when performing the search.
        /// </summary>
        public ICommand SearchCommand
        {
            get
            {
                return (ICommand)GetValue(SearchCommandProperty);
            }
            set
            {
                SetValue(SearchCommandProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets the width of the search field.
        /// </summary>
        public double SearchWidth
        {
            get
            {
                return (double)GetValue(SearchWidthProperty);
            }
            set
            {
                SetValue(SearchWidthProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets the size of the search button.
        /// </summary>
        public double SearchButtonSize
        {
            get
            {
                return (double)GetValue(SearchButtonSizeProperty);
            }
            set
            {
                SetValue(SearchButtonSizeProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets the display mode of the control.
        /// </summary>
        public DisplayModeValue DisplayMode
        {
            get
            {
                return (DisplayModeValue)GetValue(DisplayModeProperty);
            }
            set
            {
                SetValue(DisplayModeProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets the visibility of the placeholder text.
        /// </summary>
        public Visibility PlaceholderTextVisibility
        {
            get
            {
                return (Visibility)GetValue(PlaceholderTextVisibilityProperty);
            }
            set
            {
                SetValue(PlaceholderTextVisibilityProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets the visibility of the search field.
        /// </summary>
        public Visibility SearchTextGridVisibility
        {
            get
            {
                return (Visibility)GetValue(SearchTextGridVisibilityProperty);
            }
            set
            {
                SetValue(SearchTextGridVisibilityProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets the opacity of the search field.
        /// </summary>
        public double SearchTextGridOpacity
        {
            get
            {
                return (double)GetValue(SearchTextGridOpacityProperty);
            }
            set
            {
                SetValue(SearchTextGridOpacityProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets the opacity of the shadow on the control.
        /// </summary>
        public double ShadowOpacity
        {
            get
            {
                return (double)GetValue(ShadowOpacityProperty);
            }
            set
            {
                SetValue(ShadowOpacityProperty, value);
            }
        }

        /// <summary>
        /// Gets a value indicating whether the search text is visible.
        /// </summary>
        public bool IsTextVisible
        {
            get
            {
                return (bool)GetValue(IsTextVisibleProperty);
            }
            private set
            {
                SetValue(IsTextVisibleProperty, value);
                RaiseIsTextVisibleChanged(value);
            }
        }
    }
}