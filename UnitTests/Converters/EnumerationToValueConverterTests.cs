// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Microsoft.Toolkit.Uwp.UI.Converters;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace UnitTests.Converters
{
    [TestClass]
    public class EnumerationToValueConverterTests
    {
        [TestCategory("Converters")]
        [DataTestMethod]
        [DataRow(TestEnum.Value1, TestEnum.Value1, 10)]
        [DataRow(TestEnum.Value2, TestEnum.Value1, -23)]
        [DataRow(TestEnum.Value2, TestEnum.Value2, 10)]
        [DataRow(TestEnum.Value1, TestEnum.Value2, -23)]
        public void Convert_WithValidEnumInputs_ShouldReturnExpectedResult(TestEnum valueToConvert, TestEnum converterParameter, int expectedResult)
        {
            var converter = new EnumerationValueConverter
            {
                TrueValue = 10,
                FalseValue = -23,
            };

            var result = converter.Convert(valueToConvert, typeof(int), converterParameter, "language");

            Assert.AreEqual(expectedResult, result);
        }

        [TestCategory("Converters")]
        [DataTestMethod]
        [DataRow(TestEnum.Value1, (int)TestEnum.Value1, 10)]
        [DataRow(TestEnum.Value2, (int)TestEnum.Value1, -23)]
        [DataRow(TestEnum.Value2, (int)TestEnum.Value2, 10)]
        [DataRow(TestEnum.Value1, (int)TestEnum.Value2, -23)]
        public void Convert_WithValidIntegerInputs_ShouldReturnExpectedResult(TestEnum valueToConvert, int converterParameter, int expectedResult)
        {
            var converter = new EnumerationValueConverter
            {
                TrueValue = 10,
                FalseValue = -23,
            };

            var result = converter.Convert(valueToConvert, typeof(int), converterParameter, "language");

            Assert.AreEqual(expectedResult, result);
        }

        [TestCategory("Converters")]
        [DataTestMethod]
        [DataRow((int)TestEnum.Value1, (int)TestEnum.Value1, 10)]
        [DataRow((int)TestEnum.Value2, (int)TestEnum.Value1, -23)]
        [DataRow((int)TestEnum.Value2, (int)TestEnum.Value2, 10)]
        [DataRow((int)TestEnum.Value1, (int)TestEnum.Value2, -23)]
        public void Convert_WithValidIntegerValueAndParameter_ShouldReturnExpectedResult(int valueToConvert, int converterParameter, int expectedResult)
        {
            var converter = new EnumerationValueConverter
            {
                TrueValue = 10,
                FalseValue = -23,
            };

            var result = converter.Convert(valueToConvert, typeof(int), converterParameter, "language");

            Assert.AreEqual(expectedResult, result);
        }

        [TestCategory("Converters")]
        [DataTestMethod]
        [DataRow(true)]
        [DataRow("not-an-enum-value")]
        public void Convert_WithInvalidConverterParameter_ShouldAssert(object converterParameter)
        {
            var converter = new EnumerationValueConverter
            {
                TrueValue = 10,
                FalseValue = -23,
            };

            Action action = () => converter.Convert(TestEnum.Value1, typeof(int), converterParameter, language: "language");

            Assert.ThrowsException<ArgumentException>(action);
        }

        [TestCategory("Converters")]
        [DataTestMethod]
        [DataRow(true)]
        [DataRow("not-an-enum-value")]
        public void Convert_WithInvalidInput_ShouldThrow(object valueToConvert)
        {
            var converter = new EnumerationValueConverter
            {
                TrueValue = 10,
                FalseValue = -23,
            };

            Action action = () => converter.Convert(valueToConvert, typeof(int), parameter: TestEnum.Value1, language: "language");

            Assert.ThrowsException<ArgumentException>(action);
        }

        [TestCategory("Converters")]
        [TestMethod]
        public void ConvertBack_ShouldThrowNotImplementedException()
        {
            var converter = new EnumerationValueConverter
            {
                TrueValue = 10,
                FalseValue = -23,
            };

            Action action = () => converter.ConvertBack(10, typeof(TestEnum), parameter: null, language: null);

            Assert.ThrowsException<NotImplementedException>(action);
        }
    }

    public enum TestEnum
    {
        /// <summary>
        /// The first value of the enum.
        /// </summary>
        Value1,

        /// <summary>
        /// The second value of the enum.
        /// </summary>
        Value2,
    }
}
