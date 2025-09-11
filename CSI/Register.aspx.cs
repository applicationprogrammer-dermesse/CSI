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
    public partial class Register : System.Web.UI.Page
    {
        public bool IsPageRefresh = false;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {

                ViewState["ViewStateId"] = System.Guid.NewGuid().ToString();
                Session["SessionId"] = ViewState["ViewStateId"].ToString();
                loadBranch();
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

        private void loadBranch()
        {
            using (SqlConnection conN = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["MyConnCSI"].ConnectionString))
            {
                string stR = @"dbo.LoadBranches";
                using (SqlCommand cmD = new SqlCommand(stR, conN))
                {
                    conN.Open();
                    cmD.CommandTimeout = 0;
                    cmD.CommandType = CommandType.StoredProcedure;
                    SqlDataReader dR = cmD.ExecuteReader();
                    
                    ddDepartment.Items.Clear();
                    ddDepartment.DataSource = dR;
                    ddDepartment.DataValueField = "BrCode";
                    ddDepartment.DataTextField = "BrName";
                    ddDepartment.DataBind();
                    ddDepartment.Items.Insert(0, new ListItem("Select Branch/Dept.", "0"));
                    ddDepartment.SelectedIndex = 0;
                }
            }
        }

        protected void btnRegister_Click(object sender, EventArgs e)
        {
            if (IsPageRefresh == true)
            {
                Response.Redirect(Request.Url.AbsoluteUri);
            }
            else
            {
                if (inpEmpNo.Value == string.Empty)
                {
                    inpEmpNo.Focus();
                    lblMsg.Text = "Please supply employee number";
                    ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "Popup", "ShowSuccessMsg();", true);
                    return;
                }
                else if (inpEmpName.Value == string.Empty)
                {
                    inpEmpName.Focus();
                    lblMsg.Text = "Please supply employee name.";
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
                else if (ddDepartment.SelectedValue == "0")
                {
                    ddDepartment.Focus();
                    lblMsg.Text = "Please select Branch/Department";
                    ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "Popup", "ShowSuccessMsg();", true);
                    return;
                }
                else
                {

                 
                    try
                    {
                        int num = Convert.ToInt32(inpEmpNo.Value);
                        string sNum = num.ToString("00000");
                        inpEmpNo.Value = sNum.ToString();

                        using (SqlConnection conN = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["MyConnCSI"].ConnectionString))
                        {
                            string stR1 = @"dbo.RegisterAccount";
                            using (SqlCommand cmD = new SqlCommand(stR1, conN))
                            {
                                conN.Open();
                                cmD.CommandTimeout = 0;
                                cmD.CommandType = CommandType.StoredProcedure;
                                cmD.Parameters.AddWithValue("@vUser_ID", inpEmpNo.Value);
                                cmD.Parameters.AddWithValue("@vUser_FullName", inpEmpName.Value);
                                cmD.Parameters.AddWithValue("@vUser_Pass", MyClass.Encrypt(inpPassword.Value, true));
                                cmD.Parameters.AddWithValue("@BrCode",ddDepartment.SelectedValue);
                                cmD.Parameters.AddWithValue("@BrName",ddDepartment.SelectedItem.Text);
					            if (ddDepartment.SelectedItem.Text=="DCI-Logistics")
                                {
                                    cmD.Parameters.AddWithValue("@vUser_Type", 2);
                                }
                                else if (ddDepartment.SelectedItem.Text=="DCI-Accounting")
                                {
                                    cmD.Parameters.AddWithValue("@vUser_Type", 4);
                                }
                                else
                                {
                                    cmD.Parameters.AddWithValue("@vUser_Type", 3);
                                }
                                cmD.Parameters.AddWithValue("@ResultValue", SqlDbType.Int).Direction = ParameterDirection.Output;
                                cmD.ExecuteNonQuery();
                                int Result = Convert.ToInt32(cmD.Parameters["@ResultValue"].Value);
                                if (Result == 99)
                                {
                                    lblMsg.Text = "Employee Number =  <b>" + inpEmpNo.Value + "</b>\n already exists!";
                                    ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "Popup", "ShowSuccessMsg();", true);
                                    return;


                                }
                                else
                                {
                                    lblMsg.Text = "Account succesfully created.  Click Go to Log in link.";
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
}