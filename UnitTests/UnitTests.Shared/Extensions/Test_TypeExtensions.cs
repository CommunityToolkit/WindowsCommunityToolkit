// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using Microsoft.Toolkit.Extensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTests.Extensions
{
    [TestClass]
    public class Test_TypeExtensions
    {
        [TestCategory("TypeExtensions")]
        [TestMethod]
        [DataRow("bool", typeof(bool))]
        [DataRow("int", typeof(int))]
        [DataRow("float", typeof(float))]
        [DataRow("double", typeof(double))]
        [DataRow("decimal", typeof(decimal))]
        [DataRow("object", typeof(object))]
        [DataRow("string", typeof(string))]
        public void Test_TypeExtensions_BuiltInTypes(string name, Type type)
        {
            Assert.AreEqual(name, type.ToTypeString());
        }

        [TestCategory("TypeExtensions")]
        [TestMethod]
        [DataRow("int?", typeof(int?))]
        [DataRow("System.DateTime?", typeof(DateTime?))]
        [DataRow("(int, float)", typeof((int, float)))]
        [DataRow("(double?, string, int)?", typeof((double?, string, int)?))]
        [DataRow("int[]", typeof(int[]))]
        [DataRow("int[,]", typeof(int[,]))]
        [DataRow("System.Span<float>", typeof(Span<float>))]
        [DataRow("System.Memory<char>", typeof(Memory<char>))]
        [DataRow("System.Collections.Generic.IEnumerable<int>", typeof(IEnumerable<int>))]
        [DataRow("System.Collections.Generic.Dictionary<int, System.Collections.Generic.List<float>>", typeof(Dictionary<int, List<float>>))]
        public void Test_TypeExtensions_GenericTypes(string name, Type type)
        {
            Assert.AreEqual(name, type.ToTypeString());
        }

        [TestCategory("TypeExtensions")]
        [TestMethod]
        [DataRow("UnitTests.Extensions.Test_TypeExtensions.Animal", typeof(Animal))]
        [DataRow("UnitTests.Extensions.Test_TypeExtensions.Animal.Cat", typeof(Animal.Cat))]
        [DataRow("UnitTests.Extensions.Test_TypeExtensions.Animal.Dog", typeof(Animal.Dog))]
        [DataRow("UnitTests.Extensions.Test_TypeExtensions.Animal.Rabbit<int?>", typeof(Animal.Rabbit<int?>))]
        [DataRow("UnitTests.Extensions.Test_TypeExtensions.Animal.Rabbit<string>", typeof(Animal.Rabbit<string>))]
        [DataRow("UnitTests.Extensions.Test_TypeExtensions.Animal.Rabbit<int>.Foo", typeof(Animal.Rabbit<int>.Foo))]
        [DataRow("UnitTests.Extensions.Test_TypeExtensions.Animal.Rabbit<(string, int)?>.Foo", typeof(Animal.Rabbit<(string, int)?>.Foo))]
        [DataRow("UnitTests.Extensions.Test_TypeExtensions.Animal.Rabbit<int>.Foo<string>", typeof(Animal.Rabbit<int>.Foo<string>))]
        [DataRow("UnitTests.Extensions.Test_TypeExtensions.Animal.Rabbit<int>.Foo<int[]>", typeof(Animal.Rabbit<int>.Foo<int[]>))]
        [DataRow("UnitTests.Extensions.Test_TypeExtensions.Animal.Rabbit<string[]>.Foo<object>", typeof(Animal.Rabbit<string[]>.Foo<object>))]
        [DataRow("UnitTests.Extensions.Test_TypeExtensions.Animal.Rabbit<(string, int)?>.Foo<(int, int?)>", typeof(Animal.Rabbit<(string, int)?>.Foo<(int, int?)>))]
        [DataRow("UnitTests.Extensions.Test_TypeExtensions.Animal.Giraffe<float, System.DateTime>", typeof(Animal.Giraffe<float, DateTime>))]
        [DataRow("UnitTests.Extensions.Test_TypeExtensions.Animal.Giraffe<string, (int?, object)>", typeof(Animal.Giraffe<string, (int?, object)>))]
        [DataRow("UnitTests.Extensions.Test_TypeExtensions.Animal.Giraffe<string, (int?, object)?>.Foo", typeof(Animal.Giraffe<string, (int?, object)?>.Foo))]
        [DataRow("UnitTests.Extensions.Test_TypeExtensions.Animal.Giraffe<float, System.DateTime>.Foo", typeof(Animal.Giraffe<float, DateTime>.Foo))]
        [DataRow("UnitTests.Extensions.Test_TypeExtensions.Animal.Giraffe<string, (int?, object)?>.Foo<string>", typeof(Animal.Giraffe<string, (int?, object)?>.Foo<string>))]
        [DataRow("UnitTests.Extensions.Test_TypeExtensions.Animal.Giraffe<float, System.DateTime>.Foo<(float?, int)?>", typeof(Animal.Giraffe<float, DateTime>.Foo<(float?, int)?>))]
        [DataRow("UnitTests.Extensions.Test_TypeExtensions.Vehicle<double>", typeof(Vehicle<double>))]
        [DataRow("UnitTests.Extensions.Test_TypeExtensions.Vehicle<int?>[]", typeof(Vehicle<int?>[]))]
        [DataRow("System.Collections.Generic.List<UnitTests.Extensions.Test_TypeExtensions.Vehicle<int>>", typeof(List<Vehicle<int>>))]
        [DataRow("System.Collections.Generic.List<UnitTests.Extensions.Test_TypeExtensions.Animal.Rabbit<int?>>", typeof(List<Animal.Rabbit<int?>>))]
        [DataRow("System.Collections.Generic.List<UnitTests.Extensions.Test_TypeExtensions.Animal.Giraffe<float, System.DateTime[]>>", typeof(List<Animal.Giraffe<float, DateTime[]>>))]
        public void Test_TypeExtensions_NestedTypes(string name, Type type)
        {
            Assert.AreEqual(name, type.ToTypeString());
        }

        private class Animal
        {
            public struct Cat
            {
            }

            public struct Cat<T1>
            {
                public struct Bar
                {
                }

                public struct Bar<T2>
                {
                }
            }

            public class Dog
            {
            }

            public class Rabbit<T>
            {
                public class Foo
                {
                }

                public class Foo<T2>
                {
                }
            }

            public class Giraffe<T1, T2>
            {
                public class Foo
                {
                }

                public class Foo<T3>
                {
                }
            }
        }

        private class Vehicle<T>
        {
        }
    }

    internal struct Foo<T>
    {
    }
}
