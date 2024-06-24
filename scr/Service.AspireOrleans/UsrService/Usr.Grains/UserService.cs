using Infra.Core.Core.Guard;
using Infra.Core.System.Extensions.ObjectExt;
using Infra.Helper.Encrypt;
using Infra.Helper.Enums;
using Infra.IdGenerater.Yitter;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Shared.App.ResultModel;
using Shared.App.Services;
using System.Net;
using System.Text.Json;
using Usr.App.Contract.Dtos;
using Usr.GrainInterfaces;
using Usr.Repository.Core;
using Usr.Repository.Entities;

namespace Usr.Grains
{
    public class UserService(ILogger<UserService> logger)
        : Grain, IUserService
    {
        static UserService()
        {
            _userDbContext = new UserDbContext();
        }

        public async Task<AppSrvResult<HttpStatusCode>> ChangeStatusAsync(long id, int status)
        {
            try
            {
                #region InputCheck 输入检测
                var user = await _userDbContext.Users.FirstOrDefaultAsync(u => u.Uid == id); // Detached
                if (user == null)
                    return AbstractAppService.Problem(HttpStatusCode.BadRequest, "账户不存在");

                if (status < 0 || status > 5)
                    return AbstractAppService.Problem(HttpStatusCode.BadRequest, "目标状态非法");
                #endregion

                user.Status = status;
                await _userDbContext.SaveChangesAsync();

                _logger.LogInformation(
                    $"\n ChangeStatus for a User successfully. id: {id} \n----");

                return AbstractAppService.AppSrvResult(HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                return AbstractAppService.Problem(HttpStatusCode.InternalServerError, ex.Message + "----");
            }
        }

        public async Task<AppSrvResult<long>> CreateAsync(UserCreationDto input)
        {
            try
            {
                input.TrimStringFields();

                #region InputCheck 输入检测
                if (await _userDbContext.Users.FirstOrDefaultAsync(u => u.Email == input.Email) != null)
                    return AbstractAppService.Problem(HttpStatusCode.BadRequest, "账户已经存在");

                Checker.Argument.IsNotEmpty(input.UserName, "UserName");
                Checker.Argument.IsNotEmpty(input.Password, "Password");
                Checker.Argument.IsEqual(input.Password, input.RePassword, "Password", "RePassword");
                #endregion

                string encryPwd = ReCodePwd(input.Password);

                User user = new()
                {
                    Uid = IdGenerater.GetNextId(),
                    Password = encryPwd,
                    Email = input.Email,
                    UserName = input.UserName,
                };

                await _userDbContext.AddAsync(user);
                await _userDbContext.SaveChangesAsync();

                _logger.LogInformation(
                    $"\n Create a User successfully. {JsonSerializer.Serialize(user)} \n----");

                return user.Uid;
            }
            catch (Exception ex)
            {
                return AbstractAppService.Problem(HttpStatusCode.InternalServerError, ex.Message + "----");
            }
        }

        public async Task<AppSrvResult<HttpStatusCode>> DeleteAsync(long id)
        {
            try
            {
                #region InputCheck 输入检测
                var user = await _userDbContext.Users.FirstOrDefaultAsync(u => u.Uid == id); // Detached
                if (user == null)
                    return AbstractAppService.Problem(HttpStatusCode.BadRequest, "账户不存在");
                #endregion

                user.IsDelete = true;
                await _userDbContext.SaveChangesAsync();

                _logger.LogInformation(
                   $"\n Delete for a User successfully. id: {id} \n----");

                return AbstractAppService.AppSrvResult(HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                return AbstractAppService.Problem(HttpStatusCode.InternalServerError, ex.Message + "----");
            }
        }

        public async Task<AppSrvResult<HttpStatusCode>> UpdateInfoAsync(long id, UserUpdationDto input)
        {
            try
            {
                input.TrimStringFields();

                #region InputCheck 输入检测
                Checker.Argument.IsNotEmpty(input.UserName, "UserName");
                Checker.Argument.IsNotEmpty(input.Email, "Email");

                var user = await _userDbContext.Users.FirstOrDefaultAsync(u => u.Uid == id); // Detached
                if (user == null)
                    return AbstractAppService.Problem(HttpStatusCode.BadRequest, "账户不存在");
                #endregion

                user.UserName = input.UserName;
                user.Email = input.Email;

                await _userDbContext.SaveChangesAsync();

                _logger.LogInformation(
                   $"\n Update for a User successfully. id: {id} \n----");

                return AbstractAppService.AppSrvResult(HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                return AbstractAppService.Problem(HttpStatusCode.InternalServerError, ex.Message + "----");
            }
        }

        public async Task<AppSrvResult<UserInfoDto>> GetUserInfoAsync(long id)
        {
            try
            {
                var user = await _userDbContext.Users.FirstOrDefaultAsync(u => u.Uid == id && !u.IsDelete);
                if (user == null)
                    return AbstractAppService.Problem(HttpStatusCode.BadRequest, "账户不存在");

                UserInfoDto userInfoDto = new()
                {
                    Uid = id,
                    UserName = user.UserName,
                    Email = user.Email,
                    Status = user.Status,
                };

                return userInfoDto;

            }
            catch (Exception ex)
            {
                return AbstractAppService.Problem(HttpStatusCode.InternalServerError, ex.Message + "----");
            }
        }

        public async Task<AppSrvResult<UserValidatedInfoDto>> LoginAsync(UserLoginDto input)
        {
            try
            {
                input.TrimStringFields();

                string encryPwd = ReCodePwd(input.Password);

                var user = await _userDbContext.Users.FirstOrDefaultAsync(
                    u => u.Email == input.Email && u.Password == encryPwd && !u.IsDelete);
                if (user == null)
                    return AbstractAppService.Problem(HttpStatusCode.BadRequest, "用户名或密码错误");

                UserValidatedInfoDto userValidatedInfoDto = new()
                {
                    Uid = user.Uid,
                    UserName = user.UserName,
                    Email = user.Email,
                    Status = user.Status,
                };

                _logger.LogInformation(
                   $"\n A User login successfully. id: {user.Uid} \n----");

                return userValidatedInfoDto;
            }
            catch (Exception ex)
            {
                return AbstractAppService.Problem(HttpStatusCode.InternalServerError, ex.Message + "----");
            }
        }

        public async Task<AppSrvResult<HttpStatusCode>> UpdatePasswordAsync(long id, UserChangePwdDto input)
        {
            try
            {
                input.TrimStringFields();
                Checker.Argument.IsEqual(input.Password, input.RePassword, "Password", "RePassword");

                string oldPwd = ReCodePwd(input.OldPassword);
                var user = await _userDbContext.Users.FirstOrDefaultAsync(
                    u => u.Password == oldPwd);
                if (user == null)
                    return AbstractAppService.Problem(HttpStatusCode.BadRequest, "原密码错误");

                user.Password = ReCodePwd(input.Password);

                await _userDbContext.SaveChangesAsync();

                _logger.LogInformation(
                   $"\n Update pwd for a User successfully. id: {id} \n----");

                return HttpStatusCode.OK;
            }
            catch (Exception ex)
            {
                return AbstractAppService.Problem(HttpStatusCode.InternalServerError, ex.Message + "----");
            }
        }

        public async Task<AppSrvResult<HttpStatusCode>> UpdateAccountRolesAsync(long id, RoleTypeEnum type)
        {
            try
            {
                var user = await _userDbContext.Users.FirstOrDefaultAsync(u => u.Uid == id);
                if (user == null)
                    return AbstractAppService.Problem(HttpStatusCode.BadRequest, "账户不存在");

                user.Status = (int)type;

                _logger.LogInformation(
                   $"\n Update role for a User successfully. id: {id} \n----");

                return HttpStatusCode.OK;

            }
            catch (Exception ex)
            {
                return AbstractAppService.Problem(HttpStatusCode.InternalServerError, ex.Message + "----");
            }
        }

        private static string ReCodePwd(string pwd)
        {
            try
            {
                string decryPwd = WebUtility.UrlDecode(pwd);
                decryPwd = AESFromUtil.EncryptString("string");
                string password = AESFromUtil.DecryptString(decryPwd);
                string encryPwd = AESToUtil.EncryptString(password);

                return encryPwd;
            }
            catch (Exception)
            {
                return string.Empty;
            }
        }

        private static readonly UserDbContext _userDbContext;
        //private readonly UserDbContext _userDbContext = userDbContext;
        private readonly ILogger _logger = logger;
    }
}
