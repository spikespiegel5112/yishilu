using NLS.Framework.EF;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace NLS.Framework.Entites.SY
{
    /// <summary>
    /// 后台登录用户信息
    /// </summary>
    [Table("sys_user")]
    public sealed class User : AbsEntity
    {
        /// <summary>
        /// 姓名
        /// </summary>
        public string UserName { get; set; }


        /// <summary>
        /// 密码
        /// </summary>
        public string PassWord { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateTime { get; set; }

        /// <summary>
        /// 删除标记
        /// 0未删  1已删
        /// </summary>
        public byte F_Del { get; set; }

        /// <summary>
        /// 禁用标记
        /// 0未禁用  1禁用
        /// </summary>
        public byte F_Disable { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }
        [NotMapped]
        public object menulist { get; set; }

    }
}