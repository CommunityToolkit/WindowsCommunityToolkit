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

using System.Collections.Generic;
using System.Collections.ObjectModel;
using Microsoft.Graph;

namespace Microsoft.Toolkit.Uwp.Services.MicrosoftGraph
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
