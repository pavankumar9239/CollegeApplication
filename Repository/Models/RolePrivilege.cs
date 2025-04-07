using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Models
{
    public class RolePrivilege
    {
        public int Id { get; set; }
        public string RolePrivilegeName { get; set; }
        public string Description { get; set; }
        public int RoleId { get; set; }
        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime ModifiedDate { get; set; }

        public Role Role { get; set; }
    }
}
