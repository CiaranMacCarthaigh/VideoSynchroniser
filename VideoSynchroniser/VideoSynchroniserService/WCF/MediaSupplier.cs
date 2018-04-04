using System.Configuration;
using System.IO;
using System.Linq;
using System.ServiceModel;
using Media;

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
            return files.Select(i => new MediaItem(i.Name)).ToArray();
        }

        private FileInfo[] GetFiles()
        {
            DirectoryInfo folder = new DirectoryInfo(FolderPath);
            FileInfo[] files = folder.GetFiles("*.avi");
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
