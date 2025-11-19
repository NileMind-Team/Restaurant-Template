using NileFood.Domain.Entities.Identity;

namespace NileFood.Domain.Entities;
public class Notification
{
    public int Id { get; set; }

    public string SenderId { get; set; } = null!;
    public ApplicationUser Sender { get; set; } = null!;

    public SenderType SenderType { get; set; }
    public ReceiverType ReceiverType { get; set; }

    public string Title { get; set; } = null!;
    public string Message { get; set; } = null!;
    public string Type { get; set; } = null!;

    public bool IsGeneral { get; set; } = false;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

}

public enum SenderType
{
    Admin,
    Restaurant,
    Branch
}

public enum ReceiverType
{
    User,
    Restaurant,
    All
}