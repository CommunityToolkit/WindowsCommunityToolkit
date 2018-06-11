// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Graph;
using Microsoft.Toolkit.Services.MicrosoftGraph;
using Microsoft.Toolkit.Uwp.UI.Extensions;
using Newtonsoft.Json;
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
                if (!string.Equals(control.PlanId, control.InternalPlanId))
                {
                    control.InternalPlanId = control.PlanId;
                }

                if (MicrosoftGraphService.Instance.IsAuthenticated)
                {
                    await control.InitPlanAsync().ConfigureAwait(false);
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

        private async void Add_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                MicrosoftGraphService graphService = MicrosoftGraphService.Instance;
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
                    _input.Text = string.Empty;
                    await graphClient.Planner.Tasks.Request().AddAsync(task);
                    await Task.Delay(TimeSpan.FromSeconds(5));
                    await InitPlanAsync();
                }
            }
            catch (Exception exception)
            {
                MessageDialog messageDialog = new MessageDialog(exception.Message);
                await messageDialog.ShowAsync();
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

                        task.Add("assignments", assignments);
                        break;
                    case nameof(plannerTaskViewModel.Title):
                        task.Add("title", plannerTaskViewModel.Title);
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
                        task.Add("bucketId", plannerTaskViewModel.BucketId);
                        break;
                    case nameof(plannerTaskViewModel.DueDateTime):
                        task.Add("dueDateTime", plannerTaskViewModel.DueDateTime);
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
                        MicrosoftGraphService graphService = MicrosoftGraphService.Instance;
                        await graphService.TryLoginAsync();
                        GraphServiceClient graphClient = graphService.GraphProvider;
                        PlannerTask taskToUpdate = await graphClient.Planner.Tasks[plannerTaskViewModel.Id].Request().GetAsync();
                        if (task.ContainsKey("assignments"))
                        {
                            PlannerAssignments assignments = task["assignments"] as PlannerAssignments;
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

        private async void List_Tapped(object sender, TappedRoutedEventArgs e)
        {
            if (e.OriginalSource is FrameworkElement sourceElement
                && sourceElement.FindAscendantByName("DeleteTask") is FrameworkElement removeButton
                && removeButton.Tag is PlannerTaskViewModel task)
            {
                try
                {
                    MicrosoftGraphService graphService = MicrosoftGraphService.Instance;
                    await graphService.TryLoginAsync();
                    GraphServiceClient graphClient = graphService.GraphProvider;
                    PlannerTask taskToUpdate = await graphClient.Planner.Tasks[task.Id].Request().GetAsync();
                    await graphClient.Planner.Tasks[task.Id].Request().Header("If-Match", taskToUpdate.GetEtag()).DeleteAsync();
                    Tasks.Remove(task);
                    await Task.Delay(TimeSpan.FromSeconds(5));
                    await InitPlanAsync();
                }
                catch (Exception exception)
                {
                    MessageDialog messageDialog = new MessageDialog(exception.Message);
                    await messageDialog.ShowAsync();
                }
            }
        }
    }
}
