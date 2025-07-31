using System;
using System.Collections.Generic;
using System.Linq;

namespace NLS.ExtentionsCore.Page
{
    [Serializable]
    public class PageResult
    {
        public int Count { get; set; }
    }

    public sealed class PageResut<T> : PageResult
    {
        public PageResut()
        {
        }

        public PageResut(List<T> list, int count)
        {
            Data = list;
            Count = count;
        }

        public List<T> Data { get; set; }
    }

    public static class Page_Extend
    {
        public static PageResut<T> Page<T>(this IQueryable<T> q, int pageindex, int pagesize)
        {
            if (pageindex < 1)
            {
                pageindex = 1;
            }
            if (pagesize < 1)
            {
                pageindex = 20;
            }
            int count = q.Count();
            return new PageResut<T>
            {
                Count = count,
                Data = q.Skip((pageindex - 1) * pagesize).Take(pagesize).ToList()
            };
        }
    }
}