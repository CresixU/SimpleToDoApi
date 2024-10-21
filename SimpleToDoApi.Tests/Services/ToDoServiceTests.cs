using FluentAssertions;
using FluentAssertions.Common;
using Moq;
using SimpleToDoApi.Models.Dtos;
using SimpleToDoApi.Models.Entities;
using SimpleToDoApi.Models.Enums;
using SimpleToDoApi.Repositories;
using SimpleToDoApi.Services;

namespace SimpleToDoApi.Tests.Services
{
    public class ToDoServiceTests
    {
        private readonly Mock<ITodoRepository> _todoRepositoryMock;
        private readonly ToDoService _toDoService;

        public ToDoServiceTests()
        {
            _todoRepositoryMock = new Mock<ITodoRepository>();
            _toDoService = new ToDoService(_todoRepositoryMock.Object);
        }

        [Fact]
        public async Task GetAllTodos_ShouldReturnSuccessResultWithTodos()
        {
            // Arrange
            var todos = new List<Todo>
            {
                new() { Id = Guid.NewGuid(), Title = "Test Todo 1" },
                new() { Id = Guid.NewGuid(), Title = "Test Todo 2" }
            };
            _todoRepositoryMock.Setup(repo => repo.GetAllAsync())
                .ReturnsAsync(todos);

            // Act
            var result = await _toDoService.GetAllTodos();

            // Assert
            result.IsSucces.Should().BeTrue();
            result.Result.Should().BeEquivalentTo(todos);
        }

        [Fact]
        public async Task GetSpecifiedTodo_ShouldReturnSuccessResultWithTodo_WhenIdExists()
        {
            // Arrange
            var todo = new Todo { Id = Guid.NewGuid(), Title = "Test Todo" };
            _todoRepositoryMock.Setup(repo => repo.GetByIdAsync(todo.Id))
                .ReturnsAsync(todo);

            // Act
            var result = await _toDoService.GetSpecifiedTodo(todo.Id);

            // Assert
            result.IsSucces.Should().BeTrue();
            result.Result.Should().Be(todo);
        }

        [Fact]
        public async Task GetSpecifiedTodo_ShouldReturnSuccessResultWithNull_WhenIdNotExists()
        {
            // Arrange
            _todoRepositoryMock.Setup(repo => repo.GetByIdAsync(It.IsAny<Guid>()))
                .ReturnsAsync((Todo?)null);

            // Act
            var result = await _toDoService.GetSpecifiedTodo(Guid.NewGuid());

            // Assert
            result.IsSucces.Should().BeTrue();
            result.Result.Should().BeNull();
        }

        [Fact]
        public async Task CreateToDo_ShouldReturnSuccessResultWithId_WhenDataIsValid()
        {
            // Arrange
            var dto = new TodoCreateDto("Test Todo", "Test Description", DateTime.Now.AddDays(1), 50);
            var newTodoId = Guid.NewGuid();
            _todoRepositoryMock.Setup(repo => repo.AddAsync(It.IsAny<Todo>()))
                .ReturnsAsync(newTodoId);

            // Act
            var result = await _toDoService.CreateToDo(dto);

            // Assert
            result.IsSucces.Should().BeTrue();
            result.Result.Should().Be(newTodoId);
        }

        [Fact]
        public async Task CreateToDo_ShouldReturnErrorResult_WhenPercentageIsInvalid()
        {
            // Arrange
            var dto = new TodoCreateDto("Test Todo", "Test Description", DateTime.Now.AddDays(1), 150);

            // Act
            var result = await _toDoService.CreateToDo(dto);

            // Assert
            result.IsSucces.Should().BeFalse();
            result.Errors.Should().Contain("Percent value should be between 0 and 100");
        }

        [Fact]
        public async Task CreateToDo_ShouldReturnErrorResult_WhenDateIsExpired()
        {
            // Arrange
            var dto = new TodoCreateDto("Test Todo", "Test Description", DateTime.Now.AddDays(-1), 50);

            // Act
            var result = await _toDoService.CreateToDo(dto);

            // Assert
            result.IsSucces.Should().BeFalse();
            result.Errors.Should().Contain("Expire date can not be past");
        }

        [Fact]
        public async Task DeleteToDo_ShouldReturnSuccessResult_WhenIdExists()
        {
            // Arrange
            var todo = new Todo { Id = Guid.NewGuid() };
            _todoRepositoryMock.Setup(repo => repo.GetByIdAsync(todo.Id))
                .ReturnsAsync(todo);
            _todoRepositoryMock.Setup(repo => repo.DeleteAsync(todo))
                .ReturnsAsync(true);

            // Act
            var result = await _toDoService.DeleteToDo(todo.Id);

            // Assert
            result.IsSucces.Should().BeTrue();
        }

        [Fact]
        public async Task DeleteToDo_ShouldReturnErrorResult_WhenIdNotExists()
        {
            // Arrange
            _todoRepositoryMock.Setup(repo => repo.GetByIdAsync(It.IsAny<Guid>()))
                .ReturnsAsync((Todo?)null);

            // Act
            var result = await _toDoService.DeleteToDo(Guid.NewGuid());

            // Assert
            result.IsSucces.Should().BeFalse();
            result.Errors.Should().Contain("Todo not found");
        }

