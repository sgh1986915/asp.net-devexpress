using System;
using System.Collections.Generic;
using System.Linq;
using System.Configuration;
using System.Net.Mail;

using System.Data.SqlClient;

namespace APR.Web.UI.Portal.Account
{
    public partial class ForgotPassword : System.Web.UI.Page
    {
        public const int ResetPasswordLinkValidityInMins = 10;

        protected void Page_Load(object sender, EventArgs e)
        {
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            lblStatus.Text = string.Empty;

            var userName = GetUserNameForEmail(txtEmail.Text);
            if (String.IsNullOrWhiteSpace(userName))
            {
                lblStatus.Text = "Please enter a valid email id.";
                return;
            }

            CreateResetPasswordRequest(txtEmail.Text, userName);
        }

        private string GetUserNameForEmail(string Email)
        {
            var query = "GetUserNameByEmail";
            using (var cn = new SqlConnection(ConfigurationManager.ConnectionStrings["ApplicationDB"].ConnectionString))
            {
                cn.Open();
                var cmd = new SqlCommand(query, cn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Email", Email);

                var userName = cmd.ExecuteScalar().ToString();

                cn.Close();
                return userName;
            }
        }

        private void CreateResetPasswordRequest(string Email, string UserName)
        {
            var requestID = Guid.NewGuid().ToString();

            var query = "CreateResetPasswordRequest";
            using (var cn = new SqlConnection(ConfigurationManager.ConnectionStrings["ApplicationDB"].ConnectionString))
            {
                cn.Open();
                var cmd = new SqlCommand(query, cn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Email", Email);
                cmd.Parameters.AddWithValue("@RequestID", requestID);
                cmd.Parameters.AddWithValue("@UserName", UserName);
                cmd.Parameters.AddWithValue("@ExpiresAt", DateTime.Now.AddMinutes(ResetPasswordLinkValidityInMins));

                cmd.ExecuteNonQuery();

                cn.Close();
            }

            var passwordResetUrl = ConfigurationManager.AppSettings["PasswordResetUrl"].Replace("{#requestid#}", requestID);

            var objStreamReader = new System.IO.StreamReader(Server.MapPath("~/PasswordReset.html"));
            var htmlBody = objStreamReader.ReadToEnd();

            htmlBody = htmlBody.Replace("[#USER#]", UserName);
            htmlBody = htmlBody.Replace("[#PASSWORDRESETURL#]", passwordResetUrl);

            var objMailMessage = new MailMessage(ConfigurationManager.AppSettings["MailFromAddress"], Email,
                ConfigurationManager.AppSettings["MailSubject"], htmlBody);
            objMailMessage.IsBodyHtml = true;

            var objSmtpClient = new SmtpClient(ConfigurationManager.AppSettings["MailSmtpClient"]);
            if (ConfigurationManager.AppSettings["MailUserName"] == String.Empty)
            {
                objSmtpClient.UseDefaultCredentials = true;
            }
            else
            {
                objSmtpClient.Credentials = new System.Net.NetworkCredential(ConfigurationManager.AppSettings["MailUserName"], ConfigurationManager.AppSettings["MailPassword"]);
            }
            objSmtpClient.Send(objMailMessage);
        }
    }
}
