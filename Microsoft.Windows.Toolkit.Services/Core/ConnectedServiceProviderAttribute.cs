using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.Windows.Toolkit.Services.Core
{
    [AttributeUsage(AttributeTargets.Class)]
    public class ConnectedServiceProviderAttribute : Attribute
    {
        public string ProviderPublisherKeyName { get; set; }
        public string DeveloperPortalUrl { get; set; }

        public ConnectedServiceProviderAttribute(string providerPublisherKeyName, string serviceDeveloperInformationUrl)
        {
            ProviderPublisherKeyName = providerPublisherKeyName;
            DeveloperPortalUrl = serviceDeveloperInformationUrl;
        }
    }
}
