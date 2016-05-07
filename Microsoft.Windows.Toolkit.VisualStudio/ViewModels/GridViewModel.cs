using Microsoft.VisualStudio.ConnectedServices;
using Microsoft.VisualStudio.PlatformUI;
using Microsoft.Windows.Toolkit.VisualStudio.Helpers;
using Microsoft.Windows.Toolkit.VisualStudio.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace Microsoft.Windows.Toolkit.VisualStudio.ViewModels
{
	  internal class GridViewModel : ConnectedServiceGrid
	  {

        private List<UWPToolkitConnectedServiceInstance> instances;

        public GridViewModel()
		{
			Description = "Connect to one or more Social Services.";
			CreateServiceInstanceText = "Create";
			CanConfigureServiceInstance = true;
			ConfigureServiceInstanceText = "Configure";
            CanCreateServiceInstance = false;
        }

		public override IEnumerable<Tuple<string, string>> ColumnMetadata
		{
			get {
                return new List<Tuple<string, string>>
                {
                    Tuple.Create(Constants.APP_ID_COLUMN_ID, Constants.APP_ID_COLUMN_DISPLAY_NAME),
                    Tuple.Create(Constants.APP_SECRET_COLUMN_ID, Constants.APP_SECRET_COLUMN_DISPLAY_NAME),
                    Tuple.Create(Constants.ACCESS_TOKEN_COLUMN_ID, Constants.ACCESS_TOKEN_COLUMN_DISPLAY_NAME),
                    Tuple.Create(Constants.ACCESS_TOKEN_SECRET_COLUMN_ID, Constants.ACCESS_TOKEN_SECRET_DISPLAY_NAME)
                };
            }
        }

		public override IEnumerable<Tuple<string, string>> DetailMetadata
		{
            get
            {
                return new[]
                {
	                Tuple.Create("Detail1", "Detail1 Display"),
	                Tuple.Create("Detail2", "Detail2 Display")
                };
			 }
		}

		/// <summary>
		/// Retrieves the ConnectedServiceInstance objects.  This is called when the grid gets loaded
		/// and when the user clicks the "Refresh" button.
		/// </summary>
		public override Task<IEnumerable<ConnectedServiceInstance>> EnumerateServiceInstancesAsync(CancellationToken ct)
		{
            this.instances = new List<UWPToolkitConnectedServiceInstance>();

            var results = DataProviderDiscovery.Instance.FindAllDataProviders();
            
            foreach (var result in results)
            {
                this.instances.Add(this.CreateInstance(result, Constants.APP_ID_COLUMN_PLACEHOLDER_VALUE, Constants.APP_ID_COLUMN_PLACEHOLDER_VALUE, Constants.ACCESS_TOKEN_COLUMN_PLACEHOLDER_VALUE, Constants.ACCESS_TOKEN_SECRET_PLACEHOLDER_VALUE, "Some detail 1", "Some detail 2"));
            }

		    return Task.FromResult<IEnumerable<ConnectedServiceInstance>>(this.instances);
		}

		/// <summary>
		/// Configures the selected ConnectedServiceInstance.
		/// </summary>
		public override Task<bool> ConfigureServiceInstanceAsync(ConnectedServiceInstance instance, CancellationToken ct)
		{
            var appKeyAndSecretViewModel = new AppKeyAndSecretViewModel
            {
                ConnectedServiceInstance = instance as UWPToolkitConnectedServiceInstance
            };
            
            Window window = new Window
            {
                Content = appKeyAndSecretViewModel.View,
                MaxHeight = 400,
                MaxWidth = 400
            };

            appKeyAndSecretViewModel.Window = window;
            
            window.ShowDialog();

            return Task.FromResult(true);
		}

        /// <summary>
        /// Creates a new ConnectedServiceInstance with the specified values.
        /// </summary>
        private UWPToolkitConnectedServiceInstance CreateInstance(DataProviderModel model, string appIdValue, string appSecretValue, string accessToken, string accessTokenSecret, string detail1, string detail2)
		{
            UWPToolkitConnectedServiceInstance instance = new UWPToolkitConnectedServiceInstance();

            instance.DataProviderModel = model;
            instance.Name = model.ProviderPublisherName;
		    instance.InstanceId = model.ProviderPublisherName;
		    instance.Metadata.Add(Constants.APP_ID_COLUMN_ID, appIdValue);
            instance.Metadata.Add(Constants.APP_SECRET_COLUMN_ID, appSecretValue);
            instance.Metadata.Add(Constants.ACCESS_TOKEN_COLUMN_ID, accessToken);
            instance.Metadata.Add(Constants.ACCESS_TOKEN_SECRET_COLUMN_ID, accessTokenSecret);
            instance.Metadata.Add("Detail1", detail1);
	    	instance.Metadata.Add("Detail2", detail2);

            return instance;
		}
        
    }
}
