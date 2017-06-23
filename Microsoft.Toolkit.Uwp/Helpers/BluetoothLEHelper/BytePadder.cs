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

namespace Microsoft.Toolkit.Uwp
{
    /// <summary>
    /// Helper class used to pad bytes
    /// </summary>
    public static class BytePadder
    {
        /// <summary>
        /// Takes an input array of bytes and returns an array with more zeros in the front
        /// </summary>
        /// <param name="input">a byte array.</param>
        /// <param name="length">the length of the data.</param>
        /// <returns>A byte array with more zeros in front"/></returns>
        public static byte[] GetBytes(byte[] input, int length)
        {
            var bytes = new byte[length];

            if (input.Length >= length)
            {
                return input;
            }

            for (var i = 0; i < input.Length; i++)
            {
                bytes[i] = input[i];
            }

            return bytes;
        }
    }
}