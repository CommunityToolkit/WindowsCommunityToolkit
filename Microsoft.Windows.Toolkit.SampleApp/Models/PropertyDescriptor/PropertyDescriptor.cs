using System.Collections.Generic;
using System.Dynamic;

namespace Microsoft.Windows.Toolkit.SampleApp.Models
{
    public class PropertyDescriptor
    {
        public ExpandoObject Expando { get; set; }

        public List<PropertyOptions> Options { get; private set; }

        public PropertyDescriptor()
        {
            Options = new List<PropertyOptions>();
        }
    }
}
