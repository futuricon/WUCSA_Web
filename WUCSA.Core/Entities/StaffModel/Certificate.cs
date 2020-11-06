using System;
using System.Collections.Generic;
using System.Text;

namespace WUCSA.Core.Entities.StaffModel
{
    public class Certificate
    {
        public int Id { get; set; }
        public string CertPath { get; set; }

        public string CertName { get; set; }

        public int StaffId { get; set; }
        public Staff Staff { get; set; }
    }
}
