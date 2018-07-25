// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Microsoft.Toolkit.Services.Core;
#if WINRT
using Windows.Security.Cryptography;
using Windows.Security.Cryptography.Core;
using Windows.Storage.Streams;
#endif

namespace Microsoft.Toolkit.Uwp.Services
{
    /// <summary>
    /// Uwp specific signature generator using cryptographic library
    /// </summary>
    public class UwpSignatureManager : ISignatureManager
    {
        /// <summary>
        /// Generate request signature.
        /// </summary>
        /// <param name="baseString">String to sign</param>
        /// <param name="secret">Secret to use to sign</param>
        /// <param name="append">If true append &amp; to the base string</param>
        /// <returns>Signature.</returns>
        public string GetSignature(string baseString, string secret, bool append = false)
        {
            #if WINRT
            var key = append ? secret + "&" : secret;

            IBuffer keyMaterial = CryptographicBuffer.ConvertStringToBinary(key, BinaryStringEncoding.Utf8);
            MacAlgorithmProvider mac = MacAlgorithmProvider.OpenAlgorithm(MacAlgorithmNames.HmacSha1);
            CryptographicKey cryptoKey = mac.CreateKey(keyMaterial);
            IBuffer dataToBeSigned = CryptographicBuffer.ConvertStringToBinary(baseString, BinaryStringEncoding.Utf8);
            IBuffer hash = CryptographicEngine.Sign(cryptoKey, dataToBeSigned);
            return CryptographicBuffer.EncodeToBase64String(hash);
            #endif

            return null;
        }
    }
}
