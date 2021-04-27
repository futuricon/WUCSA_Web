using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using WUCSA.Core.Entities.Base;
using WUCSA.Core.Interfaces;

namespace WUCSA.Core.Entities.ParticipantModel
{
    public class Staff : IEntity<string>
    {
        public Staff()
        {
            ProfilePhotoPath = "/img/profile_image.png";
        }

        [StringLength(32)]
        public string Id { get; set; } = GeneratorId.GenerateLong();

        [StringLength(100)]
        public string ProfilePhotoPath { get; set; }

        [Required(ErrorMessage = "Please enter Name")]
        [StringLength(40, ErrorMessage = "Characters must be less than 40")]
        [Display(Name = "Name")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Please enter Last Name")]
        [StringLength(40, ErrorMessage = "Characters must be less than 40")]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Please enter positiron")]
        [StringLength(80, ErrorMessage = "Characters must be less than 80")]
        [Display(Name = "Position")]
        public string Position { get; set; }

        [Required(ErrorMessage = "Пожалуйста, введите должность")]
        [StringLength(80, ErrorMessage = "Символов должно быть меньше 80")]
        [Display(Name = "Должность")]
        public string PositionRu { get; set; }

        [Required(ErrorMessage = "Iltimos, kiriting")]
        [StringLength(80, ErrorMessage = "Belgilar 80 dan kam bo'lishi kerak")]
        [Display(Name = "Position")]
        public string PositionUz { get; set; }

        [Display(Name = "Date of Birth")]
        public DateTime BirthDate { get; set; }

        [StringLength(400, ErrorMessage = "Characters must be less than 400")]
        [Display(Name = "Description")]
        public string Description { get; set; }

        [StringLength(400, ErrorMessage = "Символов должно быть меньше 400")]
        [Display(Name = "Описание")]
        public string DescriptionRu { get; set; }

        [StringLength(400, ErrorMessage = "Belgilar 400 dan kam bo'lishi kerak")]
        [Display(Name = "Tavsif")]
        public string DescriptionUz { get; set; }

        public bool IsMember { get; set; }

        public virtual ICollection<Certificate> Certificates { get; set; } = new List<Certificate>();
    }
}
