using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using WUCSA.Core.Entities.Base;
using WUCSA.Core.Entities.UserModel;
using WUCSA.Core.Interfaces;

namespace WUCSA.Core.Entities.GalleryModel
{
    public enum MediaType
    {
        Video,
        Image
    }

    public class Media : IEntity<string>
    {
        public Media()
        {
            IsDeleted = false;
        }
        [StringLength(32)]
        public string Id { get; set; } = GeneratorId.GenerateLong();

        public string MediaPath { get; set; }

        public MediaType MediaType { get; set; }

        public DateTime PostedDate { get; set; } = DateTime.Now;

        public virtual ICollection<MediaTag> MediaTags { get; set; } = new List<MediaTag>();

        public virtual ICollection<MediaLike> LikesCount { get; set; } = new List<MediaLike>();

        public virtual AppUser Author { get; set; }

        public bool IsDeleted { get; set; }
    }
}
