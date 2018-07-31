// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Collections.ObjectModel;
using Microsoft.Graph;
using Windows.System.Profile;
using Windows.UI.Xaml;

namespace Microsoft.Toolkit.Uwp.UI.Controls.Graph
{
    /// <summary>
    /// The PlannerTaskList Control displays a simple list of Planner tasks.
    /// </summary>
    public partial class PlannerTaskList
    {
        /// <summary>
        /// Gets required delegated permissions for the <see cref="PlannerTaskList"/> control
        /// </summary>
        public static string[] RequiredDelegatedPermissions
        {
            get
            {
                return new string[] { "User.Read", "User.ReadBasic.All", "People.Read", "Group.ReadWrite.All" };
            }
        }

        /// <summary>
        /// Identifies the <see cref="PlanId"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty PlanIdProperty =
            DependencyProperty.Register(
                nameof(PlanId),
                typeof(string),
                typeof(PlannerTaskList),
                new PropertyMetadata(string.Empty, PlanIdPropertyChanged));

        /// <summary>
        /// Identifies the <see cref="DisplayBucketList"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty DisplayBucketListProperty =
            DependencyProperty.Register(
                nameof(DisplayBucketList),
                typeof(bool),
                typeof(PlannerTaskList),
                new PropertyMetadata(true));

        /// <summary>
        /// Identifies the <see cref="DisplayPlanList"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty DisplayPlanListProperty =
            DependencyProperty.Register(
                nameof(DisplayPlanList),
                typeof(bool),
                typeof(PlannerTaskList),
                new PropertyMetadata(true));

        /// <summary>
        /// Identifies the <see cref="AllTasksLabel"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty AllTasksLabelProperty =
            DependencyProperty.Register(
                nameof(AllTasksLabel),
                typeof(string),
                typeof(PlannerTaskList),
                new PropertyMetadata("All"));

        /// <summary>
        /// Identifies the <see cref="ClosedTasksLabel"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty ClosedTasksLabelProperty =
            DependencyProperty.Register(
                nameof(ClosedTasksLabel),
                typeof(string),
                typeof(PlannerTaskList),
                new PropertyMetadata("Closed"));

        /// <summary>
        /// Identifies the <see cref="DeleteConfirmDialogMessage"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty DeleteConfirmDialogMessageProperty =
            DependencyProperty.Register(
                nameof(DeleteConfirmDialogMessage),
                typeof(string),
                typeof(PlannerTaskList),
                new PropertyMetadata("Are you sure to delete this task?"));

        /// <summary>
        /// Identifies the <see cref="DeleteConfirmDialogYesLabel"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty DeleteConfirmDialogYesLabelProperty =
            DependencyProperty.Register(
                nameof(DeleteConfirmDialogYesLabel),
                typeof(string),
                typeof(PlannerTaskList),
                new PropertyMetadata("Yes"));

        /// <summary>
        /// Identifies the <see cref="DeleteConfirmDialogNoLabel"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty DeleteConfirmDialogNoLabelProperty =
            DependencyProperty.Register(
                nameof(DeleteConfirmDialogNoLabel),
                typeof(string),
                typeof(PlannerTaskList),
                new PropertyMetadata("No"));

        /// <summary>
        /// Identifies the <see cref="TaskType"/> dependency property.
        /// </summary>
        internal static readonly DependencyProperty TaskTypeProperty =
            DependencyProperty.Register(
                nameof(TaskType),
                typeof(string),
                typeof(PlannerTaskList),
                new PropertyMetadata(string.Empty, TaskTypePropertyChanged));

        /// <summary>
        /// Identifies the <see cref="Tasks"/> dependency property.
        /// </summary>
        internal static readonly DependencyProperty TasksProperty =
            DependencyProperty.Register(
                nameof(Tasks),
                typeof(ObservableCollection<PlannerTaskViewModel>),
                typeof(PlannerTaskList),
                new PropertyMetadata(new ObservableCollection<PlannerTaskViewModel>()));

        /// <summary>
        /// Identifies the <see cref="TaskFilterSource"/> dependency property.
        /// </summary>
        internal static readonly DependencyProperty TaskFilterSourceProperty =
            DependencyProperty.Register(
                nameof(TaskFilterSource),
                typeof(ObservableCollection<PlannerBucket>),
                typeof(PlannerTaskList),
                new PropertyMetadata(new ObservableCollection<PlannerBucket>()));

        /// <summary>
        /// Identifies the <see cref="Buckets"/> dependency property.
        /// </summary>
        internal static readonly DependencyProperty BucketsProperty =
            DependencyProperty.Register(
                nameof(Buckets),
                typeof(ObservableCollection<PlannerBucket>),
                typeof(PlannerTaskList),
                new PropertyMetadata(new ObservableCollection<PlannerBucket>()));

