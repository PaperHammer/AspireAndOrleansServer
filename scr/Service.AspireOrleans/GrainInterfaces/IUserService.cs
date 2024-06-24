using Infra.Helper.Enums;
using Shared.App.Attributes;
using Shared.App.Interfaces;
using Shared.App.ResultModel;
using System.Net;
using Usr.App.Contract.Dtos;

namespace GrainInterfaces
{
    [Alias("GrainInterfaces.IUserService")]
    public interface IUserService : IAppService, IGrainWithStringKey
    {
        /// <summary>
        /// 新增用户
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [OperateLog(LogName = "新增用户")]
        [Alias("User_CreateAsync")]
        Task<AppSrvResult<long>> CreateAsync(UserCreationDto input);

        /// <summary>
        /// 修改用户
        /// </summary>
        /// <param name="id"></param>
        /// <param name="input"></param>
        /// <returns></returns>
        [OperateLog(LogName = "修改用户")]
        [Alias("User_UpdateInfoAsync")]
        Task<AppSrvResult<HttpStatusCode>> UpdateInfoAsync(long id, UserUpdationDto input);

        /// <summary>
        /// 删除用户
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [OperateLog(LogName = "删除用户")]
        [Alias("User_DeleteAsync")]
        Task<AppSrvResult<HttpStatusCode>> DeleteAsync(long id);

        /// <summary>
        /// 修改用户状态
        /// </summary>
        /// <param name="id"></param>
        /// <param name="status"></param>
        /// <returns></returns>
        [OperateLog(LogName = "修改用户状态")]
        [Alias("User_ChangeStatusAsync")]
        Task<AppSrvResult<HttpStatusCode>> ChangeStatusAsync(long id, int status);

        /// <summary>
        /// 获取用户信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Alias("User_GetUserInfoAsync")]
        Task<AppSrvResult<UserInfoDto>> GetUserInfoAsync(long id);

        /// <summary>
        /// 登录
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [OperateLog(LogName = "登录")]
        [Alias("User_LoginAsync")]
        Task<AppSrvResult<UserValidatedInfoDto>> LoginAsync(UserLoginDto input);

        /// <summary>
        /// 修改密码
        /// </summary>
        /// <param name="id"></param>
        /// <param name="input"></param>
        /// <returns></returns>
        [OperateLog(LogName = "修改密码")]
        [Alias("User_UpdatePasswordAsync")]
        Task<AppSrvResult<HttpStatusCode>> UpdatePasswordAsync(long id, UserChangePwdDto input);

        /// <summary>
        /// 修改身份
        /// </summary>
        /// <param name="id"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        [OperateLog(LogName = "修改身份")]
        [Alias("User_UpdateAccountRolesAsync")]
        Task<AppSrvResult<HttpStatusCode>> UpdateAccountRolesAsync(long id, RoleTypeEnum type);
    }
}
