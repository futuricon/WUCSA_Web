using System;
using System.Collections.Generic;
using System.Text;

namespace WUCSA.Core.Entities.Base
{
    public static class GeneratorId
    {
        public static string GenerateLong()
        {
            return $"{DateTime.Now:yyyyMMddHHmmssffffff}";
        }

        public static string GenerateComplex()
        {
            return Guid.NewGuid().ToString().ToLower().Replace("-", "");
        }
    }
}
