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
using System.Collections;

namespace WpfApp1.ProfilePages
{
    /// <summary>
    /// Interaction logic for ProfileCreationPage5.xaml
    /// </summary>
    public partial class ProfileCreationPage5 : Page
    {
        Boolean running = false;
        Boolean isClicked = false;
        int currentIndex = 1; 
        public ProfileCreationPage5()
        {
            InitializeComponent();
            gameButton.Visibility = Visibility.Hidden;
            
            //Save current instance of the page
            CurrentPageModel.fifthPage = this;
            //Save current instance of the user control
            CurrentPageModel.fifthControl = page5Controls;
            CurrentPageModel.fifthhValidation = false;
        }

        private void NextPageHandler(object sender, MouseButtonEventArgs e)
        {
            //Get the current instance of the navigation class
            Boolean isValidated = CurrentPageModel.fifthhValidation;
            if (isValidated == true)
            {
                CurrentPageModel currentClass = CurrentPageModel.getcurrentclass();
                currentClass.currentpage = "5";
                Page page6 = CurrentPageModel.sixthPage;
                if (page6 == null)
                {
                    this.NavigationService.Navigate(new Uri(@"\ProfilePages\ProfileCreationPage6.xaml", UriKind.RelativeOrAbsolute));
                }
                else
                {
                    //Load in the instance of the page 
                    this.NavigationService.Navigate(page6);
                    //Load in the current navigation control
                    WpfApp1.NavigationControls.NavigationControls sixthControl = (WpfApp1.NavigationControls.NavigationControls)CurrentPageModel.sixthControl;
                    //Set the button manipulation 
                    sixthControl.buttonManipulation(currentClass.currentpage);
                    //Set the page number 
                    sixthControl.PageNumber.Text = sixthControl.currentPageNumber(currentClass.currentpage);

                }
            }
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
            
            gameButton.MouseEnter += onMouseEnter;
            gameButton.MouseLeave += onMouseExit;
            
            
            Console.WriteLine("Game has started");

        }

        private ArrayList generateRow()
        {
            ArrayList rows = new ArrayList();
            rows.Add(0); // For Top Navigation Tracking 
            rows.Add(1); // For the My Folders Tab 
            rows.Add(0); // For the Name Tab on the top right 
            rows.Add(3); // For the controls in the bottom right 
            rows.Add(3); // For the Shared Tab 
            rows.Add(0); // For the top navigation Tracking 
            rows.Add(4); // For the Bin Tab 
            return rows;
        }

        private ArrayList generateCol()
        {
            ArrayList col = new ArrayList();
            col.Add(2); // For Top Navigation Tracking 
            col.Add(1); // For the My Folders Tab 
            col.Add(15); // For the Name Tab on the top right 
            col.Add(14); // For the controls in the bottom right 
            col.Add(0); // For the Shared Tab 
            col.Add(4); // For the top navigation Tracking 
            col.Add(0); // For the Bin Tab 
            return col;
        }

        private void onMouseExit(object sender, MouseEventArgs e)
        {
            Console.WriteLine("Exit");
            Color color = (Color)ColorConverter.ConvertFromString("#c9d1d6");
            gameButton.Background = new SolidColorBrush(color);
        }

        private void onMouseEnter(object sender, MouseEventArgs e)
        {
            Console.WriteLine("Enter");
            Color color = (Color)ColorConverter.ConvertFromString("#23aeff");
            gameButton.Background = new SolidColorBrush(color);
        }

        private void gameButton_Click(object sender, RoutedEventArgs e)
        {
            isClicked = true;
            ArrayList rows = generateRow();
            ArrayList col = generateCol();
            running = true;
            currentIndex++;
            if (isClicked == true)
              {
                if(currentIndex < rows.Count)
                { 
                gameButton.SetValue(Grid.RowProperty, rows[currentIndex]);
                gameButton.SetValue(Grid.ColumnProperty, col[currentIndex]);
                isClicked = false;
                }
                else
                {
                    MessageBox.Show("You have successfully finished this survey");
                    CurrentPageModel.fifthhValidation = true;
                    currentIndex = 0;
                    gameButton.Visibility = Visibility.Hidden;
                    isClicked = false;
                    running = false;

                }
            }
          
        }


        void restartGame(object sender, RoutedEventArgs e)
        {
            gameButton.Visibility = Visibility.Hidden;
            gameStateControl.Content = "Start";
            ArrayList rows = generateRow();
            ArrayList col = generateCol();
            gameButton.SetValue(Grid.RowProperty, rows[0]);
            gameButton.SetValue(Grid.ColumnProperty, col[0]);
            gameStateControl.Click -= new RoutedEventHandler(restartGame);
            gameStateControl.Click += new RoutedEventHandler(gameStateControl_Click);
            currentIndex = 1; 
            isClicked = false;
            running = false;
            Console.WriteLine("Game has restarted");

        }

      
    }


    

      


}

  
