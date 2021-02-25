using Microsoft.AspNetCore.Identity;
using System;
using System.ComponentModel.DataAnnotations;
using WUCSA.Core.Entities.Base;
using WUCSA.Core.Interfaces;

namespace WUCSA.Core.Entities.UserModel
{
    public class AppUser : IdentityUser<string>
    {
        public AppUser()
        {
            ProfilePhotoPath = "/img/default_user_avatar.png";
        }

        [StringLength(32)]
        public override string Id { get; set; } = GeneratorId.GenerateComplex();

        [StringLength(40, ErrorMessage = "Characters must be less than 40")]
        public string FirstName { get; set; }

        [StringLength(40, ErrorMessage = "Characters must be less than 40")]
        public string LastName { get; set; }

        [StringLength(64)]
        public string ProfilePhotoPath { get; set; }
    }
}
