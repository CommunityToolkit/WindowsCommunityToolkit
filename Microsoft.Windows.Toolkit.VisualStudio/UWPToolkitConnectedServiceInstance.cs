using Microsoft.VisualStudio.ConnectedServices;
using Microsoft.Windows.Toolkit.VisualStudio.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.Windows.Toolkit.VisualStudio
{
    public class UWPToolkitConnectedServiceInstance : ConnectedServiceInstance
    {
        public DataProviderModel DataProviderModel { get; set; }
    }
}
