namespace StreamingCatalogue
{
    public interface ITvShow : IMediaContent
    {
        char ContentType { get { return 'S'; } }
        string Name { set; get; }
        DateOnly ReleaseDate { set; get; }
        string Genre { set; get; }

        int Rating { set; get; }
        int SeasonNumber { set; get; }
        int NumberOfEpisodes { set; get; }

        string ToString();

        string GetUniqueId();

    }
    public class TvShow : MediaContent, ITvShow
    {
        public char ContentType { get { return 'S'; } }
        public string Name { set; get; }
        public DateOnly ReleaseDate { set; get; }
        public string Genre { set; get; }
        private int _rating;
        public virtual int Rating
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
        public int SeasonNumber { set; get; }
        public int NumberOfEpisodes { set; get; }
        public TvShow(string name, DateOnly releaseDate, int seasonNumber, string genre = "", int numberOfEpisodes = 0, int rating = 0)
        {
            Name = name;
            Genre = genre;
            ReleaseDate = releaseDate;
            NumberOfEpisodes = numberOfEpisodes;
            _rating = rating;
            SeasonNumber = seasonNumber;
        }

        public override string ToString()
        {
            return $"{Name} - Season {SeasonNumber} - {ReleaseDate.ToString()} - Episode count:{NumberOfEpisodes} - Rating: {Rating}/5 - Genre: {Genre}";
        }
        public override string GetUniqueId()
        {

            return $"{ContentType}-{Name}-{SeasonNumber}";

        }
    }
}