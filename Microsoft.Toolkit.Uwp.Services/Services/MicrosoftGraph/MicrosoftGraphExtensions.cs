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

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Microsoft.Graph;

namespace Microsoft.Toolkit.Uwp.Services.MicrosoftGraph
{
    /// <summary>
    /// GraphServiceClient Extensions
    /// </summary>
    [Obsolete("This class is being deprecated. Please use the .NET Standard Library counterpart found in Microsoft.Toolkit.Services.MicrosoftGraph")]
    public static class MicrosoftGraphExtensions
    {
        /// <summary>
        /// Add items from source to dest
        /// </summary>
        /// <param name="source">A collection of messages</param>
        /// <param name="dest">The destination collection</param>
        [Obsolete("This method is being deprecated. Please use the .NET Standard Library counterpart found in Microsoft.Toolkit.Services.MicrosoftGraph")]
        public static void AddTo(this IUserMessagesCollectionPage source, ObservableCollection<Graph.Message> dest)
        {
            Toolkit.Services.MicrosoftGraph.MicrosoftGraphExtensions.AddTo(source, dest);
        }

        /// <summary>
        /// Create a list of recipients
        /// </summary>
        /// <param name="source">A collection of email addresses</param>
        /// <param name="dest">A collection of Microsoft Graph recipients</param>
        [Obsolete("This method is being deprecated. Please use the .NET Standard Library counterpart found in Microsoft.Toolkit.Services.MicrosoftGraph")]
        public static void CopyTo(this string[] source, List<Recipient> dest)
        {
            Toolkit.Services.MicrosoftGraph.MicrosoftGraphExtensions.CopyTo(source, dest);
        }
    }
}
