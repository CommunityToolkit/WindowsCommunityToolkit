// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Diagnostics.CodeAnalysis;

[assembly: SuppressMessage("General", "SWC1001:XmlDocumentationCommentShouldBeSpelledCorrectly", MessageId = "Theming", Justification = "Correct spelling")]

namespace Microsoft.Toolkit.Uwp.UI.Controls.Design
{
#if !VS_DESIGNER_PROCESS_ISOLATION
    internal static partial class ControlTypes
    {
        // HACK: Don't forget to update, if the namespace changes.
        public const string RootNamespace = "Microsoft.Toolkit.Uwp.UI.Controls";
    }
#endif
    /// <summary>
    /// Names for ToolboxCategoryAttribute.
    /// </summary>
    internal static class ToolboxCategoryPaths
    {
        /// <summary>
        /// Basic Controls category.
        /// </summary>
        public const string Toolkit = "Windows Community Toolkit";
    }
}