        [Fact]
        public async Task GetIncomingTodos_ShouldReturnSuccess_WhenValidDaysProvided()
        {
            // Arrange
            var todos = new List<Todo>
        {
            new Todo { Id = Guid.NewGuid(), Title = "Test 1", ExpiryDate = DateTime.UtcNow.AddDays(1) },
            new Todo { Id = Guid.NewGuid(), Title = "Test 2", ExpiryDate = DateTime.UtcNow.AddDays(3) }
        };

            _todoRepositoryMock.Setup(repo => repo.GetIncomingAsync(IncomingTodoDateType.NextDay))
                           .ReturnsAsync(todos);

            // Act
            var result = await _toDoService.GetIncomingTodos((int)IncomingTodoDateType.NextDay);

            // Assert
            result.IsSucces.Should().BeTrue();
            result.Result.Should().BeEquivalentTo(todos);
        }

        [Fact]
        public async Task GetIncomingTodos_ShouldReturnError_WhenInvalidDaysProvided()
        {
            // Act
            var result = await _toDoService.GetIncomingTodos(999);

            // Assert
            result.IsSucces.Should().BeFalse();
            result.Errors.Should().Contain("Invalid selected days type");
        }

        [Fact]
        public async Task UpdateToDo_ShouldReturnSuccess_WhenTodoUpdatedSuccessfully()
        {
            // Arrange
            var existingTodo = new Todo { Id = Guid.NewGuid(), Title = "Existing", PercentComplete = 50, ExpiryDate = DateTime.UtcNow.AddDays(1) };
            var updateDto = new TodoUpdateDto("Updated", string.Empty, DateTime.UtcNow.AddDays(2), 80);

            _todoRepositoryMock.Setup(repo => repo.GetByIdAsync(existingTodo.Id))
                .ReturnsAsync(existingTodo);
            _todoRepositoryMock.Setup(repo => repo.UpdateAsync(existingTodo))
                .ReturnsAsync(true);

            // Act
            var result = await _toDoService.UpdateToDo(existingTodo.Id, updateDto);

            // Assert
            result.IsSucces.Should().BeTrue();
            existingTodo.Title.Should().Be(updateDto.Title);
            existingTodo.PercentComplete.Should().Be(updateDto.PercentComplete);
            existingTodo.ExpiryDate.Should().Be(updateDto.ExpiryDate);
            existingTodo.IsDone.Should().BeFalse();
        }

        [Fact]
        public async Task UpdateToDo_ShouldReturnError_WhenTodoNotFound()
        {
            // Arrange
            var id = Guid.NewGuid();
            var updateDto = new TodoUpdateDto("Updated", string.Empty, DateTime.UtcNow.AddDays(2), 80);

            _todoRepositoryMock.Setup(repo => repo.GetByIdAsync(id))
                .ReturnsAsync((Todo?)null);

            // Act
            var result = await _toDoService.UpdateToDo(id, updateDto);

            // Assert
            result.IsSucces.Should().BeFalse();
            result.Errors.Should().Contain("Todo not found");
        }

        [Fact]
        public async Task SetTodoPercentage_ShouldReturnSuccess_WhenPercentageIsUpdated()
        {
            // Arrange
            var existingTodo = new Todo { Id = Guid.NewGuid(), Title = "Existing", PercentComplete = 50 };
            int newPercentage = 75;

            _todoRepositoryMock.Setup(repo => repo.GetByIdAsync(existingTodo.Id))
                .ReturnsAsync(existingTodo);

            // Act
            var result = await _toDoService.SetTodoPercentage(existingTodo.Id, newPercentage);

            // Assert
            result.IsSucces.Should().BeTrue();
            existingTodo.PercentComplete.Should().Be(newPercentage);
        }

        [Fact]
        public async Task SetTodoPercentage_ShouldReturnError_WhenTodoNotFound()
        {
            // Arrange
            var id = Guid.NewGuid();
            int percentage = 75;

            _todoRepositoryMock.Setup(repo => repo.GetByIdAsync(id))
                .ReturnsAsync((Todo?)null);

            // Act
            var result = await _toDoService.SetTodoPercentage(id, percentage);

            // Assert
            result.IsSucces.Should().BeFalse();
            result.Errors.Should().Contain("Todo not found");
        }

        [Fact]
        public async Task MarkAsDone_ShouldReturnSuccess_WhenTodoIsMarkedAsDone()
        {
            // Arrange
            var existingTodo = new Todo { Id = Guid.NewGuid(), Title = "Existing", PercentComplete = 50 };

            _todoRepositoryMock.Setup(repo => repo.GetByIdAsync(existingTodo.Id))
                .ReturnsAsync(existingTodo);
            _todoRepositoryMock.Setup(repo => repo.MarkAsDone(existingTodo))
                .ReturnsAsync(true);

            // Act
            var result = await _toDoService.MarkAsDone(existingTodo.Id);

            // Assert
            result.IsSucces.Should().BeTrue();
            result.Result.Should().BeTrue();
        }

        [Fact]
        public async Task MarkAsDone_ShouldReturnError_WhenTodoNotFound()
        {
            // Arrange
            var id = Guid.NewGuid();

            _todoRepositoryMock.Setup(repo => repo.GetByIdAsync(id))
                .ReturnsAsync((Todo?)null);

            // Act
            var result = await _toDoService.MarkAsDone(id);

            // Assert
            result.IsSucces.Should().BeFalse();
            result.Errors.Should().Contain("Todo not found");
        }
    }
}
