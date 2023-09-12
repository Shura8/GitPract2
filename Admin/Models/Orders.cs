using System;
using System.Collections.Generic;

namespace VapeAPI.Models
{
    public partial class Orders
    {
        public int? IdOrder { get; set; }
        public string NumberOrder { get; set; } 
        public int? OrderSum { get; set; }
        public string TimeOrder { get; set; }
        public int? ShopId { get; set; }
        public int? StatusOrderId { get; set; }
        public int? UsersId { get; set; }
        public bool? IsDeleted { get; set; }

    }
}
