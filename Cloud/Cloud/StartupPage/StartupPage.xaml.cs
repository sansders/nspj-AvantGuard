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

namespace Cloud.StartupPage
{
    /// <summary>
    /// Interaction logic for StartupPage.xaml
    /// </summary>
    public partial class StartupPage : Page
    {
        private List<MyItem> newList = new List<MyItem>();

        public StartupPage()
        {
            InitializeComponent();
        }


        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            mainFrame.NavigationService.Navigate(new Uri(@"..\..\MyFoldersPage\MyFoldersPage.xaml", UriKind.RelativeOrAbsolute));
            MyFoldersButton.Background = (Brush)(new BrushConverter().ConvertFrom("#23aeff"));
            RecentButton.Background = (Brush)(new BrushConverter().ConvertFrom("#8c9199"));
            SharedButton.Background = (Brush)(new BrushConverter().ConvertFrom("#8c9199"));
            FavoritesButton.Background = (Brush)(new BrushConverter().ConvertFrom("#8c9199"));
            BinButton.Background = (Brush)(new BrushConverter().ConvertFrom("#8c9199"));
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            mainFrame.NavigationService.Navigate(new Uri(@"..\..\RecentPage\RecentPage.xaml", UriKind.RelativeOrAbsolute));
            MyFoldersButton.Background = (Brush)(new BrushConverter().ConvertFrom("#8c9199"));
            RecentButton.Background = (Brush)(new BrushConverter().ConvertFrom("#23aeff"));
            SharedButton.Background = (Brush)(new BrushConverter().ConvertFrom("#8c9199"));
            FavoritesButton.Background = (Brush)(new BrushConverter().ConvertFrom("#8c9199"));
            BinButton.Background = (Brush)(new BrushConverter().ConvertFrom("#8c9199"));
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            mainFrame.NavigationService.Navigate(new Uri(@"..\..\SharedPage\SharedPage.xaml", UriKind.RelativeOrAbsolute));
            MyFoldersButton.Background = (Brush)(new BrushConverter().ConvertFrom("#8c9199"));
            RecentButton.Background = (Brush)(new BrushConverter().ConvertFrom("#8c9199"));
            SharedButton.Background = (Brush)(new BrushConverter().ConvertFrom("#23aeff"));
            FavoritesButton.Background = (Brush)(new BrushConverter().ConvertFrom("#8c9199"));
            BinButton.Background = (Brush)(new BrushConverter().ConvertFrom("#8c9199"));
        }

        private void Button_Click_4(object sender, RoutedEventArgs e)
        {
            mainFrame.NavigationService.Navigate(new Uri(@"..\..\FavoritesPage\FavoritesPage.xaml", UriKind.RelativeOrAbsolute));
            MyFoldersButton.Background = (Brush)(new BrushConverter().ConvertFrom("#8c9199"));
            RecentButton.Background = (Brush)(new BrushConverter().ConvertFrom("#8c9199"));
            SharedButton.Background = (Brush)(new BrushConverter().ConvertFrom("#8c9199"));
            FavoritesButton.Background = (Brush)(new BrushConverter().ConvertFrom("#23aeff"));
            BinButton.Background = (Brush)(new BrushConverter().ConvertFrom("#8c9199"));
        }

        private void Button_Click_5(object sender, RoutedEventArgs e)
        {
            mainFrame.NavigationService.Navigate(new Uri(@"..\..\BinPage\BinPage.xaml", UriKind.RelativeOrAbsolute));
            MyFoldersButton.Background = (Brush)(new BrushConverter().ConvertFrom("#8c9199"));
            RecentButton.Background = (Brush)(new BrushConverter().ConvertFrom("#8c9199"));
            SharedButton.Background = (Brush)(new BrushConverter().ConvertFrom("#8c9199"));
            FavoritesButton.Background = (Brush)(new BrushConverter().ConvertFrom("#8c9199"));
            BinButton.Background = (Brush)(new BrushConverter().ConvertFrom("#23aeff"));
        }

        private void Button_Click_6(object sender, RoutedEventArgs e)
        {
            if (mainFrame.NavigationService.CanGoBack)
            {
                mainFrame.NavigationService.GoBack();
            }
        }

        private void Button_Click_7(object sender, RoutedEventArgs e)
        {
            if (mainFrame.NavigationService.CanGoForward)
            {
                mainFrame.NavigationService.GoForward(); 
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            mainFrame.NavigationService.Navigate(new Uri(@"..\..\TextEditor\Editor.xaml", UriKind.RelativeOrAbsolute));
        }
    }
}
