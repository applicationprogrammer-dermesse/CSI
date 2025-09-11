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
    public partial class ListRRwithoutPOforPosting : System.Web.UI.Page
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
                        loadRRforPosting(); 
                    }
                    else
                    {
                        Response.Redirect("~/AccessDenied.aspx");
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

        private void loadRRforPosting()
        {
            using (SqlConnection conN = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["MyConnCSI"].ConnectionString))
            {
                string stR = @"SELECT A.[RecNum] 
                              ,A.[RRNo]
                              ,A.[sup_ItemCode]
	                          ,B.sup_DESCRIPTION
                              ,A.[sup_UnitCost]
                              ,A.vQtyReceived
                              ,A.[TotalAmount]
                              ,A.[sup_RetailCost]
                              ,A.[RetailQuantity]
                              ,A.[vBatchNo]
                              ,A.[vDateExpiry]
                              ,A.[EncodedBy]
                          FROM [ITEM_RREntryWithoutPO] A
                          LEFT JOIN Sup_ItemMaster B
                          ON A.sup_ItemCode = B.sup_ItemCode
                          WHERE A.[vStat]=1";


                using (SqlCommand cmD = new SqlCommand(stR, conN))
                {
                    conN.Open();
                    cmD.CommandTimeout = 0;
                    //cmD.CommandType = CommandType.StoredProcedure;
                    //cmD.Parameters.AddWithValue("@RRNo", txtRRNumber.Text);
                    SqlDataAdapter dA = new SqlDataAdapter(cmD);
                    DataTable dT = new DataTable();
                    dA.Fill(dT);
                    gvRR.DataSource = dT;
                    gvRR.DataBind();

                }
            }
        }


        protected void gvRR_RowEditing(object sender, GridViewEditEventArgs e)
        {
            gvRR.EditIndex = e.NewEditIndex;
            loadRRforPosting();
        }

        protected void gvRR_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            gvRR.EditIndex = -1;
            loadRRforPosting();
        }

        protected void gvRR_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {

            try
            {
                GridViewRow row = gvRR.Rows[e.RowIndex];
                TextBox txtsup_RetailCost = (TextBox)row.FindControl("txtsup_RetailCost");
                TextBox txtRetailQuantity = (TextBox)row.FindControl("txtRetailQuantity");
                TextBox txtTotalAmount = (TextBox)row.FindControl("txtTotalAmount");
                
                string connString = System.Configuration.ConfigurationManager.ConnectionStrings["MyConnCSI"].ConnectionString;

                string sql = @"Update ITEM_RREntryWithoutPO set sup_RetailCost=@sup_RetailCost, RetailQuantity=@RetailQuantity,TotalAmount=@TotalAmount where RecNum = @RecNum";
                using (SqlConnection conn = new SqlConnection(connString))
                {
                    conn.Open();

                    using (SqlCommand cmD = new SqlCommand(sql, conn))
                    {
                        cmD.CommandTimeout = 0;

                        cmD.Parameters.AddWithValue("@RecNum", row.Cells[0].Text);
                        cmD.Parameters.AddWithValue("@sup_RetailCost", txtsup_RetailCost.Text);
                        cmD.Parameters.AddWithValue("@RetailQuantity", Convert.ToInt32(txtRetailQuantity.Text));
                        cmD.Parameters.AddWithValue("@TotalAmount", txtTotalAmount.Text);
                        
                        cmD.ExecuteNonQuery();
                    }
                }

                gvRR.EditIndex = -1;
                loadRRforPosting();
            }
            catch (Exception x)
            {
                lblMsg.Text = x.Message;
                ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "Popup", "ShowSuccessMsg();", true);
                return;
            }

        }

        protected void gvRR_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "Post")
            {
                try
                {
                    GridViewRow gvr = (GridViewRow)(((LinkButton)e.CommandSource).NamingContainer);
                    //int RowIndex = gvr.RowIndex;
                    Label lblsup_RetailCost = (Label)gvr.FindControl("lblsup_RetailCost");

                    string connString = System.Configuration.ConfigurationManager.ConnectionStrings["MyConnCSI"].ConnectionString;

                    string sql = @"dbo.PostUnpostedRREntry";
                    using (SqlConnection conn = new SqlConnection(connString))
                    {
                        conn.Open();

                        using (SqlCommand cmD = new SqlCommand(sql, conn))
                        {
                            cmD.CommandTimeout = 0;
                            cmD.CommandType = CommandType.StoredProcedure;
                            cmD.Parameters.AddWithValue("@ID", gvr.Cells[0].Text);
                            cmD.Parameters.AddWithValue("@PostedBy", Session["UserFullName"].ToString());
                            cmD.Parameters.AddWithValue("@sup_UnitCost", lblsup_RetailCost.Text);
                            cmD.Parameters.AddWithValue("@sup_ItemCode", gvr.Cells[2].Text);
                            cmD.ExecuteNonQuery();
                        }
                    }

                    gvRR.EditIndex = -1;
                    loadRRforPosting();

                    lblMsg.Text = "Successfully Posted";
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

        protected void gvRR_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            if (IsPageRefresh == true)
            {
                Response.Redirect(Request.Url.AbsoluteUri);
            }
            else
            {
                try 
                {
                    string RecNo = gvRR.DataKeys[e.RowIndex].Value.ToString();
                    string connString = System.Configuration.ConfigurationManager.ConnectionStrings["MyConnCSI"].ConnectionString;
                    string sql = @"Delete from ITEM_RREntryWithoutPO  where RecNum='" + RecNo + "'";
                    using (SqlConnection conn = new SqlConnection(connString))
                    {
                        conn.Open();

                        using (SqlCommand cmd = new SqlCommand(sql, conn))
                        {
                            cmd.CommandTimeout = 0;
                            cmd.ExecuteNonQuery();
                        }
                    }

                    loadRRforPosting();
                }
                catch (Exception y)
                {
                    lblMsg.Text = y.Message;
                    ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "Popup", "ShowSuccessMsg();", true);
                    return;
                }
            }
        }
    }
}