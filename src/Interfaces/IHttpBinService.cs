using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HttpClientPollyExample.Interfaces
{
    public interface IHttpBinService
    {
        Task Get(int code);
    }
}
