using System.Collections.ObjectModel;
using System.Threading.Tasks.Dataflow;
using StreamingCatalogue;

StreamingRegister Register = new StreamingRegister();

Film film = new Film("film1", new DateOnly(), "horror", 60);

Register.AddStreamingService(new StreamingService("Notflix", 12.99M));
Register.AddStreamingService(new StreamingService("AmazingPlus", 5.99M));


Register.AddContent(film, "Notflix");

Register.AddContent(new Film("film1", new DateOnly(), "horror", 60), "Notflix");
Register.AddContent(new Film("film2", new DateOnly(), "horror", 60), "Notflix");
Register.AddContent(new Film("film3", new DateOnly(), "horror", 60), "Notflix");
Register.AddContent(new Film("film4", new DateOnly(), "horror", 60), "Notflix");
Register.AddContent(new TvShow("TvShow1", new DateOnly(), 5, "horror", 60), "Notflix");
Register.AddContent(new TvShow("TvShow2", new DateOnly(), 3, "horror", 60), "Notflix");
Register.AddContent(new Film("film5", new DateOnly(), "horror", 60), "AmazingPlus");
Register.AddContent(new TvShow("TvShow1", new DateOnly(), 2, "horror", 60), "AmazingPlus");
Register.AddContent(new TvShow("TvShow4", new DateOnly(), 5, "horror", 60), "AmazingPlus");

ReadOnlyCollection<(string StreamingServiceName, IMediaContent Content)>? noftlixFilms = Register.GetAllContentsInStreamingService("Notflix");

foreach (var x in Register.GetEntireRegister())
{
    Console.WriteLine($"{x.StreamingServiceName} * {x.Content}");
}

Console.WriteLine();
Console.WriteLine();
Console.WriteLine();
Console.WriteLine();
Console.WriteLine("Get all tv shows: (press Enter to continue)");
Console.ReadLine();
foreach (var x in Register.GetEntireRegister().Where(x => x.Content.ContentType == 'S'))
{
    Console.WriteLine($"{x.StreamingServiceName} * {x.Content}");
}
Console.WriteLine("Press Enter to continue");
Console.ReadLine();
Console.WriteLine();
Console.WriteLine();



Console.WriteLine("Get all Films: (press Enter to continue)");
Console.ReadLine();
foreach (var x in Register.GetEntireRegister().Where(x => x.Content.ContentType == 'F'))
{
    Console.WriteLine($"{x.StreamingServiceName} * {x.Content}");
}
Console.WriteLine("Press Enter to continue");
Console.ReadLine();
Console.WriteLine();
Console.WriteLine();



Console.WriteLine("Get all Films in Notflix: (press Enter to continue)");
Console.ReadLine();
foreach (var x in Register.GetAllContentsInStreamingService("Notflix").Where(x => x.Content.ContentType == 'F'))
{
    Console.WriteLine($"{x.StreamingServiceName} * {x.Content}");
}
Console.WriteLine("Press Enter to continue");
Console.ReadLine();
Console.WriteLine();
Console.WriteLine();


Console.WriteLine("Get all seasons of TvShow1: (press Enter to continue)");
Console.ReadLine();
foreach (var x in Register.GetEntireRegister().Where(s => s.Content.ContentType == 'S' && s.Content.Name == "TvShow1"))
{
    Console.WriteLine($"{x.StreamingServiceName} * {x.Content}");
}
Console.WriteLine("Press Enter to exit");
Console.ReadLine();
Console.WriteLine();
Console.WriteLine();
