﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Documents.Serialization;
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
        private int sec;
        private Random rand = new Random(); //рандом
        private List<string> list_buttons; //массив клавиш
        private Result result = new Result { fail = 0, score = 0 };


        public MainWindow()
        {
            InitializeComponent();
            menu1.Width = this.Width;
            //default dictionary
            ResourceDictionary rd = new ResourceDictionary();
            rd.Source = new Uri("\\Skins\\dictionary_default.xaml", UriKind.Relative);
            Resources = rd;
            //настройка таймера
            timer1 = new System.Windows.Threading.DispatcherTimer();
            timer1.Tick += new EventHandler(timer_Tick);

            //привязка slider и difficulty
            Binding b = new Binding();
            b.ElementName = "slider_d";
            b.Path = new PropertyPath("Value"); // свойство элемента-источника
            label_d.SetBinding(Label.ContentProperty, b);

            create_list();
        }

        #region [Skins]
        private void Skin1_Click(object sender, RoutedEventArgs e)
        {
            ResourceDictionary rd = new ResourceDictionary();
            rd.Source = new Uri("\\Skins\\skin1.xaml", UriKind.Relative);
            Resources = rd;
        }

        private void Skin2_Click_1(object sender, RoutedEventArgs e)
        {
            ResourceDictionary rd = new ResourceDictionary();
            rd.Source = new Uri("\\Skins\\skin2.xaml", UriKind.Relative);
            Resources = rd;
        }

        private void Skin3_Click_2(object sender, RoutedEventArgs e)
        {
            ResourceDictionary rd = new ResourceDictionary();
            rd.Source = new Uri("\\Skins\\skin3.xaml", UriKind.Relative);
            Resources = rd;
        }
        #endregion

        #region [Animations]
        private void Start_b_MouseEnter(object sender, MouseEventArgs e)
        {
            Animation_button(sender, e);
        }

        private void End_b_MouseEnter_1(object sender, MouseEventArgs e)
        {
            Animation_button(sender, e);
        }

        private void Animation_button(object sender, MouseEventArgs e)
        {
            var anime = new DoubleAnimation();
            anime.From = 0;
            anime.To = 1;
            anime.Duration = TimeSpan.FromMilliseconds(2000);
            (sender as Button).BeginAnimation(OpacityProperty, anime);
        }

        private void mi_0_MouseEnter(object sender, MouseEventArgs e)
        {
            (sender as MenuItem).Background = Brushes.Yellow;
        }

        private void mi_1_MouseEnter(object sender, MouseEventArgs e)
        {
            (sender as MenuItem).Background = Brushes.Red;
        }

        private void mi_2_MouseEnter(object sender, MouseEventArgs e)
        {
            (sender as MenuItem).Background = Brushes.Green;
        }
        #endregion

        #region [Timer]
        private void Start_b_Click(object sender, RoutedEventArgs e)
        {
            string value;
            bool IsNormal = false;
            int interval;
            //отключим старый
            if (timer1.IsEnabled)
            {
                timer1.Stop();
            }
            //в строку
            value = slider_d.Value.ToString();
            //проверка на целое число
            if (value.Length > 2)
            {
                IsNormal = false;
            }
            else { IsNormal = true; }

            //включим новый
            if (slider_d.Value > 0 && IsNormal)
            {
                interval = (int)slider_d.Value - 6;
                if (interval < 0)
                {
                    interval = -interval;
                }
                timer1.Interval = new TimeSpan(0, 0, interval);
                sec = 20;
                var result = MessageBox.Show($"Таймер установлен!{Environment.NewLine}Нажмите OK{Environment.NewLine}Успехов! :)", "", MessageBoxButton.OK, MessageBoxImage.Information);
                if (result == MessageBoxResult.OK)
                {
                    timer1.Start();
                }
            }
            else
            {
                MessageBox.Show($"Установите сложность слайдером!{Environment.NewLine}Внимание! Значение должно быть целочисленным!", "Внимание!", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void End_b_Click(object sender, RoutedEventArgs e)
        {
            //выключаем
            if (timer1.IsEnabled)
            {
                timer1.Stop();
                MessageBox.Show("Тамер выключен.", "", MessageBoxButton.OK, MessageBoxImage.Information);
                this.time_left.Content = $"Time left: 0 sec";
            }
        }

        private void timer_Tick(object sender, EventArgs e)
        {
            sec--;
            if (sec <= 0)
            {
                this.text_label.Content = $"Игра окончена :)";
                this.time_left.Content = $"Time left: 0 sec";
                timer1.Stop();
                return;
            }
            this.text_label.Content = $"Нажмите: {list_buttons[rand.Next(this.list_buttons.Count)]}";
            this.time_left.Content = $"Time left: {sec} sec";
        }
        #endregion

        //for menu
        private void Window_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            this.menu1.Width = this.Width;
        }
        private void create_list()
        {
            this.list_buttons = new List<string>
            {
                "`", "1", "2", "3", "4", "5", "6", "7", "8", "9", "0", "-", "=", "Backspace",
                "Tab", "q", "w", "e", "r", "t", "y", "u", "i", "o", "p", "[", "]", "\\",
                "Caps lock", "a", "s", "d", "f", "g", "h", "j", "k", "l", ";", "'", "Enter",
                "Shift", "z", "x", "c", "v", "b", "n", "m", ",", ".", "/", "Shift",
                "Ctrl", "Win", "Alt", "Space", "Alt", "Win", "Ctrl"
            };
        }

        private void Rules_item_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show($"Игра-тренажер.{Environment.NewLine}С помощью данного тренажера вы сможете улучшить навык обращения с клавиатурой.{Environment.NewLine}Выбирая сложность(difficulty) вы сможете регулировать время, дающееся на нажатие.{Environment.NewLine}10 - 1 секунда, 9 - 2 секунды и т.д.{Environment.NewLine}После успешного нажатия вам начислится score, равный 20.{Environment.NewLine}Если вы ошиблись, очки не начислятся, а отметка fail увеличится на 1", "Правила", MessageBoxButton.OK, MessageBoxImage.Information);
        }
    }
}
