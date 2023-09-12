using System;
using System.Collections.Generic;

namespace VapeAPI.Models
{
    public partial class OrderElectronicCigarette
    {
        public int? IdOrderElectronicCigarette { get; set; }
        public int? OrderId { get; set; }
        public int? ElectronicCigaretteId { get; set; }
        public bool? IsDeleted { get; set; }

    }
}
