using System;
using System.Collections.Generic;

namespace VapeAPI.Models
{
    public partial class Fluids
    {
        public int? IdFluid { get; set; }
        public string NameFluid { get; set; } 
        public int? TasteId { get; set; }
        public int? Nicotine { get; set; }
        public int? VolumeFluid { get; set; }
        public int? CostFluid { get; set; }
        public int? CountFluid { get; set; }
        public bool? IsDeleted { get; set; }

    }
}
