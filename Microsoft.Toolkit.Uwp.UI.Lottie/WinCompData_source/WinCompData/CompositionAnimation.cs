// Copyright(c) Microsoft Corporation.All rights reserved.
// Licensed under the MIT License.

using System;
using System.Collections.Generic;

namespace Microsoft.Toolkit.Uwp.UI.Lottie.WinCompData
{
#if !WINDOWS_UWP
    public
#endif
    abstract class CompositionAnimation : CompositionObject
    {
        readonly Dictionary<string, CompositionObject> _referencedParameters = new Dictionary<string, CompositionObject>();

        protected private CompositionAnimation(CompositionAnimation other)
        {
            if (other != null)
            {
                foreach (var pair in other._referencedParameters)
                {
                    _referencedParameters.Add(pair.Key, pair.Value);
                }
                Target = other.Target;

                LongDescription = other.LongDescription;
                ShortDescription = other.ShortDescription;
                Comment = other.Comment;
            }
        }

        public string Target { get; set; }

        // True iff this object's state is expected to never change.
        public bool IsFrozen { get; private set; }

        /// <summary>
        /// Marks the <see cref="CompositionAnimation"/> to indicate that its state 
        /// should never change again. Note that this is a weak guarantee as there 
        /// are not checks on all mutators to ensure that changes aren't made after 
        /// freezing. However correct code must never mutate a frozen object.
        ///</summary>
        public void Freeze()
        {
            IsFrozen = true;
        }

        public void SetReferenceParameter(string key, CompositionObject compositionObject)
        {
            if (IsFrozen)
            {
                throw new InvalidOperationException();
            }
            _referencedParameters.Add(key, compositionObject);
        }

        public IEnumerable<KeyValuePair<string, CompositionObject>> ReferenceParameters => _referencedParameters;

        internal abstract CompositionAnimation Clone();
    }
}
