using System;
using System.Collections.Generic;

namespace VapeAPI.Models
{
    public partial class OrderFluid
    {
        public int? IdOrderFluid { get; set; }
        public int? OrderId { get; set; }
        public int? FluidId { get; set; }
        public bool? IsDeleted { get; set; }

    }
}
