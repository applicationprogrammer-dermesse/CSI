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
    public partial class Login : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btnLogin_Click(object sender, EventArgs e)
        {
            if (inpUserID.Value == string.Empty)
            {
                inpUserID.Focus();
                lblMsg.Text = "Please supply user ID";
                ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "Popup", "ShowSuccessMsg();", true);
                return;
            }
            else if (inpPassword.Value == string.Empty)
            {
                inpPassword.Focus();
                lblMsg.Text = "Please supply password.";
                ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "Popup", "ShowSuccessMsg();", true);
                return;
            }
            else
            {
                try
                {
                    int num = Convert.ToInt32(inpUserID.Value);
                    string sNum = num.ToString("00000");
                    inpUserID.Value = sNum.ToString();

                    using (SqlConnection conN = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["MyConnCSI"].ConnectionString))
                    {
                        string stR1 = @"Select vUser_ID,vUser_Pass,vUser_Type,vUser_Branch,vUser_Stat,vUser_FullName,
                            vUser_EmailAdd,BrName, IsApprover, AreaApprover
                                 from MyUserLogin where vUser_ID=@User_ID and vUser_Pass=@User_Pass";
               
                        
                        using (SqlCommand cmD = new SqlCommand(stR1, conN))
                        {
                            conN.Open();
                            cmD.Parameters.AddWithValue("@User_ID", inpUserID.Value);
                            //cmD.Parameters.AddWithValue("@User_Pass", inpPassword.Value);
                            cmD.Parameters.AddWithValue("@User_Pass", MyClass.Encrypt(inpPassword.Value, true));

                            SqlDataAdapter loginDA = new SqlDataAdapter(cmD);
                            DataTable loginDT = new DataTable();
                            loginDA.Fill(loginDT);
                            if (loginDT.Rows.Count > 0)
                            {
                                Session["UserID"] = loginDT.Rows[0]["vUser_ID"].ToString();
                                Session["UserType"] = loginDT.Rows[0]["vUser_Type"].ToString().TrimEnd();
                                Session["UserBranchCode"] = loginDT.Rows[0]["vUser_Branch"].ToString().TrimEnd();
                                Session["UserBranchName"] = loginDT.Rows[0]["BrName"].ToString().TrimEnd();
                                Session["UserFullName"] = loginDT.Rows[0]["vUser_FullName"].ToString().TrimEnd();
                                Session["IsApprover"] = loginDT.Rows[0]["IsApprover"].ToString().TrimEnd();
                                Session["AreaApprover"] = loginDT.Rows[0]["AreaApprover"].ToString().TrimEnd();
                                //Session["UserEmailAdd"] = loginDT.Rows[0]["vUser_EmailAdd"].ToString().TrimEnd();

                                Response.Redirect("~/HomePage.aspx");
                            }

                            else
                            {

                                lblMsg.Text = "User info does not exists!";
                                ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "Popup", "ShowSuccessMsg();", true);
                                return;

                            }

                        }
                    }
                }
                catch (FormatException x)
                {
                    lblMsg.Text = x.Message;
                    ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "Popup", "ShowSuccessMsg();", true);
                    return;
                }
                catch (SqlException s)
                {
                    lblMsg.Text = s.Message;
                    ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "Popup", "ShowSuccessMsg();", true);
                    return;
                }
            }

        }
    }
}