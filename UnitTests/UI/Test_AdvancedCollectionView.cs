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
using System.Collections;
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
    public class Test_AdvancedCollectionView
    {
        [TestCategory("AdvancedCollectionView")]
        [UITestMethod]
        public void Test_AdvancedCollectionView_ParameterlessCtor()
        {
            var a = new AdvancedCollectionView()
            {
                Filter = (x) => x.ToString().Length < 5
            };

            a.Source = new List<string>
            {
                "lorem",
                "ipsum",
                "dolor",
                "sit",
                "amet"
            };

            Assert.AreEqual(2, a.Count);
        }

        [TestCategory("AdvancedCollectionView")]
        [UITestMethod]
        public void Test_AdvancedCollectionView_Filter()
        {
            var l = new List<string>
            {
                "lorem",
                "ipsum",
                "dolor",
                "sit",
                "amet"
            };

            var a = new AdvancedCollectionView(l)
            {
                Filter = (x) => x.ToString().Length < 5
            };

            Assert.AreEqual(2, a.Count);
        }

        [TestCategory("AdvancedCollectionView")]
        [UITestMethod]
        public void Test_AdvancedCollectionView_Updating()
        {
            var l = new ObservableCollection<string>
            {
                "lorem",
                "ipsum",
                "dolor",
                "sit",
                "amet"
            };

            var a = new AdvancedCollectionView(l);

            Assert.AreEqual(5, a.Count);

            l.Add("new item");

            Assert.AreEqual(6, a.Count);
        }

        [TestCategory("AdvancedCollectionView")]
        [UITestMethod]
        public void Test_AdvancedCollectionView_Sorting()
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

            var a = new AdvancedCollectionView(l)
            {
                SortDescriptions =
                {
                    new SortDescription(nameof(Person.Age), SortDirection.Descending)
                }
            };

            Assert.AreEqual(42, ((Person)a.First()).Age);
        }

        [TestCategory("AdvancedCollectionView")]
        [UITestMethod]
        public void Test_AdvancedCollectionView_Combined()
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

            var a = new AdvancedCollectionView(l)
            {
                SortDescriptions =
                {
                    new SortDescription(nameof(Person.Age), SortDirection.Descending)
                },
                Filter = (x) => ((Person)x).Name.Length > 5
            };

            Assert.AreEqual(42, ((Person)a.First()).Age);
            Assert.AreEqual(1, a.Count);

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

            Assert.AreEqual(42, ((Person)a.First()).Age);
            Assert.AreEqual(2, a.Count);
        }

        [TestCategory("AdvancedCollectionView")]
        [UITestMethod]
        public void Test_AdvancedCollectionView_Sorting_OnSelf()
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

            var a = new AdvancedCollectionView(l)
            {
                SortDescriptions =
                {
                    new SortDescription(SortDirection.Descending)
                }
            };

            Assert.AreEqual(42, ((Person)a.First()).Age);
        }

        [TestCategory("AdvancedCollectionView")]
        [UITestMethod]
        public void Test_AdvancedCollectionView_Sorting_OnSelf_CustomComparable()
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

            var a = new AdvancedCollectionView(l)
            {
                SortDescriptions =
                {
                    new SortDescription(SortDirection.Ascending, new DelegateComparable((x, y) => -((Person)x).Age.CompareTo(((Person)y).Age)))
                }
            };

            Assert.AreEqual(42, ((Person)a.First()).Age);
        }

        [TestCategory("AdvancedCollectionView")]
        [UITestMethod]
        public void Test_AdvancedCollectionView_Sorting_CustomComparable()
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

            var a = new AdvancedCollectionView(l)
            {
                SortDescriptions =
                {
                    new SortDescription(nameof(Person.Age), SortDirection.Ascending, new DelegateComparable((x, y) => -((int)x).CompareTo((int)y)))
                }
            };

            Assert.AreEqual(42, ((Person)a.First()).Age);
        }

        private class DelegateComparable : IComparer
        {
            private Func<object, object, int> _func;

            public DelegateComparable(Func<object, object, int> func)
            {
                _func = func;
            }

            public int Compare(object x, object y) => _func(x, y);
        }
    }
}