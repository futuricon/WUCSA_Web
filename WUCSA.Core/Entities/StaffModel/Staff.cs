using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using WUCSA.Core.Entities.Base;

namespace WUCSA.Core.Entities.StaffModel
{
    public class Staff
    {
        public Staff()
        {
            ProfilePhotoPath = "/img/default_user_avatar.png";
        }

        [StringLength(32)]
        public string Id { get; set; } = GeneratorId.GenerateComplex();

        public string ProfilePhotoPath { get; set; }

        [Required(ErrorMessage = "Please enter name")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Please enter name")]
        public string NameRu { get; set; }

        [Required(ErrorMessage = "Please enter name")]
        public string NameUz { get; set; }

        [Required(ErrorMessage = "Please enter surname")]
        public string Surname { get; set; }

        [Required(ErrorMessage = "Please enter surname")]
        public string SurnameRu { get; set; }

        [Required(ErrorMessage = "Please enter surname")]
        public string SurnameUz { get; set; }

        [Required(ErrorMessage = "Please enter position")]
        public string Position { get; set; }

        [Required(ErrorMessage = "Please enter position")]
        public string PositionRu { get; set; }

        [Required(ErrorMessage = "Please enter position")]
        public string PositionUz { get; set; }

        [Required(ErrorMessage = "Please enter description")]
        public string Description { get; set; }

        [Required(ErrorMessage = "Please enter description")]
        public string DescriptionRu { get; set; }

        [Required(ErrorMessage = "Please enter description")]
        public string DescriptionUz { get; set; }

        public virtual ICollection<Certificate> Certificates { get; set; }
    }
}
