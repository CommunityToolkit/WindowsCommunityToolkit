// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Toolkit.Services.Core;

namespace Microsoft.Toolkit.Services.PlatformSpecific.NetFramework
{
    internal class NetFrameworkSignatureManager : ISignatureManager
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
            var key = append ? secret + "&" : secret;

            var baseStringByte = Encoding.UTF8.GetBytes(baseString);
            var keyByte = Encoding.UTF8.GetBytes(key);

            using (HMACSHA1 hmac = new HMACSHA1(keyByte))
            {
                hmac.Initialize();
                var hash = hmac.ComputeHash(baseStringByte);
                string base64 = Convert.ToBase64String(hash);
                return base64;
            }
        }
    }
}
