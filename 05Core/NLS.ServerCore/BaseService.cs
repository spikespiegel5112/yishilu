using NLS.Framework.EF;
using NLS.Framework.EF.DBRepository;
using System;
using System.Collections.Generic;
using System.Text;

namespace NLS.ServiceCore
{
    public abstract class BaseService<T> where T : AbsBaseContext, new()
    {
        protected readonly DBContextRepository<T> DBRepository;

        public BaseService()
        {
            DBRepository = new DBContextRepository<T>();
        }
    }
}
