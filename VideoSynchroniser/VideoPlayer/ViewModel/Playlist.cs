using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using VideoPlayer.ServiceReference;

namespace VideoPlayer.ViewModel
{
    public class Playlist : INotifyPropertyChanged
    {
        #region Fields
        private ObservableCollection<MediaItem> _playList = new ObservableCollection<MediaItem>();

        private MediaItem _currentlyPlayingItem = null;

        private IMediaSupplierChannel _service;
        #endregion

        #region INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;
        #endregion

        #region Constructor(s)
        public Playlist()
        {
            _service = new ServiceReference.MediaSupplierClient();
        }
        #endregion

        #region Methods
        protected virtual void RaisePropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public void MediaFinishedPlaying()
        {
            if (_playList.Count == 0)
            {
                return;
            }

            int nextIndex;
            if (_currentlyPlayingItem == null)
            {
                nextIndex = -1;
            }
            else
            {
                nextIndex = _playList.IndexOf(_currentlyPlayingItem);
            }

            nextIndex++;

            if (nextIndex >= _playList.Count)
            {
                nextIndex = 0;
            }

            CurrentlyPlayingItem = _playList[nextIndex];
        }

        public async Task UpdatePlaylist()
        {
            MediaItem[] serverItems = await _service.RetrieveMediaItemsAsync();
            MediaItem[] localItems = await RetrieveLocalItemsAsync();
        }

        private async Task<MediaItem[]> RetrieveLocalItemsAsync()
        {

        }
        #endregion

        #region Properties
        public MediaItem CurrentlyPlayingItem
        {
            get
            {
                return _currentlyPlayingItem;
            }
            set
            {
                if (_currentlyPlayingItem != value)
                {
                    _currentlyPlayingItem = value;
                    RaisePropertyChanged();
                }
            }
        }
        #endregion
    }
}
