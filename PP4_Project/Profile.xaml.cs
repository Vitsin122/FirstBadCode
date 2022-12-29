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
using System.Data.SqlClient;
using System.Text.RegularExpressions;
using System.Media;
using System.Net;
using System.Xml.Linq;
using System.ComponentModel;
using Microsoft.Win32;
using System.Reflection;
using System.IO;
using System.Data;
using System.Globalization;
using PP4_Project.Table1;

namespace PP4_Project
{
    /// <summary>
    /// Логика взаимодействия для Profile.xaml
    /// </summary>
    public partial class Profile : Window
    {

        private BindingList<differentiated_payment> _differentiated_payment;

        public Profile()
        {
            InitializeComponent();
        }
        private string? UserLogin { get; set; }
        private BindingList<ItemDataClass> itemDataClass;
        private void Window_Initialized(object sender, EventArgs e)
        {

            Valuta.Items.Add("USD");
            Valuta.Items.Add("EURO");

            SqlCommand command = new SqlCommand($"SELECT Name, Last_Name, Login FROM UsersT WHERE Login = '{MainWindow.Log}'", MainWindow.sqlConnection);

            SqlDataReader reader = command.ExecuteReader();

            if (reader.HasRows)
            {
                reader.Read();

                Name.Text = Convert.ToString(reader.GetValue(0));
                LastName.Text = Convert.ToString(reader.GetValue(1));
                LoginT.Text = Convert.ToString(reader.GetValue(2));
                UserLogin = Convert.ToString(reader.GetValue(2));
            }

            if (reader != null && !reader.IsClosed)
                reader.Close();

            command.CommandText = $"SELECT Avatar FROM UsersT WHERE Login = '{UserLogin}' AND Avatar IS NOT NULL";

            if (command != null)
            {
                SqlDataReader sqlData = command.ExecuteReader();
                sqlData.Read();

                if (sqlData.HasRows)
                {
                    var image = new BitmapImage();
                    using (var mem = new MemoryStream((byte[])sqlData.GetValue(0)))
                    {
                        mem.Position = 0;
                        image.BeginInit();
                        image.CreateOptions = BitmapCreateOptions.PreservePixelFormat;
                        image.CacheOption = BitmapCacheOption.OnLoad;
                        image.UriSource = null;
                        image.StreamSource = mem;
                        image.EndInit();
                    }
                    image.Freeze();
                    ProfileImage.Source = image;
                }

                sqlData.Close();
            }
        }

        private void Accept1_Click(object sender, RoutedEventArgs e)
        {
            string INSERT = String.Format($"UPDATE UsersT SET Name = N'{Name.Text}' WHERE Login = N'{UserLogin}'");

            SqlCommand command = new SqlCommand(INSERT, MainWindow.sqlConnection);

            command.ExecuteNonQuery();

            MessageBox.Show("Имя изменено!");
        }

        private void Accept2_Click(object sender, RoutedEventArgs e)
        {
            string INSERT = String.Format($"UPDATE UsersT SET Last_Name = N'{LastName.Text}' WHERE Login = N'{UserLogin}'");

            SqlCommand command = new SqlCommand(INSERT, MainWindow.sqlConnection);

            command.ExecuteNonQuery();

            MessageBox.Show("Фамилия изменена!");
        }

        private void Accept3_Click(object sender, RoutedEventArgs e)
        {
            string INSERT = String.Format($"UPDATE UsersT SET Login = '{LoginT.Text}' WHERE Login = N'{UserLogin}'");

            SqlCommand command = new SqlCommand(INSERT, MainWindow.sqlConnection);

            command.ExecuteNonQuery();

            MessageBox.Show("Логин обновлён!");
        }
        private void CalcButton_Click(object sender, RoutedEventArgs e)
        {
            if (CreditCombo?.SelectedItem?.ToString() != null)
            {

                if (CreditCombo.SelectedItem.ToString() == "Аунитетный рассчёт")
                {
                    double Sum1 = Convert.ToDouble(Total_Amount.Text);
                    double Bet1 = Convert.ToDouble(Bet.Text) / 100;
                    double Term1 = Convert.ToDouble(Term.Text);

                    double x = (((Sum1 / Term1) * Bet1) + (Sum1 / Term1));

                    Contribution.Text = x.ToString();
                }
                else if (CreditCombo.SelectedItem.ToString() == "Дифференцированный рассчёт")
                {
                    double Sum1 = Convert.ToDouble(Total_Amount.Text);
                    double Bet1 = Convert.ToDouble(Bet.Text) / 100;
                    double Term1 = Convert.ToDouble(Term.Text);

                    int Term2 = Convert.ToInt32(Term1);

                    double Sum3 = Sum1;
                    double Term3 = Term1;

                    double x = 0.0;
                    Table taskWindow = new Table();
                    _differentiated_payment = new BindingList<differentiated_payment>();
                    for (int i = 0; i < Term2; i++)
                    {
                        x = ((Sum3 / Term3) + (Sum3 * Bet1 * (31.0 / 366.0)));
                        _differentiated_payment.Add(new differentiated_payment(i + 1, x));
                        Term3--;
                        Sum3 -= (Sum1 / Term2);
                    }
                    taskWindow.paymentGrid.ItemsSource = _differentiated_payment;
                    taskWindow.Show();

                    Contribution.Text = x.ToString();
                }
            }
            else
            {
                MessageBox.Show("Введите метод рассчёта");
            }
        }

        private void ValueBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                WebClient client = new WebClient();
                var xml = client.DownloadString("https://www.cbr-xml-daily.ru/daily.xml");
                XDocument xdoc = XDocument.Parse(xml);
                var el = xdoc.Element("ValCurs").Elements("Valute");
                string dollar = el.Where(x => x.Attribute("ID").Value == "R01235").Select(x => x.Element("Value").Value).FirstOrDefault();
                string eur = el.Where(x => x.Attribute("ID").Value == "R01239").Select(x => x.Element("Value").Value).FirstOrDefault();

