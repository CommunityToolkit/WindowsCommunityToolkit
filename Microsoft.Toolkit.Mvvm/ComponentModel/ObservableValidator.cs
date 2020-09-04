// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;

namespace Microsoft.Toolkit.Mvvm.ComponentModel
{
    /// <summary>
    /// A base class for objects implementing the <see cref="INotifyDataErrorInfo"/> interface. This class
    /// also inherits from <see cref="ObservableObject"/>, so it can be used for observable items too.
    /// </summary>
    public abstract class ObservableValidator : ObservableObject, INotifyDataErrorInfo
    {
        /// <summary>
        /// The <see cref="Dictionary{TKey,TValue}"/> instance used to store previous validation results.
        /// </summary>
        private readonly Dictionary<string, List<ValidationResult>> errors = new Dictionary<string, List<ValidationResult>>();

        /// <summary>
        /// Indicates the total number of properties with errors (not total errors).
        /// This is used to allow <see cref="HasErrors"/> to operate in O(1) time, as it can just
        /// check whether this value is not 0 instead of having to traverse <see cref="errors"/>.
        /// </summary>
        private int totalErrors;

        /// <inheritdoc/>
        public event EventHandler<DataErrorsChangedEventArgs>? ErrorsChanged;

        /// <inheritdoc/>
        public bool HasErrors => this.totalErrors > 0;

        /// <inheritdoc/>
        public IEnumerable GetErrors(string? propertyName)
        {
            if (!(propertyName is null) &&
                this.errors.TryGetValue(propertyName, out List<ValidationResult> errors))
            {
                return errors;
            }

            // The INotifyDataErrorInfo.GetErrors method doesn't specify exactly what to
            // return when the input property name is invalid, but given that the return
            // type is marked as a non-nullable reference type, here we're returning an
            // empty array to respect the contract. We do the same when the input property
            // is null as well, as we don't support entity-level errors at the moment.
            return Array.Empty<ValidationResult>();
        }

        /// <summary>
        /// Validates a property with a specified name and a given input value.
        /// </summary>
        /// <param name="value">The value to test for the specified property.</param>
        /// <param name="propertyName">The name of the property to validate.</param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="propertyName"/> is <see langword="null"/>.</exception>
        protected void ValidateProperty(object value, [CallerMemberName] string? propertyName = null)
        {
            if (propertyName is null)
            {
                ThrowArgumentNullExceptionForNullPropertyName();
            }

            bool errorsChanged = false;

            // Clear the errors for the specified property, if any
            if (this.errors.TryGetValue(propertyName!, out List<ValidationResult>? propertyErrors) &&
                propertyErrors.Count > 0)
            {
                propertyErrors.Clear();

                errorsChanged = true;
            }

            List<ValidationResult> results = new List<ValidationResult>();

            // Validate the property
            if (Validator.TryValidateProperty(
                value,
                new ValidationContext(this, null, null) { MemberName = propertyName },
                results))
            {
                if (errorsChanged)
                {
                    this.totalErrors--;
                }
            }
            else
            {
                if (!errorsChanged)
                {
                    this.totalErrors++;
                }

                // We can avoid the repeated dictionary lookup if we just check the result of the previous
                // one here. If the retrieved errors collection is null, we can log the ones produced by
                // the validation we just performed. We also don't need to create a new list and add them
                // to that, as we can directly set the resulting list as value in the mapping. If the
                // property already had some other logged errors instead, we can add the new ones as a range.
                if (propertyErrors is null)
                {
                    this.errors.Add(propertyName!, results);
                }
                else
                {
                    propertyErrors.AddRange(results);
                }

                errorsChanged = true;
            }

            // Only raise the event once if needed. This happens either when the target property
            // had existing errors and is now valid, or if the validation has failed and there are
            // new errors to broadcast, regardless of the previous validation state for the property.
            if (errorsChanged)
            {
                ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(propertyName));
            }
        }

        /// <summary>
        /// Throws an <see cref="ArgumentNullException"/> when a property name given as input is <see langword="null"/>.
        /// </summary>
        private static void ThrowArgumentNullExceptionForNullPropertyName()
        {
            throw new ArgumentNullException("propertyName", "The input property name cannot be null when validating a property");
        }
    }
}