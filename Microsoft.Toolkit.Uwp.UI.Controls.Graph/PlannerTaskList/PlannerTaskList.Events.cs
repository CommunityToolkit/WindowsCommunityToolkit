using System;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Graph;
using Microsoft.Toolkit.Services.MicrosoftGraph;
using Newtonsoft.Json;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Microsoft.Toolkit.Uwp.UI.Controls.Graph
{
    /// <summary>
    /// The PlannerTaskList Control displays a simple list of Planner tasks.
    /// </summary>
    public partial class PlannerTaskList
    {
        private static async void PlanIdPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            PlannerTaskList control = d as PlannerTaskList;
            if (!string.IsNullOrWhiteSpace(control.PlanId))
            {
                if (MicrosoftGraphService.Instance.IsAuthenticated)
                {
                    await control.InitPlanAsync().ConfigureAwait(false);
                }
                else
                {
                    MicrosoftGraphService.Instance.IsAuthenticatedChanged += async (a, b) =>
                    {
                        await control.InitPlanAsync().ConfigureAwait(false);
                    };
                }
            }
        }

        private static void TaskTypePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            PlannerTaskList control = d as PlannerTaskList;
            if (!string.IsNullOrWhiteSpace(control.TaskType))
            {
                control.LoadTasks();
            }
        }

        private async void Add_Click(object sender, RoutedEventArgs e)
        {
            var graphService = MicrosoftGraphService.Instance;
            await graphService.TryLoginAsync();
            GraphServiceClient graphClient = graphService.GraphProvider;
            if (!string.IsNullOrWhiteSpace(_input.Text))
            {
                PlannerTask task = new PlannerTask
                {
                    Title = _input.Text
                };
                if (TaskType != TaskTypeAllTasksId && TaskType != TaskTypeClosedTasksId)
                {
                    task.BucketId = TaskType;
                }

                task.PlanId = PlanId;
                await graphClient.Planner.Tasks.Request().AddAsync(task);
                await Task.Delay(TimeSpan.FromSeconds(5));
                await InitPlanAsync();
            }
        }

        private void List_ItemClick(object sender, ItemClickEventArgs e)
        {
            ListView list = sender as ListView;
            foreach (object item in list.Items)
            {
                if (list.ContainerFromItem(item) is ListViewItem itemContainer)
                {
                    DataTemplate template = itemContainer.ContentTemplateSelector.SelectTemplate(e.ClickedItem == item, itemContainer);
                    itemContainer.ContentTemplate = template;
                }
            }
        }

        private async void TaskViewModel_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            PlannerTaskViewModel plannerTaskViewModel = sender as PlannerTaskViewModel;
            PlannerTask task = new PlannerTask();
            bool skipUpdate = false;
            switch (e.PropertyName)
            {
                case nameof(plannerTaskViewModel.AssignmentIds):
                    await GetAssignmentsAsync(plannerTaskViewModel);
                    if (task.Assignments == null)
                    {
                        task.Assignments = new PlannerAssignments();
                    }

                    foreach (string assignee in plannerTaskViewModel.AssignmentIds)
                    {
                        task.Assignments.AddAssignee(assignee);
                    }

                    break;
                case nameof(plannerTaskViewModel.Title):
                    task.Title = plannerTaskViewModel.Title;
                    break;
                case nameof(plannerTaskViewModel.BucketId):
                    string bucketName = string.Empty;
                    foreach (PlannerBucket bucket in Buckets)
                    {
                        if (bucket.Id == plannerTaskViewModel.BucketId)
                        {
                            bucketName = bucket.Name;
                            break;
                        }
                    }

                    plannerTaskViewModel.BucketName = bucketName;
                    task.BucketId = plannerTaskViewModel.BucketId;
                    break;
                case nameof(plannerTaskViewModel.DueDateTime):
                    task.DueDateTime = plannerTaskViewModel.DueDateTime;
                    break;
                default:
                    skipUpdate = true;
                    break;
            }

            if (!skipUpdate)
            {
                var graphService = MicrosoftGraphService.Instance;
                await graphService.TryLoginAsync();
                GraphServiceClient graphClient = graphService.GraphProvider;
                using (HttpRequestMessage request = graphClient.Planner.Tasks[plannerTaskViewModel.Id].Request().GetHttpRequestMessage())
                {
                    request.Method = new HttpMethod(HttpMethodPatch);
                    string json = JsonConvert.SerializeObject(task, new JsonSerializerSettings() { NullValueHandling = NullValueHandling.Ignore });
                    request.Content = new StringContent(json, System.Text.Encoding.UTF8, MediaTypeJson);
                    request.Headers.Add(HttpHeaderIfMatch, plannerTaskViewModel.ETag);
                    await graphClient.AuthenticationProvider.AuthenticateRequestAsync(request);
                    HttpResponseMessage response = await graphClient.HttpProvider.SendAsync(request);
                    response.Dispose();
                }

                PlannerTask newTask = await graphClient.Planner.Tasks[plannerTaskViewModel.Id].Request().GetAsync();
                plannerTaskViewModel.ETag = newTask.GetEtag();
            }
        }
    }
}
