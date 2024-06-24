using Email.App.Contract.Data;
using HtmlAgilityPack;
using MailKit.Net.Smtp;
using MimeKit;

namespace Email.App.Contract.Utils
{
    public static class EmailUtil
    {
        //static readonly string[] files = [
        //    "../Email.App.Contract/Html/VertifyCode.html",
        //];

        public static EmailConfig EmailConfig { get; set; } = new();

        //public static string SendEmailAddress = string.Empty;
        //public static string SMTPServer = string.Empty;
        //public static string SMTPPort = string.Empty;
        //public static string AuthorizationCode = string.Empty;

        public static async Task<string> SendAsync(string targetEmail, string content)
        {
            try
            {
                string code = "";

                await Task.Run(() =>
                {
                    MimeMessage mail = new();
                    mail.From.Add(new MailboxAddress("PaperHammer 账户管理", EmailConfig.ServeEmail));
                    mail.To.Add(new MailboxAddress(targetEmail.Split('@')[0], targetEmail));
                    mail.Subject = content;

                    //string htmlContent = File.ReadAllText(files[0]);
                    string htmlContent = new(_html);
                    HtmlDocument doc = new();
                    doc.LoadHtml(htmlContent);
                    HtmlNode bodyNode = doc.DocumentNode.SelectSingleNode("//body");

                    if (bodyNode != null)
                    {
                        string bodyContent = bodyNode.InnerHtml;

                        string placeholder = "[目标邮箱]";
                        string replacement = PrivacyShieldUtil.EmailShield(targetEmail);
                        string modifiedHtmlContent = htmlContent.Replace(placeholder, replacement);

                        code = RandomUtil.GetVerifyCode();
                        placeholder = "[验证码]";
                        replacement = code;
                        modifiedHtmlContent = modifiedHtmlContent.Replace(placeholder, replacement);

                        BodyBuilder bodyBuilder = new()
                        {
                            HtmlBody = modifiedHtmlContent,
                            TextBody = "饥了么 账户验证码"
                        };
                        mail.Body = bodyBuilder.ToMessageBody();
                    }

                    using (var client = new SmtpClient())
                    {
                        // 设置客户端的服务器证书验证回调函数
                        /*
                            在使用TLS/SSL协议进行通信时，客户端在与服务器建立安全连接时会验证服务器的证书以确保连接的安全性。
                            默认情况下，客户端会对服务器的证书进行验证，包括验证证书的有效性、过期时间、颁发机构等。
                            如果服务器的证书验证失败，客户端会拒绝建立连接。

                            然而，在某些情况下，您可能希望忽略证书验证，例如在开发和测试阶段或者与自签名证书的服务器进行通信时。
                            通过设置ServerCertificateValidationCallback属性为一个回调函数，
                            并将其返回值设为true，可以绕过服务器证书验证，即使服务器的证书存在问题也会被接受。

                            在这里，(s, c, h, e) => true 是一个匿名函数，它接收四个参数：
                            s 表示发送请求的对象，c 表示服务器提供的证书，h 表示所请求的主机名，e 表示验证错误（如果有）。
                            该匿名函数直接返回true，表示接受所有服务器证书，无论是否验证成功。
                        */
                        client.ServerCertificateValidationCallback = (s, c, h, e) => true;
                        //smtp服务器，端口，是否开启ssl
                        client.Connect(EmailConfig.SMTPServer, EmailConfig.SMTPPort, true);
                        client.Authenticate(EmailConfig.ServeEmail, EmailConfig.AuthorizationCode);
                        client.Send(mail);
                        client.Disconnect(true);
                    }
                });

                return code;
            }
            catch (Exception)
            {
                throw new("Email Service Runtime Error");
            }
        }

