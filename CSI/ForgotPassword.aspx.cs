using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;

namespace CSI
{
    public partial class ForgotPassword : System.Web.UI.Page
    {
        public bool IsPageRefresh = false;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {

                ViewState["ViewStateId"] = System.Guid.NewGuid().ToString();
                Session["SessionId"] = ViewState["ViewStateId"].ToString();

            }
            else
            {
                if (ViewState["ViewStateId"].ToString() != Session["SessionId"].ToString())
                {
                    IsPageRefresh = true;
                }
                Session["SessionId"] = System.Guid.NewGuid().ToString();
                ViewState["ViewStateId"] = Session["SessionId"].ToString();
            }
        }

        protected void btnUpdate_Click(object sender, EventArgs e)
        {

            int num = Convert.ToInt32(inpEmpNo.Value);
            string sNum = num.ToString("00000");
            inpEmpNo.Value = sNum.ToString();


            using (SqlConnection conN = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["MyConnCSI"].ConnectionString))
            {
                string stR = @"dbo.ForgotPassword";
                using (SqlCommand cmD = new SqlCommand(stR, conN))
                {
                    conN.Open();
                    cmD.CommandType = CommandType.StoredProcedure;
                    cmD.CommandTimeout = 0;
                    cmD.Parameters.AddWithValue("@vUser_EmpNo", inpEmpNo.Value);
                    cmD.Parameters.AddWithValue("@vUser_Pass", MyClass.Encrypt(inpPassword.Value, true));
                    cmD.Parameters.AddWithValue("@ResultValue", SqlDbType.Int).Direction = ParameterDirection.Output;
                    cmD.ExecuteNonQuery();
                    int Result = Convert.ToInt32(cmD.Parameters["@ResultValue"].Value);

                    if (Result == 99)
                    {
                        lblMsg.Text = "Employee number does not exists!";
                        ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "Popup", "ShowSuccessMsg();", true);
                        return;
                    }
                    else
                    {
                        lblMsg.Text = "Password succesfully updated! Click Go to Log in link.";
                        ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "Popup", "ShowSuccessMsg();", true);
                        return;
                    }
                }
            }
        }


    }
}