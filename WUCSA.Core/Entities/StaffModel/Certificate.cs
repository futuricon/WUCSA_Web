using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using WUCSA.Core.Entities.Base;
using WUCSA.Core.Entities.UserModel;
using WUCSA.Core.Interfaces;

namespace WUCSA.Core.Entities.StaffModel
{
    public class Certificate : IEntity<string>
    {
        [StringLength(32)]
        public string Id { get; set; } = GeneratorId.GenerateLong();

        [StringLength(100)]
        public string CertPath { get; set; }

        [StringLength(40, ErrorMessage = "Characters must be less than 40")]
        [Display(Name = "Certificate Title")]
        public string CertName { get; set; }

        public virtual Staff Staff { get; set; }
    }
}
