using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Filters;
using NLS.AspNetCore.Controllers;
using System.Linq;

namespace NLS.AspNetCore.Authorize
{
    /// <summary>
    /// Jwt验证
    /// 加在需要验证的方法或控制器上
    /// </summary>
    public class NLSAuthorizeAttribute : AuthorizeAttribute, IAuthorizationFilter
    {
        public NLSAuthorizeAttribute()
        {
        }

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var _claims = context.HttpContext.User?.Claims.FirstOrDefault(p => p.Type == "UserId");
            if (_claims != null)
            {
                if (string.IsNullOrWhiteSpace(_claims.Value))
                {
                    return;
                }
                if (int.TryParse(_claims.Value, out int result))
                {
                    NLSBaseController.NLSSession.UserId = result;
                    // __session.UserId = result;
                }
            }
        }
    }
}