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
    public partial class ReportAdjustments : System.Web.UI.Page
    {
        public bool IsPageRefresh = false;
        public int theExpiration;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["UserFullName"] == null)
            {
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "redirect script",
                "alert('You been idle for a long period of time, Need to Sign in again!'); location.href='Login.aspx';", true);
            }
            else
            {
                if (!IsPostBack)
                {

                    ViewState["ViewStateId"] = System.Guid.NewGuid().ToString();
                    Session["SessionId"] = ViewState["ViewStateId"].ToString();

                    if (Convert.ToInt32(Session["UserBranchCode"].ToString()) > 1 & Convert.ToInt32(Session["UserBranchCode"].ToString()) < 30)
                    {

                        loadPerBranch();

                    }
                    else
                    {
                        loadBranch();


                    }


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
        }

        private void loadPerBranch()
        {
            try
            {
                using (SqlConnection conN = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["MyConnCSI"].ConnectionString))
                {
                    string stR = @"SELECT BrCode,BrName FROM MyBranchList Where BrCode='" + Session["UserBranchCode"].ToString() + "'  ORDER BY BrCode";
                    using (SqlCommand cmD = new SqlCommand(stR, conN))
                    {
                        conN.Open();
                        SqlDataReader dR = cmD.ExecuteReader();

                        ddBranch.Items.Clear();
                        ddBranch.DataSource = dR;
                        ddBranch.DataValueField = "BrCode";
                        ddBranch.DataTextField = "BrName";
                        ddBranch.DataBind();

                    }
                }
            }
            catch (Exception x)
            {

                lblMsg.Text = x.Message;
                ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "Popup", "ShowSuccessMsg();", true);
                return;
            }

        }

        private void loadBranch()
        {
            try
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

                        ddBranch.Items.Clear();
                        ddBranch.DataSource = dR;
                        ddBranch.DataValueField = "BrCode";
                        ddBranch.DataTextField = "BrName";
                        ddBranch.DataBind();
                        ddBranch.Items.Insert(0, new ListItem("All Branches", "0"));
                        ddBranch.SelectedIndex = 0;
                    }
                }
            }
            catch (Exception x)
            {

                lblMsg.Text = x.Message;
                ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "Popup", "ShowSuccessMsg();", true);
                return;
            }

        }

        protected void btnGenerate_Click(object sender, EventArgs e)
        {
            using (SqlConnection sqlConn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["MyConnCSI"].ConnectionString))
            {
                string stR = @"dbo.ReportAdjustment";
                using (SqlCommand cmD = new SqlCommand(stR, sqlConn))
                {
                    sqlConn.Open();
                    cmD.CommandTimeout = 0;
                    cmD.CommandType = CommandType.StoredProcedure;
                    cmD.Parameters.AddWithValue("@BrCode", ddBranch.SelectedValue);
                    cmD.Parameters.AddWithValue("@dFROM", txtDateFrom.Text);
                    cmD.Parameters.AddWithValue("@dTO", txtDateTo.Text);

                    DataTable dT = new DataTable();
                    SqlDataAdapter dA = new SqlDataAdapter(cmD);
                    dA.Fill(dT);

                    gvAdjustment.DataSource = dT;
                    gvAdjustment.DataBind();

                }
            }
        }

        protected void gvAdjustment_PreRender(object sender, EventArgs e)
        {
            if (gvAdjustment.Rows.Count > 0)
            {
                gvAdjustment.UseAccessibleHeader = true;
                gvAdjustment.HeaderRow.TableSection = TableRowSection.TableHeader;
                gvAdjustment.FooterRow.TableSection = TableRowSection.TableFooter;
            }
        }

    }
}