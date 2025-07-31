using System;
using System.Collections.Generic;
using System.Text;

namespace NLS.ServiceCore.SY.SysUser.Dto
{
        [Serializable]
        public class UserMenuItem
        {
            public long id { get; set; }
            public string title { get; set; }
            public string icon { get; set; }
            public string name { get; set; }
            public long? parentid { get; set; }
            public int sequence { get; set; }
            public string path { get; set; }
            public bool ischeck { get; set; }
            public object meta { get; set; }
            public List<UserMenuItem> children { get; set; }
        }
}
