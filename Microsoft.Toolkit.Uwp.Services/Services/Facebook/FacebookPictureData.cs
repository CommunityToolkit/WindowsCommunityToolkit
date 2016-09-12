using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.Toolkit.Uwp.Services.Facebook
{
    /// <summary>
    /// Strongly types Facebook Picture object. Partial for extending properties.
    /// </summary>
    public partial class FacebookPictureData
    {
        /// <summary>
        /// Gets or sets data property.
        /// </summary>
        public FacebookPicture Data { get; set; }
    }
}
