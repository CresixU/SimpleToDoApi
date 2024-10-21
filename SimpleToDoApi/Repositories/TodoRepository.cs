using Microsoft.EntityFrameworkCore;
using SimpleToDoApi.Data;
using SimpleToDoApi.Models.Entities;
using SimpleToDoApi.Models.Enums;
using SimpleToDoApi.Tools;

namespace SimpleToDoApi.Repositories
{
    public sealed class TodoRepository(TodoDbContext context) : ITodoRepository
    {
        private readonly TodoDbContext _context = context;
        private readonly TodoMapper _mapper = new();

        public async Task<IEnumerable<Todo>> GetAllAsync()
        {
            return await _context.Todos.ToListAsync();
        }

        public async Task<Todo?> GetByIdAsync(Guid id)
        {
            return await _context.Todos.FindAsync(id);
        }

        public async Task<IEnumerable<Todo>> GetIncomingAsync(IncomingTodoDateType dateType)
        {
            var now = DateTime.UtcNow;

            var secondDate = new DateTime(now.Year, now.Month, now.Day)
                .AddDays((int)dateType + 1).AddTicks(-1);

            return await _context.Todos
                .Where(t => t.ExpiryDate >= now && t.ExpiryDate <= secondDate)
                .ToListAsync();
        }

        public async Task<Guid> AddAsync(Todo todo)
        {
            _context.Todos.Add(todo);
            await _context.SaveChangesAsync();

            return todo.Id;
        }

        public async Task<bool> UpdateAsync(Todo todo)
        {
            _context.Entry(todo).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> DeleteAsync(Todo todo)
        {
            _context.Todos.Remove(todo);
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> SetCompletionAsync(Todo todo, int percentComplete)
        {
            todo.PercentComplete = percentComplete;
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> MarkAsDone(Todo todo)
        {
            todo.PercentComplete = 100;

            await _context.SaveChangesAsync();
            return true;
        }
    }
}
