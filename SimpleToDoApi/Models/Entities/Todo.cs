using System.ComponentModel.DataAnnotations.Schema;

namespace SimpleToDoApi.Models.Entities
{
    public class Todo
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public DateTime ExpiryDate { get; set; }
        public int PercentComplete { get; set; }
        [NotMapped]
        public bool IsDone { get => PercentComplete == 100; }
    }
}
