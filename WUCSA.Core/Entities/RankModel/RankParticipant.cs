using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using WUCSA.Core.Entities.Base;
using WUCSA.Core.Entities.StaffModel;

namespace WUCSA.Core.Entities.RankModel
{
    public enum Gender
    {
        Man,
        Woman
    }

    public class RankParticipant
    {
        public RankParticipant()
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

        public virtual Rank Rank { get; set; }

        public virtual ICollection<Participant> Participants { get; set; } = new List<Participant>();

        public bool IsDeleted { get; set; }
    }
}