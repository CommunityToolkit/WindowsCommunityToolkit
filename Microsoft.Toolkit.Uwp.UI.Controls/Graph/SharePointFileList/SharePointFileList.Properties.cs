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

using System.Windows.Input;
using Microsoft.Graph;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;

namespace Microsoft.Toolkit.Uwp.UI.Controls.Graph
{
    /// <summary>
    /// The SharePointFiles Control displays a simple list of SharePoint Files.
    /// </summary>
    public partial class SharePointFileList
    {
        /// <summary>
        /// Token to access Microsoft Graph API
        /// </summary>
        public static readonly DependencyProperty GraphAccessTokenProperty =
            DependencyProperty.Register(
                nameof(GraphAccessToken),
                typeof(string),
                typeof(SharePointFileList),
                new PropertyMetadata(string.Empty, GraphAccessTokenPropertyChanged));

        /// <summary>
        /// Url of OneDrive to display
        /// </summary>
        public static readonly DependencyProperty DriveUrlProperty =
            DependencyProperty.Register(
                nameof(DriveUrl),
                typeof(string),
                typeof(SharePointFileList),
                new PropertyMetadata(string.Empty, DriveUrlPropertyChanged));

        /// <summary>
        /// How details of a file shows
        /// </summary>
        public static readonly DependencyProperty DetailPaneProperty =
            DependencyProperty.Register(
                nameof(DetailPane),
                typeof(DetailPaneDisplayMode),
                typeof(SharePointFileList),
                new PropertyMetadata(DetailPaneDisplayMode.Disabled, DetailPanePropertyChanged));

        /// <summary>
        /// Page size of each request
        /// </summary>
        public static readonly DependencyProperty PageSizeProperty =
            DependencyProperty.Register(
                nameof(PageSize),
                typeof(int),
                typeof(SharePointFileList),
                new PropertyMetadata(20));

        /// <summary>
        /// Share link copied message
        /// </summary>
        public static readonly DependencyProperty ShareLinkCopiedMessageProperty =
            DependencyProperty.Register(
                nameof(ShareLinkCopiedMessage),
                typeof(string),
                typeof(SharePointFileList),
                new PropertyMetadata("Link copied!"));

        /// <summary>
        /// All files message
        /// </summary>
        public static readonly DependencyProperty AllFilesMessageProperty =
            DependencyProperty.Register(
                nameof(AllFilesMessage),
                typeof(string),
                typeof(SharePointFileList),
                new PropertyMetadata("All Files"));

        /// <summary>
        /// Delete confirm message
        /// </summary>
        public static readonly DependencyProperty DeleteConfirmMessageProperty =
            DependencyProperty.Register(
                nameof(DeleteConfirmMessage),
                typeof(string),
                typeof(SharePointFileList),
                new PropertyMetadata("Do you want to delete this file?"));

        /// <summary>
        /// Delete confirm Ok message
        /// </summary>
        public static readonly DependencyProperty DeleteConfirmOkMessageProperty =
            DependencyProperty.Register(
                nameof(DeleteConfirmOkMessage),
                typeof(string),
                typeof(SharePointFileList),
                new PropertyMetadata("OK"));

        /// <summary>
        /// Delete confirm cancel message
        /// </summary>
        public static readonly DependencyProperty DeleteConfirmCancelMessageProperty =
            DependencyProperty.Register(
                nameof(DeleteConfirmCancelMessage),
                typeof(string),
                typeof(SharePointFileList),
                new PropertyMetadata("Cancel"));

        /// <summary>
        /// Uploading files message template
        /// </summary>
        public static readonly DependencyProperty UploadingFilesMessageTemplateProperty =
            DependencyProperty.Register(
                nameof(UploadingFilesMessageTemplate),
                typeof(string),
                typeof(SharePointFileList),
                new PropertyMetadata("Uploading {0} files..."));

        internal static readonly DependencyProperty ThumbnailImageSourceProperty =
             DependencyProperty.Register(
                 nameof(ThumbnailImageSource),
                 typeof(ImageSource),
                 typeof(SharePointFileList),
                 new PropertyMetadata(null));

        internal static readonly DependencyProperty HasMoreProperty =
            DependencyProperty.Register(
                nameof(HasMore),
                typeof(bool),
                typeof(SharePointFileList),
                new PropertyMetadata(false));

        internal static readonly DependencyProperty SelectedFileProperty =
            DependencyProperty.Register(
                nameof(SelectedFile),
                typeof(DriveItem),
                typeof(SharePointFileList),
                new PropertyMetadata(null));

        internal static readonly DependencyProperty SizeProperty =
            DependencyProperty.Register(
                nameof(FileSize),
                typeof(long),
                typeof(SharePointFileList),
                new PropertyMetadata(0L));

        internal static readonly DependencyProperty LastModifiedProperty =
            DependencyProperty.Register(
                nameof(LastModified),
                typeof(string),
                typeof(SharePointFileList),
                null);

        private static readonly DependencyProperty IsDetailPaneVisibleProperty =
            DependencyProperty.Register(
                nameof(IsDetailPaneVisible),
                typeof(bool),
                typeof(SharePointFileList),
                new PropertyMetadata(false));

