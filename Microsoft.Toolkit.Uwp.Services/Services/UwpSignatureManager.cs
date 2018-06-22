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

        public string GetSignature(string baseSignature, string secret)
        {
            IBuffer keyMaterial = CryptographicBuffer.ConvertStringToBinary(secret + "&", BinaryStringEncoding.Utf8);
            MacAlgorithmProvider hmacSha1Provider = MacAlgorithmProvider.OpenAlgorithm("HMAC_SHA1");
            CryptographicKey macKey = hmacSha1Provider.CreateKey(keyMaterial);
            IBuffer dataToBeSigned = CryptographicBuffer.ConvertStringToBinary(baseSignature, BinaryStringEncoding.Utf8);
            IBuffer signatureBuffer = CryptographicEngine.Sign(macKey, dataToBeSigned);
            string signature = CryptographicBuffer.EncodeToBase64String(signatureBuffer);

            return signature;
        }
    }
}
