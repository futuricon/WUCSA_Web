using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace WUCSA.Core.Entities.UserModel
{
    public enum Role
    {
        SuperAdmin,
        Admin
    }

    public class UserRole : IdentityRole
    {
        public UserRole(Role role) : base(role.ToString())
        {
            Role = role;
        }

        [StringLength(32)]
        public override string Id { get; set; }

        public Role Role { get; set; }
    }
}
