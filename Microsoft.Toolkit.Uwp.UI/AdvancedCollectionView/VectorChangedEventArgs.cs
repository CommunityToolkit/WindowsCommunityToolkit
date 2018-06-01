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

using Windows.Foundation.Collections;

namespace Microsoft.Toolkit.Uwp.UI
{
    /// <summary>
    /// Vector changed EventArgs
    /// </summary>
    internal class VectorChangedEventArgs : IVectorChangedEventArgs
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="VectorChangedEventArgs"/> class.
        /// </summary>
        /// <param name="cc">collection change type</param>
        /// <param name="index">index of item changed</param>
        /// <param name="item">item changed</param>
        public VectorChangedEventArgs(CollectionChange cc, int index = -1, object item = null)
        {
            CollectionChange = cc;
            Index = (uint)index;
        }

        /// <summary>
        /// Gets the type of change that occurred in the vector.
        /// </summary>
        /// <returns>
        /// The type of change in the vector.
        /// </returns>
        public CollectionChange CollectionChange { get; }

        /// <summary>
        /// Gets the position where the change occurred in the vector.
        /// </summary>
        /// <returns>
        /// The zero-based position where the change occurred in the vector, if applicable.
        /// </returns>
        public uint Index { get; }
    }
}