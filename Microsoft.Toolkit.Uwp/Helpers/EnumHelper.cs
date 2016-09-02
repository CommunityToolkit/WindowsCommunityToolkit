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
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Microsoft.Toolkit.Uwp
{
    /// <summary>
    /// This class provides static helper methods for enumerations.
    /// </summary>
    public class SelectList
    {
        /// <summary>
        /// Returns a list containing the values of an enumerable.
        /// </summary>
        /// <typeparam name="TEnum">The enumeration type.</typeparam>
        /// <returns>List of enum values</returns>
        public static List<TEnum> Of<TEnum>()
            where TEnum : struct, IComparable, IFormattable, IConvertible
        {
            var t = typeof(TEnum);
            var typeInfos = t.GetTypeInfo();

            if (typeInfos.IsEnum)
            {
                var values = Enum.GetValues(typeof(TEnum)).Cast<TEnum>().ToList();
                if (values != null)
                {
                    return values;
                }
            }

            return null;
        }
    }
}
