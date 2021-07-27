using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using WUCSA.Core.Entities.Base;
using WUCSA.Core.Entities.EventModel;
using WUCSA.Core.Interfaces;

namespace WUCSA.Core.Entities.RankModel
{
    public class SportType : IEntity<string>
    {
        public SportType()
        {
            IsDeleted = false;
        }

        [StringLength(32)]
        public string Id { get; set; } = GeneratorId.GenerateLong();

        [StringLength(80)]
        public string Slug { get; set; }

        [Required(ErrorMessage = "Please enter name")]
        [StringLength(100, ErrorMessage = "Characters must be less than 100")]
        [Display(Name = "Name of the type of sport")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Пожалуйста, введите название")]
        [StringLength(100, ErrorMessage = "Символов должно быть меньше 100")]
        [Display(Name = "Название направления")]
        public string NameRu { get; set; }

        [Required(ErrorMessage = "Iltimos, y'onalishni nomini kiriting")]
        [StringLength(100, ErrorMessage = "Belgilar 100 dan kam bo'lishi kerak")]
        [Display(Name = "Name of ")]
        public string NameUz { get; set; }

        [StringLength(300, ErrorMessage = "Characters must be less than 300")]
        [Display(Name = "Description")]
        public string Description { get; set; }

        [StringLength(300, ErrorMessage = "Символов должно быть меньше 300")]
        [Display(Name = "Описание")]
        public string DescriptionRu { get; set; }

        [StringLength(300, ErrorMessage = "Belgilar 300 dan kam bo'lishi kerak")]
        [Display(Name = "Mazmuni")]
        public string DescriptionUz { get; set; }

        public string RulesFilePath { get; set; }

        public virtual ICollection<EventSportType> EventSportTypes { get; set; }

        public virtual ICollection<RankParticipant> RankParticipants { get; set; }

        public virtual ICollection<RankSportType> RankSportTypes { get; set; }

        public bool IsDeleted { get; set; }
    }
}
