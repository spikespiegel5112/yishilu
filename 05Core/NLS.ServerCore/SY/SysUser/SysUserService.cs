using Microsoft.EntityFrameworkCore;
using NLS.AspNetCore.Controllers;
using NLS.AspNetCore.Linq;
using NLS.Framework;
using NLS.Framework.EF.Repository;
using NLS.Framework.Entites.SY;
using NLS.Framework.ExtentionCore;
using NLS.ServiceCore.SY.SysUser.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NLS.ServiceCore.SY.SysUser
{
    public class SysUserService : BaseService<NLSEntitesContext>, INLSServiceApplication, ISysUserService
    {
        private readonly IRepository<User> sysUserRepository;
        private readonly IRepository<SysMenu> sysMenuRepository;
        private readonly IRepository<WxUser> wxuserRepository;
        private readonly IRepository<Sys_Setting> sysettingRepository;
        private readonly ICommonUnits commonUnits;

        public SysUserService(IRepository<Sys_Setting> sysettingRepository, IRepository<User> sysUserRepository, IRepository<SysMenu> sysMenuRepository, IRepository<WxUser> wxuserRepository, ICommonUnits commonUnits)
        {
            this.sysUserRepository = sysUserRepository;
            this.sysMenuRepository = sysMenuRepository;
            this.wxuserRepository = wxuserRepository;
            this.sysettingRepository = sysettingRepository;
            this.commonUnits = commonUnits;
        }

        #region 后台用户登录
        public async Task<ResponseParamters<User>> PcUserLoginAsync(User model)
        {
            ResponseParamters<User> result = new ResponseParamters<User>();
            string pwd = commonUnits.GetMD5(model.PassWord);
            var userinfo = await sysUserRepository.GetAllAsNoTracking(w => w.UserName == model.UserName && w.PassWord == pwd).FirstOrDefaultAsync();
            if (userinfo != null)
            {
                userinfo.menulist = GetMenulist(await sysMenuRepository.GetAllAsNoTracking().ToListAsync());
                result.State = ResultStatusCode.Success;
                result.Data = userinfo;
            }
            else
            {
                result.State = ResultStatusCode.LoginFailed;
                result.Msg = "用户名或密码错误";
            }
            return result;
        }
        public List<UserMenuItem> GetMenulist(List<SysMenu> menulist)
        {
            List<UserMenuItem> returnlist = new List<UserMenuItem>();
            foreach (var item in menulist)
            {
                if (item.ParentId == 0)
                {
                    UserMenuItem menuitem = new UserMenuItem();
                    menuitem.children = new List<UserMenuItem>();
                    menuitem.id = item.Id;
                    menuitem.parentid = 0;
                    menuitem.title = item.MenuImg;
                    menuitem.icon = item.MenuImg;
                    menuitem.name = item.ActionUrl;
                    menuitem.path = item.ActionUrl;
                    menuitem.sequence = item.Sequence;
                    foreach (var item2 in menulist)
                    {
                        UserMenuItem menuitem2 = new UserMenuItem();
                        if (item2.ParentId == item.Id)
                        {
                            menuitem2.id = item2.Id;
                            menuitem2.title = item2.MenuName;
                            menuitem2.icon = item2.MenuImg;
                            menuitem2.parentid = item.Id;
                            menuitem2.path = item2.ActionUrl;
                            menuitem2.name = item2.ActionUrl;
                            menuitem2.sequence = item2.Sequence;
                            menuitem2.meta = new
                            {
                                icon = item2.MenuImg,
                                title = item2.MenuName
                            };
                            menuitem.children.Add(menuitem2);
                        }
                    }
                    menuitem.children = menuitem.children.OrderBy(w => w.sequence).ToList();
                    if (menuitem.children.Count <= 1)
                    {
                        menuitem.meta = new
                        {
                            hideInBread = true
                        };
                    }
                    else
                    {
                        menuitem.meta = new
                        {
                            icon = item.MenuImg,
                            title = item.MenuName
                        };
                    }
                    returnlist.Add(menuitem);
                }
            }
            returnlist = returnlist.OrderBy(w => w.sequence).ToList();
            return returnlist;
        }
        #endregion

        #region 微信用户列表
        public async Task<PageResult<WxUser>> GetWxUserPageList(UserSerchModel input) {
            var userlist = wxuserRepository.GetAllAsNoTracking();
            if (!string.IsNullOrEmpty(input.searchkey))
            {
                userlist = userlist.Where(w => w.NickName.Contains(input.searchkey) || w.Phone.Contains(input.searchkey) || w.Name.Contains(input.searchkey));
            }
            return await userlist.GetPageAsync(input);
        }

        public async Task<ResponseParamters<string>> AddOrUpdateWxUser(WxUser model)
        {
            ResponseParamters<string> responseParamters = new ResponseParamters<string>();
            try
            {
                if (model.Id == 0)
                {

                    await DBRepository.InsertAsync<WxUser>($"insert into  wx_user (Name,Phone,CreateTime) " +
                        $"values ('{model.Name}','{model.Phone}','{DateTime.Now}')");
                }
                else
                {
                    DBRepository.Update($"update wx_user set `Name`='{model.Name}',Phone='{model.Phone}' where Id={model.Id}");
                }
                responseParamters.State = ResultStatusCode.Success;
            }
            catch (System.Exception ex)
            {
                responseParamters.State = ResultStatusCode.SaveFailed;
                responseParamters.Msg = ex.Message;
            }
            return responseParamters;
        }

        public async Task<List<WxUser>> GetPrizeUserList(int F_Prize) {
            return await DBRepository.SearchListAsync<WxUser>($"select * from wx_user where F_Prize={F_Prize} and Phone is not null");
        }

        public ResponseParamters<string> UpdateUserPrizeState(string uids,int prizetype)
        {
            ResponseParamters<string> responseParamters = new ResponseParamters<string>();
            string Prize_Name = "";
            switch (prizetype)
            {
                case 0:
                    Prize_Name = "特别奖";
                    break;
                case 1:
                    Prize_Name = "一等奖：Macbook Pro";
                    break;
                case 2:
                    Prize_Name = "二等奖：华为手机";
                    break;
                case 3:
                    Prize_Name = "三等奖：旅游基金 / 筋膜枪 / 卷发棒";
                    break;
                case 4:
                    Prize_Name = "四等奖 ：SWITCH / 蓝牙耳机";
                    break;
                case 5:
                    Prize_Name = "五等奖";
                    break;
                default:
                    break;
            }
            int count= DBRepository.Update($"update wx_user set F_Prize=1 ,Prize_Time='{DateTime.Now}',Prize_Name='{Prize_Name}' where Id in ({uids})");
            if (count > 0) {
                responseParamters.State = ResultStatusCode.Success;
                responseParamters.Msg = "抽奖成功";
            }
            else {
                responseParamters.State = ResultStatusCode.SaveFailed;
                responseParamters.Msg = "抽奖失败";
            }

            return responseParamters;
        }

        public ResponseParamters<string> ClearUserPrizeState()
        {
            ResponseParamters<string> responseParamters = new ResponseParamters<string>();
            int count = DBRepository.Update($"update wx_user set F_Prize=0,Prize_Time=null,Prize_Name=null,Prize_State=1,Prize_Id=0 ");
            DBRepository.ExecuteSqlReturnRows("truncate table wx_user_prize");
            if (count > 0)
            {
                responseParamters.State = ResultStatusCode.Success;
                responseParamters.Msg = "清空成功";
            }
            else
            {
                responseParamters.State = ResultStatusCode.SaveFailed;
                responseParamters.Msg = "清空失败";
            }

            return responseParamters;
        }

        /// <summary>
        /// 中奖核销
        /// </summary>
        /// <param name="u_id"></param>
        /// <returns></returns>
        public ResponseParamters<string> ChangeUserPrizeState(int u_id)
        {
            ResponseParamters<string> responseParamters = new ResponseParamters<string>();
            int count = DBRepository.Update($"update wx_user set Prize_State=2 where  Id={u_id}");
            if (count > 0)
            {
                responseParamters.State = ResultStatusCode.Success;
                responseParamters.Msg = "核销成功";
            }
            else
            {
                responseParamters.State = ResultStatusCode.SaveFailed;
                responseParamters.Msg = "核销失败";
            }
            return responseParamters;
        }

        /// <summary>
        /// 增加扫码次数
        /// </summary>
        /// <param name="u_id"></param>
        /// <param name="scan_type"></param>
        /// <returns></returns>
        public ResponseParamters<string> PageAccess(int u_id,int scan_type) {
            ResponseParamters<string> responseParamters = new ResponseParamters<string>();
            try
            {
                string pa = "";
                switch (scan_type)
                {
                    case 0:
                        pa = "Home_ScanCount";
                        break;
                    case 1:
                        pa = "Eye_ScanCount";
                        break;
                    case 2:
                        pa = "Regard_ScanCount";
                        break;
                    case 3:
                        pa = "Light_ScanCount";
                        break;
                    default:
                        break;
                }
                int count = DBRepository.Update($"update wx_user set {pa}={pa}+1 where Id={u_id}");
                if (count > 0)
                {
                    responseParamters.State = ResultStatusCode.Success;
                    responseParamters.Msg = "次数更新成功";
                }
                else {
                    responseParamters.State = ResultStatusCode.SaveFailed;
                    responseParamters.Msg = "次数更新失败";
                }
            }
            catch (Exception ex)
            {
                responseParamters.State = ResultStatusCode.UnknowError;
                responseParamters.Data = ex.Message;
            }
            return responseParamters;
        }
        /// <summary>
        /// 获取用户信息
        /// </summary>
        /// <param name="u_id"></param>
        /// <returns></returns>
        public async Task<WxUser> GetUserInfo(int u_id)
        {
            var userinfo = await wxuserRepository.GetAllAsNoTracking(w => w.Id == u_id).FirstOrDefaultAsync();
            return userinfo;
        }
        #endregion

        #region 系统设置
        /// <summary>
        /// 更新系统设置
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<ResponseParamters<string>> AddOrUpdateSetting(Sys_Setting model)
        {
            ResponseParamters<string> responseParamters = new ResponseParamters<string>();
            try
            {
                if (model.Id == 0)
                {
                    await DBRepository.InsertAsync<Sys_Setting>($"insert into  sys_setting (Main_1,Main_2,Draw_Img,Selection_Img,Last_Img,Red_Envelopes,Main_3) " +
                        $"values ('{model.Main_1}','{model.Main_2}','{model.Draw_Img}','{model.Selection_Img}','{model.Last_Img}','{model.Red_Envelopes}','{model.Main_3}')");
                }
                else
                {
                    DBRepository.Update($"update sys_setting set Main_1='{model.Main_1}',Main_2='{model.Main_2}',Draw_Img='{model.Draw_Img}',Selection_Img='{model.Selection_Img}',Last_Img='{model.Last_Img}',Red_Envelopes='{model.Red_Envelopes}',Main_3='{model.Main_3}' where Id={model.Id}");
                }
                responseParamters.State = ResultStatusCode.Success;
            }
            catch (System.Exception ex)
            {
                responseParamters.State = ResultStatusCode.SaveFailed;
                responseParamters.Msg = ex.Message;
            }
            return responseParamters;
        }
        
        #endregion

    }
}
