

using System.Collections.ObjectModel;
using NUnit.Framework.Internal;
using Moq;
using System.ComponentModel.Design;

namespace StreamingCatalogue.UnitTests
{
    [TestFixture]
    public class StreamingRegisterTests
    {
        private IStreamingService _streamingService { get; set; }
        private IStreamingRegister _streamingRegister { get; set; }
        [SetUp]
        public void Setup()
        {
            // var mock = new Mock<IStreamingService>();
            // mock.Setup(X500DistinguishedName=>)
            _streamingRegister = new StreamingRegister();
        }
        [Test]
        public void SetRating_should_be_invoked_in_all_streamingServices_when_content_not_found()
        {
            // Arrange
            List<Mock<IStreamingService>> mocks = new List<Mock<IStreamingService>>();
            List<IStreamingService> mockedStreamingServicesList = new List<IStreamingService>();
            for (int i = 0; i < 5; i++)
            {
                var mock = new Mock<IStreamingService>();
                mock.Setup(x => x.SetRating(It.IsAny<string>(), It.IsAny<int>(), It.IsAny<char>(), It.IsAny<int>())).Verifiable();
                mocks.Add(mock);
                mockedStreamingServicesList.Add(mock.Object);
            }
            _streamingRegister = new StreamingRegister(mockedStreamingServicesList);

            // Act
            _streamingRegister.SetRating("Hulk", 2003, 'F', 2);

            // Assert
            foreach (var m in mocks)
            {
                m.VerifyAll();
            }
        }
        [Test]
        public void SetRating_should_return_false_when_content_not_found()
        {
            // Arrange

            // Act
            bool returnedValue = _streamingRegister.SetRating("Hulk", 2003, 'F', 2);

            //Assert
            Assert.That(returnedValue, Is.False);
        }
        [Test]
        public void SetRating_should_return_true_when_Content_found()
        {
            // Arrange
            var mock = new Mock<IStreamingService>();
            mock.Setup(x => x.SetRating(It.IsAny<string>(), It.IsAny<int>(), It.IsAny<char>(), It.IsAny<int>())).Returns(true);

            var mock2 = new Mock<IStreamingService>();
            mock.Setup(x => x.SetRating(It.IsAny<string>(), It.IsAny<int>(), It.IsAny<char>(), It.IsAny<int>())).Returns(true);

            var mock3 = new Mock<IStreamingService>();
            mock.Setup(x => x.SetRating(It.IsAny<string>(), It.IsAny<int>(), It.IsAny<char>(), It.IsAny<int>())).Returns(true);

            _streamingRegister = new StreamingRegister([mock.Object]);

            // Act
            bool returnValue = _streamingRegister.SetRating("Hulk", 2003, 'F', 3);

            // Assert
            Assert.That(returnValue, Is.True);
        }
        [Test]
        public void GetAllContentsInStreamingService_should_call_GetAllContents()
        {
            // Arrange
            var mock = new Mock<IStreamingService>();
            mock.Setup(x => x.GetAllContents()).Returns(new ReadOnlyCollection<IMediaContent>([])).Verifiable();
            mock.Setup(x => x.Name).Returns("Notflix");
            _streamingRegister = new StreamingRegister([mock.Object]);

            // Act
            _streamingRegister.GetAllContentsInStreamingService("Notflix");

            // Assert
            mock.VerifyAll();
        }
        [Test]
        public void GetContentAndService_should_call_call_GetContent_in_all_StreamingServices_until_correct()
        {
            // Arrange
            var mock1 = new Mock<IStreamingService>();
            mock1.Setup(x => x.GetContent(It.IsAny<string>(), It.IsAny<int>(), It.IsAny<char>())).Returns(value: null).Verifiable();

            var mock2 = new Mock<IStreamingService>();
            mock2.Setup(x => x.GetContent("Hulk", 2003, 'F')).Returns(new Film("Hulk", new DateOnly(2003, 7, 3), "Action", 138)).Verifiable();

            var mock3 = new Mock<IStreamingService>();
            mock3.Setup(x => x.GetContent(It.IsAny<string>(), It.IsAny<int>(), It.IsAny<char>())).Returns(value: null).Verifiable();

            _streamingRegister = new StreamingRegister([mock1.Object, mock2.Object, mock3.Object]);

            // Act
            (string StreamingService, IMediaContent Content)? returnedValue = _streamingRegister.GetContentAndService("Hulk", 2003, 'F');

            // Assert
            mock1.VerifyAll();
            mock2.VerifyAll();
            mock3.Verify(x => x.GetContent(It.IsAny<string>(), It.IsAny<int>(), It.IsAny<char>()), Times.Never);

        }


