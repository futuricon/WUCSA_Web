using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using WUCSA.Core.Entities.ParticipantModel;

namespace WUCSA.Core.Entities.RankModel
{
    public class RankParticipant
    {
        [StringLength(32)]
        public string RankId { get; set; }
        public virtual Rank Rank { get; set; }

        [StringLength(32)]
        public string ParticipantId { get; set; }
        public virtual Participant Participant { get; set; }

        [RegularExpression("([1-9][0-9]*)", ErrorMessage = "This is not valid number")]
        [Display(Name = "Rank number")]
        public int PositionNumber { get; set; }
    }
}
