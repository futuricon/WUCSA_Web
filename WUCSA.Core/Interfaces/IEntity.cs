using System;
using System.Collections.Generic;
using System.Text;

namespace WUCSA.Core.Interfaces
{
    public interface IEntity<TKey>
    {
        TKey Id { get; set; }
    }
}