        [Test]
        public void RemoveContent_should_call_RemoveContent_on_all_StreamingServices_until_found()
        {
            // Arrange
            var mock1 = new Mock<IStreamingService>();
            mock1.Setup(x => x.RemoveContent(It.IsAny<string>(), It.IsAny<int>(), It.IsAny<char>())).Returns(false).Verifiable();

            var mock2 = new Mock<IStreamingService>();
            mock2.Setup(x => x.RemoveContent("Hulk", 2003, 'F')).Returns(true).Verifiable();

            var mock3 = new Mock<IStreamingService>();
            mock3.Setup(x => x.RemoveContent(It.IsAny<string>(), It.IsAny<int>(), It.IsAny<char>())).Returns(false).Verifiable();

            _streamingRegister = new StreamingRegister([mock1.Object, mock2.Object, mock3.Object]);

            // Act
            bool returnValue = _streamingRegister.RemoveContent("Hulk", 2003, 'F');

            // Assert
            Assert.That(returnValue, Is.True);
            mock1.VerifyAll();
            mock2.VerifyAll();
            mock3.Verify(x => x.RemoveContent(It.IsAny<string>(), It.IsAny<int>(), It.IsAny<char>()), Times.Never());
        }

        [Test]
        public void GetAllStreamingServices_should_return_all_StreamingServices()
        {
            // Arrange
            var mock1 = new Mock<IStreamingService>();
            mock1.Setup(x => x.Name).Returns("Notflix");
            mock1.Setup(x => x.Price).Returns(12.99M);

            var mock2 = new Mock<IStreamingService>();
            mock2.Setup(x => x.Name).Returns("Dasny");
            mock2.Setup(x => x.Price).Returns(10.99M);

            var mock3 = new Mock<IStreamingService>();
            mock3.Setup(x => x.Name).Returns("AmazingPrimed");
            mock3.Setup(x => x.Price).Returns(14.99M);

            List<IStreamingService> mockedServices = new List<IStreamingService>([mock1.Object, mock2.Object, mock3.Object]);

            _streamingRegister = new StreamingRegister(mockedServices);

            // Act
            ReadOnlyCollection<IStreamingService> allServices = _streamingRegister.GetAllStreamingServices();

            // Assert
            for (int i = 0; i < allServices.Count; i++)
            {
                Assert.That(allServices[i], Is.EqualTo(mockedServices[i]));
            }
        }
        [Test]
        public void GetStreamingService_should_return_null_when_not_found()
        {
            // Arrange
            var mock1 = new Mock<IStreamingService>();
            mock1.Setup(x => x.Name).Returns("Notflix");
            mock1.Setup(x => x.Price).Returns(12.99M);

            var mock2 = new Mock<IStreamingService>();
            mock2.Setup(x => x.Name).Returns("Dasny");
            mock2.Setup(x => x.Price).Returns(10.99M);

            var mock3 = new Mock<IStreamingService>();
            mock3.Setup(x => x.Name).Returns("AmazingPrimed");
            mock3.Setup(x => x.Price).Returns(14.99M);

            List<IStreamingService> mockedServices = new List<IStreamingService>([mock1.Object, mock2.Object, mock3.Object]);

            _streamingRegister = new StreamingRegister(mockedServices);

            // Act

            IStreamingService? service = _streamingRegister.GetStreamingService("Ruko");

            // Assert
            Assert.That(service, Is.Null);
        }
        [Test]
        public void GetStreamingService_should_return_StreamingService_when_found()
        {
            // Arrange
            var mock1 = new Mock<IStreamingService>();
            mock1.SetupGet(x => x.Name).Returns("Notflix");
            mock1.Setup(x => x.Price).Returns(12.99M);

            var mock2 = new Mock<IStreamingService>();
            mock2.SetupGet(x => x.Name).Returns("Dasny");
            mock2.Setup(x => x.Price).Returns(10.99M);

            var mock3 = new Mock<IStreamingService>();
            mock3.SetupGet(x => x.Name).Returns("AmazingPrimed");
            mock3.Setup(x => x.Price).Returns(14.99M);

            List<IStreamingService> mockedServices = new List<IStreamingService>([mock1.Object, mock2.Object, mock3.Object]);

            _streamingRegister = new StreamingRegister(mockedServices);

            // Act

            IStreamingService? service = _streamingRegister.GetStreamingService("Notflix");

            // Assert
            Assert.That(service, Is.EqualTo(mock1.Object));
        }
        [Test]
        public void AddStreamingService_should_AddStreamingService_when_not_found()
        {
            // Arrange
            var mock1 = new Mock<IStreamingService>();
            mock1.Setup(x => x.Name).Returns("Notflix");

            var mock2 = new Mock<IStreamingService>();
            mock2.Setup(x => x.Name).Returns("Dasny");

            var mock3 = new Mock<IStreamingService>();
            mock2.Setup(x => x.Name).Returns("AmazingPrimed");

            List<IStreamingService> mockedServices = new List<IStreamingService>([mock1.Object, mock2.Object, mock3.Object]);

            _streamingRegister = new StreamingRegister(mockedServices);

            var mockToAdd = new Mock<IStreamingService>();
            mockToAdd.Setup(x => x.Name).Returns("Ruko");

            // Act

            bool wasServiceAdded = _streamingRegister.AddStreamingService(mockToAdd.Object);
            IStreamingService? serviceAdded = _streamingRegister.GetStreamingService("Ruko");

            // Assert

            Assert.That(wasServiceAdded, Is.True);
            Assert.That(serviceAdded, Is.EqualTo(mockToAdd.Object));
        }
        [Test]
        public void AddStreamingService_should_return_false_when_already_found()
        {
            // Arrange
            var mock1 = new Mock<IStreamingService>();
            mock1.Setup(x => x.Name).Returns("Notflix");
            mock1.Setup(x => x.Price).Returns(12.99M);

            var mock2 = new Mock<IStreamingService>();
            mock2.Setup(x => x.Name).Returns("Dasny");
            mock2.Setup(x => x.Price).Returns(10.99M);

            var mock3 = new Mock<IStreamingService>();
            mock3.Setup(x => x.Name).Returns("AmazingPrimed");
            mock3.Setup(x => x.Price).Returns(14.99M);

            List<IStreamingService> mockedServices = new List<IStreamingService>([mock1.Object, mock2.Object, mock3.Object]);

            _streamingRegister = new StreamingRegister(mockedServices);

            var mockToAdd = new Mock<IStreamingService>();
            mockToAdd.Setup(x => x.Name).Returns("Dasny");

            // Act

            bool wasServiceAdded = _streamingRegister.AddStreamingService(mockToAdd.Object);
            IStreamingService? serviceAdded = _streamingRegister.GetStreamingService("Notflix");

            // Assert

            Assert.That(wasServiceAdded, Is.False);
        }
        [Test]
        public void AddContent_should_return_false_when_found()
        {
            // Arrange
            IMediaContent newContent = new Film("Hulk", new DateOnly(2003, 7, 3), "Action", 138);

            var mock1 = new Mock<IStreamingService>();
            mock1.Setup(x => x.Name).Returns("Notflix");
            mock1.Setup(x => x.Price).Returns(12.99M);
            mock1.Setup(x => x.GetAllContents()).Returns(new ReadOnlyCollection<IMediaContent>(new List<IMediaContent>()));

            var mock2 = new Mock<IStreamingService>();
            mock2.Setup(x => x.Name).Returns("Dasny");
            mock2.Setup(x => x.Price).Returns(10.99M);
            mock2.Setup(x => x.GetAllContents()).Returns(new ReadOnlyCollection<IMediaContent>(new List<IMediaContent>([newContent])));
            mock2.Setup(x => x.GetContent(It.IsAny<string>(), It.IsAny<int>(), It.IsAny<char>())).Returns(newContent);

            var mock3 = new Mock<IStreamingService>();
            mock3.Setup(x => x.Name).Returns("AmazingPrimed");
            mock3.Setup(x => x.Price).Returns(14.99M);
            mock3.Setup(x => x.GetAllContents()).Returns(new ReadOnlyCollection<IMediaContent>(new List<IMediaContent>()));

            List<IStreamingService> mockedServices = new List<IStreamingService>([mock1.Object, mock2.Object, mock3.Object]);

            _streamingRegister = new StreamingRegister(mockedServices);

            // Act
            bool wasContentAdded = _streamingRegister.AddContent(newContent, "AmazingPrimed");

            // Assert
            Assert.That(wasContentAdded, Is.False);
        }
        [Test]
        public void AddContent_should_add_content_when_not_found()
        {
            // Arrange
            IMediaContent newContent = new Film("Hulk", new DateOnly(2003, 7, 3), "Action", 138);

            var mock1 = new Mock<IStreamingService>();
            mock1.Setup(x => x.Name).Returns("Notflix");
            mock1.Setup(x => x.Price).Returns(12.99M);
            mock1.Setup(x => x.GetAllContents()).Returns(new ReadOnlyCollection<IMediaContent>(new List<IMediaContent>()));

            var mock2 = new Mock<IStreamingService>();
            mock2.Setup(x => x.Name).Returns("Dasny");
            mock2.Setup(x => x.Price).Returns(10.99M);
            mock2.Setup(x => x.GetAllContents()).Returns(new ReadOnlyCollection<IMediaContent>(new List<IMediaContent>()));

            var mock3 = new Mock<IStreamingService>();
            mock3.Setup(x => x.Name).Returns("AmazingPrimed");
            mock3.Setup(x => x.Price).Returns(14.99M);
            mock3.Setup(x => x.AddContent(It.IsAny<IMediaContent>())).Returns(true);
            mock3.Setup(x => x.GetAllContents()).Returns(new ReadOnlyCollection<IMediaContent>(new List<IMediaContent>()));

            List<IStreamingService> mockedServices = new List<IStreamingService>([mock1.Object, mock2.Object, mock3.Object]);

            _streamingRegister = new StreamingRegister(mockedServices);

            // Act
            bool wasContentAdded = _streamingRegister.AddContent(newContent, "AmazingPrimed");

            // Assert
            Assert.That(wasContentAdded, Is.True);
            mock3.Verify(x => x.AddContent(It.IsAny<IMediaContent>()));
        }
        [Test]
        public void RemoveStreamingService_should_return_false_when_not_found()
        {
            // Arrange
            IMediaContent newContent = new Film("Hulk", new DateOnly(2003, 7, 3), "Action", 138);

            var mock1 = new Mock<IStreamingService>();
            mock1.Setup(x => x.Name).Returns("Notflix");
            mock1.Setup(x => x.Price).Returns(12.99M);

            var mock2 = new Mock<IStreamingService>();
            mock2.Setup(x => x.Name).Returns("Dasny");
            mock2.Setup(x => x.Price).Returns(10.99M);

            var mock3 = new Mock<IStreamingService>();
            mock3.Setup(x => x.Name).Returns("AmazingPrimed");
            mock3.Setup(x => x.Price).Returns(14.99M);

            List<IStreamingService> mockedServices = new List<IStreamingService>([mock1.Object, mock2.Object, mock3.Object]);

            _streamingRegister = new StreamingRegister(mockedServices);

            // Act
            bool wasStreamingServiceRemoved = _streamingRegister.RemoveStreamingService("Ruko");

            // Assert
            Assert.That(wasStreamingServiceRemoved, Is.False);
        }
        [Test]
        public void RemoveStreamingService_should_remove_streaming_service_when_found()
        {
            // Arrange
            IMediaContent newContent = new Film("Hulk", new DateOnly(2003, 7, 3), "Action", 138);

            var mock1 = new Mock<IStreamingService>();
            mock1.Setup(x => x.Name).Returns("Notflix");
            mock1.Setup(x => x.Price).Returns(12.99M);

            var mock2 = new Mock<IStreamingService>();
            mock2.Setup(x => x.Name).Returns("Dasny");
            mock2.Setup(x => x.Price).Returns(10.99M);

            var mock3 = new Mock<IStreamingService>();
            mock3.Setup(x => x.Name).Returns("AmazingPrimed");
            mock3.Setup(x => x.Price).Returns(14.99M);

            List<IStreamingService> mockedServices = new List<IStreamingService>([mock1.Object, mock2.Object, mock3.Object]);

            _streamingRegister = new StreamingRegister(mockedServices);

            // Act
            bool wasStreamingServiceRemoved = _streamingRegister.RemoveStreamingService("Notflix");

            // Assert
            Assert.That(wasStreamingServiceRemoved, Is.True);
            Assert.That(_streamingRegister.GetStreamingService("Notflix"), Is.Null);
        }
    }
}