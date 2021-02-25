using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using WUCSA.Core.Entities.Base;

namespace WUCSA.Core.Entities.BlogModel
{
    public class BlogTag
    {
        [StringLength(32)]
        public string BlogId { get; set; }
        public virtual Blog Blog { get; set; }

        [StringLength(32)]
        public string TagId { get; set; }
        public virtual Tag Tag { get; set; }
    }
}
