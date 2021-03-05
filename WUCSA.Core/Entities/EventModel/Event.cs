using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using WUCSA.Core.Entities.Base;
using WUCSA.Core.Entities.UserModel;
using WUCSA.Core.Interfaces;

namespace WUCSA.Core.Entities.EventModel
{
    public class Event : IEntity<string>
    {
        [StringLength(32)]
        public string Id { get; set; } = GeneratorId.GenerateLong();

        [StringLength(64)]
        public string CoverPhotoPath { get; set; }

        public virtual ICollection<EventParticipant> EventParticipants { get; set; } = new List<EventParticipant>();

        public DateTime PostedDate { get; set; }

        [Required(ErrorMessage = "Please enter the event date")]
        public DateTime EventDate { get; set; }

        [Required(ErrorMessage = "Please enter the event type")]
        [StringLength(80, ErrorMessage = "Characters must be less than 60")]
        public string CompetitionType { get; set; }

        [Required(ErrorMessage = "Please enter a description")]
        [StringLength(160, ErrorMessage = "Characters must be less than 160")]
        public string Description { get; set; }
        
        [Required(ErrorMessage = "Пожалуйста, введите описание")]
        [StringLength(160, ErrorMessage = "Символов должно быть меньше 160")]
        public string DescriptionRu { get; set; }

        [Required(ErrorMessage = "Iltimos, maqola mavzusining sarlavhasini kiriting")]
        [StringLength(80, ErrorMessage = "Belgilar 160 dan kam bo'lishi kerak")]
        public string DescriptionUz { get; set; }

        [Required(ErrorMessage = "Please enter the event location")]
        [StringLength(80, ErrorMessage = "Characters must be less than 80")]
        public string Location { get; set; }
    }
}
