// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Microsoft.Toolkit.Services.Core;
using Windows.Security.Cryptography;
using Windows.Security.Cryptography.Core;
using Windows.Storage.Streams;

namespace Microsoft.Toolkit.Uwp.Services
{
    public class UwpSignatureManager : ISignatureManager
    {

        /// <summary>
        /// Generate request signature.
        /// </summary>
        /// <param name="baseSignature">Base string.</param>
        /// <param name="secret">Consumer secret key.</param>
        /// <returns>Signature.</returns>

        public string GetSignature(string baseSignature, string secret, bool append = false)
        {
            var key = append ? secret + "&" : secret;

            IBuffer keyMaterial = CryptographicBuffer.ConvertStringToBinary(key, BinaryStringEncoding.Utf8);
            MacAlgorithmProvider mac = MacAlgorithmProvider.OpenAlgorithm(MacAlgorithmNames.HmacSha1);
            CryptographicKey cryptoKey = mac.CreateKey(keyMaterial);
            IBuffer dataToBeSigned = CryptographicBuffer.ConvertStringToBinary(baseSignature, BinaryStringEncoding.Utf8);
            IBuffer hash = CryptographicEngine.Sign(cryptoKey, dataToBeSigned);
            return CryptographicBuffer.EncodeToBase64String(hash);


            
            //IBuffer keyMaterial = CryptographicBuffer.ConvertStringToBinary(secret + "&", BinaryStringEncoding.Utf8);
            //MacAlgorithmProvider mac = MacAlgorithmProvider.OpenAlgorithm("HMAC_SHA1");
            //CryptographicKey cryptoKey = mac.CreateKey(keyMaterial);
            //IBuffer dataToBeSigned = CryptographicBuffer.ConvertStringToBinary(baseSignature, BinaryStringEncoding.Utf8);
            //IBuffer hash = CryptographicEngine.Sign(cryptoKey, dataToBeSigned);
            //return CryptographicBuffer.EncodeToBase64String(hash);
        }
    }
}
