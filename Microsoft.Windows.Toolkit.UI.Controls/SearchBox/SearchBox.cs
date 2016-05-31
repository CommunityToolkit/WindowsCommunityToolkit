using System.Threading.Tasks;
using Windows.System;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml;

namespace Microsoft.Windows.Toolkit.UI.Controls
{
    using System;

    /// <summary>
    /// Defines a text box control used for search.
    /// </summary>
    [TemplatePart(Name = "TextBox", Type = typeof(TextBox))]
    [TemplatePart(Name = "SearchButtonGrid", Type = typeof(Grid))]
    public partial class SearchBox : Control
    {
        private const double _animationDurationMilliseconds = 500;

        private TextBox _textBox;

        private Grid _searchButtonGrid;

        /// <summary>
        /// Initializes a new instance of the <see cref="SearchBox"/>.       
        /// </summary>
        public SearchBox()
        {
            DefaultStyleKey = typeof(SearchBox);
        }

        /// <summary>
        /// Called when applying the control's template.
        /// </summary>
        protected override void OnApplyTemplate()
        {
            if (_textBox != null)
            {
                _textBox.LostFocus -= TextBox_LostFocus;
                _textBox.KeyUp -= TextBox_KeyUp;
                _textBox = null;
            }

            if (_searchButtonGrid != null)
            {
                _searchButtonGrid.Tapped -= SearchButtonGrid_Tapped;
                _searchButtonGrid.PointerEntered -= SearchButtonGrid_PointerEntered;
                _searchButtonGrid.PointerExited -= SearchButtonGrid_PointerExited;
                _searchButtonGrid.PointerPressed -= SearchButtonGrid_PointerPressed;
                _searchButtonGrid.PointerReleased -= SearchButtonGrid_PointerEntered;
                _searchButtonGrid = null;
            }

            _textBox = GetTemplateChild("TextBox") as TextBox;
            _searchButtonGrid = GetTemplateChild("SearchButtonGrid") as Grid;

            if (_textBox != null)
            {
                _textBox.LostFocus += TextBox_LostFocus;
                _textBox.KeyUp += TextBox_KeyUp;
            }

            if (_searchButtonGrid != null)
            {
                _searchButtonGrid.Tapped += SearchButtonGrid_Tapped;
                _searchButtonGrid.PointerEntered += SearchButtonGrid_PointerEntered;
                _searchButtonGrid.PointerExited += SearchButtonGrid_PointerExited;
                _searchButtonGrid.PointerPressed += SearchButtonGrid_PointerPressed;
                _searchButtonGrid.PointerReleased += SearchButtonGrid_PointerEntered;
            }

            UpdateSearchTextGridVisibility();
            base.OnApplyTemplate();
        }

        /// <summary>
        /// Resets the search box control.
        /// </summary>
        public void ResetControl()
        {
            Text = string.Empty;
            HideSearchText();
        }

        private void TextBox_KeyUp(object sender, KeyRoutedEventArgs e)
        {
            var txtBox = sender as TextBox;
            if (txtBox != null)
            {
                Text = txtBox.Text;
                UpdatePlaceholderTextVisibility(Text);

                if (e.Key == VirtualKey.Enter)
                {
                    ExecuteSearchCommand(Text);
                }
                else if (e.Key == VirtualKey.Escape)
                {
                    ResetControl();
                }
            }
        }

        private async void SearchButtonGrid_Tapped(object sender, TappedRoutedEventArgs e)
        {
            if (IsTextVisible)
            {
                if (!ExecuteSearchCommand(Text))
                {
                    HideSearchText();
                }
            }
            else
            {
                await ShowSearchText();
                _textBox?.Focus(FocusState.Keyboard);
            }
        }

        private void TextBox_LostFocus(object sender, RoutedEventArgs e)
        {

            TextBox txt = sender as TextBox;
            if (txt != null)
            {
                Text = txt.Text;
                UpdatePlaceholderTextVisibility(Text);
            }

            if (SearchCommand != null && !SearchCommand.CanExecute(Text)) HideSearchText();
        }

        private void SearchButtonGrid_PointerEntered(object sender, PointerRoutedEventArgs e) => ShadowOpacity = 0.2;

        private void SearchButtonGrid_PointerExited(object sender, PointerRoutedEventArgs e) => ShadowOpacity = 0.0;

        private void SearchButtonGrid_PointerPressed(object sender, PointerRoutedEventArgs e) => ShadowOpacity = 0.6;
        
        private bool ExecuteSearchCommand(string text)
        {
            if (!string.IsNullOrEmpty(text) && SearchCommand != null && SearchCommand.CanExecute(text))
            {
                SearchCommand.Execute(text);
                return true;
            }

            return false;
        }

        private async void HideSearchText()
        {
            if (DisplayMode == DisplayModeValue.Expand)
            {
                var oldValue = SearchWidth;
                await
                    this.AnimateDoublePropertyAsync(
                        "SearchWidth",
                        SearchWidth,
                        0.0,
                        _animationDurationMilliseconds);

                SearchTextGridVisibility = Visibility.Collapsed;
                IsTextVisible = false;
                SearchWidth = oldValue;
            }
            if (DisplayMode == DisplayModeValue.FadeIn)
            {
                await
                    this.AnimateDoublePropertyAsync(
                        "SearchTextGridOpacity",
                        1.0,
                        0.0,
                        _animationDurationMilliseconds);

                SearchTextGridVisibility = Visibility.Collapsed;
                IsTextVisible = false;
                SearchTextGridOpacity = 1.0;
            }
        }

        private async Task ShowSearchText()
        {
            if (DisplayMode == DisplayModeValue.Expand)
            {
                SearchTextGridVisibility = Visibility.Visible;
                await
                    this.AnimateDoublePropertyAsync(
                        "SearchWidth",
                        0.0,
                        SearchWidth,
                        _animationDurationMilliseconds);

                IsTextVisible = true;
            }

            if (DisplayMode == DisplayModeValue.FadeIn)
            {
                SearchTextGridVisibility = Visibility.Visible;
                await
                    this.AnimateDoublePropertyAsync(
                        "SearchTextGridOpacity",
                        0.0,
                        1.0,
                        _animationDurationMilliseconds);

                IsTextVisible = true;
            }
        }

        private void UpdatePlaceholderTextVisibility(object value)
        {
            PlaceholderTextVisibility = string.IsNullOrEmpty(value?.ToString())
                                                 ? Visibility.Visible
                                                 : Visibility.Collapsed;
        }

        private void UpdateSearchTextGridVisibility()
        {
            if (DisplayMode == DisplayModeValue.Visible)
            {
                SearchTextGridVisibility = Visibility.Visible;
                IsTextVisible = true;
            }
            else
            {
                SearchTextGridVisibility = Visibility.Collapsed;
                IsTextVisible = false;
            }
        }

        private void RaiseIsTextVisibleChanged(bool isTextVisible)
        {
            IsTextVisibleChanged?.Invoke(this, isTextVisible);
        }
    }
}