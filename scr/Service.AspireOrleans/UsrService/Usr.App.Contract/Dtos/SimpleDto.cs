using Orleans;
using Shared.App.Dtos.Base;

namespace Usr.App.Contract.Dtos
{
    /// <summary>
    /// 用于解决返回基本类型
    /// </summary>
    /// <typeparam name="T"></typeparam>
    [GenerateSerializer]
    public class SimpleDto<T> : IDto
        where T : notnull
    {
        public SimpleDto()
        {
        }

        public SimpleDto(T value)
        {
            Value = value;
        }

        /// <summary>
        /// 需要传递的值
        /// </summary>
        [Id(0)]
        public T Value { get; set; } = default!;
    }
}
