using Shared.App.Attributes;
using Shared.App.Interfaces;
using Shared.App.ResultModel;
using System.Net;
using Usr.App.Contract.Dtos;
using Usr.App.Contract.Enums;

namespace GrainInterfaces.Usr
{
    [Alias("GrainInterfaces.Usr.IUsrService")]
    public interface IUsrService : IAppService, IGrainWithStringKey
    {
        /// <summary>
        /// 新增用户
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [OperateLog(LogName = "新增用户")]
        [Alias("CreateAsync")]
        Task<AppSrvResult<long>> CreateAsync(UserCreationDto input);

        /// <summary>
        /// 修改用户
        /// </summary>
        /// <param name="id"></param>
        /// <param name="input"></param>
        /// <returns></returns>
        [OperateLog(LogName = "修改用户")]
        [Alias("UpdateInfoAsync")]
        Task<AppSrvResult<HttpStatusCode>> UpdateInfoAsync(long id, UserUpdationDto input);

        /// <summary>
        /// 删除用户
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [OperateLog(LogName = "删除用户")]
        [Alias("DeleteAsync")]
        Task<AppSrvResult<HttpStatusCode>> DeleteAsync(long id);

        /// <summary>
        /// 修改用户状态
        /// </summary>
        /// <param name="id"></param>
        /// <param name="status"></param>
        /// <returns></returns>
        [OperateLog(LogName = "修改用户状态")]
        [Alias("ChangeStatusAsync")]
        Task<AppSrvResult<HttpStatusCode>> ChangeStatusAsync(long id, int status);

        /// <summary>
        /// 获取用户信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Alias("GetUserInfoAsync")]
        Task<AppSrvResult<UserInfoDto>> GetUserInfoAsync(long id);

        /// <summary>
        /// 登录
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [OperateLog(LogName = "登录")]
        [Alias("LoginAsync")]
        Task<AppSrvResult<UserValidatedInfoDto>> LoginAsync(UserLoginDto input);

        /// <summary>
        /// 修改密码
        /// </summary>
        /// <param name="id"></param>
        /// <param name="input"></param>
        /// <returns></returns>
        [OperateLog(LogName = "修改密码")]
        [Alias("UpdatePasswordAsync")]
        Task<AppSrvResult<HttpStatusCode>> UpdatePasswordAsync(long id, UserChangePwdDto input);

        /// <summary>
        /// 修改身份
        /// </summary>
        /// <param name="id"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        [OperateLog(LogName = "修改身份")]
        [Alias("UpdateAccountRolesAsync")]
        Task<AppSrvResult<HttpStatusCode>> UpdateAccountRolesAsync(long id, RolesEnum type);
    }
}
