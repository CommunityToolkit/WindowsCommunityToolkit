using Microsoft.VisualStudio.ConnectedServices;
using Microsoft.VisualStudio.PlatformUI;
using Microsoft.Windows.Toolkit.VisualStudio.Views;
using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Input;

namespace Microsoft.Windows.Toolkit.VisualStudio.ViewModels
{
    public class AppKeyAndSecretViewModel : INotifyPropertyChanged
    {

        public event PropertyChangedEventHandler PropertyChanged;

        public ICommand OkCommand { get; set; }

        public ICommand CancelCommand { get; set; }

        public Window Window { get; set; }
        public FrameworkElement View { get; set; }

        private string appId;

        public string AppId
        {
            get { return appId; }
            set
            {
                appId = value;
                OnPropertyChanged("AppId");
            }
        }

        private string appSecret;

        public string AppSecret
        {
            get { return appSecret; }
            set
            {
                appSecret = value;
                OnPropertyChanged("AppSecret");
            }
        }

        private string accessToken;

        public string AccessToken
        {
            get { return accessToken; }
            set
            {
                accessToken = value;
                OnPropertyChanged("AccessToken");
            }
        }

        private string accessTokenSecret;

        public string AccessTokenSecret
        {
            get { return accessTokenSecret; }
            set
            {
                accessTokenSecret = value;
                OnPropertyChanged("AccessTokenSecret");
            }
        }
        
        private UWPToolkitConnectedServiceInstance connectedServiceInstance;

        public UWPToolkitConnectedServiceInstance ConnectedServiceInstance
        {
            get { return connectedServiceInstance; }
            set
            {
                connectedServiceInstance = value;
                AppId = connectedServiceInstance.Metadata[Constants.APP_ID_COLUMN_ID].ToString();
                AppSecret = connectedServiceInstance.Metadata[Constants.APP_SECRET_COLUMN_ID].ToString();
                AccessToken = connectedServiceInstance.Metadata[Constants.ACCESS_TOKEN_COLUMN_ID].ToString();
                AccessTokenSecret = connectedServiceInstance.Metadata[Constants.ACCESS_TOKEN_SECRET_COLUMN_ID].ToString();
            }
        }
        
        public AppKeyAndSecretViewModel()
        {
            this.View = new AppKeyAndSecretView();
            this.View.DataContext = this;
            this.OkCommand = new DelegateCommand(ExecuteOkClicked);
            this.CancelCommand = new DelegateCommand(ExecuteCancelClicked);
        }

        private void ExecuteOkClicked(object sender)
        {
            connectedServiceInstance.Metadata[Constants.APP_ID_COLUMN_ID] = AppId;
            connectedServiceInstance.Metadata[Constants.APP_SECRET_COLUMN_ID] = AppSecret;
            connectedServiceInstance.Metadata[Constants.ACCESS_TOKEN_COLUMN_ID] = AccessToken;
            connectedServiceInstance.Metadata[Constants.ACCESS_TOKEN_SECRET_COLUMN_ID] = AccessTokenSecret;
            Window.Close();
        }

        private void ExecuteCancelClicked(object sender)
        {
            Window.Close();
        }

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
