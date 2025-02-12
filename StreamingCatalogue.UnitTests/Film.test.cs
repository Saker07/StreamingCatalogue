namespace StreamingCatalogue.UnitTests;

[TestFixture]
public class FilmTests
{
    private Film _film { get; set; }
    [SetUp]
    public void Setup()
    {
        _film = new Film("Inception", new DateOnly(2010, 1, 1), "Action", 148);
    }

    [TestCase(-1)]
    [TestCase(6)]
    public void Rating_should_throw_when_parameter_is_out_of_range(int ratingValue)
    {
        Assert.That(() => { _film.Rating = ratingValue; return true; }, Throws.TypeOf<ArgumentOutOfRangeException>());
    }
}