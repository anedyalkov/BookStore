using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace BookStore.Domain.Enums
{
    public enum OrderStatus
    {
        [Display(Name = "Завършена")]
        Completed = 0
    }
}
