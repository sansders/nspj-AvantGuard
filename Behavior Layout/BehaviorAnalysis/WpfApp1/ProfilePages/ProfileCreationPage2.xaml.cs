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
    /// Interaction logic for Page2.xaml
    /// </summary>
    public partial class Page2 : Page
    {
        
        public Page2()
        {
            InitializeComponent();
            CurrentPageModel.secondPage = this;
            CurrentPageModel.secondControl = page2Controls;
            CurrentPageModel.secondValidation = false;
        }

        private void Label_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {

            this.NavigationService.Navigate(new Uri(@"\ProfilePages\ProfileCreationPage1.xaml", UriKind.RelativeOrAbsolute));

        }

        private void ToggleCheckOption(object sender, RoutedEventArgs e)
        {
            RadioButton button = sender as RadioButton;
            if(button.IsChecked == null)
            {
                Console.WriteLine("No option is checked");
            }
            else
            {
                CurrentPageModel.secondValidation = true;
                Console.WriteLine(button.Content);
            }
        }

        private void NextPageHandler(object sender, MouseButtonEventArgs e)
        {
            Boolean isValidated = CurrentPageModel.secondValidation;
            if(isValidated == true)
            { 
                CurrentPageModel currentClass = CurrentPageModel.getcurrentclass();
                currentClass._currentPage = "2";
                Page page3 = CurrentPageModel.thirdPage;
            
                if (page3 == null)
                {
                    Page currentPage = new ProfileCreationPage3();
                    this.NavigationService.Navigate(currentPage);
                       }
                else
                {
                   this.NavigationService.Navigate(page3);
                   WpfApp1.NavigationControls.NavigationControls thirdControl = (WpfApp1.NavigationControls.NavigationControls)CurrentPageModel.thirdControl;
                   thirdControl.buttonManipulation(currentClass.currentpage);
                   thirdControl.PageNumber.Text = thirdControl.currentPageNumber(currentClass.currentpage);
                }
            }
            else
            {
                MessageBox.Show("No option have been chosen. Please choose your option");
            }
            //Save the Instance of the second page//
            CurrentPageModel.secondPage = this;
            //Save the Instance of the second page controls//
            CurrentPageModel.secondControl = page2Controls;
           
        }

        private void PreviousPageHandler(object sender, MouseButtonEventArgs e)
        {
            CurrentPageModel currentClass = CurrentPageModel.getcurrentclass();
            currentClass._currentPage = "0";
            //Gets the Saved Instance of the first page and load it//
            Page page1 = CurrentPageModel.firstPage;
            if (page1 == null)
            {
                Page currentPage = new Page1();
                this.NavigationService.Navigate(currentPage);
                this.NavigationService.Navigate(new Uri(@"\ProfilePages\ProfileCreationPage1.xaml", UriKind.RelativeOrAbsolute));
            }
            else
            {
                this.NavigationService.Navigate(page1);
                WpfApp1.NavigationControls.NavigationControls firstControl = (WpfApp1.NavigationControls.NavigationControls)CurrentPageModel.firstControl;
                firstControl.buttonManipulation(currentClass.currentpage);
                firstControl.PageNumber.Text = firstControl.currentPageNumber(currentClass.currentpage);
            }
            //Save the Instance of the second page//
            CurrentPageModel.secondPage = this;
            //Save the Instance of the second page controls//
            CurrentPageModel.secondControl = page2Controls;
        }

        
    }
}
