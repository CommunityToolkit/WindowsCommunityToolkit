// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Uwp.UI;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.VisualStudio.TestTools.UnitTesting.AppContainer;

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
        public void Test_AdvancedCollectionView_Filter_Preserves_Order()
        {
            var l = new ObservableCollection<string>
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
            Assert.AreEqual("sit", a[0]);
            Assert.AreEqual("amet", a[1]);

            a.Insert(4, "how");

            Assert.AreEqual(3, a.Count);
            Assert.AreEqual("sit", a[0]);
            Assert.AreEqual("how", a[1]);
            Assert.AreEqual("amet", a[2]);
        }

        [TestCategory("AdvancedCollectionView")]
        [UITestMethod]
        public void Test_AdvancedCollectionView_Filter_Preserves_Order_When_Inserting_Duplicate()
        {
            var l = new ObservableCollection<string>
            {
                "lorem",
                "ipsum",
                "dolor",
                "sit",
                "amet"
            };

            var a = new AdvancedCollectionView(l)
            {
                Filter = (x) => x.ToString().Length >= 5
            };

            Assert.AreEqual(3, a.Count);
            Assert.AreEqual("lorem", a[0]);
            Assert.AreEqual("ipsum", a[1]);
            Assert.AreEqual("dolor", a[2]);

            a.Insert(3, "ipsum");

            Assert.AreEqual(4, a.Count);
            Assert.AreEqual("lorem", a[0]);
            Assert.AreEqual("ipsum", a[1]);
            Assert.AreEqual("dolor", a[2]);
            Assert.AreEqual("ipsum", a[3]);
        }

        [TestCategory("AdvancedCollectionView")]
        [UITestMethod]
        public void Test_AdvancedCollectionView_Updating_Filter_Preserves_Order_With_Duplicate()
        {
            var l = new ObservableCollection<string>
            {
                "lorem",
                "ipsum",
                "dolor",
                "sit",
                "ipsum",
                "amet"
            };

            var a = new AdvancedCollectionView(l)
            {
                Filter = (x) => x.ToString().Length < 5
            };

            Assert.AreEqual(2, a.Count);
            Assert.AreEqual("sit", a[0]);
            Assert.AreEqual("amet", a[1]);

            a.Filter = (x) => x.ToString().Length >= 5;

            Assert.AreEqual(4, a.Count);
            Assert.AreEqual("lorem", a[0]);
            Assert.AreEqual("ipsum", a[1]);
            Assert.AreEqual("dolor", a[2]);
            Assert.AreEqual("ipsum", a[3]);
        }

        [TestCategory("AdvancedCollectionView")]
        [UITestMethod]
        public void Test_AdvancedCollectionView_Filter_Preserves_Order_When_Inserting_After_Items_In_View()
        {
            var l = new ObservableCollection<string>
            {
                "lorem",
                "ipsum",
                "dolor",
                "sitter",
                "amet"
            };

            var a = new AdvancedCollectionView(l)
            {
                Filter = (x) => x.ToString().Length < 5
            };

            Assert.AreEqual(1, a.Count);
            Assert.AreEqual(a[0], "amet");

            a.Insert(0, "how");

            Assert.AreEqual(2, a.Count);
            Assert.AreEqual(a[0], "how");
            Assert.AreEqual(a[1], "amet");
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

        [TestCategory("AdvancedCollectionView")]
        [UITestMethod]
        public void Test_AdvancedCollectionView_Filter_With_Shaping()
        {
            var l = new List<string>
            {
                "lorem",
                "ipsum",
                "dolor",
                "sit",
                "amet"
            };

            var a = new AdvancedCollectionView(l, true)
            {
                Filter = (x) => x.ToString().Length < 5
            };

            Assert.AreEqual(2, a.Count);
        }

        [TestCategory("AdvancedCollectionView")]
        [UITestMethod]
        public void Test_AdvancedCollectionView_Filter_With_Shaping_And_Refresh_Call()
        {
            var l = new List<string>
            {
                "lorem",
                "ipsum",
                "dolor",
                "sit",
                "amet"
            };

            var a = new AdvancedCollectionView(l, true);

            a.Filter = (x) => x.ToString().Length < 5;

            Assert.AreEqual(2, a.Count);
        }

        [TestCategory("AdvancedCollectionView")]
        [UITestMethod]
        public void Test_AdvancedCollectionView_Updating_With_Shaping()
        {
            var l = new ObservableCollection<string>
            {
                "lorem",
                "ipsum",
                "dolor",
                "sit",
                "amet"
            };

            var a = new AdvancedCollectionView(l, true);

            Assert.AreEqual(5, a.Count);

            l.Add("new item");

            Assert.AreEqual(6, a.Count);
        }

        [TestCategory("AdvancedCollectionView")]
        [UITestMethod]
        public void Test_AdvancedCollectionView_Sorting_With_Shaping()
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

            var a = new AdvancedCollectionView(l, true)
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
        public void Test_AdvancedCollectionView_Using_Shaping()
        {
            var myPerson = new Person()
            {
                Name = "lorem",
                Age = 4
            };
            var l = new ObservableCollection<Person>
            {
                myPerson,
                new Person()
                {
                    Name = "imsum",
                    Age = 8
                },
            };

            var a = new AdvancedCollectionView(l, true);

            myPerson.Name = "myName";

            Assert.AreEqual("myName", ((Person)a.First()).Name);
            Assert.AreEqual(2, a.Count);
        }

        [TestCategory("AdvancedCollectionView")]
        [UITestMethod]
        public void Test_AdvancedCollectionView_Sorting_Using_Shaping()
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

            var a = new AdvancedCollectionView(l, true);

            a.SortDescriptions.Add(new SortDescription(nameof(Person.Age), SortDirection.Descending));

            Assert.AreEqual(42, ((Person)a.First()).Age);
        }

        [TestCategory("AdvancedCollectionView")]
        [UITestMethod]
        public void Test_AdvancedCollectionView_Combined_With_Shaping()
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

            var a = new AdvancedCollectionView(l, true)
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
        public void Test_AdvancedCollectionView_Combined_Using_Shaping_Changing_Properties()
        {
            var personLorem = new Person()
            {
                Name = "lorem",
                Age = 4
            };

            var l = new ObservableCollection<Person>
            {
                personLorem,
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

            var a = new AdvancedCollectionView(l, true)
            {
                SortDescriptions =
                {
                    new SortDescription(nameof(Person.Age), SortDirection.Descending)
                },
                Filter = (x) => ((Person)x).Name.Length > 5
            };

            a.ObserveFilterProperty(nameof(Person.Name));

            Assert.AreEqual(42, ((Person)a.First()).Age);
            Assert.AreEqual(1, a.Count);

            personLorem.Name = "lorems";
            personLorem.Age = 96;

            Assert.AreEqual(96, ((Person)a.First()).Age);
            Assert.AreEqual(2, a.Count);
        }

        [TestCategory("AdvancedCollectionView")]
        [UITestMethod]
        public void Test_AdvancedCollectionView_Combined_Using_Shaping()
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

            var a = new AdvancedCollectionView(l, true);

            a.Filter = (x) => ((Person)x).Name.Length > 5;
            a.RefreshFilter();

            a.SortDescriptions.Add(new SortDescription(nameof(Person.Age), SortDirection.Descending));

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
        public void Test_AdvancedCollectionView_Combined_Using_Shaping_Filter_Back_In()
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

            var a = new AdvancedCollectionView(l, true);

            a.Filter = (x) => ((Person)x).Name.Length > 5;
            a.RefreshFilter();

            a.Filter = (x) => ((Person)x).Name.Length > 4;
            a.RefreshFilter();

            a.SortDescriptions.Add(new SortDescription(nameof(Person.Age), SortDirection.Descending));

            Assert.AreEqual(42, ((Person)a.First()).Age);
            Assert.AreEqual(4, a.Count);

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
            Assert.AreEqual(5, a.Count);
        }

        [TestCategory("AdvancedCollectionView")]
        [UITestMethod]
        public void Test_AdvancedCollectionView_Sorting_OnSelf_With_Shaping()
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

            var a = new AdvancedCollectionView(l, true)
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
        public void Test_AdvancedCollectionView_Sorting_OnSelf_CustomComparable_With_Shaping()
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

            var a = new AdvancedCollectionView(l, true)
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
        public void Test_AdvancedCollectionView_Sorting_CustomComparable_With_Shaping()
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

            var a = new AdvancedCollectionView(l, true)
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

        [TestCategory("AdvancedCollectionView")]
        [UITestMethod]
        public void Test_AdvancedCollectionView_Shaping_OutOfRangeCheck()
        {
            var _random = new Random();
            var Models = new ObservableCollection<Model>(Enumerable.Range(0, 20).Select(i => new Model
            {
                Id = i + 1,
                Title = $"Title: {i + 1}",
                Year = _random.Next(2015, 2020)
            }));

            IAdvancedCollectionView View1 = new AdvancedCollectionView(Models, true);
            View1.ObserveFilterProperty(nameof(Model.Year));
            View1.Filter = model => ((Model)model).Year <= 2020;

            IAdvancedCollectionView View2 = new AdvancedCollectionView(Models, true);
            View2.ObserveFilterProperty(nameof(Model.Year));
            View2.Filter = model => ((Model)model).Year >= 2021;
            
            // In an attempt to reproduce the bug, initially we need to remove the first item ...
            Model model1 = View1.FirstOrDefault(x => ((Model)x).Id == 1) as Model;
            if(model1 != null)
            {
                View1.Remove(model1);
                model1.Year = _random.Next(2021, 2030);
                View2.Add(model1);
            }

            Assert.IsTrue(!View1.Contains(model1));
            Assert.IsTrue(View2.Contains(model1));

            // ... and continue on by removing the fifth item, which had led to an issue with Drag & Drop
            //      - see https://github.com/CommunityToolkit/WindowsCommunityToolkit/issues/4339
            Model model5 = View1.FirstOrDefault(x => ((Model)x).Id == 5) as Model;
            if (model5 != null)
            {
                View1.Remove(model5);
                model5.Year = _random.Next(2021, 2030);
                View2.Add(model5);
            }

            Assert.IsTrue(!View1.Contains(model5));
            Assert.IsTrue(View2.Contains(model5));
        }

        [TestCategory("AdvancedCollectionView")]
        [UITestMethod]
        public void Test_AdvancedCollectionView_Shaping_RemoveLastItem()
        {
            var _random = new Random();
            var Models = new ObservableCollection<Model>(Enumerable.Range(0, 20).Select(i => new Model
            {
                Id = i + 1,
                Title = $"Title: {i + 1}",
                Year = _random.Next(2015, 2020)
            }));

            IAdvancedCollectionView View1 = new AdvancedCollectionView(Models, true);
            View1.ObserveFilterProperty(nameof(Model.Year));
            View1.Filter = model => ((Model)model).Year <= 2020;

            IAdvancedCollectionView View2 = new AdvancedCollectionView(Models, true);
            View2.ObserveFilterProperty(nameof(Model.Year));
            View2.Filter = model => ((Model)model).Year >= 2021;

            int lastIndex = View1.Count - 1;
            if(lastIndex >= 0)
            {
                Model modelLast = (Model)View1[lastIndex];

                if (modelLast != null)
                {
                    View1.Remove(modelLast);
                    modelLast.Year = _random.Next(2021, 2030);
                    View2.Add(modelLast);
                }

                Assert.IsTrue(!View1.Contains(modelLast));
                Assert.IsTrue(View2.Contains(modelLast));
            }
        }

        /// <summary>
        /// Test class, kept the same as reported in the GitHub issue
        ///     - https://github.com/CommunityToolkit/WindowsCommunityToolkit/issues/4339
        /// </summary>
        public class Model : ObservableObject
        {
            private int _id;

            public int Id
            {
                get { return _id; }
                set { SetProperty(ref _id, value); }
            }

            private string _title;

            public string Title
            {
                get { return _title; }
                set { SetProperty(ref _title, value); }
            }

            private int _year;

            public int Year
            {
                get { return _year; }
                set { SetProperty(ref _year, value); }
            }
        }
    }
}