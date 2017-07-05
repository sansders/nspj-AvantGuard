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
        private List<MyItem> newList = new List<MyItem>();

        public MyFoldersPage()
        {
            InitializeComponent();

            this.listView.Items.Add(new MyItem { Name = "Business Presentation", Owner = "WeiHan9898", LastModified = "27 May 2017", FileSize = "903MB" });
            this.listView.Items.Add(new MyItem { Name = "My Story", Owner = "SohJT", LastModified = "23 May 2017", FileSize = "742MB" });
            this.listView.Items.Add(new MyItem { Name = "MatthewHo.docx", Owner = "Grymb3l", LastModified = "9 May 2017", FileSize = "103MB" });
            this.listView.Items.Add(new MyItem { Name = "Chengdu PPT", Owner = "ShonTei", LastModified = "19 April 2017", FileSize = "1.4GB" });
            this.listView.Items.Add(new MyItem { Name = "My Presentation", Owner = "me", LastModified = "4 April 2017", FileSize = "78KB" });

            foreach (MyItem item in listView.Items)
            {
                newList.Add(item);
            }
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

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        { 
            if (!string.IsNullOrWhiteSpace(searchBar.Text))
            {
                listView.Items.Clear();

                for (int i = 0; i < newList.Count; i++)
                {
                    var item = newList[i];
                    if (item.Name.ToLower().Contains(searchBar.Text.ToLower()))
                    {
                        listView.Items.Add(item);
                    }
                }

            }

            else
            {
                listView.Items.Clear();

                for (int i = 0; i < newList.Count; i++)
                {
                    var item = newList[i];
                    listView.Items.Add(item);
                }
            }
        }

        private void TextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            TextBox tb = (TextBox)sender;
            tb.Text = string.Empty;
            tb.GotFocus -= TextBox_GotFocus;
        }
    }
}
