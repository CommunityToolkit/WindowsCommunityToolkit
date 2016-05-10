using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.Windows.Toolkit.Services.Facebook
{
    internal class GraphData
    {
        public string id { get; set; }
        public From from { get; set; }
        public string type { get; set; }
        public DateTime created_time { get; set; }
        public DateTime updated_time { get; set; }
        public string message { get; set; }
        public string full_picture { get; set; }
        public string link { get; set; }
    }
}
