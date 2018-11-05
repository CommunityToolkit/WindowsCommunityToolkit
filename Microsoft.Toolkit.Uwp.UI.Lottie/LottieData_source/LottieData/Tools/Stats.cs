// Copyright(c) Microsoft Corporation.All rights reserved.
// Licensed under the MIT License.

using System;
using System.Linq;

namespace Microsoft.Toolkit.Uwp.UI.Lottie.LottieData.Tools
{
    /// <summary>
    /// Calculates stats for a <see cref="LottieComposition"/>.
    /// </summary>
#if !WINDOWS_UWP
    public
#endif
    sealed class Stats
    {
        readonly Version _version;
        readonly string _name;
        readonly double _width;
        readonly double _height;
        readonly TimeSpan _duration;
        readonly int _preCompLayerCount;
        readonly int _solidLayerCount;
        readonly int _imageLayerCount;
        readonly int _maskCount;
        readonly int _nullLayerCount;
        readonly int _shapeLayerCount;
        readonly int _textLayerCount;
        readonly int _maskAdditiveCount;
        readonly int _maskDarkenCount;
        readonly int _maskDifferenceCount;
        readonly int _maskIntersectCount;
        readonly int _maskLightenCount;
        readonly int _maskSubtractiveCount;

        // Creates a string that describes the Lottie.
        public Stats(LottieComposition lottieComposition)
        {
            if (lottieComposition == null) { return; }

            _name = lottieComposition.Name;
            _version = lottieComposition.Version;
            _width = lottieComposition.Width;
            _height = lottieComposition.Height;
            _duration = lottieComposition.Duration;

            // Get the layers stored in assets.
            var layersInAssets =
                from asset in lottieComposition.Assets
                where asset.Type == Asset.AssetType.LayerCollection
                let layerCollection = (LayerCollectionAsset)asset
                from layer in layerCollection.Layers.GetLayersBottomToTop()
                select layer;

            foreach (var layer in lottieComposition.Layers.GetLayersBottomToTop().Concat(layersInAssets))
            {
                switch (layer.Type)
                {
                    case Layer.LayerType.PreComp:
                        _preCompLayerCount++;
                        break;
                    case Layer.LayerType.Solid:
                        _solidLayerCount++;
                        break;
                    case Layer.LayerType.Image:
                        _imageLayerCount++;
                        break;
                    case Layer.LayerType.Null:
                        _nullLayerCount++;
                        break;
                    case Layer.LayerType.Shape:
                        _shapeLayerCount++;
                        break;
                    case Layer.LayerType.Text:
                        _textLayerCount++;
                        break;
                    default:
                        throw new InvalidOperationException();
                }
                foreach (var mask in layer.Masks)
                {
                    _maskCount++;
                    switch (mask.Mode)
                    {
                        case Mask.MaskMode.Additive:
                            _maskAdditiveCount++;
                            break;
                        case Mask.MaskMode.Darken:
                            _maskDarkenCount++;
                            break;
                        case Mask.MaskMode.Difference:
                            _maskDifferenceCount++;
                            break;
                        case Mask.MaskMode.Intersect:
                            _maskIntersectCount++;
                            break;
                        case Mask.MaskMode.Lighten:
                            _maskLightenCount++;
                            break;
                        case Mask.MaskMode.Subtract:
                            _maskSubtractiveCount++;
                            break;
                    }
                }
                _maskCount += layer.Masks.Count();
            }
        }

        public int PreCompLayerCount => _preCompLayerCount;
        public int SolidLayerCount => _solidLayerCount;
        public int ImageLayerCount => _imageLayerCount;
        public int MaskCount => _maskCount;
        public int MaskAdditiveCount => _maskAdditiveCount;
        public int MaskDarkenCount => _maskDarkenCount;
        public int MaskDifferenceCount => _maskDifferenceCount;
        public int MaskIntersectCount => _maskIntersectCount;
        public int MaskLightenCount => _maskLightenCount;
        public int MaskSubtractCount => _maskSubtractiveCount;

        public int NullLayerCount => _nullLayerCount;
        public int ShapeLayerCount => _shapeLayerCount;
        public int TextLayerCount => _textLayerCount;
        public double Width => _width;
        public double Height => _height;
        public TimeSpan Duration => _duration;
        public string Name => _name;
        public Version Version => _version;
    }
}
