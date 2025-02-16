using System.Collections.ObjectModel;
using System.Dynamic;

namespace StreamingCatalogue
{
    public interface IStreamingRegister
    {
        ReadOnlyCollection<(string StreamingServiceName, IMediaContent Content)> GetEntireRegister();
        ReadOnlyCollection<(string StreamingServiceName, IMediaContent Content)> GetAllContentsInStreamingService(string serviceName);
        (string StreamingServiceName, IMediaContent Content)? GetContentAndService(string title, int releaseYear, char contentType);
        bool SetRating(string name, int yearOfRelease, char contentType, int rating);
        bool AddContent(IMediaContent content, string streamingServiceName);
        bool RemoveContent(string name, int releaseYear, char contentType);
        bool AddStreamingService(IStreamingService streamingService);
        bool RemoveStreamingService(string serviceName);
        IStreamingService? GetStreamingService(string serviceName);
        ReadOnlyCollection<IStreamingService> GetAllStreamingServices();

    }
    public class StreamingRegister : IStreamingRegister
    {
        private List<IStreamingService> StreamingServices { set; get; }
        public StreamingRegister(List<IStreamingService>? streamingServices = null)
        {
            StreamingServices = streamingServices != null ? streamingServices : new List<IStreamingService>();
        }

        public ReadOnlyCollection<(string StreamingServiceName, IMediaContent Content)> GetEntireRegister()
        {
            List<(string StreamingServiceName, IMediaContent Content)> returnList = new List<(string StreamingServiceName, IMediaContent Content)>();
            foreach (var service in StreamingServices)
            {
                List<IMediaContent> contentList = new List<IMediaContent>(service.GetAllContents());
                foreach (var content in contentList)
                {
                    returnList.Add((service.Name, content));
                }
            }
            return new ReadOnlyCollection<(string StreamingServiceName, IMediaContent Content)>(returnList);
        }

        public ReadOnlyCollection<(string StreamingServiceName, IMediaContent Content)>? GetAllContentsInStreamingService(string streamingServiceName)
        {
            IStreamingService? service = StreamingServices.Where(service => service.Name == streamingServiceName).FirstOrDefault();
            List<(string StreamingServiceName, IMediaContent Content)> returnList = new List<(string StreamingServiceName, IMediaContent Content)>();

            if (service != null)
            {
                ReadOnlyCollection<IMediaContent> contentList = service.GetAllContents();
                foreach (var content in contentList)
                {
                    returnList.Add((service.Name, content));
                }
                return new ReadOnlyCollection<(string StreamingServiceName, IMediaContent Content)>(returnList);
            }
            return null;
        }

        public (string StreamingServiceName, IMediaContent Content)? GetContentAndService(string title, int releaseYear, char contentType)
        {
            (string StreamingServiceName, IMediaContent Content)? returnTuple = null;
            IMediaContent? returnContent = null;
            foreach (var service in StreamingServices)
            {
                returnContent = service.GetContent(title, releaseYear, contentType);
                if (returnContent != null)
                {
                    returnTuple = (service.Name, returnContent);
                    break;
                }
            }
            return returnTuple;
        }

        public bool SetRating(string contentName, int yearOfRelease, char contentType, int rating)
        {
            GetContentAndService(contentName, yearOfRelease, contentType);
            foreach (var service in StreamingServices)
            {
                bool isRatingSet = service.SetRating(contentName, yearOfRelease, contentType, rating);
                if (isRatingSet)
                {
                    return true;
                }
            }
            return false;

        }
        public bool AddContent(IMediaContent content, string streamingServiceName)
        {
            ReadOnlyCollection<(string StreamingServiceName, IMediaContent Content)>? streamingRegister = GetEntireRegister();
            (string StreamingServiceName, IMediaContent Content)? serviceContent = streamingRegister?.Where(serviceContent => serviceContent.Content.GetUniqueId() == content.GetUniqueId()).FirstOrDefault();
            if (serviceContent == (null, null))
            {
                bool? isContentAdded = StreamingServices.Where(service => service.Name == streamingServiceName).FirstOrDefault()?.AddContent(content);
                return isContentAdded != null && isContentAdded == true ? true : false;
            }
            else
            {
                return false;
            }
        }
        public bool RemoveContent(string contentName, int yearOfRelease, char contentType)
        {
            bool contentRemoved = false;
            foreach (var service in StreamingServices)
            {
                contentRemoved = service.RemoveContent(contentName, yearOfRelease, contentType);
                if (contentRemoved)
                {
                    break;
                }
            }
            return contentRemoved;
        }
        public bool AddStreamingService(IStreamingService streamingService)
        {
            if (StreamingServices.Where(service => service.Name == streamingService.Name).FirstOrDefault() == null)
            {
                StreamingServices.Add(streamingService);
                return true;
            }
            return false;
        }
        public bool RemoveStreamingService(string serviceName)
        {
            IStreamingService? foundStreamingService = StreamingServices.Where(service => service.Name == serviceName).FirstOrDefault();
            if (foundStreamingService != null)
            {
                StreamingServices.Remove(foundStreamingService);
                return true;
            }
            return false;
        }

        public IStreamingService? GetStreamingService(string serviceName)
        {
            return StreamingServices.Where(service => service.Name == serviceName).FirstOrDefault();
        }
        public ReadOnlyCollection<IStreamingService> GetAllStreamingServices()
        {
            return new ReadOnlyCollection<IStreamingService>(StreamingServices);
        }
    }
}