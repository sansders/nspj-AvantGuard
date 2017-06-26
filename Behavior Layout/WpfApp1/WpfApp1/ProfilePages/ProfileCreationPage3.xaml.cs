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
using WpfApp1.NavigationControls;

namespace WpfApp1.ProfilePages
{
    /// <summary>
    /// Interaction logic for ProfileCreationPage3.xaml
    /// </summary>
    public partial class ProfileCreationPage3 : Page
    {
        public ProfileCreationPage3()
        {
            InitializeComponent();
            //Set the Instance of the third page 
            CurrentPageModel.thirdPage = this;
            //Set the Instance of the third page contol 
            CurrentPageModel.thirdControl = page3Controls;
        }

        private void ToggleCheckOption(object sender, RoutedEventArgs e)
        {
            RadioButton button = sender as RadioButton;
            if (button.IsChecked == null)
            {
                Console.WriteLine("No option is checked");
            }
            else
            {
                Console.WriteLine(button.Content);
            }
        }

        private void NextPageHandler(object sender, MouseButtonEventArgs e)
        {
            CurrentPageModel currentClass = CurrentPageModel.getcurrentclass();
            currentClass._currentPage = "3";
            Page page4 = CurrentPageModel.fourthPage;
            if (page4 == null)
            {
                this.NavigationService.Navigate(new Uri(@"\ProfilePages\ProfileCreationPage4.xaml", UriKind.RelativeOrAbsolute));
            }
            else
            {
                this.NavigationService.Navigate(page4);
                WpfApp1.NavigationControls.NavigationControls fourthControl = (WpfApp1.NavigationControls.NavigationControls)CurrentPageModel.fourthControl;
                fourthControl.buttonManipulation(currentClass.currentpage);
                fourthControl.PageNumber.Text = fourthControl.currentPageNumber(currentClass.currentpage);
            }
            //Save the Instance of the thirdPage page//
            CurrentPageModel.thirdPage = this;
            //Save the Instance of the second page controls//
            CurrentPageModel.thirdControl = page3Controls;
        }

        private void PreviousPageHandler(object sender, MouseButtonEventArgs e)
        {
            CurrentPageModel currentClass = CurrentPageModel.getcurrentclass();
            currentClass._currentPage = "1";
            //Gets the Saved Instance of the first page and load it//
            Page page2 = CurrentPageModel.secondPage;
            if (page2 == null)
            { this.NavigationService.Navigate(new Uri(@"\ProfilePages\ProfileCreationPage2.xaml", UriKind.RelativeOrAbsolute)); }
            else
            {
                this.NavigationService.Navigate(page2);
                WpfApp1.NavigationControls.NavigationControls secondControl = (WpfApp1.NavigationControls.NavigationControls)CurrentPageModel.secondControl;
                secondControl.buttonManipulation(currentClass.currentpage);
                secondControl.PageNumber.Text = secondControl.currentPageNumber(currentClass.currentpage);
            }
            //Save the Instance of the thirdPage page//
            CurrentPageModel.thirdPage = this;
            //Save the Instance of the second page controls//
            CurrentPageModel.thirdControl = page3Controls;


        }
    }
}