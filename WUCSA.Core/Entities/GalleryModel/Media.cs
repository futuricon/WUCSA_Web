using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using WUCSA.Core.Entities.Base;
using WUCSA.Core.Entities.UserModel;
using WUCSA.Core.Interfaces;

namespace WUCSA.Core.Entities.GalleryModel
{
    public class Media : IEntity<string>
    {
        [StringLength(32)]
        public string Id { get; set; } = GeneratorId.GenerateLong();

        public string MediaPath { get; set; }

        public DateTime PostedDate { get; set; } = DateTime.Now;

        public virtual ICollection<MediaTag> MediaTags { get; set; } = new List<MediaTag>();

        public virtual AppUser Author { get; set; }
    }
}
