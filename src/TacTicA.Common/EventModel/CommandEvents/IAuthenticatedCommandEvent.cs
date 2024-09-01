namespace TacTicA.Common.EventModel.CommandEvents
{
    public interface IAuthenticatedCommandEvent : ICommandEvent
    {
        Guid UserId { get; set; }
    }
}