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
using Newtonsoft.Json;

namespace NLS.ServiceCore.SY.Task
{
    public class TaskService : BaseService<NLSEntitesContext>, INLSServiceApplication, ITaskService
    {
        private readonly IRepository<Wx_User_Task> usertaskRepository;
        private readonly IRepository<WxUser> wxuserRepository;
        private readonly IRepository<Prize> prizeRepository;
        private readonly IRepository<Wx_User_Prize> wxuserprizeRepository;
        public NLSEntitesContext context { get; set; }

        public TaskService(IRepository<Prize> prizeRepository, IRepository<WxUser> wxuserRepository, IRepository<Wx_User_Task> usertaskRepositorys, IRepository<Wx_User_Prize> wxuserprizeRepository)
        {
            this.usertaskRepository = usertaskRepositorys;
            this.wxuserRepository = wxuserRepository;
            this.prizeRepository = prizeRepository;
            this.wxuserprizeRepository = wxuserprizeRepository;
            context = new NLSEntitesContext();
        }

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
                    var othertaskinfo = await usertaskRepository.GetAllAsNoTracking(w => w.User_Id == usertask.User_Id && w.Task_Type != usertask.Task_Type && w.Task_State == 1).ToListAsync();
                    if (othertaskinfo.Count == 0)
                    {
                        usertask.CreateTime = DateTime.Now;
                        usertask.Task_State = 1;
                        int i = await DBRepository.InsertAsync<Wx_User_Task>($"insert into  wx_user_task (User_Id,Task_Type,Task_State,CreateTime) " +
                            $"values ('{usertask.User_Id}','{usertask.Task_Type}','{usertask.Task_State}','{usertask.CreateTime}')");
                        if (i > 0)
                        {
                            responseParamters.State = ResultStatusCode.Success;
                            responseParamters.Msg = "任务领取成功";
                        }
                        else
                        {
                            responseParamters.State = ResultStatusCode.SaveFailed;
                            responseParamters.Msg = "任务领取失败";
                        }
                    }
                    else
                    {
                        string taskname2 = "";
                        switch (othertaskinfo[0].Task_Type)
                        {
                            case 1:
                                taskname2 = "眼";
                                break;
                            case 2:
                                taskname2 = "视";
                                break;
                            case 3:
                                taskname2 = "光";
                                break;
                            default:
                                break;
                        }
                        responseParamters.State = ResultStatusCode.SaveFailed;
                        responseParamters.Msg = $"您有个{taskname2}的任务正在进行，请先完成！";
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
                        taskname = "\"眼\"";
                        break;
                    case 2:
                        taskname = "\"视\"";
                        break;
                    case 3:
                        taskname = "\"光\"";
                        break;
                    case 4:
                        taskname = "\"万新光学\"";
                        break;
                    case 5:
                        taskname = "\"优立光学\"";
                        break;
                    case 6:
                        taskname = "\"凯米\"";
                        break;
                    case 7:
                        taskname = "\"world\"";
                        break;
                    case 8:
                        taskname = "\"奥天光学\"";
                        break;
                    case 9:
                        taskname = "\"新天鸿光学\"";
                        break;
                    case 10:
                        taskname = "\"BOLON\"";
                        break;
                    default:
                        break;
                }
                var usertaskinfo= await usertaskRepository.GetAllAsNoTracking(p => p.User_Id == usertask.User_Id&&p.Task_Type==usertask.Task_Type).FirstOrDefaultAsync();
                if (usertaskinfo != null)
                {
                    if (usertaskinfo.Task_State == 2) {
                        responseParamters.State = ResultStatusCode.Success;
                        responseParamters.Msg = $"{taskname}您已点亮";
                        responseParamters.Data = usertask.Task_Type.ToString();
                    }
                }
                else {
                    int count= await DBRepository.InsertAsync<Wx_User_Task>($"insert  wx_user_task (User_Id,Task_Type,Task_State,CreateTime) values ('{ usertask.User_Id}',{ usertask.Task_Type},2,'{DateTime.Now}')");
                    if (count > 0)
                    {
                        responseParamters.State = ResultStatusCode.Success;
                        responseParamters.Msg = $@"恭喜您，{taskname}点亮成功";
                        responseParamters.Data = usertask.Task_Type.ToString();
                    }
                    else {
                        responseParamters.State = ResultStatusCode.SaveFailed;
                        responseParamters.Data = usertask.Task_Type.ToString();
                        responseParamters.Msg = $"抱歉，{taskname}点亮失败";
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

        public ResponseParamters<string> ClearUseTask()
        {
            ResponseParamters<string> responseParamters = new ResponseParamters<string>();
            DBRepository.ExecuteSqlReturnRows("truncate table wx_user_task");
            responseParamters.State = ResultStatusCode.Success;
            return responseParamters;
        }

        #region 获取用户任务是否点亮
        public async Task<LightenUpModel> GetLighten(int u_id) {
            LightenUpModel result = new LightenUpModel();
            var lightenlist =await usertaskRepository.GetAllAsNoTracking(w => w.User_Id == u_id&&w.Task_State==2).ToListAsync();
            var eye= lightenlist.Where(w => w.Task_Type == 1).FirstOrDefault();
            result.f_eye = eye != null ? true : false;

            var regard = lightenlist.Where(w => w.Task_Type == 2).FirstOrDefault();
            result.f_regard = regard != null ? true : false;

            var light = lightenlist.Where(w => w.Task_Type ==3).FirstOrDefault();
            result.f_light = light != null ? true : false;

            return result;
        }

        public async Task<LightenUpModel2> GetLighten2(int u_id)
        {
            LightenUpModel2 result = new LightenUpModel2();
            var lightenlist = await usertaskRepository.GetAllAsNoTracking(w => w.User_Id == u_id && w.Task_State == 2).ToListAsync();
            var eye = lightenlist.Where(w => w.Task_Type == 4).FirstOrDefault();
            result.f_four = eye != null ? true : false;

            var regard = lightenlist.Where(w => w.Task_Type == 5).FirstOrDefault();
            result.f_five = regard != null ? true : false;

            var light = lightenlist.Where(w => w.Task_Type ==6).FirstOrDefault();
            result.f_six = light != null ? true : false;

            var light1 = lightenlist.Where(w => w.Task_Type == 7).FirstOrDefault();
            result.f_seven = light1 != null ? true : false;

            var light2 = lightenlist.Where(w => w.Task_Type == 8).FirstOrDefault();
            result.f_eight = light2 != null ? true : false;

            var light3 = lightenlist.Where(w => w.Task_Type == 9).FirstOrDefault();
            result.f_nine = light3 != null ? true : false;

            var light4 = lightenlist.Where(w => w.Task_Type ==10).FirstOrDefault();
            result.f_ten = light4 != null ? true : false;

            return result;
        }

        #endregion

        #region 转盘抽奖

        public async Task<List<Prize>> GetPrizeList(int u_id) {
            try
            {
                var ent = await (from m in context.Prize
                                 join wxuser in context.WxUser.Where(w => w.Id == u_id) on m.Id equals wxuser.Prize_Id into JoinedEmpWxuser
                                 from wxuser in JoinedEmpWxuser.DefaultIfEmpty()
                                 where m.Prize_Type==1
                                 orderby m.Order ascending
                                 select new Prize
                                 {
                                     Id = m.Id,
                                     F_Draw= wxuser!=null?"true":"false",
                                     Prize_Name = m.Prize_Name,
                                     Probability = m.Probability,
                                     Remark=m.Remark,
                                     Prize_Content=m.Prize_Content,
                                     Remaining_Number = m.Remaining_Number,
                                     Unit=m.Unit,
                                     Order=m.Order
                                 }).ToListAsync();

                return ent;
            }
            catch (Exception )
            {
                return new List<Prize>();
            }
        }

        public async Task<List<Prize>> GetPrizeList2(int u_id)
        {
            try
            {
                var ent = await (from m in context.Prize
                                 where m.Prize_Type == 2
                                 orderby m.Order ascending
                                 select new Prize
                                 {
                                     Id = m.Id,
                                     Prize_Name = m.Prize_Name,
                                     Probability = m.Probability,
                                     Remark = m.Remark,
                                     Prize_Content = m.Prize_Content,
                                     Remaining_Number = m.Remaining_Number,
                                     Unit = m.Unit,
                                     Order = m.Order
                                 }).ToListAsync();

                return ent;
            }
            catch (Exception)
            {
                return new List<Prize>();
            }
        }


        public async Task<List<Wx_User_Prize>> GetDrawPrizeList(int u_id)
        {
            try
            {
                return await wxuserprizeRepository.GetAllAsNoTracking().Where(W => W.User_Id == u_id).ToListAsync(); ;
            }
            catch (Exception)
            {
                return new List<Wx_User_Prize>();
            }
        }

        public class PrizeModel {
            public long Prize_Id { get; set; }
            public string Prize_Name { get; set; }
            public string  ZhanTai { get; set; }
            public int ZhanTai_Id { get; set; }
        }

        public async Task<ResponseParamters<PrizeModel>> UserLuckDraw2(int u_id)
        {
            ResponseParamters<PrizeModel> responseParamters = new ResponseParamters<PrizeModel>();
            try
            {
                if (u_id != 0)
                {
                    try
                    {
                        string json = "[{ ZhanTai_Id:9,ZhanTai: '新天鸿4B62 - 4C73'},{ ZhanTai_Id:4,ZhanTai: '万新'},{ ZhanTai_Id:5,ZhanTai: '优立'},{ ZhanTai_Id:6,ZhanTai: '凯米'},{ ZhanTai_Id:7,ZhanTai: 'world'},{ ZhanTai_Id:8,ZhanTai: '奥天'},{ ZhanTai_Id:10,ZhanTai: 'BOLON'}]";


                        List<PrizeModel> array = JsonConvert.DeserializeObject<List<PrizeModel>>(json);
                        var userinfos = await wxuserRepository.GetAllAsNoTracking().Where(w => w.Id == u_id).FirstOrDefaultAsync();
                        if (userinfos != null)
                        {
                            var tasklist = await usertaskRepository.GetAllAsNoTracking().Where(w => w.User_Id == u_id && w.Task_Type > 3).ToListAsync();

                            var prizelist = await wxuserprizeRepository.GetAllAsNoTracking().Where(w => w.User_Id == u_id).ToListAsync();

                            if (tasklist.Count >= 2)
                            {
                                if ((tasklist.Count / 2 - prizelist.Count) > 0)
                                {
                                    var awardlist = await prizeRepository.GetAllAsNoTracking().Where(w => w.Remaining_Number > 0&&w.Prize_Type==2).ToListAsync();
                                    var ent = GetRandomAwards(awardlist);
                                    if (ent.Id != 0)
                                    {
                                        int count = 0;
                                        Random rnd = new Random();
                                        List<PrizeModel> newarray = array.OrderBy(i => rnd.NextDouble()).Take(1).ToList();
                                        if (ent.Id == 9 || (ent.Id == 10) || (ent.Id == 11) || (ent.Id == 12))
                                        {
                                            newarray[0].ZhanTai = "谢谢参与";
                                            newarray[0].ZhanTai_Id = 0;
                                        }
                                        string updatesql = $"insert wx_user_prize (User_Id,Prize_Id,Prize_Content,ZhanTai_Id,CreateTime) values ({u_id},{ent.Id},'{newarray[0].ZhanTai}',{newarray[0].ZhanTai_Id},now());";
                                        updatesql += $"update prize set Remaining_Number=Remaining_Number-1 where Id={ent.Id};";
                                        count = DBRepository.ExecuteSqlReturnRows(updatesql);
                                        if (count > 0)
                                        {
                                            PrizeModel nn = new PrizeModel();
                                            nn.Prize_Id = ent.Id;
                                            nn.Prize_Name = ent.Prize_Name;
                                            nn.ZhanTai = newarray[0].ZhanTai;
                                            nn.ZhanTai_Id= newarray[0].ZhanTai_Id;
                                            responseParamters.State = ResultStatusCode.Success;
                                            responseParamters.Data = nn;
                                        }
                                    }
                                    else
                                    {
                                        responseParamters.State = ResultStatusCode.SaveFailed;
                                        responseParamters.Msg = "奖品已被抽完";
                                    }
                                }
                                else {
                                    responseParamters.State = ResultStatusCode.SaveFailed;
                                    responseParamters.Msg = "您抽奖机会已用完";
                                }
                               
                            }
                            else {
                                responseParamters.State = ResultStatusCode.SaveFailed;
                                responseParamters.Msg = "您暂无抽奖机会，请前去完成点亮任务";
                            }
                        }

                    }
                    catch (Exception ex)
                    {
                        responseParamters.State = ResultStatusCode.UnknowError;
                        responseParamters.Msg = ex.Message;
                    }
                }
                else
                {
                    responseParamters.State = ResultStatusCode.RequestError;
                    responseParamters.Msg = "参数错误";
                }
            }
            catch (Exception ex)
            {
                responseParamters.State = ResultStatusCode.UnknowError;
                responseParamters.Msg = ex.Message;
            }
            return responseParamters;
        }

        public async Task<ResponseParamters<Prize>> UserLuckDraw(int u_id)
        {
            ResponseParamters<Prize> responseParamters = new ResponseParamters<Prize>();
            try
            {
                if (u_id != 0)
                {
                     try
                       {
                        var userinfos = await wxuserRepository.GetAllAsNoTracking().Where(w => w.Id == u_id).FirstOrDefaultAsync();
                        if (userinfos != null)
                        {
                            if (userinfos.F_Prize == 0)
                            {
                                var awardlist = await prizeRepository.GetAllAsNoTracking().Where(w => w.Remaining_Number > 0 && w.Prize_Type == 1).ToListAsync();
                                var ent = GetRandomAwards(awardlist);
                                if (ent.Id != 0)
                                {
                                    int count = 0;
                                    string updatesql = $"update wx_user set Prize_Name='{ent.Prize_Name}',Prize_Time='{ DateTime.Now}',F_Prize=1,Prize_Id={ (int)ent.Id} where Id={u_id};";
                                    updatesql += $"update prize set Remaining_Number=Remaining_Number-1 where Id={ent.Id};";
                                    count = DBRepository.ExecuteSqlReturnRows(updatesql);
                                    if (count > 0)
                                    {
                                        responseParamters.State = ResultStatusCode.Success;
                                        ent.DrawCount = 1;
                                        responseParamters.Data = ent;
                                    }
                                }
                                else
                                {
                                    responseParamters.State = ResultStatusCode.SaveFailed;
                                    responseParamters.Msg = "奖品已被抽完";
                                }
                            }
                            else
                            {
                                var prize = await prizeRepository.GetAllAsNoTracking().Where(w => w.Id == userinfos.Prize_Id).FirstOrDefaultAsync();
                                responseParamters.State = ResultStatusCode.Success;
                                prize.DrawCount =2;
                                responseParamters.Data = prize;
                            }
                        }

                            }
                            catch (Exception ex)
                            {
                                responseParamters.State = ResultStatusCode.UnknowError;
                                responseParamters.Msg = ex.Message;
                            }
                }
                else
                {
                    responseParamters.State = ResultStatusCode.RequestError;
                    responseParamters.Msg = "参数错误";
                }
            }
            catch (Exception ex)
            {
                responseParamters.State = ResultStatusCode.UnknowError;
                responseParamters.Msg = ex.Message;
            }
            return responseParamters;
        }

        public Prize GetRandomAwards(List<Prize> list)
        {
            var rand = new Random();
            int RateSum = Convert.ToInt32(list.Sum(p => p.Probability));

            int i = rand.Next(0, RateSum);
            double index = 0;
            double end = 0;
            Dictionary<double, double> rate = new Dictionary<double, double>();
            foreach (var item in list)
            {
                end += double.Parse(item.Probability.ToString());
                rate.Add(index, end);
                if (i >= index && i <= end)
                {

                    if (item.Remaining_Number <= 0)
                    {   //奖品已经没有数量

                        i = rand.Next(0, RateSum);
                        index = 0;
                        end = 0;
                        continue;
                    }

                    return item;
                }
                index = end;
            }
            return new Prize
            {
                Id = 0,
                Prize_Name = "0"
            };
        }
        #endregion
    }
}
