using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.Windows.Toolkit.SampleApp.Models
{
    public class PropertyOptions
    {
        public string Name { get; set; }
        public string OriginalString { get; set; }
        public PropertyKind Kind { get; set; }
        public object DefaultValue { get; set; }
    }

    public class SliderPropertyOptions : PropertyOptions
    {
        public double MinValue { get; set; }
        public double MaxValue { get; set; }
    }
}
