using SimpleToDoApi.Models.Entities;
using SimpleToDoApi.Models.Enums;

namespace SimpleToDoApi.Repositories
{
    public interface ITodoRepository
    {
        Task<IEnumerable<Todo>> GetAllAsync();
        Task<Todo?> GetByIdAsync(Guid id);
        Task<IEnumerable<Todo>> GetIncomingAsync(IncomingTodoDateType dateType);
        Task<Guid> AddAsync(Todo todo);
        Task<bool> UpdateAsync(Todo todo);
        Task<bool> DeleteAsync(Todo todo);
        Task<bool> SetCompletionAsync(Todo todo, int percentComplete);
        Task<bool> MarkAsDone(Todo todo);
    }
}
