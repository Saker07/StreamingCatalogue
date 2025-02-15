using System.Collections.ObjectModel;
using System.Dynamic;

namespace StreamingCatalogue
{
    public interface IStreamingRegister
    {
        ReadOnlyCollection<(string StreamingServiceName, IFilm Film)> GetEntireRegister();
        ReadOnlyCollection<(string StreamingServiceName, IFilm Film)> GetAllFilmsInStreamingService(string serviceName);
        (string StreamingServiceName, IFilm Film)? GetFilmAndService(string title, int releaseYear);
        bool SetRating(string filmName, int yearOfRelease, int rating);
        bool AddFilm(IFilm film, string streamingServiceName);
        bool RemoveFilm(string name, int releaseYear);
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

        public ReadOnlyCollection<(string StreamingServiceName, IFilm Film)> GetEntireRegister()
        {
            List<(string StreamingServiceName, IFilm Film)> returnList = new List<(string StreamingServiceName, IFilm Film)>();
            foreach (var service in StreamingServices)
            {
                List<IFilm> serviceFilms = new List<IFilm>(service.GetAllFilms());
                serviceFilms.Sort();
                foreach (var film in serviceFilms)
                {
                    returnList.Add((service.Name, film));
                }
            }
            return new ReadOnlyCollection<(string StreamingServiceName, IFilm Film)>(returnList);
        }

        public ReadOnlyCollection<(string StreamingServiceName, IFilm Film)>? GetAllFilmsInStreamingService(string streamingServiceName)
        {
            IStreamingService? service = StreamingServices.Where(service => service.Name == streamingServiceName).FirstOrDefault();
            List<(string StreamingServiceName, IFilm Film)> returnList = new List<(string StreamingServiceName, IFilm Film)>();

            if (service != null)
            {
                ReadOnlyCollection<IFilm> filmList = service.GetAllFilms();
                foreach (var film in filmList)
                {
                    returnList.Add((service.Name, film));
                }
                return new ReadOnlyCollection<(string StreamingServiceName, IFilm Film)>(returnList);
            }
            return null;
        }

        public (string StreamingServiceName, IFilm Film)? GetFilmAndService(string title, int releaseYear)
        {
            (string StreamingServiceName, IFilm Film)? returnTuple = null;
            IFilm? returnFilm = null;
            foreach (var service in StreamingServices)
            {
                returnFilm = service.GetFilm(title, releaseYear);
                if (returnFilm != null)
                {
                    returnTuple = (service.Name, returnFilm);
                    break;
                }
            }
            return returnTuple;
        }

        public bool SetRating(string filmName, int yearOfRelease, int rating)
        {
            GetFilmAndService(filmName, yearOfRelease);
            foreach (var service in StreamingServices)
            {
                bool isRatingSet = service.SetRating(filmName, yearOfRelease, rating);
                if (isRatingSet)
                {
                    return true;
                }
            }
            return false;

        }
        public bool AddFilm(IFilm film, string streamingServiceName)
        {
            ReadOnlyCollection<(string StreamingServiceName, IFilm Film)>? streamingRegister = GetEntireRegister();
            (string StreamingServiceName, IFilm Film)? serviceFilm = streamingRegister?.Where(serviceFilm => serviceFilm.Film.Name == film.Name && serviceFilm.Film.ReleaseDate.Year == film.ReleaseDate.Year).FirstOrDefault();
            if (serviceFilm == (null, null))
            {
                bool? isFilmAdded = StreamingServices.Where(service => service.Name == streamingServiceName).FirstOrDefault()?.AddFilm(film);
                return isFilmAdded != null && isFilmAdded == true ? true : false;
            }
            else
            {
                return false;
            }
        }
        public bool RemoveFilm(string filmName, int yearOfRelease)
        {
            bool filmRemoved = false;
            foreach (var service in StreamingServices)
            {
                filmRemoved = service.RemoveFilm(filmName, yearOfRelease);
                if (filmRemoved)
                {
                    break;
                }
            }
            return filmRemoved;
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