using SimpleToDoApi.Models.Dtos;
using SimpleToDoApi.Models.Entities;

namespace SimpleToDoApi.Tools
{
    public sealed class TodoMapper
    {
        public static Todo Map(TodoCreateDto dto)
        {
            var result = new Todo()
            {
                Description = dto.Description,
                ExpiryDate = dto.ExpiryDate,
                PercentComplete = dto.PercentComplete,
                Title = dto.Title,
            };

            return result;
        }

        public static void Map(TodoUpdateDto source, Todo destination)
        {
            destination.Description = source.Description;
            destination.ExpiryDate = source.ExpiryDate;
            destination.PercentComplete = source.PercentComplete;
            destination.Title = source.Title;
        }
    }
}
