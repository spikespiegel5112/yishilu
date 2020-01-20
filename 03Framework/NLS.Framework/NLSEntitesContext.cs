using Microsoft.EntityFrameworkCore;
using NLS.Framework.EF;
using NLS.Framework.Entites.SY;

namespace NLS.Framework
{
    public sealed class NLSEntitesContext : AbsBaseContext
    {
        public NLSEntitesContext() : base("__Config/config.json", "main_db")
        {
        }

        public NLSEntitesContext(string csname)
            : base("__Config/config.json", csname)
        {
        }

        /// <summary>
        /// 系统用户
        /// </summary>
        public DbSet<User> Users { get; set; }
        /// <summary>
        /// 微信用户
        /// </summary>
        public DbSet<WxUser> WxUser { get; set; }
        /// <summary>
        /// 系统菜单
        /// </summary>
        public DbSet<SysMenu> SysMenu { get; set; }
        /// <summary>
        /// 系统设置
        /// </summary>
        public DbSet<Sys_Setting> Sys_Setting { get; set; }
        /// <summary>
        /// 用户任务
        /// </summary>
        public DbSet<Wx_User_Task> Wx_User_Task { get; set; }
        public DbSet<Prize> Prize { get; set; }
    }
}