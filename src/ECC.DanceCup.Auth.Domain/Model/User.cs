namespace ECC.DanceCup.Auth.Domain.Model;

public class User
{
    public User(
        long id,
        int version,
        DateTime createdAt,
        DateTime changedAt,
        string username,
        byte[] passwordHash,
        byte[] passwordSalt)
    {
        Id = id;
        Version = version;
        CreatedAt = createdAt;
        ChangedAt = changedAt;
        Username = username;
        PasswordHash = passwordHash;
        PasswordSalt = passwordSalt;
    }

    public long Id { get; }
    
    public int Version { get; }
    
    public DateTime CreatedAt { get; }
    
    public DateTime ChangedAt { get; }
    
    public string Username { get; }
    
    public byte[] PasswordHash { get; }
    
    public byte[] PasswordSalt { get; }
}