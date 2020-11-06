using System;
using System.Collections.Generic;
using System.Text;

namespace WUCSA.Core.Entities.GalleryModel
{
    public class MediaTag
    {
        public int Id { get; set; }
        public string Tag { get; set; }

        public string MediaId { get; set; }
        public Media Media { get; set; }
    }
}
