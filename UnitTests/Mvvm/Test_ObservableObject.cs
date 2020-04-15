// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.ComponentModel;
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
                Assert.AreSame(model, s);

                changing = (e, model.Data);
            };

            model.PropertyChanged += (s, e) =>
            {
                Assert.AreSame(model, s);

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
    }
}
