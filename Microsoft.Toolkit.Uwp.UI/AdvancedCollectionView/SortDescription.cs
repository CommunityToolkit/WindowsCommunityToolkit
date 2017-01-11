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

namespace Microsoft.Toolkit.Uwp.UI
{
    /// <summary>
    /// Sort description
    /// </summary>
    public class SortDescription
    {
        /// <summary>
        /// Gets the name of property to sort on
        /// </summary>
        public string PropertyName { get; private set; }

        /// <summary>
        /// Gets the direction of sort
        /// </summary>
        public SortDirection Direction { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="SortDescription"/> class.
        /// </summary>
        /// <param name="propertyName">name of property to sort on</param>
        /// <param name="direction">direction of sort</param>
        public SortDescription(string propertyName, SortDirection direction)
        {
            PropertyName = propertyName;
            Direction = direction;
        }
    }
}