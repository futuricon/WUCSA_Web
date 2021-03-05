using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using WUCSA.Core.Entities.ParticipantModel;

namespace WUCSA.Core.Entities.EventModel
{
    public class EventParticipant
    {
        [StringLength(32)]
        public string EventId { get; set; }
        public virtual Event Event { get; set; }

        [StringLength(32)]
        public string ParticipantId { get; set; }
        public virtual Participant Participant { get; set; }
    }
}
