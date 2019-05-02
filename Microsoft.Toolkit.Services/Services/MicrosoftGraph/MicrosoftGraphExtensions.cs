// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Collections.Generic;
using System.Collections.ObjectModel;
using Microsoft.Graph;

namespace Microsoft.Toolkit.Services.MicrosoftGraph
{
    /// <summary>
    /// GraphServiceClient Extensions
    /// </summary>
    public static class MicrosoftGraphExtensions
    {
        /// <summary>
        /// Add items from source to dest
        /// </summary>
        /// <param name="source">A collection of messages</param>
        /// <param name="dest">The destination collection</param>
        public static void AddTo(this IUserMessagesCollectionPage source, ObservableCollection<Graph.Message> dest)
        {
            foreach (var item in source)
            {
                dest.Add(item);
            }
        }

        /// <summary>
        /// Create a list of recipients
        /// </summary>
        /// <param name="source">A collection of email addresses</param>
        /// <param name="dest">A collection of Microsoft Graph recipients</param>
        public static void CopyTo(this string[] source, List<Recipient> dest)
        {
            foreach (var recipient in source)
            {
                dest.Add(new Recipient
                {
                    EmailAddress = new EmailAddress
                    {
                        Address = recipient,
                    }
                });
            }
        }
    }
}
