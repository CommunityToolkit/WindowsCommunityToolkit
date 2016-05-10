using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.Windows.Toolkit.Services.Core
{
    [AttributeUsage(AttributeTargets.Class)]
    public class ConnectedServiceDataConfigAttribute : Attribute
    {
        public string ProviderPublisherKeyName { get; set; }

        public ConnectedServiceDataConfigAttribute(string providerPublisherKeyName)
        {
            ProviderPublisherKeyName = providerPublisherKeyName;
        }
    }
}
