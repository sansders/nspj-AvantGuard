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
using WpfApp1.Model1;
namespace WpfApp1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            CurrentPageModel currentClass = new CurrentPageModel();
            CurrentPageModel.setMainWindow(this);
            rootBox.Source = new Uri(@"\ProfilePages\ProfileCreationPage6.xaml", UriKind.RelativeOrAbsolute);
            
        }



        //private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        //{

        //}

        //private void MouseDownHandler(object sender, MouseButtonEventArgs e)
        //{
            


        //}

        //private void ToggleCheckOption(object sender, RoutedEventArgs e)
        //{
        //    var radioButton = sender as RadioButton;
        //    if (radioButton == null)
        //    {
        //        return;
        //    }
        //    else
        //    {
        //        String data = radioButton.Content as String;
        //        Console.WriteLine(data);
        //    }
        //}

    }
}
