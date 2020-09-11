// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.Contracts;
using System.Linq;
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
        [Pure]
        public IEnumerable GetErrors(string? propertyName)
        {
            // Entity-level errors when the target property is null or empty
            if (string.IsNullOrEmpty(propertyName))
            {
                return this.GetAllErrors();
            }

            // Property-level errors, if any
            if (this.errors.TryGetValue(propertyName!, out List<ValidationResult> errors))
            {
                return errors;
            }

            // The INotifyDataErrorInfo.GetErrors method doesn't specify exactly what to
            // return when the input property name is invalid, but given that the return
            // type is marked as a non-nullable reference type, here we're returning an
            // empty array to respect the contract. This also matches the behavior of
            // this method whenever errors for a valid properties are retrieved.
            return Array.Empty<ValidationResult>();
        }

        /// <summary>
        /// Implements the logic for entity-level errors gathering for <see cref="GetErrors"/>.
        /// </summary>
        /// <returns>An <see cref="IEnumerable"/> instance with all the errors in <see cref="errors"/>.</returns>
        [Pure]
        [MethodImpl(MethodImplOptions.NoInlining)]
        private IEnumerable GetAllErrors()
        {
            return this.errors.Values.SelectMany(errors => errors);
        }

        /// <summary>
        /// Validates a property with a specified name and a given input value.
        /// </summary>
        /// <param name="value">The value to test for the specified property.</param>
        /// <param name="propertyName">The name of the property to validate.</param>
        /// <returns>Whether or not the validation was successful.</returns>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="propertyName"/> is <see langword="null"/>.</exception>
        protected bool ValidateProperty(object? value, [CallerMemberName] string? propertyName = null)
        {
            if (propertyName is null)
            {
                ThrowArgumentNullExceptionForNullPropertyName();
            }

            // Check if the property had already been previously validated, and if so retrieve
            // the reusable list of validation errors from the errors dictionary. This list is
            // used to add new validation errors below, if any are produced by the validator.
            // If the property isn't present in the dictionary, add it now to avoid allocations.
            if (!this.errors.TryGetValue(propertyName!, out List<ValidationResult>? propertyErrors))
            {
                propertyErrors = new List<ValidationResult>();

                this.errors.Add(propertyName!, propertyErrors);
            }

            bool errorsChanged = false;

            // Clear the errors for the specified property, if any
            if (propertyErrors.Count > 0)
            {
                propertyErrors.Clear();

                errorsChanged = true;
            }

            // Validate the property, by adding new errors to the existing list
            bool isValid = Validator.TryValidateProperty(
                value,
                new ValidationContext(this, null, null) { MemberName = propertyName },
                propertyErrors);

            // Update the state and/or the errors for the property
            if (isValid)
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

                errorsChanged = true;
            }

            // Only raise the event once if needed. This happens either when the target property
            // had existing errors and is now valid, or if the validation has failed and there are
            // new errors to broadcast, regardless of the previous validation state for the property.
            if (errorsChanged)
            {
                ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(propertyName));
            }

            return isValid;
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