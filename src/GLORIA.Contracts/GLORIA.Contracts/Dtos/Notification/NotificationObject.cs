using GLORIA.Contracts.Enums;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace GLORIA.Contracts.Dtos.Notification
{
    public class NotificationObject
    {
        public NotificationObjectType Type { get; set; }

        [BsonRepresentation(BsonType.String)]
        public Guid Id { get; set; }
    }
}
