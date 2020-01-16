namespace NLS.Framework.Jwt
{
    /// <summary>
    /// 定义会话信息
    /// </summary>
    public interface INLSNullSession
    {
        long? UserId { get; set; }
    }
}