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
    /// This interface represents a data source whose items can be loaded incrementally.
    /// </summary>
    /// <typeparam name="TSource">Type of collection element.</typeparam>
    /// <seealso cref="IncrementalLoadingCollection{TSource, IType}"/>
    [Obsolete("This interface is being deprecated. Please use the .NET Standard Library counterpart found in Microsoft.Toolkit.")]
    public interface IIncrementalSource<TSource> : Collections.IIncrementalSource<TSource>
    {
    }
}
