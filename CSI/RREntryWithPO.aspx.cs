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
    public partial class RREntryWithPO : System.Web.UI.Page
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

                    txtRRNumber.Attributes.Add("onfocus", "this.select()");
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

        protected void btnLoadDetail_Click(object sender, EventArgs e)
        {
            if (IsPageRefresh == true)
            {
                Response.Redirect(Request.Url.AbsoluteUri);
            }
            else
            {
                try
                {
                    using (SqlConnection conN = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["MyConnCSI"].ConnectionString))
                    {
                        string stR1 = @"dbo.LoaddRRFromPurchasing";


                        using (SqlCommand cmD = new SqlCommand(stR1, conN))
                        {
                            conN.Open();
                            cmD.CommandTimeout = 0;
                            cmD.CommandType = CommandType.StoredProcedure;
                            cmD.Parameters.AddWithValue("@RRNo", txtRRNumber.Text);
                            SqlDataAdapter dA = new SqlDataAdapter(cmD);
                            DataTable dT = new DataTable();
                            dA.Fill(dT);
                            if (dT.Rows.Count > 0)
                            {
                                lblSupplier.Text = dT.Rows[0]["PO_Supplier"].ToString();
                                lblPONumber.Text = dT.Rows[0]["PONo"].ToString();
                                lblRRDate.Text = Convert.ToDateTime(dT.Rows[0]["RRDate"].ToString()).ToShortDateString();

                                gvRR.DataSource = dT;
                                gvRR.DataBind();
                            }

                            else
                            {
                                lblPONumber.Text = string.Empty;
                                lblRRDate.Text = string.Empty;
                                lblSupplier.Text = string.Empty;
                                gvRR.DataSource = null;
                                gvRR.DataBind();

                                txtRRNumber.Focus();
                                lblMsg.Text = "RR Number does not exists or already posted!";
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

        protected void gvRR_RowEditing(object sender, GridViewEditEventArgs e)
        {
            gvRR.EditIndex = e.NewEditIndex;
            loadRRInfo();
        }

        protected void gvRR_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            gvRR.EditIndex = -1;
            loadRRInfo();
        }

        protected void gvRR_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {


            GridViewRow row = gvRR.Rows[e.RowIndex];
            DropDownList dItem = (DropDownList)row.FindControl("ddItem");

            string connString = System.Configuration.ConfigurationManager.ConnectionStrings["MyConnCSI"].ConnectionString;

            string sql = @"Update Sup_ItemMaster set sup_PurchasingCode=@PurchasingCode where sup_ItemCode = @sup_ItemCode";
            using (SqlConnection conn = new SqlConnection(connString))
            {
                conn.Open();

                using (SqlCommand cmD = new SqlCommand(sql, conn))
                {
                    cmD.CommandTimeout = 0;
                    cmD.Parameters.AddWithValue("@sup_ItemCode", dItem.SelectedValue);
                    cmD.Parameters.AddWithValue("@PurchasingCode", row.Cells[2].Text);
                    cmD.ExecuteNonQuery();
                }
            }

            gvRR.EditIndex = -1;
            loadRRInfo();

        }

        



        private void loadRRInfo()
        {
            using (SqlConnection conN = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["MyConnCSI"].ConnectionString))
            {
                string stR = @"dbo.LoaddRRFromPurchasing";


                using (SqlCommand cmD = new SqlCommand(stR, conN))
                {
                    conN.Open();
                    cmD.CommandTimeout = 0;
                    cmD.CommandType = CommandType.StoredProcedure;
                    cmD.Parameters.AddWithValue("@RRNo", txtRRNumber.Text);
                    SqlDataAdapter dA = new SqlDataAdapter(cmD);
                    DataTable dT = new DataTable();
                    dA.Fill(dT);
                    gvRR.DataSource = dT;
                    gvRR.DataBind();
             
                }
            }
        }

        protected void gvRR_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            
                if (Convert.ToString(DataBinder.Eval(e.Row.DataItem, "ItemCode")) == "Assign Code")
                {
                    if (e.Row.RowType == DataControlRowType.DataRow)
                    {
                        //((LinkButton)e.Row.FindControl("btnUpdate")).ForeColor = Color.Gray;
                        ((LinkButton)e.Row.FindControl("lnkPOST")).Enabled = false;
                        ((LinkButton)e.Row.FindControl("lnkPOST")).Text = string.Empty;

                    }
                }
            


            if (e.Row.RowType == DataControlRowType.DataRow && (e.Row.RowState & DataControlRowState.Edit) == DataControlRowState.Edit)
            {
                // Here you will get the Control you need like:
                DropDownList dlItem = (DropDownList)e.Row.FindControl("ddItem");

                using (SqlConnection sqlConn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["MyConnCSI"].ConnectionString))
                {

                    string stR = @"SELECT DISTINCT sup_ItemCode,sup_ItemCode + ' - ' + sup_DESCRIPTION as [sup_DESCRIPTION] 
                                FROM  Sup_ItemMaster
								ORDER BY sup_DESCRIPTION";

                    using (SqlCommand cmD = new SqlCommand(stR, sqlConn))
                    {
                        sqlConn.Open();
                        cmD.CommandTimeout = 0;
                        DataTable dT = new DataTable();
                        SqlDataAdapter dA = new SqlDataAdapter(cmD);
                        dA.Fill(dT);
                        dlItem.Items.Clear();
                        dlItem.DataSource = dT;
                        dlItem.DataTextField = "sup_DESCRIPTION";
                        dlItem.DataValueField = "sup_ItemCode";
                        dlItem.DataBind();
                        dlItem.Items.Insert(0, new ListItem("Please select item", "0"));
                        dlItem.SelectedIndex = 0;
                    }
                }

            }
        }


        

        //private void PostPO(GridViewCommandEventArgs e)
        //{
            
        //        using (SqlConnection sqlConn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["MyConnCSI"].ConnectionString))
        //        {
        //            string stR = "dbo.PostUnpostedPO";

        //            sqlConn.Open();

        //            using (SqlCommand cmD = new SqlCommand(stR, sqlConn))
        //            {
        //                cmD.CommandTimeout = 0;
        //                cmD.CommandType = CommandType.StoredProcedure;
        //                //cmD.Parameters.AddWithValue("@ID", Convert.ToInt32(Rec));
        //                cmD.Parameters.AddWithValue("@EncodedBy", Session["UserFullName"].ToString());
        //                cmD.ExecuteNonQuery();

        //            }
        //        }


        //        loadRRInfo();
            
        //}

        


        protected void gvRR_RowCommand(object sender, GridViewCommandEventArgs e)
        {


            if (e.CommandName == "Post")
            {
                GridViewRow gvr = (GridViewRow)(((LinkButton)e.CommandSource).NamingContainer);
                int RowIndex = gvr.RowIndex;
                TextBox textDateExpiry = (TextBox)gvr.FindControl("txtDateExpiry");
                TextBox textBatchNo = (TextBox)gvr.FindControl("txtBatchNo");
                Label labelQuantity = (Label)gvr.FindControl("lblQuantity");
                Label labelPO_UOP = (Label)gvr.FindControl("lblPO_UOP");
                Label labelRR_Amount = (Label)gvr.FindControl("lblRR_Amount");
                Label labelCode = (Label)gvr.FindControl("lblCode");
                TextBox txtActualQty = (TextBox)gvr.FindControl("txtActualQty");
                TextBox txtUnitCost = (TextBox)gvr.FindControl("txtUnitCost");
                
                if (textDateExpiry.Text == string.Empty)
                {

                    lblConfirmDetailID.Text = gvr.Cells[0].Text;
                    lblConfirmRRNo.Text = gvr.Cells[1].Text;
                    lblConfirmPurchasingCode.Text = gvr.Cells[2].Text;
                    lblItem.Text = gvr.Cells[3].Text;
                    lblConfirmItemCode.Text = labelCode.Text;
                    lblConfirmUnitCost.Text = labelPO_UOP.Text;
                    lblConfirmQtyReceived.Text = txtActualQty.Text;
                    lblConfirmTotalAmount.Text=labelRR_Amount.Text;
                    lblConfirmBatchNo.Text =textBatchNo.Text;
                    lblConfirmateExpiry.Text = textDateExpiry.Text;
                    lblRetailPrice.Text = txtUnitCost.Text;
                    lblOrigQuantity.Text = labelQuantity.Text;
                    lblMsgConfirm.Text = "Are you sure you want to post this item without Date Expiry? ";
                    ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "Popup", "ShowConfirmMsg();", true);
                    return;
                }
                else
                {
                    using (SqlConnection sqlConn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["MyConnCSI"].ConnectionString))
                    {
                        string stR = @"dbo.InsertITEM_RREntryWithPO";

                        using (SqlCommand cmD = new SqlCommand(stR, sqlConn))
                        {
                            sqlConn.Open();
                            cmD.CommandTimeout = 0;
                            cmD.CommandType=CommandType.StoredProcedure;
                            cmD.Parameters.AddWithValue("@RRDetailID",gvr.Cells[0].Text);
                            cmD.Parameters.AddWithValue("@RRNo",gvr.Cells[1].Text);
                            cmD.Parameters.AddWithValue("@RRDate",lblRRDate.Text);
                            cmD.Parameters.AddWithValue("@RRSupplier",lblSupplier.Text);
                            cmD.Parameters.AddWithValue("@PONo",lblPONumber.Text);
                            cmD.Parameters.AddWithValue("@sup_PurchasingCode",gvr.Cells[2].Text);
                            cmD.Parameters.AddWithValue("@sup_ItemCode", labelCode.Text);
                            cmD.Parameters.AddWithValue("@sup_DESCRIPTION",gvr.Cells[3].Text);
                            cmD.Parameters.AddWithValue("@sup_UnitCost",labelPO_UOP.Text);
                            cmD.Parameters.AddWithValue("@sup_RetailCost", txtUnitCost.Text);
                            cmD.Parameters.AddWithValue("@POQuantity", labelQuantity.Text);
                            cmD.Parameters.AddWithValue("@vQtyReceived", txtActualQty.Text);
                            cmD.Parameters.AddWithValue("@TotalAmount",labelRR_Amount.Text);
                            cmD.Parameters.AddWithValue("@vBatchNo",textBatchNo.Text);
                            cmD.Parameters.AddWithValue("@vDateExpiry",textDateExpiry.Text);
                            cmD.Parameters.AddWithValue("@EncodedBy",Session["UserFullName"].ToString());
                            cmD.ExecuteNonQuery();
                        }
                    }
                }


                loadRRInfo();
                lblMsg.Text = gvr.Cells[3].Text + " Successfully Send to For Posting";
                ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "Popup", "ShowSuccessMsg();", true);
                return;

            }



        }


