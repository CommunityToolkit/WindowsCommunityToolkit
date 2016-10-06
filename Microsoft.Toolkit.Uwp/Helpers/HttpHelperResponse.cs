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
using System.Threading.Tasks;
using Windows.Web.Http;

namespace Microsoft.Toolkit.Uwp
{
    /// <summary>
    /// HttpHelperResponse instance to hold data from Http Response.
    /// </summary>
    public class HttpHelperResponse : IDisposable
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="HttpHelperResponse"/> class.
        /// Default constructor.
        /// </summary>
        public HttpHelperResponse()
        {
            StatusCode = HttpStatusCode.Ok;
            Result = null;
        }

        /// <summary>
        /// Gets or sets holds request StatusCode.
        /// </summary>
        public HttpStatusCode StatusCode { get; set; }

        /// <summary>
        /// Gets or sets holds request Result.
        /// </summary>
        public IHttpContent Result { get; set; }

        /// <summary>
        /// Reads the Content as string and returns it to the caller.
        /// </summary>
        /// <returns>string content</returns>
        public Task<string> GetTextResultAsync()
        {
            if (this.Result == null)
            {
                return null;
            }

            return Result.ReadAsStringAsync().AsTask();
        }

        /// <summary>
        /// Gets a value indicating whether holds request Success boolean.
        /// </summary>
        public bool Success => StatusCode == HttpStatusCode.Ok;

        /// <summary>
        /// Dispose underlying content
        /// </summary>
        public void Dispose()
        {
            try
            {
                Result?.Dispose();
            }
            catch (ObjectDisposedException)
            {
                // known issue
                // http://stackoverflow.com/questions/39109060/httpmultipartformdatacontent-dispose-throws-objectdisposedexception
            }
        }
    }
}
