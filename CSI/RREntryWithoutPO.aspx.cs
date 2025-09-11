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
    public partial class RREntryWithoutPO : System.Web.UI.Page
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
                    loadItemSupplies();
                    loadSuppliers();
                    loadUnpostedEntry();

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


        private void loadItemSupplies()
        {
            using (SqlConnection sqlConn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["MyConnCSI"].ConnectionString))
            {

                string stR = @"SELECT DISTINCT sup_ItemCode,sup_ItemCode + ' - ' + sup_DESCRIPTION as [sup_DESCRIPTION] 
                                FROM Sup_ItemMaster where sup_Itemstat=1
								ORDER BY sup_DESCRIPTION";

                using (SqlCommand cmD = new SqlCommand(stR, sqlConn))
                {
                    sqlConn.Open();
                    cmD.CommandTimeout = 0;
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
                }
            }
        }

        private void loadSuppliers()
        {
            using (SqlConnection sqlConn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["MyConnPUR"].ConnectionString))
            {

                string stR = @"SELECT DISTINCT SuppCode,SuppName 
                                FROM [SUPPLIER]
                                WHERE SuppName IS NOT NULL AND SuppName<>''
								ORDER BY SuppName";

                using (SqlCommand cmD = new SqlCommand(stR, sqlConn))
                {
                    sqlConn.Open();
                    cmD.CommandTimeout = 0;
                    DataTable dT = new DataTable();
                    SqlDataAdapter dA = new SqlDataAdapter(cmD);
                    dA.Fill(dT);
                    ddSupplier.Items.Clear();
                    ddSupplier.DataSource = dT;
                    ddSupplier.DataTextField = "SuppName";
                    ddSupplier.DataValueField = "SuppCode";
                    ddSupplier.DataBind();
                    ddSupplier.Items.Insert(0, new ListItem("Please select supplier", "0"));
                    ddSupplier.SelectedIndex = 0;
                }
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                using (SqlConnection sqlConn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["MyConnCSI"].ConnectionString))
                {

                    string stR = @"dbo.SaveRREntry";

                    using (SqlCommand cmD = new SqlCommand(stR, sqlConn))
                    {
                        sqlConn.Open();
                        cmD.CommandTimeout = 0;
                        cmD.CommandType = CommandType.StoredProcedure;
                        cmD.Parameters.AddWithValue("@RRNo", txtRRNumber.Text.ToUpper());
                        cmD.Parameters.AddWithValue("@RRDate", txtDateReceived.Text);
                        cmD.Parameters.AddWithValue("@RRSupplier", ddSupplier.SelectedItem.Text);
                        cmD.Parameters.AddWithValue("@sup_ItemCode", ddItem.SelectedValue);
                        cmD.Parameters.AddWithValue("@sup_DESCRIPTION", ddItem.SelectedItem.Text);
                        cmD.Parameters.AddWithValue("@sup_UnitCost", txtUnitCost.Text);
                        cmD.Parameters.AddWithValue("@vQtyReceived", txtQty.Text);
                        cmD.Parameters.AddWithValue("@vBatchNo", txtBatchNumber.Text.TrimEnd());
                        cmD.Parameters.AddWithValue("@vDateExpiry", txtDateExpiry.Text);

                        cmD.Parameters.AddWithValue("@TotalAmount", txtTotalAmount.Text);
		                cmD.Parameters.AddWithValue("@RetailQuantity", txtRetailQty.Text);
                        cmD.Parameters.AddWithValue("@sup_RetailCost", txtRetailPrice.Text);
                        cmD.ExecuteNonQuery();


                    }
                }

                loadItemSupplies();
                txtBatchNumber.Text = string.Empty;
                txtDateExpiry.Text = string.Empty;
                txtQty.Text = string.Empty;
                txtUnitCost.Text = string.Empty;
                txtRetailPrice.Text = string.Empty;
                txtRetailQty.Text = string.Empty;
                txtTotalAmount.Text = string.Empty;
                loadUnpostedEntry();
            }
            catch (Exception x)
            {
                lblMsg.Text = x.Message;
                ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "Popup", "ShowSuccessMsg();", true);
                return;
            }

        }


        private void loadUnpostedEntry()
        {
            using (SqlConnection sqlConn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["MyConnCSI"].ConnectionString))
            {

                string stR = @"SELECT [RRNo]
                               ,[RRDate]
                               ,[RRSupplier]
                               ,[sup_ItemCode]
                               ,[sup_DESCRIPTION]
                               ,[sup_UnitCost]
                               ,[vQtyReceived]
                               ,TotalAmount
		                       ,RetailQuantity
			                    ,sup_RetailCost
                               ,LTRIM(RTRIM([vBatchNo])) AS [vBatchNo]
                               ,[vDateExpiry] 
                               ,RecNum
                                FROM [ITEM_RREntryWithoutPO] WHERE vStat=0";

                using (SqlCommand cmD = new SqlCommand(stR, sqlConn))
                {
                    sqlConn.Open();
                    cmD.CommandTimeout = 0;
                    //cmD.CommandType = CommandType.StoredProcedure;
                    DataTable dT = new DataTable();
                    SqlDataAdapter dA = new SqlDataAdapter(cmD);
                    dA.Fill(dT);

                    gvItems.DataSource = dT;
                    gvItems.DataBind();

                }
            }
        }

        protected void ddItem_SelectedIndexChanged(object sender, EventArgs e)
        {
            txtBatchNumber.Text = string.Empty;
            txtDateExpiry.Text = string.Empty;
            txtQty.Text = string.Empty;
            txtUnitCost.Text = string.Empty;
        }

        protected void gvItems_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            gvItems.EditIndex = -1;
            loadUnpostedEntry();
        }

        protected void gvItems_RowEditing(object sender, GridViewEditEventArgs e)
        {
            gvItems.EditIndex = e.NewEditIndex;
            loadUnpostedEntry();
        }

        protected void gvItems_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            try
            {
                string TheID = gvItems.DataKeys[e.RowIndex].Value.ToString();

                string connString = System.Configuration.ConfigurationManager.ConnectionStrings["MyConnCSI"].ConnectionString;

                string sql = @"DELETE FROM ITEM_RREntryWithoutPO where RecNum = @TheID";
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

            loadUnpostedEntry();
        }

        protected void gvItems_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            try
            {
                string TheRecNum = gvItems.DataKeys[e.RowIndex].Value.ToString();

                GridViewRow row = gvItems.Rows[e.RowIndex];
                TextBox txtrQuantity = (TextBox)row.FindControl("txtvQuantity");
                TextBox txtrUnitCost = (TextBox)row.FindControl("txtvUnitCost");
                TextBox txtrBatchNo = (TextBox)row.FindControl("txtvBatchNo");
                TextBox txtrDateExpiry = (TextBox)row.FindControl("txtvDateExpiry");

                TextBox txtrTotalAmount = (TextBox)row.FindControl("txtvTotalAmount");
                TextBox txtrRetailQuantity = (TextBox)row.FindControl("txtvRetailQuantity");
                TextBox txtrsup_RetailCost = (TextBox)row.FindControl("txtvsup_RetailCost");

                string connString = System.Configuration.ConfigurationManager.ConnectionStrings["MyConnCSI"].ConnectionString;

                string sql = @"Update ITEM_RREntryWithoutPO  set vQtyReceived=@vQtyReceived
                        ,sup_UnitCost=@sup_UnitCost
                          ,vBatchNo=@vBatchNo
                            ,vDateExpiry=@vDateExpiry
                            ,TotalAmount=@TotalAmount
		                   ,RetailQuantity=@RetailQuantity
			                ,sup_RetailCost=@sup_RetailCost

                        where RecNum = @RecNum";
                using (SqlConnection conn = new SqlConnection(connString))
                {
                    conn.Open();

                    using (SqlCommand cmD = new SqlCommand(sql, conn))
                    {
                        cmD.CommandTimeout = 0;
                        cmD.Parameters.AddWithValue("@RecNum", TheRecNum.ToString());
                        cmD.Parameters.AddWithValue("@sup_UnitCost",txtrUnitCost.Text);
                        cmD.Parameters.AddWithValue("@vQtyReceived",txtrQuantity.Text);
                        cmD.Parameters.AddWithValue("@vBatchNo",txtrBatchNo.Text);
                        cmD.Parameters.AddWithValue("@vDateExpiry", txtrDateExpiry.Text);

                        cmD.Parameters.AddWithValue("@TotalAmount", txtrTotalAmount.Text);
		                cmD.Parameters.AddWithValue("@RetailQuantity", txtrRetailQuantity.Text);
                        cmD.Parameters.AddWithValue("@sup_RetailCost", txtrsup_RetailCost.Text);
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
            loadUnpostedEntry();
        }

        protected void btnPost_Click(object sender, EventArgs e)
        {
            if (gvItems.Rows.Count == 0)
            {
                lblMsg.Text = "No record to post.";
                ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "Popup", "ShowSuccessMsg();", true);
                return;
            }
            else
            {
                PostRRentry();
            }
        }

        private void PostRRentry()
        {
            if (IsPageRefresh == true)
            {
                Response.Redirect(Request.Url.AbsoluteUri);
            }
            else
            {
                try
                {

                    foreach (GridViewRow gvR in gvItems.Rows)
                    {
                        if (gvR.RowType == DataControlRowType.DataRow)
                        {


                            string Rec = gvItems.DataKeys[gvR.RowIndex].Value.ToString();
                            using (SqlConnection sqlConn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["MyConnCSI"].ConnectionString))
                            {
                                string stR = "dbo.SubmitUnpostedRREntryWithoutPO";

                                sqlConn.Open();

                                using (SqlCommand cmD = new SqlCommand(stR, sqlConn))
                                {
                                    cmD.CommandTimeout = 0;
                                    cmD.CommandType = CommandType.StoredProcedure;

                                    cmD.Parameters.AddWithValue("@ID", Convert.ToInt32(Rec));
                                    cmD.Parameters.AddWithValue("@EncodedBy", Session["UserFullName"].ToString());
                                    cmD.ExecuteNonQuery();

                                }
                            }

                        }
                    }


                    gvItems.DataSource = null;
                    gvItems.DataBind();
                    lblMsg.Text = "RR entry succesfully send to for posting.";
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

    }
}