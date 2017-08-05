using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
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
using WpfApp1;
using WpfApp1.NavigationControls;
namespace NSPJProject
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            //this.AddChild(new WpfApp1.ProfilePages.Page1());
            rootBox.Source = new Uri(@"LoginPage.xaml", UriKind.RelativeOrAbsolute);
            //CurrentPageModel currentModel = new CurrentPageModel();
            //Page currentPage = new WpfApp1.ProfilePages.Page1();
            //rootBox.NavigationService.Navigate(currentPage);
        }

        
    }
}
