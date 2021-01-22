// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Windows.Input;

namespace Microsoft.Toolkit.Uwp.UI.Controls
{
    /// <summary>
    /// A unit of metadata to display in <see cref="MetadataControl"/>.
    /// </summary>
    public struct MetadataUnit
    {
        /// <summary>
        /// Gets or sets the label of the unit.
        /// </summary>
        public string Label { get; set; }

        /// <summary>
        /// Gets or sets the automation name that will be set on the unit.
        /// If not set, <see cref="Label"/> will be used.
        /// </summary>
        public string AccessibleLabel { get; set; }

        /// <summary>
        /// Gets or sets the command associated to the unit.
        /// If null, the unit will be displayed as a text field.
        /// If set, the unit will be displayed as an hyperlink.
        /// </summary>
        public ICommand Command { get; set; }

        /// <summary>
        /// Gets or sets the parameter that will be provided to the <see cref="Command"/>.
        /// </summary>
        public object CommandParameter { get; set; }
    }
}
