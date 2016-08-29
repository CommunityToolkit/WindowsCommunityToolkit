namespace Microsoft.Toolkit.Uwp.SampleApp.SamplePages
{
    using System;
    using System.IO;
    using Microsoft.Toolkit.Uwp.Services.MicrosoftGraph;
    using Windows.UI.Popups;
    using Windows.UI.Xaml;
    using Windows.UI.Xaml.Controls;
    using Windows.UI.Xaml.Media.Imaging;
    using Windows.Storage.Streams;
    using System.Threading;
    using System.Collections.ObjectModel;
    using System.Threading.Tasks;
    using Windows.UI.Core;

    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MicrosoftGraphPage : Page
    {
        public MicrosoftGraphPage()
        {
            this.InitializeComponent();
            MessagesBox.Visibility = Visibility.Collapsed;
            UserBox.Visibility = Visibility.Collapsed;
        }

        private async void ConnectButton_Click(object sender, RoutedEventArgs e)
        {
            if (!await Tools.CheckInternetConnection())
            {
                return;
            }

            // Initialize the service
            MicrosoftGraphService.Instance.Initialize(ClientId.Text);

            // Login via Azure Active Directory
            if (!await MicrosoftGraphService.Instance.LoginAsync())
            {
                var error = new MessageDialog("Unable to login to Office 365");
                await error.ShowAsync();
                return;
            }

            Shell.Current.DisplayWaitRing = true;

            // Retrieve user's info from Azure Active Directory
            // MicrosoftGraphUserFields[] selectFields = { MicrosoftGraphUserFields.DisplayName, MicrosoftGraphUserFields.Department, MicrosoftGraphUserFields.JobTitle, MicrosoftGraphUserFields.Id };
            // var user = await MicrosoftGraphService.Instance.GetUserAsync(selectFields);
            var user = await MicrosoftGraphService.Instance.GetUserAsync();
            UserPanel.DataContext = user;

            using (IRandomAccessStream photoStream = await MicrosoftGraphService.Instance.GetUserPhotoAsync())
            {
               BitmapImage photo = new BitmapImage();
               if (photoStream != null)
                {
                    await photo.SetSourceAsync(photoStream);
                }
               else
                {
                    photo.UriSource = new Uri("ms-appx:///SamplePages/MicrosoftGraph Service/user.png");
                }

                this.Photo.Source = photo;
            }

            Shell.Current.DisplayWaitRing = false;
            MessagesBox.Visibility = Visibility.Visible;
            UserBox.Visibility = Visibility.Visible;
            ClientIdBox.Visibility = Visibility.Collapsed;
        }

        /// <summary>
        /// Store the collection of messages
        /// </summary>
        private Graph.IUserMessagesCollectionPage messages = null;

        private async void GetMessagesButton_Click(object sender, RoutedEventArgs e)
        {
            if (!await Tools.CheckInternetConnection())
            {
                return;
            }

            string txtTop = TopText.Text;
            int top = 0;
            if (string.IsNullOrEmpty(txtTop))
            {
                top = 10;
            }
            else
            {
                top = Convert.ToInt32(txtTop);
            }

            bool isFirstCall = true;

            var incrementalCollection = new IncrementalCollection<Graph.Message>((CancellationToken cts, uint count) =>
              {
                  return Task.Run<ObservableCollection<Graph.Message>>(async () =>
                    {
                        ObservableCollection<Graph.Message> intermediateList = new ObservableCollection<Graph.Message>();
                        if (isFirstCall)
                        {
                            messages = await MicrosoftGraphService.Instance.GetUserMessagesAsync(cts, top);

                            isFirstCall = false;
                        }
                        else
                        {
                            messages = await messages.NextPageAsync(cts);
                        }

                        if (cts.IsCancellationRequested)
                        {
                            return intermediateList;
                        }
                        if (messages != null)
                        {
                            messages.AddTo(intermediateList);
                        }

                        return intermediateList;
                    });
              });

            MessagesList.ItemsSource = incrementalCollection;
        }

        private async void GetNextMessagesButton_Click(object sender, RoutedEventArgs e)
        {
            if (!await Tools.CheckInternetConnection())
            {
                return;
            }

            Shell.Current.DisplayWaitRing = true;

            messages = await messages.NextPageAsync();
            if (messages != null)
            {
                MessagesList.ItemsSource = messages;
            }

            Shell.Current.DisplayWaitRing = false;
        }

        private void ClientIdExpandButton_Click(object sender, RoutedEventArgs e)
        {

            SetVisibilityStatusPanel(ClientIdBox, (Button)sender);
        }

        private void UserBoxExpandButton_Click(object sender, RoutedEventArgs e)
        {
            SetVisibilityStatusPanel(UserPanel, (Button)sender);
        }

        private void MessageBoxExpandButton_Click(object sender, RoutedEventArgs e)
        {
            SetVisibilityStatusPanel(MessagesPanel, (Button)sender);
            SetVisibilityStatusPanel(MessagesList, (Button)sender);
        }

        private void SetVisibilityStatusPanel(FrameworkElement box, Button switchButton)
        {
            if (box.IsVisible())
            {
                switchButton.Content = "";
                box.Visibility = Visibility.Collapsed;
            }
            else
            {
                switchButton.Content = "";
                box.Visibility = Visibility.Visible;
            }
        }

      
    }
}
