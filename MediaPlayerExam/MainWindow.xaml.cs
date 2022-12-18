using Microsoft.Win32;
using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Xml;

namespace MediaPlayerExam
{
    public partial class MainWindow : Window
    {
        public System.Windows.Threading.DispatcherTimer timer = new System.Windows.Threading.DispatcherTimer();

        public string[] pathMovies = new string[100];

        public int indexMovie = 0;

        public bool offVolume;
        public bool fullSizeMovie;
        public bool pauseVideo;

        public bool englishLanguage;
        public bool ukrainianLanguage;
        public MainWindow()
        {
            System.Threading.Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo(Properties.Settings.Default.languageCode);
            if(Properties.Settings.Default.languageCode == "En")
            {
                englishLanguage = true;
                ukrainianLanguage = false;
            }
            if (Properties.Settings.Default.languageCode == "Ua")
            {
                ukrainianLanguage = true;
                englishLanguage = false;
            }
                
            InitializeComponent();
            timer.Interval = new TimeSpan(0, 0, 1);
            timer.Tick += MovieTimer_Tick;

        }
        #region Functions
        public void OpenMovieDialog()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = " Files | *.mp4; *.mp3;";
            openFileDialog.Multiselect = true;
            if (openFileDialog.ShowDialog() == true)
            {

                pathMovies = openFileDialog.FileNames;
                mediaElement.Source = new Uri(pathMovies[0]);
                listCurrentFiles.ItemsSource = pathMovies;


                if(englishLanguage == true)
                listCurrentFilesLabel.Content = $"All files: " + pathMovies.Length.ToString();
                else
                    listCurrentFilesLabel.Content = $"Всього файлів: " + pathMovies.Length.ToString();


                timer.Stop();
                mediaElement.Stop();
                if (pathMovies[0].Contains(".mp4"))
                {
                    mediaElement.SetValue(Grid.ColumnProperty, 1);
                    imageUGrid.SetValue(Grid.ColumnProperty, 0);
                }
                if (pathMovies[0].Contains(".mp3"))
                {
                    mediaElement.SetValue(Grid.ColumnProperty, 0);
                    imageUGrid.SetValue(Grid.ColumnProperty, 1);
                }
                Title = Path.GetFileNameWithoutExtension(Path.GetFileNameWithoutExtension(pathMovies[0]));
            }
        }
        private void CheckFormatFile()
        {
            if (pathMovies[indexMovie].Contains(".mp4"))
            {
                imageUGrid.SetValue(Grid.ColumnProperty, 0);
                mediaElement.SetValue(Grid.ColumnProperty, 1);
            }
            if (pathMovies[indexMovie].Contains(".mp3"))
            {
                mediaElement.SetValue(Grid.ColumnProperty, 0);
                imageUGrid.SetValue(Grid.ColumnProperty, 1);
            }
        }
    
        #endregion

        #region Timer movie
        private void MovieTimer_Tick(object sender, EventArgs e)
        {
            currentTimeMovieText.Text = mediaElement.Position.Hours.ToString() + ":" + mediaElement.Position.Minutes.ToString() + ":" + mediaElement.Position.Seconds.ToString();
            durationSliderMovie.Value = mediaElement.Position.TotalSeconds;
        }
        #endregion

        #region Buttons
        public void PlayMovie()
        {
            if (mediaElement.Source != null)
            {
                mediaElement.Play();
                if (ukrainianLanguage == true)
                operationInfoText.Text = "Відтворення...";
                else
                    operationInfoText.Text = "Play...";
                timer.Start();
            }
            else
            {
                OpenMovieDialog();
            }
        }
        public void PauseMovie()
        {
            mediaElement.Pause();
            if (ukrainianLanguage == true)
                operationInfoText.Text = "Пауза...";
            else
                operationInfoText.Text = "Pause...";
            timer.Stop();
        }
        public void StopMovie()
        {
            mediaElement.Stop();
            if (ukrainianLanguage == true)
                operationInfoText.Text = "Стоп...";
            else
                operationInfoText.Text = "Stop...";
            timer.Stop();
        }

