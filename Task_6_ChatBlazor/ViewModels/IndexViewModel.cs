using MagicMvvm;
using Task_6_ChatApi.Core.Entities;
using Task_6_ChatBlazor.Core.Entities;

namespace Task_6_ChatBlazor.ViewModels;

public class IndexViewModel : ViewModelBase
{
    private readonly ChatClient _client;
    public IndexViewModel( )
    {
        _client = new ChatClient("https://localhost:7157/chat");

        _client.OnConnected += (obj, user) =>
        {
            if (!OnlineUsers.Contains(user, new UserComparer()))
            {
                OnlineUsers.Add(user);
                StateHasChanged();
            }
        };

        _client.OnDisconnected += (obj, user) =>
        {
            if(OnlineUsers.Contains(user, new UserComparer()))
            {
                OnlineUsers.Remove(user);
                StateHasChanged();
            }
        };

        _client.OnMessageReceived += (obj, user) =>
        {
            Messages.Add(user);
            StateHasChanged();
        };
    }

    private void _client_OnDisconnected(object? sender, User e)
    {
        throw new NotImplementedException();
    }

    public bool StartedChat { get; set; }
    public string? CurrentUsername { get; set; } 
    public string? CurrentMessage { get; set; }
    public User? CurrentUser { get; set;}
    public string Error { get; set; }   
    public List<Message> Messages { get; set; } = new List<Message>();
    public List<User> OnlineUsers { get; set; } = new List<User>();


    public override void OnInitialized()
    {
        Error = string.Empty;
    }

    public async Task StartChatAsync()
    {
        if (string.IsNullOrWhiteSpace(CurrentUsername))
        {
            Error = "Please enter your name";
            return;
        }

        StartedChat = true;
        CurrentUser = new User(CurrentUsername);

        await _client.StartAsync(CurrentUser);

        OnlineUsers = (await _client.GetOnlineUsersAsync()).ToList();
    }

    public async Task SendMessageAsync()
    {
        if (string.IsNullOrEmpty(CurrentMessage))
        {
            Error = "Please enter message";
            return;
        }

        await _client.SendMessageAsync(CurrentMessage);
        CurrentMessage = string.Empty;
    }
} 