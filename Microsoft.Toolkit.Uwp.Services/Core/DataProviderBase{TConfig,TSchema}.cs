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
using System.Threading.Tasks;

namespace Microsoft.Toolkit.Uwp.Services
{
    /// <summary>
    /// Base class for data providers in this library.
    /// </summary>
    /// <typeparam name="TConfig">Strong typed query configuration object.</typeparam>
    /// <typeparam name="TSchema">Strong typed object to parse the response items into.</typeparam>
    [Obsolete("This class is being deprecated. Please use the .NET Standard Library counterpart found in Microsoft.Toolkit.Services.")]
    public abstract class DataProviderBase<TConfig, TSchema> : Toolkit.Services.DataProviderBase<TConfig, TSchema>
        where TSchema : SchemaBase
    {
    }
}
