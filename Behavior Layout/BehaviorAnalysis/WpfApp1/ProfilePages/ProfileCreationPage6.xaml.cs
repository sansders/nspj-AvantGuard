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

        private int setNextIndex(int nextIndex)
        {
            int returnIndex = 0;
            if (roundNumber == 0)
            {
                TypingModel newModel = new TypingModel();
                paragraphString = newModel.getTextInArray();
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

            rtb.SelectAll();
            TextSelection currentSelection = rtb.Selection;
            TextPointer start = rtb.Document.ContentStart;
            TypingModel newModel = new TypingModel();
            TextPointer startPos = GetTextPointAt(start, starting);
            TextPointer endPos = GetTextPointAt(start, ending);
            currentSelection.Select(startPos, endPos);
            Color color = (Color)ColorConverter.ConvertFromString("#23aeff");
            currentSelection.ApplyPropertyValue(TextElement.ForegroundProperty, new SolidColorBrush(color));
            currentSelection.ApplyPropertyValue(TextElement.FontWeightProperty, FontWeights.Bold);

        }

        private void setCorrectTextColor(int starting, int ending, RichTextBox rtb)
        {

            rtb.SelectAll();
            TextSelection currentSelection = rtb.Selection;
            TextPointer start = rtb.Document.ContentStart;
            TypingModel newModel = new TypingModel();
            TextPointer startPos = GetTextPointAt(start, starting);
            TextPointer endPos = GetTextPointAt(start, ending);
            currentSelection.Select(startPos, endPos);
            Color color = (Color)ColorConverter.ConvertFromString("#00E500");
            currentSelection.ApplyPropertyValue(TextElement.ForegroundProperty, new SolidColorBrush(color));
            currentSelection.ApplyPropertyValue(TextElement.FontWeightProperty, FontWeights.Bold);

        }

        private void setErrorTextColor(int starting, int ending, RichTextBox rtb)
        {

            rtb.SelectAll();
            TextSelection currentSelection = rtb.Selection;
            TextPointer start = rtb.Document.ContentStart;
            TypingModel newModel = new TypingModel();
            TextPointer startPos = GetTextPointAt(start, starting);
            TextPointer endPos = GetTextPointAt(start, ending);
            currentSelection.Select(startPos, endPos);
            Color color = (Color)ColorConverter.ConvertFromString("#Ab3334");
            currentSelection.ApplyPropertyValue(TextElement.ForegroundProperty, new SolidColorBrush(color));
            currentSelection.ApplyPropertyValue(TextElement.FontWeightProperty, FontWeights.Bold);

        }

        private void removeTextColor(RichTextBox rtb)
        {
            rtb.SelectAll();
            TextSelection currentSelection = rtb.Selection;
            currentSelection.ApplyPropertyValue(TextElement.ForegroundProperty, new SolidColorBrush(Colors.Black));
            currentSelection.ApplyPropertyValue(TextElement.FontWeightProperty, FontWeights.Normal);

        }

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



        private void keyboardFunctions(object sender, KeyEventArgs e)
        {
            
            if (e.Key == Key.Space)
            {
                Console.WriteLine("Activated ");
                if (roundNumber == paragraphString.Length - 1)
                {

                    if (enterField.Text.Equals(paragraphString[roundNumber]) || enterField.Text.Equals(" " + paragraphString[roundNumber]))
                    {
                        setCorrectTextColor(currentIndex, nextIndex, paragraphBox);
                        currentCorrectList.Add(currentIndex);
                        nextCorrectList.Add(nextIndex);
                    }
                    else
                    {
                        setErrorTextColor(currentIndex, nextIndex, paragraphBox);
                        currentErrorList.Add(currentIndex);
                        nextErrorList.Add(nextIndex);
                    }
                    nextIndex = setNextIndex(nextIndex);
                    //removeTextColor(paragraphBox);
                    // setCorrect(currentCorrectList, nextCorrectList, paragraphBox);
                    //if (currentErrorList.Count != 0)
                    //{
                    //    setError(currentErrorList, nextErrorList, paragraphBox);
                    //}
                    if(nextErrorList.Count() + nextCorrectList.Count() == 47)
                    {
                        CurrentPageModel.sixthValidation = true;
                        MessageBox.Show("You have successfully completed the Typing Test");
                        Console.WriteLine("Number of Errors are " + nextErrorList.Count());
                        Console.WriteLine("Number of Corrects are " + nextCorrectList.Count());
                    }

                }
                else
                { 
                    if (enterField.Text.Equals(paragraphString[roundNumber]) || enterField.Text.Equals(" " + paragraphString[roundNumber]))
                    {
                        Console.Write("Hiuhsada");
                        roundNumber++;
                        currentCorrectList.Add(currentIndex);
                        nextCorrectList.Add(nextIndex);
                        //setCorrectTextColor(current)
                        setCorrectTextColor(currentIndex, nextIndex, paragraphBox);
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
                        roundNumber++;
                        currentErrorList.Add(currentIndex);
                        nextErrorList.Add(nextIndex);
                        //removeTextColor(paragraphBox);
                        //if (currentCorrectList.Count != 0)
                        //{
                        //    setCorrect(currentCorrectList, nextCorrectList, paragraphBox);
                        //}
                        //setError(currentErrorList, nextErrorList, paragraphBox);
                        setErrorTextColor(currentIndex, nextIndex, paragraphBox);
                        currentIndex = nextIndex;
                        nextIndex = setNextIndex(nextIndex);
                        setNextTextColor(currentIndex, nextIndex, paragraphBox);
                        enterField.Text = "";
                       
                    }
                }

            }
      
        }

        private void enterField_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
            {
                Keyboard.ClearFocus();
            }
        }

        private void EnterField_GotFocus(object sender, RoutedEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void setCorrect(List<int> currentCorrectList, List<int> nextCorrectList, RichTextBox paragraphBox)
        {
            var startAndEnd = currentCorrectList.Zip(nextCorrectList, (s, e) => new { start = s, end = e });
            foreach (var element in startAndEnd)
            {
                setCorrectTextColor(element.start, element.end, paragraphBox);
            }
        }

        private void setError(List<int> currentError, List<int> nextError, RichTextBox paragraphBox)
        {
            var startAndEnd = currentError.Zip(nextError, (s, e) => new { start = s, end = e });
            foreach (var element in startAndEnd)
            {
                setErrorTextColor(element.start, element.end, paragraphBox);
            }
        }

        private void enterField_LostFocus(object sender, RoutedEventArgs e)
        {
            if (enterField.Text.Equals(""))
            {
                enterField.Text = "Press Space to Submit";
                
            }
        }

        private void enterField_GotFocus(object sender, RoutedEventArgs e)
        {
            Console.WriteLine("Testing");
            if (enterField.Text.Equals("Press Space to Submit"))
            {
                enterField.Text = "";
            }

        }

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