        public void RetryMovie()
        {
            timer.Stop();
            mediaElement.Stop();
            PlayMovie();
        }
        public void BackSound()
        {
            try
            {
                indexMovie--;
                mediaElement.Source = new Uri(pathMovies[indexMovie]);
            }
            catch
            {
                indexMovie = 0;
            }
            timer.Stop();
            PlayMovie();
            Title = Path.GetFileNameWithoutExtension(Path.GetFileNameWithoutExtension(pathMovies[indexMovie]));
            CheckFormatFile();
        }
        public void NextSound()
        {
            try
            {
                indexMovie++;
                mediaElement.Source = new Uri(pathMovies[indexMovie]);

            }
            catch
            {
                indexMovie = 0;
            }
            timer.Stop();
            PlayMovie();
            Title = Path.GetFileNameWithoutExtension(Path.GetFileNameWithoutExtension(pathMovies[indexMovie]));
            CheckFormatFile();
        }
        public void Back5Sec()
        {
            mediaElement.Position -= TimeSpan.FromSeconds(5);
           
        }
        public void Next5Sec()
        {
            mediaElement.Position += TimeSpan.FromSeconds(5);
        }
        public void OffVolume()
        {
            string pathVolume = @"volume.png";
            string pathVolumeOff = @"volumeOff.png";
            if (offVolume == true)
            {
                mediaElement.Volume = 0.5;
                sliderVolume.IsEnabled = true;
                offVolume = false;
                offVoiceBtn.Background = new ImageBrush { ImageSource = new BitmapImage(new Uri(pathVolume, UriKind.Relative)) };
            }
            else
            {
                mediaElement.Volume = 0;
                sliderVolume.IsEnabled = false;
                offVolume = true;
                offVoiceBtn.Background = new ImageBrush { ImageSource = new BitmapImage(new Uri(pathVolumeOff, UriKind.Relative)) };
            }
        }


        private void startMovieBtn_Click(object sender, RoutedEventArgs e)
        {
            PlayMovie();
        }

        private void pauseMovieBtn_Click(object sender, RoutedEventArgs e)
        {
            PauseMovie();
        }

        private void stopMovieBtn_Click(object sender, RoutedEventArgs e)
        {
            StopMovie();
        }

        private void retryMovieBtn_Click(object sender, RoutedEventArgs e)
        {
            RetryMovie();
        }

        private void backSoundBtn_Click(object sender, RoutedEventArgs e)
        {
            BackSound();
        }

        private void back5SecBtn_Click(object sender, RoutedEventArgs e)
        {
            Back5Sec();
        }

        private void next5SecBtn_Click(object sender, RoutedEventArgs e)
        {
            Next5Sec();
        }

        private void nextSoundBtn_Click(object sender, RoutedEventArgs e)
        {
            NextSound();
        }

        private void offVoiceBtn_Click(object sender, RoutedEventArgs e)
        {
            OffVolume();
        }
        #endregion

