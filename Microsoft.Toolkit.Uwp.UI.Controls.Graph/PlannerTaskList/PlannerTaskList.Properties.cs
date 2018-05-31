using System;
using System.Collections.ObjectModel;
using Microsoft.Graph;
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
        /// Id of Planner Plan to Display
        /// </summary>
        public static readonly DependencyProperty PlanIdProperty =
            DependencyProperty.Register(
                nameof(PlanId),
                typeof(string),
                typeof(PlannerTaskList),
                new PropertyMetadata(string.Empty, PlanIdPropertyChanged));

        /// <summary>
        /// Show bucket list or not
        /// </summary>
        public static readonly DependencyProperty DisplayBucketListProperty =
            DependencyProperty.Register(
                nameof(DisplayBucketList),
                typeof(bool),
                typeof(PlannerTaskList),
                new PropertyMetadata(true));

        /// <summary>
        /// Show bucket list or not
        /// </summary>
        public static readonly DependencyProperty DisplayPlanListProperty =
            DependencyProperty.Register(
                nameof(DisplayPlanList),
                typeof(bool),
                typeof(PlannerTaskList),
                new PropertyMetadata(true));

        /// <summary>
        /// Label of all tasks
        /// </summary>
        public static readonly DependencyProperty AllTasksLabelProperty =
            DependencyProperty.Register(
                nameof(AllTasksLabel),
                typeof(string),
                typeof(PlannerTaskList),
                new PropertyMetadata("All"));

        /// <summary>
        /// Label of closed tasks
        /// </summary>
        public static readonly DependencyProperty ClosedTasksLabelProperty =
            DependencyProperty.Register(
                nameof(ClosedTasksLabel),
                typeof(string),
                typeof(PlannerTaskList),
                new PropertyMetadata("Closed"));

        /// <summary>
        /// Type of tasks to display
        /// </summary>
        internal static readonly DependencyProperty TaskTypeProperty =
            DependencyProperty.Register(
                nameof(TaskType),
                typeof(string),
                typeof(PlannerTaskList),
                new PropertyMetadata(string.Empty, TaskTypePropertyChanged));

        /// <summary>
        /// Default date
        /// </summary>
        internal static readonly DependencyProperty TodayProperty =
            DependencyProperty.Register(
                nameof(Today),
                typeof(DateTimeOffset),
                typeof(PlannerTaskList),
                new PropertyMetadata(DateTimeOffset.Now));

        /// <summary>
        /// Tasks to display in list
        /// </summary>
        internal static readonly DependencyProperty TasksProperty =
            DependencyProperty.Register(
                nameof(Tasks),
                typeof(ObservableCollection<PlannerTaskViewModel>),
                typeof(PlannerTaskList),
                new PropertyMetadata(new ObservableCollection<PlannerTaskViewModel>()));

        /// <summary>
        /// All types of tasks
        /// </summary>
        internal static readonly DependencyProperty TaskFilterSourceProperty =
            DependencyProperty.Register(
                nameof(TaskFilterSource),
                typeof(ObservableCollection<PlannerBucket>),
                typeof(PlannerTaskList),
                new PropertyMetadata(new ObservableCollection<PlannerBucket>()));

        /// <summary>
        /// All buckets in current plan
        /// </summary>
        internal static readonly DependencyProperty BucketsProperty =
            DependencyProperty.Register(
                nameof(Buckets),
                typeof(ObservableCollection<PlannerBucket>),
                typeof(PlannerTaskList),
                new PropertyMetadata(new ObservableCollection<PlannerBucket>()));

        /// <summary>
        /// All plans of current user
        /// </summary>
        internal static readonly DependencyProperty PlansProperty =
            DependencyProperty.Register(
                nameof(Plans),
                typeof(ObservableCollection<PlannerPlan>),
                typeof(PlannerTaskList),
                new PropertyMetadata(new ObservableCollection<PlannerPlan>()));

        /// <summary>
        /// Gets or sets Id of Planner Plan to Display
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

        internal DateTimeOffset Today
        {
            get { return (DateTimeOffset)GetValue(TodayProperty); }
            set { SetValue(TodayProperty, value); }
        }

        internal ObservableCollection<PlannerPlan> Plans
        {
            get { return (ObservableCollection<PlannerPlan>)GetValue(PlansProperty); }
            set { SetValue(PlansProperty, value); }
        }
    }
}
