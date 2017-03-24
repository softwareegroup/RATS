using System;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;

namespace RATS
{
    /// <summary>
    /// Client Class
    /// @Version 0.2.5
    /// Added loader for buttons, and left/right semi join
    /// </summary>
    public partial class Client : Window
    {
        private MainWindow mainWindow;
        
        public const int student = 1;
        public const int teacher = 2;
        public const int admin = 3;

        private int state = 0;

        public Client(MainWindow mainWindow, int _state)
        {
            this.mainWindow = mainWindow;
            this.state = _state;

            InitializeComponent();
            InitButtons(); 
        }

        //
        // Submit query button click
        //

        
        private void submitBtn_Click(object sender, RoutedEventArgs e)
        {
            string SQL = "";

            string myText2 = new TextRange(richTextBox.Document.ContentStart, richTextBox.Document.ContentEnd).Text;

            string myText =  userInputBox.Text;

            string s = "here's a \"\n\tstring\" to test";

           // MessageBox.Show(GetEscapedString(myText));
            MessageBox.Show(GetEscapedString(myText2));


            Parser parser = new Parser(myText);

            SQL = parser.parseSQL(parser.currentWord);
            textBlock.Text = SQL;
            weboutput.Navigate(new System.Uri("http://php.radford.edu/~jbeebe/query.php?action=" + SQL));//Submit parsed algebra through php script

        }

        //
        // get escape characters from text box
        // From http://stackoverflow.com/questions/1368020/how-to-output-unicode-string-to-rtf-using-c
        //

        static string GetEscapedString(string s)
        {
            var sb = new StringBuilder();
            foreach (var c in s)
            {
                if (c == '\\' || c == '{' || c == '}')
                    sb.Append(@"\" + c);
                else if (c <= 0x7f)
                    sb.Append(c);
                else
                    sb.Append("\\u" + Convert.ToUInt32(c) + "?");
            }
            return sb.ToString();
        }

        //
        // The following can the symbols buttons being inserted into the userInputBox
        //


        private void projectBtn_Click(object sender, RoutedEventArgs e)
        {

            richTextBox.CaretPosition.InsertTextInRun(projectBtn.Content.ToString());

            userInputBox.Text += projectBtn.Content.ToString();
          
        }

       

        private void restrictBtn_Click(object sender, RoutedEventArgs e)
        {

            richTextBox.CaretPosition.InsertTextInRun(restrictBtn.Content.ToString());


            userInputBox.Text += restrictBtn.Content.ToString();

        }

       

        private void joinBtn_Click(object sender, RoutedEventArgs e)
        {
            richTextBox.CaretPosition.InsertTextInRun(joinBtn.Content.ToString());

            userInputBox.Text += joinBtn.Content.ToString();
        }

        private void leftSemiBtn_Click(object sender, RoutedEventArgs e)
        {
            richTextBox.CaretPosition.InsertTextInRun(leftSemiBtn.Content.ToString());

            userInputBox.Text += leftSemiBtn.Content.ToString();
        }

        private void rightSemiBtn_Click(object sender, RoutedEventArgs e)
        {
            richTextBox.CaretPosition.InsertTextInRun(rightSemiBtn.Content.ToString());

            userInputBox.Text += rightSemiBtn.Content.ToString();
        }

        private void antiBtn_Click(object sender, RoutedEventArgs e)
        {
            richTextBox.CaretPosition.InsertTextInRun(antiBtn.Content.ToString());

            userInputBox.Text += antiBtn.Content.ToString();
        }

        private void unionBtn_Click(object sender, RoutedEventArgs e)
        {
            richTextBox.CaretPosition.InsertTextInRun(unionBtn.Content.ToString());

            userInputBox.Text += unionBtn.Content.ToString();
        }

        private void interBtn_Click(object sender, RoutedEventArgs e)
        {
            richTextBox.CaretPosition.InsertTextInRun(interBtn.Content.ToString());

            userInputBox.Text += interBtn.Content.ToString();
        }


        private void logbutton_Click(object sender, RoutedEventArgs e)
        {
            mainWindow.Show();
            Close();
        }

        private void InitButtons()
        {
            projectBtn.Content = "\u03a0"; //code for pi symbol            
            restrictBtn.Content = "\u03c3"; //code for small sigma symbol
            joinBtn.Content = "\u2a1d"; //code for natural join symbol
            leftSemiBtn.Content = "\u22c9"; //code for left semi join symbol
            rightSemiBtn.Content = "\u22CA"; //code for right semi join symbol
            antiBtn.Content = "\u25b7"; //code for antijoin symbol
            unionBtn.Content = "\u222a"; //code for union symbol
            interBtn.Content = "\u2229"; //code for intersection symbol
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
