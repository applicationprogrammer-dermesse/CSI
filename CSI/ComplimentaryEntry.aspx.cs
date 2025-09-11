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
    public partial class ComplimentaryEntry : System.Web.UI.Page
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
                    loadCompliNo();
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
                string stR = @"Select Sup_CategoryNum,Sup_CategoryName from Sup_Category WHERE Sup_CategoryNum in ('1','2','3','4','5')";
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

                string stR = @"dbo.LoadItemMasterBranch";

                using (SqlCommand cmD = new SqlCommand(stR, sqlConn))
                {
                    sqlConn.Open();
                    cmD.CommandTimeout = 0;
                    cmD.CommandType = CommandType.StoredProcedure;
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
            GetItemBatchBranch(ddItem.SelectedValue);
        }


        private void GetItemBatchBranch(string theItemCode)
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


                        string stRGetBal = @"GetItemBatchesBranch";
                        using (SqlCommand cmD = new SqlCommand(stRGetBal, Conn))
                        {
                            Conn.Open();
                            cmD.CommandType = CommandType.StoredProcedure;
                            cmD.Parameters.AddWithValue("@ItemCode", theItemCode);
                            cmD.Parameters.AddWithValue("@BrCode", Session["UserBranchCode"].ToString());
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
                                lblMsg.Text = "No Available Balance!";
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
            GetItemBatchBranch(ddItem.SelectedValue);
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {


            SaveComplimentary();

        }

        private void SaveComplimentary()
        {
            if (IsPageRefresh == true)
            {
                Response.Redirect(Request.Url.AbsoluteUri);
            }
            else
            {
                try
                {
                    using (SqlConnection sqlConn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["MyConnCSI"].ConnectionString))
                    {
                        string stR = @"dbo.InsertIntoItemPickBranchCompli";

                        using (SqlCommand cmD = new SqlCommand(stR, sqlConn))
                        {
                            sqlConn.Open();
                            cmD.CommandTimeout = 0;
                            cmD.CommandType = CommandType.StoredProcedure;
                            cmD.Parameters.AddWithValue("@BrCode", Session["UserBranchCode"].ToString());
                            cmD.Parameters.AddWithValue("@HeaderID", txtID.Text);
                            cmD.Parameters.AddWithValue("@ReceiptNo", txtComplimentaryNumber.Text);
                            cmD.Parameters.AddWithValue("@CustomerName", txtCustomerName.Text.ToUpper());
                            cmD.Parameters.AddWithValue("@Sup_ItemCode", ddItem.SelectedValue);
                            cmD.Parameters.AddWithValue("@vQtyPicked", txtQty.Text);
                            cmD.Parameters.AddWithValue("@vBatchNo", txtBatchNumber.Text);
                            cmD.Parameters.AddWithValue("@vDateExpiry", txtDateExpiry.Text);
                            cmD.Parameters.AddWithValue("@vPickedBy", Session["UserFullName"].ToString());
                            cmD.Parameters.AddWithValue("@DateEncoded", txtTransactionDate.Text);
                            cmD.ExecuteNonQuery();

                        }
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
                    string stR = @"SELECT a.[vRecNum]
                                      ,a.[HeaderID]
                                      ,a.[ReceiptNo]
                                      ,a.[CustomerName]
                                      ,a.[Sup_ItemCode]
	                                  ,b.sup_DESCRIPTION
                                      ,a.[vQtyPicked]
                                      ,a.[vBatchNo]
                                      ,a.[vDateExpiry]
                                      ,a.[vTrandate]
                                      ,a.[vPickedBy]
                                  FROM [ITEM_PickBranch] a
                                  LEFT JOIN [Sup_ItemMaster] B
	                                ON a.[Sup_ItemCode]=B.Sup_ItemCode
                                    LEFT JOIN [ITEM_RecordsHeader] C
                                      ON a.HeaderID=C.HeaderID
                                  where a.[TransactionType]=4 and a.[vStat]=0
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
                    string sql = @"Delete from ITEM_PickBranch  where vRecNum='" + RecNo + "'";
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
                                string stR = @"dbo.SubmitComplimentaryEntry";

                                using (SqlCommand cmD = new SqlCommand(stR, sqlConn))
                                {
                                    sqlConn.Open();
                                    cmD.CommandTimeout = 0;
                                    cmD.CommandType = CommandType.StoredProcedure;
                                    cmD.Parameters.AddWithValue("@RecNum", gvR.Cells[0].Text);
                                    cmD.ExecuteNonQuery();
                                }
                            }

                        }
                    }

                    loadCategory();
                    UpdateCompliNo();
                    //txtComplimentaryNumber.Text = string.Empty;
                    txtTransactionDate.Text = string.Empty;
                    ddItem.DataSource = null;
                    ddItem.Items.Clear();
                    loadUnpostedEntry();

                    lblMsg.Text = "Successfully send to Logistics for posting.";
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

        private void loadCompliNo()
        {
            using (SqlConnection conN = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["MyConnCSI"].ConnectionString))
            {
                string stR = @"SELECT CONCAT('Compli-',[BrCode],'-',YEAR([CurrentDate]),'-',RIGHT('0000000'+CAST(ComplimentaryNo AS VARCHAR(7)),7)) FROM SystemMaster where [BrCode]='" + Session["UserBranchCode"].ToString() + "'";
                using (SqlCommand cmD = new SqlCommand(stR, conN))
                {
                    conN.Open();
                    SqlDataReader dR = cmD.ExecuteReader();
                    while (dR.Read())
                    {
                        txtComplimentaryNumber.Text = dR[0].ToString();
                    }

                }
            }
    
        } 


        private void UpdateCompliNo()
        {
            using (SqlConnection sqlConn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["MyConnCSI"].ConnectionString))
            {
                string stR = @"update SystemMaster set ComplimentaryNo = ComplimentaryNo + 1 where BrCode=@BrCode";

                using (SqlCommand cmD = new SqlCommand(stR, sqlConn))
                {
                    sqlConn.Open();
                    cmD.CommandTimeout = 0;
                    //cmD.CommandType = CommandType.StoredProcedure;
                    cmD.Parameters.AddWithValue("@BrCode", Session["UserBranchCode"].ToString());
                    cmD.ExecuteNonQuery();
                }
            }

            loadCompliNo();
        }


    }

}