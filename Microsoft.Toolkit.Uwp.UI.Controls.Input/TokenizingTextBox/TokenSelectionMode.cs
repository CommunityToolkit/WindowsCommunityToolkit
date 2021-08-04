// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.Toolkit.Uwp.UI.Controls
{
    /// <summary>
    /// Indicates how tokens are selected in the <see cref="TokenizingTextBox"/>.
    /// </summary>
    public enum TokenSelectionMode
    {
        /// <summary>
        /// Only one token can be selected at a time. A new token should replace the active selection.
        /// </summary>
        Single,

        /// <summary>
        /// Multiple tokens can be selected at a time.
        /// </summary>
        Multiple,
    }
}
