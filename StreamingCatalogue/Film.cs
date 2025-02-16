namespace StreamingCatalogue
{
    public interface IFilm : IMediaContent
    {
        string Name { set; get; }
        DateOnly ReleaseDate { set; get; }
        string Genre { set; get; }
        int Rating { set; get; }
        int Runtime { set; get; }
        char ContentType { get; }
        string GetUniqueId();
    }

    public class Film : MediaContent, IFilm
    {

        public string Name { set; get; }
        public DateOnly ReleaseDate { set; get; }
        public string Genre { set; get; }
        private int _rating;
        public int Rating
        {
            get
            {
                return _rating;
            }
            set
            {
                if (value >= 0 && value <= 5)
                {
                    _rating = value;
                }
                else
                {
                    throw new ArgumentOutOfRangeException(value.ToString(), "is out of range 1-5.");
                }
            }
        }
        public int Runtime { set; get; }
        public Film(string name, DateOnly releaseDate, string genre = "", int runtime = 0, int rating = 0)
        {
            Name = name;
            ReleaseDate = releaseDate;
            Genre = genre;
            Runtime = runtime;
            Rating = rating;
        }
        public char ContentType { get { return 'F'; } }
        public override string ToString()
        {
            return $"{Name} - {ReleaseDate.ToString()} - Rating: {Rating}/5 - Genre: {Genre}";
        }
        public override string GetUniqueId()
        {

            return $"{ContentType}-{Name}-{ReleaseDate.Year}";

        }
    }
}