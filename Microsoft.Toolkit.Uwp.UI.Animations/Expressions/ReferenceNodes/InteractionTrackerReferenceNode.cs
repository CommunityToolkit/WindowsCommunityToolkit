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

using Windows.UI.Composition.Interactions;

namespace Microsoft.Toolkit.Uwp.UI.Animations.Expressions
{
    public sealed class InteractionTrackerReferenceNode : ReferenceNode
    {
        internal InteractionTrackerReferenceNode(string paramName, InteractionTracker it = null)
            : base(paramName, it)
        {
        }

        internal static InteractionTrackerReferenceNode CreateTargetReference()
        {
            var node = new InteractionTrackerReferenceNode(null);
            node.NodeType = ExpressionNodeType.TargetReference;

            return node;
        }

        // Animatable properties
        public BooleanNode IsPositionRoundingSuggested
        {
            get { return ReferenceProperty<BooleanNode>("IsPositionRoundingSuggested"); }
        }

        public ScalarNode MinScale
        {
            get { return ReferenceProperty<ScalarNode>("MinScale"); }
        }

        public ScalarNode MaxScale
        {
            get { return ReferenceProperty<ScalarNode>("MaxScale"); }
        }

        public ScalarNode NaturalRestingScale
        {
            get { return ReferenceProperty<ScalarNode>("NaturalRestingScale"); }
        }

        public ScalarNode Scale
        {
            get { return ReferenceProperty<ScalarNode>("Scale"); }
        }

        public ScalarNode ScaleInertiaDecayRate
        {
            get { return ReferenceProperty<ScalarNode>("ScaleInertiaDecayRate"); }
        }

        public ScalarNode ScaleVelocityInPercentPerSecond
        {
            get { return ReferenceProperty<ScalarNode>("ScaleVelocityInPercentPerSecond"); }
        }

        public Vector3Node MinPosition
        {
            get { return ReferenceProperty<Vector3Node>("MinPosition"); }
        }

        public Vector3Node MaxPosition
        {
            get { return ReferenceProperty<Vector3Node>("MaxPosition"); }
        }

        public Vector3Node NaturalRestingPosition
        {
            get { return ReferenceProperty<Vector3Node>("NaturalRestingPosition"); }
        }

        public Vector3Node Position
        {
            get { return ReferenceProperty<Vector3Node>("Position"); }
        }

        public Vector3Node PositionInertiaDecayRate
        {
            get { return ReferenceProperty<Vector3Node>("PositionInertiaDecayRate"); }
        }

        public Vector3Node PositionVelocityInPixelsPerSecond
        {
            get { return ReferenceProperty<Vector3Node>("PositionVelocityInPixelsPerSecond"); }
        }
    }
}