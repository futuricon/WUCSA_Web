using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using WUCSA.Core.Entities.Base;
using WUCSA.Core.Interfaces;

namespace WUCSA.Core.Entities.GalleryModel
{
    public class Media : IEntity<string>
    {
        [StringLength(32)]
        public string Id { get; set; } = GeneratorId.GenerateLong();

        public string MediaPath { get; set; }

        public DateTime UploadDate { get; set; }

        public ICollection<MediaTag> MediaTags { get; set; }
    }
}
