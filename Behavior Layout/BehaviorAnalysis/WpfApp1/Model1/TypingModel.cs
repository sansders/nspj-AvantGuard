using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace WpfApp1.Model1
{
    class TypingModel
    {
        private String _paragraphText;

        public TypingModel()
        {
            
          
        }


        public String paragraphText
        {
            get { return _paragraphText; }
            set { _paragraphText = value; }
        }

       private String readTextFromFile()
        {
            String returnString = System.IO.File.ReadAllText(@"..\\..\\Resources\\Paragraph.txt");
            return returnString;
        }

        private StringBuilder convertToStringBuilder(string [] words)
        {
            StringBuilder fullString = new StringBuilder();
            for (int i = 0; i < words.Length; i++)
            {
                fullString.Append(words[i]);
                fullString.Append(' ');
            }
            return fullString;
        }

        public string[] saveTextInArray(StringReader _paragraphText)
        {

            string[] words = _paragraphText.ReadToEnd().Split(' ');
            for(int i = 0; i < words.Length; i++)
            {
                Console.WriteLine(words[i]);
            }
            return words;
        }

        public string[] getTextInArray()
        {
            StringReader currString = new StringReader(readTextFromFile());
            string[] words = saveTextInArray(currString);
            return words;
        }

    }
}
