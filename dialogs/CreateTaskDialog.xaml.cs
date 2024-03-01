using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
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
    /// Interakční logika pro CreateTaskDialog.xaml
    /// </summary>
    public partial class CreateTaskDialog : Window
    {
        public CreateTaskDialog()
        {
            InitializeComponent();

            name.GotFocus += (sender, args) =>
            {
                name.Text = "";
                name.Foreground = Brushes.Black;
                name.HorizontalContentAlignment = HorizontalAlignment.Left;
            };

            description.GotFocus += (sender, args) =>
            {
                description.Text = "";
                description.Foreground = Brushes.Black;
                description.VerticalContentAlignment = VerticalAlignment.Top;
                description.HorizontalContentAlignment = HorizontalAlignment.Left;
            };

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            int id = MainWindow.taskData.getNewTaskID();
            string name = this.name.Text;
            string description = this.description.Text;
            DateTime completeDate = DateTime.Now;
            if (this.date.SelectedDate != null)
            {
                completeDate = this.date.SelectedDate.Value;
            }         
            string formatDate = completeDate.ToString("dd.MM.yyyy");
            DateTime created = DateTime.Now;

            new SqlHelper().insertTask(MainWindow.conn, id, 0, name, description, created, completeDate);
            MainWindow.taskData.addTask(id, false, name, description, completeDate, created, false);

            MainWindow.treeViewTasks.Items.Refresh();

            this.Close();
         

        }
    }
}