//        private void PostPOwithoutExpiry()
//        {


//                        using (SqlConnection sqlConn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["MyConnCSI"].ConnectionString))
//                        {
//                            string stR = @"Update [ITEM_Pick] set vStat=2,DRNumber=@DRNumber,
//                                    DateDelivered=@DateDelivered,DatePostedDelivered=@DatePostedDelivered,DeliveredBy=@DeliveredBy
//                                        where vRecNum = @RecNum";

//                            using (SqlCommand cmD = new SqlCommand(stR, sqlConn))
//                            {
//                                sqlConn.Open();
//                                cmD.CommandTimeout = 0;
//                                cmD.Parameters.AddWithValue("@RecNum", row.Cells[1].Text);
//                                cmD.Parameters.AddWithValue("@RRDetailID",);
//                                   cmD.Parameters.AddWithValue("@RRNo",);
//                                   cmD.Parameters.AddWithValue("@RRDate",);
//                                   cmD.Parameters.AddWithValue("@RRSupplier",);
//                                   cmD.Parameters.AddWithValue("@PONo",);
//                                   cmD.Parameters.AddWithValue("@sup_PurchasingCode",);
//                                   cmD.Parameters.AddWithValue("@sup_ItemCode",);
//                                   cmD.Parameters.AddWithValue("@sup_DESCRIPTION",);
//                                   cmD.Parameters.AddWithValue("@sup_UnitCost",);
//                                   cmD.Parameters.AddWithValue("@vQtyReceived",);
//                                   cmD.Parameters.AddWithValue("@TotalAmount",);
//                                   cmD.Parameters.AddWithValue("@vBatchNo",);
//                                   cmD.Parameters.AddWithValue("@vDateExpiry",);
//                                   cmD.Parameters.AddWithValue("@EncodedBy",);
//                                cmD.ExecuteNonQuery();
//                            }
//                        }

