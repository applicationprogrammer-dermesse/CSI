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
    public partial class ForIssuance : System.Web.UI.Page
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


                    if(Session["UserBranchCode"].ToString() == "1")
                    {
                        btnPost.Enabled = true;
                        btnPrint.Enabled = true;
                    }
                    else
                    {
                        btnPost.Enabled = false;
                        btnPrint.Enabled = false;
                    }

                    lblBranchCode.Text = Session["BrCode"].ToString();
                    lblBranch.Text = Session["BranchName"].ToString();
                    lblNo.Text = Session["No"].ToString();
                    lblDatePosted.Text = Session["DatePosted"].ToString();
                    lblCategory.Text = Session["Category"].ToString();
                    lblType.Text = Session["Type"].ToString();

                    loadForIssuanceDetail();

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


        private void loadForIssuanceDetail()
        {
            using (SqlConnection sqlConn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["MyConnCSI"].ConnectionString))
            {
                string stR = @"dbo.LoadForIssuanceDetail";
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

            loadForIssuanceDetail();
        }

        protected void gvForIssuance_RowEditing(object sender, GridViewEditEventArgs e)
        {
            gvForIssuance.EditIndex = e.NewEditIndex;
            loadForIssuanceDetail();
        }

        protected void gvForIssuance_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            gvForIssuance.EditIndex = -1;
            loadForIssuanceDetail();
        }

        protected void gvForIssuance_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            UpdateQty(e);

               // GridViewRow row = gvForIssuance.Rows[e.RowIndex];
               // TextBox txtQuantity = (TextBox)row.FindControl("txtQuantity");

               //string connString = System.Configuration.ConfigurationManager.ConnectionStrings["MyConnCSI"].ConnectionString;
               //string sql = @"GetItemBatchesBalance";
               //using (SqlConnection conn = new SqlConnection(connString))
               //{
               //    conn.Open(); using (SqlCommand cmD = new SqlCommand(sql, conn))
               //    {
               //        cmD.CommandTimeout = 0;
               //        cmD.CommandType = CommandType.StoredProcedure;
               //        cmD.Parameters.AddWithValue("@HeaderID", row.Cells[2].Text);
               //        //cmD.Parameters.AddWithValue("@HeaderID", 9505);
               //        SqlDataAdapter dA = new SqlDataAdapter(cmD);
               //        DataTable dT = new DataTable();
               //        dA.Fill(dT);

               //        int theBalanceHeader = Convert.ToInt32(dT.Rows[0][5]);
               //        int thePickQty = Convert.ToInt32(txtQuantity.Text);
               //        if (theBalanceHeader >= thePickQty)
               //        {
               //            UpdateQty(e);
               //        }

               //        else
               //        {
               //            lblMsg.Text = "Insufficient balance.  Remaining balance is : " + dT.Rows[0][5].ToString();
               //            ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "Popup", "ShowSuccessMsg();", true);
               //            return;
               //        }
               //    }
               //}
        }

        private void UpdateQty(GridViewUpdateEventArgs e)
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
            loadForIssuanceDetail();
        }

        protected void btnPost_Click(object sender, EventArgs e)
        {
            if (IsPageRefresh == true)
            {
                Response.Redirect(Request.Url.AbsoluteUri);
            }
            else
            {
                
                
                using (SqlConnection sqlConn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["MyConnCSI"].ConnectionString))
                {
                    string stR = @"dbo.CheckZeroQty";
                    using (SqlCommand cmD = new SqlCommand(stR, sqlConn))
                    {
                        sqlConn.Open();
                        cmD.CommandTimeout = 0;
                        cmD.CommandType = CommandType.StoredProcedure;
                        cmD.Parameters.AddWithValue("@BrCode", Session["UserBranchCode"].ToString());
                        cmD.Parameters.AddWithValue("@ControlNo", lblNo.Text);

                        DataTable dT = new DataTable();
                        SqlDataAdapter dA = new SqlDataAdapter(cmD);
                        dA.Fill(dT);

                        if (dT.Rows.Count > 0)
                        {
                            lblMsg.Text = "There are item/s with zero quantity. Please edit/remove the item/s before submitting.";
                            ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "Popup", "ShowSuccessMsg();", true);
                            return; 
                        }
                        else
                        {
                            if (lblCategory.Text == "DPI Items(For Selling)")
                            {
                                SubmitIssuanceToSMS();
                            }
                            else if (lblCategory.Text == "DCI Items(For Selling)")
                            {
                                SubmitIssuanceToSMS();
                            }
                            else
                            {
                                SubmitIssuance();
                                return;
                            }
                        }
                    }
                }


                
            }
        }

        private void SubmitIssuanceToSMS()
        {
            if (IsPageRefresh == true)
            {
                Response.Redirect(Request.Url.AbsoluteUri);
            }
            else
            {

                foreach (GridViewRow gvR in gvForIssuance.Rows)
                    {
                        if (gvR.RowType == DataControlRowType.DataRow)
                        {

                            Label lblQuantity = (Label)gvR.FindControl("lblQuantity");

                            using (SqlConnection sqlConn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["MyConnCSI"].ConnectionString))
                            {
                                string stR = @"dbo.PostUnpostedIssuanceSMS";

                                using (SqlCommand cmD = new SqlCommand(stR, sqlConn))
                                {
                                    sqlConn.Open();
                                    cmD.CommandTimeout = 0;
                                    cmD.CommandType = CommandType.StoredProcedure;
                                    cmD.Parameters.AddWithValue("@RecNum", gvR.Cells[1].Text);
                                    cmD.Parameters.AddWithValue("@HeaderID", gvR.Cells[2].Text);
                                    cmD.Parameters.AddWithValue("@Qty", lblQuantity.Text);
                                    cmD.Parameters.AddWithValue("@BrCode", lblBranchCode.Text);
                                    cmD.Parameters.AddWithValue("@PostedDeliveredBranch", Session["UserFullName"].ToString());
                                    cmD.Parameters.AddWithValue("@DateDelivered", txtDateDeliverd.Text);
                                    cmD.Parameters.AddWithValue("@DeliveredBy", txtDeliveredBy.Text);
                                    cmD.Parameters.AddWithValue("@vUser_ID", Session["UserID"].ToString());
                                    cmD.Parameters.AddWithValue("@DRno", txtDRNumber.Text);
                                    cmD.Parameters.AddWithValue("@BrName",lblBranch.Text);
                                    cmD.Parameters.AddWithValue("@Sup_ControlNo", lblNo.Text);
                                    cmD.ExecuteNonQuery();
                                }
                            }

                        }


                    }
                    UpdateIssuanceNo();




                    lblMsgSuccessPosting.Text = lblBranch.Text + " - " + lblNo.Text + " Successfully posted!";
                    ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "Popup", "ShowMsgSuccessPosting();", true);
                    return;

            }
        }

        private void UpdateIssuanceNo()
        {
            using (SqlConnection conN = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["MyConnSMS"].ConnectionString))
            {


                string stR = @"UPDATE SystemMaster SET IssuanceNo=IssuanceNo + 1 WHERE BrCode=1";
                using (SqlCommand cmD = new SqlCommand(stR, conN))
                {
                    conN.Open();
                    cmD.CommandTimeout = 0;
         
                    cmD.ExecuteNonQuery();
         
                }
            }

        }
        private void SubmitIssuance()
        {
            if (IsPageRefresh == true)
            {
                Response.Redirect(Request.Url.AbsoluteUri);
            }
            else
            {
                try
                {
                    if (Convert.ToInt32(lblBranchCode.Text) >= 4 && Convert.ToInt32(lblBranchCode.Text) <= 30)
                    {
                        foreach (GridViewRow gvR in gvForIssuance.Rows)
                        {
                            if (gvR.RowType == DataControlRowType.DataRow)
                            {
                                using (SqlConnection sqlConn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["MyConnCSI"].ConnectionString))
                                {
                                    string stR = @"Update [ITEM_Pick] set vStat=2,DRNumber=@DRNumber,
                                            DateDelivered=@DateDelivered,DatePostedDelivered=@DatePostedDelivered,DeliveredBy=@DeliveredBy
                                                where vRecNum = @RecNum";

                                    using (SqlCommand cmD = new SqlCommand(stR, sqlConn))
                                    {
                                        sqlConn.Open();
                                        cmD.CommandTimeout = 0;
                                        cmD.Parameters.AddWithValue("@RecNum", gvR.Cells[1].Text);
                                        cmD.Parameters.AddWithValue("@DRNumber", txtDRNumber.Text);
                                        cmD.Parameters.AddWithValue("@DateDelivered", txtDateDeliverd.Text);
                                        cmD.Parameters.AddWithValue("@DeliveredBy", txtDeliveredBy.Text.ToUpper());
                                        cmD.Parameters.AddWithValue("@DatePostedDelivered", DateTime.Now);
                                        cmD.ExecuteNonQuery();
                                    }
                                }

                            }
                        }

                        lblMsgSuccessPosting.Text = lblBranch.Text + " - " + lblNo.Text + " Successfully posted!";
                        ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "Popup", "ShowMsgSuccessPosting();", true);
                        return;
                    }
                    else
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
                                        cmD.CommandType = CommandType.StoredProcedure;
                                        cmD.Parameters.AddWithValue("@BrCode", lblBranchCode.Text);
                                        cmD.Parameters.AddWithValue("@RecNum", gvR.Cells[1].Text);
                                        cmD.Parameters.AddWithValue("@HeaderID", gvR.Cells[2].Text);
                                        cmD.Parameters.AddWithValue("@Qty", lblQuantity.Text);
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
                }
                catch (Exception x)
                {

                    lblMsg.Text = x.Message;
                    ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "Popup", "ShowSuccessMsg();", true);
                    return;
                }
            }
        }

        protected void btnPrint_Click(object sender, EventArgs e)
        {

            Session["TheBranch"] = lblBranch.Text;
            Session["TheNo"] = lblNo.Text;
            //Session["TheDeliveredBy"] = txtDeliveredBy.Text;
            //Session["TheDateDelivered"] = txtDateDeliverd.Text;

            Response.Write("<script>window.open ('PrintIssueSlip.aspx?iNo=" + lblNo.Text + " ','_blank');</script>");

            //Response.Redirect("PrintIssueSlip.aspx?iNo=" + lblNo.Text + "&iBranch=" + lblBranch.Text);  

        }

        protected void gvForIssuance_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "ChangeItem")
            {
                if (Convert.ToInt32(Session["UserBranchCode"]) == 1)
                {
                    GridViewRow gvr = (GridViewRow)(((LinkButton)e.CommandSource).NamingContainer);
                    Label lblQuantity = (Label)gvr.FindControl("lblQuantity");
                    int RowIndex = gvr.RowIndex;

                    string theOrderID = gvr.Cells[1].Text;
                    string theHeaderID = gvr.Cells[2].Text;
                    string theItemCode = gvr.Cells[3].Text;
                    string theQtyToPick = lblQuantity.Text;
                    string theItemDesc = gvr.Cells[4].Text;
                    GetItemBatch(theItemCode, theOrderID, theQtyToPick, theItemDesc, theHeaderID);
                }
                else
                {
                    lblMsg.Text = "You are not authorized to select item batches.";
                    ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "Popup", "ShowSuccessMsg();", true);
                    return;
                }
            }
        }

        private void GetItemBatch(string theItemCode, String theOrderID, string theQtyToPick, string theItemDesc, string theHeaderID)
        {
            if (IsPageRefresh == true)
            {
                Response.Redirect(Request.Url.AbsoluteUri);
            }
            else
            {
                try
                {
                    lblOrderID.Text = theOrderID.ToString();
                    lblQtyToPick.Text = theQtyToPick.ToString();
                    lblItemCodeToPick.Text = theItemCode.ToString();
                    lblItemDescToPick.Text = theItemDesc.ToString();
                    lblHeaderID.Text = theHeaderID.ToString();
                    using (SqlConnection Conn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["MyConnCSI"].ConnectionString))
                    {


                        string stRGetBal = @"dbo.GetItemBatchesReplacement";
                        using (SqlCommand cmD = new SqlCommand(stRGetBal, Conn))
                        {
                            Conn.Open();
                            cmD.CommandType = CommandType.StoredProcedure;
                            cmD.Parameters.AddWithValue("@ItemCode", theItemCode);
                            cmD.Parameters.AddWithValue("@CategoryType", 0);
                            cmD.Parameters.AddWithValue("@HeaderID", theHeaderID.ToString());
                            DataTable dT = new DataTable();
                            SqlDataAdapter dA = new SqlDataAdapter(cmD);
                            dA.Fill(dT);

                            if (dT.Rows.Count > 0)
                            {
                                gvItemBatch.DataSource = dT;
                                gvItemBatch.DataBind();
                                ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "Popup", "ShowGridItemBatch();", true);
                                return;
                            }
                            else
                            {
                                lblMsg.Text = "No available batch replacement.";
                                ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "Popup", "ShowSuccessMsg();", true);
                                return;
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
        }


        protected void gvItemBatch_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            if (IsPageRefresh == true)
            {
                Response.Redirect(Request.Url.AbsoluteUri);
            }
            else
            {
                GridViewRow row = gvItemBatch.Rows[e.RowIndex];
                TextBox txtBalance = (TextBox)row.FindControl("txtBalance");
                if (Convert.ToInt32(lblQtyToPick.Text) > Convert.ToInt32(txtBalance.Text))
                {
                    lblMsg.Text = "Cannot change batch/Expiry for item <br /><b>" + lblItemDescToPick.Text + "</b> <br /> Not enough balance on the selected batch.";
                    ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "Popup", "ShowSuccessMsg();", true);
                    return;
                }
                else
                {

                    using (SqlConnection sqlConn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["MyConnCSI"].ConnectionString))
                    {
                        string stR = @"UPDATE [ITEM_Pick] SET HeaderID=@HeaderID,vBatchNo=@vBatchNo,vDateExpiry=@vDateExpiry where vRecNum=@vRecNum";

                        using (SqlCommand cmD = new SqlCommand(stR, sqlConn))
                        {
                            sqlConn.Open();
                            cmD.CommandTimeout = 0;
                            //cmD.CommandType = CommandType.StoredProcedure;
                            cmD.Parameters.AddWithValue("@vRecNum", lblOrderID.Text);
                            cmD.Parameters.AddWithValue("@HeaderID", gvItemBatch.Rows[e.RowIndex].Cells[0].Text);
                            cmD.Parameters.AddWithValue("@vBatchNo", gvItemBatch.Rows[e.RowIndex].Cells[2].Text);
                            cmD.Parameters.AddWithValue("@vDateExpiry", gvItemBatch.Rows[e.RowIndex].Cells[3].Text);
                            cmD.ExecuteNonQuery();
                        }
                    }

                    loadForIssuanceDetail();
                }
                
            }
        }

        protected void PrintMultiple_Click(object sender, EventArgs e)
        {
            Session["BrCodeToPrint"] = lblBranchCode.Text;
            Session["BranchNameToPrint"] = lblBranch.Text;

            Response.Redirect("~/PrintMultipleRequest.aspx?val=" + Session["BrCodeToPrint"].ToString() + " - " + Session["BranchNameToPrint"].ToString());
        }


    }
}