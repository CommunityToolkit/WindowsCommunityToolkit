// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTests.Mvvm
{
    [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1601", Justification = "Type only used for testing")]
    [TestClass]
    public partial class Test_INotifyPropertyChangedAttribute
    {
        [TestCategory("Mvvm")]
        [TestMethod]
        public void Test_INotifyPropertyChanged_Events()
        {
            var model = new SampleModel();

            (PropertyChangedEventArgs, int) changed = default;

            model.PropertyChanged += (s, e) =>
            {
                Assert.IsNull(changed.Item1);
                Assert.AreSame(model, s);
                Assert.IsNotNull(s);
                Assert.IsNotNull(e);

                changed = (e, model.Data);
            };

            model.Data = 42;

            Assert.AreEqual(changed.Item1?.PropertyName, nameof(SampleModel.Data));
            Assert.AreEqual(changed.Item2, 42);
        }

        [INotifyPropertyChanged]
        public partial class SampleModel
        {
            private int data;

            public int Data
            {
                get => data;
                set => SetProperty(ref data, value);
            }
        }

        [TestCategory("Mvvm")]
        [TestMethod]
        public void Test_INotifyPropertyChanged_WithoutHelpers()
        {
            Assert.IsTrue(typeof(INotifyPropertyChanged).IsAssignableFrom(typeof(SampleModelWithoutHelpers)));
            Assert.IsFalse(typeof(INotifyPropertyChanging).IsAssignableFrom(typeof(SampleModelWithoutHelpers)));

            // This just needs to check that it compiles
            _ = nameof(SampleModelWithoutHelpers.PropertyChanged);

            var methods = typeof(SampleModelWithoutHelpers).GetMethods(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.DeclaredOnly);

            Assert.AreEqual(methods.Length, 2);
            Assert.AreEqual(methods[0].Name, "OnPropertyChanged");
            Assert.AreEqual(methods[1].Name, "OnPropertyChanged");

            var types = typeof(SampleModelWithoutHelpers).GetNestedTypes(BindingFlags.NonPublic);

            Assert.AreEqual(types.Length, 0);
        }

        [INotifyPropertyChanged(IncludeAdditionalHelperMethods = false)]
        public partial class SampleModelWithoutHelpers
        {
        }

        [TestCategory("Mvvm")]
        [TestMethod]
        public void Test_INotifyPropertyChanged_WithGeneratedProperties()
        {
            Assert.IsTrue(typeof(INotifyPropertyChanged).IsAssignableFrom(typeof(SampleModelWithINPCAndObservableProperties)));
            Assert.IsFalse(typeof(INotifyPropertyChanging).IsAssignableFrom(typeof(SampleModelWithINPCAndObservableProperties)));

            SampleModelWithINPCAndObservableProperties model = new();
            List<PropertyChangedEventArgs> eventArgs = new();

            model.PropertyChanged += (s, e) => eventArgs.Add(e);

            model.X = 42;
            model.Y = 66;

            Assert.AreEqual(eventArgs.Count, 2);
            Assert.AreEqual(eventArgs[0].PropertyName, nameof(SampleModelWithINPCAndObservableProperties.X));
            Assert.AreEqual(eventArgs[1].PropertyName, nameof(SampleModelWithINPCAndObservableProperties.Y));
        }

        // See https://github.com/CommunityToolkit/WindowsCommunityToolkit/issues/4167
        [INotifyPropertyChanged]
        public partial class SampleModelWithINPCAndObservableProperties
        {
            [ObservableProperty]
            private int x;

            [ObservableProperty]
            private int y;
        }

        [TestCategory("Mvvm")]
        [TestMethod]
        public void Test_INotifyPropertyChanged_WithGeneratedProperties_ExternalNetStandard20Assembly()
        {
            Assert.IsTrue(typeof(INotifyPropertyChanged).IsAssignableFrom(typeof(NetStandard.SampleModelWithINPCAndObservableProperties)));
            Assert.IsFalse(typeof(INotifyPropertyChanging).IsAssignableFrom(typeof(NetStandard.SampleModelWithINPCAndObservableProperties)));

            NetStandard.SampleModelWithINPCAndObservableProperties model = new();
            List<PropertyChangedEventArgs> eventArgs = new();

            model.PropertyChanged += (s, e) => eventArgs.Add(e);

            model.X = 42;
            model.Y = 66;

            Assert.AreEqual(eventArgs.Count, 2);
            Assert.AreEqual(eventArgs[0].PropertyName, nameof(NetStandard.SampleModelWithINPCAndObservableProperties.X));
            Assert.AreEqual(eventArgs[1].PropertyName, nameof(NetStandard.SampleModelWithINPCAndObservableProperties.Y));
        }
    }
}
