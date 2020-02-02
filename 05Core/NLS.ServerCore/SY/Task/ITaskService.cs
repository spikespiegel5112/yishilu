using NLS.AspNetCore.Controllers;
using NLS.Framework.Entites.SY;
using NLS.ServiceCore.SY.Task.Dto;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using static NLS.ServiceCore.SY.Task.TaskService;

namespace NLS.ServiceCore.SY.Task
{
    public interface ITaskService : INLSServiceApplication
    {
        Task<ResponseParamters<string>> ReceiveTask(Wx_User_Task usertask);

        Task<ResponseParamters<string>> UserLigntTask(Wx_User_Task usertask);

        Task<LightenUpModel> GetLighten(int u_id);

        Task<ResponseParamters<Prize>> UserLuckDraw(int u_id);
        Task<ResponseParamters<PrizeModel>> UserLuckDraw2(int u_id);
        Task<List<Wx_User_Prize>> GetDrawPrizeList(int u_id);

        Task<List<Prize>> GetPrizeList(int u_id);

        Task<List<Prize>> GetPrizeList2(int u_id);


        Task<LightenUpModel2> GetLighten2(int u_id);
        ResponseParamters<string> ClearUseTask();

    }
}
