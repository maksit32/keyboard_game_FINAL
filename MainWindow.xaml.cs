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
using System.Windows.Documents.Serialization;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Text.Json;
using System.IO;
using System.Diagnostics;

namespace keyboard_prj
{

    public partial class MainWindow : Window
    {
        #region [Data]
        public System.Windows.Threading.DispatcherTimer timer1 { get; set; }
        private int sec;
        private Random rand = new Random(); //рандом
        private Dictionary<string, string> dict_buttons; //массив клавиш
        private User result = new User { fail = 0, score = 0, Name = "" };
        private bool IsPlaying = false;
        private bool IsPressed = false;
        private bool FirstEnter = true;
        private bool MusicPlay = true;
        public List<bool> XScore { get; set; } = new List<bool> { }; //проверка для увеличения score
        public List<User> lst_users { get; set; } = new List<User> { };
        public MediaPlayer mplayer { get; set; } = new MediaPlayer();
        #endregion

        #region [Construct]
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

        private void keyboard_game_Loaded(object sender, RoutedEventArgs e)
        {
            enable_rb.IsChecked = true;
            //media
            mplayer.Open(new Uri("..\\..\\Music\\game_music.mp3", UriKind.Relative));
            mplayer.MediaEnded += MusicReplay;
            mplayer.Play();
        }

        private void MusicReplay(object sender, EventArgs e)
        {
            mplayer.Stop();
            mplayer.Play();
        }

        //for menu
        private void Window_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            this.menu1.Width = this.Width;
        }
        private void create_list()
        {
            //словарь (клавиша в label, клавиша в keys(enum))
            this.dict_buttons = new Dictionary<string, string>
            {
                {"`", "Oem3" }, { "1", "D1" }, { "2", "D2" }, { "3", "D3" }, { "4", "D4" }, { "5", "D5" }, { "6", "D6" }, { "7", "D7" }, { "8", "D8" }, { "9", "D9" }, { "0", "D0" }, { "-", "OemMinus" }, { "+", "OemPlus" }, { "Backspace", "Back" },
                { "Tab", "Tab" }, { "q", "Q" }, { "w", "W" }, { "e", "E" }, { "r", "R" }, { "t", "T" }, { "y", "Y" }, { "u", "U" }, { "i", "I" }, { "o", "O" }, { "p", "P" }, { "[", "Oem4" }, { "]", "Oem6" }, { "\\", "Oem5" },
                { "Caps lock", "CapsLock" }, { "a", "A" }, { "s", "S" }, { "d", "D" }, { "f", "F" }, { "g", "G" }, { "h", "H" }, { "j", "J" }, { "k", "K" }, { "l", "L" }, { ";", "OemSemicolon" }, { "'", "Oem7" }, { "Enter", "Enter" },
                { "Shift", "LeftShift" }, { "z", "Z" }, { "x", "X" }, { "c", "C" }, { "v", "V" }, { "b", "B" }, { "n", "N" }, { "m", "M" }, { ",", "OemComma" }, { ".", "OemPeriod" }, { "/", "Oem2" },
                { "Ctrl", "LeftCtrl" }, { "Win", "LWin" }, { "Alt", "LeftAlt" }, { "Space", "Space" }
            };
        }

