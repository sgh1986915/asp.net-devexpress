using System;
using System.Collections.Generic;
using System.Linq;
using System.Configuration;
using System.Data.SqlClient;

using System.Web.Security;

namespace APR.Web.UI.Portal.Account
{
    public partial class ResetPassword : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                try
                {
                    var requestID = Guid.Parse(Request.QueryString["RequestID"]);
                    var UserID = FetchUserForRequestID(requestID);

                    txtUserName.Text = UserID;
                }
                catch (ArgumentOutOfRangeException)
                {
                    lblStatus.Text = "Your password reset token has timed out. Please regenerate a password reset token.";
                    return;
                }
                catch (InvalidOperationException)
                {
                    lblStatus.Text = "You have already used this token to reset password. Please regenerate a password reset token.";
                    return;
                }
                catch
                {
                    lblStatus.Text = "Invalid password reset token. Please click on the URL received in mail or regenerate password reset token.";
                    return;
                }
            }
        }

        private string FetchUserForRequestID(Guid RequestID)
        {
            var query = "FetchUserForResetRequestID";
            using (var cn = new SqlConnection(ConfigurationManager.ConnectionStrings["ApplicationDB"].ConnectionString))
            {
                cn.Open();
                var cmd = new SqlCommand(query, cn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@RequestID", RequestID);

                var userName = cmd.ExecuteScalar().ToString();

                cn.Close();

                if (userName == "-1")
                {
                    throw new ArgumentOutOfRangeException();
                }
                else
                {
                    if (userName == "-2")
                    {
                        throw new InvalidOperationException();
                    }
                    else
                    {
                        if (String.IsNullOrWhiteSpace(userName))
                        {
                            throw new ArgumentNullException();
                        }
                    }
                }
                return userName;
            }
        }

        protected void ResetPasswordPushButton_Click(object sender, EventArgs e)
        {
            MarkRequestAsUsed(Request.QueryString["RequestID"]);

            FormsAuthentication.RedirectToLoginPage();
        }

        private void MarkRequestAsUsed(string RequestID)
        {
            var query = "MarkRequestTokenAsUsed";
            using (var cn = new SqlConnection(ConfigurationManager.ConnectionStrings["ApplicationDB"].ConnectionString))
            {
                cn.Open();
                var cmd = new SqlCommand(query, cn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@RequestID", RequestID);

                cmd.ExecuteScalar();

                cn.Close();
            }
        }
    }
}
