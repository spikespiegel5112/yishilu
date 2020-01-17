using Microsoft.EntityFrameworkCore;
using NLS.AspNetCore.Controllers;
using NLS.Framework;
using NLS.Framework.EF.Repository;
using NLS.Framework.Entites.SY;
using NLS.ServiceCore.SY.Task.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NLS.ServiceCore.SY.Task
{
    public class TaskService : BaseService<NLSEntitesContext>, INLSServiceApplication, ITaskService
    {
        private readonly IRepository<Wx_User_Task> usertaskRepository;

        #region 领取任务
        public async Task<ResponseParamters<string>> ReceiveTask(Wx_User_Task usertask)
        {
            ResponseParamters<string> responseParamters = new ResponseParamters<string>();
            try
            {
                var task = await DBRepository.SearchFirstOrDefaultAsync<Wx_User_Task>($"select Id from wx_user_task where User_Id={usertask.User_Id} and Task_Type={usertask.Task_Type}");
                if (task != null) {
                    responseParamters.State = ResultStatusCode.ValueExists;
                    responseParamters.Msg = "该任务已经领取";
                }
                else
                {
                    usertask.CreateTime = DateTime.Now;
                    usertask.Task_State = 1;
                    int i= await DBRepository.InsertAsync<Wx_User_Task>($"insert into  wx_user_task (User_Id,Task_Type,Task_State,CreateTime) " +
                        $"values ('{usertask.User_Id}','{usertask.Task_Type}','{usertask.Task_State}','{usertask.CreateTime}')");
                    if (i > 0)
                    {
                        responseParamters.State = ResultStatusCode.Success;
                        responseParamters.Msg = "任务领取成功";
                    }
                    else {
                        responseParamters.State = ResultStatusCode.SaveFailed;
                        responseParamters.Msg = "任务领取失败";
                    }
                }
            }
            catch (Exception ex)
            {
                responseParamters.State = ResultStatusCode.UnknowError;
                responseParamters.Data = ex.Message;
            }

            return responseParamters;
        }
        #endregion

        #region 用户点亮任务
        public async Task<ResponseParamters<string>> UserLigntTask(Wx_User_Task usertask)
        {
            ResponseParamters<string> responseParamters = new ResponseParamters<string>();
            try
            {
                string taskname = "";
                switch (usertask.Task_Type)
                {
                    case 1:
                        taskname = "眼";
                        break;
                    case 2:
                        taskname = "视";
                        break;
                    case 3:
                        taskname = "光";
                        break;
                    default:
                        break;
                }
                var usertaskinfo= await usertaskRepository.GetAllAsNoTracking(p => p.User_Id == usertask.User_Id&&p.Task_Type==usertask.Task_Type).FirstOrDefaultAsync();
                if (usertaskinfo != null)
                {
                    if (usertaskinfo.Task_State == 2) {
                        responseParamters.State = ResultStatusCode.SaveFailed;
                        responseParamters.Msg = $"{taskname}的任务您已完成";
                    }
                    else
                    {
                        var othertaskinfo = await usertaskRepository.GetAllAsNoTracking(w => w.User_Id == usertask.User_Id && w.Task_Type != usertask.Task_Type && w.Task_State == 1).ToListAsync();
                        if (othertaskinfo.Count == 0)
                        {
                            DBRepository.Update($"update wx_user_task set Task_State=2 where Id={usertaskinfo.Id}");
                            responseParamters.State = ResultStatusCode.Success;
                            responseParamters.Msg = $"恭喜您，任务点亮成功";
                        }
                        else {
                            string taskname2 = "";
                              switch (othertaskinfo[0].Task_Type)
                                {
                                    case 1:
                                        taskname = "眼";
                                        break;
                                    case 2:
                                        taskname = "视";
                                        break;
                                    case 3:
                                        taskname = "光";
                                        break;
                                    default:
                                        break;
                                }
                                responseParamters.State = ResultStatusCode.SaveFailed;
                                responseParamters.Msg = $"您有个{taskname2}的任务正在进行，请先完成！";
                        }
                    }
                }
                else {
                    responseParamters.State = ResultStatusCode.SaveFailed;
                    responseParamters.Msg = $"您还未领取{taskname}任务";
                }
            }
            catch (Exception ex)
            {
                responseParamters.State = ResultStatusCode.UnknowError;
                responseParamters.Data = ex.Message;
            }

            return responseParamters;
        }
         #endregion

        #region 获取用户任务是否点亮
            public async Task<LightenUpModel> GetLighten(int u_id) {
            LightenUpModel result = new LightenUpModel();
            var lightenlist =await usertaskRepository.GetAllAsNoTracking(w => w.User_Id == u_id).ToListAsync();
            var eye= lightenlist.Where(w => w.Task_Type == 1).FirstOrDefault();
            result.f_eye = eye != null ? true : false;

            var regard = lightenlist.Where(w => w.Task_Type == 2).FirstOrDefault();
            result.f_regard = regard != null ? true : false;

            var light = lightenlist.Where(w => w.Task_Type ==3).FirstOrDefault();
            result.f_light = light != null ? true : false;

            return result;
        }
        #endregion

    }
}
