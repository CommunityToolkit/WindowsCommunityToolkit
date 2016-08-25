using Microsoft.Toolkit.Uwp.Services.MicrosoftGraph;
using System;
using System.IO;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Imaging;




namespace Microsoft.Toolkit.Uwp.SampleApp.SamplePages
{

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
                var error = new MessageDialog("Unable to log to Office 365");
                await error.ShowAsync();
                return;
            }

            Shell.Current.DisplayWaitRing = true;

            // Retrieve user's info from Azure Active Directory
            var user = await MicrosoftGraphService.Instance.GetUserAsync();
            UserPanel.DataContext = user;

            using (Stream photoStream = await MicrosoftGraphService.Instance.GetUserPhotoAsync())
            {
               BitmapImage photo = new BitmapImage();
               photoStream.Position = 0;
               await photo.SetSourceAsync(photoStream.AsRandomAccessStream());
                this.Photo.Source = photo;
            }

            Shell.Current.DisplayWaitRing = false;
            MessagesBox.Visibility = Visibility.Visible;
            UserBox.Visibility = Visibility.Visible;
            ClientIdBox.Visibility = Visibility.Collapsed;
        }

        private async void GetMessagesButton_Click(object sender, RoutedEventArgs e)
        {
            if (!await Tools.CheckInternetConnection())
            {
                return;
            }

            Shell.Current.DisplayWaitRing = true;
            string txtTop = TopText.Text;
            Graph.IUserMessagesCollectionPage messages = null;
            if (string.IsNullOrEmpty(txtTop))
            {
                messages = await MicrosoftGraphService.Instance.GetUserMessagesAsync();
            }

            int top = Convert.ToInt32(txtTop);

            messages = await MicrosoftGraphService.Instance.GetUserMessagesAsync(top);
            MessagesList.ItemsSource = messages;
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
