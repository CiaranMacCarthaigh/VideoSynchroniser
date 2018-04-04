using System;
using VideoPlayer.ViewModel;
using Windows.Media.Playback;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Animation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace VideoPlayer
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        private DispatcherTimer _timer;

        public MainPage()
        {
            this.InitializeComponent();
            DataContext = new Playlist();
            Playlist.PropertyChanged += Playlist_PropertyChanged;

            Window.Current.CoreWindow.PointerCursor = null;

            _timer = new DispatcherTimer();
            _timer.Interval = TimeSpan.FromSeconds(5);
            _timer.Tick += _timer_Tick;
        }

        private void Playlist_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "CurrentlyPlayingItem")
            {
                Storyboard storyboard = (Storyboard)this.Resources["TextFadeInAnimation"];
                storyboard.Begin();
            }
        }

        private async void _timer_Tick(object sender, object e)
        {
            _timer.Stop();
            try
            {
                await Playlist.UpdatePlaylistAsync();
            }
            catch (Exception ex)
            {
                MessageDialog dialog = new MessageDialog(ex.ToString());
                await dialog.ShowAsync();
            }
            finally
            {
                _timer.Start();
            }
        }

        private Playlist Playlist
        {
            get
            {
                return DataContext as Playlist;
            }
            set
            {
                DataContext = value;
            }
        }

        private async void Page_Loaded(object sender, RoutedEventArgs e)
        {
            MediaPlayer.MediaPlayer.MediaEnded += MediaPlayer_MediaEnded;
            MediaPlayer.MediaPlayer.MediaFailed += MediaPlayer_MediaFailed;
            await Playlist.Initialise();
            _timer.Start();
        }

        private async void MediaPlayer_MediaFailed(MediaPlayer sender, MediaPlayerFailedEventArgs args)
        {
            await Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, async () =>
            {
                MessageDialog dialog = new MessageDialog(args.Error.ToString());
                await dialog.ShowAsync();
            });
        }

        private async void MediaPlayer_MediaEnded(MediaPlayer sender, object args)
        {
            await Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
            {
                Playlist.NextItem();
            });
        }

        private void Storyboard_Completed(object sender, object e)
        {
            Storyboard storyboard = (Storyboard)this.Resources["TextFadeOutAnimation"];
            storyboard.Begin();
        }
    }
}