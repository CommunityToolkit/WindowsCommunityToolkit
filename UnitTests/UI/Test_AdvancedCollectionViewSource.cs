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

using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Microsoft.Toolkit.Uwp.UI;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework.AppContainer;
using Assert = Microsoft.VisualStudio.TestPlatform.UnitTestFramework.Assert;

namespace UnitTests.UI
{
    [TestClass]
    public class Test_AdvancedCollectionViewSource
    {
        [TestCategory("AdvancedCollectionViewSource")]
        [UITestMethod]
        public void Test_AdvancedCollectionViewSource_Filter()
        {
            var l = new List<string>
            {
                "lorem",
                "ipsum",
                "dolor",
                "sit",
                "amet"
            };

            var a = new AdvancedCollectionViewSource(l)
            {
                Filter = (x) => x.ToString().Length < 5
            };

            Assert.AreEqual(a.Count, 2);
        }

        [TestCategory("AdvancedCollectionViewSource")]
        [UITestMethod]
        public void Test_AdvancedCollectionViewSource_Updating()
        {
            var l = new ObservableCollection<string>
            {
                "lorem",
                "ipsum",
                "dolor",
                "sit",
                "amet"
            };

            var a = new AdvancedCollectionViewSource(l);

            Assert.AreEqual(a.Count, 5);

            l.Add("new item");

            Assert.AreEqual(a.Count, 6);
        }

        [TestCategory("AdvancedCollectionViewSource")]
        [UITestMethod]
        public void Test_AdvancedCollectionViewSource_Sorting()
        {
            var l = new ObservableCollection<Person>
            {
                new Person()
                {
                    Name = "lorem",
                    Age = 4
                },
                new Person()
                {
                    Name = "imsum",
                    Age = 8
                },
                new Person()
                {
                    Name = "dolor",
                    Age = 15
                },
                new Person()
                {
                    Name = "sit",
                    Age = 16
                },
                new Person()
                {
                    Name = "amet",
                    Age = 23
                },
                new Person()
                {
                    Name = "consectetur",
                    Age = 42
                },
            };

            var a = new AdvancedCollectionViewSource(l)
            {
                SortDescriptions =
                {
                    new SortDescription(nameof(Person.Age), SortDirection.Descending)
                }
            };

            Assert.AreEqual(((Person)a.First()).Age, 42);
        }

        [TestCategory("AdvancedCollectionViewSource")]
        [UITestMethod]
        public void Test_AdvancedCollectionViewSource_Combined()
        {
            var l = new ObservableCollection<Person>
            {
                new Person()
                {
                    Name = "lorem",
                    Age = 4
                },
                new Person()
                {
                    Name = "imsum",
                    Age = 8
                },
                new Person()
                {
                    Name = "dolor",
                    Age = 15
                },
                new Person()
                {
                    Name = "sit",
                    Age = 16
                },
                new Person()
                {
                    Name = "amet",
                    Age = 23
                },
                new Person()
                {
                    Name = "consectetur",
                    Age = 42
                },
            };

            var a = new AdvancedCollectionViewSource(l)
            {
                SortDescriptions =
                {
                    new SortDescription(nameof(Person.Age), SortDirection.Descending)
                },
                Filter = (x) => ((Person)x).Name.Length > 5
            };

            Assert.AreEqual(((Person)a.First()).Age, 42);
            Assert.AreEqual(a.Count, 1);

            l.Add(new Person
            {
                Name = "foo",
                Age = 50
            });

            l.Add(new Person
            {
                Name = "Person McPersonface",
                Age = 10
            });

            Assert.AreEqual(((Person)a.First()).Age, 42);
            Assert.AreEqual(a.Count, 2);
        }
    }
}