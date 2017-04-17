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
using System.Collections;

namespace Microsoft.Toolkit.Uwp.UI.Converters
{
    /// <summary>
    /// This class converts a collection size into an other object.
    /// Can be used to convert to bind a visibility, a color or an image to the size of the collection.
    /// </summary>
    public class EmptyCollectionToObjectConverter : EmptyObjectToObjectConverter
    {
        /// <summary>
        /// Checks collection for emptiness.
        /// </summary>
        /// <param name="value">Value to be checked.</param>
        /// <returns>True if value is an empty collection or does not implement IEnumerable, false otherwise.</returns>
        protected override bool CheckValueIsEmpty(object value)
        {
            bool isEmpty = true;
            var collection = value as IEnumerable;
            if (collection != null)
            {
                var enumerator = collection.GetEnumerator();
                isEmpty = !enumerator.MoveNext();
            }

            return isEmpty;
        }
    }
}