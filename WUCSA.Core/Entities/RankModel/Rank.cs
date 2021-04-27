using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using WUCSA.Core.Entities.Base;
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
        [StringLength(32)]
        public string Id { get; set; } = GeneratorId.GenerateLong();

        [Required(ErrorMessage = "Please select kind of sport")]
        [Display(Name = "Sport Type")]
        public virtual SportType SportType { get; set; }

        [Required(ErrorMessage = "Please enter the ranking year")]
        [Display(Name = "Rank Year")]
        public DateTime RankYear { get; set; }

        [Required(ErrorMessage = "Please select location")]
        [Display(Name = "Location")]
        public RankLocation RankLocation { get; set; }

        [StringLength(400, ErrorMessage = "Characters must be less than 400")]
        [Display(Name = "Description")]
        public string Description { get; set; }

        [StringLength(400, ErrorMessage = "Символов должно быть меньше 400")]
        [Display(Name = "Описание")]
        public string DescriptionRu { get; set; }

        [StringLength(400, ErrorMessage = "Belgilar 400 dan kam bo'lishi kerak")]
        [Display(Name = "Tavsif")]
        public string DescriptionUz { get; set; }

        public virtual ICollection<RankParticipant> RankParticipants { get; set; } = new List<RankParticipant>();

        public string RankPartsFileUrl { get; set; }
    }
}
