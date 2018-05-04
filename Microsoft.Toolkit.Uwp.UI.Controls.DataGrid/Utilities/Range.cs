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

namespace Microsoft.Toolkit.Uwp.Utilities
{
    internal class Range<T>
    {
        public Range(int lowerBound, int upperBound, T value)
        {
            LowerBound = lowerBound;
            UpperBound = upperBound;
            Value = value;
        }

        public int Count
        {
            get
            {
                return UpperBound - LowerBound + 1;
            }
        }

        public int LowerBound
        {
            get;
            set;
        }

        public int UpperBound
        {
            get;
            set;
        }

        public T Value
        {
            get;
            set;
        }

        public bool ContainsIndex(int index)
        {
            return LowerBound <= index && UpperBound >= index;
        }

        public bool ContainsValue(object value)
        {
            return (this.Value == null) ? value == null : this.Value.Equals(value);
        }

        public Range<T> Copy()
        {
            return new Range<T>(LowerBound, UpperBound, Value);
        }
    }
}
