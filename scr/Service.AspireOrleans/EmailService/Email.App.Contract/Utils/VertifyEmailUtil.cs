using System.Collections.Concurrent;

namespace Email.App.Contract.Utils
{
    public class VertifyEmailUtil
    {
        // 线程安全的字典
        private static ConcurrentDictionary<string, (string, DateTime)> waitForvertify;

        static VertifyEmailUtil()
        {
            waitForvertify = new();
        }

        public static void Add(string email, string code, DateTime start)
        {
            waitForvertify[email] = (code, start);
        }

        public static async Task VertifyAsync(string email, string code, DateTime response)
        {
            try
            {
                bool res = await Task.Run(() =>
                {
                    waitForvertify.TryGetValue(email, out var tpl);
                    if (tpl.Item1 != code) throw new("验证码错误");

                    DateTime start = tpl.Item2;
                    double span = (response - start).TotalMinutes;
                    bool res = span <= 5;

                    return res;
                });
                
                if (!res) throw new("验证码已失效");
            }
            catch (Exception)
            {
                throw;
            }            
        }
    }
}
