using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.Toolkit.Uwp.Services.LinkedIn
{
    /// <summary>
    /// Configuration object for specifying richer query information.
    /// </summary>
    public class LinkedInDataConfig
    {
        /// <summary>
        /// Gets or sets the query string for filtering service results.
        /// </summary>
        public string Query { get; set; }
    }
}
