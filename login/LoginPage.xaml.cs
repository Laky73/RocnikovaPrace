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
    /// Interakční logika pro LoginPage.xaml
    /// </summary>
    public partial class LoginPage : Page
    {
        MySqlConnection conn = MainWindow.conn;

        public LoginPage()
        {
            InitializeComponent();
        }

        private void button_LogIn(object sender, RoutedEventArgs e)
        {
            string name = tb_name.Text;
            string password = tb_pass.Password;

            if (name == "" || password == "")
            {
                MessageBox.Show("Please fill in all fields");
                return;
            }

            string select = "SELECT * FROM users WHERE name = '" + name + "';";
            using (MySqlCommand command = new MySqlCommand(select, conn))
            {
                using (MySqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {

                        string dbPass = reader.GetString(reader.GetOrdinal("password"));
                        if (password == dbPass)
                        {
                            new User(name, password);
                            reader.Close();
                            Window.GetWindow(this).Hide();
                            MainWindow mainWindow = new MainWindow();
                            Application.Current.MainWindow = mainWindow;
                            mainWindow.Show();
                            Window.GetWindow(this).Close();                                      
                            return;
                        }
                    }
                }
            }

            MessageBox.Show("Incorrect password or username!");
        }
    }
}
