using System.ComponentModel.DataAnnotations;

namespace BookStore.Domain.Enums
{
    public enum OrderStatus
    {
        [Display(Name = "Завършена")]
        Completed = 0
    }
}
