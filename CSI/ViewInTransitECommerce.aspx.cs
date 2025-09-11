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
    public partial class ViewInTransitECommerce : System.Web.UI.Page
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



                    if (Session["UserBranchCode"].ToString() == "50" | Session["UserBranchCode"].ToString() == "51" | Session["UserBranchCode"].ToString() == "55")
                    {


                        lblOrderDate.Text = Session["OrderDate"].ToString();
                        lblReferenceNo.Text = Session["ReferenceNo"].ToString();
                        lblSource.Text = Session["Source"].ToString();
                        //lblType.Text = Session["Type"].ToString();
                        //lblCustomerName.Text = Session["CustomerName"].ToString();
                        //lblCustomerAddress.Text = Session["Customer Address"].ToString();
                        //lblContactNo.Text = Session["ContactNo"].ToString();
                        //lblEmailAddress.Text = Session["EmailAddress"].ToString();
                        //lblDateDelivered.Text = Session["DateDelivered"].ToString();
                        //lblDeliveredBy.Text = Session["DeliveredBy"].ToString();

                        loadInTransitOrderEntry();
                        btnPost.Enabled = true;
                        btnCancelled.Enabled = true;
                        loadItemForSelling();
                        //this.RegisterPostBackControl();
                    }
                    else
                    {
                        btnPost.Enabled = false;
                        btnCancelled.Enabled = false;
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

        private void loadItemForSelling()
        {
            using (SqlConnection sqlConn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["MyConnCSI"].ConnectionString))
            {

                string stR = @"SELECT DISTINCT a.sup_ItemCode,a.sup_ItemCode + ' - ' + b.sup_DESCRIPTION as [sup_DESCRIPTION] 
                                FROM [ITEM_RecordsHeader] a
                                LEFT JOIN Sup_ItemMaster b
                                ON a.sup_ItemCode=b.sup_ItemCode
                                WHERE a.BrCode=@BrCode and a.Sup_CategoryNum=8
								ORDER BY sup_DESCRIPTION";

                using (SqlCommand cmD = new SqlCommand(stR, sqlConn))
                {
                    sqlConn.Open();
                    cmD.CommandTimeout = 0;
                    cmD.Parameters.AddWithValue("@BrCode", Session["UserBranchCode"].ToString());
                    DataTable dT = new DataTable();
                    SqlDataAdapter dA = new SqlDataAdapter(cmD);
                    dA.Fill(dT);
                    ddITemToAdd.Items.Clear();
                    ddITemToAdd.DataSource = dT;
                    ddITemToAdd.DataTextField = "sup_DESCRIPTION";
                    ddITemToAdd.DataValueField = "sup_ItemCode";
                    ddITemToAdd.DataBind();
                    ddITemToAdd.Items.Insert(0, new ListItem("Please select item", "0"));
                    ddITemToAdd.SelectedIndex = 0;
                }
            }
        }


        private void loadInTransitOrderEntry()
        {
            using (SqlConnection sqlConn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["MyConnCSI"].ConnectionString))
            {

                string stR = @"dbo.LoadInTransitOrderDetail";

                using (SqlCommand cmD = new SqlCommand(stR, sqlConn))
                {
                    sqlConn.Open();
                    cmD.CommandTimeout = 0;
                    cmD.CommandType = CommandType.StoredProcedure;
                    cmD.Parameters.AddWithValue("@ReferenceNo", Session["ReferenceNo"].ToString());
                    cmD.Parameters.AddWithValue("@Source", Session["Source"].ToString());
                    DataTable dT = new DataTable();
                    SqlDataAdapter dA = new SqlDataAdapter(cmD);
                    dA.Fill(dT);

                    gvItems.DataSource = dT;
                    gvItems.DataBind();

                    //if (dT.Rows.Count > 0)
                    //{
                    //    lblMOP.Text = dT.Rows[0][9].ToString();
                    //    lblDeliveryInstruction.Text = dT.Rows[0][10].ToString();
                    //    lblShippingFee.Text = dT.Rows[0][11].ToString();
                    //}

                }
            }
        }







        //here
        protected void btnPost_Click(object sender, EventArgs e)
        {
            if (IsPageRefresh == true)
            {
                Response.Redirect(Request.Url.AbsoluteUri);
            }
            else
            {
                foreach (GridViewRow gvR in gvItems.Rows)
                {
                    if (gvR.RowType == DataControlRowType.DataRow)
                    {

                        Label lblQuantity = (Label)gvR.FindControl("lblQuantity");

                        using (SqlConnection sqlConn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["MyConnCSI"].ConnectionString))
                        {
                            string stR = @"dbo.PostInTransitOrder";

                            using (SqlCommand cmD = new SqlCommand(stR, sqlConn))
                            {
                                sqlConn.Open();
                                cmD.CommandTimeout = 0;
                                cmD.CommandType = CommandType.StoredProcedure;
                                cmD.Parameters.AddWithValue("@OrderID", gvR.Cells[0].Text);
                                cmD.Parameters.AddWithValue("@HeaderID", gvR.Cells[1].Text);
                                cmD.Parameters.AddWithValue("@DatePostedDelivered", txtPostedDateDelivered.Text);
                                cmD.Parameters.AddWithValue("@vQtyOnline", lblQuantity.Text);
                                cmD.ExecuteNonQuery();
                            }
                        }

                    }
                }

                lblMsgSuccessPosting.Text = "Successfully posted!";
                ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "Popup", "ShowMsgSuccessPosting();", true);
                return;
            }
        }

        protected void gvItems_RowEditing(object sender, GridViewEditEventArgs e)
        {
            gvItems.EditIndex = e.NewEditIndex;
            loadInTransitOrderEntry();
        }

        protected void gvItems_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            gvItems.EditIndex = -1;
            loadInTransitOrderEntry();
        }

        protected void gvItems_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            try
            {
                string TheID = gvItems.DataKeys[e.RowIndex].Value.ToString();

                GridViewRow row = gvItems.Rows[e.RowIndex];
                TextBox txtQuantity = (TextBox)row.FindControl("txtQuantity");
                TextBox txtSRP = (TextBox)row.FindControl("txtSRP");
                Label lblOrigQuantity = (Label)row.FindControl("lblOrigQuantity");
                
                
                string connString = System.Configuration.ConfigurationManager.ConnectionStrings["MyConnCSI"].ConnectionString;

                string sql = @"Update [ITEM_OnlineSold] set vQtyPicked=@vQtyPicked, vUnitCost=@SRP where OrderID = @OrderID";
                using (SqlConnection conn = new SqlConnection(connString))
                {
                    conn.Open();

                    using (SqlCommand cmD = new SqlCommand(sql, conn))
                    {
                        cmD.CommandTimeout = 0;
                        cmD.Parameters.AddWithValue("@OrderID", TheID.ToString());
                        cmD.Parameters.AddWithValue("@vQtyPicked", txtQuantity.Text);
                        cmD.Parameters.AddWithValue("@SRP", txtSRP.Text);
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


            gvItems.EditIndex = -1;
            loadInTransitOrderEntry();
        }

        protected void btnCancelled_Click(object sender, EventArgs e)
        {

            if (IsPageRefresh == true)
            {
                Response.Redirect(Request.Url.AbsoluteUri);
            }
            else
            {
                foreach (GridViewRow gvR in gvItems.Rows)
                {
                    if (gvR.RowType == DataControlRowType.DataRow)
                    {

                       // Label lblQuantity = (Label)gvR.FindControl("lblQuantity");

                        using (SqlConnection sqlConn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["MyConnCSI"].ConnectionString))
                        {
                            string stR = @"dbo.CancelledInTransitOrder";

                            using (SqlCommand cmD = new SqlCommand(stR, sqlConn))
                            {
                                sqlConn.Open();
                                cmD.CommandTimeout = 0;
                                cmD.CommandType = CommandType.StoredProcedure;
                                cmD.Parameters.AddWithValue("@OrderID", gvR.Cells[0].Text);
                                //cmD.Parameters.AddWithValue("@HeaderID", gvR.Cells[1].Text);
                                cmD.Parameters.AddWithValue("@DatePostedDelivered", txtCancelledDate.Text);
                                //cmD.Parameters.AddWithValue("@vQtyOnline", lblQuantity.Text);
                                cmD.ExecuteNonQuery();
                            }
                        }

                    }
                }

                lblMsgSuccessPosting.Text = "Done Cancelled!";
                ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "Popup", "ShowMsgSuccessPosting();", true);
                return;
            }
        }

        protected void gvItems_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "ChangeItem")
            {
                GridViewRow gvr = (GridViewRow)(((LinkButton)e.CommandSource).NamingContainer);
                int RowIndex = gvr.RowIndex;

                string theOrderID = gvr.Cells[0].Text;
                string theItemCode = gvr.Cells[3].Text;
                string theItemDesc = gvr.Cells[4].Text;
                GetItemList(theItemCode, theOrderID, theItemDesc);
            }
        }


        private void GetItemList(string theItemCodeToChange, String theOrderIDToChange, string theItemDescToChange)
        {
            if (IsPageRefresh == true)
            {
                Response.Redirect(Request.Url.AbsoluteUri);
            }
            else
            {
                try
                {
                   

                    lblOrderIDToChange.Text = theOrderIDToChange.ToString();
                    lblItemCodeToChange.Text = theItemCodeToChange.ToString();
                    lblItemDescToChange.Text = theItemDescToChange.ToString();
                    using (SqlConnection Conn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["MyConnCSI"].ConnectionString))
                    {


                        string stRGetBal = @"dbo.LoadItemECommerceReplacement";
                        using (SqlCommand cmD = new SqlCommand(stRGetBal, Conn))
                        {
                            Conn.Open();
                            cmD.CommandType = CommandType.StoredProcedure;
                            cmD.Parameters.AddWithValue("@ItemCode",lblItemCodeToChange.Text);
                            DataTable dT = new DataTable();
                            SqlDataAdapter dA = new SqlDataAdapter(cmD);
                            dA.Fill(dT);

                            ddItem.Items.Clear();
                            ddItem.DataSource = dT;
                            ddItem.DataTextField = "sup_DESCRIPTION";
                            ddItem.DataValueField = "sup_ItemCode";
                            ddItem.DataBind();
                            ddItem.Items.Insert(0, new ListItem("Please select item", "0"));
                            ddItem.SelectedIndex = 0;



                            gvItemBatch.DataSource = null;
                            gvItemBatch.DataBind();
                            
                            ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "Popup", "ShowGridItemToChange();", true);

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


        private void ShowItmBatches()
        {

            using (SqlConnection Conn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["MyConnCSI"].ConnectionString))
            {


                string stRGetBal = @"dbo.GetItemBatchesOnline";
                using (SqlCommand cmD = new SqlCommand(stRGetBal, Conn))
                {
                    Conn.Open();
                    cmD.CommandType = CommandType.StoredProcedure;
                    cmD.Parameters.AddWithValue("@ItemCode", ddItem.SelectedValue);
                    cmD.Parameters.AddWithValue("@CategoryType", 8);
                    cmD.Parameters.AddWithValue("@BrCode", Session["UserBranchCode"].ToString());
                    DataTable dT = new DataTable();
                    SqlDataAdapter dA = new SqlDataAdapter(cmD);
                    dA.Fill(dT);

                        gvItemBatch.DataSource = dT;
                        gvItemBatch.DataBind();
                }
            }
        }

        

        protected void ddItem_SelectedIndexChanged(object sender, EventArgs e)
        {
            ShowItmBatches();
        }


        //private void RegisterPostBackControl()
        //{
        //    foreach (GridViewRow row in gvItemBatch.Rows)
        //    {
        //        LinkButton lnkFull = row.FindControl("lnkFull") as LinkButton;
        //        ScriptManager.GetCurrent(this).RegisterPostBackControl(lnkFull);
        //    }
        //}


        //protected void FullPostBack(object sender, EventArgs e)
        //{
        //    //string fruitName = ((sender as LinkButton).NamingContainer as GridViewRow).Cells[0].Text;
        //    //string message = "alert('Full PostBack: You clicked " + fruitName + "')";
        //    //ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", message, true);

        //    //GridViewRow row = gvItemBatch.Rows[e.RowIndex];
        //    string theHid = ((sender as LinkButton).NamingContainer as GridViewRow).Cells[0].Text;
        //    //TextBox textQtyPicked = (TextBox)row.FindControl("txtQtyPicked");
        //    //TextBox textSRP = (TextBox)row.FindControl("txtSRP");

        //    using (SqlConnection sqlConn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["MyConnCSI"].ConnectionString))
        //    {
        //        string stR = @"dbo.UpdateItemECommerce";

        //        using (SqlCommand cmD = new SqlCommand(stR, sqlConn))
        //        {
        //            sqlConn.Open();
        //            cmD.CommandTimeout = 0;
        //            cmD.CommandType = CommandType.StoredProcedure;
        //            cmD.Parameters.AddWithValue("@OrderID", lblOrderIDToChange.Text);
        //            cmD.Parameters.AddWithValue("@HeaderID", theHid.ToString());// gvItemBatch.Rows[e.RowIndex].Cells[0].Text);
        //            cmD.Parameters.AddWithValue("@Sup_ItemCode", ddItem.SelectedValue);

        //            cmD.ExecuteNonQuery();

        //        }
        //    }


        //    loadInTransitOrderEntry();

        //}

        protected void gvItemBatch_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            if (IsPageRefresh == true)
            {
                Response.Redirect(Request.Url.AbsoluteUri);
            }
            else
            {
                //try
                //{
                GridViewRow row = gvItemBatch.Rows[e.RowIndex];
                //TextBox textQtyPicked = (TextBox)row.FindControl("txtQtyPicked");
                TextBox textSRP = (TextBox)row.FindControl("txtSRP");

                using (SqlConnection sqlConn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["MyConnCSI"].ConnectionString))
                {
                    string stR = @"dbo.UpdateItemECommerce";

                    using (SqlCommand cmD = new SqlCommand(stR, sqlConn))
                    {
                        sqlConn.Open();
                        cmD.CommandTimeout = 0;
                        cmD.CommandType = CommandType.StoredProcedure;
                        cmD.Parameters.AddWithValue("@OrderID", lblOrderIDToChange.Text);
                        cmD.Parameters.AddWithValue("@HeaderID", gvItemBatch.Rows[e.RowIndex].Cells[0].Text);
                        cmD.Parameters.AddWithValue("@Sup_ItemCode", ddItem.SelectedValue);
                        cmD.Parameters.AddWithValue("@srp", textSRP.Text);
                        
                        cmD.ExecuteNonQuery();

                    }
                }


                loadInTransitOrderEntry();

                lblMsgSuccessPosting.Text = "Item successfully change!";
                ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "Popup", "ShowMsgSuccessPosting();", true);
                return;


            }
        }

        protected void gvItemBatch_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            //if (e.Row.RowType == DataControlRowType.DataRow)
            //{
            //    LinkButton lb = e.Row.FindControl("lnkSelect") as LinkButton;
            //    ScriptManager.GetCurrent(this).RegisterAsyncPostBackControl(lb);
            //}

            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                
                //LinkButton i1 = (LinkButton)(e.Row.Cells[7].Controls[1]);
                //ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "DoPostBack", "__doPostBack(sender, e)", true);
            }
        }

        protected void btnPostBack_Click(object sender, EventArgs e)
        {
            loadInTransitOrderEntry();
        }

        protected void lnkPost_Click(object sender, EventArgs e)
        {
            if (IsPageRefresh == true)
            {
                Response.Redirect(Request.Url.AbsoluteUri);
            }
            else
            {
                foreach (GridViewRow gvR in gvItems.Rows)
                {
                    if (gvR.RowType == DataControlRowType.DataRow)
                    {

                        Label lblQuantity = (Label)gvR.FindControl("lblQuantity");

                        using (SqlConnection sqlConn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["MyConnCSI"].ConnectionString))
                        {
                            string stR = @"dbo.PostInTransitOrder";

                            using (SqlCommand cmD = new SqlCommand(stR, sqlConn))
                            {
                                sqlConn.Open();
                                cmD.CommandTimeout = 0;
                                cmD.CommandType = CommandType.StoredProcedure;
                                cmD.Parameters.AddWithValue("@OrderID", gvR.Cells[0].Text);
                                cmD.Parameters.AddWithValue("@HeaderID", gvR.Cells[1].Text);
                                cmD.Parameters.AddWithValue("@DatePostedDelivered", txtPostedDateDelivered.Text);
                                cmD.Parameters.AddWithValue("@vQtyOnline", lblQuantity.Text);
                                cmD.ExecuteNonQuery();
                            }
                        }

                    }
                }

                lblMsgSuccessPosting.Text = "Successfully posted!";
                ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "Popup", "ShowMsgSuccessPosting();", true);
                return;
            }
        }

        protected void lnkCancelled_Click(object sender, EventArgs e)
        {
            if (IsPageRefresh == true)
            {
                Response.Redirect(Request.Url.AbsoluteUri);
            }
            else
            {
                foreach (GridViewRow gvR in gvItems.Rows)
                {
                    if (gvR.RowType == DataControlRowType.DataRow)
                    {

                        // Label lblQuantity = (Label)gvR.FindControl("lblQuantity");

                        using (SqlConnection sqlConn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["MyConnCSI"].ConnectionString))
                        {
                            string stR = @"dbo.CancelledInTransitOrder";

                            using (SqlCommand cmD = new SqlCommand(stR, sqlConn))
                            {
                                sqlConn.Open();
                                cmD.CommandTimeout = 0;
                                cmD.CommandType = CommandType.StoredProcedure;
                                cmD.Parameters.AddWithValue("@OrderID", gvR.Cells[0].Text);
                                //cmD.Parameters.AddWithValue("@HeaderID", gvR.Cells[1].Text);
                                cmD.Parameters.AddWithValue("@DatePostedDelivered", txtCancelledDate.Text);
                                //cmD.Parameters.AddWithValue("@vQtyOnline", lblQuantity.Text);
                                cmD.ExecuteNonQuery();
                            }
                        }

                    }
                }

                lblMsgSuccessPosting.Text = "Done Cancelled!";
                ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "Popup", "ShowMsgSuccessPosting();", true);
                return;
            }
        }

        protected void ddITemToAdd_SelectedIndexChanged(object sender, EventArgs e)
        {
        
            using (SqlConnection Conn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["MyConnCSI"].ConnectionString))
            {


                string stRGetBal = @"dbo.GetItemBatchesOnline";
                using (SqlCommand cmD = new SqlCommand(stRGetBal, Conn))
                {
                    Conn.Open();
                    cmD.CommandType = CommandType.StoredProcedure;
                    cmD.Parameters.AddWithValue("@ItemCode",ddITemToAdd.SelectedValue);
                    cmD.Parameters.AddWithValue("@CategoryType", 8);
                    cmD.Parameters.AddWithValue("@BrCode", Session["UserBranchCode"].ToString());
                    DataTable dT = new DataTable();
                    SqlDataAdapter dA = new SqlDataAdapter(cmD);
                    dA.Fill(dT);

                    if (dT.Rows.Count > 0)
                    {
                        lblItemDesc.Text =ddITemToAdd.SelectedItem.Text;
                        gvItemBatchToAddd.DataSource = dT;
                        gvItemBatchToAddd.DataBind();
                        ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "Popup", "ShowGridItemBatchToAddd();", true);
                        return;
                    }
                    else
                    {
                        lblItemDesc.Text = string.Empty;
                        lblMsg.Text = "No Available Balance!";
                        ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "Popup", "ShowSuccessMsg();", true);
                        return;
                    }
                }
            }
        
     }



        protected void gvItemBatchToAddd_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            if (IsPageRefresh == true)
            {
                Response.Redirect(Request.Url.AbsoluteUri);
            }
            else
            {
                //try
                //{
                GridViewRow row = gvItemBatchToAddd.Rows[e.RowIndex];
                TextBox textQtyPickedToAdd = (TextBox)row.FindControl("txtQtyPickedToAdd");
                TextBox textSRPToAdd = (TextBox)row.FindControl("txtSRPToAdd");

                using (SqlConnection sqlConn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["MyConnCSI"].ConnectionString))
                {
                    string stR = @"dbo.InsertIntoItemSoldToAdd";

                    using (SqlCommand cmD = new SqlCommand(stR, sqlConn))
                    {
                        sqlConn.Open();
                        cmD.CommandTimeout = 0;
                        cmD.CommandType = CommandType.StoredProcedure;
                        cmD.Parameters.AddWithValue("@TransactionType", 5);
                        cmD.Parameters.AddWithValue("@Sup_CategoryNum", 8);
                        cmD.Parameters.AddWithValue("@HeaderID", gvItemBatchToAddd.Rows[e.RowIndex].Cells[0].Text);
                        cmD.Parameters.AddWithValue("@Sup_ItemCode",ddITemToAdd.SelectedValue);
                        cmD.Parameters.AddWithValue("@vQtyPicked", textQtyPickedToAdd.Text);
                        cmD.Parameters.AddWithValue("@vUnitCost", textSRPToAdd.Text);
                        cmD.Parameters.AddWithValue("@vPickedBy", Session["UserFullName"].ToString());
                        cmD.Parameters.AddWithValue("@ReferenceNo", lblReferenceNo.Text);
                        cmD.Parameters.AddWithValue("@OrderDate", lblOrderDate.Text);
                        cmD.Parameters.AddWithValue("@Source", lblSource.Text);
                        cmD.Parameters.AddWithValue("@BrCode", Session["UserBranchCode"].ToString());
                        cmD.ExecuteNonQuery();

                    }
                }

                loadInTransitOrderEntry();

                

            }
        }

        protected void gvItems_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            if (IsPageRefresh == true)
            {
                Response.Redirect(Request.Url.AbsoluteUri);
            }
            else
            {
                try
                {
                    string RecNo = gvItems.DataKeys[e.RowIndex].Value.ToString();
                    string connString = System.Configuration.ConfigurationManager.ConnectionStrings["MyConnCSI"].ConnectionString;
                    string sql = @"Delete from ITEM_OnlineSold  where OrderID='" + RecNo + "'";
                    using (SqlConnection conn = new SqlConnection(connString))
                    {
                        conn.Open();

                        using (SqlCommand cmd = new SqlCommand(sql, conn))
                        {
                            cmd.CommandTimeout = 0;
                            cmd.ExecuteNonQuery();
                        }
                    }

                    gvItems.EditIndex = -1;
                    loadInTransitOrderEntry();
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