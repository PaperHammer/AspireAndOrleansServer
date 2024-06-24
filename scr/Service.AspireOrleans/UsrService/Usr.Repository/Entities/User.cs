using System.ComponentModel.DataAnnotations.Schema;

namespace Usr.Repository.Entities
{
    public class User
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        /// <summary>
        /// 账号
        /// </summary>
        public long Uid { get; set; }

        /// <summary>
        /// email
        /// </summary>
        public string Email { get; set; } = string.Empty;

        /// <summary>
        /// 姓名
        /// </summary>
        public string UserName { get; set; } = string.Empty;

        /// <summary>
        /// 密码
        /// </summary>
        public string Password { get; set; } = string.Empty;

        /// <summary>
        /// 账号权限/状态
        /// </summary>
        public int Status { get; set; }

        /// <summary>
        /// 是否被软删除
        /// </summary>
        public bool IsDelete { get; set; }
    }
}
