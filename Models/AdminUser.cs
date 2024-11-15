using System.ComponentModel.DataAnnotations;
namespace LibraryMgmt.Models
{
    public class AdminUser
    {
        [Key]
        public Guid Guid {  get; set; } = Guid.NewGuid();
        public string? Name { get; set; }
        public string? PasswordHash { get; set; }
        public string? Email { get; set; }
    }
}
