using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using WUCSA.Core.Interfaces;

namespace WUCSA.Core.Entities.UserModel
{
    public enum Role
    {
        SuperAdmin,
        Admin,
        Editor
    }

    public class UserRole : IdentityRole, IEntity<string>
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
