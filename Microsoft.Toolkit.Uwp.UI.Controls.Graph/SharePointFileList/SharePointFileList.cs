// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Graph;
using Newtonsoft.Json;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Microsoft.Toolkit.Uwp.UI.Controls.Graph
{
    /// <summary>
    /// The SharePointFiles Control displays a simple list of SharePoint Files.
    /// </summary>
    [TemplatePart(Name = ControlFileList, Type = typeof(ListView))]
    [TemplatePart(Name = ControlBack, Type = typeof(Button))]
    [TemplatePart(Name = ControlCancel, Type = typeof(Button))]
    [TemplatePart(Name = ControlDelete, Type = typeof(Button))]
    [TemplatePart(Name = ControlDownload, Type = typeof(Button))]
    [TemplatePart(Name = ControlLoadMore, Type = typeof(Button))]
    [TemplatePart(Name = ControlShare, Type = typeof(Button))]
    [TemplatePart(Name = ControlUpload, Type = typeof(Button))]
    [TemplatePart(Name = ControlError, Type = typeof(HyperlinkButton))]
    [TemplateVisualState(Name = UploadStatusNotUploading, GroupName = UploadStatus)]
    [TemplateVisualState(Name = UploadStatusUploading, GroupName = UploadStatus)]
    [TemplateVisualState(Name = UploadStatusError, GroupName = UploadStatus)]
    [TemplateVisualState(Name = DetailPaneStatesHide, GroupName = DetailPaneStates)]
    [TemplateVisualState(Name = DetailPaneStatesSide, GroupName = DetailPaneStates)]
    [TemplateVisualState(Name = DetailPaneStatesBottom, GroupName = DetailPaneStates)]
    [TemplateVisualState(Name = DetailPaneStatesFull, GroupName = DetailPaneStates)]
    [TemplateVisualState(Name = NavStatesFolderReadonly, GroupName = NavStates)]
    [TemplateVisualState(Name = NavStatesFolderEdit, GroupName = NavStates)]
    [TemplateVisualState(Name = NavStatesFileReadonly, GroupName = NavStates)]
    [TemplateVisualState(Name = NavStatesFileEdit, GroupName = NavStates)]
    public partial class SharePointFileList : Control
    {
        /// <summary>
        /// File is selected
        /// </summary>
        public event EventHandler<FileSelectedEventArgs> FileSelected;

        private string _driveId;
        private string _driveName;
        private Stack<DriveItem> _driveItemPath = new Stack<DriveItem>();
        private string _pathVisualState = string.Empty;
        private IDriveItemChildrenCollectionRequest _nextPageRequest = null;
        private CancellationTokenSource _cancelUpload = new CancellationTokenSource();
        private CancellationTokenSource _cancelLoadFile = new CancellationTokenSource();
        private CancellationTokenSource _cancelGetDetails = new CancellationTokenSource();
        private ListView _list;

        /// <summary>
        /// Initializes a new instance of the <see cref="SharePointFileList"/> class.
        /// </summary>
        public SharePointFileList()
        {
            DefaultStyleKey = typeof(SharePointFileList);
        }

        /// <summary>
        /// Called when applying the control template.
        /// </summary>
        protected override void OnApplyTemplate()
        {
            _list = GetTemplateChild(ControlFileList) as ListView;
            if (_list != null)
            {
                _list.SelectionChanged += List_SelectionChanged;
                _list.ItemClick += List_ItemClick;
            }

            Button back = GetTemplateChild(ControlBack) as Button;
            if (back != null)
            {
                back.Click += Back_Click;
            }

            Button cancel = GetTemplateChild(ControlCancel) as Button;
            if (cancel != null)
            {
                cancel.Click += Cancel_Click;
            }

            Button delete = GetTemplateChild(ControlDelete) as Button;
            if (delete != null)
            {
                delete.Click += Delete_Click;
            }

            Button download = GetTemplateChild(ControlDownload) as Button;
            if (download != null)
            {
                download.Click += Download_Click;
            }

            Button loadMore = GetTemplateChild(ControlLoadMore) as Button;
            if (loadMore != null)
            {
                loadMore.Click += LoadMore_Click;
            }

            Button share = GetTemplateChild(ControlShare) as Button;
            if (share != null)
            {
                share.Click += Share_Click;
            }

            Button upload = GetTemplateChild(ControlUpload) as Button;
            if (upload != null)
            {
                upload.Click += Upload_Click;
            }

            HyperlinkButton error = GetTemplateChild(ControlError) as HyperlinkButton;
            if (error != null)
            {
                error.Click += ShowErrorDetails_Click;
            }

            base.OnApplyTemplate();
        }

        /// <summary>
        /// Retrieves an appropriate Drive URL from a SharePoint document library root URL
        /// </summary>
        /// <param name="rawDocLibUrl">Raw URL for SharePoint document library</param>
        /// <returns>Drive URL</returns>
        public async Task<string> GetDriveUrlFromSharePointUrlAsync(string rawDocLibUrl)
        {
            if (string.IsNullOrEmpty(rawDocLibUrl))
            {
                return rawDocLibUrl;
            }

            rawDocLibUrl = WebUtility.UrlDecode(rawDocLibUrl);

            Match match = Regex.Match(rawDocLibUrl, @"(https?://([^/]+)((/[^/?]+)*?)(/[^/?]+))(/(Forms/\w+.aspx)?)?(\?.*)?$", RegexOptions.IgnoreCase);
            string docLibUrl = match.Groups[1].Value;
            string hostName = match.Groups[2].Value;
            string siteRelativePath = match.Groups[3].Value;
            if (string.IsNullOrEmpty(siteRelativePath))
            {
                siteRelativePath = "/";
            }

            if (await GraphService.TryLoginAsync())
            {
                GraphServiceClient graphServiceClient = GraphService.GraphProvider;

                Site site = await graphServiceClient.Sites.GetByPath(siteRelativePath, hostName).Request().GetAsync();
                ISiteDrivesCollectionPage drives = await graphServiceClient.Sites[site.Id].Drives.Request().GetAsync();

                Drive drive = drives.SingleOrDefault(o => WebUtility.UrlDecode(o.WebUrl).Equals(docLibUrl, StringComparison.CurrentCultureIgnoreCase));
                if (drive == null)
                {
                    throw new Exception("Drive not found");
                }

                return graphServiceClient.Drives[drive.Id].RequestUrl;
            }

            return rawDocLibUrl;
        }

        private async Task InitDriveAsync(string driveUrl)
        {
            try
            {
                string realDriveURL;
                if (driveUrl.StartsWith("https://graph.microsoft.com/", StringComparison.CurrentCultureIgnoreCase))
                {
                    realDriveURL = driveUrl;
                }
                else
                {
                    realDriveURL = await GetDriveUrlFromSharePointUrlAsync(driveUrl);
                }

                await GraphService.TryLoginAsync();
                GraphServiceClient graphServiceClient = GraphService.GraphProvider;
                HttpRequestMessage message = new HttpRequestMessage(HttpMethod.Get, realDriveURL);
                await graphServiceClient.AuthenticationProvider.AuthenticateRequestAsync(message);

                HttpResponseMessage result = await graphServiceClient.HttpProvider.SendAsync(message);
                if (result.StatusCode == HttpStatusCode.OK)
                {
                    string json = await result.Content.ReadAsStringAsync();
                    Drive drive = JsonConvert.DeserializeObject<Drive>(json);
                    if (drive != null)
                    {
                        _driveId = drive.Id;
                        _driveName = drive.Name;
                        _driveItemPath.Clear();
                        DriveItem rootDriveItem = await graphServiceClient.Drives[_driveId].Root.Request().GetAsync();
                        _driveItemPath.Push(rootDriveItem);
                        UpdateCurrentPath();
                        await LoadFilesAsync(rootDriveItem.Id);
                        BackButtonVisibility = Visibility.Collapsed;
                    }
                }
            }
            catch (Exception)
            {
            }
        }

        private async Task LoadFilesAsync(string driveItemId, int pageIndex = 0)
        {
            IsDetailPaneVisible = false;
            HideDetailsPane();
            if (!string.IsNullOrEmpty(_driveId))
            {
                try
                {
                    _cancelLoadFile.Cancel(false);
                    _cancelLoadFile.Dispose();
                    _cancelLoadFile = new CancellationTokenSource();
                    _list.Items.Clear();
                    VisualStateManager.GoToState(this, NavStatesFolderReadonly, false);
                    QueryOption queryOption = new QueryOption("$top", PageSize.ToString());

                    await GraphService.TryLoginAsync();
                    GraphServiceClient graphServiceClient = GraphService.GraphProvider;
                    Task<IDriveItemChildrenCollectionPage> taskFiles = graphServiceClient.Drives[_driveId].Items[driveItemId].Children.Request(new List<Option> { queryOption }).GetAsync(_cancelLoadFile.Token);
                    IDriveItemChildrenCollectionPage files = await taskFiles;
                    if (!taskFiles.IsCanceled)
                    {
                        _list.Items.Clear();
                        foreach (DriveItem file in files)
                        {
                            _list.Items.Add(file);
                        }

                        _nextPageRequest = files.NextPageRequest;
                        HasMore = _nextPageRequest != null;
                        VisualStateManager.GoToState(this, NavStatesFolderReadonly, false);
                        _pathVisualState = NavStatesFolderReadonly;
                        if (_driveItemPath.Count > 1)
                        {
                            IDriveItemPermissionsCollectionPage permissions = await graphServiceClient.Drives[_driveId].Items[driveItemId].Permissions.Request().GetAsync();
                            foreach (Permission permission in permissions)
                            {
                                if (permission.Roles.Contains("write") || permission.Roles.Contains("owner"))
                                {
                                    VisualStateManager.GoToState(this, NavStatesFolderEdit, false);
                                    _pathVisualState = NavStatesFolderEdit;
                                    break;
                                }
                            }
                        }
                        else
                        {
                            _pathVisualState = NavStatesFolderEdit;
                            VisualStateManager.GoToState(this, NavStatesFolderEdit, false);
                        }
                    }
                }
                catch (Exception)
                {
                }
            }
        }

        private async Task LoadNextPageAsync()
        {
            try
            {
                if (_nextPageRequest != null)
                {
                    Task<IDriveItemChildrenCollectionPage> taskItems = _nextPageRequest.GetAsync(_cancelLoadFile.Token);
                    IDriveItemChildrenCollectionPage items = await taskItems;
                    if (!taskItems.IsCanceled)
                    {
                        foreach (DriveItem item in items)
                        {
                            _list.Items.Add(item);
                        }

                        _nextPageRequest = items.NextPageRequest;
                        HasMore = _nextPageRequest != null;
                    }
                }
            }
            catch
            {
            }
        }

        private void HideDetailsPane()
        {
            VisualStateManager.GoToState(this, DetailPaneStatesHide, false);
            if (_driveItemPath.Count <= 1)
            {
                BackButtonVisibility = Visibility.Collapsed;
            }
        }

        private void UpdateCurrentPath()
        {
           CurrentPath = _driveName + " > " + string.Join(" > ", _driveItemPath.Select(s => s.Name).Reverse().Skip(1));
        }

        private void ShowDetailsPane()
        {
            switch (DetailPane)
            {
                case DetailPaneDisplayMode.Side:
                    if (_driveItemPath.Count <= 1)
                    {
                        BackButtonVisibility = Visibility.Collapsed;
                    }

                    VisualStateManager.GoToState(this, DetailPaneStatesSide, false);
                    break;
                case DetailPaneDisplayMode.Bottom:
                    if (_driveItemPath.Count <= 1)
                    {
                        BackButtonVisibility = Visibility.Collapsed;
                    }

                    VisualStateManager.GoToState(this, DetailPaneStatesBottom, false);
                    break;
                case DetailPaneDisplayMode.Full:
                    BackButtonVisibility = Visibility.Visible;
                    VisualStateManager.GoToState(this, DetailPaneStatesFull, false);
                    break;
                default:
                    HideDetailsPane();
                    break;
            }
        }
    }
}
