using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.Toolkit.Uwp.Services.Facebook
{
    public class FacebookPage
    {
        /// <summary>
        /// Gets a string description of the strongly typed properties in this model.
        /// </summary>
        public static string Fields => "id, link, name";

        /// <summary>
        /// Gets or sets id property.
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Gets or sets a link to the entity instance.
        /// </summary>
        public string Link { get; set; }

        /// <summary>
        /// Gets or sets name of the page
        /// </summary>
        public string Name { get; set; }
    }
}