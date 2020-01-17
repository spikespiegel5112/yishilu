using System;
using System.Collections.Generic;
using System.Text;

namespace NLS.Cache
{
    public interface ICacheManager
    {
        T Get<T>(string key) where T : class;
    }
}
