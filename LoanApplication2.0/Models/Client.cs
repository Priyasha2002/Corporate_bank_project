using System.ComponentModel.DataAnnotations;

namespace LoanApplicationSystem2._0.Models
{
    public class Client
    {
        [Key]
        public int ApplicationId { get; set; } // Auto-incremented

        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        [Required]
        [EmailAddress]
        [StringLength(100)]
        public string Email { get; set; }

        [Required]
        [StringLength(100)]
        public string Password { get; set; }
    }
}
