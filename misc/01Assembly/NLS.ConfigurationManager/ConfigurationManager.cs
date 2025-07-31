using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;

namespace NLS.Configuration
{
    /// <summary>
    /// CongigurationManager
    /// </summary>
    public sealed class ConfigurationManager
    {
        private static IConfigurationRoot Configuration { get; set; }
        private static IConfigurationSection C_AppSetting { get; set; }
        private static string ConfigPath { get; set; } = "__Config/config.json";

        /// <summary>
        /// 获取ConnectionString
        /// </summary>
        /// <param name="KeyN">Key</param>
        /// <returns>ConnectionString</returns>
        public static string ConnectionString(string KeyN)
        {
            CheckDataAndInit(ConfigPath, KeyN);
            if (Configuration == null) { return string.Empty; }
            return Configuration.GetConnectionString(KeyN);
        }

        /// <summary>
        /// 获取ConnectionString
        /// </summary>
        /// <param name="ConfigPath">配置文件地址</param>
        /// <param name="KeyN">Key</param>
        /// <returns>ConnectionString</returns>
        public static string ConnectionString(string ConfigPath, string KeyN)
        {
            CheckDataAndInit(ConfigPath, KeyN);
            if (Configuration == null) { return string.Empty; }
            return Configuration.GetConnectionString(KeyN);
        }

        /// <summary>
        /// 获取AppSetting
        /// </summary>
        /// <param name="KeyN">Key</param>
        /// <returns>AppSetting Content</returns>
        public static string AppSetting(string KeyN)
        {
            CheckDataAndInit(ConfigPath, KeyN);
            if (C_AppSetting == null) { return string.Empty; }
            return C_AppSetting[KeyN];
        }

        /// <summary>
        /// 获取AppSetting
        /// </summary>
        /// <param name="ConfigPath">配置文件地址</param>
        /// <param name="KeyN">Key</param>
        /// <returns>AppSetting Content</returns>
        public static string AppSetting(string ConfigPath, string KeyN)
        {
            CheckDataAndInit(ConfigPath, KeyN);
            if (C_AppSetting == null) { return string.Empty; }
            return C_AppSetting[KeyN];
        }

        /// <summary>
        /// 获取Model
        /// </summary>
        public static T GetModel<T>(string KeyN) where T : class, new()
        {
            CheckDataAndInit(ConfigPath, KeyN);
            var Sub_Section = Configuration.GetSection(KeyN);
            var T_Ins = Activator.CreateInstance<T>();
            var T_Properties = T_Ins.GetType().GetProperties();
            for (int i = 0; i < T_Properties.Length; i++)
            {
                T_Properties[i].SetValue(T_Ins, Sub_Section[T_Properties[i].Name]);
            }
            return T_Ins;
        }

        /// <summary>
        /// 获取Model
        /// </summary>
        public static T GetModel<T>(string ConfigPath, string KeyN) where T : class, new()
        {
            CheckDataAndInit(ConfigPath, KeyN);
            var Sub_Section = Configuration.GetSection(KeyN);
            var T_Ins = Activator.CreateInstance<T>();
            var T_Properties = T_Ins.GetType().GetProperties();
            for (int i = 0; i < T_Properties.Length; i++)
            {
                T_Properties[i].SetValue(T_Ins, Sub_Section[T_Properties[i].Name]);
            }
            return T_Ins;
        }

        /// <summary>
        /// 获取ModelList
        /// </summary>
        public static List<T> GetModelList<T>(string KeyN) where T : class, new()
        {
            CheckDataAndInit(ConfigPath, KeyN);
            List<T> list = new List<T>();
            var Sub_Section = Configuration.GetSection(KeyN).GetChildren();
            foreach (var item in Sub_Section)
            {
                var T_Ins = Activator.CreateInstance<T>();
                var T_Properties = T_Ins.GetType().GetProperties();
                for (int i = 0; i < T_Properties.Length; i++)
                {
                    T_Properties[i].SetValue(T_Ins, item[T_Properties[i].Name]);
                }
                list.Add(T_Ins);
            }
            return list;
        }

        /// <summary>
        /// 获取ModelList
        /// </summary>
        public static List<T> GetModelList<T>(string ConfigPath, string KeyN) where T : class, new()
        {
            CheckDataAndInit(ConfigPath, KeyN);
            List<T> list = new List<T>();
            var Sub_Section = Configuration.GetSection(KeyN).GetChildren();
            foreach (var item in Sub_Section)
            {
                var T_Ins = Activator.CreateInstance<T>();
                var T_Properties = T_Ins.GetType().GetProperties();
                for (int i = 0; i < T_Properties.Length; i++)
                {
                    T_Properties[i].SetValue(T_Ins, item[T_Properties[i].Name]);
                }
                list.Add(T_Ins);
            }
            return list;
        }

        private static void CheckDataAndInit(string ConfigPath, string KeyN)
        {
            if (string.IsNullOrWhiteSpace(ConfigPath)) { throw new ArgumentNullException("ConfigPath不可为空"); }
            if (string.IsNullOrWhiteSpace(KeyN)) { throw new ArgumentNullException("KeyN不可为空"); }
            if (ConfigPath != ConfigurationManager.ConfigPath || Configuration == null)
            {
                ConfigurationManager.ConfigPath = ConfigPath;
                InitBuilder(ConfigPath);
            }
        }

        private static void InitBuilder(string ConfigPath)
        {
            try
            {
                var Builders = new ConfigurationBuilder()
                              .SetBasePath(AppContext.BaseDirectory)
                              .AddJsonFile(ConfigPath, optional: true, reloadOnChange: true);
                Configuration = Builders.Build();
                C_AppSetting = Configuration.GetSection("AppSettings");
            }
            catch { }
        }

        private static IConfigurationSection GetSection(string ConfigPath, string key)
        {
            var Builders = new ConfigurationBuilder()
                          .SetBasePath(AppContext.BaseDirectory)
                          .AddJsonFile(ConfigPath, optional: true, reloadOnChange: true);
            Configuration = Builders.Build();
            return Configuration.GetSection(key);
        }
    }
}