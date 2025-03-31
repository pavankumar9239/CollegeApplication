using Microsoft.AspNetCore.Antiforgery;
using System.ComponentModel.DataAnnotations;

namespace CollegeApplication.Models
{
    public class RolePrivilegeDto
    {
        public int Id { get; set; }
        [Required]
        public string RolePrivilegeName { get; set; }
        public string Description { get; set; }
        public int RoleId { get; set; }
        public bool IsActive { get; set; }
    }
}
