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

using Microsoft.Toolkit.Parsers.Markdown.Enums;

namespace Microsoft.Toolkit.Parsers.Markdown.Helpers
{
    /// <summary>
    /// A helper class for the trip chars. This is an optimization. If we ask each class to go
    /// through the rage and look for itself we end up looping through the range n times, once
    /// for each inline. This class represent a character that an inline needs to have a
    /// possible match. We will go through the range once and look for everyone's trip chars,
    /// and if they can make a match from the trip char then we will commit to them.
    /// </summary>
    internal class InlineTripCharHelper
    {
        // Note! Everything in first char and suffix should be lower case!
        public char FirstChar { get; set; }

        public InlineParseMethod Method { get; set; }
    }
}