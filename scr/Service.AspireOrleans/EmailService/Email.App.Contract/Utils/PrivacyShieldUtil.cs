namespace Email.App.Contract.Utils
{
    public class PrivacyShieldUtil
    {
        public static string EmailShield(string email)
        {
            string[] str = email.Split('@');

            string userName = str[0];
            string shieldRes = (str[0].Length > 2 ? userName[..2] : "") + "**" + userName[^1];

            return shieldRes + str[1];
        }
    }
}
