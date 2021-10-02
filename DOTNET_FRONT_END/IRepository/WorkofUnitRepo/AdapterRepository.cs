using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DOTNET_FRONT_END.IRepository
{
    public interface AdapterRepository
    {
        IHomeRepo repoHome { get; }
    }
}
