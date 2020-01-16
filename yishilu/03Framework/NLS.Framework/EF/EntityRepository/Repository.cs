using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using NLS.ExtentionsCore.Page;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace NLS.Framework.EF.Repository
{
    public class Repository<T> : IFrameworkDependency, IRepository<T> where T : AbsEntity
    {
        private readonly NLSEntitesContext context;

        public Repository()
        {
            context = new NLSEntitesContext();
        }

        public int Delete(T entity)
        {
            context.Remove(entity);
            return context.SaveChanges();
        }

        public int Delete(long id)
        {
            context.Remove(context.Set<T>().Where(p => p.Id == id).FirstOrDefault());
            return context.SaveChanges();
        }

        public int Delete(Expression<Func<T, bool>> expression)
        {
            if (expression != null)
            {
                context.RemoveRange(GetAllAsNoTracking(expression).ToList());
                return context.SaveChanges();
            }
            return 0;
        }

        public async Task<int> DeleteAsync(T entity)
        {
            context.Remove(entity);
            return await context.SaveChangesAsync();
        }

        public async Task<int> DeleteAsync(long id)
        {
            context.Remove(await context.Set<T>().Where(p => p.Id == id).FirstOrDefaultAsync());
            return await context.SaveChangesAsync();
        }

        public async Task<int> DeleteAsync(Expression<Func<T, bool>> expression)
        {
            if (expression != null)
            {
                context.RemoveRange(GetAllAsNoTracking(expression).ToList());
                return await context.SaveChangesAsync();
            }
            return await Task.FromResult(0);
        }

        public IQueryable<T> GetAll(Expression<Func<T, bool>> expression)
        {
            if (expression != null)
            {
                return context.Set<T>().Where(expression);
            }
            return context.Set<T>().AsNoTracking();
        }

        public IQueryable<T> GetAllAsNoTracking(Expression<Func<T, bool>> expression = null)
        {
            if (expression != null)
            {
                return context.Set<T>().Where(expression).AsNoTracking();
            }
            return context.Set<T>().AsNoTracking();
        }

        [Obsolete("弃用的，请使用 NLS.AspNetCore.Linq.Page.GetPageAsync() 方法，分页方法须继承自 AbsBasePageModel")]
        public PageResut<TC> SearchPageList<TC>(Expression<Func<TC, bool>> whereLambda, Expression<Func<TC, IKey>> orderbyLambda, bool isAsc, int pageIndex, int pageSize) where TC : class, new()
        {
            int ItemCount = context.Set<TC>().Where(whereLambda).Count();
            List<TC> t_list = new List<TC>();
            //倒序或升序
            if (isAsc)
            {
                var temp = context.Set<TC>().Where(whereLambda)
                      .OrderBy<TC, IKey>(orderbyLambda)
                      .Skip(pageSize * (pageIndex - 1))
                      .Take(pageSize);
                t_list = temp.ToList();
            }
            else
            {
                var temp = context.Set<TC>().Where(whereLambda)
                     .OrderByDescending<TC, IKey>(orderbyLambda)
                     .Skip(pageSize * (pageIndex - 1))
                     .Take(pageSize);
                t_list = temp.ToList();
            }
            return new PageResut<TC>(t_list, ItemCount);
        }
    }
}