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
    /// Interakční logika pro Welcome.xaml
    /// </summary>
    public partial class Welcome : Page
    {
        public Welcome()
        {
            InitializeComponent();
        }

        private void button_LogIn(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("../login/LoginPage.xaml", UriKind.Relative));
        }
        private void button_SignUp(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("../login/SignupPage.xaml", UriKind.Relative));
        }
    }
}
