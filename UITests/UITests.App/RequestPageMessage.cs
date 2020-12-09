// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Microsoft.Toolkit.Mvvm.Messaging.Messages;

namespace UITests.App
{
    public class RequestPageMessage : AsyncRequestMessage<bool>
    {
        public string PageName { get; private set; }

        public RequestPageMessage(string name)
        {
            PageName = name;
        }
    }
}
