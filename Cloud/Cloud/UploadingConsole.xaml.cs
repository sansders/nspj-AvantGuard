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

namespace Layout.Upload
{
    /// <summary>
    /// Interaction logic for Page2.xaml
    /// </summary>
    public partial class Page2 : Page
    {
        private bool isToggled = false;
        public Page2()
        {
            InitializeComponent();
        }

        private void ToggleButton_Checked(object sender, RoutedEventArgs e)
        {
          
            if (console.Visibility == Visibility.Hidden && isToggled == false)
            {

                Console.Write(toggle.IsChecked);
                console.Visibility = Visibility.Visible;
                isToggled = true;

            }

        }

        private void ToggleButton_unChecked(object sender, RoutedEventArgs e)
        {

            if (console.Visibility == Visibility.Visible && isToggled == true)
            {
                console.Visibility = Visibility.Hidden;
                Console.Write(toggle.IsChecked);
                isToggled = false;

            }

        }

        private void consoleClicked(object sender, EventArgs e)
        {
            if (console.Visibility == Visibility.Hidden && isToggled == false)
            {

                toggle.IsChecked = true;
                console.Visibility = Visibility.Visible;
                isToggled = true;

            }
            else if (console.Visibility == Visibility.Visible && isToggled == true)
            {
                console.Visibility = Visibility.Hidden;
                toggle.IsChecked = false;
                isToggled = false;

            }
        }


        private void Button_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new NavigationPage());

        }

       


    }
}

