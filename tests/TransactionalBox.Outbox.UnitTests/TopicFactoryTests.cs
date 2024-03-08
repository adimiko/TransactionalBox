using TransactionalBox.Outbox.Internals;
using TransactionalBox.Outbox.UnitTests.SeedWork;
using Xunit;

namespace TransactionalBox.Outbox.UnitTests
{
    public class TopicFactoryTests
    {
        private readonly TopicFactory _topicFactory = new TopicFactory();

        [Fact]
        public void TopicFactoryTest()
        {
            // Arrange
            const string serviceName = "ModuleName";
            const char separator = '-';
            var message = new TestMessage();

            // Act
            var actualTopic = _topicFactory.Create(serviceName, message);

            // Assert
            var expectedTopic = serviceName + separator + "TestMessage";

            Assert.Equal(expectedTopic, actualTopic);
        }
    }
}
