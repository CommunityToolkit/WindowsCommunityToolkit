using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.Windows.Toolkit.Services.Core
{
    [AttributeUsage(AttributeTargets.Class)]
    public class ConnectedServiceOAuthAttribute : Attribute
    {
        public string ProviderPublisherKeyName { get; set; }

        public ConnectedServiceOAuthAttribute(string providerPublisherKeyName)
        {
            ProviderPublisherKeyName = providerPublisherKeyName;
        }
        
    }
}
