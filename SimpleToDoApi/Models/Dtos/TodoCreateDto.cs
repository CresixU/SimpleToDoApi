namespace SimpleToDoApi.Models.Dtos
{
    public sealed record TodoCreateDto(
        string Title, 
        string Description,
        DateTime ExpiryDate,
        int PercentComplete);
}
