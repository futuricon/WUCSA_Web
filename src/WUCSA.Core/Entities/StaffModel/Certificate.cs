using System.ComponentModel.DataAnnotations;
using WUCSA.Core.Entities.Base;
using WUCSA.Core.Interfaces;

namespace WUCSA.Core.Entities.StaffModel
{
    public class Certificate : IEntity<string>
    {
        [StringLength(32)]
        public string Id { get; set; } = GeneratorId.GenerateLong();

        [StringLength(100)]
        public string CertPath { get; set; }

        [StringLength(80)]
        public string CertName { get; set; }

        public virtual Staff Staff { get; set; }
    }
}
