using System;
using System.Net;
using System.Linq;
using System.Reactive.Linq;
using ReactiveUI;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace Silverlight.Common.Controls.ActionQueue
{
    public class ActionQueueViewModel : ReactiveObject
    {
        private ObservableCollection<Task> _Tasks;
        public ObservableCollection<Task> Tasks
        {
            get { return _Tasks ?? (_Tasks = new ObservableCollection<Task>()); }
            set { _Tasks = value; this.RaisePropertyChanged(r=> r._Tasks); }
        }
       
        private int _PendingTaskCount;
        public int PendingTaskCount
        {
            get { return _PendingTaskCount; }
            set { _PendingTaskCount = value; this.RaisePropertyChanged(r=> r.PendingTaskCount);}
        }

        private bool _PopupOpen;
        public bool PopupOpen
        {
            get { return _PopupOpen; }
            set { _PopupOpen = value; this.RaisePropertyChanged(r=> r.PopupOpen); }
        }

        private bool _IsWorking;
        public bool IsWorking
        {
            get { return _IsWorking; }
            set { _IsWorking = value; this.RaisePropertyChanged(r=> r.IsWorking); }
        }
        private int _PendingCount;
        public int PendingCount
        {
            get { return _PendingCount; }
            set { _PendingCount = value; this.RaisePropertyChanged(r=> r.PendingCount);}
        }
        

        public string Category { get; protected set; }
        

        public ActionQueueViewModel(string category)
        {
            if (DesignerProperties.IsInDesignTool)
                return;

            Category = category;
            MessageBus.Current.Listen<TaskStarted>().Where(t => t.Task.Category == Category).Subscribe(Task_Started);
        }

        public void Task_Started(TaskStarted taskStarted)
        {
            var task = taskStarted.Task;
            Tasks.Add(task);
            task.TaskCompleted += new EventHandler<TaskCompletedEventArgs>(task_TaskCompleted);

            IsWorking = Tasks.Any(t => t.Completed == null);
            PendingCount = Tasks.Count(t => t.Completed == null);
        }

        void task_TaskCompleted(object sender, TaskCompletedEventArgs e)
        {
            IsWorking = Tasks.Any(t => t.Completed == null);
            PendingCount = Tasks.Count(t => t.Completed == null);
        }

    }

    public interface Task : INotifyPropertyChanged
    {
        string Category { get; set; }

        string Name { get; set; }
        string Description { get; set; }

        DateTime Started { get; set; }
        DateTime? Completed { get; set; }

        Exception Exception { get; set; }

        event EventHandler<TaskCompletedEventArgs> TaskCompleted;
    }

    public class TaskCompletedEventArgs:EventArgs
    {
        public Task Task { get; protected set; }

        public TaskCompletedEventArgs(Task task)
        {
            Task = task;
        }
    }

    public class TaskStarted
    {
        public Task Task { get; set; }
    }

    public class TaskBase : ReactiveObject, Task
    {
        public event EventHandler<TaskCompletedEventArgs> TaskCompleted;

        private string _Category;
        public string Category
        {
            get { return _Category; }
            set { _Category = value; this.RaisePropertyChanged(t => t.Category); }
        }

        private string _Name;
        public string Name
        {
            get { return _Name; }
            set { _Name = value; this.RaisePropertyChanged(t => t.Name); }
        }

        private string _Description;
        public string Description
        {
            get { return _Description; }
            set { _Description = value; this.RaisePropertyChanged(t => t.Description); }
        }
        private DateTime _Started;
        public DateTime Started
        {
            get { return _Started; }
            set { _Started = value; this.RaisePropertyChanged(t => t.Started); }
        }

        private DateTime? _Completed;
        public DateTime? Completed
        {
            get { return _Completed; }
            set { _Completed = value; this.RaisePropertyChanged(t => t.Completed); }
        }

        private Exception _Exception;
        public Exception Exception
        {
            get { return _Exception; }
            set { _Exception = value; this.RaisePropertyChanged(t => t.Exception); }
        }


        protected void OnTaskCompleted(Exception exception)
        {
            Exception = exception;
            Completed = DateTime.Now;

            if (TaskCompleted != null)
                TaskCompleted(this, new TaskCompletedEventArgs(this));
        }

    }


}
