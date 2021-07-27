using System.ComponentModel.DataAnnotations;
using WUCSA.Core.Entities.RankModel;

namespace WUCSA.Core.Entities.EventModel
{
    public class EventSportType
    {
        [StringLength(32)]
        public string EventId { get; set; }
        public virtual Event Event { get; set; }

        [StringLength(32)]
        public string SportTypeId { get; set; }
        public virtual SportType SportType { get; set; }
    }
}
