using Email.App.Contract.Utils;
using GrainInterfaces.Email;
using Infra.Core.Core.Guard;
using Microsoft.Extensions.Logging;
using Shared.App.ResultModel;
using Shared.App.Services;
using System.Net;

namespace Grains.Email
{
    public class EmailService
        (ILogger<EmailService> logger)
            : Grain, IEmailService
    {
        public async Task<AppSrvResult<HttpStatusCode>> RequestCodeAsync(string email)
        {
            try
            {
                Checker.Argument.IsIllegalEmail(email);

                string? code = await EmailUtil.SendAsync(email, "饥了么 账户安全代码") ?? throw new System.Exception("发送失败");
                VertifyEmailUtil.Add(email, code, DateTime.Now);

                return HttpStatusCode.OK;
            }
            catch (Exception ex)
            {
                return AbstractAppService.Problem(HttpStatusCode.InternalServerError, ex.Message + "----");
            }
        }

        public async Task<AppSrvResult<HttpStatusCode>> VertifyAsync(string email, string code)
        {
            try
            {
                await VertifyEmailUtil.VertifyAsync(email, code, DateTime.Now);

                return HttpStatusCode.OK;
            }
            catch (Exception ex)
            {
                return AbstractAppService.Problem(HttpStatusCode.InternalServerError, ex.Message + "----");
            }
        }

        private readonly ILogger _logger = logger;
    }
}