        /// <summary>
        /// Identifies the <see cref="Plans"/> dependency property.
        /// </summary>
        internal static readonly DependencyProperty PlansProperty =
            DependencyProperty.Register(
                nameof(Plans),
                typeof(ObservableCollection<PlannerPlan>),
                typeof(PlannerTaskList),
                new PropertyMetadata(new ObservableCollection<PlannerPlan>()));

        internal static readonly DependencyProperty InternalPlanIdProperty =
            DependencyProperty.Register(
                nameof(InternalPlanId),
                typeof(string),
                typeof(PlannerTaskList),
                new PropertyMetadata(string.Empty, InternalPlanIdPropertyChanged));

        internal static readonly DependencyProperty CanAddTaskProperty =
            DependencyProperty.Register(
                nameof(CanAddTask),
                typeof(bool),
                typeof(PlannerTaskList),
                new PropertyMetadata(false));

        /// <summary>
        /// Gets or sets Id of Planner Plan to Display, this is optional
        /// </summary>
        public string PlanId
        {
            get { return ((string)GetValue(PlanIdProperty))?.Trim(); }
            set { SetValue(PlanIdProperty, value?.Trim()); }
        }

        /// <summary>
        /// Gets or sets a value indicating whether show plan list or not
        /// </summary>
        public bool DisplayPlanList
        {
            get { return (bool)GetValue(DisplayPlanListProperty); }
            set { SetValue(DisplayPlanListProperty, value); }
        }

        /// <summary>
        /// Gets or sets a value indicating whether show bucket list or not
        /// </summary>
        public bool DisplayBucketList
        {
            get { return (bool)GetValue(DisplayBucketListProperty); }
            set { SetValue(DisplayBucketListProperty, value); }
        }

        /// <summary>
        /// Gets or sets label of all tasks
        /// </summary>
        public string AllTasksLabel
        {
            get { return (string)GetValue(AllTasksLabelProperty); }
            set { SetValue(AllTasksLabelProperty, value); }
        }

        /// <summary>
        /// Gets or sets message of delete confirm dialog
        /// </summary>
        public string DeleteConfirmDialogMessage
        {
            get { return (string)GetValue(DeleteConfirmDialogMessageProperty); }
            set { SetValue(DeleteConfirmDialogMessageProperty, value); }
        }

        /// <summary>
        /// Gets or sets label of Yes command of delete confirm dialog
        /// </summary>
        public string DeleteConfirmDialogYesLabel
        {
            get { return (string)GetValue(DeleteConfirmDialogYesLabelProperty); }
            set { SetValue(DeleteConfirmDialogYesLabelProperty, value); }
        }

        /// <summary>
        /// Gets or sets label of No command of delete confirm dialog
        /// </summary>
        public string DeleteConfirmDialogNoLabel
        {
            get { return (string)GetValue(DeleteConfirmDialogNoLabelProperty); }
            set { SetValue(DeleteConfirmDialogNoLabelProperty, value); }
        }

        /// <summary>
        /// Gets or sets label of closed tasks
        /// </summary>
        public string ClosedTasksLabel
        {
            get { return (string)GetValue(ClosedTasksLabelProperty); }
            set { SetValue(ClosedTasksLabelProperty, value); }
        }

        internal ObservableCollection<PlannerBucket> Buckets
        {
            get { return (ObservableCollection<PlannerBucket>)GetValue(BucketsProperty); }
            set { SetValue(BucketsProperty, value); }
        }

        internal ObservableCollection<PlannerBucket> TaskFilterSource
        {
            get { return (ObservableCollection<PlannerBucket>)GetValue(TaskFilterSourceProperty); }
            set { SetValue(TaskFilterSourceProperty, value); }
        }

        internal ObservableCollection<PlannerTaskViewModel> Tasks
        {
            get { return (ObservableCollection<PlannerTaskViewModel>)GetValue(TasksProperty); }
            set { SetValue(TasksProperty, value); }
        }

        internal string TaskType
        {
            get { return (string)GetValue(TaskTypeProperty); }
            set { SetValue(TaskTypeProperty, value); }
        }

        internal ObservableCollection<PlannerPlan> Plans
        {
            get { return (ObservableCollection<PlannerPlan>)GetValue(PlansProperty); }
            set { SetValue(PlansProperty, value); }
        }

        internal string InternalPlanId
        {
            get { return (string)GetValue(InternalPlanIdProperty); }
            set { SetValue(InternalPlanIdProperty, value); }
        }

        internal bool CanAddTask
        {
            get { return (bool)GetValue(CanAddTaskProperty); }
            private set { SetValue(CanAddTaskProperty, value); }
        }

        internal bool IsWindowsPhone
        {
            get
            {
                return AnalyticsInfo.VersionInfo.DeviceFamily == "Windows.Mobile";
            }
        }
    }
}
