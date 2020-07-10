// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.ComponentModel;
using System.Threading.Tasks;
using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTests.Mvvm
{
    [TestClass]
    public class Test_ObservableObject
    {
        [TestCategory("Mvvm")]
        [TestMethod]
        public void Test_ObservableObject_Events()
        {
            var model = new SampleModel<int>();

            (PropertyChangingEventArgs, int) changing = default;
            (PropertyChangedEventArgs, int) changed = default;

            model.PropertyChanging += (s, e) =>
            {
                Assert.IsNull(changing.Item1);
                Assert.IsNull(changed.Item1);
                Assert.AreSame(model, s);
                Assert.IsNotNull(s);
                Assert.IsNotNull(e);

                changing = (e, model.Data);
            };

            model.PropertyChanged += (s, e) =>
            {
                Assert.IsNotNull(changing.Item1);
                Assert.IsNull(changed.Item1);
                Assert.AreSame(model, s);
                Assert.IsNotNull(s);
                Assert.IsNotNull(e);

                changed = (e, model.Data);
            };

            model.Data = 42;

            Assert.AreEqual(changing.Item1?.PropertyName, nameof(SampleModel<int>.Data));
            Assert.AreEqual(changing.Item2, 0);
            Assert.AreEqual(changed.Item1?.PropertyName, nameof(SampleModel<int>.Data));
            Assert.AreEqual(changed.Item2, 42);
        }

        public class SampleModel<T> : ObservableObject
        {
            private T data;

            public T Data
            {
                get => data;
                set => Set(ref data, value);
            }
        }

        [TestCategory("Mvvm")]
        [TestMethod]
        public void Test_ObservableObject_ProxyCrudWithProperty()
        {
            var model = new WrappingModelWithProperty { Person = new Person { Name = "Alice" } };

            (PropertyChangingEventArgs, string) changing = default;
            (PropertyChangedEventArgs, string) changed = default;

            model.PropertyChanging += (s, e) =>
            {
                Assert.AreSame(model, s);

                changing = (e, model.Name);
            };

            model.PropertyChanged += (s, e) =>
            {
                Assert.AreSame(model, s);

                changed = (e, model.Name);
            };

            model.Name = "Bob";

            Assert.AreEqual(changing.Item1?.PropertyName, nameof(WrappingModelWithProperty.Name));
            Assert.AreEqual(changing.Item2, "Alice");
            Assert.AreEqual(changed.Item1?.PropertyName, nameof(WrappingModelWithProperty.Name));
            Assert.AreEqual(changed.Item2, "Bob");
            Assert.AreEqual(model.Person.Name, "Bob");
        }

        public class Person
        {
            public string Name { get; set; }
        }

        public class WrappingModelWithProperty : ObservableObject
        {
            public Person Person { get; set; }

            public string Name
            {
                get => Person.Name;
                set => Set(() => Person.Name, value);
            }
        }

        [TestCategory("Mvvm")]
        [TestMethod]
        public void Test_ObservableObject_ProxyCrudWithField()
        {
            var model = new WrappingModelWithField { Person = new Person { Name = "Alice" } };

            (PropertyChangingEventArgs, string) changing = default;
            (PropertyChangedEventArgs, string) changed = default;

            model.PropertyChanging += (s, e) =>
            {
                Assert.AreSame(model, s);

                changing = (e, model.Name);
            };

            model.PropertyChanged += (s, e) =>
            {
                Assert.AreSame(model, s);

                changed = (e, model.Name);
            };

            model.Name = "Bob";

            Assert.AreEqual(changing.Item1?.PropertyName, nameof(WrappingModelWithField.Name));
            Assert.AreEqual(changing.Item2, "Alice");
            Assert.AreEqual(changed.Item1?.PropertyName, nameof(WrappingModelWithField.Name));
            Assert.AreEqual(changed.Item2, "Bob");
            Assert.AreEqual(model.Person.Name, "Bob");
        }

        public class WrappingModelWithField : ObservableObject
        {
#pragma warning disable SA1401 // Fields should be private
            public Person Person;
#pragma warning restore SA1401

            public string Name
            {
                get => Person.Name;
                set => Set(() => Person.Name, value);
            }
        }

        [TestCategory("Mvvm")]
        [TestMethod]
        public async Task Test_ObservableObject_NotifyTask()
        {
            var model = new SampleModelWithTask<int>();
            var tcs = new TaskCompletionSource<int>();
            var task = tcs.Task;

            (PropertyChangingEventArgs, Task<int>) changing = default;
            (PropertyChangedEventArgs, Task<int>) changed = default;

            model.PropertyChanging += (s, e) =>
            {
                Assert.AreSame(model, s);

                changing = (e, model.Data);
            };

            model.PropertyChanged += (s, e) =>
            {
                Assert.AreSame(model, s);

                changed = (e, model.Data);
            };

            model.Data = task;

            Assert.IsFalse(task.IsCompleted);
            Assert.AreEqual(changing.Item1?.PropertyName, nameof(SampleModelWithTask<int>.Data));
            Assert.IsNull(changing.Item2);
            Assert.AreEqual(changed.Item1?.PropertyName, nameof(SampleModelWithTask<int>.Data));
            Assert.AreSame(changed.Item2, task);

            changed = default;

            tcs.SetResult(42);

            await Task.Delay(100); // Time for the notification to dispatch

            Assert.IsTrue(task.IsCompleted);
            Assert.AreEqual(changed.Item1?.PropertyName, nameof(SampleModel<int>.Data));
            Assert.AreSame(changed.Item2, task);
        }

        public class SampleModelWithTask<T> : ObservableObject
        {
            private Task<T> data;

            public Task<T> Data
            {
                get => data;
                set => SetAndNotifyOnCompletion(ref data, () => data, value);
            }
        }
    }
}
