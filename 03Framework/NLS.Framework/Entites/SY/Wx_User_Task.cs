using NLS.Framework.EF;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace NLS.Framework.Entites.SY
{
    [Table("wx_user_task")]
    public  class Wx_User_Task : AbsEntity
    {
        public int User_Id { get; set; }
        /// <summary>
        /// 1:眼  2：视  3：光
        /// </summary>
        public int Task_Type { get; set; }
        /// <summary>
        ///  1:正在进行   2：已完成
        /// </summary>
        public int Task_State { get; set; }
        public DateTime CreateTime { get; set; }
    }
}
