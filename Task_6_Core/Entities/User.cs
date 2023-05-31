namespace Task_6_ChatApi.Core.Entities;

public class User
{
    public string Username { get; set; }

    public User(string userName)
    {
        Username = userName;
    }

    public override string ToString() => Username;
    public override bool Equals(object? obj)
    {
        if (obj == null || GetType() != obj.GetType())
        {
            return false;
        }

        var user = obj as User;

        return Username.Equals(user?.Username);
    }
    public override int GetHashCode()
    {
        return Username.GetHashCode();
    }
    public static User Unknown() => new("[unknown]");
}

