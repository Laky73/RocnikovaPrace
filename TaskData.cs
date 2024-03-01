using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Animation;

namespace TaskManager
{
    public class TaskData : INotifyPropertyChanged
    {
        public List<TaskDataClass> tasks = new List<TaskDataClass>();

        public event PropertyChangedEventHandler? PropertyChanged;

        public void addTask(int id, bool completed, string name, string description, DateTime completeDate, DateTime createDate, bool pinned)
        {
            tasks.Add(new TaskDataClass(id, completed, name, description, completeDate, createDate, pinned));
        }

        public TaskDataClass getTask(int id)
        {
            foreach (TaskDataClass task in tasks)
            {
                if (task.id == id) return task;
            }
            return null;
        }

        public void removeTask(int id)
        {
            tasks.RemoveAt(id);
        }

        public int getNewTaskID()
        {
            return tasks.Count;
        }


    }

    public class TaskDataClass : INotifyPropertyChanged
    {
        public int id { get; set; }
        public bool completed { get; set; }
        public bool pinned { get; set; }
        public string name { get; set; }
        public string description { get; set; }
        public DateTime completeDate { get; set; }
        public DateTime createDate { get; set; }
        public string completeDateString { get; set; }
        public string createDateString { get; set; }
        public Visibility pinVisible { get; set; }

        public TaskDataClass(int taskId, bool taskCompleted, string taskName, string taskDescription, DateTime taskCompleteDate, DateTime createDate, bool pinned)
        {
            id = taskId;
            completed = taskCompleted;
            name = taskName;
            description = taskDescription;
            completeDate = taskCompleteDate;
            this.createDate = createDate;
            completeDateString = completeDate.ToString("dd.MM.yyyy");
            createDateString = createDate.ToString("dd.MM.yyyy");
            this.pinned = pinned;
            
            if (pinned) pinVisible = Visibility.Visible;
            else pinVisible = Visibility.Hidden;
        }

        public event PropertyChangedEventHandler? PropertyChanged;

       
    }
}
