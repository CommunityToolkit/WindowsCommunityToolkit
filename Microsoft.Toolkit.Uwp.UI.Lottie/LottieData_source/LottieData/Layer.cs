// Copyright(c) Microsoft Corporation.All rights reserved.
// Licensed under the MIT License.

using System.Collections.Generic;
using System.Linq;

namespace Microsoft.Toolkit.Uwp.UI.Lottie.LottieData
{
    /// <summary>
    /// Base class for layer objects. 
    /// </summary>
    /// <remarks>
    /// Each <see cref="Layer"/>, apart from the root <see cref="PreCompLayer"/> belongs to a <see cref="PreCompLayer"/> and has 
    /// an index that determines its rendering order, and is also used to identify it as the owner of a set of transforms that
    /// can be inherited by other <see cref="Layer"/>s.</remarks>
#if !WINDOWS_UWP
    public
#endif
    abstract class Layer : LottieObject
    {
        static Mask[] _emptyMasks = new Mask[0];
        readonly Mask[] _masks;

        protected private Layer(
            string name,
            int index,
            int? parent,
            bool isHidden,
            Transform transform,
            double timeStretch,
            double startFrame,
            double inFrame,
            double outFrame,
            BlendMode blendMode,
            bool is3d,
            bool autoOrient,
            IEnumerable<Mask> masks) : base(name)
        {
            Index = index;
            Parent = parent;
            IsHidden = isHidden;
            Transform = transform;
            TimeStretch = timeStretch;
            StartTime = startFrame;
            InPoint = inFrame;
            OutPoint = outFrame;
            BlendMode = blendMode;
            Is3d = is3d;
            AutoOrient = autoOrient;
            _masks = masks != null ? masks.ToArray() : null;
        }

        public bool AutoOrient { get; }

        public bool IsHidden { get; }

        public BlendMode BlendMode { get; }

        /// <summary>
        /// The frame at which this <see cref="Layer"/> starts playing. May be negative.
        /// </summary>
        /// <remarks><see cref="Layer"/>s all start together.</remarks>
        public double StartTime { get; }

        /// <summary>
        /// The frame at which this <see cref="Layer"/> becomes visible. <see cref="OutPoint"/>.
        /// </summary>
        public double InPoint { get; }

        /// <summary>
        /// The frame at which this <see cref="Layer"/> becomes invisible. <see cref="OutPoint"/>.
        /// </summary>
        public double OutPoint { get; }

        public double TimeStretch { get; }

        public abstract LayerType Type { get; }

        public Transform Transform { get; }

        public bool Is3d { get; }

        /// <summary>
        /// Used to uniquely identify a <see cref="Layer"/> within the owning <see cref="LayerContainer"/>.
        /// </summary>
        internal int Index { get; }

        /// <summary>
        /// Identifies the index of the <see cref="Layer"/> from which transforms are inherited,
        /// or null if no transforms are inherited.
        /// </summary>
        public int? Parent { get; }

        /// <summary>
        /// List of masks appplied to the layer
        /// </summary>
        public IEnumerable<Mask> Masks => _masks == null ? _emptyMasks : _masks;

        public enum LayerType
        {
            PreComp,
            Solid,
            Image,
            Null,
            Shape,
            Text,
        }
    }
}
