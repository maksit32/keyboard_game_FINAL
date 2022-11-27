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

namespace keyboard_prj
{
    
    public partial class Name_form : Window
    {
        private bool IsCorrect = false;
        public Name_form()
        {
            InitializeComponent();
            ResourceDictionary rd = new ResourceDictionary();
            rd.Source = new Uri("\\Skins\\name_dictionary.xaml", UriKind.Relative);
            Resources = rd;
        }

        private void name_textbox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            int size = 0;
            foreach (var item in e.Text)
            {
                size++;
            }

            if(size == 0)
            {
                IsCorrect = false;
            }

            for (int i = 0; i < size; i++)
            {
                //отсеиваем цифры в имени
                if (e.Text[i] >= '0' && e.Text[i] <= '9')
                {
                    IsCorrect = false;
                    break;
                }
                else
                {
                    IsCorrect = true;
                }
            }
        }

        public bool Get_IsCorrect()
        {
            return IsCorrect;
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            if(!IsCorrect)
            {
                MessageBox.Show("Имя введено неправильно. Пожалуйста, исправьте его.", "ВНИМАНИЕ!", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }
    }
}
