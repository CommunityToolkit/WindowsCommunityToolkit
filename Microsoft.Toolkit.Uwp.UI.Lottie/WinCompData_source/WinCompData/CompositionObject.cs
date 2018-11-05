// Copyright(c) Microsoft Corporation.All rights reserved.
// Licensed under the MIT License.

using Microsoft.Toolkit.Uwp.UI.Lottie.WinCompData.Tools;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Microsoft.Toolkit.Uwp.UI.Lottie.WinCompData
{
#if !WINDOWS_UWP
    public
#endif
    abstract class CompositionObject : IDisposable, IDescribable
    {
        readonly ListOfNeverNull<Animator> _animators = new ListOfNeverNull<Animator>();
        CompositionPropertySet _propertySet;

        protected private CompositionObject()
        {
            if (Type == CompositionObjectType.CompositionPropertySet)
            {
                // The property set on a property set is itself. 
                _propertySet = (CompositionPropertySet)this;
            }
        }

        public string Comment { get; set; }

        /// <summary>
        /// A description of the object. This may be used to add comments to generated code.
        /// Cf. the <see cref="Comment"/> property which is a property on real composition
        /// objects that is used for debugging.
        /// </summary>
        public string LongDescription { get; set; }

        /// <summary>
        /// A description of the object. This may be used to add comments to generated code.
        /// Cf. the <see cref="Comment"/> property which is a property on real composition
        /// objects that is used for debugging.
        /// </summary>
        public string ShortDescription { get; set; }

        public CompositionPropertySet Properties
        {
            get
            {
                if (_propertySet == null) { _propertySet = new CompositionPropertySet(this); }
                return _propertySet;
            }
        }

        /// <summary>
        /// Binds an animation to a property.
        /// </summary>
        /// <param name="target">The name of the property.</param>
        /// <param name="animation">The animation</param>
        public void StartAnimation(string target, CompositionAnimation animation)
        {
            // Clone the animation so that the existing animation object can be reconfigured.
            // If the animation is frozen, it is safe to not do the clone.
            var clone = animation.IsFrozen ? animation : animation.Clone();

            var animator = new Animator
            {
                Animation = clone,
                AnimatedProperty = target,
                AnimatedObject = this,
            };

            if (!(animation is ExpressionAnimation))
            {
                animator.Controller = new AnimationController(this, target);
            }
            _animators.Add(animator);
        }

        /// <summary>
        /// The animators that are bound to this object.
        /// </summary>
        public IEnumerable<Animator> Animators => _animators;

        public AnimationController TryGetAnimationController(string target) =>
            _animators.Where(a => a.AnimatedProperty == target).Single().Controller;

        public abstract CompositionObjectType Type { get; }
        public void Dispose()
        {
        }

        /// <summary>
        /// An animation bound to a property on this object.
        /// </summary>
        public sealed class Animator
        {
            /// <summary>
            /// The property being animated by this animator.
            /// </summary>
            public string AnimatedProperty { get; internal set; }
            /// <summary>
            /// The object whose property is being animated by this animator.
            /// </summary>
            public CompositionObject AnimatedObject { get; internal set; }
            public CompositionAnimation Animation { get; internal set; }
            public AnimationController Controller { get; internal set; }
            public override string ToString() => $"{Animation.Type} bound to {AnimatedProperty}";
        }
    }
}
