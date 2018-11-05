// Copyright(c) Microsoft Corporation.All rights reserved.
// Licensed under the MIT License.

using System;
using System.Collections.Generic;
using System.Linq;

namespace Microsoft.Toolkit.Uwp.UI.Lottie.LottieData
{
#if !WINDOWS_UWP
    public
#endif
    sealed class LottieComposition : LottieObject
    {
        /// <summary>
        /// Creates a LottieComposition object. 
        /// </summary>
        /// <param name="name">The name of the composition</param>
        /// <param name="width">Width of animation canvas as specified in AfterEffects.</param>
        /// <param name="height">Height of animation canvas as specified in AfterEffects.</param>
        /// <param name="inPoint">Frame at which animation begins as specified in AfterEffects.</param>
        /// <param name="outPoint">Frame at which animation ends as specified in AfterEffects.</param>
        /// <param name="framesPerSecond">FrameRate (frames per second) at which animation data was generated in AfterEffects.</param>
        /// <param name="is3d">True if the composition is 3d.</param>
        /// <param name="version">The version of the schema of the composition.</param>
        /// <param name="assets">Assets that are part of the composition.</param>
        /// <param name="char">Character definitions that are part of the composition.</param>
        /// <param name="layers">The layers in the composition.</param>
        /// <param name="markers">Markers that define named portions of the composition.</param>
        public LottieComposition(
            string name,
            double width,
            double height,
            double inPoint,
            double outPoint,
            double framesPerSecond,
            bool is3d,
            Version version,
            AssetCollection assets,
            IEnumerable<Char> chars,
            IEnumerable<Font> fonts,
            LayerCollection layers,
            IEnumerable<Marker> markers) : base(name)
        {
            Is3d = is3d;
            Width = width;
            Height = height;
            InPoint = inPoint;
            OutPoint = outPoint;
            FramesPerSecond = framesPerSecond;
            Duration = TimeSpan.FromSeconds(((outPoint - inPoint) / framesPerSecond));
            Version = version;
            Layers = layers;
            Assets = assets;
            Chars = chars.ToArray();
            Fonts = fonts.ToArray();
            Markers = markers.ToArray();
        }

        public bool Is3d { get; }
        public double FramesPerSecond { get; }
        public double Width { get; }
        public double Height { get; }

        /// <summary>
        /// The frame at which the animation begins.
        /// </summary>
        public double InPoint { get; }

        /// <summary>
        /// The frame at which the animation ends.
        /// </summary>
        public double OutPoint { get; }
        public IEnumerable<Char> Chars { get; }
        public IEnumerable<Font> Fonts { get; }
        public IEnumerable<Marker> Markers { get; }
        public TimeSpan Duration { get; }
        public AssetCollection Assets { get; }
        public LayerCollection Layers { get; }

        public override LottieObjectType ObjectType => LottieObjectType.LottieComposition;
        /// <summary>
        /// Lottie version.
        /// </summary>
        public Version Version { get; }
    }
}
