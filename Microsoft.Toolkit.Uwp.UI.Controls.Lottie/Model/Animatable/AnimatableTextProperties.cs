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

namespace Microsoft.Toolkit.Uwp.UI.Controls.Lottie.Model.Animatable
{
    internal class AnimatableTextProperties
    {
        private readonly AnimatableColorValue color;
        private readonly AnimatableColorValue stroke;
        private readonly AnimatableFloatValue strokeWidth;
        private readonly AnimatableFloatValue tracking;

        internal AnimatableColorValue Color => color;

        internal AnimatableColorValue Stroke => stroke;

        internal AnimatableFloatValue StrokeWidth => strokeWidth;

        internal AnimatableFloatValue Tracking => tracking;

        public AnimatableTextProperties(AnimatableColorValue color, AnimatableColorValue stroke, AnimatableFloatValue strokeWidth, AnimatableFloatValue tracking)
        {
            this.color = color;
            this.stroke = stroke;
            this.strokeWidth = strokeWidth;
            this.tracking = tracking;
        }
    }
}
