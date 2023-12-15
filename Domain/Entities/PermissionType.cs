using System.ComponentModel.DataAnnotations;

namespace Domain.Entities
{
    public class PermissionType : Entity<int>
    {
        [Required]
        public string Description { get; set; }
    }
}