        #region Sider
        private void sliderVolume_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            mediaElement.Volume = sliderVolume.Value;
        }
        private void durationSliderMovie_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            mediaElement.Position = TimeSpan.FromSeconds(durationSliderMovie.Value);
        }

        #endregion

        #region Media
        private void mediaElement_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Middle)
            {
                if (fullSizeMovie == false)
                {
                    grid.Children.Remove(UGridOperations);
                    grid.Children.Remove(UGridButtons);
                    grid.Children.Remove(UGridDuration);
                    grid.Children.Remove(UGridDurationSlider);
                    grid.Children.Remove(UGridMenu);
                    grid.Children.Remove(movieUGrid);
                    this.Content = movieUGrid;
                    this.WindowState = WindowState.Maximized;
                    fullSizeMovie = true;
                }
                else
                {
                    this.Content = grid;
                    grid.Children.Add(UGridOperations);
                    grid.Children.Add(UGridButtons);
                    grid.Children.Add(UGridDuration);
                    grid.Children.Add(UGridDurationSlider);
                    grid.Children.Add(UGridMenu);
                    grid.Children.Add(movieUGrid);
                    this.WindowState = WindowState.Normal;
                    fullSizeMovie = false;
                }
            }
            if (e.ChangedButton == MouseButton.Left)
            {
                if (pauseVideo == false)
                {
                    PauseMovie();
                    pauseVideo = true;
                }
                else
                {
                    PlayMovie();
                    pauseVideo = false;
                }
            }
        }
        private void mediaElement_MediaOpened(object sender, RoutedEventArgs e)
        {
            finishTimeMovieText.Text = mediaElement.NaturalDuration.TimeSpan.Hours.ToString() + ":" + mediaElement.NaturalDuration.TimeSpan.Minutes.ToString() + ":" + mediaElement.NaturalDuration.TimeSpan.Seconds.ToString();
            durationSliderMovie.Minimum = 0;
            durationSliderMovie.Maximum = mediaElement.NaturalDuration.TimeSpan.TotalSeconds;
        }

        private void mediaElement_MediaEnded(object sender, RoutedEventArgs e)
        {
            try
            {
                indexMovie++;
                mediaElement.Source = new Uri(pathMovies[indexMovie]);
            }
            catch
            {
                indexMovie = 0;
                mediaElement.Source = new Uri(pathMovies[indexMovie]);
            }
        }
        #endregion

        #region Drag
        private void Window_Drop(object sender, DragEventArgs e)
        {
            try
            {

                String[] FileName = (String[])e.Data.GetData(DataFormats.FileDrop, true);

                if (FileName.Length > 0)
                {
                    String[] VideoPath = FileName;

                    openVideoDragDrop(VideoPath);
                }
                e.Handled = true;
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show(ex.Message);
            }
        }
        private void openVideoDragDrop(string[] path)
        {
            pathMovies = path;
            mediaElement.Source = new Uri(pathMovies[0]);
            mediaElement.Stop();

            listCurrentFiles.ItemsSource = pathMovies;


            if (englishLanguage == true)
                listCurrentFilesLabel.Content = $"All files: " + pathMovies.Length.ToString();
            else
                listCurrentFilesLabel.Content = $"Всього файлів: " + pathMovies.Length.ToString();

            if (pathMovies[0].Contains(".mp4"))
            {

                imageUGrid.SetValue(Grid.ColumnProperty, 0);
                mediaElement.SetValue(Grid.ColumnProperty, 1);

            }
            if (pathMovies[0].Contains(".mp3"))
            {

                mediaElement.SetValue(Grid.ColumnProperty, 0);
                imageUGrid.SetValue(Grid.ColumnProperty, 1);

            }
            Title = Path.GetFileNameWithoutExtension(Path.GetFileNameWithoutExtension(pathMovies[0]));
        }


        #endregion

        #region Menu item "File"
        private void ItemOpenFile_Click(object sender, RoutedEventArgs e)
        {
            OpenMovieDialog();
        }
        private void ItemNedavFile_Click(object sender, RoutedEventArgs e)
        {

            if (listNedavFiles.SelectedIndex != -1)
            {
                mediaElement.Source = new Uri(listNedavFiles.Items[listNedavFiles.SelectedIndex].ToString());
                if (listNedavFiles.Items[listNedavFiles.SelectedIndex].ToString().Contains(".mp4"))
                {
                    imageUGrid.SetValue(Grid.ColumnProperty, 0);
                    mediaElement.SetValue(Grid.ColumnProperty, 1);
                }
                if (listNedavFiles.Items[listNedavFiles.SelectedIndex].ToString().Contains(".mp3"))
                {
                    mediaElement.SetValue(Grid.ColumnProperty, 0);
                    imageUGrid.SetValue(Grid.ColumnProperty, 1);
                }
                Title = Path.GetFileNameWithoutExtension(Path.GetFileNameWithoutExtension(listNedavFiles.Items[listNedavFiles.SelectedIndex].ToString()));
                PlayMovie();

            }
        }
        private void ItemListCurrentFile_Click(object sender, RoutedEventArgs e)
        {

            if (listCurrentFiles.SelectedIndex != -1)
            {
                mediaElement.Source = new Uri(pathMovies[listCurrentFiles.SelectedIndex]);
                if (pathMovies[listCurrentFiles.SelectedIndex].Contains(".mp4"))
                {
                    imageUGrid.SetValue(Grid.ColumnProperty, 0);
                    mediaElement.SetValue(Grid.ColumnProperty, 1);
                }
                if (pathMovies[listCurrentFiles.SelectedIndex].Contains(".mp3"))
                {
                    mediaElement.SetValue(Grid.ColumnProperty, 0);
                    imageUGrid.SetValue(Grid.ColumnProperty, 1);
                }
                Title = Path.GetFileNameWithoutExtension(Path.GetFileNameWithoutExtension(listCurrentFiles.Items[listCurrentFiles.SelectedIndex].ToString()));
                PlayMovie();
            }
        }

        #endregion

        #region Menu item "Viev"

        private void EnglishMenuItem_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Forms.DialogResult result = System.Windows.Forms.MessageBox.Show("Увага програма буде перезапущена\nдля впровадження опції мови.", "Увага", (System.Windows.Forms.MessageBoxButtons)MessageBoxButton.YesNo, (System.Windows.Forms.MessageBoxIcon)MessageBoxImage.Question);
            if (result == System.Windows.Forms.DialogResult.Yes)
            {
                englishLanguage = true;
                ukrainianLanguage = false;
                Properties.Settings.Default.languageCode = "En";
                Properties.Settings.Default.Save();
                System.Windows.Forms.Application.Restart();
            }
        }

        private void UkrainianMenuItem_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Forms.DialogResult result = System.Windows.Forms.MessageBox.Show("Увага програма буде перезапущена\nдля впровадження опції мови.", "Увага", (System.Windows.Forms.MessageBoxButtons)MessageBoxButton.YesNo, (System.Windows.Forms.MessageBoxIcon)MessageBoxImage.Question);
            if (result == System.Windows.Forms.DialogResult.Yes)
            {
                englishLanguage = false;
                ukrainianLanguage = true;
                Properties.Settings.Default.languageCode = "Ua";
                Properties.Settings.Default.Save();
                System.Windows.Forms.Application.Restart();
            }
        }
        #endregion

        #region Menu item "Audio"
        private void MensheSpeed_Click(object sender, RoutedEventArgs e)
        {
            mediaElement.SpeedRatio -= 1;
        }
        private void BolsheSpeed_Click(object sender, RoutedEventArgs e)
        {
            mediaElement.SpeedRatio += 1;
        }

        #endregion

        #region Menu item "Help"
        private void ShowInfoItem_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Видео можно открыть в полный екран если клацнуть средней кнопки мыши по екрану, можно также перекинуть файлы с папки прямо их перенести в форму.","Инфо",MessageBoxButton.OK,MessageBoxImage.Question);
        }
        
        #endregion

        #region Windows events
        private void Window_Closed(object sender, EventArgs e)
        {
            string path = "moviesList.txt";
            if (!File.Exists(path))
            {
                File.Create(path);
            }
            else
            {
                    using (StreamWriter writer = new StreamWriter(path, false))
                    {
                        foreach(var item in pathMovies)
                    {
                        if(item != null)
                            writer.WriteLine(item.ToString());
                    }
                   
                    }
                
            }
    
        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

            string path = "moviesList.txt";
            if (!File.Exists(path))
            {
                File.Create(path);
            }
            else
            {
                using (StreamReader reader = new StreamReader(path))
                {
                    while (!reader.EndOfStream)
                    {
                        listNedavFiles.Items.Add(reader.ReadLine());
                    }
                }
            }
            if (listNedavFiles.Items.Count > 0)
            {
                listNedavFiles.Visibility = Visibility.Visible;

                if(englishLanguage == true)
                nedavFilesLabel.Content = $"All files: " + listNedavFiles.Items.Count.ToString();
                else
                    nedavFilesLabel.Content = $"Всього файлів: " + listNedavFiles.Items.Count.ToString();
            }
            else
            {
                listNedavFiles.Visibility = Visibility.Collapsed;
            }

        }
        #endregion

    }
}
