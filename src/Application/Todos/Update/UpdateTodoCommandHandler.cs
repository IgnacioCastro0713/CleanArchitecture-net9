using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Domain.Todos;
using SharedKernel;

namespace Application.Todos.Update;

internal sealed class UpdateTodoCommandHandler(
    ITodoRepository todoRepository,
    IUnitOfWork unitOfWork)
    : ICommandHandler<UpdateTodoCommand>
{
    public async Task<Result> Handle(UpdateTodoCommand command, CancellationToken cancellationToken)
    {
        TodoItem? todoItem = await todoRepository.GetByIdAsync(command.TodoItemId, cancellationToken);

        if (todoItem is null)
        {
            return Result.Failure(TodoItemErrors.NotFound(command.TodoItemId));
        }

        todoItem.Description = command.Description;

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
