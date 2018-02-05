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
using System.Threading;
using System.Threading.Tasks;

namespace Microsoft.Toolkit.Uwp.SampleApp.SamplePages
{
    /// <summary>
    /// A sample implementation of the <see cref="IIncrementalSource{TSource}"/> interface.
    /// </summary>
    /// <seealso cref="IIncrementalSource{TSource}"/>
    public class PeopleSource : Collections.IIncrementalSource<Person>
    {
        private readonly List<Person> _people;

        /// <summary>
        /// Initializes a new instance of the <see cref="PeopleSource"/> class.
        /// </summary>
        public PeopleSource()
        {
            // Creates an example collection.
            _people = new List<Person>();

            for (int i = 1; i <= 200; i++)
            {
                var p = new Person { Name = "Person " + i };
                _people.Add(p);
            }
        }

        /// <summary>
        /// Retrieves items based on <paramref name="pageIndex"/> and <paramref name="pageSize"/> arguments.
        /// </summary>
        /// <param name="pageIndex">
        /// The zero-based index of the page that corresponds to the items to retrieve.
        /// </param>
        /// <param name="pageSize">
        /// The number of <see cref="Person"/> items to retrieve for the specified <paramref name="pageIndex"/>.
        /// </param>
        /// <param name="cancellationToken">
        /// Used to propagate notification that operation should be canceled.
        /// </param>
        /// <returns>
        /// Returns a collection of <see cref="Person"/>.
        /// </returns>
        public async Task<IEnumerable<Person>> GetPagedItemsAsync(int pageIndex, int pageSize, CancellationToken cancellationToken = default(CancellationToken))
        {
            // Gets items from the collection according to pageIndex and pageSize parameters.
            var result = (from p in _people
                          select p).Skip(pageIndex * pageSize).Take(pageSize);

            // Simulates a longer request...
            await Task.Delay(1000);

            return result;
        }
    }
}
