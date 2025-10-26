using Domain.Abstractions;

namespace Domain.Users;

public class User : Entity<UserId>
{
    public User(long id, string raw) : base(new UserId(id))
    {
        Raw = raw;
    }
    
    private User() : base()
    {
    }
    
    public string Raw { get; set; }  
}