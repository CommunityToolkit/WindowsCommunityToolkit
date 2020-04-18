// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using Microsoft.Toolkit.Mvvm.Messaging;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTests.Mvvm
{
    [TestClass]
    public class Test_Messenger
    {
        [TestCategory("Mvvm")]
        [TestMethod]
        public void Test_Messenger_UnregisterRecipientWithMessageType()
        {
            var messenger = new Messenger();
            var recipient = new object();

            messenger.Unregister<MessageA>(recipient);
        }

        [TestCategory("Mvvm")]
        [TestMethod]
        public void Test_Messenger_UnregisterRecipientWithMessageTypeAndToken()
        {
            var messenger = new Messenger();
            var recipient = new object();

            messenger.Unregister<MessageA, string>(recipient, nameof(MessageA));
        }

        [TestCategory("Mvvm")]
        [TestMethod]
        public void Test_Messenger_UnregisterRecipientWithToken()
        {
            var messenger = new Messenger();
            var recipient = new object();

            messenger.Unregister(recipient, nameof(MessageA));
        }

        [TestCategory("Mvvm")]
        [TestMethod]
        public void Test_Messenger_UnregisterRecipientWithRecipient()
        {
            var messenger = new Messenger();
            var recipient = new object();

            messenger.Unregister(recipient);
        }

        [TestCategory("Mvvm")]
        [TestMethod]
        public void Test_Messenger_RegisterAndUnregisterRecipientWithMessageType()
        {
            var messenger = new Messenger();
            var recipient = new object();

            messenger.Register<MessageA>(recipient, m => { });

            messenger.Unregister<MessageA>(recipient);

            Assert.IsFalse(messenger.IsRegistered<MessageA>(recipient));
        }

        [TestCategory("Mvvm")]
        [TestMethod]
        public void Test_Messenger_RegisterAndUnregisterRecipientWithMessageTypeAndToken()
        {
            var messenger = new Messenger();
            var recipient = new object();

            messenger.Register<MessageA, string>(recipient, nameof(MessageA), m => { });

            messenger.Unregister<MessageA, string>(recipient, nameof(MessageA));

            Assert.IsFalse(messenger.IsRegistered<MessageA, string>(recipient, nameof(MessageA)));
        }

        [TestCategory("Mvvm")]
        [TestMethod]
        public void Test_Messenger_RegisterAndUnregisterRecipientWithToken()
        {
            var messenger = new Messenger();
            var recipient = new object();

            messenger.Register<MessageA, string>(recipient, nameof(MessageA), m => { });

            messenger.Unregister(recipient, nameof(MessageA));

            Assert.IsFalse(messenger.IsRegistered<MessageA, string>(recipient, nameof(MessageA)));
        }

        [TestCategory("Mvvm")]
        [TestMethod]
        public void Test_Messenger_RegisterAndUnregisterRecipientWithRecipient()
        {
            var messenger = new Messenger();
            var recipient = new object();

            messenger.Register<MessageA, string>(recipient, nameof(MessageA), m => { });

            messenger.Unregister(recipient);

            Assert.IsFalse(messenger.IsRegistered<MessageA, string>(recipient, nameof(MessageA)));
        }

        [TestCategory("Mvvm")]
        [TestMethod]
        public void Test_Messenger_IsRegistered_Register_Send_UnregisterOfTMessage_WithNoToken()
        {
            object a = new object();

            Assert.IsFalse(Messenger.Default.IsRegistered<MessageA>(a));

            string result = null;
            Messenger.Default.Register<MessageA>(a, m => result = m.Text);

            Assert.IsTrue(Messenger.Default.IsRegistered<MessageA>(a));

            Messenger.Default.Send(new MessageA { Text = nameof(MessageA) });

            Assert.AreEqual(result, nameof(MessageA));

            Messenger.Default.Unregister<MessageA>(a);

            Assert.IsFalse(Messenger.Default.IsRegistered<MessageA>(a));

            result = null;
            Messenger.Default.Send(new MessageA { Text = nameof(MessageA) });

            Assert.IsNull(result);
        }

        [TestCategory("Mvvm")]
        [TestMethod]
        public void Test_Messenger_IsRegistered_Register_Send_UnregisterRecipient_WithNoToken()
        {
            object a = new object();

            Assert.IsFalse(Messenger.Default.IsRegistered<MessageA>(a));

            string result = null;
            Messenger.Default.Register<MessageA>(a, m => result = m.Text);

            Assert.IsTrue(Messenger.Default.IsRegistered<MessageA>(a));

            Messenger.Default.Send(new MessageA { Text = nameof(MessageA) });

            Assert.AreEqual(result, nameof(MessageA));

            Messenger.Default.Unregister(a);

            Assert.IsFalse(Messenger.Default.IsRegistered<MessageA>(a));

            result = null;
            Messenger.Default.Send(new MessageA { Text = nameof(MessageA) });

            Assert.IsNull(result);
        }

        [TestCategory("Mvvm")]
        [TestMethod]
        public void Test_Messenger_IsRegistered_Register_Send_UnregisterOfTMessage_WithToken()
        {
            object a = new object();

            Assert.IsFalse(Messenger.Default.IsRegistered<MessageA>(a));

            string result = null;
            Messenger.Default.Register<MessageA, string>(a, nameof(MessageA), m => result = m.Text);

            Assert.IsTrue(Messenger.Default.IsRegistered<MessageA, string>(a, nameof(MessageA)));

            Messenger.Default.Send(new MessageA { Text = nameof(MessageA) }, nameof(MessageA));

            Assert.AreEqual(result, nameof(MessageA));

            Messenger.Default.Unregister<MessageA, string>(a, nameof(MessageA));

            Assert.IsFalse(Messenger.Default.IsRegistered<MessageA, string>(a, nameof(MessageA)));

            result = null;
            Messenger.Default.Send(new MessageA { Text = nameof(MessageA) }, nameof(MessageA));

            Assert.IsNull(result);
        }

        [TestCategory("Mvvm")]
        [TestMethod]
        public void Test_Messenger_DuplicateRegistrationWithMessageType()
        {
            var messenger = new Messenger();
            var recipient = new object();

            messenger.Register<MessageA>(recipient, m => { });

            Assert.ThrowsException<InvalidOperationException>(() =>
            {
                messenger.Register<MessageA>(recipient, m => { });
            });
        }

        [TestCategory("Mvvm")]
        [TestMethod]
        public void Test_Messenger_DuplicateRegistrationWithMessageTypeAndToken()
        {
            var messenger = new Messenger();
            var recipient = new object();

            messenger.Register<MessageA, string>(recipient, nameof(MessageA), m => { });

            Assert.ThrowsException<InvalidOperationException>(() =>
            {
                messenger.Register<MessageA, string>(recipient, nameof(MessageA), m => { });
            });
        }

        public sealed class MessageA
        {
            public string Text { get; set; }
        }
    }
}
