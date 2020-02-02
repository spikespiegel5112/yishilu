using NLS.Framework.EF;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace NLS.Framework.Entites.SY
{
    [Table("wx_user_prize")]
    public class Wx_User_Prize : AbsEntity
    {
        public int User_Id { get; set; }
        public int Prize_Id { get; set; }
        public string Prize_Content { get; set; }
        public DateTime CreateTime { get; set; }
        public int ZhanTai_Id { get; set; }
    }
}
