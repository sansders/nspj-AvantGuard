using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
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

namespace NSPJProject
{
    /// <summary>
    /// Interaction logic for LoginPage.xaml
    /// </summary>
    public partial class LoginPage : Page
    {
        public LoginPage()
        {
            InitializeComponent();
        }

        public static string GetSha512FromString(string strData)
        {
            var message = Encoding.ASCII.GetBytes(strData.Insert(2, "026620758babadb008ee7b98e1bb07351f08d49228c15f6f31c4ee75cb9a26f5079b81c01f14f78cf5f9639e49d7319ee3c3fcc1f94e686b8d605c93f2ab9fb4"));
            SHA512Managed hashString = new SHA512Managed();
            string hex = "";

            
            var hashValue = hashString.ComputeHash(message);
            var hashValue1 = hashString.ComputeHash(hashValue);
            var hashValue2 = hashString.ComputeHash(hashValue1);
            var hashValue3 = hashString.ComputeHash(hashValue2);
            var hashValue4 = hashString.ComputeHash(hashValue3);
            var hashValue5 = hashString.ComputeHash(hashValue4);
            var hashValue6 = hashString.ComputeHash(hashValue5);
            var hashValue7 = hashString.ComputeHash(hashValue6);
            var hashValue8 = hashString.ComputeHash(hashValue7);
            var hashValue9 = hashString.ComputeHash(hashValue8);
            var hashValue10 = hashString.ComputeHash(hashValue9);

            var hashValue11 = hashString.ComputeHash(hashValue10);
            var hashValue12 = hashString.ComputeHash(hashValue11);
            var hashValue13 = hashString.ComputeHash(hashValue12);
            var hashValue14 = hashString.ComputeHash(hashValue13);
            var hashValue15 = hashString.ComputeHash(hashValue14);
            var hashValue16 = hashString.ComputeHash(hashValue15);
            var hashValue17 = hashString.ComputeHash(hashValue16);
            var hashValue18 = hashString.ComputeHash(hashValue17);
            var hashValue19 = hashString.ComputeHash(hashValue18);
            var hashValue20 = hashString.ComputeHash(hashValue19);

            var hashValue21 = hashString.ComputeHash(hashValue20);
            var hashValue22 = hashString.ComputeHash(hashValue21);
            var hashValue23 = hashString.ComputeHash(hashValue22);
            var hashValue24 = hashString.ComputeHash(hashValue23);
            var hashValue25 = hashString.ComputeHash(hashValue24);
            var hashValue26 = hashString.ComputeHash(hashValue25);
            var hashValue27 = hashString.ComputeHash(hashValue26);
            var hashValue28 = hashString.ComputeHash(hashValue27);
            var hashValue29 = hashString.ComputeHash(hashValue28);
            var hashValue30 = hashString.ComputeHash(hashValue29);

            var hashValue31 = hashString.ComputeHash(hashValue30);
            var hashValue32 = hashString.ComputeHash(hashValue31);
            var hashValue33 = hashString.ComputeHash(hashValue32);
            var hashValue34 = hashString.ComputeHash(hashValue33);
            var hashValue35 = hashString.ComputeHash(hashValue34);
            var hashValue36 = hashString.ComputeHash(hashValue35);
            var hashValue37 = hashString.ComputeHash(hashValue36);
            var hashValue38 = hashString.ComputeHash(hashValue37);
            var hashValue39 = hashString.ComputeHash(hashValue38);
            var hashValue40 = hashString.ComputeHash(hashValue39);

            var hashValue41 = hashString.ComputeHash(hashValue40);
            var hashValue42 = hashString.ComputeHash(hashValue41);
            var hashValue43 = hashString.ComputeHash(hashValue42);
            var hashValue44 = hashString.ComputeHash(hashValue43);
            var hashValue45 = hashString.ComputeHash(hashValue44);
            var hashValue46 = hashString.ComputeHash(hashValue45);
            var hashValue47 = hashString.ComputeHash(hashValue46);
            var hashValue48 = hashString.ComputeHash(hashValue47);
            var hashValue49 = hashString.ComputeHash(hashValue48);
            var hashValue50 = hashString.ComputeHash(hashValue49);

            var hashValue51 = hashString.ComputeHash(hashValue50);
            var hashValue52 = hashString.ComputeHash(hashValue51);
            var hashValue53 = hashString.ComputeHash(hashValue52);
            var hashValue54 = hashString.ComputeHash(hashValue53);
            var hashValue55 = hashString.ComputeHash(hashValue54);
            var hashValue56 = hashString.ComputeHash(hashValue55);
            var hashValue57 = hashString.ComputeHash(hashValue56);
            var hashValue58 = hashString.ComputeHash(hashValue57);
            var hashValue59 = hashString.ComputeHash(hashValue58);
            var hashValue60 = hashString.ComputeHash(hashValue59);

            var hashValue61 = hashString.ComputeHash(hashValue60);
            var hashValue62 = hashString.ComputeHash(hashValue61);
            var hashValue63 = hashString.ComputeHash(hashValue62);
            var hashValue64 = hashString.ComputeHash(hashValue63);
            var hashValue65 = hashString.ComputeHash(hashValue64);
            var hashValue66 = hashString.ComputeHash(hashValue65);
            var hashValue67 = hashString.ComputeHash(hashValue66);
            var hashValue68 = hashString.ComputeHash(hashValue67);
            var hashValue69 = hashString.ComputeHash(hashValue68);
            var hashValue70 = hashString.ComputeHash(hashValue69);

            var hashValue71 = hashString.ComputeHash(hashValue70);
            var hashValue72 = hashString.ComputeHash(hashValue71);
            var hashValue73 = hashString.ComputeHash(hashValue72);
            var hashValue74 = hashString.ComputeHash(hashValue73);
            var hashValue75 = hashString.ComputeHash(hashValue74);
            var hashValue76 = hashString.ComputeHash(hashValue75);
            var hashValue77 = hashString.ComputeHash(hashValue76);
            var hashValue78 = hashString.ComputeHash(hashValue77);
            var hashValue79 = hashString.ComputeHash(hashValue78);
            var hashValue80 = hashString.ComputeHash(hashValue79);

            var hashValue81 = hashString.ComputeHash(hashValue80);
            var hashValue82 = hashString.ComputeHash(hashValue81);
            var hashValue83 = hashString.ComputeHash(hashValue82);
            var hashValue84 = hashString.ComputeHash(hashValue83);
            var hashValue85 = hashString.ComputeHash(hashValue84);
            var hashValue86 = hashString.ComputeHash(hashValue85);
            var hashValue87 = hashString.ComputeHash(hashValue86);
            var hashValue88 = hashString.ComputeHash(hashValue87);
            var hashValue89 = hashString.ComputeHash(hashValue88);
            var hashValue90 = hashString.ComputeHash(hashValue89);

            var hashValue91 = hashString.ComputeHash(hashValue90);
            var hashValue92 = hashString.ComputeHash(hashValue91);
            var hashValue93 = hashString.ComputeHash(hashValue92);
            var hashValue94 = hashString.ComputeHash(hashValue93);
            var hashValue95 = hashString.ComputeHash(hashValue94);
            var hashValue96 = hashString.ComputeHash(hashValue95);
            var hashValue97 = hashString.ComputeHash(hashValue96);
            var hashValue98 = hashString.ComputeHash(hashValue97);
            var hashValue99 = hashString.ComputeHash(hashValue98);


            foreach (byte x in hashValue99)
            {
                hex += String.Format("{0:x2}", x);
            }
           

            return hex;
        }

        private void ButtonNext_Click(object sender, RoutedEventArgs e)
        {
            PasswordTextBox.Password = GetSha512FromString(PasswordTextBox.Password);
            MessageBox.Show(GetSha512FromString(PasswordTextBox.Password));
        }

        private void ButtonSignUp_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new Uri(@"SignUp1.xaml", UriKind.RelativeOrAbsolute));
        }

        private void ButtonForgetPass_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new Uri(@"ForgotPassword1.xaml", UriKind.RelativeOrAbsolute));
        }
    }
}
