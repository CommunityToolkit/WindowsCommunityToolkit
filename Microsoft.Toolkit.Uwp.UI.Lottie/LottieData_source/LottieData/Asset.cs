// Copyright(c) Microsoft Corporation.All rights reserved.
// Licensed under the MIT License.

namespace Microsoft.Toolkit.Uwp.UI.Lottie.LottieData
{
#if !WINDOWS_UWP
    public
#endif
    abstract class Asset
    {
        protected private Asset(string id)
        {
            Id = id;
        }

        public string Id { get; }

        public abstract AssetType Type { get; }

        public enum AssetType
        {
            LayerCollection,
            Image,
        }
    }
}
