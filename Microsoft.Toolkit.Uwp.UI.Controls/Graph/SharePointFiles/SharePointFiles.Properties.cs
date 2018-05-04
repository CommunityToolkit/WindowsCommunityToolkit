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

using Microsoft.Graph;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;

namespace Microsoft.Toolkit.Uwp.UI.Controls.Graph
{
    /// <summary>
    /// The SharePointFiles Control displays a simple list of SharePoint Files.
    /// </summary>
    public partial class SharePointFiles : Control
    {
        /// <summary>
        /// Token to access Microsoft Graph API
        /// </summary>
        public static readonly DependencyProperty GraphAccessTokenProperty =
            DependencyProperty.Register(
                nameof(GraphAccessToken),
                typeof(string),
                typeof(SharePointFiles),
                new PropertyMetadata(string.Empty, GraphAccessTokenPropertyChanged));

        /// <summary>
        /// Url of OneDrive to display
        /// </summary>
        public static readonly DependencyProperty DriveUrlProperty =
            DependencyProperty.Register(
                nameof(DriveUrl),
                typeof(string),
                typeof(SharePointFiles),
                new PropertyMetadata(string.Empty, DriveUrlPropertyChanged));

        /// <summary>
        /// How DetailPane shows
        /// </summary>
        public static readonly DependencyProperty DetailPaneProperty =
            DependencyProperty.Register(
                nameof(DetailPane),
                typeof(DetailPaneDisplayMode),
                typeof(SharePointFiles),
                new PropertyMetadata(DetailPaneDisplayMode.Disabled, DetailPanePropertyChanged));

        /// <summary>
        /// Page size of each request
        /// </summary>
        public static readonly DependencyProperty PageSizeProperty =
            DependencyProperty.Register(
                nameof(PageSize),
                typeof(int),
                typeof(SharePointFiles),
                new PropertyMetadata(20));

        internal static readonly DependencyProperty HasMoreProperty =
            DependencyProperty.Register(
                nameof(HasMore),
                typeof(bool),
                typeof(SharePointFiles),
                new PropertyMetadata(false));

        internal static readonly DependencyProperty SelectedFileProperty =
            DependencyProperty.Register(
                nameof(SelectedFile),
                typeof(DriveItem),
                typeof(SharePointFiles),
                new PropertyMetadata(null));

        internal static readonly DependencyProperty SizeProperty =
            DependencyProperty.Register(
                nameof(FileSize),
                typeof(long),
                typeof(SharePointFiles),
                new PropertyMetadata(0L));

        internal static readonly DependencyProperty LastModifiedProperty =
            DependencyProperty.Register(
                nameof(LastModified),
                typeof(string),
                typeof(SharePointFiles),
                null);

        private static readonly DependencyProperty IsDetailPaneVisibleProperty =
            DependencyProperty.Register(
                nameof(IsDetailPaneVisible),
                typeof(bool),
                typeof(SharePointFiles),
                new PropertyMetadata(false));

        private int _fileUploading;
        private string _errorMessage;

        /// <summary>
        /// Gets or sets token to access Microsoft Graph API
        /// </summary>
        public string GraphAccessToken
        {
            get { return ((string)GetValue(GraphAccessTokenProperty))?.Trim(); }
            set { SetValue(GraphAccessTokenProperty, value?.Trim()); }
        }

        /// <summary>
        /// Gets or sets url of OneDrive to display
        /// </summary>
        public string DriveUrl
        {
            get { return ((string)GetValue(DriveUrlProperty))?.Trim(); }
            set { SetValue(DriveUrlProperty, value?.Trim()); }
        }

        /// <summary>
        /// Gets or sets how DetailPane shows
        /// </summary>
        public DetailPaneDisplayMode DetailPane
        {
            get { return (DetailPaneDisplayMode)GetValue(DetailPaneProperty); }
            set { SetValue(DetailPaneProperty, value); }
        }

        /// <summary>
        /// Gets or sets page size of each request
        /// </summary>
        public int PageSize
        {
            get { return (int)GetValue(PageSizeProperty); }
            set { SetValue(PageSizeProperty, value); }
        }

        internal bool HasMore
        {
            get { return (bool)GetValue(HasMoreProperty); }
            set { SetValue(HasMoreProperty, value); }
        }

        internal DriveItem SelectedFile
        {
            get { return (DriveItem)GetValue(SelectedFileProperty); }
            set { SetValue(SelectedFileProperty, value); }
        }

        internal long FileSize
        {
            get { return (long)GetValue(SizeProperty); }
            set { SetValue(SizeProperty, value); }
        }

        internal string LastModified
        {
            get { return (string)GetValue(LastModifiedProperty); }
            set { SetValue(LastModifiedProperty, value); }
        }

        private int FileUploading
        {
            get
            {
                return _fileUploading;
            }

            set
            {
                _fileUploading = value;
                if (value > 0)
                {
                    _status.Text = $"Uploading {value} files...";
                    _status.TextDecorations = Windows.UI.Text.TextDecorations.None;
                    _status.Foreground = new SolidColorBrush(Windows.UI.Colors.Black);
                    if (string.IsNullOrEmpty(_errorMessage))
                    {
                        _status.Visibility = Visibility.Visible;
                    }
                    else
                    {
                        _status.Visibility = Visibility.Collapsed;
                    }

                    _cancel.Visibility = Visibility.Visible;
                }
                else
                {
                    _status.Visibility = Visibility.Collapsed;
                    _cancel.Visibility = Visibility.Collapsed;
                }
            }
        }

        private string ErrorMessage
        {
            get
            {
                return _errorMessage;
            }

            set
            {
                _errorMessage = value;
                if (!string.IsNullOrEmpty(value))
                {
                    _error.Visibility = Visibility.Visible;
                    _status.Visibility = Visibility.Collapsed;
                }
                else
                {
                    if (FileUploading > 0)
                    {
                        _status.Visibility = Visibility.Visible;
                    }

                    _error.Visibility = Visibility.Collapsed;
                }
            }
        }

        private bool IsDetailPaneVisible
        {
            get
            {
                return (bool)GetValue(IsDetailPaneVisibleProperty);
            }

            set
            {
                SetValue(IsDetailPaneVisibleProperty, value);
                if (value)
                {
                    ShowDetailsPane();
                }
                else
                {
                    HideDetailsPane();
                }
            }
        }
    }
}
