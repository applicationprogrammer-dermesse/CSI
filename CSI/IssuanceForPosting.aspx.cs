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
    public partial class IssuanceForPosting : System.Web.UI.Page
    {
        public bool IsPageRefresh = false;
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




                    lblBranchCode.Text = Session["BrCode"].ToString();
                    lblBranch.Text = Session["BranchName"].ToString();
                    lblNo.Text = Session["No"].ToString();
                    lblDatePosted.Text = Session["DatePosted"].ToString();
                    lblCategory.Text = Session["Category"].ToString();
                    lblDeliveredBy.Text =  Session["DelBy"].ToString();
                    lblIssueSlipNo.Text = Session["IssNo"].ToString();
                    loadIssuanceForPostingDetail();

                    if (Session["UserBranchCode"].ToString() == "1")
                    {
                        btnPost.Enabled = false;
                    }
                    else
                    {
                        btnPost.Enabled = true;
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


        private void loadIssuanceForPostingDetail()
        {
            using (SqlConnection sqlConn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["MyConnCSI"].ConnectionString))
            {
                string stR = @"dbo.LoadIssuanceForPostingDetail";
                using (SqlCommand cmD = new SqlCommand(stR, sqlConn))
                {
                    sqlConn.Open();
                    cmD.CommandTimeout = 0;
                    cmD.CommandType = CommandType.StoredProcedure;
                    cmD.Parameters.AddWithValue("@BrCode", Session["BrCode"].ToString());
                    cmD.Parameters.AddWithValue("@ControlNo", Session["No"].ToString());

                    DataTable dT = new DataTable();
                    SqlDataAdapter dA = new SqlDataAdapter(cmD);
                    dA.Fill(dT);

                    gvForIssuance.DataSource = dT;
                    gvForIssuance.DataBind();

                }
            }
        }

        protected void gvForIssuance_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            if (Session["UserBranchCode"].ToString() != "1")
            {
                gvForIssuance.EditIndex = -1;
                loadIssuanceForPostingDetail();

                lblMsg.Text = "You are not authorize to delete item.  Please contact DCI-Logistics!";
                ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "Popup", "ShowSuccessMsg();", true);
                return;
            }
            else
            {
                try
                {
                    string TheID = gvForIssuance.DataKeys[e.RowIndex].Value.ToString();

                    string connString = System.Configuration.ConfigurationManager.ConnectionStrings["MyConnCSI"].ConnectionString;

                    string sql = @"DELETE FROM ITEM_Pick where vRecNum = @TheID";
                    using (SqlConnection conn = new SqlConnection(connString))
                    {
                        conn.Open();

                        using (SqlCommand cmD = new SqlCommand(sql, conn))
                        {
                            cmD.CommandTimeout = 0;
                            cmD.Parameters.AddWithValue("@TheID", TheID.ToString());
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

                loadIssuanceForPostingDetail();
            }
        }

        protected void gvForIssuance_RowEditing(object sender, GridViewEditEventArgs e)
        {
            if (Session["UserBranchCode"].ToString() != "1")
            {
                gvForIssuance.EditIndex = -1;
                loadIssuanceForPostingDetail();

                lblMsg.Text = "You are not authorize to edit info.  Please contact DCI-Logistics!";
                ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "Popup", "ShowSuccessMsg();", true);


                return;
            }
            else
            {
                gvForIssuance.EditIndex = e.NewEditIndex;
                loadIssuanceForPostingDetail();
            }
        }

        protected void gvForIssuance_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            gvForIssuance.EditIndex = -1;
            loadIssuanceForPostingDetail();
        }

        protected void gvForIssuance_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            if (Session["UserBranchCode"].ToString() != "1")
            {
                gvForIssuance.EditIndex = -1;
                loadIssuanceForPostingDetail();

                lblMsg.Text = "You are not authorize to edit info.  Please contact DCI-Logistics!";
                ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "Popup", "ShowSuccessMsg();", true);
                return;
            }
            else
            {
                try
                {
                    string TheRecNum = gvForIssuance.DataKeys[e.RowIndex].Value.ToString();

                    GridViewRow row = gvForIssuance.Rows[e.RowIndex];
                    TextBox txtQuantity = (TextBox)row.FindControl("txtQuantity");

                    string connString = System.Configuration.ConfigurationManager.ConnectionStrings["MyConnCSI"].ConnectionString;

                    string sql = @"Update ITEM_Pick set vQtyPicked=@Qty where vRecNum = @RecNum";
                    using (SqlConnection conn = new SqlConnection(connString))
                    {
                        conn.Open();

                        using (SqlCommand cmD = new SqlCommand(sql, conn))
                        {
                            cmD.CommandTimeout = 0;
                            cmD.Parameters.AddWithValue("@RecNum", TheRecNum.ToString());
                            cmD.Parameters.AddWithValue("@Qty", txtQuantity.Text);
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

                gvForIssuance.EditIndex = -1;
                loadIssuanceForPostingDetail();
            }
        }

        protected void btnPost_Click(object sender, EventArgs e)
        {
            if (IsPageRefresh == true)
            {
                Response.Redirect(Request.Url.AbsoluteUri);
            }
            else
            {
                try
                {

                    foreach (GridViewRow gvR in gvForIssuance.Rows)
                    {
                        
                        
                        if (gvR.RowType == DataControlRowType.DataRow)
                        {

                            Label lblQuantity = (Label)gvR.FindControl("lblQuantity");

                            using (SqlConnection sqlConn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["MyConnCSI"].ConnectionString))
                            {
                                string stR = @"dbo.PostUnpostedIssuance";

                                using (SqlCommand cmD = new SqlCommand(stR, sqlConn))
                                {
                                    sqlConn.Open();
                                    cmD.CommandTimeout = 0;
                                    cmD.CommandType=CommandType.StoredProcedure;
                                    cmD.Parameters.AddWithValue("@BrCode",lblBranchCode.Text);
                                    cmD.Parameters.AddWithValue("@RecNum", gvR.Cells[1].Text);
                                    cmD.Parameters.AddWithValue("@HeaderID", gvR.Cells[2].Text);
                                    cmD.Parameters.AddWithValue("@Qty",lblQuantity.Text);
                                    cmD.Parameters.AddWithValue("@PostedDeliveredBranch", Session["UserFullName"].ToString());
                                    cmD.Parameters.AddWithValue("@DatePostedBR", DateTime.Now);
                                    cmD.ExecuteNonQuery();
                                }
                            }

                        }
                    }

                    lblMsgSuccessPosting.Text = lblBranch.Text + " - " + lblNo.Text + " Successfully posted!";
                    ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "Popup", "ShowMsgSuccessPosting();", true);
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

        protected void gvForIssuance_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (Session["UserBranchCode"].ToString() != "1")
                {

                    ((LinkButton)e.Row.FindControl("lnkView")).Enabled = false;
                    ((LinkButton)e.Row.FindControl("lnkDelete")).Enabled = false;
                    //((LinkButton)e.Row.FindControl("btnSelectItemID")).ForeColor = System.Drawing.Color.Red;
                    
                }
            }
        }


    }
}