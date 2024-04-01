using System.Text.Json;
using System.Text.Json.Serialization;
using TransactionalBox.Outbox.Serialization;

namespace TransactionalBox.Outbox.Internals.Serializers
{
    internal sealed class OutboxSerializer : IOutboxSerializer
    {
        private static JsonSerializerOptions _options = new JsonSerializerOptions()
        {
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingDefault,
        };

        public string Serialize<T>(T outboxMessage)
            where T : class, IOutboxMessagePayload
        {
            return JsonSerializer.Serialize(outboxMessage, _options);
        }
    }
}
