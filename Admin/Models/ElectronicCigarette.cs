using System;
using System.Collections.Generic;

namespace VapeAPI.Models
{
    public partial class ElectronicCigarette
    {
        public int? IdElectronicCigarette { get; set; }
        public string NameElectronicCigarette { get; set; } 
        public string DescribeElectronicCigarette { get; set; } 
        public int? TypeElectronicCigaretteId { get; set; }
        public int? TractionElectronicCigarette { get; set; }
        public int? CostElectronicCigarette { get; set; }
        public int? CountElectronicCigarette { get; set; }
        public bool? IsDeleted { get; set; }

    }
}
