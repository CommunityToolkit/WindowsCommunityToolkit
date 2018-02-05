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

namespace UnitTests.UI
{
    /// <summary>
    /// Sample class to test AdvancedCollectionViewSource functionality
    /// </summary>
    internal class Person : IComparable
    {
        public string Name { get; set; }

        public int Age { get; set; }

        public int CompareTo(object obj)
        {
            var other = obj as Person;

            if (other == null)
            {
                return -1;
            }

            return Age.CompareTo(other.Age);
        }
    }
}