﻿// ******************************************************************
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

namespace Tests
{
    using System;
    using System.Diagnostics;
    using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
    using UnitTests;
    using Microsoft.Toolkit.Uwp.Helpers;

    [TestClass]
    public class Test_DeepLinkParser
    {
        public TestContext TestContext { get; set; }

        private const string SAMPLELINK = @"myapp://MainPage/Options?option1=value1&option2=value2&option3=value3";
        private static readonly Uri SAMPLELINKURI = new Uri(SAMPLELINK);
        private static readonly DeepLinkParser _stringParser = new TestDeepLinkParser(SAMPLELINK);
        private static readonly DeepLinkParser _uriParser = new TestDeepLinkParser(SAMPLELINKURI);

        [TestMethod]
        public void Test_DeepLink_string_RootValue()
        {
            Assert.AreEqual("MainPage/Options", _stringParser.Root);
        }

        [TestMethod]
        public void Test_DeepLink_string_PullOptions()
        {
            Assert.AreEqual("value1", _stringParser["option1"]);
            Assert.AreEqual("value2", _stringParser["option2"]);
            Assert.AreEqual("value3", _stringParser["option3"]);
        }

        [TestMethod]
        public void Test_DeepLink_string_OptionNotFound()
        {
            try
            {
                var nonexistentvalue = _stringParser["nonexistentoption"];

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
        public void Test_DeepLink_string_DuplicateKeys()
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

        [TestMethod]
        public void Test_DeepLink_string_null_empty_whitespace()
        {
            try
            {
                string s = null;
                var p = new TestDeepLinkParser(s);

                Assert.Fail("Should have thrown ArgumentNullException for null string");
            }
            catch (ArgumentNullException aex)
            {
                Debug.WriteLine(aex.ToString());
            }
            catch
            {
                Assert.Fail("Should have thrown ArgumentNullException for null string");
            }

            try
            {
                string s = string.Empty;
                var p = new TestDeepLinkParser(s);

                Assert.Fail("Should have thrown ArgumentNullException for empty string");
            }
            catch (ArgumentNullException aex)
            {
                Debug.WriteLine(aex.ToString());
            }
            catch
            {
                Assert.Fail("Should have thrown ArgumentNullException for empty string");
            }

            try
            {
                string s = "   ";
                var p = new TestDeepLinkParser(s);

                Assert.Fail("Should have thrown ArgumentNullException for whitespace-only string");
            }
            catch (ArgumentNullException aex)
            {
                Debug.WriteLine(aex.ToString());
            }
            catch
            {
                Assert.Fail("Should have thrown ArgumentNullException for whitespace-only string");
            }
        }

        [TestMethod]
        public void Test_DeepLink_uri_RootValue()
        {
            Assert.AreEqual("MainPage/Options", _uriParser.Root);
        }

        [TestMethod]
        public void Test_DeepLink_uri_PullOptions()
        {
            Assert.AreEqual("value1", _uriParser["option1"]);
            Assert.AreEqual("value2", _uriParser["option2"]);
            Assert.AreEqual("value3", _uriParser["option3"]);
        }

        [TestMethod]
        public void Test_DeepLink_uri_OptionNotFound()
        {
            try
            {
                var nonexistentvalue = _uriParser["nonexistentoption"];

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
        public void Test_DeepLink_uri_DuplicateKeys()
        {
            try
            {
                var s = new Uri(SAMPLELINKURI.OriginalString + "&option2=value4");
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

        [TestMethod]
        public void Test_DeepLink_uri_null()
        {
            try
            {
                Uri s = null;
                var p = new TestDeepLinkParser(s);

                Assert.Fail("Should have thrown ArgumentNullException for null string");
            }
            catch (ArgumentNullException aex)
            {
                Debug.WriteLine(aex.ToString());
            }
            catch
            {
                Assert.Fail("Should have thrown ArgumentNullException for null string");
            }
        }
    }

    [TestClass]
#pragma warning disable SA1402 // File may only contain a single class
    public class CollectionCapableDeepLinkParserTests
#pragma warning restore SA1402 // File may only contain a single class
    {
        private const string SAMPLELINK = @"myapp://MainPage/Options?option1=value1&option2=value2&option3=value3&option2=value4";
        private static readonly Uri SAMPLELINKURI = new Uri(SAMPLELINK);
        private static readonly DeepLinkParser _stringParser = new TestCollectionCapableDeepLinkParser(SAMPLELINK);
        private static readonly DeepLinkParser _uriParser = new TestCollectionCapableDeepLinkParser(SAMPLELINKURI);

        [TestMethod]
        public void Test_DeepLinkCollection_string_RootValue()
        {
            Assert.AreEqual("MainPage/Options", _stringParser.Root);
        }

        [TestMethod]
        public void Test_DeepLinkCollection_string_PullOptions()
        {
            Assert.AreEqual("value1", _stringParser["option1"]);
            Assert.AreEqual("value2,value4", _stringParser["option2"]);
            Assert.AreEqual("value3", _stringParser["option3"]);
        }

        [TestMethod]
        public void Test_DeepLinkCollection_string_OptionNotFound()
        {
            try
            {
                var nonexistentvalue = _stringParser["nonexistentoption"];

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
        public void Test_DeepLinkCollection_string_null_empty_whitespace()
        {
            try
            {
                string s = null;
                var p = new TestCollectionCapableDeepLinkParser(s);

                Assert.Fail("Should have thrown ArgumentNullException for null string");
            }
            catch (ArgumentNullException aex)
            {
                Debug.WriteLine(aex.ToString());
            }
            catch
            {
                Assert.Fail("Should have thrown ArgumentNullException for null string");
            }

            try
            {
                string s = string.Empty;
                var p = new TestCollectionCapableDeepLinkParser(s);

                Assert.Fail("Should have thrown ArgumentNullException for empty string");
            }
            catch (ArgumentNullException aex)
            {
                Debug.WriteLine(aex.ToString());
            }
            catch
            {
                Assert.Fail("Should have thrown ArgumentNullException for empty string");
            }

            try
            {
                string s = "   ";
                var p = new TestCollectionCapableDeepLinkParser(s);

                Assert.Fail("Should have thrown ArgumentNullException for whitespace-only string");
            }
            catch (ArgumentNullException aex)
            {
                Debug.WriteLine(aex.ToString());
            }
            catch
            {
                Assert.Fail("Should have thrown ArgumentNullException for whitespace-only string");
            }
        }

        [TestMethod]
        public void Test_DeepLinkCollection_uri_RootValue()
        {
            Assert.AreEqual("MainPage/Options", _uriParser.Root);
        }

        [TestMethod]
        public void Test_DeepLinkCollection_uri_PullOptions()
        {
            Assert.AreEqual("value1", _uriParser["option1"]);
            Assert.AreEqual("value2,value4", _uriParser["option2"]);
            Assert.AreEqual("value3", _uriParser["option3"]);
        }

        [TestMethod]
        public void Test_DeepLinkCollection_uri_OptionNotFound()
        {
            try
            {
                var nonexistentvalue = _uriParser["nonexistentoption"];

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
        public void Test_DeepLinkCollection_uri_null()
        {
            try
            {
                Uri s = null;
                var p = new TestCollectionCapableDeepLinkParser(s);

                Assert.Fail("Should have thrown ArgumentNullException for null string");
            }
            catch (ArgumentNullException aex)
            {
                Debug.WriteLine(aex.ToString());
            }
            catch
            {
                Assert.Fail("Should have thrown ArgumentNullException for null string");
            }
        }
    }
}
