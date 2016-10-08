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

using System.Text;

namespace Microsoft.Toolkit.Uwp.Services.MicrosoftGraph
{
    /// <summary>
    /// Microsoft Graph Helper
    /// </summary>
    internal class MicrosoftGraphHelper
    {
        /// <summary>
        /// Build string with an array's items
        /// </summary>
        /// <typeparam name='T'>enum type</typeparam>
        /// <param name='t'>an array of enum containing the fields</param>
        /// <returns>a string with all fields separate by a comma.</returns>
        internal static string BuildString<T>(T[] t)
        {
            StringBuilder sb = new StringBuilder();

            foreach (var field in t)
            {
                sb.Append(field.ToString());
                sb.Append(',');
            }

            string tempo = sb.ToString();

            // Remove the trailing comma character
            int lastPosition = tempo.Length - 1;

            return tempo.Substring(0, lastPosition);
        }
    }
}
