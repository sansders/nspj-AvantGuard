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
        List<Double> xAxisList = new List<Double>();
        List<Double> yAxisList = new List<Double>();
        List<List<Double>> fullXList = new List<List<Double>>();
        List<List<Double>> fullYList = new List<List<Double>>();
        List<Double> xNumber = new List<Double>();
        List<Double> yNumber = new List<Double>();
        private List<double> xList;

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
                    Console.WriteLine("Hello");
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
            else
            {
                MessageBox.Show("Button Game hasn't been completed. Please complete it first");
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
                if(currentIndex == 1)
                    {
                        GameField.MouseMove += new MouseEventHandler(moveActivated);
                    }
                else
                    {
                        GameField.MouseMove -= new MouseEventHandler(moveActivated);
                        GameField.MouseMove += new MouseEventHandler(moveActivated);
                        SaveCurrentCoordinate();
                        RunNextCoordinate();
                    }
                isClicked = false;
                }
                else
                {
                    MessageBox.Show("Click next to proceed to the next page");
                    Console.WriteLine("Current X Size is " + fullXList.Count());
                    Console.WriteLine("Current Y Size is " + fullYList.Count());
                   
                    //List<Double> firstX = fullXList[0];
                    //Console.WriteLine(firstX[0]);
                    //List<Double> secondX = fullXList[1];
                    //Console.WriteLine(secondX[1]);




                    //Console.WriteLine(firstX.Count);
                    //Console.WriteLine(secondX.Count);

                    //Console.WriteLine("cdescads");
                    //foreach (List<Double> element in fullXList)
                    //{
                    //    Console.WriteLine(element.Count);
                    //}
                    //foreach (double element in firstX)
                    //{
                    //    Console.WriteLine(element);
                    //}

                    List<Double> xFirstList = fullXList[1];
                    List<Double> yFirstList = fullYList[1];
                    List<Double> xSecondList = fullXList[2];
                    List<Double> ySecondList = fullYList[2];
                    //line = drawLine(line, xFirstList[0] , yFirstList[0], xSecondList[0], ySecondList[0]);
                    var windows = new Window();
                    windows.Width = 300;
                    windows.Height = 300;
                    Canvas newCanvas = new Canvas();
                    newCanvas.Width = 300;
                    newCanvas.Height = 300;
                    newCanvas.Background = new SolidColorBrush(Colors.Green);
                    TextBlock popUpText = new TextBlock();
                    popUpText.Text = "Current X Size is " + fullXList.Count() + "\n"
                       + "Current Y Size is " + fullYList.Count();

                    Line line = new Line();
                    line = drawLine(line, xFirstList[0], yFirstList[0], xSecondList[0], ySecondList[0]);

                    Console.WriteLine(xFirstList[0]);
                    Console.WriteLine(xSecondList[0]);

                    windows.Content = newCanvas;
                    newCanvas.Children.Add(popUpText);
                    newCanvas.Children.Add(line);
                    windows.ShowDialog();

                    CurrentPageModel.fifthhValidation = true;
                    currentIndex = 0;
                    gameButton.Visibility = Visibility.Hidden;
                    isClicked = false;
                    running = false;

                }
            }
          
        }


        private void moveActivated(object sender, MouseEventArgs e)
        {
            Point pointToScreen = Mouse.GetPosition(GameField);
            xAxisList.Add(pointToScreen.X);
            yAxisList.Add(pointToScreen.Y);
            //var pointToScreen = PointToScreen(e.GetPosition(GameField));
            Header.Text = "Move your mouse to the circle that pops up and click it" + "\n" + " X: " + pointToScreen.X + " Y: " + pointToScreen.Y;
        }

        private Line drawLine(Line line, double x1, double y1, double x2, double y2)
        {

            line.X1 = x1;
            line.Y1 = y1;
            line.X2 = x2;
            line.Y2 = y2;
            line.StrokeThickness = 5;
            line.Stroke = new SolidColorBrush(Colors.Black);
            return line;
        }

        private void RunNextCoordinate()
        {
            xAxisList.Clear();
            yAxisList.Clear();
           
        }

        private void SaveCurrentCoordinate()
        {

            Console.WriteLine("x and y at start axis list is now both have a value of " + xAxisList.Count + yAxisList.Count);

            List<Double> newXList = new List<double>();
            foreach (double element in xAxisList)
            {
                newXList.Add(element);
            }

            List<Double> newYList = new List<double>();
            foreach (double element in yAxisList)
            {
                newYList.Add(element);
            }


            fullXList.Add(newXList);
            fullYList.Add(newYList);
            xNumber.Add(xAxisList.Count);
            yNumber.Add(yAxisList.Count);

           

            //xAxisList.Clear();
            //yAxisList.Clear();
            List<Double> myList = fullXList[0];
            Console.WriteLine("cjmdisuacnadswoincasoiucdns");
            
            Console.WriteLine("x and y axis list is now both have a value of " + xAxisList.Count + yAxisList.Count);
            
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

  
