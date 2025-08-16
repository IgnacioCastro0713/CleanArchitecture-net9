using Domain.Todos;
using Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Todos;

internal sealed class TodoRepository : Repository<TodoItem>, ITodoRepository
{
    public TodoRepository(ApplicationDbContext context) : base(context)
    {
    }

    public async Task<TodoItem?> GetAssignedByIdAsync(Guid id, Guid userId, CancellationToken cancellationToken)
    {
        return await DbContext.TodoItems
            .SingleOrDefaultAsync(t => t.Id == id && t.UserId == userId, cancellationToken);
    }
}
