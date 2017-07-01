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
    /// Interaction logic for ProfileCreationPage6.xaml
    /// </summary>
    public partial class ProfileCreationPage6 : Page
    {
        int currentIndex = 0;
        int nextIndex;
        string[] paragraphString;
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
            //var textRange = paragraphBox.Selection;
            //var start = paragraphBox.Document.ContentStart;
            //var startPos = start.GetPositionAtOffset(0);
            //var endPos = start.GetPositionAtOffset(2);
            //textRange.Select(startPos, endPos);
            //textRange.ApplyPropertyValue(TextElement.ForegroundProperty, new SolidColorBrush(Colors.Blue));

            
            RichTextBox rtb = paragraphBox as RichTextBox;
            
            rtb.SelectAll();
            TextSelection currentSelection = rtb.Selection;
            TextPointer start = rtb.Document.ContentStart;

            TypingModel newModel = new TypingModel();
            paragraphString = newModel.getTextInArray();
            int currentLength = paragraphString[0].Length;
            nextIndex = 0 + currentLength + 1;


            TextPointer startPos = GetTextPointAt(start, currentIndex);
            TextPointer endPos = GetTextPointAt(start, nextIndex);
            currentSelection.Select(startPos, endPos);
            Color color = (Color)ColorConverter.ConvertFromString("#23aeff");
            currentSelection.ApplyPropertyValue(TextElement.ForegroundProperty, new SolidColorBrush(color));
            currentSelection.ApplyPropertyValue(TextElement.FontWeightProperty, FontWeights.Bold);

           


        }

        private void setTextColor(int startPos, int endPos)
        {

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


        private void ScrollBar_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {

        }

        private void SubmitPageHandler(object sender, MouseButtonEventArgs e)
        {

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

        private void keyboardFunctions(object sender, KeyEventArgs e)
        {
            if(e.Key == Key.Enter)
            {
                
                if(enterField.Text.Equals(paragraphString[currentIndex]))
                {
                    MessageBox.Show(enterField.Text);
                }
                
            }
            if(e.Key == Key.Escape)
            {
                Keyboard.ClearFocus();
                if(enterField.Text.Equals(""))
                {
                    enterField.Text = "Enter Input Here";
                    header.Focus();
                }

            }
        }

        private void enterField_LostFocus(object sender, RoutedEventArgs e)
        {
            if(enterField.Text.Equals(""))
            {
                enterField.Text = "Enter Input Here";
            }
        }

        private void enterField_GotFocus(object sender, RoutedEventArgs e)
        {
            
            if(enterField.Text.Equals("Enter Input Here"))
            {
                Console.WriteLine("Testing");
                enterField.Text = "";
            }
        }

      
    }
}
