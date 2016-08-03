using Microsoft.Toolkit.Uwp;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using System;
using System.Diagnostics;
using UnitTests;

namespace Tests
{
    [TestClass]
    public class DeepLinkParserTests
    {
        public TestContext TestContext { get; set; }

        private const string SAMPLELINK = @"MainPage/Options?option1=value1&option2=value2&option3=value3";
        private static readonly DeepLinkParser _parser = new TestDeepLinkParser(SAMPLELINK);

        [TestMethod]
        public void Test_DeepLink_RootValue()
        {
            Assert.AreEqual("MainPage/Options", _parser.Root);
        }

        [TestMethod]
        public void Test_DeepLink_PullOptions()
        {
            Assert.AreEqual("value1", _parser["option1"]);
            Assert.AreEqual("value2", _parser["option2"]);
            Assert.AreEqual("value3", _parser["option3"]);
        }

        [TestMethod]
        public void Test_DeepLink_OptionNotFound()
        {
            try
            {
                var nonexistentvalue = _parser["nonexistentoption"];

                Assert.Fail("Should have thrown KeyNotFoundException");
            }
            catch (System.Collections.Generic.KeyNotFoundException)
            {
            }
            catch
            {
                Assert.Fail("Should have thrown KeyNotFoundException");
            }
        }

        [TestMethod]
        public void Test_DeepLink_DuplicateKeys()
        {
            try
            {
                var s = SAMPLELINK + "&option2=value4";
                var p = new TestDeepLinkParser(s);

                Assert.Fail("Should have thrown ArgumentException");
            }
            catch (ArgumentException aex)
            {
                Debug.WriteLine(aex.ToString());
            }
            catch
            {
                Assert.Fail("Should have thrown ArgumentException");
            }
        }
    }

    [TestClass]
#pragma warning disable SA1402 // File may only contain a single class
    public class CollectionCapableDeepLinkParserTests
#pragma warning restore SA1402 // File may only contain a single class
    {
        private const string SAMPLELINK = @"MainPage/Options?option1=value1&option2=value2&option3=value3&option2=value4";
        private static readonly DeepLinkParser _parser = new TestCollectionCapableDeepLinkParser(SAMPLELINK);

        [TestMethod]
        public void Test_CollectionDeepLink_RootValue()
        {
            Assert.AreEqual("MainPage/Options", _parser.Root);
        }

        [TestMethod]
        public void Test_CollectionDeepLink_PullOptions()
        {
            Assert.AreEqual("value1", _parser["option1"]);
            Assert.AreEqual("value2,value4", _parser["option2"]);
            Assert.AreEqual("value3", _parser["option3"]);
        }

        [TestMethod]
        public void Test_CollectionDeepLink_OptionNotFound()
        {
            try
            {
                var nonexistentvalue = _parser["nonexistentoption"];

                Assert.Fail("Should have thrown KeyNotFoundException");
            }
            catch (System.Collections.Generic.KeyNotFoundException)
            {
            }
            catch
            {
                Assert.Fail("Should have thrown KeyNotFoundException");
            }
        }
    }
}
