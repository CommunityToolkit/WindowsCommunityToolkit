// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

namespace Microsoft.Toolkit.Uwp.Services.Facebook
{
    /// <summary>
    /// Class used to store JSON data response from Facebook
    /// </summary>
    /// <typeparam name="T">Type of the inner data</typeparam>
    internal class FacebookDataHost<T>
    {
        /// <summary>
        /// Gets or sets internal data.
        /// </summary>
        public T Data { get; set; }
    }
}
