using System;
using System.Collections.Generic;

namespace VapeAPI.Models
{
    public partial class Cigarettes
    {
        public int? IdCigarette { get; set; }
        public string NameCigarette { get; set; } 
        public string DescribeCigarette { get; set; } 
        public int? TobaccoId { get; set; }
        public int? CostCigarette { get; set; }
        public int? CountCigarette { get; set; }
        public bool? IsDeleted { get; set; }

    }
}
