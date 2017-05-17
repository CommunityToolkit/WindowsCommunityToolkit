using System;
using System.Text;
using Windows.Security.Cryptography;
using Windows.Storage.Streams;

namespace Microsoft.Toolkit.Uwp
{
    /// <summary>
    /// Extension methods for Gatt Convert.
    /// </summary>
    public static class GattConvert
    {
        /// <summary>
        /// Convert the string to an IBuffer
        /// </summary>
        /// <param name="data">The data.</param>
        /// <returns>An IBuffer of data.</returns>
        public static IBuffer ToIBuffer(string data)
        {
            var writer = new DataWriter();
            writer.WriteString(data);

            return writer.DetachBuffer();
        }

        /// <summary>
        /// Convert a hex to an IBuffer
        /// </summary>
        /// <param name="hex">A hex value.</param>
        /// <returns>An IBuffer</returns>
        public static IBuffer ToIBufferFromHexString(string hex)
        {
            hex = hex.Replace("-", string.Empty);

            var numberChars = hex.Length;
            var bytes = new byte[numberChars / 2];

            for (var i = 0; i < numberChars; i += 2)
            {
                bytes[i / 2] = Convert.ToByte(hex.Substring(i, 2), 16);
            }

            var writer = new DataWriter();
            writer.WriteBytes(bytes);

            return writer.DetachBuffer();
        }

        /// <summary>
        /// Convert a 32 bit integer to an IBuffer
        /// </summary>
        /// <param name="data">A 32 bit integer.</param>
        /// <returns>An IBuffer.</returns>
        public static IBuffer ToIBuffer(Int32 data)
        {
            var writer = new DataWriter();
            writer.WriteInt32(data);

            return writer.DetachBuffer();
        }

        /// <summary>
        /// Convert an IBuffer to a UTF8 string.
        /// </summary>
        /// <param name="buffer">The buffer.</param>
        /// <returns>A UTF 8 string.</returns>
        public static string ToUTF8String(IBuffer buffer)
        {
            CryptographicBuffer.CopyToByteArray(buffer, out byte[] data);
            return Encoding.UTF8.GetString(data);
        }

        /// <summary>
        /// Convert an IBuffer to a UTF16 string.
        /// </summary>
        /// <param name="buffer">The buffer.</param>
        /// <returns>A UTF 16 string.</returns>
        public static string ToUTF16String(IBuffer buffer)
        {
            CryptographicBuffer.CopyToByteArray(buffer, out byte[] data);
            return Encoding.Unicode.GetString(data);
        }

        /// <summary>
        /// Convert an IBuffer to a 32 bit integer.
        /// </summary>
        /// <param name="buffer">The buffer.</param>
        /// <returns>A 32 bit integer.</returns>
        public static int ToInt32(IBuffer buffer)
        {
            CryptographicBuffer.CopyToByteArray(buffer, out byte[] data);
            data = GetBytes(data, 4);
            return BitConverter.ToInt32(data, 0);
        }

        /// <summary>
        /// Convert an IBuffer to a hex string.
        /// </summary>
        /// <param name="buffer">The buffer.</param>
        /// <returns>A hex string.</returns>
        public static string ToHexString(IBuffer buffer)
        {
            CryptographicBuffer.CopyToByteArray(buffer, out byte[] data);
            return BitConverter.ToString(data);
        }

        /// <summary>
        /// Takes an input array of bytes and returns an array with more zeros in the front
        /// </summary>
        /// <param name="input">A byte array to convert.</param>
        /// <param name="length">The length of the byte array.</param>
        /// <returns>A byte array with more zeros in front"/></returns>
        private static byte[] GetBytes(byte[] input, int length)
        {
            byte[] result = new byte[length];

            if (input.Length >= length)
            {
                result = input;
            }

            int offset = length - input.Length;
            for (var i = 0; i < input.Length; i++)
            {
                result[offset + i] = input[i];
            }

            if (BitConverter.IsLittleEndian)
            {
                Array.Reverse(result);
            }

            return result;
        }
    }
}
