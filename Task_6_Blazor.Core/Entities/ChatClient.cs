using Microsoft.AspNetCore.SignalR.Client;
using Task_6_ChatApi.Core.Entities;
using Task_6_ChatBlazor.Core.Interfaces;

namespace Task_6_ChatBlazor.Core.Entities;

public class ChatClient : IChatClient
{
    private readonly string _hubUrl;

    private bool _started;
    private User? _user;
    private HubConnection _hubConnection = null!;

    public ChatClient(string hubUrl)
    {
        if (string.IsNullOrEmpty(hubUrl))
        {
            throw  new ArgumentNullException(nameof(hubUrl));
        }

        _hubUrl = hubUrl.TrimEnd('/');
    }
        
    public event EventHandler<User>? OnConnected;
    public event EventHandler<User>? OnDisconnected;
    public event EventHandler<Message>? OnMessageReceived;

    public async Task<IEnumerable<User>> GetOnlineUsersAsync()
    {
        if (!_started)
        {
            throw new InvalidOperationException("Client not started");
        }

        return await _hubConnection.InvokeAsync<IEnumerable<User>>(HubConnectionMethodsName.GetOnlineUsers);
    }

    public async Task SendMessageAsync(string message)
    {
        if (!_started)
        {
            throw new InvalidOperationException("Client not started");
        }

        var msg = new Message(_user!, message);
        await _hubConnection.SendAsync(HubConnectionMethodsName.SendMessageAsync, msg);
    }


    public async Task StartAsync(User user)
    {
        if (_started)
        {
            return;
        }

        _hubConnection = new HubConnectionBuilder().WithUrl(_hubUrl).Build();


        _hubConnection.On<Message>(HubConnectionPrefixName.ReceivedMessage, message =>
        {
                OnMessageReceived?.Invoke(this, message);
        });

        _hubConnection.On<User>(HubConnectionPrefixName.Connected, user1 =>
        {
            OnConnected?.Invoke(this, user1);
        });

        _hubConnection.On<User>(HubConnectionPrefixName.Disconnected, user1 =>
        {
            OnDisconnected?.Invoke(this, user1);
        });

        await _hubConnection.StartAsync();
        await _hubConnection.SendAsync(HubConnectionMethodsName.RegisterAsync, user);

        _user = user;
        _started = true;
    }

    public async Task StopAsync()
    {
        if (!_started)
        {
            return;
        }

        await _hubConnection.StopAsync();
        await _hubConnection.DisposeAsync();

        _hubConnection = null!;
        _started = false;
    }


    public async ValueTask DisposeAsync()
    {
        await this.StopAsync();
    }
}