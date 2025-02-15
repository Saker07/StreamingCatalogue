using System.Collections.ObjectModel;
using System.Threading.Tasks.Dataflow;
using StreamingCatalogue;

StreamingRegister test = new StreamingRegister();

Film film = new Film("film1", new DateOnly(), "horror", 60);

test.AddStreamingService(new StreamingService("Notflix", 12.99M));
test.AddFilm(film, "Notflix");

ReadOnlyCollection<(string StreamingServiceName, IFilm Film)>? noftlixFilms = test.GetAllFilmsInStreamingService("Notflix");

if (noftlixFilms != null)
{
    foreach (var x in noftlixFilms)
    {
        Console.WriteLine($"{x.Film.Name}, {x.Film.Genre}, {x.Film.ReleaseDate}, {x.Film.Rating}");
    }
}