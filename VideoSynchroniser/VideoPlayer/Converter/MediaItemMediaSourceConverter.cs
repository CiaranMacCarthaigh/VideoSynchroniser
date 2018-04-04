using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VideoPlayer.ServiceReference;
using Windows.Media.Core;
using Windows.Storage;
using Windows.UI.Xaml.Data;

namespace VideoPlayer.Converter
{
    public class MediaItemMediaSourceConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            MediaItem item = value as MediaItem;
            if (item == null)
            {
                return null;
            }

            StorageFile storageFile = GetMediaItemFile(item);
            return MediaSource.CreateFromStorageFile(storageFile);
        }

        private StorageFile GetMediaItemFile(MediaItem item)
        {
            StorageFolder folder = ApplicationData.Current.TemporaryFolder;
            Task<StorageFile> retrieveFileTask = folder.GetFileAsync(item.Name).AsTask();
            retrieveFileTask.Wait();
            return retrieveFileTask.Result;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