                if (Valuta.Text != "")
                {
                    if (Valuta.Text == "USD")
                    {
                        if (ValueBox.Text != "")
                            RubBlock.Text = (double.Parse(dollar) * double.Parse(ValueBox.Text)).ToString();
                    }
                    else if (Valuta.Text == "EURO")
                    {
                        if (ValueBox.Text != "")
                            RubBlock.Text = (double.Parse(eur) * double.Parse(ValueBox.Text)).ToString();
                    }
                }
                else
                {
                    ValueBox.Text = "";
                    RubBlock.Text = "";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Some error. Please, check correct data");
                ValueBox.Text = "";
                RubBlock.Text = "";
            }
        }

        public class ItemDataClass : INotifyPropertyChanged
        {
            private string? _Object;
            private string? _Price;

            public string? Object
            {
                get { return _Object; }
                set { _Object = value; }
            }

            public string? Price
            {
                get { return _Price; }
                set { _Price = value; }
            }

            public event PropertyChangedEventHandler? PropertyChanged;

            protected virtual void OnPropertyChanged(string propertyName = "")
            {
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            itemDataClass = new BindingList<ItemDataClass>() {};
            
            dgSpend.ItemsSource = itemDataClass;

            itemDataClass.ListChanged += ItemDataClass_ListChanged;

            CreditCombo.Items.Add("Аунитетный рассчёт");
            CreditCombo.Items.Add("Дифференцированный рассчёт");

            SqlCommand command = new SqlCommand($"SELECT Money FROM UsersT WHERE Login = '{UserLogin}'", MainWindow.sqlConnection);

            SqlDataReader reader = command.ExecuteReader();

            if (reader.HasRows)
            {
                reader.Read();
                string str = reader.GetDecimal(0).ToString();
                if (reader != null)
                    reader.Close();
                MoneyBox.Text = str;
            }
        }

        private void ItemDataClass_ListChanged(object? sender, ListChangedEventArgs e)
        {
            switch (e.ListChangedType)
            {
                case ListChangedType.Reset:
                    break;
                case ListChangedType.ItemAdded:
                    double price;
                    ItemDataClass data = ((BindingList<ItemDataClass>)sender).Last<ItemDataClass>();
                    if (data != null && double.TryParse(data.Price, out price))
                    {
                        MoneyBox.Text = (double.Parse(MoneyBox.Text) + price).ToString();
                    }
                    break;
                case ListChangedType.ItemDeleted:
                    break;
                case ListChangedType.ItemMoved:
                    break;
                case ListChangedType.ItemChanged:
                    break;
                case ListChangedType.PropertyDescriptorAdded:
                    break;
                case ListChangedType.PropertyDescriptorDeleted:
                    break;
                case ListChangedType.PropertyDescriptorChanged:
                    break;
                default:
                    break;
            }
        }

        private void ChangeImageButton_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Image files (*.png;*.jpeg;*.jpg)|*.png;*.jpeg;*.jpg";
            if (openFileDialog.ShowDialog() == true)
            {
                byte[] image_bytes = File.ReadAllBytes(openFileDialog.FileName);
                SqlCommand command = new SqlCommand();
                command.Connection = MainWindow.sqlConnection;
                command.CommandText = $"UPDATE UsersT SET Avatar = @ImageData WHERE Login = '{UserLogin}'";
                command.Parameters.Add("@ImageData", SqlDbType.Image, 1000000);
                command.Parameters["@ImageData"].Value = image_bytes;
                
                command.ExecuteNonQuery();

                command.CommandText = String.Format($"SELECT Avatar FROM UsersT WHERE Login = '{UserLogin}'");

                SqlDataReader sqlDataReader = command.ExecuteReader();

                if (sqlDataReader.HasRows == true)
                {
                    var image = new BitmapImage();
                    using (var mem = new MemoryStream(image_bytes))
                    {
                        mem.Position = 0;
                        image.BeginInit();
                        image.CreateOptions = BitmapCreateOptions.PreservePixelFormat;
                        image.CacheOption = BitmapCacheOption.OnLoad;
                        image.UriSource = null;
                        image.StreamSource = mem;
                        image.EndInit();
                    }
                    image.Freeze();

                    ProfileImage.Source = image;
                }

            };
        }

        private void cmbGrid_Loaded(object sender, RoutedEventArgs e)
        {
            ComboBox cmb = (ComboBox)sender;
            cmb.Items.Add("USD");
            cmb.Items.Add("EURO");
            cmb.Items.Add("RUB");
        }

        private void CreditCombo_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            CalcButton.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));
        }

        private void MoneyBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            Decimal val;
            Decimal.TryParse(MoneyBox.Text.Replace(",", "").Replace(".", ""), NumberStyles.Number, CultureInfo.InvariantCulture, out val);
            val /= 10000;
            SqlCommand command = new SqlCommand($"UPDATE UsersT SET Money = '{val}' WHERE Login = '{UserLogin}'", MainWindow.sqlConnection);
            command.ExecuteNonQuery();
        }

        private void Accept4_Click(object sender, RoutedEventArgs e)
        {
            if (PassBoox.Text != "")
            {
                SqlCommand command = new SqlCommand($"UPDATE UsersT SET Passowrd = '{PassBoox.Text}' WHERE Login = '{UserLogin}'", MainWindow.sqlConnection);
                command.ExecuteNonQuery();
                MessageBox.Show("Пароль изменён");
            }
            else
                MessageBox.Show("Введите пароль");
        }
    }
}
