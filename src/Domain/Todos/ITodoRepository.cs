namespace Domain.Todos;

public interface ITodoRepository
{
    Task<TodoItem?> GetByIdAsync(Guid id, CancellationToken cancellationToken);
    Task<TodoItem?> GetAssignedByIdAsync(Guid id, Guid userId, CancellationToken cancellationToken);
    void Add(TodoItem todoItem);
    void Remove(TodoItem todoItem);
}
