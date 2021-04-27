using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using WUCSA.Core.Entities.Base;
using WUCSA.Core.Entities.EventModel;
using WUCSA.Core.Interfaces;

namespace WUCSA.Core.Entities.RankModel
{
    public class SportType : IEntity<string>
    {
        [StringLength(32)]
        public string Id { get; set; } = GeneratorId.GenerateLong();

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

        public virtual ICollection<Rank> Ranks { get; set; } = new List<Rank>();

        public virtual ICollection<Event> Events { get; set; } = new List<Event>();
    }
}
