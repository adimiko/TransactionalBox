using TransactionalBox.Internals;
using Xunit;

namespace TransactionalBox.UnitTests
{
    public sealed class TopicFactoryTests
    {
        private readonly ITopicFactory _topicFactory = new TopicFactory();

        [Fact]
        public void TopicFactoryTest()
        {
            // Arrange
            const string serviceName = "ModuleName";
            const char separator = '.';
            var messageName = "TestMessage";

            // Act
            var actualTopic = _topicFactory.Create(serviceName, messageName);

            // Assert
            var expectedTopic = serviceName + separator + messageName;

            Assert.Equal(expectedTopic, actualTopic);
        }
    }
}
