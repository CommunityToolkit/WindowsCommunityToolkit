// ******************************************************************
// Copyright (c) Microsoft. All rights reserved.
// This code is licensed under the MIT License (MIT).
// THE CODE IS PROVIDED “AS IS”, WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED,
// INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.
// IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM,
// DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT,
// TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH
// THE CODE OR THE USE OR OTHER DEALINGS IN THE CODE.
// ******************************************************************

using System.Collections.Generic;
using System.Globalization;
using System.Text;
using Microsoft.Toolkit.Uwp.UI.Controls.Lottie.Model.Animatable;
using Microsoft.Toolkit.Uwp.UI.Controls.Lottie.Model.Content;
using Microsoft.Toolkit.Uwp.UI.Controls.Lottie.Value;
using Windows.UI;

namespace Microsoft.Toolkit.Uwp.UI.Controls.Lottie.Model.Layer
{
    /// <summary>
    /// Represents one layer of the composition of layers of an animation. Used mainly for debug purposes.
    /// </summary>
    public class Layer
    {
        internal enum LayerType
        {
            PreComp,
            Solid,
            Image,
            Null,
            Shape,
            Text,
            Unknown
        }

        internal enum MatteType
        {
            None,
            Add,
            Invert,
            Unknown
        }

        private readonly List<IContentModel> _shapes;
        private readonly LottieComposition _composition;
        private readonly LayerType _layerType;
        private readonly MatteType _matteType;

        internal Layer(List<IContentModel> shapes, LottieComposition composition, string layerName, long layerId, LayerType layerType, long parentId, string refId, List<Mask> masks, AnimatableTransform transform, int solidWidth, int solidHeight, Color solidColor, float timeStretch, float startFrame, int preCompWidth, int preCompHeight, AnimatableTextFrame text, AnimatableTextProperties textProperties, List<Keyframe<float?>> inOutKeyframes, MatteType matteType, AnimatableFloatValue timeRemapping)
        {
            _shapes = shapes;
            _composition = composition;
            Name = layerName;
            Id = layerId;
            _layerType = layerType;
            ParentId = parentId;
            RefId = refId;
            Masks = masks;
            Transform = transform;
            SolidWidth = solidWidth;
            SolidHeight = solidHeight;
            SolidColor = solidColor;
            TimeStretch = timeStretch;
            StartFrame = startFrame;
            PreCompWidth = preCompWidth;
            PreCompHeight = preCompHeight;
            Text = text;
            TextProperties = textProperties;
            InOutKeyframes = inOutKeyframes;
            _matteType = matteType;
            TimeRemapping = timeRemapping;
        }

        internal virtual LottieComposition Composition => _composition;

        internal virtual float TimeStretch { get; }

        internal virtual float StartFrame { get; }

        internal virtual float StartProgress => StartFrame / _composition.DurationFrames;

        internal virtual List<Keyframe<float?>> InOutKeyframes { get; }

        /// <summary>
        /// Gets the Id of the Layer
        /// </summary>
        public virtual long Id { get; }

        internal virtual string Name { get; }

        internal virtual string RefId { get; }

        internal virtual int PreCompWidth { get; }

        internal virtual int PreCompHeight { get; }

        internal virtual AnimatableTextFrame Text { get; }

        internal virtual AnimatableTextProperties TextProperties { get; }

        internal virtual AnimatableFloatValue TimeRemapping { get; }

        internal virtual List<Mask> Masks { get; }

        internal virtual LayerType GetLayerType()
        {
            return _layerType;
        }

        internal virtual MatteType GetMatteType()
        {
            return _matteType;
        }

        internal virtual long ParentId { get; }

        internal virtual List<IContentModel> Shapes => _shapes;

        internal virtual AnimatableTransform Transform { get; }

        internal virtual Color SolidColor { get; }

        internal virtual int SolidHeight { get; }

        internal virtual int SolidWidth { get; }

        /// <summary>
        /// Returns the debug information of the Layer
        /// </summary>
        /// <returns>The debug information of the Layer</returns>
        public override string ToString()
        {
            return ToString(string.Empty);
        }

        internal virtual string ToString(string prefix)
        {
            var sb = new StringBuilder();
            sb.Append(prefix).Append(Name).Append("\n");
            var parent = _composition.LayerModelForId(ParentId);
            if (parent != null)
            {
                sb.Append("\t\tParents: ").Append(parent.Name);
                parent = _composition.LayerModelForId(parent.ParentId);
                while (parent != null)
                {
                    sb.Append("->").Append(parent.Name);
                    parent = _composition.LayerModelForId(parent.ParentId);
                }

                sb.Append(prefix).Append("\n");
            }

            if (Masks.Count > 0)
            {
                sb.Append(prefix).Append("\tMasks: ").Append(Masks.Count).Append("\n");
            }

            if (SolidWidth != 0 && SolidHeight != 0)
            {
                sb.Append(prefix).Append("\tBackground: ").Append(string.Format(CultureInfo.InvariantCulture, "{0}x{1} {2}\n", SolidWidth, SolidHeight, SolidColor));
            }

            if (_shapes.Count > 0)
            {
                sb.Append(prefix).Append("\tShapes:\n");
                foreach (var shape in _shapes)
                {
                    sb.Append(prefix).Append("\t\t").Append(shape).Append("\n");
                }
            }

            return sb.ToString();
        }
    }
}