using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using WUCSA.Core.Entities.Base;
using WUCSA.Core.Entities.StaffModel;

namespace WUCSA.Core.Entities.EventModel
{
    public class EventParticipant
    {
        [StringLength(32)]
        public string Id { get; set; } = GeneratorId.GenerateLong();

        [RegularExpression("([1-9][0-9]*)", ErrorMessage = "This is not valid number")]
        [Display(Name = "Weight category")]
        public int Weight { get; set; }

        public virtual Event Event { get; set; }

        public virtual ICollection<Participant> Participants { get; set; }
    }
}