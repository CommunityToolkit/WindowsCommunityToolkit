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
using System.Threading.Tasks;

using Microsoft.VisualStudio.ConnectedServices;
using Microsoft.Windows.Toolkit.VisualStudio.Helpers;
using Microsoft.Windows.Toolkit.VisualStudio.ViewModels;

namespace Microsoft.Windows.Toolkit.VisualStudio
{
    [ConnectedServiceProviderExport("Microsoft.Windows.Toolkit.VisualStudio.SocialServices")]
    internal class UWPToolkitConnectedServiceProvider : ConnectedServiceProvider
    {
        private GridViewModel viewModel = new GridViewModel();

        public UWPToolkitConnectedServiceProvider()
        {
            Category = "Toolkits";
            Name = "UWP Toolkit Services";
            Description = "Connected Service for Social Services and more";
            Icon = null;
            CreatedBy = "Microsoft";
            Version = new Version(1, 0, 0);
            MoreInfoUri = new Uri("https://aka.ms/ConnectedServicesSDK");
        }

        public override IEnumerable<Tuple<string, Uri>> GetSupportedTechnologyLinks()
        {
            var dataProviderModels = DataProviderDiscovery.Instance.FindAllDataProviders();
            foreach (var dataProviderModel in dataProviderModels)
            {
                yield return Tuple.Create(dataProviderModel.ProviderPublisherKeyName, new Uri(dataProviderModel.ServiceDeveloperInformationUrl));
            }
        }

        public override Task<ConnectedServiceConfigurator> CreateConfiguratorAsync(ConnectedServiceProviderContext context)
        {
            ConnectedServiceConfigurator configurator = viewModel;

            return Task.FromResult(configurator);
        }
    }
}
