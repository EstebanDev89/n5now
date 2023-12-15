using System.ComponentModel.DataAnnotations;

namespace Domain.Entities
{
    public class Entity<T>
    {
        [Key]
        public virtual T Id { get; set; }
    }
}
