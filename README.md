To test the project;
  dotnet test

To run the project and see a simple sample:
  dotnet run --project .\StreamingCatalogue\



Create the catalogue with:
  new StreamingRegister()

ReadOnlyCollection<(string StreamingServiceName, IMediaContent Content)> GetEntireRegister()
  Returns all entries as a read only collection of tuples: (string StreamingServiceName, IMediaContent Content)

ReadOnlyCollection<(string StreamingServiceName, IMediaContent Content)> GetAllContentsInStreamingService(string serviceName)
  Returns all entries for a StreamingService, as a read only collection of tuple: (string StreamingServiceName, IMediaContent Content)

(string StreamingServiceName, IMediaContent Content)? GetContentAndService(string title, int releaseYear, char contentType)
  Returns a tuple of the streaming service name and the IMediaContent record

bool SetRating(string name, int yearOrSeason, char contentType, int rating);
  Set the rating of the record with the given name and year or season, contentType is 'S' for TvShows and 'F' for Films

bool AddContent(IMediaContent content, string streamingServiceName);
  Add the content record to the streaming service with name streamingServiceName

bool RemoveContent(string name, int releaseYear, char contentType);
  Remove the content record with the passed name and year of release/ season number, contentType is 'S' for TvShows and 'F' for Films

bool AddStreamingService(IStreamingService streamingService);
  Add the streaming service its price to the catalogue

bool RemoveStreamingService(string serviceName);
  Remove the streaming service with the passed name from the catalogue

IStreamingService? GetStreamingService(string serviceName);
  Get the streaming service with the passed name

ReadOnlyCollection<IStreamingService> GetAllStreamingServices();
  Get a read only collection of all streaming services
