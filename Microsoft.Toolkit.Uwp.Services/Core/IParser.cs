﻿// ******************************************************************
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

namespace Microsoft.Toolkit.Uwp.Services
{
    /// <summary>
    /// Parser interface.
    /// </summary>
    /// <typeparam name="T">Type to parse into.</typeparam>
    public interface IParser<out T>
        where T : SchemaBase
    {
        /// <summary>
        /// Parse method which all classes must implement.
        /// </summary>
        /// <param name="data">Data to parse.</param>
        /// <returns>Strong typed parsed data.</returns>
        IEnumerable<T> Parse(string data);
    }
}
