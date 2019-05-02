// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;

namespace Microsoft.Toolkit.Uwp.Services.Facebook
{
    /// <summary>
    /// Strongly typed object for presenting post data returned from service provider.
    /// </summary>
    public class FacebookPost
    {
        /// <summary>
        /// Gets a string description of the strongly typed properties in this model.
        /// </summary>
        public static string Fields => "id, message, created_time, link, full_picture";

        /// <summary>
        /// Gets or sets id property.
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Gets or sets message or post text.
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// Gets or sets time the entity instance was created.
        /// </summary>
        public DateTime Created_Time { get; set; }

        /// <summary>
        /// Gets or sets a link to the entity instance.
        /// </summary>
        public string Link { get; set; }

        /// <summary>
        /// Gets or sets a link to the accompanying image.
        /// </summary>
        public string Full_Picture { get; set; }
    }
}
