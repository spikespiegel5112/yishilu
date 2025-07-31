using NLS.Framework.EF;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace NLS.Framework.Entites.SY
{
    [Table("sys_menu")]
    public sealed class SysMenu : AbsEntity
    {
        public string MenuName { get; set; }
        public string MenuImg { get; set; }
        public int ParentId { get; set; }
        public int Sequence { get; set; }
        public string Redirect { get; set; }
        public string ActionUrl { get; set; }
    }
}
