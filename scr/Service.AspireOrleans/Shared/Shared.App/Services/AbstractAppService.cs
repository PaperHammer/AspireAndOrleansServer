using Infra.Core.Core.Guard;
using Shared.App.Interfaces;
using Shared.App.ResultModel;
using System.Linq.Expressions;
using System.Net;

namespace Shared.App.Services
{
    public class AbstractAppService : IAppService
    {
        public static AppSrvResult AppSrvResult() => new();

        public static AppSrvResult<TValue> AppSrvResult<TValue>(TValue value)
        {
            Checker.Argument.IsNotNull(value, nameof(value));
            return new AppSrvResult<TValue>(value);
        }

        public static ProblemDetails Problem(HttpStatusCode? statusCode = null, string? detail = null, string? title = null, string? instance = null, string? type = null) 
            => new(statusCode, detail, title, instance, type);

        public static Expression<Func<TEntity, object>>[] UpdatingProps<TEntity>(params Expression<Func<TEntity, object>>[] expressions) => expressions;
    }
}
