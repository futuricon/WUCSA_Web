using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using WUCSA.Core.Entities.Base;
using WUCSA.Core.Entities.UserModel;
using WUCSA.Core.Interfaces;

namespace WUCSA.Core.Entities.RankModel
{
    public enum RankLocation
    {
        World,
        National
    }

    public class Rank : IEntity<string>
    {
        public Rank()
        {
            IsDeleted = false;
        }

        [StringLength(32)]
        public string Id { get; set; } = GeneratorId.GenerateLong();

        [StringLength(80)]
        public string Slug { get; set; }

        [Display(Name = "Sport Type")]
        public virtual SportType SportType { get; set; }

        public virtual AppUser Author { get; set; }

        [Required(ErrorMessage = "Please enter the ranking Date")]
        [Display(Name = "Rank Date")]
        public DateTime RankDate { get; set; }

        [Required(ErrorMessage = "Please select location")]
        [Display(Name = "Location")]
        public RankLocation RankLocation { get; set; }

        [Required(ErrorMessage = "Please enter title")]
        [StringLength(100, ErrorMessage = "Characters must be less than 100")]
        [Display(Name = "Title of the type of sport")]
        public string Title { get; set; }

        [Required(ErrorMessage = "Пожалуйста, введите название")]
        [StringLength(100, ErrorMessage = "Символов должно быть меньше 100")]
        [Display(Name = "Название направления")]
        public string TitleRu { get; set; }

        [Required(ErrorMessage = "Iltimos, y'onalishni nomini kiriting")]
        [StringLength(100, ErrorMessage = "Belgilar 100 dan kam bo'lishi kerak")]
        [Display(Name = "Name of ")]
        public string TitleUz { get; set; }

        [StringLength(400, ErrorMessage = "Characters must be less than 400")]
        [Display(Name = "Description")]
        public string Description { get; set; }

        [StringLength(400, ErrorMessage = "Символов должно быть меньше 400")]
        [Display(Name = "Описание")]
        public string DescriptionRu { get; set; }

        [StringLength(400, ErrorMessage = "Belgilar 400 dan kam bo'lishi kerak")]
        [Display(Name = "Tavsif")]
        public string DescriptionUz { get; set; }

        [Required(ErrorMessage = "Please enter the country, state or city of the rank")]
        [StringLength(100, ErrorMessage = "Characters must be less than 100")]
        [Display(Name = "Rank Location")]
        public string Location { get; set; }

        public virtual ICollection<RankParticipant> RankParticipants { get; set; } = new List<RankParticipant>();

        public string RankPartsFilePath { get; set; }

        public bool IsDeleted { get; set; }
    }
}
