using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using WUCSA.Core.Entities.Base;
using WUCSA.Core.Entities.UserModel;
using WUCSA.Core.Interfaces;

namespace WUCSA.Core.Entities.BlogModel
{
    public class Comment : IEntity<string>
    {
        public Comment()
        {
            IsLocked = false;
        }
        [StringLength(32)]
        public string Id { get; set; } = GeneratorId.GenerateLong();

        [Required(ErrorMessage = "Please enter content")]
        [StringLength(200, ErrorMessage = "Characters must be less than 200")]
        public string Content { get; set; }

        [StringLength(64, ErrorMessage = "Characters must be less than 64")]
        public string AuthorName { get; set; }

        [DataType(DataType.EmailAddress, ErrorMessage = "This is not valid email address")]
        [StringLength(64, ErrorMessage = "Characters must be less than 64")]
        public string AuthorEmail { get; set; } 

        public DateTime PostedDate { get; set; } = DateTime.Now;

        public bool IsLocked { get; set; }

        public virtual AppUser Author { get; set; }

        public virtual Comment ParentComment { get; set; }

        public virtual Blog Blog { get; set; }

        public virtual ICollection<Comment> Replies { get; set; }
    }
}
