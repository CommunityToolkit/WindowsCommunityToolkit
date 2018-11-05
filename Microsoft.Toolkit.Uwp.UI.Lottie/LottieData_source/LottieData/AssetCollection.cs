// Copyright(c) Microsoft Corporation.All rights reserved.
// Licensed under the MIT License.

using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Microsoft.Toolkit.Uwp.UI.Lottie.LottieData
{
#if !WINDOWS_UWP
    public
#endif
    sealed class AssetCollection : IEnumerable<Asset>
    {
        readonly Asset[] _assets;
        readonly Dictionary<string, Asset> _assetsById;

        public AssetCollection(IEnumerable<Asset> assets)
        {
            _assets = assets.ToArray();
            _assetsById = _assets.ToDictionary(asset => asset.Id);
        }

        /// <summary>
        /// Returns the <see cref="Asset"/> with the given id, or null if not found.
        /// </summary>
        public Asset GetAssetById(string id)
        {
            if (id == null)
            {
                return null;
            }

            return _assetsById.TryGetValue(id, out var result) ? result : null;
        }

        public IEnumerator<Asset> GetEnumerator() => ((IEnumerable<Asset>)_assets).GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => ((IEnumerable<Asset>)_assets).GetEnumerator();
    }
}
