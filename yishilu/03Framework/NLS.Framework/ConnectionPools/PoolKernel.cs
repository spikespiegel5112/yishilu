using MySql.Data.MySqlClient;
using NLS.Configuration;
using System;
using System.Collections.Generic;
using System.Threading;

namespace NLS.Framework.ConnectionPools
{
    [Obsolete("弃用")]
    public class PoolKernel
    {
        #region Const

        /// <summary>
        /// 等待超时时间
        /// </summary>
        private static readonly int Timeout = 2000;

        /// <summary>
        /// 连接字符串
        /// </summary>
        private static string ConnectinString;

        /// <summary>
        /// 连接对象
        /// </summary>
        private static MySqlConnection _Connectin;

        /// <summary>
        /// 连接队列
        /// </summary>
        private static Stack<MySqlCommand> CommandStack;

        public static Object __lock = new object();

        #endregion Const

        #region 初始化

        /// <summary>
        /// 初始化数据库连接池
        /// </summary>
        /// <param name="configPath">配置文件地址</param>
        /// <param name="connectionName">连接字符串名称</param>
        /// <param name="count">初始化池数量，默认为30,最小为1，最大建议不超过50</param>
        public static void Init(string configPath, string connectionName, int count = 30)
        {
            lock (__lock)
            {
                if (string.IsNullOrWhiteSpace(configPath))
                {
                    throw new ArgumentNullException("[configPath]填入正确的值");
                }
                if (string.IsNullOrWhiteSpace(connectionName))
                {
                    throw new ArgumentNullException("[connectionName]不可为空");
                }
                if (count < 1)
                {
                    throw new ArgumentException("[count]输入大于0的数");
                }
                ConnectinString = ConfigurationManager.ConnectionString(configPath, connectionName);
                if (string.IsNullOrWhiteSpace(ConnectinString))
                {
                    throw new ArgumentNullException("未读取到任务有效连接地址，检查[configPath][connectionName]是否正确");
                }
                _Connectin = new MySqlConnection(ConnectinString);
                try
                {
                    _Connectin.Open();
                    if (CommandStack == null)
                    {
                        CommandStack = new Stack<MySqlCommand>();
                    }
                    while (count > 0)
                    {
                        CommandStack.Push(_Connectin.CreateCommand());
                        --count;
                    }
                }
                catch (Exception e)
                {
                    throw e;
                }
            }
        }

        /// <summary>
        /// 初始化数据库连接池
        /// </summary>
        /// <param name="connectionStr">连接字符串</param>
        /// <param name="count">初始化数量，默认为30,最小为1，最大建议不超过50</param>

        public static void Init(string connectionStr, int count = 30)
        {
            if (string.IsNullOrWhiteSpace(connectionStr))
            {
                throw new ArgumentNullException("[connectionStr]请输入正确的值");
            }
            if (count < 1)
            {
                throw new ArgumentException("[count]输入大于0的数");
            }
            _Connectin = new MySqlConnection(connectionStr);
            try
            {
                _Connectin.Open();
                if (CommandStack == null)
                {
                    CommandStack = new Stack<MySqlCommand>();
                }
                while (count > 0)
                {
                    CommandStack.Push(_Connectin.CreateCommand());
                    --count;
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        #endregion 初始化

        #region 销毁

        /// <summary>
        /// 销毁初始化后的连接对象
        /// </summary>
        public static void Destroy()
        {
            while (CommandStack.Count > 0)
            {
                CommandStack.Pop().Dispose();
            }
            _Connectin.Dispose();
        }

        #endregion 销毁

        public static MySqlCommand Pop()
        {
            lock (__lock)
            {
                int i = 0;
                while (CommandStack.Count == 0)
                {
                    ++i;
                    if (i * 50 >= Timeout)
                    {
                        CommandStack.Push(_Connectin.CreateCommand());
                        break;
                    }
                    Thread.Sleep(50);
                }
                return CommandStack.Pop();
            }
        }

        public static void Push(MySqlCommand cmd)
        {
            if (cmd != null)
            {
                if (CommandStack.Count > 30)//当前队列大于30，直接抛弃
                {
                    cmd.Dispose();
                }
                else
                {
                    cmd.Parameters.Clear();
                    cmd.Transaction = null;
                    cmd.Cancel();
                    CommandStack.Push(cmd);
                }
            }
        }

        /// <summary>
        /// 释放所有连接
        /// </summary>
        ~PoolKernel()
        {
            Destroy();
        }
    }
}