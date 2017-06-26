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
using WpfApp1.NavigationControls;
using WpfApp1.Model1;

namespace WpfApp1.ProfilePages
{
    /// <summary>
    /// Interaction logic for ProfileCreationPage5.xaml
    /// </summary>
    public partial class ProfileCreationPage5 : Page
    {
        public ProfileCreationPage5()
        {
            InitializeComponent();
            gameButton.Visibility = Visibility.Hidden;
            
            //Save current instance of the page
            CurrentPageModel.fifthPage = this;
            //Save current instance of the user control
            CurrentPageModel.fifthControl = page5Controls;

        }

        private void NextPageHandler(object sender, MouseButtonEventArgs e)
        {
            //Get the current instance of the navigation class
            CurrentPageModel currentClass = CurrentPageModel.getcurrentclass();
            currentClass.currentpage = "5";
            //Page page6 = CurrentPageModel.sixthPage;
            //if (page6 == null)
            //{
            //    this.NavigationService.Navigate(new Uri(@"\ProfilePages\ProfileCreationPage6.xaml", UriKind.RelativeOrAbsolute));
            //}
            //else
            //{
            //    //Load in the instance of the page 
            //    this.NavigationService.Navigate(page6);
            //    //Load in the current navigation control
            //    WpfApp1.NavigationControls.NavigationControls sixthControl = (WpfApp1.NavigationControls.NavigationControls)CurrentPageModel.sixthControl;
            //    //Set the button manipulation 
            //    sixthControl.buttonManipulation(currentClass.currentpage);
            //    //Set the page number 
            //    sixthControl.PageNumber.Text = sixthControl.currentPageNumber(currentClass.currentpage);

            //}

            //Save current instance of the page
            CurrentPageModel.fifthPage = this;
            //Save current instance of the user control
            CurrentPageModel.fifthControl = page5Controls;
        }

        private void PreviousPageHandler(object sender, MouseButtonEventArgs e)
        {
            //Get the current instance of the navigation class
            CurrentPageModel currentClass = CurrentPageModel.getcurrentclass();
            currentClass.currentpage = "3";
            Page page4 = CurrentPageModel.fourthPage;
            if (page4 == null)
            {
                this.NavigationService.Navigate(new Uri(@"\ProfilePages\ProfileCreationPage4.xaml", UriKind.RelativeOrAbsolute));
            }
            else
            {
                //Load in the instance of the page 
                this.NavigationService.Navigate(page4);
                //Load in the current navigation control
                WpfApp1.NavigationControls.NavigationControls fourthControl = (WpfApp1.NavigationControls.NavigationControls)CurrentPageModel.fourthControl;
                //Set the button manipulation 
                fourthControl.buttonManipulation(currentClass.currentpage);
                //Set the page number 
                fourthControl.PageNumber.Text = fourthControl.currentPageNumber(currentClass.currentpage);

            }
            //Save current instance of the page
            CurrentPageModel.fifthPage = this;
            //Save current instance of the user control
            CurrentPageModel.fifthControl = page5Controls;
        }

        private void gameStateControl_Click(object sender, RoutedEventArgs e)
        {
            instantiateGame(sender, e);
            
        }

        private void instantiateGame(object sender, RoutedEventArgs e)
        {
            gameButton.Visibility = Visibility.Visible;
            gameStateControl.Content = "Restart";
            gameStateControl.Click -= new RoutedEventHandler(gameStateControl_Click);
            gameStateControl.Click += new RoutedEventHandler(restartGame);
            Console.WriteLine("Game has started");

            gameButton.MouseEnter += onMouseEnter;
               
        }

        //Set the positions for the Circle//


        //Grid.SetRow(gameButton, 2);
        //Grid.SetColumn(gameButton, 5);

        private void onMouseEnter(object sender, MouseEventArgs e)
        {
            Console.WriteLine("Hello");
           

        }

        private void gameButton_Click(object sender, RoutedEventArgs e)
        {

        }

        void restartGame(object sender, RoutedEventArgs e)
        {
            gameButton.Visibility = Visibility.Hidden;
            gameStateControl.Content = "Start";
            gameStateControl.Click -= new RoutedEventHandler(restartGame);
            gameStateControl.Click += new RoutedEventHandler(gameStateControl_Click);
            Console.WriteLine("Game has restarted");

        }

      
    }


    

      


}

  
