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

namespace Microsoft.Toolkit.Uwp.PlatformSpecific
{
    public struct NewMember
    {

        public string Name;

        public int? ParameterCount;

        public NewMember(string s)
        {
            if (s.Length > 2 && s[s.Length - 2] == '#')
            {
                Name = s.Substring(0, s.Length - 2);
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
