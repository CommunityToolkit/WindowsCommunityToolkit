// Copyright(c) Microsoft Corporation.All rights reserved.
// Licensed under the MIT License.

using System.Collections.Generic;
using System.Linq;

namespace Microsoft.Toolkit.Uwp.UI.Lottie.LottieData
{
    /// <summary>
    /// A collection of <see cref="Layer"/>s in drawing order.
    /// </summary>
#if !WINDOWS_UWP
    public
#endif
    sealed class LayerCollection
    {
        readonly Dictionary<int, Layer> _layersById;

        public LayerCollection(IEnumerable<Layer> layers)
        {
            _layersById = layers.ToDictionary(layer => layer.Index);
        }

        /// <summary>
        /// Returns the <see cref="Layer"/>s in the <see cref="LayerContainer"/> in
        /// painting order.
        /// </summary>
        public IEnumerable<Layer> GetLayersBottomToTop() => _layersById.Values.OrderByDescending(layer => layer.Index);

        /// <summary>
        /// Returns the <see cref="Layer"/> with the given id, or null if no matching <see cref="Layer"/> is found.
        /// </summary>
        public Layer GetLayerById(int? id)
        {
            if (!id.HasValue)
            {
                return null;
            }
            return _layersById.TryGetValue(id.Value, out var result) ? result : null;
        }
    }
}
