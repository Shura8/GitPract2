using System;
using System.Collections.Generic;

namespace VapeAPI.Models
{
    public partial class OrderCigarette
    {
        public int? IdOrderCigarette { get; set; }
        public int? OrderId { get; set; }
        public int? CigaretteId { get; set; }
        public bool? IsDeleted { get; set; }

    }
}
