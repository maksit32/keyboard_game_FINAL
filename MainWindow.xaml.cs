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
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;




namespace keyboard_prj
{
   


    public partial class MainWindow : Window
    {
        public System.Windows.Threading.DispatcherTimer timer1 { get; set; }

        public MainWindow()
        {
            InitializeComponent();
            menu1.Width = this.Width;
            //default dictionary
            ResourceDictionary rd = new ResourceDictionary();
            rd.Source = new Uri("dictionary_default.xaml", UriKind.Relative);
            Resources = rd;
            //настройка таймера
            timer1 = new System.Windows.Threading.DispatcherTimer();
            timer1.Tick += new EventHandler(timer_Tick);
        }

        #region [Skins]
        private void Skin1_Click(object sender, RoutedEventArgs e)
        {
            
        }

        private void Skin2_Click_1(object sender, RoutedEventArgs e)
        {

        }

        private void Skin3_Click_2(object sender, RoutedEventArgs e)
        {

        }
        #endregion

        #region [Animations]
        private void Start_b_MouseEnter(object sender, MouseEventArgs e)
        {
            Animation_button(sender, e);
        }

        private void End_b_MouseEnter_1(object sender, MouseEventArgs e)
        {
            Animation_button(sender,e);
        }
        
        private void Animation_button(object sender, MouseEventArgs e)
        {
            var anime = new DoubleAnimation();
            anime.From = 0;
            anime.To = 1;
            anime.Duration = TimeSpan.FromMilliseconds(2000);
            (sender as Button).BeginAnimation(OpacityProperty, anime);
        }
        #endregion

        #region [Timer]
        private void Start_b_Click(object sender, RoutedEventArgs e)
        {
            //отключим старый
            if(timer1.IsEnabled)
            {
                timer1.Stop();
            }
            //включим новый
            if(slider_d.Value > 0)
            {
                timer1.Interval = new TimeSpan(0, 0, (int)slider_d.Value);
                timer1.Start();
            }
            else
            {
                MessageBox.Show("Установите сложность слайдером!", "Внимание!", MessageBoxButton.OK, MessageBoxImage.Warning);
            }    
        }

        private void End_b_Click(object sender, RoutedEventArgs e)
        {
            //выключаем
            if(timer1.IsEnabled)
            {
                timer1.Stop();
            }
        }

        private void timer_Tick(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }
        #endregion
    }
}
