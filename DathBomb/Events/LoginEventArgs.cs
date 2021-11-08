using ChatCore.Interfaces;

namespace StreamPartyCommand.Events
{
    public class LoginEventArgs
    {
        public IChatService ChatService { get; private set; }

        public LoginEventArgs(IChatService service)
        {
            this.ChatService = service;
        }
    }
}
