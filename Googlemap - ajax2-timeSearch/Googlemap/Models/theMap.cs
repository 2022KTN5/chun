//------------------------------------------------------------------------------
// <auto-generated>
//     這個程式碼是由範本產生。
//
//     對這個檔案進行手動變更可能導致您的應用程式產生未預期的行為。
//     如果重新產生程式碼，將會覆寫對這個檔案的手動變更。
// </auto-generated>
//------------------------------------------------------------------------------

namespace Googlemap.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public partial class theMap
    {
        public int Id { get; set; }
        [Display(Name = "地址")]
        public string address { get; set; }
        [Display(Name = "捐血站")]
        public string tele { get; set; }
        [Display(Name = "營業時間")]
        public string time { get; set; }
        public Nullable<double> lat { get; set; }
        public Nullable<double> lng { get; set; }
        [Display(Name = "期間限定")]
        public Nullable<System.DateTime> dateFrom { get; set; }
        [Display(Name = "        ")]
        public Nullable<System.DateTime> dateEnd { get; set; }
    }
}
