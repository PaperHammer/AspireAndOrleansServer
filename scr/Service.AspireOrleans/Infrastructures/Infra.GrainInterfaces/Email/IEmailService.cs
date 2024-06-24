using Shared.App.Interfaces;
using Shared.App.ResultModel;
using System.Net;

namespace Infra.GrainInterfaces.Email
{
    [Alias("Infra.GrainInterfaces.Email.IEmailService")]
    public interface IEmailService : IAppService, IGrainWithStringKey
    {
        /// <summary>
        /// 获取安全代码
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        Task<AppSrvResult<HttpStatusCode>> RequestCodeAsync(string email);

        /// <summary>
        /// 验证安全代码
        /// </summary>
        /// <param name="email"></param>
        /// <param name="code"></param>
        /// <returns></returns>
        Task<AppSrvResult<HttpStatusCode>> VertifyAsync(string email, string code);
    }
}
