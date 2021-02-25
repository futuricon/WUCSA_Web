﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace WUCSA.Core.Entities.GalleryModel
{
    public class MediaTag
    {
        [StringLength(32)]
        public string MediaId { get; set; }
        public virtual Media Media { get; set; }

        [StringLength(32)]
        public string MTagId { get; set; }
        public virtual MTag MTag { get; set; }
    }
}
