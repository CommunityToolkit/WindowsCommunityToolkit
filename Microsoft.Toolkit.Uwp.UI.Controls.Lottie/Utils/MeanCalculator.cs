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

namespace Microsoft.Toolkit.Uwp.UI.Controls.Lottie.Utils
{
    /// <summary>
    /// Class to calculate the average in a stream of numbers on a continuous basis.
    /// </summary>
    public class MeanCalculator
    {
        private float _sum;
        private int _n;

        /// <summary>
        /// Adds a value to the mean
        /// </summary>
        /// <param name="number">The number to be added to the mean</param>
        public virtual void Add(float number)
        {
            _sum += number;
            _n++;
            if (_n == int.MaxValue)
            {
                _sum /= 2f;
                _n /= 2;
            }
        }

        /// <summary>
        /// Gets the current mean of all values added using the <see cref="Add(float)"/> method.
        /// </summary>
        public virtual float Mean
        {
            get
            {
                if (_n == 0)
                {
                    return 0;
                }

                return _sum / _n;
            }
        }
    }
}
