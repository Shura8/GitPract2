using System;
using System.Collections.Generic;

namespace VapeAPI.Models
{
    public partial class Tobaccos
    {

        public int? IdTobacco { get; set; }
        public string NameTobacco { get; set; } 
        public string CountryTobacco { get; set; } 
        public int? StrengthId { get; set; }
        public bool? IsDeleted { get; set; }

    }
}
