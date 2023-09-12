using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Admin.Models
{
    public class AuthUser
    {
        public int IdUsers { get; set; }
        public string SurnameUsers { get; set; }
        public string NameUsers { get; set; }
        public string FatherNameUsers { get; set; }
        public int TypeUsersId { get; set; }
        public string LoginUsers { get; set; }
        public string PasswordUsers { get; set; }
        public string SaltUsers { get; set; }
        public bool IsDeleted { get; set; }
    }
}
