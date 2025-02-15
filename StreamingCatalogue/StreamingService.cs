using System.Collections.ObjectModel;
using System.Runtime.InteropServices;

namespace StreamingCatalogue
{
    public interface IStreamingService
    {
        string Name { set; get; }
        decimal Price { set; get; }
        ReadOnlyCollection<IFilm> GetAllFilms();
        IFilm? GetFilm(string name, int yearOfRelease);
        bool AddFilm(IFilm film);
        bool RemoveFilm(string title, int yearOfRelease);
        bool SetRating(string title, int yearOfRelease, int rating);
    }
    public class StreamingService : IStreamingService
    {
        public string Name { set; get; }
        public decimal Price { set; get; }
        //While having the Title and Year Of Release in both the key of dictionary and the Film object is redundant
        //it makes lookup in the dictionary faster and more readable than a list, it is needed in the Film object as otherwise it would complicate the handlig of the films
        private List<IFilm> Films { get; set; }
        public StreamingService(string name, decimal price, List<IFilm>? films = null)
        {
            Name = name;
            Price = price;
            Films = films != null ? films : new List<IFilm>();
        }

        public ReadOnlyCollection<IFilm> GetAllFilms() => new ReadOnlyCollection<IFilm>(Films);
        public IFilm? GetFilm(string name, int yearOfRelease)
        {
            if (Films != null)
            {
                return Films.Where(film => (film.Name == name && film.ReleaseDate.Year == yearOfRelease)).FirstOrDefault();
            }
            else
            {
                return null;
            }
        }
        public bool AddFilm(IFilm film)
        {
            if (GetFilm(film.Name, film.ReleaseDate.Year) == null)
            {
                Films.Add(film);
                return true;
            }
            else { return false; }

        }
        public bool RemoveFilm(string name, int releaseYear)
        {
            IFilm? filmToRemove = GetFilm(name, releaseYear);
            if (filmToRemove != null)
            {
                return Films.Remove(filmToRemove);
            }
            else { return false; }
        }
        public bool SetRating(string title, int releaseYear, int rating)
        {
            IFilm? filmFound = GetFilm(title, releaseYear);
            if (filmFound != null)
            {
                filmFound.Rating = rating;
                return true;
            }
            else
            {
                return false;
            }

        }
    }
}