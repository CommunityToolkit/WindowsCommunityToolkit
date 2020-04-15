// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Microsoft.Toolkit.Mvvm.Messaging;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTests.Mvvm
{
    [TestClass]
    public class Test_Messenger
    {
        [TestCategory("Mvvm")]
        [TestMethod]
        public void Test_Ioc_IsRegistered_Register_Send_UnregisterOfTMessage_WithNoToken()
        {
            object a = new object();

            Assert.IsFalse(Messenger.IsRegistered<MessageA>(a));

            string result = null;
            Messenger.Register<MessageA>(a, m => result = m.Text);

            Assert.IsTrue(Messenger.IsRegistered<MessageA>(a));

            Messenger.Send(new MessageA { Text = nameof(MessageA) });

            Assert.AreEqual(result, nameof(MessageA));

            Messenger.Unregister<MessageA>(a);

            Assert.IsFalse(Messenger.IsRegistered<MessageA>(a));

            result = null;
            Messenger.Send(new MessageA { Text = nameof(MessageA) });

            Assert.IsNull(result);
        }

        [TestCategory("Mvvm")]
        [TestMethod]
        public void Test_Ioc_IsRegistered_Register_Send_UnregisterRecipient_WithNoToken()
        {
            object a = new object();

            Assert.IsFalse(Messenger.IsRegistered<MessageA>(a));

            string result = null;
            Messenger.Register<MessageA>(a, m => result = m.Text);

            Assert.IsTrue(Messenger.IsRegistered<MessageA>(a));

            Messenger.Send(new MessageA { Text = nameof(MessageA) });

            Assert.AreEqual(result, nameof(MessageA));

            Messenger.Unregister(a);

            Assert.IsFalse(Messenger.IsRegistered<MessageA>(a));

            result = null;
            Messenger.Send(new MessageA { Text = nameof(MessageA) });

            Assert.IsNull(result);
        }

        [TestCategory("Mvvm")]
        [TestMethod]
        public void Test_Ioc_IsRegistered_Register_Send_UnregisterOfTMessage_WithToken()
        {
            object a = new object();

            Assert.IsFalse(Messenger.IsRegistered<MessageA>(a));

            string result = null;
            Messenger.Register<MessageA, string>(a, nameof(MessageA), m => result = m.Text);

            Assert.IsTrue(Messenger.IsRegistered<MessageA, string>(a, nameof(MessageA)));

            Messenger.Send(new MessageA { Text = nameof(MessageA) }, nameof(MessageA));

            Assert.AreEqual(result, nameof(MessageA));

            Messenger.Unregister<MessageA, string>(a, nameof(MessageA));

            Assert.IsFalse(Messenger.IsRegistered<MessageA, string>(a, nameof(MessageA)));

            result = null;
            Messenger.Send(new MessageA { Text = nameof(MessageA) }, nameof(MessageA));

            Assert.IsNull(result);
        }

        public sealed class MessageA
        {
            public string Text { get; set; }
        }
    }
}
