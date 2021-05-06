using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using WUCSA.Core.Entities.Base;
using WUCSA.Core.Entities.StaffModel;
using WUCSA.Core.Entities.UserModel;

namespace WUCSA.Core.Entities.EventModel
{
    public enum Gender
    {
        Man,
        Woman
    }

    public class EventParticipant
    {
        public EventParticipant()
        {
            IsDeleted = false;
        }

        [StringLength(32)]
        public string Id { get; set; } = GeneratorId.GenerateLong();

        [RegularExpression("([1-9][0-9]*)", ErrorMessage = "This is not valid number")]
        [Display(Name = "Weight category")]
        public int Weight { get; set; }

        [Required(ErrorMessage = "Please select gender")]
        [Display(Name = "Gender")]
        public Gender Gender { get; set; }

        public virtual AppUser Author { get; set; }

        public virtual Event Event { get; set; }

        public virtual ICollection<Participant> Participants { get; set; }

        public bool IsDeleted { get; set; }
    }
}