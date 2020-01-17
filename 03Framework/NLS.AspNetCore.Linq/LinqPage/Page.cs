using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NLS.AspNetCore.Linq
{
    [Serializable]
    public class PageResult
    {
        public int Count { get; set; }
    }

    public sealed class PageResult<T> : PageResult
    {
        public PageResult()
        {
        }

        public PageResult(List<T> list, int count)
        {
            Items = list;
            Count = count;
        }

        public List<T> Items { get; set; }
    }

    public static class Page
    {
        public static PageResult<T> GetPage<T>(this IQueryable<T> q, int pageindex, int pagesize)
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
            return new PageResult<T>
            {
                Count = count,
                Items = q.Skip((pageindex - 1) * pagesize).Take(pagesize).ToList()
            };
        }

        /// <summary>
        /// 获取分页
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="query"></param>
        /// <param name="input"></param>
        /// <returns></returns>
        public static async Task<PageResult<T>> GetPageAsync<T>(this IQueryable<T> query, AbsBasePageModel input)
        {
            int count = await query.CountAsync();
            return new PageResult<T>
            {
                Count = count,
                Items = await query.Skip((input.PageIndex - 1) * input.PageSize).Take(input.PageSize).ToListAsync()
            };
        }
    }
}