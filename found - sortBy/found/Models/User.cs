//------------------------------------------------------------------------------
// <auto-generated>
//     這個程式碼是由範本產生。
//
//     對這個檔案進行手動變更可能導致您的應用程式產生未預期的行為。
//     如果重新產生程式碼，將會覆寫對這個檔案的手動變更。
// </auto-generated>
//------------------------------------------------------------------------------

namespace found.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class User
    {
        public int uId { get; set; }
        public string account { get; set; }
        public string password { get; set; }
        public string name { get; set; }
        public Nullable<System.DateTime> created_at { get; set; }
        public Nullable<int> role { get; set; }
        public string phone { get; set; }
        public string address_city { get; set; }
        public string address_cou { get; set; }
        public string address_detail { get; set; }
        public string photo { get; set; }
    }
}