        private static string _html =
            """
            <!DOCTYPE html>
            <html lang="en">

            <head>
                <meta charset="UTF-8">
                <meta name="viewport" content="width=device-width, initial-scale=1.0">
                <title>Document</title>
            </head>

            <body>
                <table dir="ltr">
                    <tbody>
                        <tr>
                            <td id="i1" style="
                                            padding:0;
                                            font-family:'Segoe UI Semibold', 'Segoe UI Bold', 'Segoe UI', 'Helvetica Neue Medium', Arial, sans-serif;
                                            font-size:17px;
                                            color:#707070;">
                                饥了么 帐户
                            </td>
                        </tr>
                        <tr>
                            <td id="i2" style="
                                            padding:0;
                                            font-family:'Segoe UI Light', 'Segoe UI', 'Helvetica Neue Medium', Arial, sans-serif;
                                            font-size:41px; color:#2672ec;">
                                安全代码
                            </td>
                        </tr>
                        <tr>
                            <td id="i3"
                                style="
                                    padding:0;
                                    padding-top:25px;
                                    font-family:'Segoe UI', Tahoma, Verdana, Arial, sans-serif;
                                    font-size:14px;
                                    color:#2a2a2a;">

                                请为 饥了么 帐户
                                <span dir="ltr" id="iAccount" class="link" style="color:#2672ec; text-decoration:none">
                                    [目标邮箱]
                                </span>
                                使用以下安全代码。
                            </td>
                        </tr>
                        <tr>
                            <td id="i4"
                                style="
                                    padding:0;
                                    padding-top:25px;
                                    font-family:'Segoe UI', Tahoma, Verdana, Arial, sans-serif;
                                    font-size:14px;
                                    color:#2a2a2a;">

                                安全代码:
                                <span style="
                                        font-family:'Segoe UI Bold', 'Segoe UI Semibold', 'Segoe UI', 'Helvetica Neue Medium', Arial, sans-serif;
                                        font-size:14px;
                                        font-weight:bold;
                                        color:#2a2a2a;">
                                    [验证码]
                                </span>
                            </td>
                        </tr>
                        <tr>
                            <td id="i5"
                                style="
                                    padding:0;
                                    padding-top:25px;
                                    font-family:'Segoe UI', Tahoma, Verdana, Arial, sans-serif;
                                    font-size:14px;
                                    color:#2a2a2a;">
                                <span style="
                                        font-family:'Segoe UI Bold', 'Segoe UI Semibold', 'Segoe UI', 'Helvetica Neue Medium', Arial, sans-serif;
                                        font-size:14px;
                                        font-weight:bold;
                                        color:#2a2a2a;">
                                    注意：该代码约
                                    <span dir="ltr" id="iAccount" class="link" style="color:#2672ec; text-decoration:none">
                                        5分钟内
                                    </span>
                                    有效，请尽快完成验证。
                                </span>
                            </td>
                        </tr>
                        <tr>
                            <td id="i6"
                                style="
                                    padding:0;
                                    padding-top:25px;
                                    font-family:'Segoe UI', Tahoma, Verdana, Arial, sans-serif;
                                    font-size:14px;
                                    color:#2a2a2a;">

                                如果你无法识别 饥了么 帐户
                                <span dir="ltr" id="iAccount" class="link" style="color:#2672ec; text-decoration:none">
                                    [目标邮箱]
                                </span>
                                ，请忽略该邮件。
                            </td>
                        </tr>
                        <tr>
                            <td id="i7"
                                style="
                                    padding:0;
                                    padding-top:25px;
                                    font-family:'Segoe UI', Tahoma, Verdana, Arial, sans-serif;
                                    font-size:14px;
                                    color:#2a2a2a;">
                                谢谢!
                            </td>
                        </tr>
                        <tr>
                            <td id="i8"
                                style="
                                    padding:0;
                                    font-family:'Segoe UI', Tahoma, Verdana, Arial, sans-serif;
                                    font-size:14px;
                                    color:#2a2a2a;">
                                PaperHammer 帐户管理
                            </td>
                        </tr>
                    </tbody>
                </table>
            </body>

            </html>
            """;
    }
}
