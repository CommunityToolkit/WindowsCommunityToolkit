using System;
using System.Diagnostics.CodeAnalysis;
using Newtonsoft.Json;

namespace Microsoft.Windows.Toolkit.Services
{
    public abstract class SchemaBase
    {
        [CLSCompliant(false)]
        [SuppressMessage("Microsoft.Naming", "CA1709", Justification = "This property is reserved.")]
        [SuppressMessage("Microsoft.Naming", "CA1707", Justification = "This property is reserved.")]
        public string _id { get; set; }
    }
}
