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
    public partial class ForApprovalLogistics : System.Web.UI.Page
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

                    if (Session["UserBranchCode"].ToString() == "1")
                    {
                        btnPost.Enabled = true;
                    }
                    else
                    {
                        btnPost.Enabled = false;
                    }

                    lblBranchCode.Text = Session["BrCode"].ToString();
                    lblBranch.Text = Session["BranchName"].ToString();
                    lblNo.Text = Session["No"].ToString();
                    lblDateSubmit.Text = Session["DateSubmit"].ToString();
                    lblCategory.Text = Session["Category"].ToString();
                    lblType.Text = Session["Type"].ToString();
                    loadRequestDetail();

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


        private void loadRequestDetail()
        {
            using (SqlConnection sqlConn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["MyConnCSI"].ConnectionString))
            {
                string stR = @"dbo.LoadForApprovalDetailLogistics";
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

                    gvForApproval.DataSource = dT;
                    gvForApproval.DataBind();

                }
            }
        }

        public string sqlD;
        protected void gvForApproval_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            try
            {
                string TheID = gvForApproval.DataKeys[e.RowIndex].Value.ToString();

                string connString = System.Configuration.ConfigurationManager.ConnectionStrings["MyConnCSI"].ConnectionString;

                if (Session["UserBranchCode"].ToString() == "1")
                {
                    sqlD = @"Update [BranchRequest] set Sup_Stat=4 ,Sup_ApprovedBy= 'Del by: " + Session["UserFullName"].ToString() + "' ,Sup_DateApproved = '" + DateTime.Now + "' where Sup_RequestID = @TheID";
                }
                else
                {
                    sqlD = @"Delete from [BranchRequest] where Sup_RequestID = @TheID";
                }

                using (SqlConnection conn = new SqlConnection(connString))
                {
                    conn.Open();

                    using (SqlCommand cmD = new SqlCommand(sqlD, conn))
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

            loadRequestDetail();
        }

        protected void gvForApproval_RowEditing(object sender, GridViewEditEventArgs e)
        {
            gvForApproval.EditIndex = e.NewEditIndex;
            loadRequestDetail();
        }

        protected void gvForApproval_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            gvForApproval.EditIndex = -1;
            loadRequestDetail();
        }

        protected void gvForApproval_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            try
            {
                string TheID = gvForApproval.DataKeys[e.RowIndex].Value.ToString();

                GridViewRow row = gvForApproval.Rows[e.RowIndex];
                TextBox txtQuantity = (TextBox)row.FindControl("txtQuantity");

                string connString = System.Configuration.ConfigurationManager.ConnectionStrings["MyConnCSI"].ConnectionString;

                string sql = @"Update [BranchRequest] set Sup_Qty=@Sup_Qty where Sup_RequestID = @TheID";
                using (SqlConnection conn = new SqlConnection(connString))
                {
                    conn.Open();

                    using (SqlCommand cmD = new SqlCommand(sql, conn))
                    {
                        cmD.CommandTimeout = 0;
                        cmD.Parameters.AddWithValue("@TheID", TheID.ToString());
                        cmD.Parameters.AddWithValue("@Sup_Qty", txtQuantity.Text);
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

            gvForApproval.EditIndex = -1;
            loadRequestDetail();
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

                    foreach (GridViewRow gvR in gvForApproval.Rows)
                    {
                        if (gvR.RowType == DataControlRowType.DataRow)
                        {
                            using (SqlConnection sqlConn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["MyConnCSI"].ConnectionString))
                            {
                                string stR = @"Update [BranchRequest] set Sup_Stat=2 
                                              ,Sup_ApprovedByLogistics= @Sup_ApprovedBy
                                              ,Sup_DateApprovedLogistics = @Sup_DateApproved
                                        where Sup_RequestID = @TheID";

                                using (SqlCommand cmD = new SqlCommand(stR, sqlConn))
                                {
                                    sqlConn.Open();
                                    cmD.CommandTimeout = 0;
                                    cmD.Parameters.AddWithValue("@TheID", gvR.Cells[0].Text);
                                    cmD.Parameters.AddWithValue("@Sup_ApprovedBy", Session["UserFullName"].ToString());
                                    cmD.Parameters.AddWithValue("@Sup_DateApproved", DateTime.Now);
                                    cmD.ExecuteNonQuery();
                                }
                            }

                        }
                    }

                    lblMsgSuccessPosting.Text = lblBranch.Text + " - " + lblNo.Text + " Successfully approved!";
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


    }
}