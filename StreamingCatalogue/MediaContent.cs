namespace StreamingCatalogue
{
    public interface IMediaContent
    {
        string Name { set; get; }
        DateOnly ReleaseDate { set; get; }
        string Genre { set; get; }
        int Rating { set; get; }
        char ContentType { get; }
        string GetUniqueId();
    }
    public abstract class MediaContent : IMediaContent
    {
        public char ContentType { get; }
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

        public abstract string GetUniqueId();
    }
}