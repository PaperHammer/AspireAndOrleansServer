namespace Email.App.Contract.Data
{
    public class EmailConfig
    {
        public string ServeEmail { get; set; } = string.Empty;
        public string SMTPServer { get; set; } = string.Empty;
        public int SMTPPort { get; set; }
        public string AuthorizationCode { get; set; } = string.Empty;
    }
}
