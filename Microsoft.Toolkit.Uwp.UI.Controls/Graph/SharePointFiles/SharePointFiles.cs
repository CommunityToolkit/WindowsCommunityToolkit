using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Graph;
using Newtonsoft.Json;
using Windows.ApplicationModel.DataTransfer;
using Windows.Storage;
using Windows.Storage.Pickers;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Imaging;

namespace Microsoft.Toolkit.Uwp.UI.Controls.Graph
{
    /// <summary>
    /// The SharePointFiles Control displays a simple list of SharePoint Files.
    /// </summary>
    public partial class SharePointFiles : Control
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
        private Windows.UI.Xaml.Controls.Image _thumbnailImage;
        private ScrollViewer _details;
        private TextBlock _status;

        /// <summary>
        /// Initializes a new instance of the <see cref="SharePointFiles"/> class.
        /// </summary>
        public SharePointFiles()
        {
            DefaultStyleKey = typeof(SharePointFiles);
        }

        private static async void GraphAccessTokenPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            SharePointFiles control = d as SharePointFiles;
            control._graphClient = Common.GetAuthenticatedClient(control.GraphAccessToken);
            if (!string.IsNullOrEmpty(control.DriveUrl))
            {
                await control.InitDrive(control.DriveUrl);
            }
        }

        private static async void DriveUrlPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            SharePointFiles control = d as SharePointFiles;
            if (control._graphClient != null && !string.IsNullOrWhiteSpace(control.DriveUrl))
            {
                if (Uri.IsWellFormedUriString(control.DriveUrl, UriKind.Absolute))
                {
                    await control.InitDrive(control.DriveUrl);
                }
            }
        }

        private static void DetailPanePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            SharePointFiles control = d as SharePointFiles;
            if (control.IsDetailPaneVisible)
            {
                control.ShowDetailsPane();
            }
        }

        /// <summary>
        /// Called when applying the control template.
        /// </summary>
        protected override void OnApplyTemplate()
        {
            _list = GetTemplateChild("list") as ListView;
            _back = GetTemplateChild("back") as Button;
            _upload = GetTemplateChild("upload") as Button;
            _share = GetTemplateChild("share") as Button;
            _download = GetTemplateChild("download") as Button;
            _delete = GetTemplateChild("delete") as Button;
            _error = GetTemplateChild("error") as HyperlinkButton;
            _cancel = GetTemplateChild("cancel") as Button;
            _hasMore = GetTemplateChild("hasMore") as Button;
            _thumbnail = GetTemplateChild("thumbnail") as Grid;
            _thumbnailImage = GetTemplateChild("thumbnailImage") as Windows.UI.Xaml.Controls.Image;
            _details = GetTemplateChild("details") as ScrollViewer;
            _status = GetTemplateChild("status") as TextBlock;
            _list.SelectionChanged += List_SelectionChanged;
            _list.ItemClick += List_ItemClick;
            _back.Click += Back_Click;
            _upload.Click += Upload_Click;
            _share.Click += Share_Click;
            _download.Click += Download_Click;
            _delete.Click += Delete_Click;
            _error.Click += Error_Click;
            _cancel.Click += Cancel_Click;
            _hasMore.Click += LoadMore_Click;

            base.OnApplyTemplate();
        }

        private async void List_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (e.ClickedItem is DriveItem driveItem && driveItem.Folder != null)
            {
                _driveItemPath.Push(driveItem.Id);
                _back.Visibility = Visibility.Visible;
                await LoadFiles(driveItem.Id);
            }
        }

        /// <summary>
        /// Retrieves an appropriate Drive URL from a SharePoint document library root URL
        /// </summary>
        /// <param name="rawDocLibUrl">Raw URL for SharePoint document library</param>
        /// <returns>Drive URL</returns>
        public async Task<string> GetDriveUrlFromSharePointUrl(string rawDocLibUrl)
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

        private async Task InitDrive(string driveUrl)
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
                        await LoadFiles(rootDriveItem.Id);
                        _back.Visibility = Visibility.Collapsed;
                    }
                }
            }
            catch
            {
            }
        }

        private async Task LoadFiles(string driveItemId, int pageIndex = 0)
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

        private async Task LoadNextPage()
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

        private async void Back_Click(object sender, RoutedEventArgs e)
        {
            if (DetailPane == DetailPaneDisplayMode.Full && _thumbnail.Visibility == Visibility.Visible)
            {
                HideDetailsPane();
            }
            else if (_driveItemPath.Count > 1)
            {
                _driveItemPath.Pop();
                string parentItemId = _driveItemPath.Peek();
                if (_driveItemPath.Count == 1)
                {
                    _back.Visibility = Visibility.Collapsed;
                }

                await LoadFiles(parentItemId);
            }
        }

        private async void Upload_Click(object sender, RoutedEventArgs e)
        {
            ErrorMessage = string.Empty;
            FileOpenPicker picker = new FileOpenPicker();
            picker.FileTypeFilter.Add("*");
            StorageFile file = await picker.PickSingleFileAsync();
            if (file != null)
            {
                string driveItemId = _driveItemPath.Peek();
                using (Stream inputStream = await file.OpenStreamForReadAsync())
                {
                    if (inputStream.Length < 1024 * 1024 * 4)
                    {
                        FileUploading++;

                        try
                        {
                            await _graphClient.Drives[_driveId].Items[driveItemId].ItemWithPath(file.Name).Content.Request().PutAsync<DriveItem>(inputStream, _cancelUpload.Token);
                            FileUploading--;
                        }
                        catch (Exception ex)
                        {
                            FileUploading--;
                            ErrorMessage = ex.Message;
                        }

                        await LoadFiles(driveItemId);
                    }
                }
            }
        }

        private async void Share_Click(object sender, RoutedEventArgs e)
        {
            if (_list.SelectedItem is DriveItem driveItem)
            {
                Permission link = await _graphClient.Drives[_driveId].Items[driveItem.Id].CreateLink("view", "organization").Request().PostAsync();
                MessageDialog dialog = new MessageDialog(link.Link.WebUrl, "Shared link copied");
                DataPackage package = new DataPackage();
                package.SetText(link.Link.WebUrl);
                Clipboard.SetContent(package);
                await dialog.ShowAsync();
            }
        }

        private async void Download_Click(object sender, RoutedEventArgs e)
        {
            if (_list.SelectedItem is DriveItem driveItem)
            {
                FileSavePicker picker = new FileSavePicker();
                picker.FileTypeChoices.Add("All Files", new List<string>() { driveItem.Name.Substring(driveItem.Name.LastIndexOf(".")) });
                picker.SuggestedFileName = driveItem.Name;
                picker.SuggestedStartLocation = PickerLocationId.DocumentsLibrary;
                StorageFile file = await picker.PickSaveFileAsync();
                if (file != null)
                {
                    using (Stream inputStream = await _graphClient.Drives[_driveId].Items[driveItem.Id].Content.Request().GetAsync())
                    {
                        using (Stream outputStream = await file.OpenStreamForWriteAsync())
                        {
                            await inputStream.CopyToAsync(outputStream);
                        }
                    }
                }
            }
        }

        private async void Delete_Click(object sender, RoutedEventArgs e)
        {
            if (_list.SelectedItem is DriveItem driveItem)
            {
                MessageDialog confirmDialog = new MessageDialog("Do you want to delete this file?");
                confirmDialog.Commands.Add(new UICommand("OK", cmd => { }, commandId: 0));
                confirmDialog.Commands.Add(new UICommand("Cancel", cmd => { }, commandId: 1));

                confirmDialog.DefaultCommandIndex = 0;
                confirmDialog.CancelCommandIndex = 1;

                IUICommand result = await confirmDialog.ShowAsync();

                if ((int)result.Id == 0)
                {
                    await _graphClient.Drives[_driveId].Items[driveItem.Id].Request().DeleteAsync();
                    string driveItemId = _driveItemPath.Peek();
                    await LoadFiles(driveItemId);
                }
            }
        }

        private async void List_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (_list.SelectedItem is DriveItem driveItem)
            {
                _cancelGetDetails.Cancel(false);
                _cancelGetDetails.Dispose();
                _cancelGetDetails = new CancellationTokenSource();
                if (driveItem.File != null)
                {
                    try
                    {
                        SelectedFile = driveItem;
                        FileSize = driveItem.Size ?? 0;
                        LastModified = driveItem.LastModifiedDateTime?.LocalDateTime.ToString() ?? string.Empty;
                        if (FileSelected != null)
                        {
                            FileSelected.Invoke(this, new FileSelectedEventArgs() { FileSelected = driveItem });
                        }

                        _thumbnailImage.Source = null;
                        _download.Visibility = Visibility.Visible;
                        _share.Visibility = Visibility.Collapsed;
                        _delete.Visibility = Visibility.Collapsed;
                        Task<IDriveItemPermissionsCollectionPage> taskPermissions = _graphClient.Drives[_driveId].Items[driveItem.Id].Permissions.Request().GetAsync(_cancelGetDetails.Token);
                        IDriveItemPermissionsCollectionPage permissions = await taskPermissions;
                        if (!taskPermissions.IsCanceled)
                        {
                            foreach (Permission permission in permissions)
                            {
                                if (permission.Roles.Contains("write") || permission.Roles.Contains("owner"))
                                {
                                    _share.Visibility = Visibility.Visible;
                                    _delete.Visibility = Visibility.Visible;
                                    break;
                                }
                            }

                            IsDetailPaneVisible = true;
                            Task<IDriveItemThumbnailsCollectionPage> taskThumbnails = _graphClient.Drives[_driveId].Items[driveItem.Id].Thumbnails.Request().GetAsync(_cancelGetDetails.Token);
                            IDriveItemThumbnailsCollectionPage thumbnails = await taskThumbnails;
                            if (!taskThumbnails.IsCanceled)
                            {
                                ThumbnailSet thumbnailsSet = thumbnails.FirstOrDefault();
                                if (thumbnailsSet != null)
                                {
                                    _thumbnailImage.Source = new BitmapImage(new Uri(thumbnailsSet.Large.Url));
                                }
                            }
                        }
                    }
                    catch
                    {
                    }
                }
                else
                {
                    SelectedFile = null;
                    FileSize = 0;
                    LastModified = string.Empty;
                    _download.Visibility = Visibility.Collapsed;
                    _share.Visibility = Visibility.Collapsed;
                    _delete.Visibility = Visibility.Collapsed;
                    IsDetailPaneVisible = false;
                }
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

        private async void LoadMore_Click(object sender, RoutedEventArgs e)
        {
            await LoadNextPage();
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            _cancelUpload.Cancel(false);
            _cancelUpload.Dispose();
            _cancelUpload = new CancellationTokenSource();
        }

        private async void Error_Click(object sender, RoutedEventArgs e)
        {
            MessageDialog messageDialog = new MessageDialog(ErrorMessage, "Errors adding files.");
            await messageDialog.ShowAsync();
        }
    }
}
