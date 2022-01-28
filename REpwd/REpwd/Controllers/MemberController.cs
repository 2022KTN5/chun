using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Mvc;
using static REpwd.Models.MemberModel;

namespace REpwd.Controllers
{
    public class MemberController : Controller
    {
        // GET: Member
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult ForgetPwd()
        {
            return View();
        }

		[ValidateAntiForgeryToken]
		public ActionResult SendMailToken(SendMailTokenIn inModel)
		{
			SendMailTokenOut outModel = new SendMailTokenOut();

			// 檢查輸入來源
			if (string.IsNullOrEmpty(inModel.UserID))
			{
				outModel.ErrMsg = "請輸入帳號";
				return Json(outModel);
			}

			// 檢查資料庫是否有這個帳號

			// 取得資料庫連線字串
			string connStr = System.Web.Configuration.WebConfigurationManager.ConnectionStrings["."].ConnectionString;

			// 當程式碼離開 using 區塊時，會自動關閉連接
			using (SqlConnection conn = new SqlConnection(connStr))
			{
				// 資料庫連線
				conn.Open();

				// 取得會員資料
				string sql = "select * from Charity_member1 where cID = @UserID";
				SqlCommand cmd = new SqlCommand();
				cmd.CommandText = sql;
				cmd.Connection = conn;

				// 使用參數化填值
				cmd.Parameters.AddWithValue("@UserID", inModel.UserID);

				// 執行資料庫查詢動作
				SqlDataAdapter adpt = new SqlDataAdapter();
				adpt.SelectCommand = cmd;
				DataSet ds = new DataSet();
				adpt.Fill(ds);
				DataTable dt = ds.Tables[0];

				if (dt.Rows.Count > 0)
				{
					// 取出會員信箱
					string UserEmail = dt.Rows[0]["c_email"].ToString();

					// 取得系統自定密鑰，在 Web.config 設定
					string SecretKey = ConfigurationManager.AppSettings["SecretKey"];

					// 產生帳號+時間驗證碼
					string sVerify = inModel.UserID + "|" + DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss");

					// 將驗證碼使用 3DES 加密
					TripleDESCryptoServiceProvider DES = new TripleDESCryptoServiceProvider();
					MD5 md5 = new MD5CryptoServiceProvider();
					byte[] buf = Encoding.UTF8.GetBytes(SecretKey);
					byte[] result = md5.ComputeHash(buf);
					string md5Key = BitConverter.ToString(result).Replace("-", "").ToLower().Substring(0, 24);
					DES.Key = UTF8Encoding.UTF8.GetBytes(md5Key);
					DES.Mode = CipherMode.ECB;
					ICryptoTransform DESEncrypt = DES.CreateEncryptor();
					byte[] Buffer = UTF8Encoding.UTF8.GetBytes(sVerify);
					sVerify = Convert.ToBase64String(DESEncrypt.TransformFinalBlock(Buffer, 0, Buffer.Length)); // 3DES 加密後驗證碼

					// 將加密後密碼使用網址編碼處理
					sVerify = HttpUtility.UrlEncode(sVerify);

					// 網站網址
					string webPath = Request.Url.Scheme + "://" + Request.Url.Authority + Url.Content("~/");

					// 從信件連結回到重設密碼頁面
					string receivePage = "Member/ResetPwd";

					// 信件內容範本
					string mailContent = "請點擊以下連結，返回網站重新設定密碼，逾期 30 分鐘後，此連結將會失效。<br><br>";
					mailContent = mailContent + "<a href='" + webPath + receivePage + "?verify=" + sVerify + "'  target='_blank'>點此連結</a>";

					// 信件主題
					string mailSubject = "[測試] 重設密碼申請信";

					// Google 發信帳號密碼
					string GoogleMailUserID = ConfigurationManager.AppSettings["GoogleMailUserID"];
					string GoogleMailUserPwd = ConfigurationManager.AppSettings["GoogleMailUserPwd"];

					// 使用 Google Mail Server 發信
					string SmtpServer = "smtp.gmail.com";
					int SmtpPort = 587;
					MailMessage mms = new MailMessage();
					mms.From = new MailAddress(GoogleMailUserID);
					mms.Subject = mailSubject;
					mms.Body = mailContent;
					mms.IsBodyHtml = true;
					mms.SubjectEncoding = Encoding.UTF8;
					mms.To.Add(new MailAddress(UserEmail));
					using (SmtpClient client = new SmtpClient(SmtpServer, SmtpPort))
					{
						client.EnableSsl = true;
						client.Credentials = new NetworkCredential("miniwei1011@gmail.com", "Passw0rd-iii");//寄信帳密 
						client.Send(mms); //寄出信件
					}
					outModel.ResultMsg = "請於 30 分鐘內至你的信箱點擊連結重新設定密碼，逾期將無效";
				}
				else
				{
					outModel.ErrMsg = "查無此帳號";
				}
			}

			// 回傳 Json 給前端
			return Json(outModel);
		}
	}
}