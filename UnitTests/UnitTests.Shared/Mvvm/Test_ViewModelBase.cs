// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.Messaging;
using Microsoft.Toolkit.Mvvm.Messaging.Messages;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTests.Mvvm
{
    [TestClass]
    public class Test_ViewModelBase
    {
        [TestCategory("Mvvm")]
        [TestMethod]
        public void Test_ViewModelBase_Activation()
        {
            var viewmodel = new SomeViewModel<int>();

            Assert.IsFalse(viewmodel.IsActivatedCheck);

            viewmodel.IsActive = true;

            Assert.IsTrue(viewmodel.IsActivatedCheck);
            Assert.IsTrue(viewmodel.CurrentMessenger.IsRegistered<SampleMessage>(viewmodel));

            viewmodel.IsActive = false;

            Assert.IsFalse(viewmodel.IsActivatedCheck);
            Assert.IsFalse(viewmodel.CurrentMessenger.IsRegistered<SampleMessage>(viewmodel));
        }

        [TestCategory("Mvvm")]
        [TestMethod]
        public void Test_ViewModelBase_Defaults()
        {
            var viewmodel = new SomeViewModel<int>();

            Assert.AreSame(viewmodel.CurrentMessenger, Messenger.Default);
        }

        [TestCategory("Mvvm")]
        [TestMethod]
        public void Test_ViewModelBase_Injection()
        {
            var messenger = new Messenger();
            var viewmodel = new SomeViewModel<int>(messenger);

            Assert.AreSame(viewmodel.CurrentMessenger, messenger);
        }

        [TestCategory("Mvvm")]
        [TestMethod]
        public void Test_ViewModelBase_Broadcast()
        {
            var messenger = new Messenger();
            var viewmodel = new SomeViewModel<int>(messenger);

            PropertyChangedMessage<int> message = null;

            messenger.Register<PropertyChangedMessage<int>>(messenger, m => message = m);

            viewmodel.Data = 42;

            Assert.IsNotNull(message);
            Assert.AreSame(message.Sender, viewmodel);
            Assert.AreEqual(message.OldValue, 0);
            Assert.AreEqual(message.NewValue, 42);
            Assert.AreEqual(message.PropertyName, nameof(SomeViewModel<int>.Data));
        }

        public class SomeViewModel<T> : ViewModelBase
        {
            public SomeViewModel()
            {
            }

            public SomeViewModel(IMessenger messenger)
                : base(messenger)
            {
            }

            public IMessenger CurrentMessenger => Messenger;

            private T data;

            public T Data
            {
                get => data;
                set => Set(ref data, value, true);
            }

            public bool IsActivatedCheck { get; private set; }

            protected override void OnActivated()
            {
                IsActivatedCheck = true;

                Messenger.Register<SampleMessage>(this, m => { });
            }

            protected override void OnDeactivated()
            {
                base.OnDeactivated();

                IsActivatedCheck = false;
            }
        }

        public class SampleMessage
        {
        }
    }
}
