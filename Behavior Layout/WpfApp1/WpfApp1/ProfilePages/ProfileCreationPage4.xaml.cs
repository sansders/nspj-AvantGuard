﻿using System;
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
    /// Interaction logic for ProfileCreationPage4.xaml
    /// </summary>
    public partial class ProfileCreationPage4 : Page
    {
        public ProfileCreationPage4()
        {
            InitializeComponent();
            CurrentPageModel.fourthPage = this;
            CurrentPageModel.fourthControl = page4Controls;
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
            //Get the current instance of the navigation class
            CurrentPageModel currentClass = CurrentPageModel.getcurrentclass();
            currentClass.currentpage = "4";
            Page page5 = CurrentPageModel.fifthPage;
            if (page5 == null)
            {
                this.NavigationService.Navigate(new Uri(@"\ProfilePages\ProfileCreationPage5.xaml", UriKind.RelativeOrAbsolute));
            }
            else
            {
                //Load in the instance of the page 
                this.NavigationService.Navigate(page5);
                //Load in the current navigation control
                WpfApp1.NavigationControls.NavigationControls fifthControl = (WpfApp1.NavigationControls.NavigationControls)CurrentPageModel.fifthControl;
                //Set the button manipulation 
                fifthControl.buttonManipulation(currentClass.currentpage);
                //Set the page number 
                fifthControl.PageNumber.Text = fifthControl.currentPageNumber(currentClass.currentpage);
              
            }
            //Save current instance of the page
            CurrentPageModel.fourthPage = this;
            //Save current instance of the user control
            CurrentPageModel.fourthControl = page4Controls;
        }

        private void PreviousPageHandler(object sender, MouseButtonEventArgs e)
        {
            //Get the current instance of the navigation class
            CurrentPageModel currentClass = CurrentPageModel.getcurrentclass();
            currentClass.currentpage = "2";
            Page page3 = CurrentPageModel.thirdPage;
            if (page3 == null)
            {
                this.NavigationService.Navigate(new Uri(@"\ProfilePages\ProfileCreationPage3.xaml", UriKind.RelativeOrAbsolute));
            }
            else
            {
                //Load in the instance of the page 
                this.NavigationService.Navigate(page3);
                //Load in the current navigation control
                WpfApp1.NavigationControls.NavigationControls thirdControl = (WpfApp1.NavigationControls.NavigationControls)CurrentPageModel.thirdControl;
                //Set the button manipulation 
                thirdControl.buttonManipulation(currentClass.currentpage);
                //Set the page number 
                thirdControl.PageNumber.Text = thirdControl.currentPageNumber(currentClass.currentpage);
            }
            //Save current instance of the page 
            CurrentPageModel.fourthPage = this;
            //Save current instance of the user control
            CurrentPageModel.fourthControl = page4Controls;
        }
    }
}
