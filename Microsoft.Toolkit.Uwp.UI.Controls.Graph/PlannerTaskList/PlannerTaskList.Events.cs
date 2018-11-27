// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using Microsoft.Graph;
using Microsoft.Toolkit.Services.MicrosoftGraph;
using Microsoft.Toolkit.Uwp.UI.Extensions;
using Newtonsoft.Json;
using Windows.System;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;

namespace Microsoft.Toolkit.Uwp.UI.Controls.Graph
{
    /// <summary>
    /// The PlannerTaskList Control displays a simple list of Planner tasks.
    /// </summary>
    public partial class PlannerTaskList
    {
        private static async void PlanIdPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is PlannerTaskList control
                && !string.IsNullOrWhiteSpace(control.PlanId))
            {
                control.CanAddTask = !string.IsNullOrWhiteSpace(control.PlanId);
                if (!string.Equals(control.PlanId, control.InternalPlanId))
                {
                    control.InternalPlanId = control.PlanId;
                }

                if (MicrosoftGraphService.Instance.IsAuthenticated)
                {
                    await control.InitPlanAsync().ConfigureAwait(false);
                }
                else
                {
                    MicrosoftGraphService.Instance.IsAuthenticatedChanged -= control.Instance_InitPlan;
                    MicrosoftGraphService.Instance.IsAuthenticatedChanged += control.Instance_InitPlan;
                }
            }
        }

        private static void InternalPlanIdPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is PlannerTaskList control
                && !string.IsNullOrWhiteSpace(control.InternalPlanId)
                && !string.Equals(control.PlanId, control.InternalPlanId))
            {
                control.PlanId = control.InternalPlanId;
            }
        }

        private static void TaskTypePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is PlannerTaskList control
                && !string.IsNullOrWhiteSpace(control.TaskType))
            {
                control.LoadTasks();
            }
        }

        private void Input_KeyUp(object sender, KeyRoutedEventArgs e)
        {
            if (e.Key == VirtualKey.Enter
                && CanAddTask)
            {
                Add_Click(sender, e);
            }
        }

        private async void Add_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                GraphServiceClient graphClient = await GraphServiceHelper.GetGraphServiceClientAsync();
                if (graphClient != null
                    && !string.IsNullOrWhiteSpace(_input?.Text))
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
                    _input.Text = string.Empty;
                    CanAddTask = false;
                    PlannerTask taskCreated = await graphClient.Planner.Tasks.Request().AddAsync(task);
                    PlannerPlan plan = Plans.FirstOrDefault(s => s.Id == InternalPlanId);
                    PlannerTaskViewModel taskViewModel = new PlannerTaskViewModel(taskCreated);
                    Dictionary<string, string> buckets = Buckets.ToDictionary(s => s.Id, s => s.Name);
                    if (plan != null)
                    {
                        taskViewModel.GroupId = plan.Owner;
                    }

                    if (!string.IsNullOrEmpty(taskViewModel.BucketId) && buckets.ContainsKey(taskViewModel.BucketId))
                    {
                        taskViewModel.BucketName = buckets[taskViewModel.BucketId];
                    }

                    if (taskCreated.PlanId == InternalPlanId)
                    {
                        taskViewModel.PropertyChanged += TaskViewModel_PropertyChanged;
                        _allTasks.Add(taskViewModel);
                        Tasks.Insert(0, taskViewModel);
                    }
                }
            }
            catch (Exception exception)
            {
                MessageDialog messageDialog = new MessageDialog(exception.Message);
                await messageDialog.ShowAsync();
            }

            CanAddTask = true;
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
            if (Tasks.Contains(plannerTaskViewModel))
            {
                Dictionary<string, object> task = new Dictionary<string, object>();
                bool skipUpdate = false;
                switch (e.PropertyName)
                {
                    case nameof(plannerTaskViewModel.AssignmentIds):
                        await GetAssignmentsAsync(plannerTaskViewModel);
                        PlannerAssignments assignments = new PlannerAssignments();

                        foreach (string assignee in plannerTaskViewModel.AssignmentIds)
                        {
                            assignments.AddAssignee(assignee);
                        }

                        task.Add(TaskAssignmentsJsonName, assignments);
                        break;
                    case nameof(plannerTaskViewModel.Title):
                        task.Add(TaskTitleJsonName, plannerTaskViewModel.Title);
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
                        task.Add(TaskBucketIdJsonName, plannerTaskViewModel.BucketId);
                        if (string.IsNullOrEmpty(plannerTaskViewModel.BucketId))
                        {
                            skipUpdate = true;
                        }

                        break;
                    case nameof(plannerTaskViewModel.DueDateTime):
                        task.Add(TaskDueDateTimeJsonName, plannerTaskViewModel.DueDateTime);
                        if (_list.ContainerFromItem(plannerTaskViewModel) is ListViewItem taskContainer)
                        {
                            var flyout = taskContainer.ContentTemplateRoot.FindDescendants<Button>().FirstOrDefault(s => s.Flyout != null)?.Flyout;
                            if (flyout != null)
                            {
                                flyout.Hide();
                            }
                        }

                        break;
                    default:
                        skipUpdate = true;
                        break;
                }

                if (!skipUpdate)
                {
                    plannerTaskViewModel.IsUpdating = true;
                    try
                    {
                        GraphServiceClient graphClient = await GraphServiceHelper.GetGraphServiceClientAsync();
                        if (graphClient != null)
                        {
                            PlannerTask taskToUpdate = await graphClient.Planner.Tasks[plannerTaskViewModel.Id].Request().GetAsync();
                            if (task.ContainsKey(TaskAssignmentsJsonName))
                            {
                                PlannerAssignments assignments = task[TaskAssignmentsJsonName] as PlannerAssignments;
                                string[] oldAssignees = taskToUpdate.Assignments.Assignees.ToArray();
                                string[] newAssignees = assignments.Assignees.ToArray();
                                foreach (string userId in oldAssignees)
                                {
                                    if (!newAssignees.Contains(userId))
                                    {
                                        assignments.AddAssignee(userId);
                                        assignments[userId] = null;
                                    }
                                }
                            }

                            plannerTaskViewModel.ETag = taskToUpdate.GetEtag();
                            using (HttpRequestMessage request = graphClient.Planner.Tasks[plannerTaskViewModel.Id].Request().GetHttpRequestMessage())
                            {
                                request.Method = new HttpMethod(HttpMethodPatch);
                                string json = JsonConvert.SerializeObject(task);
                                request.Content = new StringContent(json, System.Text.Encoding.UTF8, MediaTypeJson);
                                request.Headers.Add(HttpHeaderIfMatch, plannerTaskViewModel.ETag);
                                await graphClient.AuthenticationProvider.AuthenticateRequestAsync(request);
                                HttpResponseMessage response = await graphClient.HttpProvider.SendAsync(request);
                                response.Dispose();
                            }
                        }
                    }
                    catch (Exception exception)
                    {
                        MessageDialog messageDialog = new MessageDialog(exception.Message);
                        await messageDialog.ShowAsync();
                    }

                    plannerTaskViewModel.IsUpdating = false;
                }
            }
        }

        private async void Instance_IsAuthenticatedChanged(object sender, EventArgs e)
        {
            if (MicrosoftGraphService.Instance.IsAuthenticated)
            {
                await LoadPlansAsync();
            }
        }

        private async void Instance_InitPlan(object sender, EventArgs e)
        {
            if (MicrosoftGraphService.Instance.IsAuthenticated)
            {
                await InitPlanAsync();
            }
        }

        private async void List_Tapped(object sender, TappedRoutedEventArgs e)
        {
            if (e.OriginalSource is FrameworkElement sourceElement
                && sourceElement.FindAscendantByName(DeleteTaskButtonName) is FrameworkElement removeButton
                && removeButton.DataContext is PlannerTaskViewModel task)
            {
                await DeleteTaskAsync(task);
            }
        }

        private async void List_PreviewKeyUp(object sender, KeyRoutedEventArgs e)
        {
            if (e.Key == VirtualKey.Enter
            && e.OriginalSource is FrameworkElement removeButton
            && removeButton.Name == DeleteTaskButtonName
            && removeButton.DataContext is PlannerTaskViewModel task)
            {
                await DeleteTaskAsync(task);
            }
        }
    }
}
