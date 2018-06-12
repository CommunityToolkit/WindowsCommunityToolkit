// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

namespace Microsoft.Toolkit.Uwp.UI.Controls.Graph
{
    /// <summary>
    /// The PlannerTaskList Control displays a simple list of Planner tasks.
    /// </summary>
    public partial class PlannerTaskList
    {
        /// <summary>
        /// id to show all tasks
        /// </summary>
        private const string TaskTypeAllTasksId = "all";

        /// <summary>
        /// id to show closed tasks
        /// </summary>
        private const string TaskTypeClosedTasksId = "closed";

        /// <summary>
        /// HTTP method PATCH
        /// </summary>
        private const string HttpMethodPatch = "PATCH";

        /// <summary>
        /// mime-type application json
        /// </summary>
        private const string MediaTypeJson = "application/json";

        /// <summary>
        /// If-Match http header name
        /// </summary>
        private const string HttpHeaderIfMatch = "If-Match";

        /// <summary>
        /// Seperator of assignee list
        /// </summary>
        private const string AssigneeSeperator = ", ";

        /// <summary>
        /// name of ListView to display tasks
        /// </summary>
        private const string ControlTasks = "tasks";

        /// <summary>
        /// name of TextBox to input task title
        /// </summary>
        private const string ControlInput = "input";

        /// <summary>
        /// name of Button to add task
        /// </summary>
        private const string ControlAdd = "add";

        /// <summary>
        /// name of Visual State Group
        /// </summary>
        private const string MobileVisualStateGroup = "MobileVisualStateGroup";

        /// <summary>
        /// name of Visual State on mobile
        /// </summary>
        private const string MobileVisualState = "Mobile";

        /// <summary>
        /// id of Yes command in delete confirm dialog
        /// </summary>
        private const string DeleteConfirmDialogYes = "YES";

        /// <summary>
        /// id of Yes command in delete confirm dialog
        /// </summary>
        private const string DeleteConfirmDialogNo = "No";

        /// <summary>
        /// delete task button name
        /// </summary>
        private const string DeleteTaskButtonName = "DeleteTask";

        /// <summary>
        /// json name of assignments of task
        /// </summary>
        private const string TaskAssignmentsJsonName = "assignments";

        /// <summary>
        /// json name of title of task
        /// </summary>
        private const string TaskTitleJsonName = "title";

        /// <summary>
        /// json name of bucketId of task
        /// </summary>
        private const string TaskBucketIdJsonName = "bucketId";

        /// <summary>
        /// json name of dueDateTime of task
        /// </summary>
        private const string TaskDueDateTimeJsonName = "dueDateTime";

    }
}
