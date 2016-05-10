using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.Windows.Toolkit.Services.Core
{
    [AttributeUsage(AttributeTargets.Property)]
    public class ConnectedServiceDataConfigQueryStringParamAttribute : Attribute
    {
        public string ProviderPublisherKeyName { get; set; }

        public ConnectedServiceDataConfigQueryStringParamAttribute(string providerPublisherKeyName)
        {
            ProviderPublisherKeyName = providerPublisherKeyName;
        }
    }
}
