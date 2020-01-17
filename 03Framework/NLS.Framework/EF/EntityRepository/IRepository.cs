using Microsoft.EntityFrameworkCore.Metadata;
using NLS.ExtentionsCore.Page;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace NLS.Framework.EF.Repository
{
    /// <summary>
    /// 数据实体交互访问接口
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IRepository<T> where T : AbsEntity, IFrameworkDependency
    {
        /// <summary>
        /// 以NoTracking获取所有数据
        /// </summary>
        /// <param name="expression">查询表达式</param>
        /// <returns>IQuery数据集</returns>
        IQueryable<T> GetAllAsNoTracking(Expression<Func<T, bool>> expression = null);

        /// <summary>
        /// 获取所有数据
        /// </summary>
        /// <param name="expression">查询表达式</param>
        /// <returns>IQuery数据集</returns>
        IQueryable<T> GetAll(Expression<Func<T, bool>> expression = null);

        /// <summary>
        /// 删除,通过实体类删除
        /// </summary>
        /// <param name="entity">实体</param>
        /// <returns>受影响行数</returns>
        Task<int> DeleteAsync(T entity);

        /// <summary>
        /// 删除,通过主键
        /// </summary>
        /// <param name="id">主键</param>
        /// <returns>受影响行数</returns>
        Task<int> DeleteAsync(long id);

        /// <summary>
        /// 删除,通过表达示
        /// </summary>
        /// <param name="expression">要删除数据表达式</param>
        /// <returns>受影响行数</returns>
        Task<int> DeleteAsync(Expression<Func<T, bool>> expression);

        /// <summary>
        /// 删除,通过实体类删除
        /// </summary>
        /// <param name="entity">实体</param>
        /// <returns>受影响行数</returns>
        int Delete(T entity);
        /// <summary>
        /// 删除,通过主键
        /// </summary>
        /// <param name="id">主键</param>
        /// <returns>受影响行数</returns>
        int Delete(long id);

        /// <summary>
        /// 删除,通过表达示
        /// </summary>
        /// <param name="expression">要删除数据表达式</param>
        /// <returns>受影响行数</returns>
        int Delete(Expression<Func<T, bool>> expression);

        /// <summary>
        /// 分页
        /// </summary>
        /// <typeparam name="TC"></typeparam>
        /// <param name="whereLambda">查询条件</param>
        /// <param name="orderbyLambda">排序条件</param>
        /// <param name="isAsc">是否升序</param>
        /// <param name="pageIndex">当前页码</param>
        /// <param name="pageSize">每页大小</param>
        /// <returns></returns>
        [Obsolete("弃用的，请使用 NLS.AspNetCore.Linq.Page.GetPageAsync() 方法，分页方法须继承自 AbsBasePageModel")]
        PageResut<TC> SearchPageList<TC>(Expression<Func<TC, bool>> whereLambda, Expression<Func<TC, IKey>> orderbyLambda, bool isAsc, int pageIndex, int pageSize) where TC : class, new();
    }
}