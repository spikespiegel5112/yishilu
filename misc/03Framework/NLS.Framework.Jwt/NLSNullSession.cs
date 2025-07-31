using System;

namespace NLS.Framework.Jwt
{
    /// <summary>
    /// 定义会话信息
    /// </summary>
    public sealed class NLSNullSession : INLSNullSession
    {
        public long? UserId { get; set; }
    }
}