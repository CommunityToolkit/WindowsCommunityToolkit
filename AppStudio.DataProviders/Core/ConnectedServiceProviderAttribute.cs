using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace AppStudio.DataProviders.Core
{
    [AttributeUsage(AttributeTargets.Class)]
    public class ConnectedServiceProviderAttribute : Attribute
    {
        public string ProviderPublisherName { get; set; }
        public string DeveloperPortalUrl { get; set; }

        public ConnectedServiceProviderAttribute(string providerPublisherName, string serviceDeveloperInformationUrl)
        {
            ProviderPublisherName = providerPublisherName;
            DeveloperPortalUrl = serviceDeveloperInformationUrl;
        }
    }

}
