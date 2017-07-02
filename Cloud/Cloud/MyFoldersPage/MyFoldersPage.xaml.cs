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

namespace Cloud.MyFoldersPage
{
    /// <summary>
    /// Interaction logic for MyFoldersPage.xaml
    /// </summary>
    public partial class MyFoldersPage : Page
    {
        public MyFoldersPage()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new Uri(@"..\..\SearchPage\SearchPage.xaml", UriKind.RelativeOrAbsolute));
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new Uri(@"..\..\RecentPage\RecentPage.xaml", UriKind.RelativeOrAbsolute));
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new Uri(@"..\..\SharedPage\SharedPage.xaml", UriKind.RelativeOrAbsolute));
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new Uri(@"..\..\FavoritesPage\FavoritesPage.xaml", UriKind.RelativeOrAbsolute));
        }

        private void Button_Click_4(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new Uri(@"..\..\BinPage\BinPage.xaml", UriKind.RelativeOrAbsolute));
        }

        private void Button_Click_5(object sender, RoutedEventArgs e)
        {
            if (this.NavigationService.CanGoBack)
            {
                this.NavigationService.GoBack();
            }
        }

        private void Button_Click_6(object sender, RoutedEventArgs e)
        {
            if (this.NavigationService.CanGoForward)
            {
                this.NavigationService.GoForward();
            }
        }
    }
}
