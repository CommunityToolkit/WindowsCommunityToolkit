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
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.Background;

namespace Microsoft.Toolkit.Uwp.Helpers
{
    /// <summary>
    /// This class provides static helper methods for background task.
    /// </summary>
    public static class BackgroundTaskHelper
    {
        /// <summary>
        /// Check if a background task has been registered with the application or not.
        /// </summary>
        /// <param name="backgroundTaskType">The type of the background task.</param>
        /// <returns>True/False indicating if a background task was registered or not</returns>
        public static bool IsBackgroundTaskRegistered(Type backgroundTaskType)
        {
            return BackgroundTaskRegistration.AllTasks.Any(t => t.Value.Name == backgroundTaskType.Name);
        }

        /// <summary>
        /// Register a background task for the application with conditions
        /// </summary>
        /// <param name="backgroundTaskType">The type of the background task</param>
        /// <param name="trigger">Trigger that indicate when the background task should be invoked</param>
        /// <param name="enforceConditions">Indicate if the background task should quit if condition is no longer valid</param>
        /// <param name="conditions">Optional conditions for the background task to run with</param>
        /// <returns>Background Task that was registered with the system</returns>
        public static async Task<BackgroundTaskRegistration> RegisterAsync(Type backgroundTaskType, IBackgroundTrigger trigger, bool enforceConditions = true, params IBackgroundCondition[] conditions)
        {
            // Check if the task is already registered.
            if (IsBackgroundTaskRegistered(backgroundTaskType) == true)
            {
                return GetBackgroundTask(backgroundTaskType) as BackgroundTaskRegistration;
            }

            // Verify access
            BackgroundAccessStatus taskRequired = await BackgroundExecutionManager.RequestAccessAsync();

            if (taskRequired == BackgroundAccessStatus.Denied)
            {
                throw new InvalidOperationException("Background access is denied!");
            }

            // Get details about the background task
            string backgroundTaskName = backgroundTaskType.Name;
            string backgroundTaskEntryPoint = backgroundTaskType.FullName;

            // build the background task
            BackgroundTaskBuilder builder = new BackgroundTaskBuilder();
            builder.Name = backgroundTaskName;
            builder.TaskEntryPoint = backgroundTaskEntryPoint;
            builder.CancelOnConditionLoss = enforceConditions;

            // add the conditions if we have them.
            conditions?.ToList().ForEach(builder.AddCondition);

            builder.SetTrigger(trigger);

            // Register it
            BackgroundTaskRegistration registered = builder.Register();

            return registered;
        }

        /// <summary>
        /// Unregister a background task
        /// </summary>
        /// <param name="backgroundTaskType">The type of the background task</param>
        public static void Unregister(Type backgroundTaskType)
        {
            if (IsBackgroundTaskRegistered(backgroundTaskType) == false)
            {
                return;
            }

            IBackgroundTaskRegistration toBeUnregister = BackgroundTaskRegistration.AllTasks.Where(t => t.Value.Name == backgroundTaskType.Name).Select(t => t).FirstOrDefault().Value;

            toBeUnregister?.Unregister(true);
        }

        /// <summary>
        /// Get the registered background task of the given type.
        /// </summary>
        /// <param name="backgroundTaskType">Type of the background task class</param>
        /// <returns>Background task if there is such background task registered. Otherwise, null</returns>
        public static IBackgroundTaskRegistration GetBackgroundTask(Type backgroundTaskType)
        {
            return BackgroundTaskRegistration.AllTasks.FirstOrDefault(t => t.Value.Name == backgroundTaskType.Name).Value;
        }
    }
}
