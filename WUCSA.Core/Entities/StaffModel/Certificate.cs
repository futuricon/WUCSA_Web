using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using WUCSA.Core.Entities.Base;

namespace WUCSA.Core.Entities.StaffModel
{
    public class Certificate
    {
        [StringLength(32)]
        public string Id { get; set; } = GeneratorId.GenerateComplex();

        public string CertPath { get; set; }

        public string CertName { get; set; }

        public int StaffId { get; set; }

        public Staff Staff { get; set; }
    }
}
