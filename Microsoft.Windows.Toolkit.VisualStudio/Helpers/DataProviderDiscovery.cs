using AppStudio.DataProviders;
using AppStudio.DataProviders.Core;
using Microsoft.Windows.Toolkit.VisualStudio.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.Windows.Toolkit.VisualStudio.Helpers
{
    internal class DataProviderDiscovery
    {
        private DataProviderDiscovery()
        {

        }

        private static DataProviderDiscovery _instance;

        public static DataProviderDiscovery Instance
        {
            get
            {
                return _instance ?? (_instance = new DataProviderDiscovery());
            }
        }

        public List<DataProviderModel> FindAllDataProviders()
        {
            var currentAssembly = Instance.GetType().GetTypeInfo().Assembly;
            var referencedAssemblies = currentAssembly.GetReferencedAssemblies();

            // TODO Make less brittle / more concise / cache it for performance / basically re-write :-)
            var dataProvidersAssemblyName = (from a in referencedAssemblies where a.Name == "AppStudio.DataProviders" select a).Single();

            var dataProvidersAssembly = AppDomain.CurrentDomain.Load(dataProvidersAssemblyName);

            var allTypesInAssembly = dataProvidersAssembly.DefinedTypes;

            var allTypesWithCustomAttributes = from t in allTypesInAssembly where t.CustomAttributes.Count() > 0 select t;

            var dataProviderModels = from t in allTypesWithCustomAttributes
                                where t.CustomAttributes.Any(attr => { return (attr.AttributeType == typeof(ConnectedServiceProviderAttribute)); })
                                select new DataProviderModel
                                {
                                    ProviderPublisherName = (t.GetCustomAttribute(typeof(ConnectedServiceProviderAttribute)) as ConnectedServiceProviderAttribute).ProviderPublisherName,
                                    ServiceDeveloperInformationUrl = (t.GetCustomAttribute(typeof(ConnectedServiceProviderAttribute)) as ConnectedServiceProviderAttribute).DeveloperPortalUrl,
                                    ProviderType = t
                                };

            return dataProviderModels.ToList();
        }



        private bool IsConnectedServiceProviderAttribute(CustomAttributeData attr)
        {
            return attr.NamedArguments.Any(arg => attr.AttributeType == typeof(ConnectedServiceProviderAttribute));
        }

    }
}
