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


namespace RATS
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// 
    /// @version 0.2.3
    /// 
    /// 
    /// 
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
          
        }
    

        private void comboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void ComboBoxItem_Selected(object sender, RoutedEventArgs e)
        {

        }


        //note for password box - should use password.securePassword to get a secure string of password 
        //entered for better security

        private void loginButton_Click_1(object sender, RoutedEventArgs e)
        {
            
           
            //the developer/backdoor login  is used to test the system without having to install Oracle Developer Tools for 
            //Visual studio in order to save time testing or in order to avoid a sql login
            //Should be removed by final release or done differently 

            if (this.username.Text == "rats" && this.password.Password == "SEisFUN")
            {

                if (comboBox.SelectedIndex == 0)
                {
                    Client student = new Client(this, 1);
                    student.Show();
                    this.Error.Text = "";
                    this.Hide();
                }
                else if (comboBox.SelectedIndex == 1)
                {
                    Client teacher = new Client(this, 2);
                    teacher.Show();
                    this.Error.Text = "";
                    this.Hide();
                }
                else if (comboBox.SelectedIndex == 2)
                {
                    Client admin = new Client(this, 3);
                    admin.Show();
                    this.Error.Text = "";
                    this.Hide();
                }
                //determine if wrong password was used by accident
            }else if(this.username.Text == "rats" && this.password.Password != "SEisFUN")
            {
                MessageBox.Show("Wrong developer password");
            }
            else
            {
                //if not using a backdoor login, then verify credentials with a sql query
                RATSsetTableAdapters.USERSTableAdapter Adapter = new RATS.RATSsetTableAdapters.USERSTableAdapter();

                try
                {
                    if (comboBox.SelectedIndex == 0)
                    {
                        //RATSsetTableAdapters.USERSTableAdapter Adapter = new RATS.RATSsetTableAdapters.USERSTableAdapter();
                        decimal login = (decimal)Adapter.Login(this.username.Text, this.password.Password);

                        if (login == 1)
                        {
                            Client student = new Client(this, 1);
                            student.Show();
                            this.password.Password = "";
                            this.Error.Text = "";
                            this.Hide();
                        }
                        else
                        {
                            decimal checkforuser = (decimal)Adapter.CheckUsername(this.username.Text);
                            if (checkforuser == 0)
                            {
                                this.Error.Text = "User not found";
                            }
                            else
                            {
                                this.Error.Text = "Invalid password or improper user role";
                            }
                        }
                    }
                    else if (comboBox.SelectedIndex == 1)
                    {
                        decimal TLogin = (decimal)Adapter.TeacherLogin(this.username.Text, this.password.Password);
                        if (TLogin == 1)
                        {
                            Client teacher = new Client(this, 2);
                            teacher.Show();
                            this.password.Password = "";
                            this.Error.Text = "";
                            this.Hide();
                        }
                        else
                        {
                            decimal checkforuser = (decimal)Adapter.CheckUsername(this.username.Text);
                            if (checkforuser == 0)
                            {
                                this.Error.Text = "User not found!";
                            }
                            else
                            {
                                this.Error.Text = "Invalid password or improper user role";
                            }
                        }
                    }
                    else if (comboBox.SelectedIndex == 2)
                    {
                        decimal ALogin = (decimal)Adapter.AdminLogin(this.username.Text, this.password.Password);
                        if (ALogin == 1)
                        {
                            Client admin = new Client(this, 3);
                            admin.Show();
                            this.password.Password = "";
                            this.Error.Text = "";
                            this.Hide();
                        }
                        else
                        {
                            decimal checkforuser = (decimal)Adapter.CheckUsername(this.username.Text);
                            if (checkforuser == 0)
                            {
                                this.Error.Text = "User not found!";
                            }
                            else
                            {
                                this.Error.Text = "Invalid password or improper user role";
                            }
                        }
                    }
                }
                catch (NullReferenceException ex) { }
                catch (Oracle.ManagedDataAccess.Client.OracleException ex)
                {
                    MessageBox.Show("You must have Oracle Developer Tools for Visual Studio 2015 installed and \n " +
                                    "afterwards go to C:\\Program Files(x86)\\Oracle Developer Tools for VS2015\\network\\admin\\sqlnet.ora \n" +
                                     "and open sqlnet.ora with notepad with elevated access and set SQLNET.AUTHENTICATION_SERVICES=(NONE) ");
                }

            }
            
        }

        private void quitButton_Click_1(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            Recover recover = new Recover(this);
            recover.Show();
            this.Hide();
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            Register reg = new Register(this);
            reg.Show();
            this.Hide();
        }
    }
}
