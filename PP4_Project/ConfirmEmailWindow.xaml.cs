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

namespace PP4_Project
{
    /// <summary>
    /// Логика взаимодействия для ConfirmEmailWindow.xaml
    /// </summary>
    public partial class ConfirmEmailWindow : Window
    {
        private static bool flag{ get; set; }
        public static bool Flag
        {
            get
            {
                return flag;
            }
        }

        public ConfirmEmailWindow()
        {
            InitializeComponent();
        }

        private void Accept_Click(object sender, RoutedEventArgs e)
        {
            int valCode;

            if(Int32.TryParse(ConfirmBox.Text, out valCode))
            {
                if (RegisterWindow.Code == valCode)
                {
                    flag = true;
                    Close();
                }
                else
                {
                    flag = false;
                    MessageBox.Show("Неверный код подтверждения");
                    Close();
                }
            }
        }
    }
}
