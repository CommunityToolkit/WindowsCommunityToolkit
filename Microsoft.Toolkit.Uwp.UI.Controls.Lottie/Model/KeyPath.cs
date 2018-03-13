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
using System.Linq;

namespace Microsoft.Toolkit.Uwp.UI.Controls.Lottie.Model
{
    /// <summary>
    /// Defines which content to target.
    /// The keypath can contain wildcards ('*') with match exactly 1 item.
    /// or globstars ('**') which match 0 or more items.
    ///
    /// For example, if your content were arranged like this:
    /// Gabriel (Shape Layer)
    ///     Body (Shape Group)
    ///         Left Hand (Shape)
    ///             Fill (Fill)
    ///             Transform (Transform)
    ///         ...
    /// Brandon (Shape Layer)
    ///     Body (Shape Group)
    ///         Left Hand (Shape)
    ///             Fill (Fill)
    ///             Transform (Transform)
    ///         ...
    ///
    ///
    /// You could:
    ///     Match Gabriel left hand fill:
    ///        new KeyPath("Gabriel", "Body", "Left Hand", "Fill");
    ///     Match Gabriel and Brandon's left hand fill:
    ///        new KeyPath("*", "Body", Left Hand", "Fill");
    ///     Match anything with the name Fill:
    ///        new KeyPath("**", "Fill");
    ///
    ///
    /// NOTE: Content that are part of merge paths or repeaters cannot currently be resolved with
    /// a <see cref="KeyPath"/>. This may be fixed in the future.
    /// </summary>
    public class KeyPath
    {
        private readonly List<string> _keys;
        private IKeyPathElement _resolvedElement;

