using Shared.App.Interfaces;
using Shared.App.ResultModel;
using System.Net;

namespace Email.GrainIntefaces
{
    [Alias("Email.GrainIntefaces.IEmailService")]
    public interface IEmailService : IAppService, IGrainWithStringKey
    {
        /// <summary>
        /// 获取安全代码
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        [Alias("Email_RequestCodeAsync")]
        Task<AppSrvResult<HttpStatusCode>> RequestCodeAsync(string email);

        /// <summary>
        /// 验证安全代码
        /// </summary>
        /// <param name="email"></param>
        /// <param name="code"></param>
        /// <returns></returns>
        [Alias("Email_VertifyAsync")]
        Task<AppSrvResult<HttpStatusCode>> VertifyAsync(string email, string code);
    }
}
