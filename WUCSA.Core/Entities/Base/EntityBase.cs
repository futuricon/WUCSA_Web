using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using WUCSA.Core.Interfaces;

namespace WUCSA.Core.Entities.Base
{
    public abstract class EntityBase : IEntity<string>
    {
        public EntityBase()
        {
            Id = GenerateId();
        }
    
        private string GenerateId()
        {
            return $"{DateTime.Now:yyyyMMddHHmmssffffff}";
        }
    
        [StringLength(32)]
        public string Id { get; set; }
    }
}
