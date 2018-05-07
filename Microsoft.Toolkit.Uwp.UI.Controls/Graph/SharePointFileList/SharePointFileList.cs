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
    [TemplatePart(Name = "list", Type = typeof(ListView))]
    public partial class SharePointFileList : Control
    {
        /// <summary>
        /// File is selected
        /// </summary>
        public event EventHandler<FileSelectedEventArgs> FileSelected;

        private GraphServiceClient _graphClient;
        private string _driveId;
        private Stack<string> _driveItemPath = new Stack<string>();
        private IDriveItemChildrenCollectionRequest _nextPageRequest = null;
        private CancellationTokenSource _cancelUpload = new CancellationTokenSource();
        private CancellationTokenSource _cancelLoadFile = new CancellationTokenSource();
        private CancellationTokenSource _cancelGetDetails = new CancellationTokenSource();

        private ListView _list;
        private Button _back;
        private Button _upload;
        private Button _share;
        private Button _download;
        private Button _delete;
        private HyperlinkButton _error;
        private Button _cancel;
        private Button _hasMore;
        private Grid _thumbnail;
        private ScrollViewer _details;
        private TextBlock _status;

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
            _list = GetTemplateChild("list") as ListView;
            if (_list != null)
            {
                _list.SelectionChanged += List_SelectionChanged;
                _list.ItemClick += List_ItemClick;
            }

            _back = GetTemplateChild("back") as Button;
            _upload = GetTemplateChild("upload") as Button;
            _share = GetTemplateChild("share") as Button;
            _download = GetTemplateChild("download") as Button;
            _delete = GetTemplateChild("delete") as Button;
            _error = GetTemplateChild("error") as HyperlinkButton;
            _cancel = GetTemplateChild("cancel") as Button;
            _hasMore = GetTemplateChild("hasMore") as Button;
            _thumbnail = GetTemplateChild("thumbnail") as Grid;
            _details = GetTemplateChild("details") as ScrollViewer;
            _status = GetTemplateChild("status") as TextBlock;

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

            Site site = await _graphClient.Sites.GetByPath(siteRelativePath, hostName).Request().GetAsync();
            ISiteDrivesCollectionPage drives = await _graphClient.Sites[site.Id].Drives.Request().GetAsync();

            Drive drive = drives.SingleOrDefault(o => WebUtility.UrlDecode(o.WebUrl).Equals(docLibUrl, StringComparison.CurrentCultureIgnoreCase));
            if (drive == null)
            {
                throw new Exception("Drive not found");
            }

            return _graphClient.Drives[drive.Id].RequestUrl;
        }

        private async Task InitDriveAsync(string driveUrl)
        {
            try
            {
                HttpRequestMessage message = new HttpRequestMessage(HttpMethod.Get, driveUrl);
                await _graphClient.AuthenticationProvider.AuthenticateRequestAsync(message);

                HttpResponseMessage result = await _graphClient.HttpProvider.SendAsync(message);
                if (result.StatusCode == HttpStatusCode.OK)
                {
                    string json = await result.Content.ReadAsStringAsync();
                    Drive drive = JsonConvert.DeserializeObject<Drive>(json);
                    if (drive != null)
                    {
                        _driveId = drive.Id;
                        _driveItemPath.Clear();
                        DriveItem rootDriveItem = await _graphClient.Drives[_driveId].Root.Request().GetAsync();
                        _driveItemPath.Push(rootDriveItem.Id);
                        await LoadFilesAsync(rootDriveItem.Id);
                        _back.Visibility = Visibility.Collapsed;
                    }
                }
            }
            catch
            {
            }
        }

        private async Task LoadFilesAsync(string driveItemId, int pageIndex = 0)
        {
            IsDetailPaneVisible = false;
            if (!string.IsNullOrEmpty(_driveId))
            {
                try
                {
                    _cancelLoadFile.Cancel(false);
                    _cancelLoadFile.Dispose();
                    _cancelLoadFile = new CancellationTokenSource();
                    _list.Items.Clear();
                    _download.Visibility = Visibility.Collapsed;
                    _share.Visibility = Visibility.Collapsed;
                    _delete.Visibility = Visibility.Collapsed;
                    QueryOption queryOption = new QueryOption("$top", PageSize.ToString());
                    Task<IDriveItemChildrenCollectionPage> taskFiles = _graphClient.Drives[_driveId].Items[driveItemId].Children.Request(new List<Option> { queryOption }).GetAsync(_cancelLoadFile.Token);
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
                        _upload.Visibility = Visibility.Collapsed;
                        if (_driveItemPath.Count > 1)
                        {
                            IDriveItemPermissionsCollectionPage permissions = await _graphClient.Drives[_driveId].Items[driveItemId].Permissions.Request().GetAsync();
                            foreach (Permission permission in permissions)
                            {
                                if (permission.Roles.Contains("write") || permission.Roles.Contains("owner"))
                                {
                                    _upload.Visibility = Visibility.Visible;
                                    break;
                                }
                            }
                        }
                        else
                        {
                            _upload.Visibility = Visibility.Visible;
                        }
                    }

                    if (_list.Items.Count > 0)
                    {
                        _list.SelectedIndex = 0;
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
            _list.Visibility = Visibility.Visible;
            _list.SetValue(Grid.RowSpanProperty, 3);
            _list.SetValue(Grid.ColumnSpanProperty, 3);
            _thumbnail.Visibility = Visibility.Collapsed;
            _details.Visibility = Visibility.Collapsed;
            if (_driveItemPath.Count <= 1)
            {
                _back.Visibility = Visibility.Collapsed;
            }
        }

        private void ShowDetailsPane()
        {
            _thumbnail.Visibility = Visibility.Visible;
            _details.Visibility = Visibility.Visible;
            switch (DetailPane)
            {
                case DetailPaneDisplayMode.Side:
                    if (_driveItemPath.Count <= 1)
                    {
                        _back.Visibility = Visibility.Collapsed;
                    }

                    _list.Visibility = Visibility.Visible;
                    _list.SetValue(Grid.RowSpanProperty, 3);
                    _list.SetValue(Grid.ColumnSpanProperty, 2);
                    _thumbnail.SetValue(Grid.RowProperty, 1);
                    _thumbnail.SetValue(Grid.RowSpanProperty, 1);
                    _thumbnail.SetValue(Grid.ColumnProperty, 2);
                    _thumbnail.SetValue(Grid.ColumnSpanProperty, 1);
                    _details.SetValue(Grid.RowProperty, 2);
                    _details.SetValue(Grid.RowSpanProperty, 2);
                    _details.SetValue(Grid.ColumnProperty, 2);
                    _details.SetValue(Grid.ColumnSpanProperty, 1);
                    break;
                case DetailPaneDisplayMode.Bottom:
                    if (_driveItemPath.Count <= 1)
                    {
                        _back.Visibility = Visibility.Collapsed;
                    }

                    _list.Visibility = Visibility.Visible;
                    _list.SetValue(Grid.RowSpanProperty, 2);
                    _list.SetValue(Grid.ColumnSpanProperty, 3);
                    _thumbnail.SetValue(Grid.RowProperty, 3);
                    _thumbnail.SetValue(Grid.RowSpanProperty, 1);
                    _thumbnail.SetValue(Grid.ColumnProperty, 0);
                    _thumbnail.SetValue(Grid.ColumnSpanProperty, 1);
                    _details.SetValue(Grid.RowProperty, 3);
                    _details.SetValue(Grid.RowSpanProperty, 1);
                    _details.SetValue(Grid.ColumnProperty, 1);
                    _details.SetValue(Grid.ColumnSpanProperty, 2);
                    break;
                case DetailPaneDisplayMode.Full:
                    _back.Visibility = Visibility.Visible;
                    _list.Visibility = Visibility.Collapsed;
                    _thumbnail.SetValue(Grid.RowProperty, 1);
                    _thumbnail.SetValue(Grid.RowSpanProperty, 3);
                    _thumbnail.SetValue(Grid.ColumnProperty, 0);
                    _thumbnail.SetValue(Grid.ColumnSpanProperty, 1);
                    _details.SetValue(Grid.RowProperty, 1);
                    _details.SetValue(Grid.RowSpanProperty, 3);
                    _details.SetValue(Grid.ColumnProperty, 1);
                    _details.SetValue(Grid.ColumnSpanProperty, 2);
                    break;
                default:
                    HideDetailsPane();
                    break;
            }
        }
    }
}
