using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfApp1.ProfilePages;

namespace WpfApp1.Model1
{
    public class CurrentPageModel
    {
        public string _currentPage;
        public static CurrentPageModel _class;
        public static MainWindow _mainWindow;
        public static System.Windows.Controls.Page _page1;
        public static System.Windows.Controls.UserControl _page1Controls;
        public static System.Windows.Controls.Page _page2;
        public static System.Windows.Controls.UserControl _page2Controls;
        public static System.Windows.Controls.Page _page3;
        public static System.Windows.Controls.UserControl _page3Controls;

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
        public static void setcurrentclass(CurrentPageModel currentclass){ 
            _class = currentclass;
        }

        //Used to get the instance of the current class
        public static CurrentPageModel getcurrentclass()
        {
            return _class;
        }

        //Used to set which is the main window which can be reference from the User Controls
        public static void setMainWindow (MainWindow currentWindow)
        {
            _mainWindow = currentWindow;
        }

        //Used to get the main window
        public static MainWindow getMainWindow()
        {
            return _mainWindow;
        }

        //Save the static instance of the first page 
        public static void setFirstPage (System.Windows.Controls.Page page1)
        {
            _page1 = page1;
        }

        public static System.Windows.Controls.Page getFirstPage()
        {
            return _page1;
        }

        //Save the static instance of the first page control 
        public static void setFirstControl (System.Windows.Controls.UserControl control)
        {
            _page1Controls = control;
        }

        public static System.Windows.Controls.UserControl getFirstControl()
        {
            return _page1Controls;
        }
      

        //Save the static instance of the second page 
        public static void setSecondPage(System.Windows.Controls.Page page2)
        {
            _page2 = page2;
        }

        public static System.Windows.Controls.Page getSecondPage()
        {
            return _page2;
        }

        //Save the static instance of the second page control 
        public static void setSecondControl(System.Windows.Controls.UserControl control)
        {
            _page2Controls = control;
        }

        public static System.Windows.Controls.UserControl getSecondControl()
        {
            return _page2Controls;
        }

        //Save the static instance of the Third page 
        public static void setThirdPage(System.Windows.Controls.Page page3)
        {
            _page3 = page3;
        }

        public static System.Windows.Controls.Page getThirdPage()
        {
            return _page3;
        }

        //Save the static instance of the Third page control 
        public static void setThirdControl(System.Windows.Controls.UserControl control)
        {
            _page3Controls = control;
        }

        public static System.Windows.Controls.UserControl getThirdControl()
        {
            return _page3Controls;
        }








    }
}
