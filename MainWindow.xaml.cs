using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using TaskManager.dialogs;

namespace TaskManager
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public static MySqlConnection conn = new MySqlConnection();    
        public static TaskData taskData = new TaskData();

        public static TreeView treeViewTasks = new TreeView();
        public static TaskDataClass currentItem;

        public MainWindow()
        {
            InitializeComponent();

            SqlHelper sqlHelper = new SqlHelper();
            sqlHelper.loadAllTasks(conn, taskData.tasks);

            treeViewTasks = tvTasks;
            tvTasks.ItemsSource = taskData.tasks;

        }

        private void newTaskButton_Click(object sender, RoutedEventArgs e)
        {
            CreateTaskDialog createTaskDialog = new CreateTaskDialog();
            createTaskDialog.ShowDialog();

            editButton.IsEnabled = false;
            removeButton.IsEnabled = false;
        }

        private void editButton_Click(object sender, RoutedEventArgs e)
        {
            EditTaskDialog editTaskDialog = new EditTaskDialog();
            editTaskDialog.ShowDialog();

            editButton.IsEnabled = false;
            removeButton.IsEnabled = false;
        }

        private void removeButton_Click(object sender, RoutedEventArgs e)
        {
            taskData.tasks.Remove(currentItem);

            string sql = "DELETE FROM " + User.name + " WHERE id = " + currentItem.id + ";";
            MySqlCommand cmd = new MySqlCommand(sql, conn);
            cmd.ExecuteNonQuery();

            tvTasks.Items.Refresh();

            editButton.IsEnabled = false;
            removeButton.IsEnabled = false;
        }

        private void tvTasks_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            if (e.NewValue != null)
            {
                removeButton.IsEnabled = true;
                editButton.IsEnabled = true;

                currentItem = (TaskDataClass)e.NewValue;
                notes.Text = currentItem.description;

                /*string sql = "SELECT * FROM " + User.name + " WHERE id = '" + currentItem.id + "'";
                MySqlCommand cmd = new MySqlCommand(sql, conn);
                using (MySqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {                   
                        string description = reader.GetString(reader.GetOrdinal("notes"));
                        notes.Text = description;
                    }
                } */  
            }
        }

        private void taskComplete(object sender, RoutedEventArgs e)
        {
            CheckBox cb = (CheckBox) sender;
            StackPanel stackPanel = (StackPanel) cb.Parent;

            TextBlock selectedTextBlock1 = (TextBlock) stackPanel.Children[0];
            string id = selectedTextBlock1.Text;

            TaskDataClass task = taskData.getTask(Int32.Parse(id));
            task.completed = true;

            new SqlHelper().updateTask(task);
        }

        private void taskNotComplete(object sender, RoutedEventArgs e)
        {
            CheckBox cb = (CheckBox)sender;
            StackPanel stackPanel = (StackPanel)cb.Parent;

            TextBlock selectedTextBlock1 = (TextBlock)stackPanel.Children[0];
            string id = selectedTextBlock1.Text;

            TaskDataClass task = taskData.getTask(Int32.Parse(id));
            task.completed = false;

            new SqlHelper().updateTask(task);
        }

        private void pinn(object sender, RoutedEventArgs e)
        {

            MenuItem item = (MenuItem)sender;
            ContextMenu contextMenu = (ContextMenu)item.Parent;
            StackPanel stackPanel = (StackPanel)contextMenu.PlacementTarget;
            TextBlock selectedTextBlock1 = (TextBlock)stackPanel.Children[0];
            string id = selectedTextBlock1.Text;

            TaskDataClass task = taskData.getTask(Int32.Parse(id));
            if (task.pinned)
            {
                task.pinned = false;
                task.pinVisible = Visibility.Hidden;

                new SqlHelper().updateTask(task);
                taskData.tasks.Remove(task);
                taskData.tasks.Add(task);

                treeViewTasks = tvTasks;
                tvTasks.ItemsSource = taskData.tasks;

                MainWindow.treeViewTasks.Items.Refresh();
            }
            else
            {
                task.pinned = true;
                task.pinVisible = Visibility.Visible;

                new SqlHelper().updateTask(task);
                taskData.tasks.Remove(task);

                List<TaskDataClass> tasksList = new List<TaskDataClass>();
                bool first = true;
                int i = 0;

                foreach (TaskDataClass t in taskData.tasks)
                {
                    if (!t.pinned && first)
                    {
                        tasksList.Add(task);
                        first = false;
                    }
                    tasksList.Add(t);
                    i++;
                }

                if (i == 0) tasksList.Add(task);

                taskData.tasks = tasksList;

                treeViewTasks = tvTasks;
                tvTasks.ItemsSource = taskData.tasks;

                MainWindow.treeViewTasks.Items.Refresh();

            }
        }

        private void button_LogOut(object sender, RoutedEventArgs e)
        {

            this.Hide();
            taskData.tasks.Clear();
            User.name = "";
            User.password = "";

            Login login = new Login();
            login.Show();
            this.Close();
        }
    }
}