        /// <summary>
        /// Initializes a new instance of the <see cref="KeyPath"/> class.
        /// </summary>
        /// <param name="keys">The keys that the KeyPath will match.</param>
        public KeyPath(params string[] keys)
        {
            _keys = keys.ToList();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="KeyPath"/> class.
        /// Copy constructor. Copies keys as well.
        /// </summary>
        /// <param name="keyPath">The original <see cref="KeyPath"/> that should be copied.</param>
        private KeyPath(KeyPath keyPath)
        {
            _keys = new List<string>(keyPath._keys);
            _resolvedElement = keyPath._resolvedElement;
        }

        /// <summary>
        /// This is used during keypath resolution. Children normally don't know about all of their parent
        /// elements so this is used to keep track of the fully qualified keypath.
        /// This returns a key keypath because during resolution, the full keypath element tree is walked
        /// and if this modified the original copy, it would remain after popping back up the element tree.
        /// </summary>
        /// <param name="key">The key</param>
        /// <returns>Returns a new KeyPath with the key added.</returns>
        internal KeyPath AddKey(string key)
        {
            KeyPath newKeyPath = new KeyPath(this);
            newKeyPath._keys.Add(key);
            return newKeyPath;
        }

        /// <summary>
        /// Method to get a KeyPath with the element resolved to the specified <see cref="IKeyPathElement"/>.
        /// </summary>
        /// <param name="element">The <see cref="IKeyPathElement"/> that the returned <see cref="KeyPath"/> will point to.</param>
        /// <returns>Returns the new KeyPath with the element resolved to the specified <see cref="IKeyPathElement"/>.</returns>
        internal KeyPath Resolve(IKeyPathElement element)
        {
            KeyPath keyPath = new KeyPath(this)
            {
                _resolvedElement = element
            };
            return keyPath;
        }

        /// <summary>
        /// KeyPaths get resolved with resolveKeyPath on LottieDrawable or LottieAnimationView.
        /// </summary>
        /// <returns>Returns a <see cref="IKeyPathElement"/> that this has been resolved to</returns>
        internal IKeyPathElement GetResolvedElement()
        {
            return _resolvedElement;
        }

        /// <summary>
        /// Method to verify if key matches at the specified depth.
        /// </summary>
        /// <param name="key">The key</param>
        /// <param name="depth">The specific depth that should be verified</param>
        /// <returns>Returns whether the key matches at the specified depth.</returns>
        internal bool Matches(string key, int depth)
        {
            if (IsContainer(key))
            {
                // This is an artificial layer we programatically create.
                return true;
            }

            if (depth >= _keys.Count)
            {
                return false;
            }

            if (_keys[depth].Equals(key) ||
                _keys[depth].Equals("**") ||
                _keys[depth].Equals("*"))
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// For a given key and depth, returns how much the depth should be incremented by when
        /// resolving a keypath to children.
        /// This can be 0 or 2 when there is a globstar and the next key either matches or doesn't match
        /// the current key.
        /// </summary>
        /// <param name="key">The key</param>
        /// <param name="depth">The specific depth that should be verified</param>
        /// <returns>Returns how much the depth should be incremented by when resolving a keypath to children</returns>
        internal int IncrementDepthBy(string key, int depth)
        {
            if (IsContainer(key))
            {
                // If it's a container then we added programatically and it isn't a part of the keypath.
                return 0;
            }

            if (!_keys[depth].Equals("**"))
            {
                // If it's not a globstar then it is part of the keypath.
                return 1;
            }

            if (depth == _keys.Count - 1)
            {
                // The last key is a globstar.
                return 0;
            }

            if (_keys[depth + 1].Equals(key))
            {
                // We are a globstar and the next key is our current key so consume both.
                return 2;
            }

            return 0;
        }

        /// <summary>
        /// Returns whether the key at specified depth is fully specific enough to match the full set of
        /// keys in this keypath.
        /// </summary>
        /// <param name="key">The key</param>
        /// <param name="depth">The specific depth that should be verified</param>
        /// <returns>Returns a bool that indicates whether the key at specified depth is fully specific enough to match the full set of keys in this keypath.</returns>
        internal bool FullyResolvesTo(string key, int depth)
        {
            if (depth >= _keys.Count)
            {
                return false;
            }

            bool isLastDepth = depth == _keys.Count - 1;
            string keyAtDepth = _keys[depth];
            bool isGlobstar = keyAtDepth.Equals("**");

            if (!isGlobstar)
            {
                bool matches = keyAtDepth.Equals(key) || keyAtDepth.Equals("*");
                return (isLastDepth || (depth == _keys.Count - 2 && EndsWithGlobstar())) && matches;
            }

            bool isGlobstarButNextKeyMatches = !isLastDepth && _keys[depth + 1].Equals(key);
            if (isGlobstarButNextKeyMatches)
            {
                return depth == _keys.Count - 2 ||
                    (depth == _keys.Count - 3 && EndsWithGlobstar());
            }

            if (isLastDepth)
            {
                return true;
            }

            if (depth + 1 < _keys.Count - 1)
            {
                // We are a globstar but there is more than 1 key after the globstar we we can't fully match.
                return false;
            }

            // Return whether the next key (which we now know is the last one) is the same as the current
            // key.
            return _keys[depth + 1].Equals(key);
        }

        /// <summary>
        /// Returns whether the keypath resolution should propagate to children. Some keypaths resolve
        /// to content other than leaf contents(such as a layer or content group transform) so sometimes
        /// this will return false.
        /// </summary>
        /// <param name="key">The key</param>
        /// <param name="depth">The specific depth that should be verified</param>
        /// <returns>Returns whether the keypath resolution should propagate to children.</returns>
        internal bool PropagateToChildren(string key, int depth)
        {
            if (key.Equals("__container"))
            {
                return true;
            }

            return depth < _keys.Count - 1 || _keys[depth].Equals("**");
        }

        /// <summary>
        /// We artificially create some container groups (like a root ContentGroup for the entire animation
        /// and for the contents of a ShapeLayer).
        /// </summary>
        /// <param name="key">The key</param>
        /// <returns>Returns whether this key is the main container one.</returns>
        private bool IsContainer(string key)
        {
            return key.Equals("__container");
        }

        private bool EndsWithGlobstar()
        {
            return _keys[_keys.Count - 1].Equals("**");
        }

        /// <summary>
        /// Return the string representation of the internal keys.
        /// </summary>
        /// <returns>The string representation of the internal keys.</returns>
        public string KeysToString()
        {
            return _keys.ToString();
        }

        /// <summary>
        /// For debuging purposes.
        /// </summary>
        /// <returns>A formated text to help understand the current KeyPath.</returns>
        public override string ToString()
        {
            return $"KeyPath{{keys={_keys},resolved={_resolvedElement != null}}}";
        }
    }
}
