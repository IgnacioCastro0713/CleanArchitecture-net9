using SharedKernel;

namespace Domain.Todos;

public sealed class TodoItem : Entity
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public string Description { get; set; }
    public DateTime? DueDate { get; set; }
    public List<string> Labels { get; set; } = [];
    public bool IsCompleted { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? CompletedAt { get; set; }
    public Priority Priority { get; set; }

    public static TodoItem Create(
        Guid userId,
        string description,
        Priority priority,
        DateTime? dueDate,
        List<string> labels,
        DateTime createdAt)
    {
        var todoItem = new TodoItem
        {
            Id = Guid.CreateVersion7(),
            UserId = userId,
            Description = description,
            Priority = priority,
            DueDate = dueDate,
            Labels = labels,
            IsCompleted = false,
            CreatedAt = createdAt
        };

        todoItem.Raise(new TodoItemCreatedDomainEvent(todoItem.Id));

        return todoItem;
    }

    public void Complete(DateTime completeDateUtc)
    {
        IsCompleted = true;
        CompletedAt = completeDateUtc;
        Raise(new TodoItemCompletedDomainEvent(Id));
    }
}
