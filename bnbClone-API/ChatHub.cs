using Microsoft.AspNetCore.SignalR;

namespace bnbClone_API
{
    public class ChatHub : Hub
    {
        public async Task JoinConversation(int conversationId)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId,$"conversation-{conversationId}");
        }
        public async Task LeaveConversation(int conversationId)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, $"conversation-{conversationId}");

        }
    }
}
