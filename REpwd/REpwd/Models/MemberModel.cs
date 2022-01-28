using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace REpwd.Models
{
    public class MemberModel
    {
        public class SendMailTokenIn
        {
            public string UserID { get; set; }
        }

        /// <summary>
        /// [寄送驗證碼]回傳
        /// </summary>
        public class SendMailTokenOut
        {
            public string ErrMsg { get; set; }
            public string ResultMsg { get; set; }
        }
    }
}