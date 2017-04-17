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

using System;

namespace Microsoft.Toolkit.Uwp.UI.Controls
{
    /// <summary>
    /// Event args for a value changing event
    /// </summary>
    public class RangeChangedEventArgs : EventArgs
    {
        /// <summary>
        /// Gets the old value.
        /// </summary>
        public double OldValue { get; private set; }

        /// <summary>
        /// Gets the new value.
        /// </summary>
        public double NewValue { get; private set; }

        /// <summary>
        /// Gets the range property that triggered the event
        /// </summary>
        public RangeSelectorProperty ChangedRangeProperty { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="RangeChangedEventArgs"/> class.
        /// </summary>
        /// <param name="oldValue">The old value</param>
        /// <param name="newValue">The new value</param>
        /// <param name="changedRangeProperty">The changed range property</param>
        public RangeChangedEventArgs(double oldValue, double newValue, RangeSelectorProperty changedRangeProperty)
        {
            OldValue = oldValue;
            NewValue = newValue;
            ChangedRangeProperty = changedRangeProperty;
        }
    }

    /// <summary>
    /// Enumeration used to determine what value triggered ValueChanged event on the
    /// RangeSelector
    /// </summary>
    public enum RangeSelectorProperty
    {
        /// <summary>
        /// Minimum value was changed
        /// </summary>
        MinimumValue,

        /// <summary>
        /// Maximum value was changed
        /// </summary>
        MaximumValue
    }
}
