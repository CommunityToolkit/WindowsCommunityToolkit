// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Microsoft.Graph;
using Microsoft.Toolkit.Services.MicrosoftGraph;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media;

namespace Microsoft.Toolkit.Uwp.UI.Controls.Graph
{
    /// <summary>
    /// The SharePointFiles Control displays a simple list of SharePoint Files.
    /// </summary>
    public partial class SharePointFileList
    {
        /// <summary>
        /// Gets the <see cref="MicrosoftGraphService"/> instance
        /// </summary>
        public static MicrosoftGraphService GraphService => MicrosoftGraphService.Instance;

        /// <summary>
        /// Gets required delegated permissions for the <see cref="SharePointFileList"/> control
        /// </summary>
        public static string[] RequiredDelegatedPermissions
        {
            get
            {
                return new string[] { "User.Read", "Files.ReadWrite.All" };
            }
        }

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

        internal static readonly DependencyProperty BackButtonVisibilityProperty =
            DependencyProperty.Register(
                nameof(BackButtonVisibility),
                typeof(Visibility),
                typeof(SharePointFileList),
                new PropertyMetadata(Visibility.Collapsed));

        internal static readonly DependencyProperty StatusMessageProperty =
            DependencyProperty.Register(
                nameof(StatusMessage),
                typeof(string),
                typeof(SharePointFileList),
                new PropertyMetadata(string.Empty));

        private static readonly DependencyProperty IsDetailPaneVisibleProperty =
            DependencyProperty.Register(
                nameof(IsDetailPaneVisible),
                typeof(bool),
                typeof(SharePointFileList),
                new PropertyMetadata(false));

        internal static readonly DependencyProperty CurrentPathProperty =
            DependencyProperty.Register(
                nameof(CurrentPath),
                typeof(string),
                typeof(SharePointFileList),
                new PropertyMetadata(string.Empty));

        /// <summary>
        /// Gets or sets drive or SharePoint document library URL to display
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

        internal string StatusMessage
        {
            get { return (string)GetValue(StatusMessageProperty); }
            set { SetValue(StatusMessageProperty, value); }
        }

        internal Visibility BackButtonVisibility
        {
            get { return (Visibility)GetValue(BackButtonVisibilityProperty); }
            set { SetValue(BackButtonVisibilityProperty, value); }
        }

        internal string CurrentPath
        {
            get { return (string)GetValue(CurrentPathProperty); }
            set { SetValue(CurrentPathProperty, value); }
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

        private int FileUploading { get; set; }

        private string ErrorMessage { get; set; }
    }
}
