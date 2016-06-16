// *********************************************************
//  Copyright (c) Microsoft. All rights reserved.
//  This code is licensed under the MIT License (MIT).
//  THE CODE IS PROVIDED “AS IS”, WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, 
//  INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, 
//  FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. 
//  IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, 
//  DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, 
//  TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH 
//  THE CODE OR THE USE OR OTHER DEALINGS IN THE CODE.
// *********************************************************
using System.Net;

namespace Microsoft.Windows.Toolkit.Services.Core
{
    internal class HttpRequestResult
    {
        public HttpRequestResult()
        {
            this.StatusCode = HttpStatusCode.OK;
            this.Result = string.Empty;
        }

        public HttpStatusCode StatusCode { get; set; }

        public string Result { get; set; }

        public bool Success { get { return this.StatusCode == HttpStatusCode.OK && !string.IsNullOrEmpty(this.Result); } }
    }
}
