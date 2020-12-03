// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTests.Mvvm
{
    [TestClass]
    public class Test_ObservableValidator
    {
        [TestCategory("Mvvm")]
        [TestMethod]
        public void Test_ObservableValidator_HasErrors()
        {
            var model = new Person();
            var args = new List<PropertyChangedEventArgs>();

            model.PropertyChanged += (s, e) => args.Add(e);

            Assert.IsFalse(model.HasErrors);

            model.Name = "No";

            // Verify that errors were correctly reported as changed, and that all the relevant
            // properties were broadcast as well (both the changed property and HasErrors). We need
            // this last one to raise notifications too so that users can bind to that in the UI.
            Assert.IsTrue(model.HasErrors);
            Assert.AreEqual(args.Count, 2);
            Assert.AreEqual(args[0].PropertyName, nameof(Person.Name));
            Assert.AreEqual(args[1].PropertyName, nameof(INotifyDataErrorInfo.HasErrors));

            model.Name = "Valid";

            Assert.IsFalse(model.HasErrors);
            Assert.AreEqual(args.Count, 4);
            Assert.AreEqual(args[2].PropertyName, nameof(Person.Name));
            Assert.AreEqual(args[3].PropertyName, nameof(INotifyDataErrorInfo.HasErrors));
        }

        [TestCategory("Mvvm")]
        [TestMethod]
        public void Test_ObservableValidator_ErrorsChanged()
        {
            var model = new Person();

            List<(object Sender, DataErrorsChangedEventArgs Args)> errors = new List<(object, DataErrorsChangedEventArgs)>();

            model.ErrorsChanged += (s, e) => errors.Add((s, e));

            model.Name = "Foo";

            Assert.AreEqual(errors.Count, 1);
            Assert.AreSame(errors[0].Sender, model);
            Assert.AreEqual(errors[0].Args.PropertyName, nameof(Person.Name));

            errors.Clear();

            model.Name = "Bar";

            Assert.AreEqual(errors.Count, 1);
            Assert.AreSame(errors[0].Sender, model);
            Assert.AreEqual(errors[0].Args.PropertyName, nameof(Person.Name));

            errors.Clear();

            model.Name = "Valid";

            Assert.AreEqual(errors.Count, 1);
            Assert.AreSame(errors[0].Sender, model);
            Assert.AreEqual(errors[0].Args.PropertyName, nameof(Person.Name));

            errors.Clear();

            model.Name = "This is fine";

            Assert.AreEqual(errors.Count, 0);
        }

        [TestCategory("Mvvm")]
        [TestMethod]
        public void Test_ObservableValidator_GetErrors()
        {
            var model = new Person();

            Assert.AreEqual(model.GetErrors(null).Cast<object>().Count(), 0);
            Assert.AreEqual(model.GetErrors(string.Empty).Cast<object>().Count(), 0);
            Assert.AreEqual(model.GetErrors("ThereIsntAPropertyWithThisName").Cast<object>().Count(), 0);
            Assert.AreEqual(model.GetErrors(nameof(Person.Name)).Cast<object>().Count(), 0);

            model.Name = "Foo";

            var errors = model.GetErrors(nameof(Person.Name)).Cast<ValidationResult>().ToArray();

            Assert.AreEqual(errors.Length, 1);
            Assert.AreEqual(errors[0].MemberNames.First(), nameof(Person.Name));

            Assert.AreEqual(model.GetErrors("ThereIsntAPropertyWithThisName").Cast<object>().Count(), 0);

            errors = model.GetErrors(null).Cast<ValidationResult>().ToArray();

            Assert.AreEqual(errors.Length, 1);
            Assert.AreEqual(errors[0].MemberNames.First(), nameof(Person.Name));

            errors = model.GetErrors(string.Empty).Cast<ValidationResult>().ToArray();

            Assert.AreEqual(errors.Length, 1);
            Assert.AreEqual(errors[0].MemberNames.First(), nameof(Person.Name));

            model.Age = -1;

            errors = model.GetErrors(null).Cast<ValidationResult>().ToArray();

            Assert.AreEqual(errors.Length, 2);
            Assert.IsTrue(errors.Any(e => e.MemberNames.First().Equals(nameof(Person.Name))));
            Assert.IsTrue(errors.Any(e => e.MemberNames.First().Equals(nameof(Person.Age))));

            model.Age = 26;

            errors = model.GetErrors(null).Cast<ValidationResult>().ToArray();

            Assert.AreEqual(errors.Length, 1);
            Assert.IsTrue(errors.Any(e => e.MemberNames.First().Equals(nameof(Person.Name))));
            Assert.IsFalse(errors.Any(e => e.MemberNames.First().Equals(nameof(Person.Age))));
        }

        [TestCategory("Mvvm")]
        [TestMethod]
        [DataRow("", false)]
        [DataRow("No", false)]
        [DataRow("This text is really, really too long for the target property", false)]
        [DataRow("1234", true)]
        [DataRow("01234567890123456789", true)]
        [DataRow("Hello world", true)]
        public void Test_ObservableValidator_ValidateReturn(string value, bool isValid)
        {
            var model = new Person { Name = value };

            Assert.AreEqual(model.HasErrors, !isValid);

            if (isValid)
            {
                Assert.IsTrue(!model.GetErrors(nameof(Person.Name)).Cast<object>().Any());
            }
            else
            {
                Assert.IsTrue(model.GetErrors(nameof(Person.Name)).Cast<object>().Any());
            }
        }

        [TestCategory("Mvvm")]
        [TestMethod]
        public void Test_ObservableValidator_TrySetProperty()
        {
            var model = new Person();
            var events = new List<DataErrorsChangedEventArgs>();

            model.ErrorsChanged += (s, e) => events.Add(e);

            // Set a correct value, this should update the property
            Assert.IsTrue(model.TrySetName("Hello", out var errors));
            Assert.IsTrue(errors.Count == 0);
            Assert.IsTrue(events.Count == 0);
            Assert.AreEqual(model.Name, "Hello");
            Assert.IsFalse(model.HasErrors);

            // Invalid value #1, this should be ignored
            Assert.IsFalse(model.TrySetName(null, out errors));
            Assert.IsTrue(errors.Count > 0);
            Assert.IsTrue(events.Count == 0);
            Assert.AreEqual(model.Name, "Hello");
            Assert.IsFalse(model.HasErrors);

            // Invalid value #2, same as above
            Assert.IsFalse(model.TrySetName("This string is too long for the target property in this model and should fail", out errors));
            Assert.IsTrue(errors.Count > 0);
            Assert.IsTrue(events.Count == 0);
            Assert.AreEqual(model.Name, "Hello");
            Assert.IsFalse(model.HasErrors);

            // Correct value, this should update the property
            Assert.IsTrue(model.TrySetName("Hello world", out errors));
            Assert.IsTrue(errors.Count == 0);
            Assert.IsTrue(events.Count == 0);
            Assert.AreEqual(model.Name, "Hello world");
            Assert.IsFalse(model.HasErrors);

            // Actually set an invalid value to show some errors
            model.Name = "No";

            // Errors should now be present
            Assert.IsTrue(model.HasErrors);
            Assert.IsTrue(events.Count == 1);
            Assert.IsTrue(model.GetErrors(nameof(Person.Name)).Cast<ValidationResult>().Any());
            Assert.IsTrue(model.HasErrors);

            // Trying to set a correct property should clear the errors
            Assert.IsTrue(model.TrySetName("This is fine", out errors));
            Assert.IsTrue(errors.Count == 0);
            Assert.IsTrue(events.Count == 2);
            Assert.IsFalse(model.HasErrors);
            Assert.AreEqual(model.Name, "This is fine");
        }

        public class Person : ObservableValidator
        {
            private string name;

            [MinLength(4)]
            [MaxLength(20)]
            [Required]
            public string Name
            {
                get => this.name;
                set => SetProperty(ref this.name, value, true);
            }

            public bool TrySetName(string value, out IReadOnlyCollection<ValidationResult> errors)
            {
                return TrySetProperty(ref name, value, out errors, nameof(Name));
            }

            private int age;

            [Range(0, 100)]
            public int Age
            {
                get => this.age;
                set => SetProperty(ref this.age, value, true);
            }
        }
    }
}
