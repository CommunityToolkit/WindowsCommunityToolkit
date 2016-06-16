// *********************************************************
//  Copyright (c) Microsoft. All rights reserved.
//  This code is licensed under the MIT License (MIT).
//  THE CODE IS PROVIDED “AS IS”, WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, 
//  INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, 
//  FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. 
//  IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, 
//  DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, 
//  TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH 
//  THE CODE OR THE USE OR OTHER DEALINGS IN THE CODE.
// *********************************************************

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

using Microsoft.VisualStudio.ConnectedServices;
using Microsoft.Windows.Toolkit.VisualStudio.Helpers;
using Microsoft.Windows.Toolkit.VisualStudio.Models;

namespace Microsoft.Windows.Toolkit.VisualStudio.ViewModels
{
	  internal class GridViewModel : ConnectedServiceGrid
	  {

        private List<UWPToolkitConnectedServiceInstance> instances;
        private List<DataProviderModel> supportedDataProviders;
        private List<Tuple<string, string>> columnMetadata;

        public GridViewModel()
		{
			Description = "Connect to one or more Social Services.";
			CreateServiceInstanceText = "Create";
			CanConfigureServiceInstance = true;
			ConfigureServiceInstanceText = "Configure";
            CanCreateServiceInstance = false;

            supportedDataProviders = DataProviderDiscovery.Instance.FindAllDataProviders();
        }

		public override IEnumerable<Tuple<string, string>> ColumnMetadata
		{
			get { return columnMetadata ?? (columnMetadata = RetrieveColumnMetadata()); }
        }

        private List<Tuple<string, string>> RetrieveColumnMetadata()
        {
            var columns = new List<Tuple<string, string>>();
                        
            foreach(var provider in supportedDataProviders)
            {
                var oAuthKeyValues = DataProviderDiscovery.Instance.FindOAuthPropertiesByProviderPublisherKeyName(provider.ProviderPublisherKeyName);

                foreach (var oAuthKeyValue in oAuthKeyValues)
                {
                    var tupleExists = (from c in columns where c.Item1 == oAuthKeyValue.Key select c).Any();
                    if(!tupleExists)
                    {
                        columns.Add(new Tuple<string, string>(oAuthKeyValue.Key, oAuthKeyValue.Key));
                    }
                }
            }

            return columns;
        }

		/// <summary>
		/// Retrieves the ConnectedServiceInstance objects.  This is called when the grid gets loaded
		/// and when the user clicks the "Refresh" button.
		/// </summary>
		public override Task<IEnumerable<ConnectedServiceInstance>> EnumerateServiceInstancesAsync(CancellationToken ct)
		{
            this.instances = new List<UWPToolkitConnectedServiceInstance>();
            
            foreach (var provider in supportedDataProviders)
            {
                var oAuthKeyValues = DataProviderDiscovery.Instance.FindOAuthPropertiesByProviderPublisherKeyName(provider.ProviderPublisherKeyName);
                this.instances.Add(this.CreateInstance(provider, oAuthKeyValues));
            }

		    return Task.FromResult<IEnumerable<ConnectedServiceInstance>>(this.instances);
		}

		/// <summary>
		/// Configures the selected ConnectedServiceInstance.
		/// </summary>
		public override Task<bool> ConfigureServiceInstanceAsync(ConnectedServiceInstance instance, CancellationToken ct)
		{
            var appKeyAndSecretViewModel = new OAuthCaptureViewModel
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
        private UWPToolkitConnectedServiceInstance CreateInstance(DataProviderModel model, Dictionary<string, string> oAuthKeyValues)
		{
            UWPToolkitConnectedServiceInstance instance = new UWPToolkitConnectedServiceInstance();

            instance.DataProviderModel = model;
            instance.Name = model.ProviderPublisherKeyName;
		    instance.InstanceId = model.ProviderPublisherKeyName;

            foreach(var columnHeader in ColumnMetadata)
            {
                string oAuthValue = Constants.OAUTH_KEY_VALUE_DEFAULT_NOT_REQUIRED_VALUE;
                if(oAuthKeyValues.ContainsKey(columnHeader.Item1))
                {
                    oAuthValue = oAuthKeyValues[columnHeader.Item1];
                }

                instance.Metadata[columnHeader.Item1] = oAuthValue;
            }

            return instance;
		}
        
    }
}
