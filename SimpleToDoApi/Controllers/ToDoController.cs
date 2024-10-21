using SimpleToDoApi.Models.Dtos;
using SimpleToDoApi.Services;

namespace SimpleToDoApi.Controllers
{
    public static class ToDoController
    {
        public static void AddEndpoints(this WebApplication app)
        {
            app.MapPost("/todos", async (IToDoService _todoService, TodoCreateDto dto) =>
            {
                var result = await _todoService.CreateToDo(dto);
                if (result.IsSucces)
                {
                    return Results.Created($"/todos/{result.Result}", result);
                }
                return Results.BadRequest(result);
            });

            app.MapDelete("/todos/{id:guid}", async (IToDoService _todoService, Guid id) =>
            {
                var result = await _todoService.DeleteToDo(id);
                if (result.IsSucces)
                {
                    return Results.Ok(result);
                }
                return Results.NotFound(result);
            });

            app.MapGet("/todos", async (IToDoService _todoService) =>
            {
                var result = await _todoService.GetAllTodos();
                return Results.Ok(result);
            });

            app.MapGet("/todos/incoming/{days:int}", async (IToDoService _todoService, int days) =>
            {
                var result = await _todoService.GetIncomingTodos(days);
                return Results.Ok(result);
            });

            app.MapGet("/todos/{id:guid}", async (IToDoService _todoService, Guid id) =>
            {
                var result = await _todoService.GetSpecifiedTodo(id);
                if (result.IsSucces && result.Result != null)
                {
                    return Results.Ok(result);
                }
                return Results.NotFound(result);
            });

            app.MapPut("/todos/{id:guid}/markasdone", async (IToDoService _todoService, Guid id) =>
            {
                var result = await _todoService.MarkAsDone(id);
                if (result.IsSucces)
                {
                    return Results.Ok(result);
                }
                return Results.NotFound(result);
            });

            app.MapPut("/todos/{id:guid}/setpercentage/{percentage:int}", async (IToDoService _todoService, Guid id, int percentage) =>
            {
                var result = await _todoService.SetTodoPercentage(id, percentage);
                if (result.IsSucces)
                {
                    return Results.Ok(result);
                }
                return Results.NotFound(result);
            });

            app.MapPut("/todos/{id:guid}", async (IToDoService _todoService, Guid id, TodoUpdateDto dto) =>
            {
                var result = await _todoService.UpdateToDo(id, dto);
                if (result.IsSucces)
                {
                    return Results.Ok(result);
                }
                return Results.NotFound(result);
            });
        }
    }
}
