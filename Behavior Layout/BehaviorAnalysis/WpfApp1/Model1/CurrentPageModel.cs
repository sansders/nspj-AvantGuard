using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfApp1.ProfilePages;
using System.Windows.Controls;
namespace WpfApp1.Model1
{
    public class CurrentPageModel
    {
        public string _currentPage;
        public static CurrentPageModel _class;
        public static MainWindow _mainWindow;
        public static System.Windows.Controls.Page _page1;
        public static System.Windows.Controls.UserControl _page1Controls;
        public static Boolean _page1Validation; 
        public static System.Windows.Controls.Page _page2;
        public static System.Windows.Controls.UserControl _page2Controls;
        public static Boolean _page2Validation;
        public static System.Windows.Controls.Page _page3;
        public static System.Windows.Controls.UserControl _page3Controls;
        public static Boolean _page3Validation;
        public static System.Windows.Controls.Page _page4;
        public static System.Windows.Controls.UserControl _page4Controls;
        public static Boolean _page4Validation;
        public static System.Windows.Controls.Page _page5;
        public static System.Windows.Controls.UserControl _page5Controls;
        public static Boolean _page5Validation;
        public static System.Windows.Controls.Page _page6;
        public static System.Windows.Controls.UserControl _page6Controls;
        public static Boolean _page6Validation;
        //Constructor 
        public CurrentPageModel()
        {
            _currentPage = "0"; //Used when initialize to set the page to Page 0 
            _class = this; //Save the current class in a variable to use as static reference
        }

        public string currentpage //Getter and setter for the current page 
        {
            get { return _currentPage; }
            set { _currentPage = value; }
        }

        //Used to set the instance of the current class
        public static void setcurrentclass(CurrentPageModel currentclass) {
            _class = currentclass;
        }

        //Used to get the instance of the current class
        public static CurrentPageModel getcurrentclass()
        {
            return _class;
        }

        //Used to set which is the main window which can be reference from the User Controls
        public static void setMainWindow(MainWindow currentWindow)
        {
            _mainWindow = currentWindow;
        }

        //Used to get the main window
        public static MainWindow getMainWindow()
        {
            return _mainWindow;
        }

        //Save the static instance of the first page 
        public static System.Windows.Controls.Page firstPage
        {
            get { return _page1; }
            set { _page1 = value; }
        }


        //Save the static instance of the first page control 
        public static System.Windows.Controls.UserControl firstControl
        {
            get { return _page1Controls; }
            set { _page1Controls = value; }
        }

        //Save the current status of first validation
        public static Boolean firstValidation
        {
           get { return _page1Validation; }
           set { _page1Validation = value; }
        }

     

        //Save the static instance of the second page 
        public static System.Windows.Controls.Page secondPage
        {
            get { return _page2; }
            set { _page2 = value; }
        }

        //Save the static instance of the second page control 
        public static System.Windows.Controls.UserControl secondControl
        {
            get { return _page2Controls; }
            set { _page2Controls = value; }
        }

        //Save the current status of second validation
        public static Boolean secondValidation
        {
            get { return _page2Validation; }
            set { _page2Validation = value; }
        }

        //Save the static instance of the Third page 
        public static System.Windows.Controls.Page thirdPage
        {
            get { return _page3; }
            set { _page3 = value; }
        }
        //Save the static instance of the Third page control 
        public static System.Windows.Controls.UserControl thirdControl
        {
            get { return _page3Controls; }
            set { _page3Controls = value; }
        }

        //Save the current status of third validation
        public static Boolean thirdValidation
        {
            get { return _page3Validation; }
            set { _page3Validation = value; }
        }

        //Save the static instance of the Fourth page
        public static System.Windows.Controls.Page fourthPage
        {
            get { return _page4; }
            set { _page4 = value; }
        }

        //Save the static instance of the Fourth page control
        public static System.Windows.Controls.UserControl fourthControl
        {
            get { return _page4Controls; }
            set { _page4Controls = value; }
        }

        //Save the current status of fourth validation
        public static Boolean fourthValidation
        {
            get { return _page4Validation; }
            set { _page4Validation = value; }
        }

        //Save the static instance of the Fifth page
        public static System.Windows.Controls.Page fifthPage
        {
            get { return _page5; }
            set { _page5 = value; }
        }

        //Save the static instance of the Fifth page control
        public static System.Windows.Controls.UserControl fifthControl
        {
            get { return _page5Controls; }
            set { _page5Controls = value; }
        }

        //Save the current status of fifth validation
        public static Boolean fifthhValidation
        {
            get { return _page5Validation; }
            set { _page5Validation = value; }
        }

        //Save the static instance of the Fifth page
        public static System.Windows.Controls.Page sixthPage
        {
            get { return _page6; }
            set { _page6 = value; }
        }

        //Save the static instance of the Fifth page control
        public static System.Windows.Controls.UserControl sixthControl
        {
            get { return _page6Controls; }
            set { _page6Controls = value; }
        }

        //Save the current status of sixth validation
        public static Boolean sixthValidation
        {
            get { return _page6Validation; }
            set { _page6Validation = value; }
        }



        //public static void setfirstpage(system.windows.controls.page page1)
        //{
        //    _page1 = page1;
        //}

        //public static system.windows.controls.page getfirstpage()
        //{
        //    return _page1;
        //}

        //public static void setFirstControl(System.Windows.Controls.UserControl control)
        //{
        //    _page1Controls = control;
        //}

        //public static System.Windows.Controls.UserControl getFirstControl()
        //{
        //    return _page1Controls;
        //}


        //public static void setSecondPage(System.Windows.Controls.Page page2)
        //{
        //    _page2 = page2;
        //}

        //public static System.Windows.Controls.Page getSecondPage()
        //{
        //    return _page2;
        //}


        //public static void setSecondControl(System.Windows.Controls.UserControl control)
        //{
        //    _page2Controls = control;
        //}

        //public static System.Windows.Controls.UserControl getSecondControl()
        //{
        //    return _page2Controls;
        //}


        //public static void setThirdPage(System.Windows.Controls.Page page3)
        //{
        //    _page3 = page3;
        //}

        //public static System.Windows.Controls.Page getThirdPage()
        //{
        //    return _page3;
        //}


        //public static void setThirdControl(System.Windows.Controls.UserControl control)
        //{
        //    _page3Controls = control;
        //}

        //public static System.Windows.Controls.UserControl getThirdControl()
        //{
        //    return _page3Controls;
        //}







    }
}
