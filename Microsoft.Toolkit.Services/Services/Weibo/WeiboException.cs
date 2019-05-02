// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;

namespace Microsoft.Toolkit.Services.Weibo
{
    /// <summary>
    /// Weibo specific exception.
    /// </summary>
    public class WeiboException : Exception
    {
        /// <summary>
        /// Gets or sets the errors returned by Weibo
        /// </summary>
        public WeiboError Error { get; set; }
    }
}