//        }
        protected void btnPostPO_Click(object sender, EventArgs e)
        {
            if (IsPageRefresh == true)
            {
                Response.Redirect(Request.Url.AbsoluteUri);
            }
            else
            {
                using (SqlConnection sqlConn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["MyConnCSI"].ConnectionString))
                {
                    string stR = @"dbo.InsertITEM_RREntryWithPO";

                    using (SqlCommand cmD = new SqlCommand(stR, sqlConn))
                    {
                        sqlConn.Open();
                        cmD.CommandTimeout = 0;
                        cmD.CommandType = CommandType.StoredProcedure;
                        cmD.Parameters.AddWithValue("@RRDetailID", lblConfirmDetailID.Text);
                        cmD.Parameters.AddWithValue("@RRNo", lblConfirmRRNo.Text);
                        cmD.Parameters.AddWithValue("@RRDate", lblRRDate.Text);
                        cmD.Parameters.AddWithValue("@RRSupplier", lblSupplier.Text);
                        cmD.Parameters.AddWithValue("@PONo", lblPONumber.Text);
                        cmD.Parameters.AddWithValue("@sup_PurchasingCode", lblConfirmPurchasingCode.Text);
                        cmD.Parameters.AddWithValue("@sup_ItemCode", lblConfirmItemCode.Text);
                        cmD.Parameters.AddWithValue("@sup_DESCRIPTION", lblItem.Text);
                        cmD.Parameters.AddWithValue("@sup_UnitCost", lblConfirmUnitCost.Text);
                        cmD.Parameters.AddWithValue("@sup_RetailCost", lblRetailPrice.Text);
                        cmD.Parameters.AddWithValue("@POQuantity", lblOrigQuantity.Text);
                        cmD.Parameters.AddWithValue("@vQtyReceived", lblConfirmQtyReceived.Text);
                        cmD.Parameters.AddWithValue("@TotalAmount", lblConfirmTotalAmount.Text);
                        cmD.Parameters.AddWithValue("@vBatchNo", lblConfirmBatchNo.Text);
                        cmD.Parameters.AddWithValue("@vDateExpiry", lblConfirmateExpiry.Text);
                        cmD.Parameters.AddWithValue("@EncodedBy", Session["UserFullName"].ToString());
                        cmD.ExecuteNonQuery();
                    }
                }


                loadRRInfo();
                lblMsg.Text = lblItem.Text + " Successfully Send to For Posting";
                ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "Popup", "ShowSuccessMsg();", true);
                return;

            }
        }
    }
}