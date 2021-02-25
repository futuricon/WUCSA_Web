using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using WUCSA.Core.Entities.Base;

namespace WUCSA.Core.Entities.BlogModel
{
    public class Tag
    {
        public Tag()
        {

        }
        public Tag(string tagName)
        {
            Name = tagName.Trim();
        }
        [StringLength(32)]
        public string Id { get; set; } = GeneratorId.GenerateLong();

        [Required]
        [StringLength(40)]
        public string Name { get; set; }

        public virtual ICollection<BlogTag> BlogTags { get; set; }

        public override string ToString() => Name;
        public static implicit operator Tag(string tagName) => new Tag(tagName);
        public static implicit operator string(Tag tag) => tag.Name;
    }
}
