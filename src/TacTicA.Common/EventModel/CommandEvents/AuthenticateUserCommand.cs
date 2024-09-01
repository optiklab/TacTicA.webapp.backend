namespace TacTicA.Common.EventModel.CommandEvents
{
    public class AuthenticateUserCommand : ICommandEvent
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }
}