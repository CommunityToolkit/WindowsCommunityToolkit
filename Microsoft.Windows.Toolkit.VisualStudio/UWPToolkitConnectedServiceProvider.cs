using Microsoft.VisualStudio.ConnectedServices;
using Microsoft.Windows.Toolkit.VisualStudio.Helpers;
using Microsoft.Windows.Toolkit.VisualStudio.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.Windows.Toolkit.VisualStudio
{

    [ConnectedServiceProviderExport("Microsoft.Windows.Toolkit.VisualStudio.SocialServices")]
    internal class UWPToolkitConnectedServiceProvider : ConnectedServiceProvider
    {
        GridViewModel viewModel = new GridViewModel();

        public UWPToolkitConnectedServiceProvider()
        {
            this.Category = "Toolkits";
            this.Name = "UWP Toolkit Services";
            this.Description = "Connected Service for Social Services and more";
            this.Icon = null;
            this.CreatedBy = "Microsoft";
            this.Version = new Version(1, 0, 0);
            this.MoreInfoUri = new Uri("https://aka.ms/ConnectedServicesSDK");
        }

        public override IEnumerable<Tuple<string, Uri>> GetSupportedTechnologyLinks()
        {
            var dataProviderModels = DataProviderDiscovery.Instance.FindAllDataProviders();
            foreach(var dataProviderModel in dataProviderModels)
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
