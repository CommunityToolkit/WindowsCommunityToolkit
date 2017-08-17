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

using Windows.Devices.Bluetooth.GenericAttributeProfile;

namespace Microsoft.Toolkit.Uwp.Connectivity
{
    /// <summary>
    /// Helper function when working with <see cref="GattProtocolError" />
    /// </summary>
    public static class GattProtocolErrorParser
    {
        /// <summary>
        /// Helper to convert an Gatt error value into a string
        /// </summary>
        /// <param name="errorValue"> the byte error value.</param>
        /// <returns>String representation of the error</returns>
        public static string GetErrorString(this byte? errorValue)
        {
            var errorString = "Protocol Error";

            if (errorValue.HasValue == false)
            {
                return errorString;
            }

            if (errorValue == GattProtocolError.AttributeNotFound)
            {
                return "Attribute Not Found";
            }

            if (errorValue == GattProtocolError.AttributeNotLong)
            {
                return "Attribute Not Long";
            }

            if (errorValue == GattProtocolError.InsufficientAuthentication)
            {
                return "Insufficient Authentication";
            }

            if (errorValue == GattProtocolError.InsufficientAuthorization)
            {
                return "Insufficient Authorization";
            }

            if (errorValue == GattProtocolError.InsufficientEncryption)
            {
                return "Insufficient Encryption";
            }

            if (errorValue == GattProtocolError.InsufficientEncryptionKeySize)
            {
                return "Insufficient Encryption Key Size";
            }

            if (errorValue == GattProtocolError.InsufficientResources)
            {
                return "Insufficient Resources";
            }

            if (errorValue == GattProtocolError.InvalidAttributeValueLength)
            {
                return "Invalid Attribute Value Length";
            }

            if (errorValue == GattProtocolError.InvalidHandle)
            {
                return "Invalid Handle";
            }

            if (errorValue == GattProtocolError.InvalidOffset)
            {
                return "Invalid Offset";
            }

            if (errorValue == GattProtocolError.InvalidPdu)
            {
                return "Invalid Pdu";
            }

            if (errorValue == GattProtocolError.PrepareQueueFull)
            {
                return "Prepare Queue Full";
            }

            if (errorValue == GattProtocolError.ReadNotPermitted)
            {
                return "Read Not Permitted";
            }

            if (errorValue == GattProtocolError.RequestNotSupported)
            {
                return "Request Not Supported";
            }

            if (errorValue == GattProtocolError.UnlikelyError)
            {
                return "UnlikelyError";
            }

            if (errorValue == GattProtocolError.UnsupportedGroupType)
            {
                return "Unsupported Group Type";
            }

            if (errorValue == GattProtocolError.WriteNotPermitted)
            {
                return "Write Not Permitted";
            }

            return errorString;
        }
    }
}