using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using WUCSA.Core.Entities.Base;
using WUCSA.Core.Entities.GalleryModel;
using WUCSA.Core.Interfaces;

namespace WUCSA.Core.Entities.UserModel
{
    public class AppUser : IdentityUser<string>, IEntity<string>
    {
        public AppUser()
        {
            ProfilePhotoPath = "/img/profile_image.png";
        }

        [StringLength(32)]
        public override string Id { get; set; } = GeneratorId.GenerateComplex();

        [StringLength(40, ErrorMessage = "Characters must be less than 40")]
        [Display(Name = "Name")]
        public string? FirstName { get; set; }

        [StringLength(40, ErrorMessage = "Characters must be less than 40")]
        [Display(Name = "Last Name")]
        public string? LastName { get; set; }

        [StringLength(100)]
        public string? ProfilePhotoPath { get; set; }

        [Display(Name = "Is Blocked")]
        public bool IsBlocked { get; set; }

        public virtual ICollection<MediaLike> MediaLikes { get; set; } = new List<MediaLike>();
    }
}
