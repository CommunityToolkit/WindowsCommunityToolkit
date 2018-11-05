// Copyright(c) Microsoft Corporation.All rights reserved.
// Licensed under the MIT License.

namespace Microsoft.Toolkit.Uwp.UI.Lottie.WinCompData
{
    /// <summary>
    /// Interface implemented by objects to expose a description in plain language.
    /// The descriptions are typically used by comments in generated code.
    /// </summary>
#if !WINDOWS_UWP
    public
#endif
    interface IDescribable
    {
        string LongDescription { get; set; }
        string ShortDescription { get; set; }
    }
}
