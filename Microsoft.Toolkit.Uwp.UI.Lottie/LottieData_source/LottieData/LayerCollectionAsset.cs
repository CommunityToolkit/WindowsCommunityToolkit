// Copyright(c) Microsoft Corporation.All rights reserved.
// Licensed under the MIT License.

namespace Microsoft.Toolkit.Uwp.UI.Lottie.LottieData
{
    /// <summary>
    /// A <see cref="LayerCollection"/> stored in the assets section of a <see cref="LottieComposition"/>.
    /// </summary>
#if !WINDOWS_UWP
    public
#endif
    sealed class LayerCollectionAsset : Asset
    {
        public LayerCollectionAsset(string id, LayerCollection layers) : base(id)
        {
            Layers = layers;
        }

        public LayerCollection Layers { get; }

        public override AssetType Type => AssetType.LayerCollection;
    }
}
