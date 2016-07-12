// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY OF
// ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO
// THE IMPLIED WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A
// PARTICULAR PURPOSE.
//
// Copyright (c) Microsoft Corporation. All rights reserved

using System;
using System.Collections.Generic;
using System.Text;

namespace Microsoft.Windows.Toolkit.Notifications
{
    /// <summary>
    /// Contains the base properties that an image needs.
    /// </summary>
    public interface IBaseImage
    {
        /// <summary>
        /// The URI of the image. Can be from your application package, application data, or the internet. Internet images must be less than 200 KB in size.
        /// </summary>
        string Source { get; set; }

        /// <summary>
        /// A description of the image, for users of assistive technologies.
        /// </summary>
        string AlternateText { get; set; }

        /// <summary>
        /// Set to true to allow Windows to append a query string to the image URI supplied in the Tile notification. Use this attribute if your server hosts images and can handle query strings, either by retrieving an image variant based on the query strings or by ignoring the query string and returning the image as specified without the query string. This query string specifies scale, contrast setting, and language.
        /// </summary>
        bool? AddImageQuery { get; set; }
    }
}
