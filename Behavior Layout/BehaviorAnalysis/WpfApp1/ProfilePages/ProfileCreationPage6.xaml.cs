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
using System.Collections;
using System.Windows.Threading;
using System.Diagnostics;

namespace WpfApp1.ProfilePages
{
    /// <summary>
    /// Interaction logic for ProfileCreationPage6.xaml
    /// </summary>
    public partial class ProfileCreationPage6 : Page
    {
        int currentIndex = 0;
        int nextIndex;
        int roundNumber = 0;
        string[] paragraphString;
        List<int> currentErrorList = new List<int>();
        List<int> nextErrorList = new List<int>();
        List<int> currentCorrectList = new List<int>();
        List<int> nextCorrectList = new List<int>();
        List<int> numberOfCorrectKeyInput = new List<int>();
        List<int> numberOfWrongKeyInput = new List<int>();
        DispatcherTimer myTimer = new DispatcherTimer();
        Stopwatch sw = new Stopwatch();
        TimeSpan myTimeSpan;
        Boolean startTimerBoolean;
        int total = 0; 


        public ProfileCreationPage6()
        {
            InitializeComponent();
            CurrentPageModel.sixthPage = this;
            CurrentPageModel.sixthControl = page6Controls;
            CurrentPageModel.sixthValidation = false;
            initializeParagraph();

        }

        private void initializeParagraph()
        {
            nextIndex = setNextIndex(nextIndex);
            setNextTextColor(currentIndex, nextIndex, paragraphBox);
        }

        //Start the timer for the total elapsed time 
        private void startTimer()
        {
            startTimerBoolean = true;
            //Start the StopWatch
            sw.Start();
            //Set the Dispatcher Timer to run this method everytime it ticks
            myTimer.Tick += new EventHandler(dt_Tick);
            myTimeSpan = new TimeSpan(0, 0, 1);
            //Set the interval of each ticks to 1 sec
            myTimer.Interval = myTimeSpan;
            //Start the dispatcher timer
            myTimer.Start();
        }

        private void stopTimer()
        {
            //Stop the StopWatch
            sw.Stop();
            sw.Reset();
            startTimerBoolean = false;
            //Stop the dispatcher timer
            myTimer.Stop();
            timerField.Text = "";
        }

        private void dt_Tick(object sender, EventArgs e)
        {
            DateTime s = new DateTime();
            //Adds the stop watch to date time.
            s = s + sw.Elapsed;
            //Append it to the timerField
            Console.WriteLine(s.ToString("mm:ss"));
            timerField.Text = s.ToString("mm:ss");
        }

        //Set the color for the next word 
        private int setNextIndex(int nextIndex)
        {
            int returnIndex = 0;
            if (roundNumber == 0)
            {
                TypingModel newModel = new TypingModel();
                //Gets the total count of words
                paragraphString = newModel.getTextInArray(); ;
                //Gets the length of the first word
                int currentLength = paragraphString[0].Length;
                returnIndex = 0 + currentLength + 1;
               
            }
            else
            {
                TypingModel newModel = new TypingModel();
                paragraphString = newModel.getTextInArray();
                int currentLength = paragraphString[roundNumber].Length;
                returnIndex = currentIndex + currentLength + 1;
               
            }
            return returnIndex;
        }

        private void setNextTextColor(int starting, int ending, RichTextBox rtb)
        {
            //Select the entire box
            rtb.SelectAll();
            //Create a selection 
            TextSelection currentSelection = rtb.Selection;
            //Set the starter pointer to the start of the content 
            TextPointer start = rtb.Document.ContentStart;
            TypingModel newModel = new TypingModel();
            //Indicate the start and end pos
            TextPointer startPos = GetTextPointAt(start, starting);
            TextPointer endPos = GetTextPointAt(start, ending);
            //Choose the area in which to change the color
            currentSelection.Select(startPos, endPos);
            Color color = (Color)ColorConverter.ConvertFromString("#23aeff");
            currentSelection.ApplyPropertyValue(TextElement.ForegroundProperty, new SolidColorBrush(color));
            currentSelection.ApplyPropertyValue(TextElement.FontWeightProperty, FontWeights.Bold);
            
        }

