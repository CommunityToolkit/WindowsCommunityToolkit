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
        private const string FileList = "list";
    }
}
