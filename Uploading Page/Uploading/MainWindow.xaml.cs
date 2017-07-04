using Layout.Upload;
using System;
using System.Collections.Generic;
using System.Configuration;
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

namespace Layout
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            ProtectConfiguration();
            _NavigationFrame.Navigate(new NavigationPage());
        }

        private static void ProtectConfiguration()
        {

            // Get the application configuration file.
            System.Configuration.Configuration config =
                    ConfigurationManager.OpenExeConfiguration(
                    ConfigurationUserLevel.None);

            // Define the Rsa provider name.
            string provider =
                "RsaProtectedConfigurationProvider";

            // Get the section to protect.
            ConfigurationSection connStrings = config.ConnectionStrings;

            if (connStrings != null)
            {
                if (!connStrings.SectionInformation.IsProtected)
                {
                    if (!connStrings.ElementInformation.IsLocked)
                    {
                        // Protect the section.
                        connStrings.SectionInformation.ProtectSection(provider);

                        connStrings.SectionInformation.ForceSave = true;
                        config.Save(ConfigurationSaveMode.Full);

                        Console.WriteLine("Section {0} is now protected by {1}",
                            connStrings.SectionInformation.Name,
                            connStrings.SectionInformation.ProtectionProvider.Name);

                    }
                    else
                        Console.WriteLine(
                             "Can't protect, section {0} is locked",
                             connStrings.SectionInformation.Name);
                }
                else
                    Console.WriteLine(
                        "Section {0} is already protected by {1}",
                        connStrings.SectionInformation.Name,
                        connStrings.SectionInformation.ProtectionProvider.Name);

            }
            else
                Console.WriteLine("Can't get the section {0}",
                    connStrings.SectionInformation.Name);

        }


        private void ToggleButton_Checked(object sender, RoutedEventArgs e)
        {

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            _NavigationFrame.Navigate(new Page2());
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            _NavigationFrame.Navigate(new Page1());
        }
    }
}
