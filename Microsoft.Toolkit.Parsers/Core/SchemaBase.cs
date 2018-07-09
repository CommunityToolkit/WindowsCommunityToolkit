// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

namespace Microsoft.Toolkit.Parsers
{
    /// <summary>
    /// Strong typed schema base class.
    /// </summary>
    public abstract class SchemaBase
    {
        /// <summary>
        /// Gets or sets identifier for strong typed record.
        /// </summary>
        public string InternalID { get; set; }
    }
}