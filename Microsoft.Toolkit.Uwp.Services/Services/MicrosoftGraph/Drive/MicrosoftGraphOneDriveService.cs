// ******************************************************************
//
// Copyright (c) Microsoft. All rights reserved.
// This code is licensed under the MIT License (MIT).
// THE CODE IS PROVIDED “AS IS”, WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED,
// INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.
// IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM,
// DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT,
// TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH
// THE CODE OR THE USE OR OTHER DEALINGS IN THE CODE.
//
// ******************************************************************

using System.Threading.Tasks;
using Microsoft.Graph;

namespace Microsoft.Toolkit.Uwp.Services.MicrosoftGraph
{
    /// <summary>
    ///  Class using Office 365 Microsoft Graph Messages API
    /// </summary>
    public class MicrosoftGraphOneDriveService
    {
        /// <summary>
        /// GraphServiceClient instance
        /// </summary>
        private GraphServiceClient _graphProvider;

        /// <summary>
        /// Initializes a new instance of the <see cref="MicrosoftGraphOneDriveService"/> class.
        /// <para>Permissions : Have full access to user files and files shared with user</para>
        /// </summary>
        /// <param name="graphClientProvider">Instance of GraphClientService class</param>
        public MicrosoftGraphOneDriveService(GraphServiceClient graphClientProvider)
        {
            _graphProvider = graphClientProvider;
        }

        /// <summary>
        /// Gets the OneDrive root folder
        /// </summary>
        /// <returns>When this method completes, it returns a OneDriveStorageFolder</returns>
        public async Task<OneDriveStorageFolder> RootFolderAsync()
        {
                var oneDriveRootItem = await _graphProvider.Me.Drive.Root.Request().GetAsync();
                return new OneDriveStorageFolder(_graphProvider, _graphProvider.Me.Drive.Root, oneDriveRootItem);
        }
    }
}
