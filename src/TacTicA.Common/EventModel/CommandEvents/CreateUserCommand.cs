namespace TacTicA.Common.EventModel.CommandEvents
{
    public class CreateUserCommand : ICommandEvent
    {
        public string Email { get; set; }
        public string Name { get; set; }
        public string Password { get; set; }
    }
}