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
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.Toolkit.Uwp.SampleApp.Controls
{
    /// <summary>
    /// Xaml Parsing Error Message and Location.
    /// </summary>
    public sealed class XamlExceptionRange : Exception
    {
        public uint StartLine { get; set; }

        public uint StartColumn { get; set; }

        public uint EndLine { get; set; }

        public uint EndColumn { get; set; }

        public XamlExceptionRange(string message, Exception error, uint startline, uint startcol, uint endline, uint endcol)
            : base(message, error)
        {
            StartLine = startline;
            StartColumn = startcol;
            EndLine = endline;
            EndColumn = endcol;
        }
    }
}
