using Orleans;

namespace Shared.App.ResultModel
{
    /// <summary>
    /// Application返回结果包装类,无返回类型(void,task)
    /// </summary>
    [GenerateSerializer]
    public sealed class AppSrvResult
    {
        public AppSrvResult()
        {
        }

        public AppSrvResult(ProblemDetails problemDetails) => ProblemDetails = problemDetails;

        //[Id(0)]
        //public bool IsSuccess => ProblemDetails == null;
        private bool _isSuccessCache;
        [Id(0)]
        public bool IsSuccess
        {
            get => _isSuccessCache = ProblemDetails == null;
            set { }
        }

        [Id(1)]
        public ProblemDetails ProblemDetails { get; set; } = default!;

        public static implicit operator AppSrvResult(ProblemDetails problemDetails)
        {
            return new()
            {
                ProblemDetails = problemDetails
            };
        }
    }

    /// <summary>
    /// Application返回结果包装类,有返回类型
    /// </summary>
    [GenerateSerializer]
    public sealed class AppSrvResult<TValue>
    {
        public AppSrvResult()
        {
        }

        public AppSrvResult(TValue value) => Content = value;

        public AppSrvResult(ProblemDetails problemDetails) => ProblemDetails = problemDetails;

        //[Id(0)]
        //public bool IsSuccess => ProblemDetails == null && Content != null;
        private bool _isSuccessCache;
        [Id(0)]
        public bool IsSuccess
        {
            get => _isSuccessCache = ProblemDetails == null && Content != null;
            set { }
        }

        [Id(1)]
        public TValue Content { get; set; } = default!;

        [Id(2)]
        public ProblemDetails ProblemDetails { get; set; } = default!;

        public static implicit operator AppSrvResult<TValue>(AppSrvResult result)
        {
            return new()
            {
                ProblemDetails = result.ProblemDetails
            };
        }

        public static implicit operator AppSrvResult<TValue>(ProblemDetails problemDetails)
        {
            return new()
            {
                ProblemDetails = problemDetails
            };
        }

        public static implicit operator AppSrvResult<TValue>(TValue value) => new(value);
    }
}
