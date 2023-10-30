namespace AuthManager.Domain.Primitives;

public abstract class Entity
{
    public Guid Id { get; protected set; }
    public DateTime CreatedOn { get; protected set; }
    public DateTime? DeletedOn { get; protected set; }
}
