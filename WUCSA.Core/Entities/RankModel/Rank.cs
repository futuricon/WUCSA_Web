using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using WUCSA.Core.Entities.Base;
using WUCSA.Core.Entities.ParticipantModel;
using WUCSA.Core.Interfaces;

namespace WUCSA.Core.Entities.RankModel
{
    public class Rank : IEntity<string>
    {
        [StringLength(32)]
        public string Id { get; set; } = GeneratorId.GenerateLong();

        [Required(ErrorMessage = "Please enter the ranking name")]
        [StringLength(40, ErrorMessage = "Characters must be less than 40")]
        [Display(Name = "Rank name")]
        public string RankName { get; set; }

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
    }
}
