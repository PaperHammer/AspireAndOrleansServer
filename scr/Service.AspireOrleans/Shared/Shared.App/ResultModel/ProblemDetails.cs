using Infra.Core.Core.Json;
using Orleans;
using System.Net;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Shared.App.ResultModel
{
    /// <summary>
    /// 错误信息类
    /// </summary>
    [GenerateSerializer]
    public sealed class ProblemDetails
    {
        public ProblemDetails()
        { }

        public ProblemDetails(HttpStatusCode? statusCode, string? detail = null, string? title = null, string? instance = null, string? type = null)
        {
            var status = statusCode.HasValue ? (int)statusCode.Value : (int)HttpStatusCode.BadRequest;
            Status = status;
            Title = title ?? "参数错误";
            Detail = detail ?? string.Empty;
            Instance = instance ?? string.Empty;
            Type = type ?? string.Concat("https://httpstatuses.com/", status);
        }

        public override string ToString() => JsonSerializer.Serialize(this, SystemTextJson.GetAdncDefaultOptions());

        [Id(0)]
        [JsonPropertyName("detail")]
        public string Detail { get; set; } = string.Empty;

        [Id(1)]
        [JsonExtensionData]
        public IDictionary<string, object> Extensions { get; } = new Dictionary<string, object>();

        [Id(2)]
        [JsonPropertyName("instance")]
        public string Instance { get; set; } = string.Empty;

        [Id(3)]
        [JsonPropertyName("status")]
        public int Status { get; set; }

        [Id(4)]
        [JsonPropertyName("title")]
        public string Title { get; set; } = string.Empty;

        [Id(5)]
        [JsonPropertyName("type")]
        public string Type { get; set; } = string.Empty;
    }
}
