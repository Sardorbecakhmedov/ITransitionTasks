namespace Task_6_ChatApi.Core.Entities;

public class Message
{
    public User Sender { get; set; }
    public string MessageText { get; set; }
    public DateTime TimeStamp { get; set; } = DateTime.UtcNow;

    public Message(User sender, string messageText)
    {
        Sender = sender;
        MessageText = messageText;
    }

}