        private void setCorrectTextColor(int starting, int ending, RichTextBox rtb)
        {
            //Select the entire box
            rtb.SelectAll();
            //Create a selection 
            TextSelection currentSelection = rtb.Selection;
            //Set the starter pointer to the start of the content 
            TextPointer start = rtb.Document.ContentStart;
            TypingModel newModel = new TypingModel();
            //Indicate the start and end pos
            TextPointer startPos = GetTextPointAt(start, starting);
            TextPointer endPos = GetTextPointAt(start, ending);
            //Choose the area in which to change the color
            currentSelection.Select(startPos, endPos);
            Color color = (Color)ColorConverter.ConvertFromString("#00E500");
            currentSelection.ApplyPropertyValue(TextElement.ForegroundProperty, new SolidColorBrush(color));
            currentSelection.ApplyPropertyValue(TextElement.FontWeightProperty, FontWeights.Bold);

        }

        private void setErrorTextColor(int starting, int ending, RichTextBox rtb)
        {

            //Select the entire box
            rtb.SelectAll();
            //Create a selection 
            TextSelection currentSelection = rtb.Selection;
            //Set the starter pointer to the start of the content 
            TextPointer start = rtb.Document.ContentStart;
            TypingModel newModel = new TypingModel();
            //Indicate the start and end pos
            TextPointer startPos = GetTextPointAt(start, starting);
            TextPointer endPos = GetTextPointAt(start, ending);
            //Choose the area in which to change the color
            currentSelection.Select(startPos, endPos);
            Color color = (Color)ColorConverter.ConvertFromString("#Ab3334");
            currentSelection.ApplyPropertyValue(TextElement.ForegroundProperty, new SolidColorBrush(color));
            currentSelection.ApplyPropertyValue(TextElement.FontWeightProperty, FontWeights.Bold);

        }

        private void removeTextColor(RichTextBox rtb)
        {
            //Select all and remove all the foreground and font weight element property
            rtb.SelectAll();
            TextSelection currentSelection = rtb.Selection;
            currentSelection.ApplyPropertyValue(TextElement.ForegroundProperty, new SolidColorBrush(Colors.Black));
            currentSelection.ApplyPropertyValue(TextElement.FontWeightProperty, FontWeights.Normal);

        }

        //A method i got from the internet to make my pointer work 
        private static TextPointer GetTextPointAt(TextPointer from, int pos)
        {
            TextPointer ret = from;
            int i = 0;

            while ((i < pos) && (ret != null))
            {
                if ((ret.GetPointerContext(LogicalDirection.Backward) == TextPointerContext.Text) || (ret.GetPointerContext(LogicalDirection.Backward) == TextPointerContext.None))
                    i++;

                if (ret.GetPositionAtOffset(1, LogicalDirection.Forward) == null)
                    return ret;

                ret = ret.GetPositionAtOffset(1, LogicalDirection.Forward);
            }
            return ret;
        }

