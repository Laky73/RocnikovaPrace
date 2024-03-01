using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace TaskManager.dialogs
{
    /// <summary>
    /// Interakční logika pro EditTaskDialog.xaml
    /// </summary>
    public partial class EditTaskDialog : Window
    {
        public EditTaskDialog()
        {
            InitializeComponent();

            TaskDataClass taskData = MainWindow.currentItem;
            
            name.Text = taskData.name;
            description.Text = taskData.description;
            date.DisplayDate = taskData.completeDate;

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            TaskDataClass taskData = MainWindow.currentItem;
            MySqlConnection conn = MainWindow.conn;

            string name = this.name.Text;
            DateTime completeDate = taskData.completeDate;
            if (this.date.SelectedDate != null)
            {
                completeDate = this.date.SelectedDate.Value;
            }
            string description = this.description.Text;

            taskData.name = name;
            taskData.completeDate = completeDate;
            taskData.description = description;

            string sql = "UPDATE " + User.name + " SET name = '" + name + "', notes = '" + description + "', completeDate = @completeDate WHERE id = " + taskData.id + ";";
            MySqlCommand cmd = new MySqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@completeDate", completeDate);
            cmd.ExecuteNonQuery();

            MainWindow.treeViewTasks.Items.Refresh();

            this.Close();

        }
    }
}
