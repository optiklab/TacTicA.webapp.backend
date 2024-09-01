using MongoDB.Bson.Serialization.Attributes;
using System;
using TacTicA.Common.Exceptions;

namespace TacTicA.Services.Items.Domain.Models
{
    public class Item
    {
        public Guid Id { get;  set; }
        public string Name { get;  set; }
        public string CategoryName { get;  set; }
        public string Description { get;  set; }
        public Guid UserId { get;  set; }
        public DateTime CreatedAt { get;  set; }

        //[BsonConstructor]
        public Item(Guid id, string categoryName, Guid userId, string name, string description, DateTime createdAt)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ActioException("empty_item_name", $"Item: name can not be empty.");
            }

            /*if (id != null)
            {
                Id = id.Value;
            }
            else */
                Id = Guid.NewGuid();

            Name = name.ToLowerInvariant();
            CategoryName = categoryName;
            Description = description;
            UserId = userId;
            CreatedAt = createdAt;
        }
    }
}