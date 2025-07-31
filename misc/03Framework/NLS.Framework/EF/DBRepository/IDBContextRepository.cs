using Microsoft.EntityFrameworkCore;
using MySql.Data.MySqlClient;
using NLS.ExtentionsCore.Page;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace NLS.Framework.EF.DBRepository
{
    internal interface IDBContextRepository<TC> where TC : DbContext, new()
    {
        #region 数据插入

        /// <summary>
        /// 通过实体模型向数据库中添加单条记录
        /// Lv
        /// 0.1
        /// 2018/02/10
        /// 返回受影响行数
        /// </summary>
        int Insert<T>(T t) where T : AbsEntity;

        /// <summary>
        /// 通过实体模型向数据库中添加单条记录
        /// Lv
        /// 0.1
        /// 2018/02/10
        /// 返回受影响行数
        /// </summary>
        Task<int> InsertAsync<T>(T t) where T : AbsEntity;

        /// <summary>
        /// 通过实模型集合向数据库中添加多条记录
        /// Lv
        /// 0.1
        /// 2018/02/10
        /// 返回受影响行数
        /// </summary>
        int Insert<T>(IEnumerable<T> t) where T : AbsEntity;

        /// <summary>
        /// 通过实模型集合向数据库中添加多条记录
        /// Lv
        /// 0.1
        /// 2018/02/10
        /// 返回受影响行数
        /// </summary>
        Task<int> InsertAsync<T>(IEnumerable<T> t) where T : AbsEntity;

        /// <summary>
        /// 执行SQL语句向数据库添加单条或多条数据
        /// Lv
        /// 0.1
        /// 2018/02/10
        /// 返回受影响行数
        /// </summary>
        int Insert<T>(string sql, params MySqlParameter[] parameters) where T : AbsEntity;

        /// <summary>
        /// 执行SQL语句向数据库添加单条或多条数据
        /// Lv
        /// 0.1
        /// 2018/02/10
        /// 返回受影响行数
        /// </summary>
        Task<int> InsertAsync<T>(string sql, params MySqlParameter[] parameters) where T : AbsEntity;

        /// <summary>
        /// 新增数据
        /// 采用转译SQL方式
        /// 通过ADO.NET
        /// </summary>
        [Obsolete("未实现方法，始终返回0 ")]
        int InsertAdoptSql<T>(T t) where T : AbsEntity;

        /// <summary>
        /// 数据批量插入
        /// </summary>
        int BulkInsert(DataTable table);

        #endregion 数据插入

        #region 更新

        /// <summary>
        /// 通过单模型更新数据库内容
        /// Lv
        /// 2018/02/26
        /// 返回受影响行数
        /// </summary>
        int Update<T>(T t) where T : AbsEntity;

        /// <summary>
        /// 通过单模型更新数据库内容
        /// Lv
        /// 2018/02/26
        /// 返回受影响行数
        /// </summary>
        Task<int> UpdateAsync<T>(T t) where T : AbsEntity;

        /// <summary>
        /// 通过模型集合更新数据库内容
        /// Lv
        /// 2018/02/26
        /// 返回受影响行数
        /// </summary>
        int Update<T>(IEnumerable<T> t) where T : AbsEntity;

        /// <summary>
        /// 通过模型集合更新数据库内容
        /// Lv
        /// 2018/02/26
        /// 返回受影响行数
        /// </summary>
        Task<int> UpdateAsync<T>(IEnumerable<T> t) where T : AbsEntity;

        /// <summary>
        /// 通过Lambda及Action更新数据库内容
        /// Lv
        /// 2018/02/26
        /// 返回受影响行数
        /// 不可用于过多行更新
        /// </summary>
        int Update<T>(Expression<Func<T, bool>> expression, Action<T> action) where T : AbsEntity;

        /// <summary>
        /// 通过Lambda及Action更新数据库内容
        /// Lv
        /// 2018/02/26
        /// 返回受影响行数
        /// 不可用于过多行更新
        /// </summary>
        Task<int> UpdateAsync<T>(Expression<Func<T, bool>> expression, Action<T> action) where T : AbsEntity;

        /// <summary>
        /// 通过SQL语句更新数据库内容
        /// Lv
        /// 2018/02/26
        /// 返回受影响行数
        /// </summary>
        int Update(string sql, params MySqlParameter[] parameters);

        /// <summary>
        /// 通过SQL语句更新数据库内容
        /// Lv
        /// 2018/02/26
        /// 返回受影响行数
        /// </summary>
        Task<int> UpdateAsync(string sql, params MySqlParameter[] parameters);

        /// <summary>
        /// 通过SQL语句更新数据库内容
        /// Lv
        /// 2018/02/26
        /// 返回受影响行数
        /// </summary>
        [Obsolete("未实现更新方法，始终返回 0 ")]
        int UpdateAdoptSql<T>(T t) where T : AbsEntity;

        #endregion 更新

        #region 删除

        /// <summary>
        /// 通过实体模型删除数据库中数据
        /// Lv
        /// 0.1
        /// 2018/02/10
        /// 返回受影响行数
        /// </summary>
        Task<int> DeleteAsync<T>(T t) where T : AbsEntity;

        /// <summary>
        /// 通过实体模型删除数据库中数据
        /// Lv
        /// 0.1
        /// 2018/02/10
        /// 返回受影响行数
        /// </summary>
        int Delete<T>(T t) where T : AbsEntity;

        /// <summary>
        /// 通过实体模型集合删除数据库中数据
        /// 返回受影响行数
        /// Lv
        /// 0.1
        /// 2018/02/10
        /// 返回受影响行数
        /// </summary>
        Task<int> DeleteAsync<T>(IEnumerable<T> t) where T : AbsEntity;

        /// <summary>
        /// 通过实体模型集合删除数据库中数据
        /// 返回受影响行数
        /// Lv
        /// 0.1
        /// 2018/02/10
        /// 返回受影响行数
        /// </summary>
        int Delete<T>(IEnumerable<T> t) where T : AbsEntity;

        /// <summary>
        /// 通过Lambda表达式删除数据库中数据
        /// 只可用于小规模数据删除
        /// Lv
        /// 0.1
        /// 2018/02/10
        /// 返回受影响行数
        /// </summary>
        Task<int> DeleteAsync<T>(Expression<Func<T, bool>> expression) where T : AbsEntity;

        /// <summary>
        /// 通过Lambda表达式删除数据库中数据
        /// 只可用于小规模数据删除
        /// Lv
        /// 0.1
        /// 2018/02/10
        /// 返回受影响行数
        /// </summary>
        int Delete<T>(Expression<Func<T, bool>> expression) where T : AbsEntity;

        #endregion 删除

        #region 查询

        /// <summary>
        /// 通过Lambda或查询单条或多条记录
        /// 条件可空
        /// Lv
        /// 2018/02/26
        /// </summary>
        IQueryable<T> Search<T>(Expression<Func<T, bool>> expression = null) where T : AbsEntity;

        /// <summary>
        /// 通过Lambda或查询单条或多条记录
        /// 条件可空
        /// Lv
        /// 2018/02/26
        /// </summary>
        Task<IQueryable<T>> SearchAsync<T>(Expression<Func<T, bool>> expression = null) where T : AbsEntity;

        /// <summary>
        /// 通过Lambda查询单条记录
        /// 条件可空
        /// Lv
        /// 2018/02/26
        /// </summary>
        T SearchFirstOrDefault<T>(Expression<Func<T, bool>> expression = null) where T : AbsEntity;

        /// <summary>
        /// 通过Lambda查询单条记录
        /// 条件可空
        /// Lv
        /// 2018/02/26
        /// </summary>
        Task<T> SearchFirstOrDefaultAsync<T>(Expression<Func<T, bool>> expression = null) where T : AbsEntity;

        /// <summary>
        /// 数据查询
        /// ADO.NET
        T SearchFirstOrDefault<T>(string sql, params MySqlParameter[] paramer) where T : class, new();

        /// <summary>
        /// 数据查询
        /// ADO.NET
        ///</summary>
        Task<T> SearchFirstOrDefaultAsync<T>(string sql, params MySqlParameter[] paramer) where T : class, new();

        /// <summary>
        /// 通过Lambda语句查询单条或多条List集合
        /// 条件可空
        /// Lv
        /// 2018/02/26
        /// </summary>
        List<T> SearchList<T>(Expression<Func<T, bool>> expression = null) where T : AbsEntity;

        /// <summary>
        /// 通过Lambda语句查询单条或多条List集合
        /// 条件可空
        /// Lv
        /// 2018/02/26
        /// </summary>
        Task<List<T>> SearchListAsync<T>(Expression<Func<T, bool>> expression = null) where T : AbsEntity;

        /// <summary>
        /// 通过SQL语句查询单条或多条List集合
        /// 条件可空
        /// Lv
        /// 2018/02/26
        /// </summary>
        List<T> SearchList<T>(string sql, params MySqlParameter[] paramer) where T : class, new();

        /// <summary>
        /// 通过SQL语句查询单条或多条List集合
        /// 条件可空
        /// Lv
        /// 2018/02/26
        /// </summary>
        Task<List<T>> SearchListAsync<T>(string sql, params MySqlParameter[] paramer) where T : class, new();

        /// <summary>
        /// 对复杂查询结果进行分页
        /// Lv
        /// 2018/04/28
        /// </summary>
        PageResut<T> SearchPageList<T>(string sql, int pageIndex, int pageSize, params MySqlParameter[] parameter) where T : class, new();

        /// <summary>
        /// 查询单列
        /// </summary>
        object SearchScalar(string sql, params MySqlParameter[] parameter);

        #endregion 查询

        #region ADO.NET

        /// <summary>
        /// 判断表是否存在
        /// ADO.NET
        /// </summary>
        bool TableExist(string tableName);

        /// <summary>
        /// 执行SQL语句,返回受影响行数
        /// </summary>
        int ExecuteSqlReturnRows(string sql, params MySqlParameter[] parameter);

        /// <summary>
        ///  获取EF所对应库的ADO.NET连接
        /// </summary>
        /// <returns>MySqlConnection</returns>
        MySqlConnection GetMySqlConnection();

        /// <summary>
        /// 执行存储过程
        /// ADO.NET
        /// </summary>
        T ExecuteStoredProcedure<T>(string procedureName, params MySqlParameter[] parameter) where T : class, new();

        #endregion ADO.NET
    }
}