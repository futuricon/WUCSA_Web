using System.ComponentModel.DataAnnotations;
using WUCSA.Core.Entities.UserModel;

namespace WUCSA.Core.Entities.GalleryModel
{
    public class MediaLike
    {
        [StringLength(32)]
        public string MediaId { get; set; }
        public virtual Media Media { get; set; }

        [StringLength(32)]
        public string UserId { get; set; }
        public virtual AppUser AppUser { get; set; }
    }
}
