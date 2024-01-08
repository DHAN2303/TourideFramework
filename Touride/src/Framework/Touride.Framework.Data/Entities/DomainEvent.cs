using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Touride.Framework.Data.Entities
{
    public abstract class DomainEvent
    {
        private List<INotification> _domainEvents;

        private List<(Type type, object @event)> _persistentDomainEvents;

        [JsonIgnore]
        public IReadOnlyCollection<(Type type, object @event)> IntegrationEvents => _persistentDomainEvents?.AsReadOnly();

        [JsonIgnore]
        public IReadOnlyCollection<INotification> DomainEvents => _domainEvents?.AsReadOnly();

        public void AddDomainEvent(INotification eventItem)
        {
            _domainEvents = _domainEvents ?? new List<INotification>();
            _domainEvents.Add(eventItem);
        }

        public void AddIntegrationEvent<T>(object eventItem)
        {
            _persistentDomainEvents = _persistentDomainEvents ?? new List<(Type type, object @event)>();

            var type = typeof(T);

            _persistentDomainEvents.Add((type, eventItem));
        }

        public void RemoveDomainEvent(INotification eventItem)
        {
            _domainEvents?.Remove(eventItem);
        }

        public void ClearDomainEvents()
        {
            _domainEvents?.Clear();
        }

        public void ClearPersistentDomainEvents()
        {
            _persistentDomainEvents?.Clear();
        }

        public static bool operator ==(DomainEvent? left, DomainEvent right)
        {
            if (Equals(left, null))
                return (Equals(right, null)) ? true : false;
            else
                return left.Equals(right);
        }

        public static bool operator !=(DomainEvent? left, DomainEvent right)
        {
            return !(left == right);
        }
    }
}
