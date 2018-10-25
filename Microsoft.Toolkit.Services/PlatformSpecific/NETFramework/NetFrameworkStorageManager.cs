// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.IO;
using System.IO.IsolatedStorage;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Toolkit.Services.Core;

namespace Microsoft.Toolkit.Services.PlatformSpecific.NetFramework
{
    internal class NetFrameworkStorageManager : IStorageManager
    {
        private const string FileName = "credential_service_data.txt";
        private const char Separator = ':';

        public async Task<string> GetAsync(string key)
        {
            var isoStore = IsolatedStorageFile.GetStore(IsolatedStorageScope.User | IsolatedStorageScope.Assembly, null, null);

            using (IsolatedStorageFileStream isoStream = new IsolatedStorageFileStream(FileName, FileMode.OpenOrCreate, isoStore))
            {
                using (StreamReader reader = new StreamReader(isoStream))
                {
                    while (!reader.EndOfStream)
                    {
                        var line = (await reader.ReadLineAsync()).Split(Separator);
                        var currentKey = line.First();
                        if (currentKey == key)
                        {
                            return line.Last();
                        }
                    }
                }
            }

            return null;
        }

        public Task SetAsync(string key, string value)
        {
            var isoStore = IsolatedStorageFile.GetStore(IsolatedStorageScope.User | IsolatedStorageScope.Assembly, null, null);

            using (IsolatedStorageFileStream isoStream = new IsolatedStorageFileStream(FileName, FileMode.Append, isoStore))
            {
                using (StreamWriter writer = new StreamWriter(isoStream))
                {
                    return writer.WriteLineAsync(string.Concat(key, Separator, value));
                }
            }
        }
    }
}
