using NLS.AspNetCore.Controllers;
using NLS.Framework.Entites.SY;
using NLS.ServiceCore.SY.Task.Dto;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace NLS.ServiceCore.SY.Task
{
    public interface ITaskService : INLSServiceApplication
    {
        Task<ResponseParamters<string>> ReceiveTask(Wx_User_Task usertask);

        Task<ResponseParamters<string>> UserLigntTask(Wx_User_Task usertask);

        Task<LightenUpModel> GetLighten(int u_id);
    }
}
