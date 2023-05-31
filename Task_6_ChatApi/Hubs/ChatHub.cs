using Microsoft.AspNetCore.SignalR;
using Task_6_ChatApi.Core.Entities;

namespace Task_6_ChatApiServer.Hubs;

public class ChatHub : Hub
{
    private static readonly Dictionary<string, User> _users = new();

    public async Task SendMessageAsync(Message message)
    {
        await this.Clients.All.SendAsync(HubConnectionPrefixName.ReceivedMessage, message);
    }


    public async Task RegisterAsync(User user)
    {
        var connectionId = Context.ConnectionId;

        if (_users.ContainsKey(connectionId))
        {
            return;
        }

        _users.Add(connectionId, user);

        await Clients.Others.SendAsync(HubConnectionPrefixName.Connected, user);

        var message = new Message(user, $"{user.Username} joined the chat");

        await Clients.Others.SendAsync(HubConnectionPrefixName.ReceivedMessage, message);
        await Clients.Others.SendAsync(HubConnectionPrefixName.Connected, user);
    }

    public IEnumerable<User> GetOnlineUsers()
    {
        return _users.Values;
    } 

    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        var connectionId = Context.ConnectionId;

        if (_users.TryGetValue(connectionId, out User? user))
        {
            user = User.Unknown();
        }

        _users.Remove(connectionId);

        var msg = new Message(user, $"{user.Username} has left the chat");

        await Clients.Others.SendAsync(HubConnectionPrefixName.ReceivedMessage, msg);
        await Clients.Others.SendAsync(HubConnectionPrefixName.Disconnected, user);
    }
}