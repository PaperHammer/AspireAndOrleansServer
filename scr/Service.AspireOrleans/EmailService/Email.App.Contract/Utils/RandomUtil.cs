using System.Text;

namespace Email.App.Contract.Utils
{
    public class RandomUtil
    {
        private static readonly string _chars = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ";
        private static readonly Random random = new();

        public static string GetVerifyCode(int len = 6)
        {
            StringBuilder sb = new();
            for (int i = 0; i < len; i++)
            {
                int idx = random.Next(0, _chars.Length);
                sb.Append(_chars[idx]);
            }

            return sb.ToString();
        }
    }
}
