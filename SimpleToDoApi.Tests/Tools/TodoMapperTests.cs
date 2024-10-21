using FluentAssertions;
using SimpleToDoApi.Models.Dtos;
using SimpleToDoApi.Models.Entities;
using SimpleToDoApi.Tools;

namespace SimpleToDoApi.Tests.Tools
{
    public class TodoMapperTests
    {
        [Fact]
        public void Map_ShouldReturnMappedObject_WhenCalled()
        {
            // Arrange
            var dto = new TodoCreateDto("Test Title", "Test Description", DateTime.UtcNow.AddDays(1), 0);

            // Act
            var result = TodoMapper.Map(dto);

            // Assert
            result.Should().NotBeNull();
            result.Description.Should().Be(dto.Description);
            result.ExpiryDate.Should().Be(dto.ExpiryDate);
            result.IsDone.Should().BeFalse();
            result.PercentComplete.Should().Be(dto.PercentComplete);
            result.Title.Should().Be(dto.Title);
        }

        [Fact]
        public void Map_ShouldMapObject_WhenCalled()
        {
            // Arrange
            var source = new TodoUpdateDto("Test Title", "Test Description", DateTime.UtcNow.AddDays(1), 100);

            var destination = new Todo(); 

            // Act
            TodoMapper.Map(source, destination);

            // Assert
            destination.Description.Should().Be(source.Description);
            destination.ExpiryDate.Should().Be(source.ExpiryDate);
            destination.IsDone.Should().BeTrue();
            destination.PercentComplete.Should().Be(source.PercentComplete);
            destination.Title.Should().Be(source.Title);
        }
    }
}
