using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using WUCSA.Core.Entities.Base;
using WUCSA.Core.Interfaces;

namespace WUCSA.Core.Entities.StaffModel
{
    public class Staff : IEntity<string>
    {
        public Staff()
        {
            ProfilePhotoPath = "/img/profile_image.png";
            IsDeleted = false;
        }

        [StringLength(32)]
        public string Id { get; set; } = GeneratorId.GenerateLong();

        [StringLength(80)]
        public string Slug { get; set; }

        [StringLength(100)]
        public string ProfilePhotoPath { get; set; }
        
        [Required(ErrorMessage = "Please enter name")]
        [StringLength(40, ErrorMessage = "Characters must be less than 40")]
        [Display(Name = "Name")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Please enter last name")]
        [StringLength(40, ErrorMessage = "Characters must be less than 40")]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Please enter a positiron")]
        [StringLength(80, ErrorMessage = "Characters must be less than 80")]
        [Display(Name = "Position")]
        public string Position { get; set; }

        [Required(ErrorMessage = "Пожалуйста, введите должность")]
        [StringLength(80, ErrorMessage = "Символов должно быть меньше 80")]
        [Display(Name = "Должность")]
        public string PositionRu { get; set; }

        [Required(ErrorMessage = "Iltimos, mansabni kiriting")]
        [StringLength(80, ErrorMessage = "Belgilar 80 dan kam bo'lishi kerak")]
        [Display(Name = "Mansab")]
        public string PositionUz { get; set; }

        [Required(ErrorMessage = "Please enter the country, state or city of the representative")]
        [StringLength(100, ErrorMessage = "Characters must be less than 400")]
        [Display(Name = "Representative from")]
        public string Location { get; set; }

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

        [StringLength(50, ErrorMessage = "Characters must be less than 50")]
        [Display(Name = "Facebook")]
        public string FacebookUrl { get; set; }

        [StringLength(50, ErrorMessage = "Characters must be less than 50")]
        [Display(Name = "Instagram")]
        public string InstagramUrl { get; set; }

        [StringLength(50, ErrorMessage = "Characters must be less than 50")]
        [Display(Name = "Twitter")]
        public string TwitterUrl { get; set; }

        [StringLength(50, ErrorMessage = "Characters must be less than 50")]
        [Display(Name = "Telegram")]
        public string TelegramUrl { get; set; }

        [StringLength(20, ErrorMessage = "Characters must be less than 20")]
        [Display(Name = "Phone Number")]
        public string PhoneNumber { get; set; }

        [StringLength(30, ErrorMessage = "Characters must be less than 30")]
        [Display(Name = "E-mail")]
        public string Email { get; set; }

        public virtual ICollection<Certificate> Certificates { get; set; } = new List<Certificate>();

        public bool IsDeleted { get; set; }
    }
}
