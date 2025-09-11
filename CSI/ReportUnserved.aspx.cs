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
    public partial class ReportUnserved : System.Web.UI.Page
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
            loadReportUnserved();
        }

        private void loadReportUnserved()
        {
            using (SqlConnection sqlConn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["MyConnCSI"].ConnectionString))
            {
                string stR = @"dbo.ReportUnserved";
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

                    gvUnserved.DataSource = dT;
                    gvUnserved.DataBind();

                }
            }
        }

        protected void gvUnserved_PreRender(object sender, EventArgs e)
        {
            if (gvUnserved.Rows.Count > 0)
            {
                gvUnserved.UseAccessibleHeader = true;
                gvUnserved.HeaderRow.TableSection = TableRowSection.TableHeader;
                gvUnserved.FooterRow.TableSection = TableRowSection.TableFooter;
            }
        }

        public string theRequestControlNo;
        public int theControlNo;
        private void getLastControlNo(string theBrCode)
        {
            try
            {
                using (SqlConnection sqlConn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["MyConnCSI"].ConnectionString))
                {
                    string stR = @"SELECT ControlNo FROM sup_RequestControlNo WHERE Brcode='" + theBrCode.ToString() + "'";

                    sqlConn.Open();
                    using (SqlCommand cmD = new SqlCommand(stR, sqlConn))
                    {
                        SqlDataReader dR = cmD.ExecuteReader();
                        cmD.CommandTimeout = 0;
                        while (dR.Read())
                        {
                            theControlNo = Convert.ToInt32(dR[0].ToString());
                            string sNum = theControlNo.ToString("00000");
                            theRequestControlNo = DateTime.Now.Year + "-" + theBrCode.ToString() + "-" + sNum;
                        }
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


        private void UpdateControlNo(string theBrCode)
        {
            try
            {
                using (SqlConnection sqlConn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["MyConnCSI"].ConnectionString))
                {
                    string stR = @"dbo.UpdateControlNo";
                    using (SqlCommand cmD = new SqlCommand(stR, sqlConn))
                    {
                        sqlConn.Open();
                        cmD.CommandTimeout = 0;
                        cmD.CommandType = CommandType.StoredProcedure;
                        cmD.Parameters.AddWithValue("@BrCode", theBrCode.ToString());
                        cmD.ExecuteNonQuery();
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

        protected void gvUnserved_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "Open")
            {
                try
                {
                    GridViewRow gvr = (GridViewRow)(((LinkButton)e.CommandSource).NamingContainer);
                    //int RowIndex = gvr.RowIndex;
                    //Label lblsup_RetailCost = (Label)gvr.FindControl("lblsup_RetailCost");
                    getLastControlNo(gvr.Cells[1].Text);

                    string connString = System.Configuration.ConfigurationManager.ConnectionStrings["MyConnCSI"].ConnectionString;

                    string sql = @"UPDATE BranchRequest SET Sup_Stat =1, Sup_ControlNo=@Sup_ControlNo WHERE Sup_RequestID=@Sup_RequestID";
                    using (SqlConnection conn = new SqlConnection(connString))
                    {
                        conn.Open();

                        using (SqlCommand cmD = new SqlCommand(sql, conn))
                        {
                            cmD.CommandTimeout = 0;
                            //cmD.CommandType = CommandType.StoredProcedure;
                            cmD.Parameters.AddWithValue("@Sup_RequestID", gvr.Cells[0].Text);
                            cmD.Parameters.AddWithValue("@Sup_ControlNo", theRequestControlNo.ToString());

                            cmD.ExecuteNonQuery();
                        }
                    }


                    UpdateControlNo(gvr.Cells[1].Text);
                    loadReportUnserved();
                    lblMsg.Text = "Successfully send to for approval";
                    ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "Popup", "ShowSuccessMsg();", true);
                    return;
                }
                catch (Exception x)
                {
                    lblMsg.Text = x.Message;
                    ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "Popup", "ShowSuccessMsg();", true);
                    return;
                }
            }
        }

        protected void gvUnserved_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (Session["UserBranchCode"].ToString() == "1")
                {
                    ((LinkButton)e.Row.FindControl("lnkOpen")).Enabled = true;

                }
                else
                {
                    //((LinkButton)e.Row.FindControl("btnUpdateSOpicked")).ForeColor = Color.Gray;
                    ((LinkButton)e.Row.FindControl("lnkOpen")).Enabled = false;
                }
            }
        }

    }
}