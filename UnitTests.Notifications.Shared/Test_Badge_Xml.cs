// ******************************************************************
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

#if WINDOWS_UWP
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
#else
using Microsoft.VisualStudio.TestTools.UnitTesting;
#endif
using Microsoft.Toolkit.Uwp.Notifications;

namespace UnitTests.Notifications
{
    [TestClass]
    public class Test_Badge_Xml
    {
        [TestMethod]
        public void Test_Badge_Xml_Numeric_0()
        {
            AssertBadgeValue("0", new BadgeNumericContent(0));
        }

        [TestMethod]
        public void Test_Badge_Xml_Numeric_1()
        {
            AssertBadgeValue("1", new BadgeNumericContent(1));
        }

        [TestMethod]
        public void Test_Badge_Xml_Numeric_2()
        {
            AssertBadgeValue("2", new BadgeNumericContent(2));
        }

        [TestMethod]
        public void Test_Badge_Xml_Numeric_546()
        {
            AssertBadgeValue("546", new BadgeNumericContent(546));
        }

        [TestMethod]
        public void Test_Badge_Xml_Numeric_Max()
        {
            AssertBadgeValue(uint.MaxValue.ToString(), new BadgeNumericContent(uint.MaxValue));
        }

        [TestMethod]
        public void Test_Badge_Xml_Glyph_None()
        {
            AssertBadgeValue("none", new BadgeGlyphContent(BadgeGlyphValue.None));
        }

        [TestMethod]
        public void Test_Badge_Xml_Glyph_Alert()
        {
            AssertBadgeValue("alert", new BadgeGlyphContent(BadgeGlyphValue.Alert));
        }

        [TestMethod]
        public void Test_Badge_Xml_Glyph_Error()
        {
            AssertBadgeValue("error", new BadgeGlyphContent(BadgeGlyphValue.Error));
        }

        private static void AssertBadgeValue(string expectedValue, INotificationContent notificationContent)
        {
            AssertPayload("<badge value='" + expectedValue + "'/>", notificationContent);
        }

        private static void AssertPayload(string expectedXml, INotificationContent notificationContent)
        {
            AssertHelper.AssertXml(expectedXml, notificationContent.GetContent());

#if WINDOWS_UWP
            // For WinRT, we'll test the XmlDocument method too, make sure it works.
            AssertHelper.AssertXml(expectedXml, notificationContent.GetXml().GetXml());
#endif
        }
    }
}
