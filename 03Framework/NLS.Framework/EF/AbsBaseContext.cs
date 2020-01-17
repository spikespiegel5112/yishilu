using Microsoft.EntityFrameworkCore;
using NLS.Configuration;

namespace NLS.Framework.EF
{
    public abstract class AbsBaseContext : DbContext
    {
        /// <summary>
        /// 连接字符串名称
        /// </summary>
        private string ConnectionName { get; set; }

        /// <summary>
        /// config地址
        /// </summary>
        private string ConfigPath { get; set; }

        public string ConnectionStr { get; set; }

        public AbsBaseContext(string configpath, string connectionname) : base()
        {
            ConnectionName = connectionname;
            ConfigPath = configpath;
            ConnectionStr = ConfigurationManager.ConnectionString(ConfigPath, ConnectionName);
            //base.Database.EnsureCreated();//不存在则创建-吕
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            ConnectionStr = ConfigurationManager.ConnectionString(ConfigPath ?? "__Config/config.json", ConnectionName ?? "main_db");
            optionsBuilder.UseMySQL(ConnectionStr);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
            => base.OnModelCreating(modelBuilder);
    }
}