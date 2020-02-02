using NLS.Framework.EF;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace NLS.Framework.Entites.SY
{
    [Table("prize")]
    public sealed class Prize : AbsEntity
    {
        public string Prize_Content { get; set; }
        public string Prize_Name { get; set; }
        public int Remaining_Number { get; set; }
        public int Probability { get; set; }
        /// <summary>
        /// 是否中奖  0：否 1:是
        /// </summary>
        [NotMapped]
        public string F_Draw { get; set; }
        [NotMapped]
        public int DrawCount { get; set; }
        public string Remark { get; set; }
        public int Order { get; set; }
        public string Unit { get; set; }
        public int Prize_Type { get; set; }
    }
}
