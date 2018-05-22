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

namespace Microsoft.Toolkit.Uwp.PlatformSpecificAnalyzer
{
    /// <summary>
    /// This class wraps a new data members
    /// </summary>
    public struct NewMember
    {
        private static char[] methodCountSeparator = { '#' };

        /// <summary>
        /// Member name
        /// </summary>
        public string Name;

        /// <summary>
        /// Parameter count (if its a method)
        /// </summary>
        public int? ParameterCount;

        /// <summary>
        /// Initializes a new instance of the <see cref="NewMember"/> struct.
        /// </summary>
        /// <param name="s">data containing name and optionally parameter count</param>
        public NewMember(string s)
        {
            string[] parts = s.Split(methodCountSeparator, StringSplitOptions.RemoveEmptyEntries);

            if (parts.Length == 2)
            {
                Name = parts[0];
                ParameterCount = int.Parse(parts[1]);
            }
            else
            {
                Name = s;
                ParameterCount = null;
            }
        }
    }
}
