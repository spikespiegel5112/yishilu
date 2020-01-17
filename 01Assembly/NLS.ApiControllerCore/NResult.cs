using Microsoft.AspNetCore.Mvc;

namespace NLS.ApiControllerCore
{
    public class NResult : JsonResult
    {
        public NResult() : base("")
        {
        }

        public override void ExecuteResult(ActionContext context)
        {
            base.ExecuteResult(context);
        }
    }
}