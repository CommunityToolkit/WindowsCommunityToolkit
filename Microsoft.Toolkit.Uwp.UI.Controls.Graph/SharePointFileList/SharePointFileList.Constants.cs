// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

namespace Microsoft.Toolkit.Uwp.UI.Controls.Graph
{
    /// <summary>
    /// The SharePointFiles Control displays a simple list of SharePoint Files.
    /// </summary>
    public partial class SharePointFileList
    {
        /// <summary>
        /// Key of the VisualStateGroup to control nav buttons
        /// </summary>
        private const string NavStates = "NavStates";

        /// <summary>
        /// Key of the VisualState when display folder in readonly mode
        /// </summary>
        private const string NavStatesFolderReadonly = "FolderReadOnly";

        /// <summary>
        /// Key of the VisualState when display folder in edit mode
        /// </summary>
        private const string NavStatesFolderEdit = "FolderEdit";

        /// <summary>
        /// Key of the VisualState when display file in readonly mode
        /// </summary>
        private const string NavStatesFileReadonly = "FileReadonly";

        /// <summary>
        /// Key of the VisualState when display file in edit mode
        /// </summary>
        private const string NavStatesFileEdit = "FileEdit";

        /// <summary>
        /// Key of the VisualStateGroup to control uploading status
        /// </summary>
        private const string UploadStatus = "UploadStatus";

        /// <summary>
        /// Key of the VisualState when not uploading files
        /// </summary>
        private const string UploadStatusNotUploading = "NotUploading";

        /// <summary>
        /// Key of the VisualState when uploading files
        /// </summary>
        private const string UploadStatusUploading = "Uploading";

        /// <summary>
        /// Key of the VisualState when uploading error occurs
        /// </summary>
        private const string UploadStatusError = "Error";

        /// <summary>
        /// Key of the VisualStateGroup to control detail pane
        /// </summary>
        private const string DetailPaneStates = "DetailPaneStates";

        /// <summary>
        /// Key of the VisualState when detail pane is hidden
        /// </summary>
        private const string DetailPaneStatesHide = "Hide";

        /// <summary>
        /// Key of the VisualState when detail pane is at right side
        /// </summary>
        private const string DetailPaneStatesSide = "Side";

        /// <summary>
        /// Key of the VisualState when detail pane is at bottom
        /// </summary>
        private const string DetailPaneStatesBottom = "Bottom";

        /// <summary>
        /// Key of the VisualState when detail pane is in full mode
        /// </summary>
        private const string DetailPaneStatesFull = "Full";

        /// <summary>
        /// Key of the ListView that contains file list
        /// </summary>
        private const string ControlFileList = "list";

        /// <summary>
        /// Key of the back button that contains file list
        /// </summary>
        private const string ControlBack = "back";

        /// <summary>
        /// Key of the upload button that contains file list
        /// </summary>
        private const string ControlUpload = "upload";

        /// <summary>
        /// Key of the share button that contains file list
        /// </summary>
        private const string ControlShare = "share";

        /// <summary>
        /// Key of the download button that contains file list
        /// </summary>
        private const string ControlDownload = "download";

        /// <summary>
        /// Key of the delete button that contains file list
        /// </summary>
        private const string ControlDelete = "delete";

        /// <summary>
        /// Key of the error button that contains file list
        /// </summary>
        private const string ControlError = "error";

        /// <summary>
        /// Key of the cancel button that contains file list
        /// </summary>
        private const string ControlCancel = "cancel";

        /// <summary>
        /// Key of the has more button that contains file list
        /// </summary>
        private const string ControlLoadMore = "hasMore";
    }
}
