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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace TaskManager.login
{
    /// <summary>
    /// Interakční logika pro SignupPage.xaml
    /// </summary>
    public partial class SignupPage : Page
    {
        MySqlConnection conn = MainWindow.conn;

        public SignupPage()
        {
            InitializeComponent();
        }

        private void button_SignUp(object sender, RoutedEventArgs e)
        {
            string name = tb_name.Text;
            string pass = tb_pass.Password;

            if (name == "" || pass == "")
            {
                MessageBox.Show("Please fill in all fields");
                return;
            }

            string select = "SELECT * FROM users WHERE name = '" + name+ "';";
            using (MySqlCommand command = new MySqlCommand(select, conn))
            {
                using (MySqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        MessageBox.Show("User already exists");
                        return;
                    }
                }
            }

            string createTable = "CREATE TABLE " + name + " (id INT PRIMARY KEY, completed INT, name VARCHAR(100), created DATE, completeDate DATE, notes TEXT, pinned INT);";
            MySqlCommand cmd2 = new MySqlCommand(createTable, conn);
            cmd2.ExecuteNonQuery();

            string insert = "INSERT INTO users (name, password) VALUES ('" + name + "', '" + pass + "');";
            MySqlCommand cmd3 = new MySqlCommand(insert, conn);
            cmd3.ExecuteNonQuery();

            new User(name, pass);
            Window.GetWindow(this).Hide();
            MainWindow mainWindow = new MainWindow();
            Application.Current.MainWindow = mainWindow;
            mainWindow.Show();
            Window.GetWindow(this).Close();


        }
    }
}
