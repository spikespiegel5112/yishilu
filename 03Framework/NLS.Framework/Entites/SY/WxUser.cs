using NLS.Framework.EF;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace NLS.Framework.Entites.SY
{
    [Table("wx_user")]
    public sealed class WxUser : AbsEntity
    {
        public string NickName { get; set; }
        public string Avatar { get; set; }
        public string Openid { get; set; }
        public DateTime CreateTime { get; set; }
        public string Phone { get; set; }
        public string Name { get; set; }
        /// <summary>
        /// 0 未中奖  1：中奖
        /// </summary>
        public int F_Prize { get; set; }
        public DateTime ? Prize_Time { get; set; }
        public string Prize_Name { get; set; }
        /// <summary>
        /// 首页扫码次数
        /// </summary>
        public int Home_ScanCount { get; set; }
        /// <summary>
        /// 眼扫码次数
        /// </summary>
        public int Eye_ScanCount { get; set; }
        /// <summary>
        /// 视扫码次数
        /// </summary>
        public int Regard_ScanCount { get; set; }
        /// <summary>
        /// 光扫码次数
        /// </summary>
        public int Light_ScanCount { get; set; }
    }
}
