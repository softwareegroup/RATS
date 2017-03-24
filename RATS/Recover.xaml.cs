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
using System.Windows.Shapes;
//
using System.Net;
using System.Net.Mail;
using System.Net.Mime;
using System.Threading;
using System.ComponentModel;
using System.Data;

namespace RATS
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class Recover : Window
    {
        private MainWindow mainWindow;

        public Recover(MainWindow mainWindow)
        {
            this.mainWindow = mainWindow;
            InitializeComponent();
        }

        private void RecoverPass(string email)
        {
            RATSsetTableAdapters.USERSTableAdapter Adapter = new RATS.RATSsetTableAdapters.USERSTableAdapter();
            decimal remail = (decimal)Adapter.CheckEmail(email);

            if (remail == 1)
            {
                String password = "";
                DataTable getpass = Adapter.Recover(email);

                foreach (DataRow row in getpass.Rows)
                {
                    password = row["password"].ToString();
                }

                //smtp code from http://csharp.net-informations.com/communications/csharp-smtp-mail.htm
                try
                {
                    MailMessage mail = new MailMessage();
                    SmtpClient SmtpServer = new SmtpClient("smtp.gmail.com");

                    mail.From = new MailAddress("ruratslab@gmail.com");
                    mail.To.Add(email);
                    mail.Subject = "RATS Recover password.";
                    mail.Body = "Your password for R.A.T.S is " + password;

                    SmtpServer.Port = 587;
                    SmtpServer.Credentials = new System.Net.NetworkCredential("ruratslab", "softwareeng2");
                    SmtpServer.EnableSsl = true;

                    SmtpServer.Send(mail);
                    MessageBox.Show("Recovery password sent.");
                    mainWindow.Show();
                    Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                }
            }else
            {
                MessageBox.Show("Email does not exist or is incorrectly typed.");
            }
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            string email = this.emailBox.Text;
            RecoverPass(email);
        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            mainWindow.Show();
            Close();
        }
    }
}
