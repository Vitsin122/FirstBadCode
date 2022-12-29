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
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Reflection;

namespace PP4_Project
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>


    public partial class MainWindow : Window
    {
        public static SqlConnection sqlConnection = null;
        public static string Log;
        public static int ID;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Reister_Click(object sender, RoutedEventArgs e)
        {
            new RegisterWindow().Show();

            Close();
        }

        private void Window_Initialized(object sender, EventArgs e)
        {
            sqlConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["UsersKey"].ConnectionString);

            sqlConnection.Open();
        }

        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {

            SqlCommand command = new SqlCommand("SELECT Login, Passowrd FROM UsersT", MainWindow.sqlConnection);

            SqlDataReader dataReader = command.ExecuteReader();

            bool flag = false;

            if (dataReader.HasRows)
            {
                while (dataReader.Read())
                {
                    if (Login.Text == (string)dataReader.GetValue(0) && Pass.Password == (string)dataReader.GetValue(1))
                    {
                        Log = Login.Text;

                        if (dataReader != null && !dataReader.IsClosed)
                            dataReader.Close();

                        flag = true;
                        break;
                    }
                }

                if (flag)
                {
                    MessageBox.Show("Log in complete");
                    
                    new Profile().Show();
                    Close();
                }
                else
                {
                    if (dataReader != null && !dataReader.IsClosed)
                        dataReader.Close();

                    MessageBox.Show("Login or Password is incorrected");
                }
            }
            else
                MessageBox.Show("Login or Password is incorrected");

            if (dataReader != null && !dataReader.IsClosed)
                dataReader.Close();
        }
    }
}
