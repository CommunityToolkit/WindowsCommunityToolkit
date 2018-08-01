// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using Microsoft.Graph;

namespace Microsoft.Toolkit.Uwp.UI.Controls.Graph
{
    internal class PlannerTaskViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private string _id;
        private string _title;
        private string _bucketId;
        private string _bucketName;
        private DateTimeOffset? _dueDateTime;
        private List<string> _assignmentIds;
        private ObservableCollection<Person> _assignments;
        private string _assignmentNames;
        private bool _isUpdating;
        private string _groupId;

        public string Id
        {
            get
            {
                return _id;
            }

            set
            {
                _id = value;
                TriggerPropertyChanged(nameof(Id));
            }
        }

        public string Title
        {
            get
            {
                return _title;
            }

            set
            {
                _title = value;
                TriggerPropertyChanged(nameof(Title));
            }
        }

        public string BucketId
        {
            get
            {
                return _bucketId;
            }

            set
            {
                _bucketId = value;
                TriggerPropertyChanged(nameof(BucketId));
            }
        }

        public string BucketName
        {
            get
            {
                return _bucketName;
            }

            set
            {
                _bucketName = value;
                TriggerPropertyChanged(nameof(BucketName));
            }
        }

        public DateTimeOffset? DueDateTime
        {
            get
            {
                return _dueDateTime;
            }

            set
            {
                _dueDateTime = value;
                TriggerPropertyChanged(nameof(DueDateTime));
                TriggerPropertyChanged(nameof(IsDueDateTimeVisible));
            }
        }

        public List<string> AssignmentIds
        {
            get
            {
                return _assignmentIds;
            }

            set
            {
                _assignmentIds = value;
                TriggerPropertyChanged(nameof(AssignmentIds));
            }
        }

        public ObservableCollection<Person> Assignments
        {
            get
            {
                return _assignments;
            }

            set
            {
                _assignments = value;
                TriggerPropertyChanged(nameof(Assignments));
            }
        }

        public string AssignmentNames
        {
            get
            {
                return _assignmentNames;
            }

            set
            {
                _assignmentNames = value;
                TriggerPropertyChanged(nameof(AssignmentNames));
                TriggerPropertyChanged(nameof(IsAssignmentNamesVisible));
            }
        }

        public int PercentComplete { get; internal set; }

        public DateTimeOffset? CreatedDateTime { get; internal set; }

        public string ETag { get; internal set; }

        public bool IsAssignmentNamesVisible
        {
            get
            {
                return !string.IsNullOrEmpty(AssignmentNames);
            }
        }

        public bool IsDueDateTimeVisible
        {
            get
            {
                return DueDateTime.HasValue;
            }
        }

        public bool IsUpdating
        {
            get
            {
                return _isUpdating;
            }

            set
            {
                _isUpdating = value;
                TriggerPropertyChanged(nameof(IsUpdating));
            }
        }

        public string GroupId
        {
            get
            {
                return _groupId;
            }

            set
            {
                _groupId = value;
                TriggerPropertyChanged(nameof(GroupId));
            }
        }

        public PlannerTaskViewModel(PlannerTask task)
        {
            Id = task.Id;
            Title = task.Title;
            BucketId = task.BucketId;
            DueDateTime = task.DueDateTime;
            AssignmentIds = task.Assignments?.Assignees.ToList();
            if (AssignmentIds == null)
            {
                AssignmentIds = new List<string>();
            }

            Assignments = new ObservableCollection<Person>(AssignmentIds.Select(s => new Person() { Id = s }));
            Assignments.CollectionChanged += Assignments_CollectionChanged;
            PercentComplete = task.PercentComplete ?? 0;
            CreatedDateTime = task.CreatedDateTime;
            ETag = task.GetEtag();
        }

        private void Assignments_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            AssignmentIds = Assignments.Select(s => s.Id).ToList();
        }

        private void TriggerPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged.Invoke(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
