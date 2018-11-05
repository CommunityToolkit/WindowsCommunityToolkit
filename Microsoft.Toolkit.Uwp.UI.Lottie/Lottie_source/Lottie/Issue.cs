// Copyright(c) Microsoft Corporation.All rights reserved.
// Licensed under the MIT License.

namespace Microsoft.Toolkit.Uwp.UI.Lottie
{
    public sealed class Issue
    {
        public string Code { get; set; }
        public string Description { get; set; }

        public string Url => $"https://airbnb.design/lottie/#{Code}";

        public override string ToString() => $"{Code}: {Description}";
    }
}
