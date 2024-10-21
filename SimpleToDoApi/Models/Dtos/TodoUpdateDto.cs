namespace SimpleToDoApi.Models.Dtos
{
    public sealed record TodoUpdateDto(
        string Title,
        string Description,
        DateTime ExpiryDate,
        int PercentComplete);
}