        private int _fileUploading;
        private string _errorMessage;
        private ICommand _backCommand;
        private ICommand _uploadCommand;
        private ICommand _shareCommand;
        private ICommand _downloadCommand;
        private ICommand _deleteCommand;
        private ICommand _showErrorDetailsCommand;
        private ICommand _loadMoreCommand;
        private ICommand _cancelCommand;

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

        /// <summary>
        /// Gets or sets the message when share link copied
        /// </summary>
        public string ShareLinkCopiedMessage
        {
            get { return (string)GetValue(ShareLinkCopiedMessageProperty); }
            set { SetValue(ShareLinkCopiedMessageProperty, value); }
        }

        /// <summary>
        /// Gets or sets the label of All Files
        /// </summary>
        public string AllFilesMessage
        {
            get { return (string)GetValue(AllFilesMessageProperty); }
            set { SetValue(AllFilesMessageProperty, value); }
        }

        /// <summary>
        /// Gets or sets the message of delete confirm dialog
        /// </summary>
        public string DeleteConfirmMessage
        {
            get { return (string)GetValue(DeleteConfirmMessageProperty); }
            set { SetValue(DeleteConfirmMessageProperty, value); }
        }

        /// <summary>
        /// Gets or sets the caption of ok button in delete confirm dialog
        /// </summary>
        public string DeleteConfirmOkMessage
        {
            get { return (string)GetValue(DeleteConfirmOkMessageProperty); }
            set { SetValue(DeleteConfirmOkMessageProperty, value); }
        }

        /// <summary>
        /// Gets or sets the caption of cancel button in delete confirm dialog
        /// </summary>
        public string DeleteConfirmCancelMessage
        {
            get { return (string)GetValue(DeleteConfirmCancelMessageProperty); }
            set { SetValue(DeleteConfirmCancelMessageProperty, value); }
        }

        /// <summary>
        /// Gets or sets the template of uploading files
        /// </summary>
        public string UploadingFilesMessageTemplate
        {
            get { return (string)GetValue(UploadingFilesMessageTemplateProperty); }
            set { SetValue(UploadingFilesMessageTemplateProperty, value); }
        }

        /// <summary>
        /// Gets the command to go back
        /// </summary>
        public ICommand BackCommand
        {
            get
            {
                if (_backCommand == null)
                {
                    _backCommand = new DelegateCommand(BackCommandAction);
                }

                return _backCommand;
            }
        }

        /// <summary>
        /// Gets the command to upload a new file
        /// </summary>
        public ICommand UploadCommand
        {
            get
            {
                if (_uploadCommand == null)
                {
                    _uploadCommand = new DelegateCommand(UploadCommandAction);
                }

                return _uploadCommand;
            }
        }

        /// <summary>
        /// Gets the command to delete the selected file
        /// </summary>
        public ICommand ShareCommand
        {
            get
            {
                if (_shareCommand == null)
                {
                    _shareCommand = new DelegateCommand(ShareCommandAction);
                }

                return _shareCommand;
            }
        }

        /// <summary>
        /// Gets the command to download the selected file
        /// </summary>
        public ICommand DownloadCommand
        {
            get
            {
                if (_downloadCommand == null)
                {
                    _downloadCommand = new DelegateCommand(DownloadCommandAction);
                }

                return _downloadCommand;
            }
        }

        /// <summary>
        /// Gets the command to delete the selected file
        /// </summary>
        public ICommand DeleteCommand
        {
            get
            {
                if (_deleteCommand == null)
                {
                    _deleteCommand = new DelegateCommand(DeleteCommandAction);
                }

                return _deleteCommand;
            }
        }

        /// <summary>
        /// Gets the command to show error messages
        /// </summary>
        public ICommand ShowErrorDetailsCommand
        {
            get
            {
                if (_showErrorDetailsCommand == null)
                {
                    _showErrorDetailsCommand = new DelegateCommand(ShowErrorDetailsCommandAction);
                }

                return _showErrorDetailsCommand;
            }
        }

        /// <summary>
        /// Gets the command to load more items
        /// </summary>
        public ICommand LoadMoreCommand
        {
            get
            {
                if (_loadMoreCommand == null)
                {
                    _loadMoreCommand = new DelegateCommand(LoadMoreCommandAction);
                }

                return _loadMoreCommand;
            }
        }

        /// <summary>
        /// Gets the command to cancel the file uploading
        /// </summary>
        public ICommand CancelCommand
        {
            get
            {
                if (_cancelCommand == null)
                {
                    _cancelCommand = new DelegateCommand(CancelCommandAction);
                }

                return _cancelCommand;
            }
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

        internal ImageSource ThumbnailImageSource
        {
            get { return (ImageSource)GetValue(ThumbnailImageSourceProperty); }
            set { SetValue(ThumbnailImageSourceProperty, value); }
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
            }
        }



        internal Visibility BackButtonVisibility
        {
            get { return (Visibility)GetValue(BackButtonVisibilityProperty); }
            set { SetValue(BackButtonVisibilityProperty, value); }
        }

        internal static readonly DependencyProperty BackButtonVisibilityProperty =
            DependencyProperty.Register(nameof(BackButtonVisibility), typeof(Visibility), typeof(SharePointFileList), new PropertyMetadata(Visibility.Collapsed));

    }
}
