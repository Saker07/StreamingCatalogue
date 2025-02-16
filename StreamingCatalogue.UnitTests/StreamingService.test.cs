using System.Collections.ObjectModel;
using NUnit.Framework.Internal;

namespace StreamingCatalogue.UnitTests
{
    [TestFixture]
    public class StreamingServiceWithContentsTest
    {
        private IStreamingService _streamingService { get; set; }
        private List<IMediaContent> _startingContentList { get; set; }
        [SetUp]
        public void Setup()
        {
            _startingContentList = [new Film("Inception", new DateOnly(2010, 1, 1), "Action", 148), new Film("Constantine", new DateOnly(2005, 3, 18), "Supernatural", 121)];
            _streamingService = new StreamingService("Notflix", 1099, new List<IMediaContent>(_startingContentList));
        }
        [TestCase("Inception", 2010, 'F', 5)]
        public void SetRating_should_set_content_rating_when_int_in_range(string title, int yearOfRelease, char contentType, int rating)
        {
            bool returnValue = _streamingService.SetRating(title, yearOfRelease, contentType, rating);
            Assert.That(returnValue, Is.True);
            Assert.That(_streamingService.GetContent(title, yearOfRelease, contentType).Rating, Is.EqualTo(rating));
        }
        [TestCase("Superman", 2020, 'F', 4)]
        public void SetRating_should_return_false_when_content_not_found(string title, int yearOfRelease, char contentType, int rating)
        {
            bool returnValue = _streamingService.SetRating(title, yearOfRelease, contentType, rating);
            Assert.That(returnValue, Is.False);
        }
        [Test]
        public void GetAllContents_should_return_all_contents_when_contents_are_present()
        {
            ReadOnlyCollection<IMediaContent>? returnValue = _streamingService.GetAllContents();
            Assert.That(returnValue.Count, Is.EqualTo(2));
            Assert.That(returnValue, Is.EqualTo(_startingContentList));
        }
        [Test]
        public void GetContent_should_return_content_when_found()
        {
            IMediaContent contentToGet = _startingContentList[0];
            IMediaContent returnedContent = _streamingService.GetContent(contentToGet.Name, contentToGet.ReleaseDate.Year, contentToGet.ContentType);
            Assert.That(returnedContent, Is.EqualTo<IMediaContent>(contentToGet));
        }
        [Test]
        public void GetContent_should_return_null_when_not_found()
        {
            IMediaContent returnedContent = _streamingService.GetContent("Thor", 2020, 'F');
            Assert.That(returnedContent, Is.Null);
        }
        [Test]
        public void AddContent_should_add_content_when_content_not_found()
        {
            IMediaContent contentToAdd = new Film("A Silent Voice", new DateOnly(2017, 3, 15), "Drama", 130, 5);
            bool wasContentAdded = _streamingService.AddContent(contentToAdd);
            Assert.That(wasContentAdded, Is.True);
            Assert.That(_streamingService.GetContent(contentToAdd.Name, contentToAdd.ReleaseDate.Year, contentToAdd.ContentType), Is.EqualTo<IMediaContent>(contentToAdd));
        }
        [Test]
        public void AddContent_should_return_false_when_content_already_found()
        {
            IMediaContent contentToAdd = new Film("Inception", new DateOnly(2010, 1, 1), "Action", 148);
            bool wasContentAdded = _streamingService.AddContent(contentToAdd);
            Assert.That(wasContentAdded, Is.False);
        }
        [Test]
        public void RemoveContent_should_remove_content_when_content_found()
        {
            IMediaContent contentToRemove = new Film("Inception", new DateOnly(2010, 1, 1), "Action", 148);
            bool wasContentRemoved = _streamingService.RemoveContent(contentToRemove.Name, contentToRemove.ReleaseDate.Year, contentToRemove.ContentType);
            Assert.That(wasContentRemoved, Is.True);
            Assert.That(_streamingService.GetContent(contentToRemove.Name, contentToRemove.ReleaseDate.Year, contentToRemove.ContentType), Is.Null);
        }
        [Test]
        public void RemoveContent_should_return_false_when_content_not_found()
        {
            IMediaContent contentToRemove = new Film("Ghost Rider", new DateOnly(2007, 3, 2), "Action", 116);
            bool wasContentRemoved = _streamingService.RemoveContent(contentToRemove.Name, contentToRemove.ReleaseDate.Year, contentToRemove.ContentType);
            Assert.That(wasContentRemoved, Is.False);
        }
    }
    [TestFixture]
    public class StreamingServiceEmptyTest
    {
        private IStreamingService _streamingService { get; set; }
        private IMediaContent _content = new Film("Patema Inverted", new DateOnly(2013, 11, 9), "Fantasy", 99, 3);
        [SetUp]
        public void Setup()
        {
            _streamingService = new StreamingService("Notflix", 1099, new List<IMediaContent>());
        }
        [Test]
        public void SetRating_should_return_false_when_content_list_empty()
        {
            bool returnValue = _streamingService.SetRating(_content.Name, _content.ReleaseDate.Year, _content.ContentType, 2);
            Assert.That(returnValue, Is.False);
        }
        [Test]
        public void AddContent_should_add_book_when_content_list_empty()
        {
            bool returnValue = _streamingService.AddContent(_content);
            Assert.That(returnValue, Is.True);
            Assert.That(_streamingService.GetContent(_content.Name, _content.ReleaseDate.Year, _content.ContentType), Is.EqualTo<IMediaContent>(_content));
        }
        [Test]
        public void RemoveBook_should_return_false_when_content_list_empty()
        {
            bool returnValue = _streamingService.RemoveContent(_content.Name, _content.ReleaseDate.Year, _content.ContentType);
            Assert.That(returnValue, Is.False);
        }
        [Test]
        public void GetContent_should_return_null_when_content_list_empty()
        {
            IMediaContent returnedContent = _streamingService.GetContent(_content.Name, _content.ReleaseDate.Year, _content.ContentType);
            Assert.That(returnedContent, Is.Null);
        }
        [Test]
        public void GetAllContents_shouldReturn_empty_list_when_content_list_empty()
        {
            ReadOnlyCollection<IMediaContent> returnedList = _streamingService.GetAllContents();
            Assert.That(returnedList.Count(), Is.EqualTo(0));
        }
    }
}