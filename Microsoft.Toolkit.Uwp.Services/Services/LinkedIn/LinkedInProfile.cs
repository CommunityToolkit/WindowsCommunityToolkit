using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.Toolkit.Uwp.Services.LinkedIn
{
    /// <summary>
    /// Strongly typed LinkedIn Basic Profile.  More details here https://developer.linkedin.com/docs/fields/basic-profile.
    /// </summary>
    public partial class LinkedInProfile
    {
        /// <summary>
        /// Gets a string description of the strongly typed properties in this model.
        /// </summary>
        public static string Fields => "first-name,last-name,headline,id,picture-url,site-standard-profile-request,num-connections,summary,public-profile-url";

        /// <summary>
        /// Gets or sets firstName property.
        /// </summary>
        public string FirstName { get; set; }

        /// <summary>
        /// Gets or sets headline property.
        /// </summary>
        public string Headline { get; set; }

        /// <summary>
        /// Gets or sets id property.
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Gets or sets lastname property.
        /// </summary>
        public string LastName { get; set; }

        /// <summary>
        /// Gets or sets picture-url property.
        /// </summary>
        public string PictureUrl { get; set; }

        /// <summary>
        /// Gets or sets num-connections property.
        /// </summary>
        public string NumConnections { get; set; }

        /// <summary>
        /// Gets or sets summary property.
        /// </summary>
        public string Summary { get; set; }

        /// <summary>
        /// Gets or sets public-profile-url property.
        /// </summary>
        public string PublicProfileUrl { get; set; }

        /// <summary>
        /// Gets or sets email-address property.  Requires r_emailaddress permission.
        /// </summary>
        public string EmailAddress { get; set; }

        /// <summary>
        /// Gets or sets siteStandardProfileRequest property.
        /// </summary>
        public LinkedInProfileRequest SiteStandardProfileRequest { get; set; }
    }
}
