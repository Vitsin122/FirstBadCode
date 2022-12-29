using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Net;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Net.Mail;

namespace PP4_Project
{
    public partial class RegisterWindow : Window
    {
        public static int Code;

        public RegisterWindow()
        {
            InitializeComponent();
        }
        private void SignButton_Click(object sender, RoutedEventArgs e)
        {
            new MainWindow().Show();
            Close();
        }
        private bool IsValidEmail(string email)
        {
            try
            {
                MailAddress m = new MailAddress(email);

                return true;
            }
            catch (FormatException)
            {
                return false;
            }
        }
        private bool IsOnlyEmail(string email)
        {
            SqlCommand command = new SqlCommand($"SELECT COUNT(*) FROM UsersT WHERE Email = '{email}'", MainWindow.sqlConnection);

            SqlDataReader reader = command.ExecuteReader();

            reader.Read();

            int count = (int)reader.GetValue(0);

            if (reader != null)
                reader.Close();

            if (count >= 1)
                return false;

            return true;
        }
        private bool IsOnlyLogin(string login)
        {
            SqlCommand command = new SqlCommand($"SELECT COUNT(*) FROM UsersT WHERE Login = '{login}'", MainWindow.sqlConnection);

            SqlDataReader reader = command.ExecuteReader();

            reader.Read();

            int count = (int)reader.GetValue(0);

            if (reader != null)
                reader.Close();

            if (count >= 1)
                return false;

            return true;
        }
        private void RegButton_Click(object sender, RoutedEventArgs e)
        {
            bool flag = true;

            var bc = new BrushConverter();

            if (!IsOnlyEmail(Email.Text) || !IsValidEmail(Email.Text) || Email.Text == "")
            {
                Email.Background = (Brush)bc.ConvertFrom("Red");
                flag = false;
            }
            else if (IsValidEmail(Email.Text) && IsOnlyEmail(Email.Text) && Email.Text != "")
            {
                Email.Background = (Brush)bc.ConvertFrom("White");
            }
            if (Login.Text == "" || !IsOnlyLogin(Login.Text))
            {
                Login.Background = (Brush)bc.ConvertFrom("Red");
                flag = false;
            }
            else if (Login.Text != "" && IsOnlyLogin(Login.Text))
            {
                Login.Background = (Brush)bc.ConvertFrom("White");
            }
            if (Password.Password == "")
            {
                Password.Background = (Brush)bc.ConvertFrom("Red");
                flag = false;
            }
            else
            {
                Password.Background = (Brush)bc.ConvertFrom("White");
            }
            if (ConfirmPassword.Password == "" || ConfirmPassword.Password != Password.Password)
            {
                ConfirmPassword.Background = (Brush)bc.ConvertFrom("Red");
                flag = false;
            }
            else if (ConfirmPassword.Password != "" && ConfirmPassword.Password == Password.Password)
            {
                ConfirmPassword.Background = (Brush)bc.ConvertFrom("White");
            }
            if (Name.Text == "")
            {
                Name.Background = (Brush)bc.ConvertFrom("Red");
                flag = false;
            }
            else
            {
                Name.Background = (Brush)bc.ConvertFrom("White");
            }
            if (LastName.Text == "")
            {
                LastName.Background = (Brush)bc.ConvertFrom("Red");
                flag = false;
            }
            else
            {
                LastName.Background = (Brush)bc.ConvertFrom("White");
            }

            if (flag == true)
            {
                Random rnd = new Random();
                Code = rnd.Next(1000, 2001);
                MailAddress fromAddres = new MailAddress("faren122@mail.ru", "MoneyControlApp");
                MailAddress toAddres = new MailAddress(Email.Text, Login.Text);
                MailMessage message = new MailMessage(fromAddres, toAddres);
                message.Body = Code.ToString();
                message.Subject = "Код подтверждения";

                SmtpClient smtpClient = new SmtpClient("smtp.mail.ru", 25);
                smtpClient.EnableSsl = true;
                smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
                smtpClient.UseDefaultCredentials = false;
                smtpClient.Credentials = new NetworkCredential(fromAddres.Address, "fdEdDb7m7ft45Yhm8Q9c");

                smtpClient.Send(message);

                var ConW = new ConfirmEmailWindow();

                ConW.ShowDialog();             

                if (ConfirmEmailWindow.Flag == true)
                {
                    ConW.Close();
                    SqlCommand sqlCommand = new SqlCommand($"INSERT INTO [UsersT] (Email, Login, Passowrd, Name, Last_Name) VALUES ('{Email.Text}', '{Login.Text}', '{Password.Password}', N'{Name.Text}', N'{LastName.Text}')", MainWindow.sqlConnection);
                    sqlCommand.ExecuteNonQuery();
                    MessageBox.Show("Registration complete");
                    MainWindow.Log = Login.Text;

                    new Profile().Show();
                    Close();
                }
                else
                {
                    MessageBox.Show("Повторите попытку");
                }
            }
        }
    }
}
