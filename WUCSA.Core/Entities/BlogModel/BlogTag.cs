using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using WUCSA.Core.Entities.Base;

namespace WUCSA.Core.Entities.BlogModel
{
    public class BlogTag
    {
        public int Id { get; set; }
        public string Tag { get; set; }

        public string BlogId { get; set; }
        public Blog Blog { get; set; }
    }
}
