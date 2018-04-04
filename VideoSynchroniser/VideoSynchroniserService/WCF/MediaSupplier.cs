using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.ServiceModel;

namespace VideoSynchroniserService.WCF
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single)]
    public class MediaSupplier : IMediaSupplier
    {
        public Stream OpenTransferStream(MediaItem item)
        {
            FileInfo[] serverFiles = GetFiles();
            FileInfo mediaFile = serverFiles.First(i => i.Name == item.Name);
            return mediaFile.OpenRead();
        }

        public MediaItem[] RetrieveMediaItems()
        {
            FileInfo[] files = GetFiles();
            IEnumerable<MediaItem> items = files.Select(i => new MediaItem(i.Name));
            return items.ToArray();
        }

        private FileInfo[] GetFiles()
        {
            DirectoryInfo folder = new DirectoryInfo(FolderPath);
            FileInfo[] files = folder.GetFiles("*.mp4");
            return files;
        }

        private string FolderPath
        {
            get
            {
                return ConfigurationManager.AppSettings["FolderPath"];
            }
        }
    }
}
