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

namespace WpfApp1.ProfilePages
{
    /// <summary>
    /// Interaction logic for Page1.xaml
    /// </summary>
    public partial class Page1 : Page 
{
        public Page1()
        {
            InitializeComponent();
            CurrentPageModel.firstPage = this;
            CurrentPageModel.firstControl = page1Controls;
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void NextPageHandler(object sender, MouseButtonEventArgs e)
        {

            CurrentPageModel currentClass = CurrentPageModel.getcurrentclass();
            currentClass._currentPage = "1";
            //Load the Saved Instance of the second page//
            Page page2 = CurrentPageModel.secondPage;
            if(page2 == null)
            {  this.NavigationService.Navigate(new Uri(@"\ProfilePages\ProfileCreationPage2.xaml", UriKind.RelativeOrAbsolute));}
            else
            {
                this.NavigationService.Navigate(page2);
                WpfApp1.NavigationControls.NavigationControls secondControl = (WpfApp1.NavigationControls.NavigationControls)CurrentPageModel.secondControl;
                secondControl.buttonManipulation(currentClass.currentpage);
                secondControl.PageNumber.Text = secondControl.currentPageNumber(currentClass.currentpage);
            }
            //Save the Instance of the first page
            CurrentPageModel.firstPage = this;
            //Save the Instance of the first page controls
            CurrentPageModel.firstControl = page1Controls;



        }

        private void PreviousPageHandler(object sender, MouseButtonEventArgs e)
        {
            //Save the Instance of the first page
            CurrentPageModel.firstPage = this;
            //Save the Instance of the first page controls
            CurrentPageModel.firstControl = page1Controls;
            //Change
            // Navigate to Xiangjing profile page 
            // this.NavigationService.Navigate();
        }

        private void ToggleCheckOption(object sender, RoutedEventArgs e)
        {
            var radioButton = sender as RadioButton;
            if (radioButton == null)
            {
                return;
            }
            else
            {
                String data = radioButton.Content as String;
                Console.WriteLine(data);
            }
        }

    }
}
