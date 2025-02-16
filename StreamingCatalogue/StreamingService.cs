using System.Collections.ObjectModel;
using System.Net.Mime;
using System.Runtime.InteropServices;

namespace StreamingCatalogue
{
    public interface IStreamingService
    {
        string Name { set; get; }
        decimal Price { set; get; }
        ReadOnlyCollection<IMediaContent> GetAllContents();
        IMediaContent? GetContent(string name, int yearOfRelease, char contentType);
        bool AddContent(IMediaContent content);
        bool RemoveContent(string title, int yearOfRelease, char contentType);
        bool SetRating(string title, int yearOfRelease, char contentType, int rating);
    }
    public class StreamingService : IStreamingService
    {
        public string Name { set; get; }
        public decimal Price { set; get; }
        //While having the Title and Year Of Release in both the key of dictionary and the Film object is redundant
        //it makes lookup in the dictionary faster and more readable than a list, it is needed in the Film object as otherwise it would complicate the handlig of the films
        private List<IMediaContent> ContentList { get; set; }
        public StreamingService(string name, decimal price, List<IMediaContent>? contentList = null)
        {
            Name = name;
            Price = price;
            ContentList = contentList != null ? contentList : new List<IMediaContent>();
        }

        public ReadOnlyCollection<IMediaContent> GetAllContents() => new ReadOnlyCollection<IMediaContent>(ContentList);
        public IMediaContent? GetContent(string name, int yearOrSeason, char contentType)
        {

            if (ContentList != null)
            {
                return ContentList.Where(content => content.GetUniqueId() == $"{contentType}-{name}-{yearOrSeason}").FirstOrDefault();
            }
            else
            {
                return null;
            }
        }
        public bool AddContent(IMediaContent content)
        {
            if (GetContent(content.Name, content.ReleaseDate.Year, content.ContentType) == null)
            {
                ContentList.Add(content);
                return true;
            }
            else { return false; }

        }
        public bool RemoveContent(string name, int releaseYear, char contentType)
        {
            IMediaContent? contentToRemove = GetContent(name, releaseYear, contentType);
            if (contentToRemove != null)
            {
                return ContentList.Remove(contentToRemove);
            }
            else { return false; }
        }
        public bool SetRating(string title, int releaseYear, char contentType, int rating)
        {
            IMediaContent? contentFound = GetContent(title, releaseYear, contentType);
            if (contentFound != null)
            {
                contentFound.Rating = rating;
                return true;
            }
            else
            {
                return false;
            }

        }
    }
}