using NLS.Framework.EF;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace NLS.Framework.Entites.SY
{
    [Table("sys_setting")]
    public class Sys_Setting : AbsEntity
    {
        public string Main_1 { get; set; }
        public string Main_2 { get; set; }
        public string Draw_Img { get; set; }
        public string Selection_Img { get; set; }
        public string Last_Img { get; set; }
        public int Red_Envelopes { get; set; }
        public string Main_3 { get; set; }

        /// <summary>
        /// 签到排序
        /// </summary>
        [NotMapped]
        public string sign_order { get; set; }
        /// <summary>
        /// 贡献值
        /// </summary>
        [NotMapped]
        public string Contribution_Value { get; set; }
        /// <summary>
        /// 超越值
        /// </summary>
        [NotMapped]
        public string Transcend_Value { get; set; }
        
    }
}
