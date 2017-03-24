using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace RATS
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class Register : Window
    {

        private MainWindow mainWindow;
        public Register(MainWindow _main)
        {
            InitializeComponent();
            mainWindow = _main;
        }

        private void button1_Click(object sender, RoutedEventArgs e) //Back Button
        {
            mainWindow.Show();
            Close();
        }

        private void button_Click(object sender, RoutedEventArgs e) //Submit button
        {
            Boolean passed = true;
            Boolean exception = true;


            if (!(userBox.Text.Length < 21 && userBox.Text.Length > 1))
            {
                MessageBox.Show("Username must be less than 20 characters and (lol good try) greater than 0");
                passed = false;
            }
            if (!(passwordBox.Password.Length > 7 && passwordBox.Password.Length < 21))
            {
                MessageBox.Show("Passwords must be between 8 and 20 characters");
                passed = false;
            }
            if (!(passwordBox.Password == passwordBox1.Password))
            {
                MessageBox.Show("Passwords do not match");
                passed = false;
            }
            if (!(emailBox.Text.Contains("@radford.edu")))
            {                
                MessageBox.Show("Must enter a valid Radford email address");
                passed = false;
            }

            if (passed == true) // Make Query to DB here
            {
                RATSsetTableAdapters.USERSTableAdapter Adapter = new RATS.RATSsetTableAdapters.USERSTableAdapter();

                try
                {
                    Adapter.Register(userBox.Text.ToLower(), passwordBox.Password, 0, 0, emailBox.Text.ToLower());
                }
                catch (Oracle.ManagedDataAccess.Client.OracleException ex)
                {
                    MessageBox.Show("User already exists!");
                    exception = false;
                }

                if (exception)
                {
                    MessageBox.Show("User created!");
                    mainWindow.Show();
                    Close();
                }

                //Thread.Sleep(2000);

                //mainWindow.Show();
                //Close();

            }

        }
    }
}
