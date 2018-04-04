using Media;
using System.IO;
using System.ServiceModel;
using System.ServiceModel.Web;

namespace VideoSynchroniserService.WCF
{
    [ServiceContract]
    public interface IMediaSupplier
    {
        [OperationContract]
        [WebInvoke(Method = "GET", UriTemplate = ServerUrls.RetrieveItems, RequestFormat = WebMessageFormat.Xml, BodyStyle = WebMessageBodyStyle.Bare)]
        MediaItem[] RetrieveMediaItems();

        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = ServerUrls.OpenStream, RequestFormat = WebMessageFormat.Xml, BodyStyle = WebMessageBodyStyle.Bare)]
        Stream OpenTransferStream(MediaItem item);
    }
}