using System.Collections.ObjectModel;
using NUnit.Framework.Internal;

namespace StreamingCatalogue.UnitTests
{
    [TestFixture]
    public class StreamingServiceWithFilmsTest
    {
        private IStreamingService _streamingService { get; set; }
        private List<IFilm> _startingFilmlist { get; set; }
        [SetUp]
        public void Setup()
        {
            _startingFilmlist = [new Film("Inception", new DateOnly(2010, 1, 1), "Action", 148), new Film("Constantine", new DateOnly(2005, 3, 18), "Supernatural", 121)];
            _streamingService = new StreamingService("Notflix", 1099, new List<IFilm>(_startingFilmlist));
        }
        [TestCase("Inception", 2010, 5)]
        public void SetRating_should_set_film_rating_when_int_in_range(string title, int yearOfRelease, int rating)
        {
            bool returnValue = _streamingService.SetRating(title, yearOfRelease, rating);
            Assert.That(returnValue, Is.True);
            Assert.That(_streamingService.GetFilm(title, yearOfRelease).Rating, Is.EqualTo(rating));
        }
        [TestCase("Superman", 2020, 4)]
        public void SetRating_should_return_false_when_film_not_found(string title, int yearOfRelease, int rating)
        {
            bool returnValue = _streamingService.SetRating(title, yearOfRelease, rating);
            Assert.That(returnValue, Is.False);
        }
        [Test]
        public void GetAllFilms_should_return_all_films_when_films_are_present()
        {
            ReadOnlyCollection<IFilm>? returnValue = _streamingService.GetAllFilms();
            Assert.That(returnValue.Count, Is.EqualTo(2));
            Assert.That(returnValue, Is.EqualTo(_startingFilmlist));
        }
        [Test]
        public void GetFilm_should_return_film_when_found()
        {
            IFilm filmToGet = _startingFilmlist[0];
            IFilm returnedFilm = _streamingService.GetFilm(filmToGet.Name, filmToGet.ReleaseDate.Year);
            Assert.That(returnedFilm, Is.EqualTo<IFilm>(filmToGet));
        }
        [Test]
        public void GetFilm_should_return_null_when_not_found()
        {
            IFilm returnedFilm = _streamingService.GetFilm("Thor", 2020);
            Assert.That(returnedFilm, Is.Null);
        }
        [Test]
        public void AddFilm_should_add_film_when_film_not_found()
        {
            IFilm filmToAdd = new Film("A Silent Voice", new DateOnly(2017, 3, 15), "Drama", 130, 5);
            bool wasFilmAdded = _streamingService.AddFilm(filmToAdd);
            Assert.That(wasFilmAdded, Is.True);
            Assert.That(_streamingService.GetFilm(filmToAdd.Name, filmToAdd.ReleaseDate.Year), Is.EqualTo<IFilm>(filmToAdd));
        }
        [Test]
        public void AddFilm_should_return_false_when_film_already_found()
        {
            IFilm filmToAdd = new Film("Inception", new DateOnly(2010, 1, 1), "Action", 148);
            bool wasFilmAdded = _streamingService.AddFilm(filmToAdd);
            Assert.That(wasFilmAdded, Is.False);
        }
        [Test]
        public void RemoveFilm_should_remove_film_when_film_found()
        {
            IFilm filmToRemove = new Film("Inception", new DateOnly(2010, 1, 1), "Action", 148);
            bool wasFilmRemoved = _streamingService.RemoveFilm(filmToRemove.Name, filmToRemove.ReleaseDate.Year);
            Assert.That(wasFilmRemoved, Is.True);
            Assert.That(_streamingService.GetFilm(filmToRemove.Name, filmToRemove.ReleaseDate.Year), Is.Null);
        }
        [Test]
        public void RemoveFilm_should_return_false_when_film_not_found()
        {
            IFilm filmToRemove = new Film("Ghost Rider", new DateOnly(2007, 3, 2), "Action", 116);
            bool wasFilmRemoved = _streamingService.RemoveFilm(filmToRemove.Name, filmToRemove.ReleaseDate.Year);
            Assert.That(wasFilmRemoved, Is.False);
        }
    }
    [TestFixture]
    public class StreamingServiceEmptyTest
    {
        private IStreamingService _streamingService { get; set; }
        private IFilm _film = new Film("Patema Inverted", new DateOnly(2013, 11, 9), "Fantasy", 99, 3);
        [SetUp]
        public void Setup()
        {
            _streamingService = new StreamingService("Notflix", 1099, new List<IFilm>());
        }
        [Test]
        public void SetRating_should_return_false_when_film_list_empty()
        {
            bool returnValue = _streamingService.SetRating(_film.Name, _film.ReleaseDate.Year, 2);
            Assert.That(returnValue, Is.False);
        }
        [Test]
        public void AddFilm_should_add_book_when_film_list_empty()
        {
            bool returnValue = _streamingService.AddFilm(_film);
            Assert.That(returnValue, Is.True);
            Assert.That(_streamingService.GetFilm(_film.Name, _film.ReleaseDate.Year), Is.EqualTo<IFilm>(_film));
        }
        [Test]
        public void RemoveBook_should_return_false_when_film_list_empty()
        {
            bool returnValue = _streamingService.RemoveFilm(_film.Name, _film.ReleaseDate.Year);
            Assert.That(returnValue, Is.False);
        }
        [Test]
        public void GetFilm_should_return_null_when_film_list_empty()
        {
            IFilm returnedFilm = _streamingService.GetFilm(_film.Name, _film.ReleaseDate.Year);
            Assert.That(returnedFilm, Is.Null);
        }
        [Test]
        public void GetAllFilms_shouldReturn_empty_list_when_film_list_empty()
        {
            ReadOnlyCollection<IFilm> returnedList = _streamingService.GetAllFilms();
            Assert.That(returnedList.Count(), Is.EqualTo(0));
        }
    }
}