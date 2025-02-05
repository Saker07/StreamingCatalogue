class Film
{

    public string Name { set; get; }
    public DateOnly YearOfRelease { set; get; }
    public string Genre { set; get; }
    public int Rating
    {
        get
        {
            return this.Rating;
        }
        set
        {
            if (value > 0 && value <= 5)
            {
                this.Rating = value;
            }
            else
            {
                throw new ArgumentOutOfRangeException(value.ToString(), "is out of range 1-5.");
            }
        }
    }
    public int Runtime { set; get; }
    public Film(string name, DateOnly yearOfRelease, string genre, int runtime, int rating = 0)
    {
        Name = name;
        YearOfRelease = yearOfRelease;
        Genre = genre;
        Runtime = runtime;
        Rating = rating;
    }
}
