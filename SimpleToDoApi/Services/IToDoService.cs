using SimpleToDoApi.Models.Dtos;
using SimpleToDoApi.Models.Entities;
using SimpleToDoApi.Models.Results;

namespace SimpleToDoApi.Services
{
    public interface IToDoService
    {
        Task<ResultModel<Guid?>> CreateToDo(TodoCreateDto dto);
        Task<ResultModel<bool>> DeleteToDo(Guid id);
        Task<ResultModel<IEnumerable<Todo>>> GetAllTodos();
        Task<ResultModel<IEnumerable<Todo>>> GetIncomingTodos(int days);
        Task<ResultModel<Todo?>> GetSpecifiedTodo(Guid id);
        Task<ResultModel<bool>> MarkAsDone(Guid id);
        Task<ResultModel<bool>> SetTodoPercentage(Guid id, int percentage);
        Task<ResultModel<bool>> UpdateToDo(Guid id, TodoUpdateDto dto);
    }
}