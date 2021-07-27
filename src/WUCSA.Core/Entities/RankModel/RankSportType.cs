using System.ComponentModel.DataAnnotations;

namespace WUCSA.Core.Entities.RankModel
{
    public class RankSportType
    {
        [StringLength(32)]
        public string RankId { get; set; }
        public virtual Rank Rank { get; set; }

        [StringLength(32)]
        public string SportTypeId { get; set; }
        public virtual SportType SportType { get; set; }
    }
}