using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Reflection;
using System.Globalization;

namespace CSI
{
    public partial class StockUsedDataEntry : System.Web.UI.Page
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


            txtBalance.Text = string.Empty;
            txtBatchNumber.Text = string.Empty;
            txtDateExpiry.Text = string.Empty;
            txtID.Text = string.Empty;
            txtPatientName.Text = string.Empty;
            txtQty.Text = string.Empty;
            txtTransactionDate.Text = string.Empty;
            ddSeriesNo.DataSource = null;
            ddSeriesNo.Items.Clear();

        }

        protected void ddItem_SelectedIndexChanged(object sender, EventArgs e)
        {
            txtBalance.Text = string.Empty;
            txtBatchNumber.Text = string.Empty;
            txtDateExpiry.Text = string.Empty;
            txtID.Text = string.Empty;
            txtQty.Text = string.Empty;

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


        protected void txtTransactionDate_TextChanged(object sender, EventArgs e)
        {
            try
            {
                if (ddCategory.SelectedItem.Text == "Select Category")
                {
                    txtTransactionDate.Text = string.Empty;
                    ddCategory.Focus();
                    lblMsg.Text = "Please select category first.";
                    ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "Popup", "ShowSuccessMsg();", true);
                    return;
                }
                else
                {
                    string ValidDate = "01/01/2022";
                    //DateTime UsedDate = DateTime.Parse(txtTransactionDate.Text);
                    DateTime UsedDate = Convert.ToDateTime(txtTransactionDate.Text);

                    if (UsedDate <= DateTime.Parse(ValidDate))
                    {
                        txtTransactionDate.Text = string.Empty;
                        lblMsg.Text = "Transaction date before January 1, 2022 is not allowed";
                        ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "Popup", "ShowSuccessMsg();", true);
                        return;
                    }
                    else
                    {
                        //CheckIfPreviousMonthisAlreadyPosted();
                        checkIfAlreadyPosted();
                        //getSeries();
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


        private void checkIfAlreadyPosted()
        {
            int CurrMonthtoCheck = Convert.ToDateTime(txtTransactionDate.Text).Month;
            int theCurrYear = Convert.ToDateTime(txtTransactionDate.Text).Year;
            CultureInfo usEnglish2 = new CultureInfo("en-US");
            DateTimeFormatInfo englishInfo2 = usEnglish2.DateTimeFormat;
            string CurrmonthName = englishInfo2.MonthNames[CurrMonthtoCheck - 1];

            using (SqlConnection sqlConn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["MyConnCSI"].ConnectionString))
            {

                sqlConn.Open();
                //string stRinvCurr = @"SELECT sup_Yeardate,sup_Monthdate FROM sup_InventoryHeaderLE_BRhistory WHERE sup_Yeardate='" + Convert.ToDateTime(txtTransactionDate.Text).Year + "' AND sup_Monthdate='" + PrevMonth + "' and sup_BranchCode='" + Session["UserBranchCode"].ToString() + "'";
                string stR = @"SELECT * FROM [dbo].[ITEM_RecordsHeaderMonthly]  WHERE BrCode=@BrCode AND [iYear] = @CurrYr AND iMonth=@CurrMo AND Sup_CategoryNum=@Category";
                using (SqlCommand cmD = new SqlCommand(stR, sqlConn))
                {
                    cmD.Parameters.AddWithValue("@BrCode", Session["UserBranchCode"].ToString());
                    cmD.Parameters.AddWithValue("@Category", ddCategory.SelectedValue);
                    cmD.Parameters.AddWithValue("@CurrYr", theCurrYear);
                    cmD.Parameters.AddWithValue("@CurrMo", CurrMonthtoCheck);

                    SqlDataReader dRcurr = cmD.ExecuteReader();
                    if (dRcurr.HasRows)
                    {
                     //   txtTransactionDate.Text = string.Empty;
                        lblMsg.Text = "Inventory for the month of <b>" + CurrmonthName + " " + theCurrYear + "</b> already posted";
                        ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "Popup", "ShowSuccessMsg();", true);
                        return;

                    }
                    else
                    {

                        //lblMsg.Text = "checkIfNotPosted()";
                        //ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "Popup", "ShowSuccessMsg();", true);
                        //return;
                        checkIfNotPosted();
                    }
                }
            }
        }


        int PrevMonth;
        int TheYear;
        private void checkIfNotPosted()
        {

            if (Convert.ToDateTime(txtTransactionDate.Text).Month != 1)
            {
                PrevMonth = Convert.ToDateTime(txtTransactionDate.Text).Month - 1;

                CultureInfo usEnglish2 = new CultureInfo("en-US");
                DateTimeFormatInfo englishInfo2 = usEnglish2.DateTimeFormat;
                //PrevrmonthName2 = englishInfo2.MonthNames[PrevMonth - 1];
                TheYear = Convert.ToDateTime(txtTransactionDate.Text).Year;
            }
            else
            {
                PrevMonth = 12;
                //PrevrmonthName2 = "December";
                TheYear = Convert.ToDateTime(txtTransactionDate.Text).Year - 1;
            }

            using (SqlConnection conN = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["MyConnCSI"].ConnectionString))
            {
                string stR = @"SELECT * FROM [dbo].[ITEM_RecordsHeaderMonthly]  WHERE BrCode=@BrCode AND [iYear] = @Yr AND iMonth=@PrevMo AND Sup_CategoryNum=@Category";
                using (SqlCommand cmD = new SqlCommand(stR, conN))
                {
                    conN.Open();
                    cmD.CommandTimeout = 0;

                    cmD.Parameters.AddWithValue("@BrCode", Session["UserBranchCode"].ToString());
                    cmD.Parameters.AddWithValue("@Category", ddCategory.SelectedValue);
                    cmD.Parameters.AddWithValue("@Yr", TheYear);
                    cmD.Parameters.AddWithValue("@PrevMo", PrevMonth);

                    SqlDataReader dR = cmD.ExecuteReader();
                    if (dR.HasRows)
                    {
                        getSeries(); 
                    }
                    else
                    {
                        txtTransactionDate.Text = string.Empty;
                        lblMsg.Text = "Previous Month not yet Posted(Monthly Posting)<br />" + ddCategory.SelectedItem.Text + "<br /> Please go to setup Monthly Posting";
                        ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "Popup", "ShowSuccessMsg();", true);
                        return;
                    }
                }
            }
        }

        private void getSeries()
        {
            if (ddCategory.SelectedValue == "1" | ddCategory.SelectedValue == "4")
            {
                using (SqlConnection sqlConn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["MyConnCSI"].ConnectionString))
                {

                    string stR = @"dbo.getSeriesNoInfoinSMS";

                    using (SqlCommand cmD = new SqlCommand(stR, sqlConn))
                    {
                        sqlConn.Open();
                        cmD.CommandTimeout = 0;
                        cmD.CommandType = CommandType.StoredProcedure;
                        cmD.Parameters.AddWithValue("@SalesDate", txtTransactionDate.Text);
                        cmD.Parameters.AddWithValue("@BrCode", Session["UserBranchCode"].ToString());
                        DataTable dT = new DataTable();
                        SqlDataAdapter dA = new SqlDataAdapter(cmD);

                        dA.Fill(dT);


                        ddSeriesNo.Items.Clear();
                        ddSeriesNo.DataSource = dT;
                        ddSeriesNo.DataTextField = "ReceiptNo";
                        ddSeriesNo.DataValueField = "CustomerName";
                        ddSeriesNo.DataBind();
                        ddSeriesNo.Items.Insert(0, "Select Series No. in SMS");
                    }
                }
            }
            else
            {
                //ddSeriesNo.SelectedItem.Text = "Not Applicable";
                ddSeriesNo.Items.Insert(0, "Not Applicable");
            }
        }

        protected void ddSeriesNo_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddSeriesNo.SelectedItem.Text=="Select Series No. in SMS")
            {
                txtPatientName.Text = string.Empty;
            }
            else
            {
                txtPatientName.Text = ddSeriesNo.SelectedValue;
            }
        }

        protected void btnDhow_Click(object sender, EventArgs e)
        {
            GetItemBatchBranch(ddItem.SelectedValue);
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (IsPageRefresh == true)
            {
                Response.Redirect(Request.Url.AbsoluteUri);
            }
            else
            {
                //try
                //{
                    if (ddSeriesNo.SelectedItem.Text == "Select Series No. in SMS" & ddCategory.SelectedValue=="4" & ckHEoption.Checked==true)
                    {
                        lblMsg.Text = "Please Select Series No. in SMS(Mandatory if High End - Uncheck if not encoded in SMS)";
                        ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "Popup", "ShowSuccessMsg();", true);
                        return;
                    }
                    else
                    {
                        using (SqlConnection sqlConn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["MyConnCSI"].ConnectionString))
                        {
                            string stR = @"dbo.InsertIntoItemPickBranch";

                            using (SqlCommand cmD = new SqlCommand(stR, sqlConn))
                            {
                                sqlConn.Open();
                                cmD.CommandTimeout = 0;
                                cmD.CommandType = CommandType.StoredProcedure;
                                cmD.Parameters.AddWithValue("@BrCode", Session["UserBranchCode"].ToString());
                                if (ddSeriesNo.SelectedItem.Text == "Select Series No. in SMS")
                                {
                                    cmD.Parameters.AddWithValue("@ReceiptNo", "0");
                                }
                                else if (ddSeriesNo.SelectedItem.Text == string.Empty)
                                {
                                    cmD.Parameters.AddWithValue("@ReceiptNo", "0");
                                }
                                else if (ddSeriesNo.SelectedItem.Text == "Not Applicable")
                                {
                                    cmD.Parameters.AddWithValue("@ReceiptNo", "0");
                                }
                                else
                                {
                                    cmD.Parameters.AddWithValue("@ReceiptNo", ddSeriesNo.SelectedItem.Text);
                                }

                                cmD.Parameters.AddWithValue("@CustomerName", txtPatientName.Text);
                                cmD.Parameters.AddWithValue("@HeaderID", txtID.Text);
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
                //}

                //catch (Exception x)
                //{
                //    lblMsg.Text = x.GetType().ToString();
                //    ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "Popup", "ShowSuccessMsg();", true);
                //    return;
                //}

            }
        }


        private void loadUnpostedEntry()
        {
            Label3.Visible = true;
            try
            {
                using (SqlConnection sqlConn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["MyConnCSI"].ConnectionString))
                {
                    string stR = @"SELECT a.[vRecNum]
                                      ,a.[HeaderID]
                                      ,CASE WHEN a.[ReceiptNo]='Select Series No. in SMS' THEN ''
                                            WHEN a.[ReceiptNo]='Not Applicable' THEN ''
									  ELSE a.[ReceiptNo] END AS [ReceiptNo]
                                      ,CASE WHEN a.[CustomerName]='Not Applicable' THEN ''
                                        ELSE a.[CustomerName] END AS [CustomerName]
                                      ,a.[Sup_ItemCode]
	                                  ,b.sup_DESCRIPTION
                                      ,a.[vQtyPicked]
                                      ,a.[vBatchNo]
                                      ,a.[vDateExpiry]
                                      ,a.[vDateEncoded]
                                      ,a.vTrandate
                                      ,a.[vPickedBy]
                                  FROM [ITEM_PickBranch] a
                                  LEFT JOIN [Sup_ItemMaster] B
	                                ON a.[Sup_ItemCode]=B.Sup_ItemCode
                                    LEFT JOIN [ITEM_RecordsHeader] C
                                      ON a.HeaderID=C.HeaderID
                                  where a.[TransactionType]=1 and a.[vStat]=0
                                  and a.[BrCode]=@BrCode    and C.Sup_CategoryNum=@CategoryNum ORDER BY A.[Sup_ItemCode]";

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
            
            string RecNo = gvPicked.DataKeys[e.RowIndex].Value.ToString();
            lblConfirmRecID.Text = RecNo.ToString();
            lblItem.Text = gvPicked.Rows[e.RowIndex].Cells[5].Text;
            lblMsgConfirm.Text = "Are you sure you want to delete these item? ";
            ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "Popup", "ShowConfirmMsg();", true);
            return;

            //DeletePickedItem(e);
        }

        //private void DeletePickedItem(GridViewDeleteEventArgs e)
        //{
           
        //}

        protected void btnDeleteItem_Click(object sender, EventArgs e)
        {
            if (IsPageRefresh == true)
            {
                Response.Redirect(Request.Url.AbsoluteUri);
            }
            else
            {
                if (IsPageRefresh == true)
                {
                    Response.Redirect(Request.Url.AbsoluteUri);
                }
                else
                {
                    try
                    {
                        //string RecNo = gvPicked.DataKeys[e.RowIndex].Value.ToString();
                        string connString = System.Configuration.ConfigurationManager.ConnectionStrings["MyConnCSI"].ConnectionString;
                        string sql = @"Delete from ITEM_PickBranch  where vRecNum='" + lblConfirmRecID.Text + "'";
                        using (SqlConnection conn = new SqlConnection(connString))
                        {
                            conn.Open();

                            using (SqlCommand cmd = new SqlCommand(sql, conn))
                            {
                                cmd.CommandTimeout = 0;
                                cmd.ExecuteNonQuery();
                            }
                        }

                        lblConfirmRecID.Text = string.Empty;
                        lblItem.Text = string.Empty;
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
        }

        protected void btnPost_Click(object sender, EventArgs e)
        {
            if (IsPageRefresh == true)
            {
                Response.Redirect(Request.Url.AbsoluteUri);
            }
            else
            {
                if (gvPicked.Rows.Count == 0)
                {
                    lblMsg.Text = "No record to post";
                    ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "Popup", "ShowSuccessMsg();", true);
                    return;
                }
                else
                {
                    try
                    {

                        foreach (GridViewRow gvR in gvPicked.Rows)
                        {
                            //string RecNo = gvPicked.DataKeys[e.RowIndex].Value.ToString();
                            int Rec = Convert.ToInt32(gvPicked.DataKeys[gvR.RowIndex].Value.ToString());


                            if (gvR.RowType == DataControlRowType.DataRow)
                            {
                                using (SqlConnection sqlConn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["MyConnCSI"].ConnectionString))
                                {
                                    string stR = @"dbo.PostUnpostedEntry";

                                    using (SqlCommand cmD = new SqlCommand(stR, sqlConn))
                                    {
                                        sqlConn.Open();
                                        cmD.CommandTimeout = 0;
                                        cmD.CommandType = CommandType.StoredProcedure;
                                        cmD.Parameters.AddWithValue("@RecNum", Rec);
                                        cmD.Parameters.AddWithValue("@HeaderID", gvR.Cells[1].Text);
                                        cmD.Parameters.AddWithValue("@Qty", gvR.Cells[6].Text);
                                        try
                                        {
                                            cmD.ExecuteNonQuery();
                                        }
                                        catch (SqlException ex)
                                        {
                                            lblMsg.Text = ex.Message;
                                            ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "Popup", "ShowSuccessMsg();", true);
                                            return;
                                        }
                                    }
                                }

                            }
                        }

                        loadCategory();
                        txtTransactionDate.Text = string.Empty;
                        txtPatientName.Text = string.Empty;
                        ddSeriesNo.DataSource = null;
                        ddSeriesNo.Items.Clear();
                        loadUnpostedEntry();
                        ddItem.DataSource = null;
                        ddItem.Items.Clear();

                        lblMsg.Text = "Successfully Posted.";
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
}