using Application.Abstractions.Authentication;
using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Domain.Todos;
using SharedKernel;

namespace Application.Todos.Delete;

internal sealed class DeleteTodoCommandHandler(
    ITodoRepository todoRepository,
    IUserContext userContext,
    IUnitOfWork unitOfWork)
    : ICommandHandler<DeleteTodoCommand>
{
    public async Task<Result> Handle(DeleteTodoCommand command, CancellationToken cancellationToken)
    {
        TodoItem? todoItem = await todoRepository.GetAssignedByIdAsync(
            command.TodoItemId,
            userContext.UserId,
            cancellationToken);

        if (todoItem is null)
        {
            return Result.Failure(TodoItemErrors.NotFound(command.TodoItemId));
        }

        todoRepository.Remove(todoItem);

        todoItem.Raise(new TodoItemDeletedDomainEvent(todoItem.Id));

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
