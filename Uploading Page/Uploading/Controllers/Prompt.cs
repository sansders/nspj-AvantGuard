using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Layout.Controllers
{
    public static class Prompt
    {
        public static string ShowDialog(string text, string caption)
        {
            Form prompt = new Form()
            {
                Width = 400,
                Height = 250,
                FormBorderStyle = FormBorderStyle.FixedDialog,
                Text = caption,
                StartPosition = FormStartPosition.CenterScreen
            };
            Label textLabel = new Label() { Left = 115, Top = 40, Text = text, Height = 300, Width = 300 };
            TextBox textBox = new TextBox() { Left = 50, Top = 50, Width = 40, Height = 40 };
            Button confirmation = new Button() { Text = "Ok", Left = 150, Width = 100, Top = 100, Height = 40, DialogResult = DialogResult.OK };
            confirmation.Click += (sender, e) => { prompt.Close(); };
            prompt.Controls.Add(confirmation);
            prompt.Controls.Add(textLabel);
            prompt.AcceptButton = confirmation;

            return prompt.ShowDialog() == DialogResult.OK ? textBox.Text : "";
        }

        public static bool hashComparison(string hash1, string hash2)
        {
            if (hash1.Equals(hash2))
            {
                return true;
            }
            else
            {
                return false;
            }

        }
    }
}
