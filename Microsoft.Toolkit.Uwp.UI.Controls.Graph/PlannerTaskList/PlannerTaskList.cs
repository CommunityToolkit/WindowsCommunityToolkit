using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Microsoft.Graph;
using Microsoft.Toolkit.Services.MicrosoftGraph;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Microsoft.Toolkit.Uwp.UI.Controls.Graph
{
    /// <summary>
    /// The PlannerTaskList Control displays a simple list of Planner tasks.
    /// </summary>
    [TemplatePart(Name = ControlTasks, Type = typeof(ListView))]
    [TemplatePart(Name = ControlInput, Type = typeof(TextBox))]
    [TemplatePart(Name = ControlAdd, Type = typeof(Button))]
    [TemplateVisualState(GroupName = "MobileVisualStateGroup", Name = "Mobile")]
    public partial class PlannerTaskList : Control
    {
        private Dictionary<string, string> _userCache = new Dictionary<string, string>();
        private List<PlannerTaskViewModel> _allTasks = new List<PlannerTaskViewModel>();
        private TextBox _input;
        private ListView _list;
        private Button _add;

        /// <summary>
        /// Initializes a new instance of the <see cref="PlannerTaskList"/> class.
        /// </summary>
        public PlannerTaskList()
        {
            this.DefaultStyleKey = typeof(PlannerTaskList);
        }

        /// <inheritdoc/>
        protected async override void OnApplyTemplate()
        {
            if (_list != null)
            {
                _list.ItemClick -= List_ItemClick;
            }

            if (_add != null)
            {
                _add.Click -= Add_Click;
            }

            base.OnApplyTemplate();
            if (GetTemplateChild(ControlTasks) is ListView list)
            {
                _list = list;
                _list.ItemClick += List_ItemClick;
            }

            if (GetTemplateChild(ControlAdd) is Button add)
            {
                _add = add;
                _add.Click += Add_Click;
            }

            _input = GetTemplateChild(ControlInput) as TextBox;
            if (MicrosoftGraphService.Instance.IsAuthenticated)
            {
                await LoadPlansAsync();
            }
            else
            {
                MicrosoftGraphService.Instance.IsAuthenticatedChanged -= Instance_IsAuthenticatedChanged;
                MicrosoftGraphService.Instance.IsAuthenticatedChanged += Instance_IsAuthenticatedChanged;
            }

            if (IsWindowsPhone)
            {
                VisualStateManager.GoToState(this, "Mobile", false);
            }
        }

        private async Task LoadPlansAsync()
        {
            var graphService = MicrosoftGraphService.Instance;
            await graphService.TryLoginAsync();
            GraphServiceClient graphClient = graphService.GraphProvider;
            IPlannerUserPlansCollectionPage plans = await graphClient.Me.Planner.Plans.Request().GetAsync();
            Plans.Clear();
            while (true)
            {
                foreach (PlannerPlan plan in plans)
                {
                    Plans.Add(plan);
                }

                if (plans.NextPageRequest == null)
                {
                    break;
                }

                plans = await plans.NextPageRequest.GetAsync();
            }
        }

        private async Task InitPlanAsync()
        {
            var graphService = MicrosoftGraphService.Instance;
            await graphService.TryLoginAsync();
            GraphServiceClient graphClient = graphService.GraphProvider;
            TaskFilterSource.Clear();
            Buckets.Clear();
            TaskFilterSource.Add(new PlannerBucket { Id = TaskTypeAllTasksId, Name = "All" });
            IPlannerPlanBucketsCollectionPage buckets = await graphClient.Planner.Plans[PlanId].Buckets.Request().GetAsync();
            while (true)
            {
                foreach (PlannerBucket bucket in buckets)
                {
                    Buckets.Add(bucket);
                    TaskFilterSource.Add(bucket);
                }

                if (buckets.NextPageRequest == null)
                {
                    break;
                }

                buckets = await buckets.NextPageRequest.GetAsync();
            }

            TaskFilterSource.Add(new PlannerBucket { Id = TaskTypeClosedTasksId, Name = "Closed" });
            TaskType = TaskTypeAllTasksId;
            await LoadAllTasksAsync();
        }

        private async Task LoadAllTasksAsync()
        {
            var graphService = MicrosoftGraphService.Instance;
            await graphService.TryLoginAsync();
            GraphServiceClient graphClient = graphService.GraphProvider;
            _allTasks.Clear();
            Tasks.Clear();
            IPlannerPlanTasksCollectionPage tasks = await graphClient.Planner.Plans[PlanId].Tasks.Request().GetAsync();
            Dictionary<string, string> buckets = Buckets.ToDictionary(s => s.Id, s => s.Name);
            while (true)
            {
                foreach (PlannerTask task in tasks)
                {
                    PlannerTaskViewModel taskViewModel = new PlannerTaskViewModel(task);
                    taskViewModel.PropertyChanged += TaskViewModel_PropertyChanged;
                    await GetAssignmentsAsync(taskViewModel, graphClient);
                    if (!string.IsNullOrEmpty(taskViewModel.BucketId) && buckets.ContainsKey(taskViewModel.BucketId))
                    {
                        taskViewModel.BucketName = buckets[taskViewModel.BucketId];
                    }

                    _allTasks.Add(taskViewModel);
                }

                LoadTasks();

                if (tasks.NextPageRequest == null)
                {
                    break;
                }

                tasks = await tasks.NextPageRequest.GetAsync();
            }
        }

        private async Task GetAssignmentsAsync(PlannerTaskViewModel taskViewModel, GraphServiceClient graphClient = null)
        {
            if (graphClient == null)
            {
                var graphService = MicrosoftGraphService.Instance;
                await graphService.TryLoginAsync();
                graphClient = graphService.GraphProvider;
            }

            string assignments = string.Empty;
            foreach (string userId in taskViewModel.AssignmentIds)
            {
                if (!string.IsNullOrEmpty(userId))
                {
                    if (!_userCache.ContainsKey(userId))
                    {
                        User user = await graphClient.Users[userId].Request().GetAsync();
                        if (user != null)
                        {
                            _userCache.Add(user.Id, user.DisplayName);
                            assignments += AssigneeSeperator + user.DisplayName;
                        }
                    }
                    else
                    {
                        assignments += AssigneeSeperator + _userCache[userId];
                    }
                }
            }

            if (assignments.Length > AssigneeSeperator.Length)
            {
                assignments = assignments.Substring(2);
            }

            taskViewModel.AssignmentNames = assignments;
        }

        private bool IsTaskVisible(PlannerTaskViewModel task)
        {
            if (TaskType == TaskTypeAllTasksId)
            {
                return task.PercentComplete != 100;
            }
            else if (TaskType == TaskTypeClosedTasksId)
            {
                return task.PercentComplete == 100;
            }
            else
            {
                return task.BucketId == TaskType;
            }
        }

        private void LoadTasks()
        {
            Tasks.Clear();
            PlannerTaskViewModel[] tasks = _allTasks.Where(IsTaskVisible).OrderByDescending(s => s.CreatedDateTime).ToArray();
            foreach (PlannerTaskViewModel task in tasks)
            {
                Tasks.Add(task);
            }
        }
    }
}
