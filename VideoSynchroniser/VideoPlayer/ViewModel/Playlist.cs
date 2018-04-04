using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using VideoPlayer.ServiceReference;
using Windows.Storage;
using Windows.Storage.Search;
using Windows.Storage.Streams;

namespace VideoPlayer.ViewModel
{
    public class Playlist : INotifyPropertyChanged
    {
        #region Fields
        private ObservableCollection<MediaItem> _playList = new ObservableCollection<MediaItem>();

        private MediaItem _currentlyPlayingItem = null;

        private IMediaSupplier _service;
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

        public void NextItem()
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
                nextIndex = _playList.IndexOf(_playList.First(i => i.Name == _currentlyPlayingItem.Name));
            }

            nextIndex++;

            if (nextIndex >= _playList.Count)
            {
                nextIndex = 0;
            }

            CurrentlyPlayingItem = _playList[nextIndex];
        }

        public async Task UpdatePlaylistAsync()
        {
            MediaItem[] serverItems = await _service.RetrieveMediaItemsAsync();
            List<MediaItem> localItems = await RetrieveLocalItemsAsync();

            foreach (MediaItem serverItem in serverItems.Where(i => localItems.Any(x => x.Name == i.Name) == false))
            {
                // Copy the item to the local server.
                byte[] transferredData = await _service.OpenTransferStreamAsync(serverItem);
                await SaveItemToLocalStorage(serverItem, transferredData);
            }

            // All server items are now local.
            // Delete the local items that are no longer on the server.
            for (int index = localItems.Count - 1; index >= 0; index--)
            {
                MediaItem candidateItem = localItems[index];
                // Don't delete the currently playing item!
                if (candidateItem.Equals(_currentlyPlayingItem))
                {
                    continue;
                }
                if (serverItems.Any(i => i.Name == candidateItem.Name) == false)
                {
                    await DeleteLocalItem(candidateItem);
                    localItems.RemoveAt(index);
                }
            }

            _playList = new ObservableCollection<MediaItem>(localItems);
            if (_currentlyPlayingItem == null)
            {
                NextItem();
            }
        }

        private async Task DeleteLocalItem(MediaItem item)
        {
            StorageFolder folder = ApplicationData.Current.TemporaryFolder;
            StorageFile file = await folder.GetFileAsync(item.Name);
            await file.DeleteAsync();
        }

        private async Task SaveItemToLocalStorage(MediaItem serverItem, byte[] transferredData)
        {
            StorageFolder folder = ApplicationData.Current.TemporaryFolder;
            StorageFile newFile = await folder.CreateFileAsync(serverItem.Name, CreationCollisionOption.FailIfExists);
            using (IRandomAccessStream stream = await newFile.OpenAsync(FileAccessMode.ReadWrite, StorageOpenOptions.AllowReadersAndWriters))
            {
                IBuffer buffer = transferredData.AsBuffer();
                await stream.WriteAsync(buffer);
            }
        }

        private async Task<List<MediaItem>> RetrieveLocalItemsAsync()
        {
            StorageFolder folder = ApplicationData.Current.TemporaryFolder;
            IReadOnlyList<IStorageFile> files = await folder.GetFilesAsync();
            return files.Select(i => new MediaItem()
            {
                Name = i.Name
            }).ToList();
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
