using Microsoft.VisualStudio.ConnectedServices;
using Microsoft.VisualStudio.PlatformUI;
using Microsoft.Windows.Toolkit.VisualStudio.Views;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Microsoft.Windows.Toolkit.VisualStudio.ViewModels
{
    public class OAuthCaptureViewModel
    {
        public ICommand OkCommand { get; set; }

        public ICommand CancelCommand { get; set; }

        public Window Window { get; set; }
        public FrameworkElement View { get; set; }

        private Dictionary<string, object> oAuthKeyValues;

        private UWPToolkitConnectedServiceInstance connectedServiceInstance;

        public UWPToolkitConnectedServiceInstance ConnectedServiceInstance
        {
            get { return connectedServiceInstance; }
            set
            {
                connectedServiceInstance = value;
                oAuthKeyValues = new Dictionary<string, object>(connectedServiceInstance.Metadata);
                CreateDynamicUI();
            }
        }
        
        public OAuthCaptureViewModel()
        {
            this.View = new OAuthCaptureView();
            this.View.DataContext = this;
            this.OkCommand = new DelegateCommand(ExecuteOkClicked);
            this.CancelCommand = new DelegateCommand(ExecuteCancelClicked);
        }

        private void CreateDynamicUI()
        {
            var view = View as OAuthCaptureView;
            var dynamicGrid = view.DynamicGrid;

            foreach(var oAuthKeyValue in oAuthKeyValues)
            {
                if (oAuthKeyValue.Value.ToString() != Constants.OAUTH_KEY_VALUE_DEFAULT_NOT_REQUIRED_VALUE)
                {
                    RowDefinition rowDefinition = new RowDefinition { Height = new GridLength(0, GridUnitType.Auto) };
                    dynamicGrid.RowDefinitions.Add(rowDefinition);

                    int rowCount = dynamicGrid.RowDefinitions.Count;

                    TextBlock label = new TextBlock { Text = oAuthKeyValue.Key };
                    Grid.SetRow(label, rowCount - 1);

                    TextBox input = new TextBox { Tag = oAuthKeyValue.Key, Text = oAuthKeyValue.Value.ToString() };
                    input.TextChanged += Input_TextChanged;
                    Grid.SetRow(input, rowCount - 1);
                    Grid.SetColumn(input, 1);

                    dynamicGrid.Children.Add(label);
                    dynamicGrid.Children.Add(input);
                }
            }
        }

        private void Input_TextChanged(object sender, TextChangedEventArgs e)
        {
            var input = (sender as TextBox);
            var oAuthKey = input.Tag;

            oAuthKeyValues[oAuthKey.ToString()] = input.Text;
        }

        private void ExecuteOkClicked(object sender)
        {
            connectedServiceInstance.Metadata.Clear();
            foreach(var oAuthKeyValue in oAuthKeyValues)
            {
                connectedServiceInstance.Metadata.Add(oAuthKeyValue.Key, oAuthKeyValue.Value);
            }
            Window.Close();
        }

        private void ExecuteCancelClicked(object sender)
        {
            Window.Close();
        }
    }
}