        //запись (сереализация)
        private void keyboard_game_Closed(object sender, EventArgs e)
        {
            if (result.Name != "")
            {
                try
                {
                    //сереализация
                    using (FileStream fs = new FileStream("..\\..\\Data\\players.json", FileMode.OpenOrCreate))
                    {
                        JsonSerializer.Serialize<User>(fs, result); //куда, что
                    }
                }
                catch
                {
                    MessageBox.Show("Ошибка записи!", "", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void music_click(object sender, RoutedEventArgs e)
        {
            BitmapImage logo = new BitmapImage();
            if (MusicPlay)
            {
                mplayer.Stop();
                logo.BeginInit();
                logo.UriSource = new Uri("..\\..\\Icons\\start.ico", UriKind.Relative);
                logo.EndInit();
                image_sound.Source = logo;
                tb_music.Text = "Включить музыку";
                MusicPlay = false;
            }
            else
            {
                mplayer.Play();
                logo.BeginInit();
                logo.UriSource = new Uri("..\\..\\Icons\\stop.ico", UriKind.Relative);
                logo.EndInit();
                image_sound.Source = logo;
                tb_music.Text = "Выключить музыку";
                MusicPlay = true;
            }
        }
        #endregion

        #region [Skins]
        private void Skin1_Click(object sender, RoutedEventArgs e)
        {
            ResourceDictionary rd = new ResourceDictionary();
            rd.Source = new Uri("\\Skins\\skin1.xaml", UriKind.Relative);
            Resources = rd;

            //меняем background у других
            mi_1.Background = Brushes.White;
            mi_2.Background = Brushes.White;
            mi_3.Background = Brushes.White;
        }

        private void Skin2_Click(object sender, RoutedEventArgs e)
        {
            ResourceDictionary rd = new ResourceDictionary();
            rd.Source = new Uri("\\Skins\\skin2.xaml", UriKind.Relative);
            Resources = rd;
            mi_0.Background = Brushes.White;
            mi_2.Background = Brushes.White;
            mi_3.Background = Brushes.White;
        }

        private void Skin3_Click(object sender, RoutedEventArgs e)
        {
            ResourceDictionary rd = new ResourceDictionary();
            rd.Source = new Uri("\\Skins\\skin3.xaml", UriKind.Relative);
            Resources = rd;
            mi_0.Background = Brushes.White;
            mi_1.Background = Brushes.White;
            mi_3.Background = Brushes.White;
        }

        private void Skin4_Click(object sender, RoutedEventArgs e)
        {
            ResourceDictionary rd = new ResourceDictionary();
            rd.Source = new Uri("\\Skins\\dictionary_default.xaml", UriKind.Relative);
            Resources = rd;
            mi_0.Background = Brushes.White;
            mi_1.Background = Brushes.White;
            mi_2.Background = Brushes.White;
        }

        private async void mi_0_MouseLeave(object sender, MouseEventArgs e)
        {

            await Task.Delay(1000);
            mi_0.Background = Brushes.White;
            mi_1.Background = Brushes.White;
            mi_2.Background = Brushes.White;
            mi_3.Background = Brushes.White;
        }

        private async void mi_1_MouseLeave(object sender, MouseEventArgs e)
        {
            await Task.Delay(1000);
            mi_0.Background = Brushes.White;
            mi_1.Background = Brushes.White;
            mi_2.Background = Brushes.White;
            mi_3.Background = Brushes.White;
        }

        private async void mi_2_MouseLeave(object sender, MouseEventArgs e)
        {
            await Task.Delay(1000);
            mi_0.Background = Brushes.White;
            mi_1.Background = Brushes.White;
            mi_2.Background = Brushes.White;
            mi_3.Background = Brushes.White;
        }
        private async void mi_3_MouseLeave(object sender, MouseEventArgs e)
        {
            await Task.Delay(1000);
            mi_0.Background = Brushes.White;
            mi_1.Background = Brushes.White;
            mi_2.Background = Brushes.White;
            mi_3.Background = Brushes.White;
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
        //тип 2
        private void Animation_button2(object sender, MouseEventArgs e)
        {
            var anime = new DoubleAnimation();
            anime.From = 0;
            anime.To = (sender as Button).Width;
            anime.Duration = TimeSpan.FromMilliseconds(130);
            (sender as Button).BeginAnimation(WidthProperty, anime);
            var anime2 = new DoubleAnimation();
            anime2.From = 0;
            anime2.To = (sender as Button).Height;
            anime2.Duration = TimeSpan.FromMilliseconds(130);
            (sender as Button).BeginAnimation(HeightProperty, anime2);
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

        private void mi_3_MouseEnter(object sender, MouseEventArgs e)
        {
            (sender as MenuItem).Background = Brushes.Blue;
        }
        #endregion

        #region [Timer]
        private void Start_b_Click(object sender, RoutedEventArgs e)
        {
            if (result.Name == "")
            {
                MessageBox.Show("Пожалуйста, введите имя!", "", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            string value;
            bool IsNormal = false;
            int interval;
            FirstEnter = true;
            //обнулили результат
            result.fail = 0;
            result.score = 0;
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
                    IsPlaying = true;
                    timer1.Start();
                    this.prg_bar.Value = 0;
                    slider_d.IsEnabled = false;
                    this.XScore.Clear();
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
                prg_bar.Value = 0;
                slider_d.IsEnabled = true;
                MessageBox.Show("Таймер выключен.", "", MessageBoxButton.OK, MessageBoxImage.Information);
                IsPlaying = false;
                this.time_left.Content = $"Времени осталось: 0 сек";
                this.text_label.Content = "Клавиши, которые нужно нажать будут отображаться тут.";
            }
        }
        //новая задача
        private void timer_Tick(object sender, EventArgs e)
        {
            //если пользователь проигнорил нажатие
            //защита от 2 нажатия + первый вход
            if (!IsPressed && !FirstEnter)
            {
                result.fail++;
                this.fail_label.Content = $"Ошибок:  {result.fail}";
                XScore.Add(false);
            }
            IsPressed = false;
            sec--;
            if (sec <= 0)
            {
                this.text_label.Content = $"Игра окончена :)";
                this.time_left.Content = $"Времени осталось: 0 сек";
                IsPlaying = false;
                slider_d.IsEnabled = true;
                timer1.Stop();
                return;
            }
            this.text_label.Content = $"Нажмите: {dict_buttons.ElementAt(rand.Next(dict_buttons.Count)).Key}";
            this.time_left.Content = $"Времени осталось: {sec} сек";
            this.score_label.Content = $"Очков:  {result.score}";
            this.fail_label.Content = $"Ошибок:  {result.fail}";
            prg_bar.Value++;
            FirstEnter = false;
        }
        #endregion

        #region [Settings]
        private void slider_d_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            //округляю
            int difficulty = Convert.ToInt32((sender as Slider).Value);
            //возвращаем целое число
            (sender as Slider).Value = difficulty;
            //выставляем скорость
            switch (difficulty)
            {
                case 0:
                    {
                        this.speed_label.Content = $"Скорость: 0 char/s ";
                    }
                    break;
                case 1:
                    {
                        this.speed_label.Content = $"Скорость: 0,2 char/s ";
                    }
                    break;
                case 2:
                    {
                        this.speed_label.Content = $"Скорость: 0,25 char/s ";
                    }
                    break;
                case 3:
                    {
                        this.speed_label.Content = $"Скорость: 0,33 char/s ";
                    }
                    break;
                case 4:
                    {
                        this.speed_label.Content = $"Скорость: 0,5 char/s ";
                    }
                    break;
                case 5:
                    {
                        this.speed_label.Content = $"Скорость: 1 char/s ";
                    }
                    break;
            }
        }

        private void name_menuitem_click(object sender, RoutedEventArgs e)
        {
            //открытие формы для заполения имени
            Name_form form = new Name_form();
            form.ShowDialog();
            if (form.Get_IsCorrect())
            {
                //записываем имя
                this.result.Name = form.name_textbox.Text;
                this.name_label.Content = $"Ваше имя:  {result.Name}";
                //красим эллипс
                if (result.Name != "")
                {
                    this.ellipce_item.Fill = Brushes.Red;
                }
            }
            else
            {
                this.ellipce_item.Fill = Brushes.Black;
                result.Name = "";
            }
        }

        private void Rules_item_Click(object sender, RoutedEventArgs e)
        {
            var animation = new DoubleAnimation();
            animation.From = 0;
            animation.To = 1;
            animation.Duration = TimeSpan.FromMilliseconds(1000);
            this.Rules_item.BeginAnimation(OpacityProperty, animation);
            MessageBox.Show($"Игра-тренажер.{Environment.NewLine}С помощью данного тренажера вы сможете улучшить навык обращения с клавиатурой.{Environment.NewLine}Выбирая сложность вы сможете регулировать время, дающееся на нажатие.{Environment.NewLine}5 - 1 секунда, 4 - 2 секунды и т.д.{Environment.NewLine}После успешного нажатия вам начислится очки, равные 20 (либо 40 если вы указали верно 3 раза подряд).{Environment.NewLine}Если вы ошиблись, очки не начислятся, а отметка 'Ошибки' увеличится на 1. {Environment.NewLine}Обязательно заполните имя! (кружок загорится красным, когда имя будет указано верно.", "Правила", MessageBoxButton.OK, MessageBoxImage.Information);
        }
        private void Skins_item_Click(object sender, RoutedEventArgs e)
        {
            var animation = new DoubleAnimation();
            animation.From = 0;
            animation.To = 1;
            animation.Duration = TimeSpan.FromMilliseconds(1000);
            this.Skins_item.BeginAnimation(OpacityProperty, animation);
        }
        private void social_mi_click(object sender, RoutedEventArgs e)
        {
            Process.Start(new ProcessStartInfo("https://e.mail.ru/cgi-bin/sentmsg?To=kornilovmaks2001111@mail.ru&from=otvet") { UseShellExecute = true });
        }

        private void restart_mi_Click(object sender, RoutedEventArgs e)
        {
            System.Diagnostics.Process.Start(Application.ResourceAssembly.Location);
            Application.Current.Shutdown();
        }
        #endregion

        #region [Radiobuttons]
        private void decline_rb_Checked(object sender, RoutedEventArgs e)
        {
            buttons_canvas.IsEnabled = false;
        }

        private void enable_rb_Checked(object sender, RoutedEventArgs e)
        {
            buttons_canvas.IsEnabled = true;
        }
        #endregion

        #region [Game]
        private void keyboard_game_KeyDown_WPF(object sender, KeyEventArgs e)
        {
            if (IsPlaying && !IsPressed) //не нажата
            {
                int index = 0;
                //ищу индекс клавиши из label и key в dictionary
                foreach (var item in dict_buttons.Keys)
                {
                    if (text_label.Content.ToString().ToLower().Contains(item.ToLower()))
                    {
                        break;
                    }
                    index++;
                }
                //сравниваю e.key и слово по индексу
                if (dict_buttons.ElementAt(index).Value.ToLower() == e.Key.ToString().ToLower())
                {
                    XScore.Add(true);
                    if (XScore.Count < 3)
                    {
                        result.score += 20;
                    }
                    else
                    {
                        for (int i = XScore.Count; i > XScore.Count - 3; i--)
                        {
                            if (XScore[i - 1] == false)
                            {
                                result.score += 20;
                                goto link1;
                            }
                        }
                        result.score += 40;
                    }
                link1:
                    this.score_label.Content = $"Очков:  {result.score}";

                }
                else
                {
                    result.fail++;
                    this.fail_label.Content = $"Ошибок:  {result.fail}";
                    XScore.Add(false);
                }
                IsPressed = true; //нажат
            }
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            KeyEventArgs e_ = new KeyEventArgs(Keyboard.PrimaryDevice, PresentationSource.FromDependencyObject(this), 0, Key.Oem3);
            keyboard_game_KeyDown_WPF(sender, e_);
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            KeyEventArgs e_ = new KeyEventArgs(Keyboard.PrimaryDevice, PresentationSource.FromDependencyObject(this), 0, Key.D1);
            keyboard_game_KeyDown_WPF(sender, e_);
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            KeyEventArgs e_ = new KeyEventArgs(Keyboard.PrimaryDevice, PresentationSource.FromDependencyObject(this), 0, Key.D2);
            keyboard_game_KeyDown_WPF(sender, e_);
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            KeyEventArgs e_ = new KeyEventArgs(Keyboard.PrimaryDevice, PresentationSource.FromDependencyObject(this), 0, Key.D3);
            keyboard_game_KeyDown_WPF(sender, e_);
        }

        private void Button_Click_4(object sender, RoutedEventArgs e)
        {
            KeyEventArgs e_ = new KeyEventArgs(Keyboard.PrimaryDevice, PresentationSource.FromDependencyObject(this), 0, Key.D4);
            keyboard_game_KeyDown_WPF(sender, e_);
        }

        private void Button_Click_5(object sender, RoutedEventArgs e)
        {
            KeyEventArgs e_ = new KeyEventArgs(Keyboard.PrimaryDevice, PresentationSource.FromDependencyObject(this), 0, Key.D5);
            keyboard_game_KeyDown_WPF(sender, e_);
        }

        private void Button_Click_6(object sender, RoutedEventArgs e)
        {
            KeyEventArgs e_ = new KeyEventArgs(Keyboard.PrimaryDevice, PresentationSource.FromDependencyObject(this), 0, Key.D6);
            keyboard_game_KeyDown_WPF(sender, e_);
        }

        private void Button_Click_7(object sender, RoutedEventArgs e)
        {
            KeyEventArgs e_ = new KeyEventArgs(Keyboard.PrimaryDevice, PresentationSource.FromDependencyObject(this), 0, Key.D7);
            keyboard_game_KeyDown_WPF(sender, e_);
        }

        private void Button_Click_8(object sender, RoutedEventArgs e)
        {
            KeyEventArgs e_ = new KeyEventArgs(Keyboard.PrimaryDevice, PresentationSource.FromDependencyObject(this), 0, Key.D8);
            keyboard_game_KeyDown_WPF(sender, e_);
        }

        private void Button_Click_9(object sender, RoutedEventArgs e)
        {
            KeyEventArgs e_ = new KeyEventArgs(Keyboard.PrimaryDevice, PresentationSource.FromDependencyObject(this), 0, Key.D9);
            keyboard_game_KeyDown_WPF(sender, e_);
        }

        private void Button_Click_10(object sender, RoutedEventArgs e)
        {
            KeyEventArgs e_ = new KeyEventArgs(Keyboard.PrimaryDevice, PresentationSource.FromDependencyObject(this), 0, Key.D0);
            keyboard_game_KeyDown_WPF(sender, e_);
        }

        private void Button_Click_11(object sender, RoutedEventArgs e)
        {
            KeyEventArgs e_ = new KeyEventArgs(Keyboard.PrimaryDevice, PresentationSource.FromDependencyObject(this), 0, Key.OemMinus);
            keyboard_game_KeyDown_WPF(sender, e_);
        }

        private void Button_Click_12(object sender, RoutedEventArgs e)
        {
            KeyEventArgs e_ = new KeyEventArgs(Keyboard.PrimaryDevice, PresentationSource.FromDependencyObject(this), 0, Key.OemPlus);
            keyboard_game_KeyDown_WPF(sender, e_);
        }

        private void Button_Click_13(object sender, RoutedEventArgs e)
        {
            KeyEventArgs e_ = new KeyEventArgs(Keyboard.PrimaryDevice, PresentationSource.FromDependencyObject(this), 0, Key.Back);
            keyboard_game_KeyDown_WPF(sender, e_);
        }

        private void Button_Click_14(object sender, RoutedEventArgs e)
        {
            KeyEventArgs e_ = new KeyEventArgs(Keyboard.PrimaryDevice, PresentationSource.FromDependencyObject(this), 0, Key.Tab);
            keyboard_game_KeyDown_WPF(sender, e_);
        }

        private void Button_Click_15(object sender, RoutedEventArgs e)
        {
            KeyEventArgs e_ = new KeyEventArgs(Keyboard.PrimaryDevice, PresentationSource.FromDependencyObject(this), 0, Key.Q);
            keyboard_game_KeyDown_WPF(sender, e_);
        }

        private void Button_Click_16(object sender, RoutedEventArgs e)
        {
            KeyEventArgs e_ = new KeyEventArgs(Keyboard.PrimaryDevice, PresentationSource.FromDependencyObject(this), 0, Key.W);
            keyboard_game_KeyDown_WPF(sender, e_);
        }

        private void Button_Click_17(object sender, RoutedEventArgs e)
        {
            KeyEventArgs e_ = new KeyEventArgs(Keyboard.PrimaryDevice, PresentationSource.FromDependencyObject(this), 0, Key.E);
            keyboard_game_KeyDown_WPF(sender, e_);
        }

        private void Button_Click_18(object sender, RoutedEventArgs e)
        {
            KeyEventArgs e_ = new KeyEventArgs(Keyboard.PrimaryDevice, PresentationSource.FromDependencyObject(this), 0, Key.R);
            keyboard_game_KeyDown_WPF(sender, e_);
        }

        private void Button_Click_19(object sender, RoutedEventArgs e)
        {
            KeyEventArgs e_ = new KeyEventArgs(Keyboard.PrimaryDevice, PresentationSource.FromDependencyObject(this), 0, Key.T);
            keyboard_game_KeyDown_WPF(sender, e_);
        }

        private void Button_Click_20(object sender, RoutedEventArgs e)
        {
            KeyEventArgs e_ = new KeyEventArgs(Keyboard.PrimaryDevice, PresentationSource.FromDependencyObject(this), 0, Key.Y);
            keyboard_game_KeyDown_WPF(sender, e_);
        }

        private void Button_Click_21(object sender, RoutedEventArgs e)
        {
            KeyEventArgs e_ = new KeyEventArgs(Keyboard.PrimaryDevice, PresentationSource.FromDependencyObject(this), 0, Key.U);
            keyboard_game_KeyDown_WPF(sender, e_);
        }

        private void Button_Click_22(object sender, RoutedEventArgs e)
        {
            KeyEventArgs e_ = new KeyEventArgs(Keyboard.PrimaryDevice, PresentationSource.FromDependencyObject(this), 0, Key.I);
            keyboard_game_KeyDown_WPF(sender, e_);
        }

        private void Button_Click_23(object sender, RoutedEventArgs e)
        {
            KeyEventArgs e_ = new KeyEventArgs(Keyboard.PrimaryDevice, PresentationSource.FromDependencyObject(this), 0, Key.O);
            keyboard_game_KeyDown_WPF(sender, e_);
        }

        private void Button_Click_24(object sender, RoutedEventArgs e)
        {
            KeyEventArgs e_ = new KeyEventArgs(Keyboard.PrimaryDevice, PresentationSource.FromDependencyObject(this), 0, Key.P);
            keyboard_game_KeyDown_WPF(sender, e_);
        }

        private void Button_Click_25(object sender, RoutedEventArgs e)
        {
            KeyEventArgs e_ = new KeyEventArgs(Keyboard.PrimaryDevice, PresentationSource.FromDependencyObject(this), 0, Key.Oem4);
            keyboard_game_KeyDown_WPF(sender, e_);
        }

        private void Button_Click_26(object sender, RoutedEventArgs e)
        {
            KeyEventArgs e_ = new KeyEventArgs(Keyboard.PrimaryDevice, PresentationSource.FromDependencyObject(this), 0, Key.Oem6);
            keyboard_game_KeyDown_WPF(sender, e_);
        }

        private void Button_Click_27(object sender, RoutedEventArgs e)
        {
            KeyEventArgs e_ = new KeyEventArgs(Keyboard.PrimaryDevice, PresentationSource.FromDependencyObject(this), 0, Key.Oem5);
            keyboard_game_KeyDown_WPF(sender, e_);
        }

        private void Button_Click_28(object sender, RoutedEventArgs e)
        {
            KeyEventArgs e_ = new KeyEventArgs(Keyboard.PrimaryDevice, PresentationSource.FromDependencyObject(this), 0, Key.CapsLock);
            keyboard_game_KeyDown_WPF(sender, e_);
        }

        private void Button_Click_29(object sender, RoutedEventArgs e)
        {
            KeyEventArgs e_ = new KeyEventArgs(Keyboard.PrimaryDevice, PresentationSource.FromDependencyObject(this), 0, Key.A);
            keyboard_game_KeyDown_WPF(sender, e_);
        }

        private void Button_Click_30(object sender, RoutedEventArgs e)
        {
            KeyEventArgs e_ = new KeyEventArgs(Keyboard.PrimaryDevice, PresentationSource.FromDependencyObject(this), 0, Key.S);
            keyboard_game_KeyDown_WPF(sender, e_);
        }

        private void Button_Click_31(object sender, RoutedEventArgs e)
        {
            KeyEventArgs e_ = new KeyEventArgs(Keyboard.PrimaryDevice, PresentationSource.FromDependencyObject(this), 0, Key.D);
            keyboard_game_KeyDown_WPF(sender, e_);
        }

        private void Button_Click_32(object sender, RoutedEventArgs e)
        {
            KeyEventArgs e_ = new KeyEventArgs(Keyboard.PrimaryDevice, PresentationSource.FromDependencyObject(this), 0, Key.F);
            keyboard_game_KeyDown_WPF(sender, e_);
        }

        private void Button_Click_33(object sender, RoutedEventArgs e)
        {
            KeyEventArgs e_ = new KeyEventArgs(Keyboard.PrimaryDevice, PresentationSource.FromDependencyObject(this), 0, Key.G);
            keyboard_game_KeyDown_WPF(sender, e_);
        }

        private void Button_Click_34(object sender, RoutedEventArgs e)
        {
            KeyEventArgs e_ = new KeyEventArgs(Keyboard.PrimaryDevice, PresentationSource.FromDependencyObject(this), 0, Key.H);
            keyboard_game_KeyDown_WPF(sender, e_);
        }

        private void Button_Click_35(object sender, RoutedEventArgs e)
        {
            KeyEventArgs e_ = new KeyEventArgs(Keyboard.PrimaryDevice, PresentationSource.FromDependencyObject(this), 0, Key.J);
            keyboard_game_KeyDown_WPF(sender, e_);
        }

        private void Button_Click_36(object sender, RoutedEventArgs e)
        {
            KeyEventArgs e_ = new KeyEventArgs(Keyboard.PrimaryDevice, PresentationSource.FromDependencyObject(this), 0, Key.K);
            keyboard_game_KeyDown_WPF(sender, e_);
        }

        private void Button_Click_37(object sender, RoutedEventArgs e)
        {
            KeyEventArgs e_ = new KeyEventArgs(Keyboard.PrimaryDevice, PresentationSource.FromDependencyObject(this), 0, Key.L);
            keyboard_game_KeyDown_WPF(sender, e_);
        }

        private void Button_Click_38(object sender, RoutedEventArgs e)
        {
            KeyEventArgs e_ = new KeyEventArgs(Keyboard.PrimaryDevice, PresentationSource.FromDependencyObject(this), 0, Key.OemSemicolon);
            keyboard_game_KeyDown_WPF(sender, e_);
        }

        private void Button_Click_39(object sender, RoutedEventArgs e)
        {
            KeyEventArgs e_ = new KeyEventArgs(Keyboard.PrimaryDevice, PresentationSource.FromDependencyObject(this), 0, Key.Oem7);
            keyboard_game_KeyDown_WPF(sender, e_);
        }

        private void Button_Click_40(object sender, RoutedEventArgs e)
        {
            KeyEventArgs e_ = new KeyEventArgs(Keyboard.PrimaryDevice, PresentationSource.FromDependencyObject(this), 0, Key.Enter);
            keyboard_game_KeyDown_WPF(sender, e_);
        }

        private void Button_Click_41(object sender, RoutedEventArgs e)
        {
            KeyEventArgs e_ = new KeyEventArgs(Keyboard.PrimaryDevice, PresentationSource.FromDependencyObject(this), 0, Key.LeftShift);
            keyboard_game_KeyDown_WPF(sender, e_);
        }

        private void Button_Click_42(object sender, RoutedEventArgs e)
        {
            KeyEventArgs e_ = new KeyEventArgs(Keyboard.PrimaryDevice, PresentationSource.FromDependencyObject(this), 0, Key.Z);
            keyboard_game_KeyDown_WPF(sender, e_);
        }

        private void Button_Click_43(object sender, RoutedEventArgs e)
        {
            KeyEventArgs e_ = new KeyEventArgs(Keyboard.PrimaryDevice, PresentationSource.FromDependencyObject(this), 0, Key.X);
            keyboard_game_KeyDown_WPF(sender, e_);
        }

        private void Button_Click_44(object sender, RoutedEventArgs e)
        {
            KeyEventArgs e_ = new KeyEventArgs(Keyboard.PrimaryDevice, PresentationSource.FromDependencyObject(this), 0, Key.C);
            keyboard_game_KeyDown_WPF(sender, e_);
        }

        private void Button_Click_45(object sender, RoutedEventArgs e)
        {
            KeyEventArgs e_ = new KeyEventArgs(Keyboard.PrimaryDevice, PresentationSource.FromDependencyObject(this), 0, Key.V);
            keyboard_game_KeyDown_WPF(sender, e_);
        }

        private void Button_Click_46(object sender, RoutedEventArgs e)
        {
            KeyEventArgs e_ = new KeyEventArgs(Keyboard.PrimaryDevice, PresentationSource.FromDependencyObject(this), 0, Key.B);
            keyboard_game_KeyDown_WPF(sender, e_);
        }

        private void Button_Click_47(object sender, RoutedEventArgs e)
        {
            KeyEventArgs e_ = new KeyEventArgs(Keyboard.PrimaryDevice, PresentationSource.FromDependencyObject(this), 0, Key.N);
            keyboard_game_KeyDown_WPF(sender, e_);
        }

        private void Button_Click_48(object sender, RoutedEventArgs e)
        {
            KeyEventArgs e_ = new KeyEventArgs(Keyboard.PrimaryDevice, PresentationSource.FromDependencyObject(this), 0, Key.M);
            keyboard_game_KeyDown_WPF(sender, e_);
        }

        private void Button_Click_49(object sender, RoutedEventArgs e)
        {
            KeyEventArgs e_ = new KeyEventArgs(Keyboard.PrimaryDevice, PresentationSource.FromDependencyObject(this), 0, Key.OemComma);
            keyboard_game_KeyDown_WPF(sender, e_);
        }

        private void Button_Click_50(object sender, RoutedEventArgs e)
        {
            KeyEventArgs e_ = new KeyEventArgs(Keyboard.PrimaryDevice, PresentationSource.FromDependencyObject(this), 0, Key.OemPeriod);
            keyboard_game_KeyDown_WPF(sender, e_);
        }

        private void Button_Click_51(object sender, RoutedEventArgs e)
        {
            KeyEventArgs e_ = new KeyEventArgs(Keyboard.PrimaryDevice, PresentationSource.FromDependencyObject(this), 0, Key.Oem2);
            keyboard_game_KeyDown_WPF(sender, e_);
        }

        private void Button_Click_52(object sender, RoutedEventArgs e)
        {
            KeyEventArgs e_ = new KeyEventArgs(Keyboard.PrimaryDevice, PresentationSource.FromDependencyObject(this), 0, Key.LeftShift);
            keyboard_game_KeyDown_WPF(sender, e_);
        }

        private void Button_Click_53(object sender, RoutedEventArgs e)
        {
            KeyEventArgs e_ = new KeyEventArgs(Keyboard.PrimaryDevice, PresentationSource.FromDependencyObject(this), 0, Key.LeftCtrl);
            keyboard_game_KeyDown_WPF(sender, e_);
        }

        private void Button_Click_54(object sender, RoutedEventArgs e)
        {
            KeyEventArgs e_ = new KeyEventArgs(Keyboard.PrimaryDevice, PresentationSource.FromDependencyObject(this), 0, Key.LWin);
            keyboard_game_KeyDown_WPF(sender, e_);
        }

        private void Button_Click_55(object sender, RoutedEventArgs e)
        {
            KeyEventArgs e_ = new KeyEventArgs(Keyboard.PrimaryDevice, PresentationSource.FromDependencyObject(this), 0, Key.LeftAlt);
            keyboard_game_KeyDown_WPF(sender, e_);
        }

        private void Button_Click_56(object sender, RoutedEventArgs e)
        {
            KeyEventArgs e_ = new KeyEventArgs(Keyboard.PrimaryDevice, PresentationSource.FromDependencyObject(this), 0, Key.Space);
            keyboard_game_KeyDown_WPF(sender, e_);
        }

        private void Button_Click_57(object sender, RoutedEventArgs e)
        {
            KeyEventArgs e_ = new KeyEventArgs(Keyboard.PrimaryDevice, PresentationSource.FromDependencyObject(this), 0, Key.LeftAlt);
            keyboard_game_KeyDown_WPF(sender, e_);
        }

        private void Button_Click_58(object sender, RoutedEventArgs e)
        {
            KeyEventArgs e_ = new KeyEventArgs(Keyboard.PrimaryDevice, PresentationSource.FromDependencyObject(this), 0, Key.LWin);
            keyboard_game_KeyDown_WPF(sender, e_);
        }

        private void Button_Click_59(object sender, RoutedEventArgs e)
        {
            KeyEventArgs e_ = new KeyEventArgs(Keyboard.PrimaryDevice, PresentationSource.FromDependencyObject(this), 0, Key.LeftCtrl);
            keyboard_game_KeyDown_WPF(sender, e_);
        }
        #endregion

    }
}
