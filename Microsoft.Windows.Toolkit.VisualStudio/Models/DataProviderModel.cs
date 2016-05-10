using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.Windows.Toolkit.VisualStudio.Models
{
    public class DataProviderModel
    {
        public TypeInfo ProviderType { get; set; }
        public string ProviderPublisherKeyName { get; set; }
        public string ServiceDeveloperInformationUrl { get; set; }
    }
}
