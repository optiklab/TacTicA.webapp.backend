using System;

namespace TacTicA.ApiGateway.Models
{
    /// <summary>
    /// Flattened DTO is a pattern where model objects of one service (Services.Items in our case)
    /// can be stored in the database of the current service (Api Gateway) in the same or slightly changed way,
    /// depending on requirements or business rules.
    /// Examples of business cases: Fraud prevention analysis, etc.
    /// </summary>
    public class FlattenedItem
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Category { get; set; }
        public string Description { get; set; }
        public Guid UserId { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}