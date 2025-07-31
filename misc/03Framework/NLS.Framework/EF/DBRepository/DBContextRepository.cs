using Microsoft.EntityFrameworkCore;
using MySql.Data.MySqlClient;
using NLS.ExtentionsCore.Page;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace NLS.Framework.EF.DBRepository
{
    public sealed class DBContextRepository<TC> : IDBContextRepository<TC> where TC : AbsBaseContext, new()
    {
        #region 构造

        private readonly TC TC_OBJECT;

        public DBContextRepository()
        {
            if (TC_OBJECT == null)
            {
                TC_OBJECT = new TC();
            }
        }

        #endregion 构造

        #region 删除

        /// <summary>
        /// 通过实体模型删除数据库中数据
        /// Lv
        /// 0.2
        /// 2018/02/10
        /// 返回受影响行数
        /// </summary>
        public int Delete<T>(T t) where T : AbsEntity
        {
            using (TC tc = new TC())
            {
                if (t == null) { return 0; }
                tc.Entry(t).State = EntityState.Deleted;
                return tc.SaveChanges();
            }
        }

        /// <summary>
        /// 通过实体模型删除数据库中数据
        /// Lv
        /// 0.2
        /// 2018/02/10
        /// 返回受影响行数
        /// </summary>
        public async Task<int> DeleteAsync<T>(T t) where T : AbsEntity
        {
            if (t == null) { return 0; }
            using (TC tc = new TC())
            {
                tc.Entry(t).State = EntityState.Deleted;
                return await tc.SaveChangesAsync();
            }
        }

        /// <summary>
        /// 通过实体模型集合删除数据库中数据
        /// 返回受影响行数
        /// Lv
        /// 0.2
        /// 2018/02/10
        /// 返回受影响行数
        /// </summary>
        public int Delete<T>(IEnumerable<T> t) where T : AbsEntity
        {
            if (t == null) { return 0; }
            using (TC tc = new TC())
            {
                foreach (var item in t)
                {
                    tc.Entry(item).State = EntityState.Deleted;
                }
                return tc.SaveChanges();
            }
        }

        /// <summary>
        /// 通过实体模型集合删除数据库中数据
        /// 返回受影响行数
        /// Lv
        /// 0.2
        /// 2018/02/10
        /// 返回受影响行数
        /// </summary>
        public async Task<int> DeleteAsync<T>(IEnumerable<T> t) where T : AbsEntity
        {
            if (t == null) { return 0; }
            using (TC tc = new TC())
            {
                foreach (var item in t)
                {
                    tc.Entry(item).State = EntityState.Deleted;
                }
                return await tc.SaveChangesAsync();
            }
        }

        /// <summary>
        /// 通过Lambda表达式删除数据库中数据
        /// 只可用于小规模数据删除
        /// Lv
        /// 0.2
        /// 2018/02/10
        /// 返回受影响行数
        /// </summary>
        public int Delete<T>(Expression<Func<T, bool>> expression) where T : AbsEntity
        {
            if (expression == null)
            {
                throw new ArgumentNullException("expression");
            }
            using (TC tc = new TC())
            {
                IEnumerable<T> t = tc.Set<T>().Where(expression);
                foreach (var item in t)
                {
                    tc.Entry<T>(item).State = EntityState.Deleted;
                }
                return tc.SaveChanges();
            }
        }

        /// <summary>
        /// 通过Lambda表达式删除数据库中数据
        /// 只可用于小规模数据删除
        /// Lv
        /// 0.2
        /// 2018/02/10
        /// 返回受影响行数
        /// </summary>
        public async Task<int> DeleteAsync<T>(Expression<Func<T, bool>> expression) where T : AbsEntity
        {
            return await Task.Run(() =>
            {
                if (expression == null)
                {
                    throw new ArgumentNullException("expression");
                }
                using (TC tc = new TC())
                {
                    //tc.Remove(expression);
                    IEnumerable<T> t = tc.Set<T>().Where(expression);
                    foreach (var item in t)
                    {
                        tc.Entry<T>(item).State = EntityState.Deleted;
                    }
                    return tc.SaveChangesAsync();
                }
            });
        }

        #endregion 删除

        #region 新增

        /// <summary>
        /// 通过实体模型向数据库中添加单条记录
        /// Lv
        /// 0.2
        /// 2018/02/10
        /// 返回受影响行数
        /// </summary>
        public int Insert<T>(T t) where T : AbsEntity
        {
            if (t == null) { return 0; }
            using (TC tc = new TC())
            {
                tc.Entry(t).State = EntityState.Added;
                return tc.SaveChanges();
            }
        }

        /// <summary>
        /// 通过实体模型向数据库中添加单条记录
        /// Lv
        /// 0.2
        /// 2018/02/10
        /// 返回受影响行数
        /// </summary>
        public async Task<int> InsertAsync<T>(T t) where T : AbsEntity
        {
            if (t == null) { return 0; }
            using (TC tc = new TC())
            {
                tc.Entry(t).State = EntityState.Added;
                return await tc.SaveChangesAsync();
            }
        }

        /// <summary>
        /// 通过实模型集合向数据库中添加多条记录
        /// Lv
        /// 0.2
        /// 2018/02/10
        /// 返回受影响行数
        /// </summary>
        public int Insert<T>(IEnumerable<T> t) where T : AbsEntity
        {
            if (t == null) { return 0; }
            using (TC tc = new TC())
            {
                foreach (var item in t)
                {
                    tc.Entry(item).State = EntityState.Added;
                }
                return tc.SaveChanges();
            }
        }

        /// <summary>
        /// 通过实模型集合向数据库中添加多条记录
        /// Lv
        /// 0.1
        /// 2018/02/10
        /// 返回受影响行数
        /// </summary>
        public async Task<int> InsertAsync<T>(IEnumerable<T> t) where T : AbsEntity
        {
            if (t == null) { return await Task.FromResult<int>(0); }
            using (TC tc = new TC())
            {
                foreach (var item in t)
                {
                    tc.Entry(item).State = EntityState.Added;
                }
                return await tc.SaveChangesAsync();
            }
        }

        /// <summary>
        /// 执行SQL语句向数据库添加单条或多条数据
        /// Lv
        /// 0.1
        /// 2018/02/10
        /// 返回受影响行数
        /// </summary>
        public int Insert<T>(string sql, params MySqlParameter[] parameters) where T : AbsEntity
        {
            return ExecuteSql(sql, parameters);
        }

        /// <summary>
        /// 执行SQL语句向数据库添加单条或多条数据
        /// Lv
        /// 0.1
        /// 2018/02/10
        /// 返回受影响行数
        /// </summary>
        public async Task<int> InsertAsync<T>(string sql, params MySqlParameter[] parameters) where T : AbsEntity
        {
            return await ExecuteSqlAsync(sql, parameters);
        }

        [Obsolete("未实现方法，始终返回0 ")]
        public int InsertAdoptSql<T>(T t) where T : AbsEntity
        {
            return 0;
        }

        public int BulkInsert(DataTable table)
        {
            if (string.IsNullOrWhiteSpace(table.TableName)) { throw new Exception("table.TableName"); }
            if (table.Rows.Count == 0) return 0;
            int insertCount = 0;
            string tempPath = Path.GetTempFileName();
            string csv = DataTableToCsv(table);
            File.WriteAllText(tempPath, csv);
            using (MySqlConnection conn = (MySqlConnection)TC_OBJECT.Database.GetDbConnection())
            {
                MySqlTransaction tran = null;
                try
                {
                    conn.Open();
                    tran = conn.BeginTransaction();
                    MySqlBulkLoader bulk = new MySqlBulkLoader(conn)
                    {
                        FieldTerminator = ",",
                        FieldQuotationCharacter = '"',
                        EscapeCharacter = '"',
                        LineTerminator = "\r\n",
                        FileName = tempPath,
                        NumberOfLinesToSkip = 0,
                        TableName = table.TableName,
                    };
                    bulk.Columns.AddRange(table.Columns.Cast<DataColumn>().Select(colum => colum.ColumnName).ToList());
                    insertCount = bulk.Load();
                    tran.Commit();
                }
                catch
                {
                    if (tran != null) tran.Rollback();
                }
                File.Delete(tempPath);
                return insertCount;
            }
        }

        #endregion 新增

        #region 查询

        /// <summary>
        /// 通过Lambda或查询单条或多条记录
        /// 条件可空
        /// Lv
        /// 2018/02/26
        /// </summary>
        public IQueryable<T> Search<T>(Expression<Func<T, bool>> expression = null) where T : AbsEntity
        {
            if (expression == null)
            {
                return TC_OBJECT.Set<T>().AsNoTracking();
            }
            return TC_OBJECT.Set<T>().Where(expression).AsNoTracking();
        }

        /// <summary>
        /// 通过Lambda或查询单条或多条记录
        /// 条件可空
        /// Lv
        /// 2018/02/26
        /// </summary>
        public async Task<IQueryable<T>> SearchAsync<T>(Expression<Func<T, bool>> expression = null) where T : AbsEntity
        {
            if (expression == null)
            {
                return await Task.FromResult<IQueryable<T>>(TC_OBJECT.Set<T>().AsNoTracking());
            }
            return await Task.FromResult<IQueryable<T>>(TC_OBJECT.Set<T>().Where(expression).AsNoTracking());
        }

        /// <summary>
        /// 通过Lambda查询单条记录
        /// 条件可空
        /// Lv
        /// 0.2
        /// 2018/02/26
        /// </summary>
        public T SearchFirstOrDefault<T>(Expression<Func<T, bool>> expression = null) where T : AbsEntity
        {
            using (TC tc = new TC())
            {
                if (expression == null)
                {
                    return tc.Set<T>().AsNoTracking().FirstOrDefault();
                }
                return tc.Set<T>().Where(expression).AsNoTracking().FirstOrDefault();
            }
        }

        /// <summary>
        /// 通过Lambda查询单条记录
        /// 条件可空
        /// Lv
        /// 0.2
        /// 2018/02/26
        /// </summary>
        public async Task<T> SearchFirstOrDefaultAsync<T>(Expression<Func<T, bool>> expression = null) where T : AbsEntity
        {
            using (TC tc = new TC())
            {
                if (expression == null)
                {
                    return await tc.Set<T>().AsNoTracking().FirstOrDefaultAsync();
                }
                return await tc.Set<T>().Where(expression).AsNoTracking().FirstOrDefaultAsync();
            }
        }

        /// <summary>
        /// 通过SQL语句查询单条记录
        /// Lv
        /// 2018/02/26
        /// </summary>
        public T SearchFirstOrDefault<T>(string sql, params MySqlParameter[] paramer) where T : class, new()
        {
            Object t_obj = null;
            ExecuteSql(sql, paramer, false, p =>
            {
                while (p.Read())
                {
                    t_obj = ReaderObject<T>(p);
                }
            });
            return t_obj == null ? default(T) : (T)t_obj;
        }

        public async Task<T> SearchFirstOrDefaultAsync<T>(string sql, params MySqlParameter[] paramer) where T : class, new()
        {
            Object t_obj = null;
            await ExecuteSqlAsync(sql, paramer, false, p =>
            {
                while (p.Read())
                {
                    t_obj = ReaderObject<T>(p);
                }
            });
            return t_obj == null ? default(T) : (T)t_obj;
        }

        /// <summary>
        /// 通过Lambda语句查询单条或多条List集合
        /// 条件可空
        /// Lv
        /// 0.2
        /// 2018/02/26
        /// </summary>
        public List<T> SearchList<T>(Expression<Func<T, bool>> expression = null) where T : AbsEntity
        {
            using (TC tc = new TC())
            {
                if (expression == null)
                {
                    return tc.Set<T>().AsNoTracking().ToList();
                }
                return tc.Set<T>().Where(expression).AsNoTracking().ToList();
            }
        }

        /// <summary>
        /// 通过Lambda语句查询单条或多条List集合
        /// 条件可空
        /// Lv
        /// 0.2
        /// 2018/02/26
        /// </summary>
        public async Task<List<T>> SearchListAsync<T>(Expression<Func<T, bool>> expression = null) where T : AbsEntity
        {
            using (TC tc = new TC())
            {
                if (expression == null)
                {
                    return await tc.Set<T>().AsNoTracking().ToListAsync();
                }
                return await tc.Set<T>().Where(expression).AsNoTracking().ToListAsync();
            }
        }

        /// <summary>
        /// 通过SQL语句查询单条或多条List集合
        /// 条件可空
        /// Lv
        /// 2018/02/26
        /// </summary>
        public List<T> SearchList<T>(string sql, params MySqlParameter[] paramer) where T : class, new()
        {
            List<T> list = new List<T>();
            ExecuteSql(sql, paramer, false, p =>
            {
                while (p.Read())
                {
                    var Result = ReaderObject<T>(p);
                    if (Result != null)
                    {
                        list.Add((T)Result);
                    }
                }
            });
            return list;
        }

        /// <summary>
        /// 通过SQL语句查询单条或多条List集合
        /// 条件可空
        /// Lv
        /// 2018/02/26
        /// </summary>
        public async Task<List<T>> SearchListAsync<T>(string sql, params MySqlParameter[] paramer) where T : class, new()
        {
            List<T> list = new List<T>();
            await ExecuteSqlAsync(sql, paramer, false, p =>
             {
                 while (p.Read())
                 {
                     var Result = ReaderObject<T>(p);
                     if (Result != null)
                     {
                         list.Add((T)Result);
                     }
                 }
             });
            // ExecuteSql(sql, paramer, false, p =>
            // {
            //     while (p.Read())
            //     {
            //         var Result = ReaderObject<T>(p);
            //         if (Result != null)
            //         {
            //             list.Add((T)Result);
            //         }
            //     }
            // });
            return await Task.FromResult(list);
        }

        public PageResut<T> SearchPageList<T>(string sql, int pageIndex, int pageSize, params MySqlParameter[] parameter) where T : class, new()
        {
            if (string.IsNullOrWhiteSpace(sql)) { throw new ArgumentNullException("SQL语句不可为空"); }
            if (pageIndex <= 0 || pageSize <= 0) { throw new SystemException("PageSize或PageIndex应大于0"); }
            using (var conn = TC_OBJECT.Database.GetDbConnection())
            {
                using (var cmd = conn.CreateCommand())
                {
                    conn.Open();
                    cmd.CommandType = CommandType.Text;
                    if (parameter.Length > 0)
                    {
                        cmd.Parameters.AddRange(parameter);
                    }
                    cmd.CommandText = string.Format("SELECT COUNT(*) FROM ({0}) _T_E_QWERTY", sql);
                    int.TryParse(cmd.ExecuteScalar().ToString(), out int ItemCount);
                    if (ItemCount == 0)
                    {
                        return new PageResut<T>();
                    }
                    int PageCount = (ItemCount / pageSize) + 1;
                    cmd.CommandText = string.Format("SELECT * FROM ({0}) _T_E_QWERTY LIMIT {1},{2}", sql, ((pageIndex - 1) * pageSize), pageSize);
                    MySqlDataReader Reader = (MySqlDataReader)cmd.ExecuteReader();
                    List<T> t_list = new List<T>();
                    while (Reader.Read())
                    {
                        var r = ReaderObject<T>(Reader);
                        if (r != null)
                        {
                            t_list.Add((T)r);
                        }
                    }
                    return new PageResut<T>(t_list, ItemCount);
                }
            }
        }

        /// <summary>
        /// 查询单列
        /// </summary>
        public object SearchScalar(string sql, params MySqlParameter[] parameter)
        {
            object p = null;
            ExecuteSql(string.Format("SELECT * FROM ({0}) __T_E_QWERTY LIMIT 1 ", sql), parameter, false, reader =>
            {
                if (reader.HasRows)
                {
                    reader.Read();
                    if (reader.FieldCount != 0 && !(reader[0] is DBNull))
                    {
                        p = reader[0];
                    }
                }
            });
            return p;
        }

        #endregion 查询

        #region 更新

        /// <summary>
        /// 通过单模型更新数据库内容
        /// Lv
        /// 0.2
        /// 2018/02/26
        /// 返回受影响行数
        /// </summary>
        public int Update<T>(T t) where T : AbsEntity
        {
            if (t == null) { return 0; }
            //using (TC tc = new TC())
            //{
            TC_OBJECT.Entry(t).State = EntityState.Modified;
            return TC_OBJECT.SaveChanges();
            //}
        }

        /// <summary>
        /// 通过单模型更新数据库内容
        /// Lv
        /// 0.2
        /// 2018/02/26
        /// 返回受影响行数
        /// </summary>
        public async Task<int> UpdateAsync<T>(T t) where T : AbsEntity
        {
            if (t == null) { return 0; }
            //using (TC tc = new TC())
            //{
            TC_OBJECT.Entry(t).State = EntityState.Modified;
            return await TC_OBJECT.SaveChangesAsync();
            //}
        }

        /// <summary>
        /// 通过模型集合更新数据库内容
        /// Lv
        /// 0.2
        /// 2018/02/26
        /// 返回受影响行数
        /// </summary>
        public int Update<T>(IEnumerable<T> t) where T : AbsEntity
        {
            if (t == null) { return 0; }
            //using (TC tc = new TC())
            //{
            foreach (var item in t)
            {
                TC_OBJECT.Entry(item).State = EntityState.Modified;
            }
            return TC_OBJECT.SaveChanges();
            //}
        }

        /// <summary>
        /// 通过模型集合更新数据库内容
        /// Lv
        /// 0.2
        /// 2018/02/26
        /// 返回受影响行数
        /// </summary>
        public async Task<int> UpdateAsync<T>(IEnumerable<T> t) where T : AbsEntity
        {
            if (t == null) { return 0; }
            //using (TC tc = new TC())
            //{
            foreach (var item in t)
            {
                TC_OBJECT.Entry(item).State = EntityState.Modified;
            }
            return await TC_OBJECT.SaveChangesAsync();
            //}
        }

        /// <summary>
        /// 通过Lambda及Action更新数据库内容
        /// Lv
        /// 2018/02/26
        /// 返回受影响行数
        /// 不可用于过多行更新
        /// </summary>
        public int Update<T>(Expression<Func<T, bool>> expression, Action<T> action) where T : AbsEntity
        {
            if (expression == null || action == null)
            {
                throw new ArgumentNullException("expression , action ");
            }
            //using (TC tc = new TC())
            //{
            IEnumerable<T> _list = TC_OBJECT.Set<T>().Where(expression);
            foreach (var item in _list)
            {
                action(item);
                TC_OBJECT.Entry<T>(item).State = EntityState.Modified;
            }
            return TC_OBJECT.SaveChanges();
            //}
        }

        /// <summary>
        /// 通过Lambda及Action更新数据库内容
        /// Lv
        /// 2018/02/26
        /// 返回受影响行数
        /// 不可用于过多行更新
        /// </summary>
        public async Task<int> UpdateAsync<T>(Expression<Func<T, bool>> expression, Action<T> action) where T : AbsEntity
        {
            if (expression == null || action == null)
            {
                throw new ArgumentNullException("expression , action ");
            }
            IEnumerable<T> _list = TC_OBJECT.Set<T>().Where(expression);
            foreach (var item in _list)
            {
                action(item);
                TC_OBJECT.Entry<T>(item).State = EntityState.Modified;
            }
            return await TC_OBJECT.SaveChangesAsync();
        }

        /// <summary>
        /// 通过SQL语句更新数据库内容
        /// Lv
        /// 2018/02/26
        /// 返回受影响行数
        /// </summary>
        public int Update(string sql, params MySqlParameter[] parameters)
        {
            return ExecuteSql(sql, parameters);
        }

        /// <summary>
        /// 通过SQL语句更新数据库内容
        /// Lv
        /// 2018/02/26
        /// 返回受影响行数
        /// </summary>
        public async Task<int> UpdateAsync(string sql, MySqlParameter[] parameters = null)
        {
            return await ExecuteSqlAsync(sql, parameters);
        }

        [Obsolete("未实现更新方法，始终返回 0 ")]
        public int UpdateAdoptSql<T>(T t) where T : AbsEntity
        {
            return 0;
        }

        #endregion 更新

        #region SQL相关

        /// <summary>
        /// 检查表 是否存在
        /// </summary>
        /// <param name="tableName">表名</param>
        /// <returns>True:存在,False:未查询到或不存在</returns>
        public bool TableExist(string tableName)
        {
            using (var conn = TC_OBJECT.Database.GetDbConnection())
            {
                using (var cmd = conn.CreateCommand())
                {
                    conn.Open();
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = string.Format("SHOW TABLES LIKE '%{0}%'", tableName);
                    return cmd.ExecuteScalar() != null;
                }
            }
        }

        /// <summary>
        /// 执行SQL语句,返回受影响行数
        /// </summary>
        public int ExecuteSqlReturnRows(string sql, params MySqlParameter[] parameter)
        {
            if (string.IsNullOrWhiteSpace(sql))
            {
                throw new ArgumentNullException("SQL语句不可为空");
            }
            return ExecuteSql(sql, parameter);
        }

        /// <summary>
        /// 执行存储过程
        /// 0.1
        /// 2018/02/26
        /// </summary>
        public T ExecuteStoredProcedure<T>(string procedureName, params MySqlParameter[] parameter) where T : class, new()
        {
            Object t_obj = null;
            ExecuteSql(procedureName, parameter, true, p =>
            {
                while (p.Read())
                {
                    t_obj = ReaderObject<T>(p);
                }
            });
            return t_obj == null ? default(T) : (T)t_obj;
        }

        #endregion SQL相关

        #region 执行SQL

        /// <summary>
        /// 执行SQL语句返回受影响行数
        /// 0.1
        /// Lv
        /// 2018/02/10
        /// </summary>
        private int ExecuteSql(string sql, params MySqlParameter[] parameters)
        {
            if (!string.IsNullOrWhiteSpace(sql))
            {
                using (var conn = TC_OBJECT.Database.GetDbConnection())
                {
                    using (var cmd = conn.CreateCommand())
                    {
                        conn.Open();
                        cmd.CommandType = CommandType.Text;
                        cmd.CommandText = sql;
                        if (parameters != null)
                        {
                            cmd.Parameters.AddRange(parameters);
                        }
                        return cmd.ExecuteNonQuery();
                    }
                }
            }
            return 0;
        }

        private async Task<int> ExecuteSqlAsync(string sql, MySqlParameter[] parameters)
        {
            if (!string.IsNullOrWhiteSpace(sql))
            {
                using (var conn = TC_OBJECT.Database.GetDbConnection())
                {
                    using (var cmd = conn.CreateCommand())
                    {
                        await conn.OpenAsync();
                        cmd.CommandType = CommandType.Text;
                        cmd.CommandText = sql;
                        if (parameters != null)
                        {
                            cmd.Parameters.AddRange(parameters);
                        }
                        return await cmd.ExecuteNonQueryAsync();
                    }
                }
            }
            return await Task.FromResult(0);
        }

        private async Task ExecuteSqlAsync(string sql, MySqlParameter[] parameter, bool isProcedure, Action<MySqlDataReader> action)
        {
            if (!string.IsNullOrWhiteSpace(sql))
            {
                using (var conn = TC_OBJECT.Database.GetDbConnection())
                {
                    using (var cmd = conn.CreateCommand())
                    {
                        await conn.OpenAsync();
                        cmd.CommandType = isProcedure ? CommandType.StoredProcedure : CommandType.Text;
                        cmd.CommandText = sql;
                        if (parameter != null)
                        {
                            cmd.Parameters.AddRange(parameter);
                        }
                        action((MySqlDataReader)(await cmd.ExecuteReaderAsync()));
                    }
                }
            }
        }

        /// <summary>
        /// 执行SQL语句
        /// 执行存储过程等
        /// 0.1
        /// Lv
        /// 2018/02/10
        /// </summary>
        private void ExecuteSql(string sql, MySqlParameter[] parameter, bool isProcedure, Action<MySqlDataReader> action)
        {
            if (!string.IsNullOrWhiteSpace(sql))
            {
                using (var conn = TC_OBJECT.Database.GetDbConnection())
                {
                    using (var cmd = conn.CreateCommand())
                    {
                        conn.Open();
                        cmd.CommandType = isProcedure ? CommandType.StoredProcedure : CommandType.Text;
                        cmd.CommandText = sql;
                        if (parameter != null)
                        {
                            cmd.Parameters.AddRange(parameter);
                        }
                        action((MySqlDataReader)cmd.ExecuteReader());
                    }
                }
            }
        }

        #endregion 执行SQL

        #region 相关

        private object ReaderObject<T>(MySqlDataReader reader)
        {
            object result = null;
            if (typeof(bool) == typeof(T))
            {
                result = ReaderElement(reader, 0, typeof(bool));
            }
            else if (typeof(byte) == typeof(T))
            {
                result = ReaderElement(reader, 0, typeof(byte));
            }
            else if (typeof(char) == typeof(T))
            {
                result = ReaderElement(reader, 0, typeof(char));
            }
            else if (typeof(DateTime) == typeof(T) || typeof(DateTime?) == typeof(T))
            {
                result = ReaderElement(reader, 0, typeof(DateTimeOffset));
            }
            else if (typeof(decimal) == typeof(T))
            {
                result = ReaderElement(reader, 0, typeof(decimal));
            }
            else if (typeof(double) == typeof(T))
            {
                result = ReaderElement(reader, 0, typeof(double));
            }
            else if (typeof(float) == typeof(T))
            {
                result = ReaderElement(reader, 0, typeof(float));
            }
            else if (typeof(Guid) == typeof(T))
            {
                result = ReaderElement(reader, 0, typeof(Guid));
            }
            else if (typeof(short) == typeof(T))
            {
                result = ReaderElement(reader, 0, typeof(short));
            }
            else if (typeof(int) == typeof(T))
            {
                result = ReaderElement(reader, 0, typeof(int));
            }
            else if (typeof(string) == typeof(T))
            {
                result = ReaderElement(reader, 0, typeof(string));
            }
            else
            {
                result = Activator.CreateInstance<T>();
                int i = 0;
                foreach (PropertyDescriptor property in TypeDescriptor.GetProperties(result))
                {
                    try
                    {
                        if (reader.GetSchemaTable().Select("ColumnName='" + property.DisplayName + "'").Length > 0)
                        {
                            i = reader.GetOrdinal(property.DisplayName);
                            property.SetValue(result, ReaderElement(reader, i, property.PropertyType));
                        }
                    }
                    catch (Exception ex)
                    {
                        throw new ArgumentException("绑定属性[" + property.Name + "]发生异常:" + ex.Message);
                    }
                }
            }
            return result;
        }

        private object ReaderElement(MySqlDataReader reader, int count, Type type)
        {
            if (typeof(bool) == type)
            {
                return reader.GetBoolean(count);
            }
            else if (typeof(byte) == type)
            {
                return reader.GetByte(count);
            }
            else if (typeof(char) == type)
            {
                return reader.GetChar(count);
            }
            else if (typeof(DateTime) == type || typeof(DateTime?) == type)
            {
                object ra = reader[count];
                if (ra is DBNull)
                {
                    return new DateTime?();
                }
                return ra;
            }
            else if (typeof(decimal) == type || typeof(decimal?) == type)
            {
                object ra = reader[count];
                if (ra is DBNull)
                {
                    return new decimal?();
                }
                return ra;
            }
            else if (typeof(double) == type)
            {
                return reader.GetDouble(count);
            }
            else if (typeof(float) == type)
            {
                return reader.GetFloat(count);
            }
            else if (typeof(Guid) == type)
            {
                return reader.GetGuid(count);
            }
            else if (typeof(short) == type)
            {
                return reader.GetInt16(count);
            }
            else if (typeof(int) == type || typeof(int?) == type)
            {
                object ra = reader[count];
                if (ra is DBNull)
                {
                    return new int?();
                }
                return ra;
            }
            else if (typeof(long) == type)
            {
                object ra = reader[count];
                if (ra is DBNull)
                {
                    return new long?();
                }
                return Convert.ToInt64(ra);
            }
            else
            {
                return reader.IsDBNull(count) ? null : reader.GetString(count);
            }
        }

        #endregion 相关

        #region MySqlConnection

        public MySqlConnection GetMySqlConnection() => (MySqlConnection)TC_OBJECT.Database.GetDbConnection();

        #endregion MySqlConnection

        #region

        /// <summary>
        ///将DataTable转换为标准的CSV
        /// </summary>
        /// <param name="table">数据表</param>
        /// <returns>返回标准的CSV</returns>
        private static string DataTableToCsv(DataTable table)
        {
            //以半角逗号（即,）作分隔符，列为空也要表达其存在。
            //列内容如存在半角逗号（即,）则用半角引号（即""）将该字段值包含起来。
            //列内容如存在半角引号（即"）则应替换成半角双引号（""）转义，并用半角引号（即""）将该字段值包含起来。
            StringBuilder sb = new StringBuilder();
            DataColumn colum;
            foreach (DataRow row in table.Rows)
            {
                for (int i = 0; i < table.Columns.Count; i++)
                {
                    colum = table.Columns[i];
                    if (i != 0) sb.Append(",");
                    if (colum.DataType == typeof(string) && row[colum].ToString().Contains(","))
                    {
                        sb.Append("\"" + row[colum].ToString().Replace("\"", "\"\"") + "\"");
                    }
                    else sb.Append(row[colum].ToString());
                }
                sb.AppendLine();
            }
            return sb.ToString();
        }

        #endregion
    }
}