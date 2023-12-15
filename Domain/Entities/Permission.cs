using System.ComponentModel.DataAnnotations;

namespace Domain.Entities
{
    public class Permission : Entity<int>
    {
        [Required]
        public string EmployeeForename { get; set; }
        [Required]
        public string EmployeeSurname { get; set; }
        [Required]
        public PermissionType PermissionType { get; set; }
        [Required]
        public int PermissionTypeId { get; set; }
        [Required]
        public DateTime PermissionDate { get; set; }
    }
}
