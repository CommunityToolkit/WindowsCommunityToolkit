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
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Microsoft.Windows.Toolkit.Services.Core
{
    /// <summary>
    /// Generic interface that all deployed service providers implement.
    /// </summary>
    /// <typeparam name="T">Reference to underlying data service provider.</typeparam>
    /// <typeparam name="U">Strongly-typed schema for data returned in list query.</typeparam>
    /// <typeparam name="V">Configuration type specifying query parameters.</typeparam>
    /// <typeparam name="W">OAuth Token information.</typeparam>
    public interface IOAuthDataService<T, U, V, W> : IDataService<T, U, V>
    {
        /// <summary>
        /// Initialize the provider with relevant oAuthTokens.
        /// </summary>
        /// <param name="oAuthTokens">Instantiated oAuthTokens.</param>
        /// <returns>Success or failure.</returns>
        bool Initialize(W oAuthTokens);

        /// <summary>
        /// Log in to the underlying data service provider.
        /// </summary>
        /// <returns>Returns success or failure indicator.</returns>
        Task<bool> LoginAsync();

        /// <summary>
        /// Log in to the underlying data service provider with requested requiredPermissions.
        /// </summary>
        /// <param name="requiredPermissions">List of requiredPermissions required.</param>
        /// <returns>Returns success or failure indicator.</returns>
        Task<bool> LoginAsync(List<string> requiredPermissions);

        /// <summary>
        /// Logs out of the underlying data service provider.
        /// </summary>
        /// <returns>Returns a Task to enable awaiting on method call.</returns>
        Task LogoutAsync();
    }
}
