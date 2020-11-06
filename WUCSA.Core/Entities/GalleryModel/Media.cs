using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using WUCSA.Core.Entities.Base;

namespace WUCSA.Core.Entities.GalleryModel
{
    public class Media
    {
        public string Id { get; set; } = GeneratorId.GenerateLong();

        public string MediaPath { get; set; }

        [Required(ErrorMessage = "Please enter Tags")]
        public string TagLine { get; set; }

        public DateTime UploadDate { get; set; }

        public ICollection<MediaTag> MediaTags { get; set; }
    }
}
