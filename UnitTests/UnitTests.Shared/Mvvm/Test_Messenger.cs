// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using Microsoft.Toolkit.Mvvm.Messaging;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTests.Mvvm
{
    [TestClass]
    public partial class Test_Messenger
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

            messenger.UnregisterAll(recipient, nameof(MessageA));
        }

        [TestCategory("Mvvm")]
        [TestMethod]
        public void Test_Messenger_UnregisterRecipientWithRecipient()
        {
            var messenger = new Messenger();
            var recipient = new object();

            messenger.UnregisterAll(recipient);
        }

        [TestCategory("Mvvm")]
        [TestMethod]
        public void Test_Messenger_RegisterAndUnregisterRecipientWithMessageType()
        {
            var messenger = new Messenger();
            var recipient = new object();

            messenger.Register<MessageA>(recipient, (r, m) => { });

            messenger.Unregister<MessageA>(recipient);

            Assert.IsFalse(messenger.IsRegistered<MessageA>(recipient));
        }

        [TestCategory("Mvvm")]
        [TestMethod]
        public void Test_Messenger_RegisterAndUnregisterRecipientWithMessageTypeAndToken()
        {
            var messenger = new Messenger();
            var recipient = new object();

            messenger.Register<MessageA, string>(recipient, nameof(MessageA), (r, m) => { });

            messenger.Unregister<MessageA, string>(recipient, nameof(MessageA));

            Assert.IsFalse(messenger.IsRegistered<MessageA, string>(recipient, nameof(MessageA)));
        }

        [TestCategory("Mvvm")]
        [TestMethod]
        public void Test_Messenger_RegisterAndUnregisterRecipientWithToken()
        {
            var messenger = new Messenger();
            var recipient = new object();

            messenger.Register<MessageA, string>(recipient, nameof(MessageA), (r, m) => { });

            messenger.UnregisterAll(recipient, nameof(MessageA));

            Assert.IsFalse(messenger.IsRegistered<MessageA, string>(recipient, nameof(MessageA)));
        }

        [TestCategory("Mvvm")]
        [TestMethod]
        public void Test_Messenger_RegisterAndUnregisterRecipientWithRecipient()
        {
            var messenger = new Messenger();
            var recipient = new object();

            messenger.Register<MessageA, string>(recipient, nameof(MessageA), (r, m) => { });

            messenger.UnregisterAll(recipient);

            Assert.IsFalse(messenger.IsRegistered<MessageA, string>(recipient, nameof(MessageA)));
        }

        [TestCategory("Mvvm")]
        [TestMethod]
        public void Test_Messenger_IsRegistered_Register_Send_UnregisterOfTMessage_WithNoToken()
        {
            object a = new object();

            Assert.IsFalse(Messenger.Default.IsRegistered<MessageA>(a));

            string result = null;
            Messenger.Default.Register<MessageA>(a, (r, m) => result = m.Text);

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
            Messenger.Default.Register<MessageA>(a, (r, m) => result = m.Text);

            Assert.IsTrue(Messenger.Default.IsRegistered<MessageA>(a));

            Messenger.Default.Send(new MessageA { Text = nameof(MessageA) });

            Assert.AreEqual(result, nameof(MessageA));

            Messenger.Default.UnregisterAll(a);

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
            Messenger.Default.Register<MessageA, string>(a, nameof(MessageA), (r, m) => result = m.Text);

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

            messenger.Register<MessageA>(recipient, (r, m) => { });

            Assert.ThrowsException<InvalidOperationException>(() =>
            {
                messenger.Register<MessageA>(recipient, (r, m) => { });
            });
        }

        [TestCategory("Mvvm")]
        [TestMethod]
        public void Test_Messenger_DuplicateRegistrationWithMessageTypeAndToken()
        {
            var messenger = new Messenger();
            var recipient = new object();

            messenger.Register<MessageA, string>(recipient, nameof(MessageA), (r, m) => { });

            Assert.ThrowsException<InvalidOperationException>(() =>
            {
                messenger.Register<MessageA, string>(recipient, nameof(MessageA), (r, m) => { });
            });
        }

        [TestCategory("Mvvm")]
        [TestMethod]
        public void Test_Messenger_IRecipient_NoMessages()
        {
            var messenger = new Messenger();
            var recipient = new RecipientWithNoMessages();

            messenger.RegisterAll(recipient);

            // We just need to verify we got here with no errors, this
            // recipient has no declared handlers so there's nothing to do
        }

        [TestCategory("Mvvm")]
        [TestMethod]
        public void Test_Messenger_IRecipient_SomeMessages_NoToken()
        {
            var messenger = new Messenger();
            var recipient = new RecipientWithSomeMessages();

            messenger.RegisterAll(recipient);

            Assert.IsTrue(messenger.IsRegistered<MessageA>(recipient));
            Assert.IsTrue(messenger.IsRegistered<MessageB>(recipient));

            Assert.AreEqual(recipient.As, 0);
            Assert.AreEqual(recipient.Bs, 0);

            messenger.Send<MessageA>();

            Assert.AreEqual(recipient.As, 1);
            Assert.AreEqual(recipient.Bs, 0);

            messenger.Send<MessageB>();

            Assert.AreEqual(recipient.As, 1);
            Assert.AreEqual(recipient.Bs, 1);

            messenger.UnregisterAll(recipient);

            Assert.IsFalse(messenger.IsRegistered<MessageA>(recipient));
            Assert.IsFalse(messenger.IsRegistered<MessageB>(recipient));
        }

        [TestCategory("Mvvm")]
        [TestMethod]
        public void Test_Messenger_IRecipient_SomeMessages_WithToken()
        {
            var messenger = new Messenger();
            var recipient = new RecipientWithSomeMessages();
            var token = nameof(Test_Messenger_IRecipient_SomeMessages_WithToken);

            messenger.RegisterAll(recipient, token);

            Assert.IsTrue(messenger.IsRegistered<MessageA, string>(recipient, token));
            Assert.IsTrue(messenger.IsRegistered<MessageB, string>(recipient, token));

            Assert.IsFalse(messenger.IsRegistered<MessageA>(recipient));
            Assert.IsFalse(messenger.IsRegistered<MessageB>(recipient));

            Assert.AreEqual(recipient.As, 0);
            Assert.AreEqual(recipient.Bs, 0);

            messenger.Send<MessageB, string>(token);

            Assert.AreEqual(recipient.As, 0);
            Assert.AreEqual(recipient.Bs, 1);

            messenger.Send<MessageA, string>(token);

            Assert.AreEqual(recipient.As, 1);
            Assert.AreEqual(recipient.Bs, 1);

            messenger.UnregisterAll(recipient, token);

            Assert.IsFalse(messenger.IsRegistered<MessageA>(recipient));
            Assert.IsFalse(messenger.IsRegistered<MessageB>(recipient));
        }

        public sealed class RecipientWithNoMessages
        {
        }

        public sealed class RecipientWithSomeMessages
             : IRecipient<MessageA>, ICloneable, IRecipient<MessageB>
        {
            public int As { get; private set; }

            public void Receive(MessageA message)
            {
                As++;
            }

            public int Bs { get; private set; }

            public void Receive(MessageB message)
            {
                Bs++;
            }

            // We also add the ICloneable interface to test that the message
            // interfaces are all handled correctly even when inteleaved
            // by other unrelated interfaces in the type declaration.
            public object Clone() => throw new NotImplementedException();
        }

        public sealed class MessageA
        {
            public string Text { get; set; }
        }

        public sealed class MessageB
        {
        }
    }
}
