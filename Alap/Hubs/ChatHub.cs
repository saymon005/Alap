using Microsoft.AspNetCore.SignalR;

namespace Alap.Hubs
{
    public class ChatHub : Hub
    {
        public async Task SendMessage(string sender, string message)
        {
            await Clients.User(Context.UserIdentifier).SendAsync("ReceiveMessage", sender, message);
        }
        public async Task BotTyping()
        {
            await Clients.User(Context.UserIdentifier).SendAsync("BotTyping");
        }

        public async Task BotDone()
        {
            await Clients.User(Context.UserIdentifier).SendAsync("BotDone");
        }

        public async Task TypingIndicator()
        {
            var user = Context.User.Identity.Name;
            await Clients.Others.SendAsync("UserTyping", user);
        }
    }


}
