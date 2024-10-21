using SimpleToDoApi.Models.Dtos;
using SimpleToDoApi.Models.Entities;
using SimpleToDoApi.Models.Enums;
using SimpleToDoApi.Models.Results;
using SimpleToDoApi.Repositories;
using SimpleToDoApi.Tools;

namespace SimpleToDoApi.Services
{
    public sealed class ToDoService(ITodoRepository todoRepository) : IToDoService
    {
        private readonly ITodoRepository _todoRepository = todoRepository;

        public async Task<ResultModel<IEnumerable<Todo>>> GetAllTodos()
        {
            var result = await _todoRepository.GetAllAsync();
            return ResultModel<IEnumerable<Todo>>.Success(result);
        }

        public async Task<ResultModel<Todo?>> GetSpecifiedTodo(Guid id)
        {
            var result = await _todoRepository.GetByIdAsync(id);
            return ResultModel<Todo?>.Success(result);
        }

        public async Task<ResultModel<IEnumerable<Todo>>> GetIncomingTodos(int days)
        {
            var isEnumParsed = Enum.TryParse<IncomingTodoDateType>(days.ToString(), out var enumResult);

            if (!isEnumParsed || !Enum.GetValues<IncomingTodoDateType>().Any(x => (int)x == days))
                return ResultModel<IEnumerable<Todo>>.Error(["Invalid selected days type"]);

            var result = await _todoRepository.GetIncomingAsync(enumResult);

            return ResultModel<IEnumerable<Todo>>.Success(result);
        }

        public async Task<ResultModel<Guid?>> CreateToDo(TodoCreateDto dto)
        {
            if (dto.PercentComplete < 0 || dto.PercentComplete > 100)
                return ResultModel<Guid?>.Error(["Percent value should be between 0 and 100"]);

            if(dto.ExpiryDate < DateTime.UtcNow)
                return ResultModel<Guid?>.Error(["Expire date can not be past"]);

            var todo = TodoMapper.Map(dto);

            var result = await _todoRepository.AddAsync(todo);

            return ResultModel<Guid?>.Success(result);
        }

        public async Task<ResultModel<bool>> UpdateToDo(Guid id, TodoUpdateDto dto)
        {
            if (dto.PercentComplete < 0 || dto.PercentComplete > 100)
                return ResultModel<bool>.Error(["Percent value should be between 0 and 100"]);

            if (dto.ExpiryDate < DateTime.UtcNow)
                return ResultModel<bool>.Error(["Expire date can not be past"]);

            var todo = await _todoRepository.GetByIdAsync(id);

            if (todo is null)
                return ResultModel<bool>.Error(["Todo not found"]);

            TodoMapper.Map(dto, todo);

            var result = await _todoRepository.UpdateAsync(todo);

            if (!result)
                return ResultModel<bool>.Error(["Failed to update"]);

            return ResultModel<bool>.Success(result);
        }

        public async Task<ResultModel<bool>> SetTodoPercentage(Guid id, int percentage)
        {
            if (percentage < 0 || percentage > 100)
                return ResultModel<bool>.Error(["Percent value should be between 0 and 100"]);

            var todo = await _todoRepository.GetByIdAsync(id);

            if (todo is null)
                return ResultModel<bool>.Error(["Todo not found"]);

            todo.PercentComplete = percentage;

            return ResultModel<bool>.Success(true);
        }

        public async Task<ResultModel<bool>> DeleteToDo(Guid id)
        {
            var todo = await _todoRepository.GetByIdAsync(id);

            if (todo is null)
                return ResultModel<bool>.Error(["Todo not found"]);

            var result = await _todoRepository.DeleteAsync(todo);

            if (!result)
                return ResultModel<bool>.Error(["Failed to delete"]);

            return ResultModel<bool>.Success(true);
        }

        public async Task<ResultModel<bool>> MarkAsDone(Guid id)
        {
            var todo = await _todoRepository.GetByIdAsync(id);

            if (todo is null)
                return ResultModel<bool>.Error(["Todo not found"]);

            var result = await _todoRepository.MarkAsDone(todo);

            if (!result)
                return ResultModel<bool>.Error(["Failed to update"]);

            return ResultModel<bool>.Success(true);
        }
    }
}
