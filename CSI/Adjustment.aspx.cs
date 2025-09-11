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
    public partial class Adjustment : System.Web.UI.Page
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
                    loadCategory();
                    
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

        private void loadCategory()
        {
            using (SqlConnection sqlConn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["MyConnCSI"].ConnectionString))
            {
                string stR = @"Select Sup_CategoryNum,Sup_CategoryName from Sup_Category order by Sup_CategoryName";
                sqlConn.Open();
                using (SqlCommand cmD = new SqlCommand(stR, sqlConn))
                {

                    SqlDataReader dR = cmD.ExecuteReader();
                    ddCategory.Items.Clear();
                    ddCategory.DataSource = dR;
                    ddCategory.DataValueField = "Sup_CategoryNum";
                    ddCategory.DataTextField = "Sup_CategoryName";
                    ddCategory.DataBind();
                    ddCategory.Items.Insert(0, new ListItem("Select Category", "0"));
                    ddCategory.SelectedIndex = 0;

                }
            }
        }

        private void loadItemSupplies()
        {
            using (SqlConnection sqlConn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["MyConnCSI"].ConnectionString))
            {

                string stR = @"SELECT DISTINCT a.sup_ItemCode,a.sup_ItemCode + ' - ' + b.sup_DESCRIPTION as [sup_DESCRIPTION] 
                                FROM [ITEM_RecordsHeader] a
                                LEFT JOIN Sup_ItemMaster b
                                ON a.sup_ItemCode=b.sup_ItemCode
                                WHERE a.BrCode=1 and a.Sup_CategoryNum=@Category
								ORDER BY sup_DESCRIPTION";

                using (SqlCommand cmD = new SqlCommand(stR, sqlConn))
                {
                    sqlConn.Open();
                    cmD.CommandTimeout = 0;
                    //cmD.CommandType = CommandType.StoredProcedure;
                    cmD.Parameters.AddWithValue("@Category", ddCategory.SelectedValue);
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

        protected void ddCategory_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddCategory.SelectedItem.Text == "Select Category")
            {


                ddItem.DataSource = null;
                ddItem.Items.Clear();

                txtBalance.Text = string.Empty;
                txtQty.Text = string.Empty;
            }
            else
            {
                loadUnpostedEntry();
                loadItemSupplies();
            }
        }

        protected void ddItem_SelectedIndexChanged(object sender, EventArgs e)
        {
            GetItemBatch(ddItem.SelectedValue);
        }


        private void GetItemBatch(string theItemCode)
        {
            if (IsPageRefresh == true)
            {
                Response.Redirect(Request.Url.AbsoluteUri);
            }
            else
            {
                try
                {
                    using (SqlConnection Conn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["MyConnCSI"].ConnectionString))
                    {


                        string stRGetBal = @"dbo.GetItemBatchesToAdjust";
                        using (SqlCommand cmD = new SqlCommand(stRGetBal, Conn))
                        {
                            Conn.Open();
                            cmD.CommandType = CommandType.StoredProcedure;
                            cmD.Parameters.AddWithValue("@ItemCode", ddItem.SelectedValue);
                            cmD.Parameters.AddWithValue("@CategoryType", ddCategory.SelectedValue);
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
                                lblMsg.Text = "No record found";
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
                try
                {
                    GridViewRow row = gvItemBatch.Rows[e.RowIndex];
                    //int RowIndex = row.RowIndex;
                    txtID.Text = row.Cells[0].Text;
                    txtBatchNumber.Text = row.Cells[2].Text;
                    txtDateExpiry.Text = row.Cells[3].Text;
                    txtBalance.Text = row.Cells[4].Text;


                }
                catch (Exception x)
                {
                    lblMsg.Text = x.Message;
                    ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "Popup", "ShowSuccessMsg();", true);
                    return;
                }
            }
        }






        protected void btnDhow_Click(object sender, EventArgs e)
        {
            GetItemBatch(ddItem.SelectedValue);
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                int theq = Convert.ToInt32(txtQty.Text);
                if (theq < 0)
                {
                    int abs1 = Math.Abs(theq);
                    if (abs1 > Convert.ToInt32(txtBalance.Text))
                    {
                        txtQty.Focus();
                        lblMsg.Text = "Insuficient balance";
                        ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "popup", "ShowSuccessMsg();", true);
                        return;
                    }
                    else
                    {
                        SaveAdjustment();
                    }
                }
                else
                {
                    SaveAdjustment();
                }
            }
            catch (Exception x)
            {
                txtQty.Focus();
                lblMsg.Text = x.Message;
                ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "popup", "ShowSuccessMsg();", true);
                return;

            }
        }

        private void SaveAdjustment()
        {
            if (IsPageRefresh == true)
            {
                Response.Redirect(Request.Url.AbsoluteUri);
            }
            else
            {
                try
                {
                    //DateTime theDate = DateTime.Now;
                    //theDate.ToString("yyyyMMdd", System.Globalization.CultureInfo.InvariantCulture);


                    using (SqlConnection sqlConn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["MyConnCSI"].ConnectionString))
                    {
                        //string stR = @"dbo.InsertIntoItemPickBranchAdjustment";
                        string stR = @"dbo.InsertIntoItemPickAdjustment";

                        using (SqlCommand cmD = new SqlCommand(stR, sqlConn))
                        {
                            sqlConn.Open();
                            cmD.CommandTimeout = 0;
                            cmD.CommandType = CommandType.StoredProcedure;
                            cmD.Parameters.AddWithValue("@OrderID", 0);
                            cmD.Parameters.AddWithValue("@TransactionType", 3);
                            cmD.Parameters.AddWithValue("@BrCode", Session["UserBranchCode"].ToString());
                            cmD.Parameters.AddWithValue("@BrName", Session["UserBranchName"].ToString());
                            //cmD.Parameters.AddWithValue("@Sup_ControlNo", theDate.ToString());
                            cmD.Parameters.AddWithValue("@HeaderID", txtID.Text);
                            cmD.Parameters.AddWithValue("@Sup_ItemCode",ddItem.SelectedValue);
                            cmD.Parameters.AddWithValue("@Sup_NoOfItemReq", 0);
                            cmD.Parameters.AddWithValue("@vQtyPicked",txtQty.Text);
                            cmD.Parameters.AddWithValue("@vBatchNo", txtBatchNumber.Text);
                            cmD.Parameters.AddWithValue("@vDateExpiry",txtDateExpiry.Text);
                            cmD.Parameters.AddWithValue("@vStat", 0);
                            cmD.Parameters.AddWithValue("@vPickedBy", Session["UserFullName"].ToString());

                            cmD.ExecuteNonQuery();

                        }

                        //using (SqlCommand cmD = new SqlCommand(stR, sqlConn))
                        //{
                        //    sqlConn.Open();
                        //    cmD.CommandTimeout = 0;
                        //    cmD.CommandType = CommandType.StoredProcedure;
                        //    cmD.Parameters.AddWithValue("@BrCode", Session["UserBranchCode"].ToString());
                        //    cmD.Parameters.AddWithValue("@HeaderID", txtID.Text);
                        //    cmD.Parameters.AddWithValue("@Sup_ItemCode", ddItem.SelectedValue);
                        //    cmD.Parameters.AddWithValue("@vQtyPicked", txtQty.Text);
                        //    cmD.Parameters.AddWithValue("@vBatchNo", txtBatchNumber.Text);
                        //    cmD.Parameters.AddWithValue("@vDateExpiry", txtDateExpiry.Text);
                        //    cmD.Parameters.AddWithValue("@vPickedBy", Session["UserFullName"].ToString());
                        //    cmD.Parameters.AddWithValue("@DateEncoded", txtTransactionDate.Text);
                        //    cmD.ExecuteNonQuery();

                        //}
                    }

                    loadItemSupplies();
                    txtID.Text = string.Empty;
                    txtBatchNumber.Text = string.Empty;
                    txtDateExpiry.Text = string.Empty;
                    txtQty.Text = string.Empty;
                    txtBalance.Text = string.Empty;

                    loadUnpostedEntry();
                }

                catch (Exception x)
                {
                    lblMsg.Text = x.GetType().ToString();
                    ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "Popup", "ShowSuccessMsg();", true);
                    return;
                }

            }
        }


        private void loadUnpostedEntry()
        {

            try
            {
                using (SqlConnection sqlConn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["MyConnCSI"].ConnectionString))
                {
                    string stR = @"SELECT a.[vRecNum],a.[HeaderID]
                                      ,a.Sup_ControlNo AS [TransactionNo]
                                      ,a.[Sup_ItemCode]
	                                  ,b.sup_DESCRIPTION
                                      ,a.[vQtyPicked]
                                      ,a.[vBatchNo]
                                      ,a.[vDateExpiry]
                                      ,a.[vTrandate]
                                      ,a.[vPickedBy]
                                  FROM [ITEM_Pick] a
                                  LEFT JOIN [Sup_ItemMaster] B
	                                ON a.[Sup_ItemCode]=B.Sup_ItemCode
                                    LEFT JOIN [ITEM_RecordsHeader] C
                                      ON a.HeaderID=C.HeaderID
                                  where a.[TransactionType]=3 and a.[vStat]=0
                                  and a.[BrCode]=@BrCode    and c.Sup_CategoryNum=@CategoryNum ORDER BY A.[Sup_ItemCode]";

                    using (SqlCommand cmD = new SqlCommand(stR, sqlConn))
                    {
                        sqlConn.Open();
                        cmD.CommandTimeout = 0;
                        cmD.Parameters.AddWithValue("@CategoryNum", ddCategory.SelectedValue);
                        cmD.Parameters.AddWithValue("@BrCode", Session["UserBranchCode"].ToString());
                        SqlDataAdapter dA = new SqlDataAdapter(cmD);
                        DataTable dT = new DataTable();
                        dA.Fill(dT);
                        gvPicked.DataSource = dT;
                        gvPicked.DataBind();
                    }
                }
            }

            catch (Exception y)
            {
                lblMsg.Text = y.GetType().ToString();
                ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "Popup", "ShowSuccessMsg();", true);
                return;
            }

        }


        protected void gvPickedItems_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            if (IsPageRefresh == true)
            {
                Response.Redirect(Request.Url.AbsoluteUri);
            }
            else
            {
                try
                {
                    string RecNo = gvPicked.DataKeys[e.RowIndex].Value.ToString();
                    string connString = System.Configuration.ConfigurationManager.ConnectionStrings["MyConnCSI"].ConnectionString;
                    string sql = @"Delete from ITEM_Pick  where vRecNum='" + RecNo + "'";
                    using (SqlConnection conn = new SqlConnection(connString))
                    {
                        conn.Open();

                        using (SqlCommand cmd = new SqlCommand(sql, conn))
                        {
                            cmd.CommandTimeout = 0;
                            cmd.ExecuteNonQuery();
                        }
                    }

                    loadUnpostedEntry();
                }
                catch (Exception y)
                {
                    lblMsg.Text = y.Message;
                    ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "Popup", "ShowSuccessMsg();", true);
                    return;
                }
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

                    foreach (GridViewRow gvR in gvPicked.Rows)
                    {
                        if (gvR.RowType == DataControlRowType.DataRow)
                        {
                            using (SqlConnection sqlConn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["MyConnCSI"].ConnectionString))
                            {
                                string stR = @"dbo.PostAdjustmentEntry";

                                using (SqlCommand cmD = new SqlCommand(stR, sqlConn))
                                {
                                    sqlConn.Open();
                                    cmD.CommandTimeout = 0;
                                    cmD.CommandType = CommandType.StoredProcedure;
                                    cmD.Parameters.AddWithValue("@RecNum", gvR.Cells[0].Text);
                                    cmD.Parameters.AddWithValue("@HeaderID", gvR.Cells[1].Text);
                                    cmD.Parameters.AddWithValue("@Qty", gvR.Cells[5].Text);
                                    cmD.ExecuteNonQuery();
                                }
                            }

                        }
                    }

                    loadCategory();
                    txtTransactionDate.Text = string.Empty;
                    ddItem.DataSource = null;
                    ddItem.Items.Clear();
                    loadUnpostedEntry();

                    lblMsg.Text = "Adjustment successfully posted.";
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