using System.Diagnostics.CodeAnalysis;

namespace Task_6_ChatApi.Core.Entities;

public class UserComparer : IEqualityComparer<User>
{
    public bool Equals(User x, User y)
    {
        return x?.Equals(y) ?? false;
    }

    public int GetHashCode([DisallowNull] User obj)
    {
        return obj.GetHashCode();
    }
}