        private void restartGame(object sender, RoutedEventArgs e)
        {
            //Confirm if the user wants to restart the game 
            if (MessageBox.Show("You sure you want to restart the Keyboard Test?", "Question", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
            {
                //If yes 
                //Stop the stopwatch timer
                stopTimer();
                //Stop the capturing of Correct Key and Wrong Key
                stopCorrectKeyCapture();
                stopWrongKeyCapture();
                //Clear all the List of Words
                currentCorrectList.Clear();
                nextCorrectList.Clear();
                currentErrorList.Clear();
                nextErrorList.Clear();
                //Remove the text color in the paragraph
                removeTextColor(paragraphBox);
                //Reinitialize in the starting state where everything is 0 
                currentIndex = 0;
                nextIndex = 0;
                roundNumber = 0;
                //Set the event handler for the button to change to start game
                restartButton.Click -= restartGame;
                restartButton.Click += startGame;
                //Hide the enter field 
                enterField.Visibility = Visibility.Hidden;
                restartButton.Content = "Start";
                

            }
            else
            {

            }
            

          

        }

        private void stopGame()
        {
            //After user completed the game
            //Stop the stopwatch
            stopTimer();
            //Stop capturing correct and wrong info
            stopCorrectKeyCapture();
            stopWrongKeyCapture();
            //Set everything to starting slate
            currentIndex = 0;
            nextIndex = 0;
            roundNumber = 0;
            //Hide the Entering field area
            enterField.Visibility = Visibility.Hidden;
            
        }

        private void startGame(object sender, RoutedEventArgs e)
        {
            //Quite self explanatory 
            initializeParagraph();
            //Change the event handler to the restart game method
            restartButton.Click -= startGame;
            restartButton.Click += restartGame;
            restartButton.Content = "Restart";
            enterField.Visibility = Visibility.Visible;
        }



        private void keyboardFunctions(object sender, KeyEventArgs e)
        {
           
            //If this is the first round, i will start the timer once one of the key is inputted
            if(roundNumber == 0 && startTimerBoolean == false)
            {
                startTimer();
            }
            //If user enters the space key
            if (e.Key == Key.Space)
            {
                Console.WriteLine("Activated ");
                //Check to see if this is the last word in the entire set of words
                if (roundNumber == paragraphString.Length - 1)
                {
                    //Checks if its correct or wrong
                    if (enterField.Text.Equals(paragraphString[roundNumber]) || enterField.Text.Equals(" " + paragraphString[roundNumber]))
                    {
                        //If correct, then save the correct word, set the color and add it into the correct list
                        startCorrectKeyCapture(enterField.Text.Length);
                        setCorrectTextColor(currentIndex, nextIndex, paragraphBox);
                        currentCorrectList.Add(currentIndex);
                        nextCorrectList.Add(nextIndex);
                        enterField.Text = "";
                    }
                    else
                    {
                        //save the wrong word, set the wrong color and add it into the wrong list
                        startWrongKeyCapture(enterField.Text.Length);
                        setErrorTextColor(currentIndex, nextIndex, paragraphBox);
                        currentErrorList.Add(currentIndex);
                        nextErrorList.Add(nextIndex);
                        enterField.Text = "";
                    }
                    
                    nextIndex = setNextIndex(nextIndex);
                    //removeTextColor(paragraphBox);
                    // setCorrect(currentCorrectList, nextCorrectList, paragraphBox);
                    //if (currentErrorList.Count != 0)
                    //{
                    //    setError(currentErrorList, nextErrorList, paragraphBox);
                    //}

                    //This is what happens after you finish 
                    TypingModel newModel = new TypingModel();
                    //Get the total number of word count
                    string [] totalCount = newModel.getTextInArray();
                    //Check to see if the word count matches
                    if(nextErrorList.Count() + nextCorrectList.Count() == totalCount.Count())
                    {
                        //If it matches, check the validation of this page to true
                        CurrentPageModel.sixthValidation = true;
                        //A big chuck of stuff that shows the average of your typing speed
                        MessageBox.Show("You have successfully completed the Typing Test");
                        Console.WriteLine("Number of Errors are " + nextErrorList.Count());
                        Console.WriteLine("Number of Corrects are " + nextCorrectList.Count());
                        double averageTypingSpeed = calAverageTypingSpeed(sw.Elapsed, numberOfCorrectKeyInput , numberOfWrongKeyInput);
                        MessageBox.Show("Number of Errors are " + nextErrorList.Count() + "\n" +
                            "Number of Corrects are " + nextCorrectList.Count() + "\n" +
                            "The Average Character Per Second is " + averageTypingSpeed + "\n" +
                            "The Average Number of Words per minute is " + ((averageTypingSpeed * 60) / 5));
                        Console.WriteLine(
                            "Number of Errors are " + nextErrorList.Count() + "\n" +
                            "Number of Corrects are " + nextCorrectList.Count() + "\n" +
                            "The Average Character Per Second is " + averageTypingSpeed + "\n" +
                            "The Average Number of Words per minute is " + ((averageTypingSpeed * 60) / 5));
                        stopGame();
                    }

                }
                else
                { 
                    if (enterField.Text.Equals(paragraphString[roundNumber]) || enterField.Text.Equals(" " + paragraphString[roundNumber]))
                    {
                        //If correct, then save the correct word, set the color and add it into the correct list
                        Console.Write("Hiuhsada");
                        startCorrectKeyCapture(enterField.Text.Length);
                        //Increment the current round to the next
                        roundNumber++;
                        currentCorrectList.Add(currentIndex);
                        nextCorrectList.Add(nextIndex);
                        //setCorrectTextColor(current)
                        setCorrectTextColor(currentIndex, nextIndex, paragraphBox);
                        //Set the current index to the next location and set the next index to the next next location 
                        currentIndex = nextIndex;
                        nextIndex = setNextIndex(nextIndex);
                        //removeTextColor(paragraphBox);
                        //if (currentErrorList.Count != 0)
                        //{
                        //    setError(currentErrorList, nextErrorList, paragraphBox);
                        //}
                        setNextTextColor(currentIndex, nextIndex, paragraphBox);
                        enterField.Text = "";
                    
                    }
                    else 
                    {
                        //save the wrong word, set the wrong color and add it into the wrong list
                        roundNumber++;
                        currentErrorList.Add(currentIndex);
                        startWrongKeyCapture(enterField.Text.Length);
                        nextErrorList.Add(nextIndex);
                        //removeTextColor(paragraphBox);
                        //if (currentCorrectList.Count != 0)
                        //{
                        //    setCorrect(currentCorrectList, nextCorrectList, paragraphBox);
                        //}
                        //setError(currentErrorList, nextErrorList, paragraphBox);
                        setErrorTextColor(currentIndex, nextIndex, paragraphBox);
                        //Set the current index to the next location and set the next index to the next next location 
                        currentIndex = nextIndex;
                        nextIndex = setNextIndex(nextIndex);
                        setNextTextColor(currentIndex, nextIndex, paragraphBox);
                        enterField.Text = "";
                       
                    }
                }

            }
      
        }

        private double calAverageTypingSpeed(TimeSpan elapsed, List<int> numberOfCorrectKeyInput , List<int> numberOfWrongKeyInput )
        {
            //Takes the total time 
            double time = elapsed.TotalMinutes + elapsed.TotalSeconds;
            Console.WriteLine("Current Elapsed Time is " + time);
            int totalInput = 0; 
            //Gets all the input regardless of correct or wrong 
            foreach (int input in numberOfCorrectKeyInput)
            {
                totalInput += input;
            }
            foreach (int input in numberOfWrongKeyInput)
            {
                totalInput += input; 
            }

            //Find the percentage that got wrong 
            TypingModel newModel = new TypingModel();
            string[] totalCount = newModel.getTextInArray();
            //Get the total number of word count
            int totalWords = totalCount.Count();
            double currentError = currentErrorList.Count();
            //Get the percentage of Wrong
            double percentageOfWrong = currentError / totalWords;
            Console.WriteLine("Current number of error is " + currentErrorList.Count);
            Console.WriteLine("Current total count is " + totalCount.Count());
            Console.WriteLine("Current percentage of wrong is " + percentageOfWrong);

            //Calculate the Total * (100 - Percentage of Wrong)
            //Which is basically the rate at which is quite average 
            double finalInput = totalInput * (1 - percentageOfWrong);
            double averageTypingSpeed = finalInput / time;
            return averageTypingSpeed;
        }

        private void startCorrectKeyCapture(int letters)
        {
            //Capture the correct words
            numberOfCorrectKeyInput.Add(letters);
            total += letters;
            Keys.Text = total.ToString() ;
        }

        private void stopCorrectKeyCapture()
        {
            //Stop the capturing of the correct words
            numberOfCorrectKeyInput.Clear();
            total = 0;
            Keys.Text = "";
        }

        private void startWrongKeyCapture(int letters)
        {
            //Capture the wrong words
            numberOfWrongKeyInput.Add(letters);
            total += letters;
            Keys.Text = total.ToString();
        }

        private void stopWrongKeyCapture()
        {
            //Stop the capturing of wrong words
            numberOfWrongKeyInput.Clear();
            total = 0;
            Keys.Text = "";
        }
        private void enterField_KeyDown(object sender, KeyEventArgs e)
        {
            //If user press escape, remove focus 
            if (e.Key == Key.Escape)
            {
                Keyboard.ClearFocus();
            }
        }

        private void EnterField_GotFocus(object sender, RoutedEventArgs e)
        {
            throw new NotImplementedException();
        }

        //// i think this is not used
        //private void setCorrect(List<int> currentCorrectList, List<int> nextCorrectList, RichTextBox paragraphBox)
        //{
        //    var startAndEnd = currentCorrectList.Zip(nextCorrectList, (s, e) => new { start = s, end = e });
        //    foreach (var element in startAndEnd)
        //    {
        //        setCorrectTextColor(element.start, element.end, paragraphBox);
        //    }
        //}

        ////i think this is not used
        //private void setError(List<int> currentError, List<int> nextError, RichTextBox paragraphBox)
        //{
        //    var startAndEnd = currentError.Zip(nextError, (s, e) => new { start = s, end = e });
        //    foreach (var element in startAndEnd)
        //    {
        //        setErrorTextColor(element.start, element.end, paragraphBox);
        //    }
        //}

        private void enterField_LostFocus(object sender, RoutedEventArgs e)
        {
            //Just loses focus la 
            if (enterField.Text.Equals(""))
            {
                enterField.Text = "Press Space to Submit";
                
            }
        }

        private void enterField_GotFocus(object sender, RoutedEventArgs e)
        {
            //Just get focus la 
            Console.WriteLine("Testing");
            if (enterField.Text.Equals("Press Space to Submit"))
            {
                enterField.Text = "";
            }

        }


        //Navigation Methods that i am too lazy to comment 
        private void SubmitPageHandler(object sender, MouseButtonEventArgs e)
        {
            if(
                CurrentPageModel.firstValidation == true &&
                CurrentPageModel.secondValidation == true && 
                CurrentPageModel.thirdValidation == true && 
                CurrentPageModel.fourthValidation == true && 
                CurrentPageModel.fifthhValidation == true && 
                CurrentPageModel.sixthValidation == true 
                )
            {
                MessageBox.Show("Successfully Completed Profiling Survey");
            }
            else
            {
                MessageBox.Show("Typing test hasn't been completed");
            }
        }

        private void PreviousPageHandler(object sender, MouseButtonEventArgs e)
        {
            //Get the current instance of the navigation class
            CurrentPageModel currentClass = CurrentPageModel.getcurrentclass();
            currentClass.currentpage = "4";
            Page page5 = CurrentPageModel.fifthPage;
            if (page5 == null)
            {
                this.NavigationService.Navigate(new Uri(@"\ProfilePages\ProfileCreationPage4.xaml", UriKind.RelativeOrAbsolute));
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
            CurrentPageModel.sixthPage = this;
            //Save current instance of the user control
            CurrentPageModel.sixthControl = page6Controls;
        }

       
    }

}
