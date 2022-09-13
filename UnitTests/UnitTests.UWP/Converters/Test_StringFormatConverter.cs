// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Microsoft.Toolkit.Uwp.UI.Converters;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Globalization;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace UnitTests.Converters
{
    [TestClass]
    public class Test_StringFormatConverter
    {
        private static readonly object NotEmptyString = "Hello, world";
        private static readonly DateTime Date = DateTime.Now;
        private static readonly decimal Amount = 333.4m;
        
        [TestCategory("Converters")]
        [TestMethod]
        [DataRow(null)]
        [DataRow("en-us")]
        public void WhenValueIsNull_ThenReturnNull(string language)
        {
            var converter = new StringFormatConverter();
            var result = converter.Convert(null, typeof(string), null, language);

            Assert.IsNull(result);
        }

        [TestCategory("Converters")]
        [TestMethod]
        [DataRow(null)]
        [DataRow("en-us")]
        public void WhenValueExistsAndParameterIsNull_ThenReturnValue(string language)
        {
            var converter = new StringFormatConverter();
            var result = converter.Convert(NotEmptyString, typeof(string), null, language);

            Assert.AreEqual(NotEmptyString, result);
        }

        [TestCategory("Converters")]
        [TestMethod]
        [DataRow(null)]
        [DataRow("en-us")]
        public void WhenParameterIsInvalidFormat_ThenReturnValue(string language)
        {
            var converter = new StringFormatConverter();
            var result = converter.Convert(Date, typeof(string), "{1:}", language);

            Assert.AreEqual(Date, result);
        }

        [TestCategory("Converters")]
        [TestMethod]
        [DataRow(null)]
        [DataRow("en-us")]
        public void WhenParameterIsNotAString_ThenReturnValue(string language)
        {
            var converter = new StringFormatConverter();
            var result = converter.Convert(NotEmptyString, typeof(string), 172, language);

            Assert.AreEqual(NotEmptyString, result);
        }

        [TestCategory("Converters")]
        [TestMethod]
        [DataRow("{0:ddd d MMM}", "ddd d MMM")]
        [DataRow("{0:HH:mm}", "HH:mm")]
        [DataRow("{0:hh:mm:ss tt}", "hh:mm:ss tt")]
        public void WhenValueIsDateTimeAndLanguageIsUnknownAndCultureInfoIsNotSet_ThenReturnValueOfTimeInInvariantCultureFormat(string converterParameter, string expectedFormat)
        {
            var converter = new StringFormatConverter();
            var result = converter.Convert(Date, typeof(string), converterParameter, null);

            Assert.AreEqual(Date.ToString(expectedFormat, CultureInfo.InvariantCulture), result);
        }

        [TestCategory("Converters")]
        [TestMethod]
        [DataRow("{0:ddd d MMM}", "ddd d MMM")]
        [DataRow("{0:HH:mm}", "HH:mm")]
        [DataRow("{0:hh:mm:ss tt}", "hh:mm:ss tt")]
        public void WhenValueIsDateTimeAndLanguageIsUnknownAndCultureInfoIsSet_ThenReturnValueOfTimeInConverterCultureInfoFormat(string converterParameter, string expectedFormat)
        {
            var converter = new StringFormatConverter();
            var result = converter.Convert(Date, typeof(string), converterParameter, null);

            Assert.AreEqual(Date.ToString(expectedFormat, CultureInfo.InvariantCulture), result);
        }

        [TestCategory("Converters")]
        [TestMethod]
        [DataRow("en-us", "{0:ddd d MMM}", "ddd d MMM")]
        [DataRow("en-us", "{0:HH:mm}", "HH:mm")]
        [DataRow("en-us", "{0:hh:mm:ss tt}", "hh:mm:ss tt")]
        public void WhenValueIsDateTimeAndLanguageIsWellKnownAndCultureInfoIsSet_ThenReturnValueOfTimeInLanguageCultureFormat(string language, string converterParameter, string expectedFormat)
        {
            var converter = new StringFormatConverter();
            var result = converter.Convert(Date, typeof(string), converterParameter, language);
            Assert.AreEqual(Date.ToString(expectedFormat, CultureInfo.GetCultureInfo(language)), result);
        }

        [TestCategory("Converters")]
        [TestMethod]
        [DataRow("{0:C2}", "C2")]
        [DataRow("{0:E}", "E")]
        public void WhenValueIsDecimalAndLanguageIsUnknownAndCultureInfoIsNotSet_ThenReturnValueOfDecimalInInvariantCultureFormat(string converterParameter, string expectedFormat)
        {
            var converter = new StringFormatConverter();
            var result = converter.Convert(Amount, typeof(string), converterParameter, null);

            Assert.AreEqual(Amount.ToString(expectedFormat, CultureInfo.InvariantCulture), result);
        }

        [TestCategory("Converters")]
        [TestMethod]
        [DataRow("en-us", "{0:C2}", "C2")]
        [DataRow("en-us", "{0:E}", "E")]
        [DataRow("fr-FR", "{0:E}", "E")]
        public void WhenValueIsDecimalAndLanguageIsWellKnown_ThenReturnValueOfDecimalInLanguageCultureFormat(string language, string converterParameter, string expectedFormat)
        {
            var converter = new StringFormatConverter();
            var result = converter.Convert(Amount, typeof(string), converterParameter, language);

            Assert.AreEqual(Amount.ToString(expectedFormat, CultureInfo.GetCultureInfo(language)), result);
        }
    }
}