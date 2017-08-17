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

namespace Microsoft.Toolkit.Uwp.Connectivity
{
    /// <summary>
    /// Helper class used when working with UUIDs
    /// </summary>
    public static class GattUuidsService
    {
        /// <summary>
        /// Helper function to convert a UUID to a name
        /// </summary>
        /// <param name="uuid">The UUID guid.</param>
        /// <returns>Name of the UUID</returns>
        public static string ConvertUuidToName(Guid uuid)
        {
            GattNativeUuid name;

            if (Enum.TryParse(ConvertUuidToShortId(uuid).ToString(), out name))
            {
                return name.ToString();
            }

            return uuid.ToString();
        }

        /// <summary>
        /// Converts from standard 128bit UUID to the assigned 32bit UUIDs. Makes it easy to compare services
        /// that devices expose to the standard list.
        /// </summary>
        /// <param name="uuid">UUID to convert to 32 bit</param>
        /// <returns>32bit version of the input UUID</returns>
        public static ushort ConvertUuidToShortId(Guid uuid)
        {
            var bytes = uuid.ToByteArray();
            var shortUuid = (ushort)(bytes[0] | (bytes[1] << 8));

            return shortUuid;
        }
    }
}
