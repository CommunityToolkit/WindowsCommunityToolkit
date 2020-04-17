// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

#pragma warning disable SA1512

// Original file header:
// ****************************************************************************
// <copyright file="ObservableObject.cs" company="GalaSoft Laurent Bugnion">
// Copyright © GalaSoft Laurent Bugnion 2011-2016
// </copyright>
// ****************************************************************************
// <author>Laurent Bugnion</author>
// <email>laurent@galasoft.ch</email>
// <date>10.4.2011</date>
// <project>GalaSoft.MvvmLight.Messaging</project>
// <web>http://www.mvvmlight.net</web>
// <license>
// See license.txt in this project or http://www.galasoft.ch/license_MIT.txt
// </license>
// ****************************************************************************

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace Microsoft.Toolkit.Mvvm.ComponentModel
{
    /// <summary>
    /// A base class for objects of which the properties must be observable.
    /// </summary>
    public abstract class ObservableObject : INotifyPropertyChanged, INotifyPropertyChanging
    {
        /// <inheritdoc cref="INotifyPropertyChanged.PropertyChanged"/>
        public event PropertyChangedEventHandler? PropertyChanged;

        /// <inheritdoc cref="INotifyPropertyChanging.PropertyChanging"/>
        public event PropertyChangingEventHandler? PropertyChanging;

        /// <summary>
        /// Raises the <see cref="PropertyChanged"/> event if needed.
        /// </summary>
        /// <param name="propertyName">(optional) The name of the property that changed.</param>
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null!)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        /// <summary>
        /// Raises the <see cref="PropertyChanging"/> event if needed.
        /// </summary>
        /// <param name="propertyName">(optional) The name of the property that changed.</param>
        protected virtual void OnPropertyChanging([CallerMemberName] string propertyName = null!)
        {
            this.PropertyChanging?.Invoke(this, new PropertyChangingEventArgs(propertyName));
        }

        /// <summary>
        /// Compares the current and new values for a given property. If the value has changed,
        /// raises the <see cref="PropertyChanging"/> event, updates the property with the new
        /// value, then raises the <see cref="PropertyChanged"/> event.
        /// </summary>
        /// <typeparam name="T">The type of the property that changed.</typeparam>
        /// <param name="field">The field storing the property's value.</param>
        /// <param name="newValue">The property's value after the change occurred.</param>
        /// <param name="propertyName">(optional) The name of the property that changed.</param>
        /// <returns><see langword="true"/> if the property was changed, <see langword="false"/> otherwise.</returns>
        /// <remarks>
        /// The <see cref="PropertyChanging"/> and <see cref="PropertyChanged"/> events are not raised
        /// if the current and new value for the target property are the same.
        /// </remarks>
        protected bool Set<T>(ref T field, T newValue, [CallerMemberName] string propertyName = null!)
        {
            if (EqualityComparer<T>.Default.Equals(field, newValue))
            {
                return false;
            }

            this.OnPropertyChanging(propertyName);

            field = newValue;

            this.OnPropertyChanged(propertyName);

            return true;
        }

        /// <summary>
        /// Compares the current and new values for a given field (which should be the backing
        /// field for a property). If the value has changed, raises the <see cref="PropertyChanging"/>
        /// event, updates the field and then raises the <see cref="PropertyChanged"/> event.
        /// The behavior mirrors that of <see cref="Set{T}"/>, with the difference being that this method
        /// will also monitor the new value of the property (a generic <see cref="Task"/>) and will also
        /// raise the <see cref="PropertyChanged"/> again for the target property when it completes.
        /// This can be used to update bindings observing that <see cref="Task"/> or any of its properties.
        /// Here is a sample property declaration using this method:
        /// <code>
        /// private Task myTask;
        ///
        /// public Task MyTask
        /// {
        ///     get => myTask;
        ///     private set => SetAndNotifyOnCompletion(ref myTask, () => myTask, value);
        /// }
        /// </code>
        /// </summary>
        /// <typeparam name="TTask">The type of <see cref="Task"/> to set and monitor.</typeparam>
        /// <param name="field">The field storing the property's value.</param>
        /// <param name="fieldExpression">
        /// An <see cref="Expression{TDelegate}"/> returning the field to update. This is needed to be
        /// able to raise the <see cref="PropertyChanged"/> to notify the completion of the input task.
        /// </param>
        /// <param name="newValue">The property's value after the change occurred.</param>
        /// <param name="propertyName">(optional) The name of the property that changed.</param>
        /// <returns><see langword="true"/> if the property was changed, <see langword="false"/> otherwise.</returns>
        /// <remarks>
        /// The <see cref="PropertyChanging"/> and <see cref="PropertyChanged"/> events are not raised
        /// if the current and new value for the target property are the same.
        /// </remarks>
        protected bool SetAndNotifyOnCompletion<TTask>(ref TTask? field, Expression<Func<TTask?>> fieldExpression, TTask? newValue, [CallerMemberName] string propertyName = null!)
            where TTask : Task
        {
            if (ReferenceEquals(field, newValue))
            {
                return false;
            }

            /* Check the status of the new task before assigning it to the
             * target field. This is so that in case the task is either
             * null or already completed, we can avoid the overhead of
             * scheduling the method to monitor its completion. */
            bool isAlreadyCompletedOrNull = newValue?.IsCompleted ?? true;

            this.OnPropertyChanging(propertyName);

            field = newValue;

            OnPropertyChanged(propertyName);

            if (isAlreadyCompletedOrNull)
            {
                return true;
            }

            /* Get the target field to set. This is needed because we can't
             * capture the ref field in a closure (for the async method). */
            if (!((fieldExpression.Body as MemberExpression)?.Member is FieldInfo fieldInfo))
            {
                ThrowArgumentExceptionForInvalidFieldExpression();

                // This is never executed, as the method above always throws
                return false;
            }

            /* We use a local async function here so that the main method can
             * remain synchronous and return a value that can be immediately
             * used by the caller. This mirrors Set<T>(ref T, T, string).
             * We use an async void function instead of a Task-returning function
             * so that if a binding update caused by the property change notification
             * causes a crash, it is immediately reported in the application instead of
             * the exception being ignored (as the returned task wouldn't be awaited),
             * which would result in a confusing behavior for users. */
            async void MonitorTask()
            {
                try
                {
                    // Await the task and ignore any exceptions
                    await newValue!;
                }
                catch
                {
                }

                TTask? currentTask = (TTask?)fieldInfo.GetValue(this);

                // Only notify if the property hasn't changed
                if (ReferenceEquals(newValue, currentTask))
                {
                    OnPropertyChanged(propertyName);
                }
            }

            MonitorTask();

            return true;
        }

        /// <summary>
        /// Throws an <see cref="ArgumentException"/> when a given <see cref="Expression{TDelegate}"/> is invalid for a property field.
        /// </summary>
        [MethodImpl(MethodImplOptions.NoInlining)]
        private static void ThrowArgumentExceptionForInvalidFieldExpression()
        {
            throw new ArgumentException("The given expression must be in the form () => field");
        }
    }
}