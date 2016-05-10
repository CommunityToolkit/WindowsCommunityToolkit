using Microsoft.Windows.Toolkit.Services.Core;
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

            var dataProvidersAssemblyName = (from a in referencedAssemblies where a.Name == Constants.DATA_PROVIDER_LIBRARY_NAME select a).Single();

            var dataProvidersAssembly = AppDomain.CurrentDomain.Load(dataProvidersAssemblyName);

            var allTypesInAssembly = dataProvidersAssembly.DefinedTypes;

            var allTypesWithCustomAttributes = from t in allTypesInAssembly where t.CustomAttributes.Count() > 0 select t;

            var dataProviderModels = from t in allTypesWithCustomAttributes
                                where t.CustomAttributes.Any(attr => { return (attr.AttributeType == typeof(ConnectedServiceProviderAttribute)); })
                                select new DataProviderModel
                                {
                                    ProviderPublisherKeyName = (t.GetCustomAttribute(typeof(ConnectedServiceProviderAttribute)) as ConnectedServiceProviderAttribute).ProviderPublisherKeyName,
                                    ServiceDeveloperInformationUrl = (t.GetCustomAttribute(typeof(ConnectedServiceProviderAttribute)) as ConnectedServiceProviderAttribute).DeveloperPortalUrl,
                                    ProviderType = t
                                };

            return dataProviderModels.ToList();
        }

        public Dictionary<string,string> FindOAuthPropertiesByProviderPublisherKeyName(string providerPublisherKeyName)
        {
            Dictionary<string, string> oAuthProperties = new Dictionary<string, string>();

            var currentAssembly = Instance.GetType().GetTypeInfo().Assembly;
            var referencedAssemblies = currentAssembly.GetReferencedAssemblies();

            var dataProvidersAssemblyName = (from a in referencedAssemblies where a.Name == Constants.DATA_PROVIDER_LIBRARY_NAME select a).Single();

            var dataProvidersAssembly = AppDomain.CurrentDomain.Load(dataProvidersAssemblyName);

            var allTypesInAssembly = dataProvidersAssembly.DefinedTypes;

            var allTypesWithCustomAttributes = from t in allTypesInAssembly where t.CustomAttributes.Count() > 0 select t;

            var oAuthType = (from t in allTypesWithCustomAttributes
                             where t.GetCustomAttributes(typeof(ConnectedServiceOAuthAttribute)).Any(attr => { return (attr as ConnectedServiceOAuthAttribute).ProviderPublisherKeyName == providerPublisherKeyName; })
                             select t).SingleOrDefault();

            if(oAuthType != null)
            {
                var props = from p in oAuthType.GetProperties() select new KeyValuePair<string, string>(p.Name, Constants.OAUTH_KEY_VALUE_DEFAULT_REQUIRED_VALUE);
                oAuthProperties = props.ToDictionary(t => t.Key, t => t.Value);
            }
            else
            {
                throw new Exception($"OAuth properties not found for providerPublisherKeyName: {providerPublisherKeyName}");
            }

            return oAuthProperties;
        }

        public string FindQueryParamStringNameByProviderPublisherKeyName(string providerPublisherKeyName)
        {
            string queryParamStringName = String.Empty;

            var currentAssembly = Instance.GetType().GetTypeInfo().Assembly;
            var referencedAssemblies = currentAssembly.GetReferencedAssemblies();

            var dataProvidersAssemblyName = (from a in referencedAssemblies where a.Name == Constants.DATA_PROVIDER_LIBRARY_NAME select a).Single();

            var dataProvidersAssembly = AppDomain.CurrentDomain.Load(dataProvidersAssemblyName);

            var allTypesInAssembly = dataProvidersAssembly.DefinedTypes;

            var allTypesWithCustomAttributes = from t in allTypesInAssembly where t.CustomAttributes.Count() > 0 select t;

            var dataConfigType = (from t in allTypesWithCustomAttributes
                             where t.GetCustomAttributes(typeof(ConnectedServiceDataConfigAttribute)).Any(attr => { return (attr as ConnectedServiceDataConfigAttribute).ProviderPublisherKeyName == providerPublisherKeyName; })
                             select t).SingleOrDefault();

            if (dataConfigType != null)
            {
                var queryStringProperty = (from p in dataConfigType.GetProperties() where p.CustomAttributes.Any(attr => { return attr.AttributeType == typeof(ConnectedServiceDataConfigQueryStringParamAttribute); }) select p).Single();
                queryParamStringName = queryStringProperty.Name;
            }
            else
            {
                throw new Exception($"No dataConfigType found for providerPublisherKeyName: {providerPublisherKeyName}");
            }

            return queryParamStringName;
        }
    }
}
