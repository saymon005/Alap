using Microsoft.AspNetCore.SignalR;

namespace Alap.Hubs
{
    public class ChatHub : Hub
    {
        public async Task SendMessage(string user, string message)
        {
            await Clients.All.SendAsync("ReceiveMessage", user, message);
        }
        public async Task BotTyping()
        {
            await Clients.All.SendAsync("BotTyping");
        }

        public async Task BotDone()
        {
            await Clients.All.SendAsync("BotDone");
        }
        public async Task TypingIndicator(string user)
        {
            await Clients.Others.SendAsync("UserTyping", user);
        }
    }

}
