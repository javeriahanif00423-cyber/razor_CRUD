
using System.ComponentModel.DataAnnotations;

namespace books_management_system.models
{
    public class Books
    {
        public int Id { get; set; }

        [StringLength(50)]
        public string? title { get; set; }

        [StringLength(50)]
        public string? author { get; set; }

        [StringLength(500)]
        public string? description { get; set; }

        [StringLength(50)]
        public string? publisher { get; set; }

        [StringLength(50)]
        public string? status { get; set; }

        [StringLength(50)]
        public string? category { get; set; }
    }
}
