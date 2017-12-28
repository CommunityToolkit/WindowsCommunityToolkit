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

using Windows.System.Profile;

namespace Microsoft.Toolkit.Uwp.Helpers
{
    /// <summary>
    /// Defines Operating System version
    /// </summary>
    public struct OSVersion
    {
        /// <summary>
        /// Value describing major version
        /// </summary>
        public ushort Major;

        /// <summary>
        /// Value describing minor version
        /// </summary>
        public ushort Minor;

        /// <summary>
        /// Value describing build
        /// </summary>
        public ushort Build;

        /// <summary>
        /// Value describing revision
        /// </summary>
        public ushort Revision;

        /// <summary>
        /// Converts OSVersion to string
        /// </summary>
        /// <returns>Major.Minor.Build.Revision as a string</returns>
        public override string ToString()
        {
            return $"{Major}.{Minor}.{Build}.{Revision}";
        }
    }
}
