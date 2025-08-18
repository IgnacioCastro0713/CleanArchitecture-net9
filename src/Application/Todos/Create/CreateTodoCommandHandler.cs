using Application.Abstractions.Authentication;
using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Domain.Todos;
using Domain.Users;
using Microsoft.EntityFrameworkCore;
using SharedKernel;

namespace Application.Todos.Create;

internal sealed class CreateTodoCommandHandler(
    IApplicationDbContext context,
    TimeProvider timeProvider,
    IUserContext userContext)
    : ICommandHandler<CreateTodoCommand, Guid>
{
    public async Task<Result<Guid>> Handle(CreateTodoCommand command, CancellationToken cancellationToken)
    {
        if (userContext.UserId != command.UserId)
        {
            return Result.Failure<Guid>(UserErrors.Unauthorized());
        }

        User? user = await context.Users
            .AsNoTracking()
            .SingleOrDefaultAsync(u => u.Id == command.UserId, cancellationToken);

        if (user is null)
        {
            return Result.Failure<Guid>(UserErrors.NotFound(command.UserId));
        }

        var todoItem = TodoItem.Create(
            user.Id,
            command.Description,
            command.Priority,
            command.DueDate,
            command.Labels,
            timeProvider.GetUtcNow().UtcDateTime
        );

        context.TodoItems.Add(todoItem);

        await context.SaveChangesAsync(cancellationToken);

        return todoItem.Id;
    }
}
