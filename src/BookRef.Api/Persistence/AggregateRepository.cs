using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using BookRef.Api.Models.Framework;
using BookRef.Api.Users.Events;
using EventStore.Client;

namespace BookRef.Api.Persistence
{
    public class AggregateRepository
    {
        private readonly EventStoreClient _eventStore;

        public AggregateRepository(EventStoreClient eventStore)
        {
            _eventStore = eventStore;
        }

        public async Task SaveAsync<T>(T aggregate) where T : Aggregate, new()
        {
            var events = aggregate.GetChanges()
                .Select(@event => new EventData(
                    Uuid.NewUuid(),
                    @event.GetType().Name,
                    Encoding.UTF8.GetBytes(JsonSerializer.Serialize(@event))))
                .ToArray();

            if (!events.Any())
            {
                return;
            }

            var streamName = GetStreamName(aggregate, aggregate.Id);

            var result = await _eventStore.AppendToStreamAsync(streamName, StreamState.NoStream, events);
        }

        public async Task<T> LoadAsync<T>(Guid aggregateId) where T : Aggregate, new()
        {
            if (aggregateId == Guid.Empty)
                throw new ArgumentException("Value cannot be null or whitespace.", nameof(aggregateId));

            var aggregate = new T();
            var streamName = GetStreamName(aggregate, aggregateId);

            var events = _eventStore.ReadStreamAsync(
                    Direction.Forwards, streamName, StreamPosition.Start);

            var eventsList = new List<object>();
            var last = await events.LastAsync();
            await foreach (var e in events) {
                var data = Encoding.UTF8.GetString(e.Event.Data.ToArray());
                var obj = JsonSerializer.Deserialize<UserCreated>(data);
                eventsList.Add(obj);
            }
            aggregate.Load(last.Event.EventNumber.ToInt64(), eventsList);

            return aggregate;
        }

        private string GetStreamName<T>(T type, Guid aggregateId) => $"{type.GetType().Name}-{aggregateId}";
    }
}
