using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using WUCSA.Core.Entities.Base;
using WUCSA.Core.Interfaces;

namespace WUCSA.Core.Entities.GalleryModel
{
    public class MTag : IEntity<string>
    {
        public MTag()
        {

        }
        public MTag(string tagName)
        {
            Name = tagName.Trim();
        }
        [StringLength(32)]
        public string Id { get; set; } = GeneratorId.GenerateLong();

        [Required]
        [StringLength(40)]
        public string Name { get; set; }

        public virtual ICollection<MediaTag> MediaTags { get; set; }
        public override string ToString() => Name;
        public static implicit operator MTag(string tagName) => new MTag(tagName);
        public static implicit operator string(MTag tag) => tag.Name;
    }
}
