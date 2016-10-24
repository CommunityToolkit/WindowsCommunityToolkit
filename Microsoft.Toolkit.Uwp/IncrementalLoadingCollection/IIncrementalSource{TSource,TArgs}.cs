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
using System.Threading;
using System.Threading.Tasks;

namespace Microsoft.Toolkit.Uwp
{
    /// <summary>
    /// This interface represents a data source whose items can be loaded incrementally using the specified arguments as input (for example, they can be a search parameter).
    /// </summary>
    /// <typeparam name="TSource">Type of collection element.</typeparam>
    /// <typeparam name="TArgs">Type of input arguments.</typeparam>
    /// <seealso cref="IncrementalLoadingCollection{TSource, IType}"/>
    public interface IIncrementalSource<TSource, TArgs> : IIncrementalSource<TSource>
    {
        /// <summary>
        /// Gets or sets a value of <typeparamref name="TArgs"/> type that specifies additional loading parameters.
        /// </summary>
        TArgs Arguments { get; set; }
    }
}
