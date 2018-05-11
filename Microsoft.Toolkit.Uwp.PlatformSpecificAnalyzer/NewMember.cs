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
    public struct NewMember
    {
        private static char[] methodCountSeparator = { '#' };

        public string Name;

        public int? ParameterCount;

        public NewMember(string s)
        {
            string[] parts = s.Split(methodCountSeparator, StringSplitOptions.RemoveEmptyEntries);

            if (parts.Length == 2)
            {
                Name = parts[0];
                ParameterCount = int.Parse(s.Substring(s.Length - 1));
            }
            else
            {
                Name = s;
                ParameterCount = null;
            }
        }
    }
}
