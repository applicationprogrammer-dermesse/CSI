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
    public partial class ListStockTransfer : System.Web.UI.Page
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
                    loadBranch();

                    if (Session["UserBranchCode"].ToString() == "1" || Session["UserID"].ToString() == "04657")
                    {
                        btnPost.Enabled = true;
                    }
                    else
                    {
                        btnPost.Enabled = false;
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

        private void loadBranch()
        {
            using (SqlConnection sqlConn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["MyConnCSI"].ConnectionString))
            {
                string stR = @"SELECT DISTINCT a.TargetBRCode,b.BrName
			                          FROM [ITEM_PickBranch] a
			                          LEFT JOIN MyBranchList b
			                          ON a.[TargetBRCode]=b.[BrCode] WHERE a.[vStat]=1 AND TransactionType=2 and a.TargetBRCode <> 1 ORDER BY b.BrName";
                sqlConn.Open();
                using (SqlCommand cmD = new SqlCommand(stR, sqlConn))
                {

                    SqlDataReader dR = cmD.ExecuteReader();
                    ddBranch.Items.Clear();
                    ddBranch.DataSource = dR;
                    ddBranch.DataValueField = "TargetBRCode";
                    ddBranch.DataTextField = "BrName";
                    ddBranch.DataBind();
                    if (Session["UserBranchCode"].ToString() == "1")
                    {
                        ddBranch.Items.Insert(0, new ListItem("Select Branch", "0"));
                        ddBranch.SelectedIndex = 0;
                        loadUnpostedPRFEntry();
                    }
                    else
                    {
                        ddBranch.Items.Insert(0, new ListItem(Session["UserBranchName"].ToString(), Session["UserBranchCode"].ToString()));
                        ddBranch.SelectedIndex = 0;
                        loadPRFNo();
                        loadUnpostedPRFEntry();
                    }


                }
            }
        }

        protected void ddBranch_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddBranch.SelectedValue == "0")
            {
                ddPRFNo.DataSource = null;
                ddPRFNo.Items.Clear();
            }
            else
            {
                loadUnpostedPRFEntryPerBranch();
                loadPRFNo();
            }
        }

        //public string stR; 
        private void loadPRFNo()
        {
            using (SqlConnection sqlConn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["MyConnCSI"].ConnectionString))
            {

                string stR = @"SELECT DISTINCT ReceiptNo
			                          FROM [ITEM_PickBranch] 
			                          WHERE [vStat]=1 AND TransactionType=2 and TargetBRCode ='" + ddBranch.SelectedValue + "' ORDER BY ReceiptNo";
                sqlConn.Open();
                using (SqlCommand cmD = new SqlCommand(stR, sqlConn))
                {

                    SqlDataReader dR = cmD.ExecuteReader();
                    ddPRFNo.Items.Clear();
                    ddPRFNo.DataSource = dR;
                    ddPRFNo.DataValueField = "ReceiptNo";
                    ddPRFNo.DataTextField = "ReceiptNo";
                    ddPRFNo.DataBind();
                    ddPRFNo.Items.Insert(0, new ListItem("Select PRF No.", "0"));
                    ddPRFNo.SelectedIndex = 0;

                }
            }
        }


        public string stR2Load;
        private void loadUnpostedPRFEntry()
        {

            try
            {
                using (SqlConnection sqlConn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["MyConnCSI"].ConnectionString))
                {
                    if (Session["UserBranchCode"].ToString() == "1")
                    {
                        stR2Load = @"SELECT a.vRecNum,a.HeaderID, b.BrName
				                              ,a.[ReceiptNo]
				                              ,d.Sup_CategoryName
				                              ,a.[Sup_ItemCode]
				                              ,c.sup_DESCRIPTION
				                              ,a.[vQtyPicked]
				                              ,a.[vBatchNo]
				                              ,a.[vDateExpiry]
				                              ,a.vPickedBy
				                              ,a.vTrandate
                                                    ,CASE WHEN a.TargetBRCode=4 THEN 'DCI-SM NORTH'
													WHEN a.TargetBRCode=5 THEN 'DCI-STA MESA'
													WHEN a.TargetBRCode=6 THEN 'DCI-MEGAMALL'                                                     
													WHEN a.TargetBRCode=9 THEN 'DCI-CEBU'                                                    
													WHEN a.TargetBRCode=10 THEN 'DCI-ALABANG'
													WHEN a.TargetBRCode=11 THEN 'DCI-SOUTHMALL'                                                    
													WHEN a.TargetBRCode=12 THEN 'DCI-BACOOR'                                                       
													WHEN a.TargetBRCode=13 THEN 'DCI-FAIRVIEW'                                                     
													WHEN a.TargetBRCode=14 THEN 'DCI-THE LINK'                                             
													WHEN a.TargetBRCode=16 THEN 'DCI-MANILA'                                                       
													WHEN a.TargetBRCode=17 THEN 'DCI-PAMPANGA'                                                     
													WHEN a.TargetBRCode=18 THEN 'DCI-SUCAT'                                                        
													WHEN a.TargetBRCode=22 THEN 'DCI-BICUTAN'                                                      
													WHEN a.TargetBRCode=23 THEN 'DCI-DASMARINAS'                                                   
													WHEN a.TargetBRCode=25 THEN 'DCI-Mall Of Asia'
													WHEN a.TargetBRCode=27 THEN 'DCI-MARILAO'     
                                                    WHEN a.TargetBRCode=29 THEN 'DCI-Alabang Festival Mall'      
													ELSE 'HO' END AS [DestBranch]
                                              ,a.Remarks
			                              FROM [ITEM_PickBranch] a
			                              LEFT JOIN MyBranchList b
			                              ON a.[BrCode]=b.[BrCode]
			                              LEFT JOIN Sup_ItemMaster c
			                            ON a.[Sup_ItemCode]=c.[Sup_ItemCode]
			                            LEFT JOIN Sup_Category d
			                            ON c.sup_CATEGORY=d.Sup_CategoryNum
			                              WHERE a.[vStat]=1 AND TransactionType=2 and TargetBRCode <> 1
			                              ORDER BY b.BrName,d.Sup_CategoryName";
                    }
                    else
                    {
                        stR2Load = @"SELECT a.vRecNum,a.HeaderID, b.BrName
				                              ,a.[ReceiptNo]
				                              ,d.Sup_CategoryName
				                              ,a.[Sup_ItemCode]
				                              ,c.sup_DESCRIPTION
				                              ,a.[vQtyPicked]
				                              ,a.[vBatchNo]
				                              ,a.[vDateExpiry]
				                              ,a.vPickedBy
				                              ,a.vTrandate
                                                ,CASE WHEN a.TargetBRCode=4 THEN 'DCI-SM NORTH'
													WHEN a.TargetBRCode=5 THEN 'DCI-STA MESA'
													WHEN a.TargetBRCode=6 THEN 'DCI-MEGAMALL'                                                     
													WHEN a.TargetBRCode=9 THEN 'DCI-CEBU'                                                    
													WHEN a.TargetBRCode=10 THEN 'DCI-ALABANG'
													WHEN a.TargetBRCode=11 THEN 'DCI-SOUTHMALL'                                                    
													WHEN a.TargetBRCode=12 THEN 'DCI-BACOOR'                                                       
													WHEN a.TargetBRCode=13 THEN 'DCI-FAIRVIEW'                                                     
													WHEN a.TargetBRCode=14 THEN 'DCI-THE LINK'                                             
													WHEN a.TargetBRCode=16 THEN 'DCI-MANILA'                                                       
													WHEN a.TargetBRCode=17 THEN 'DCI-PAMPANGA'                                                     
													WHEN a.TargetBRCode=18 THEN 'DCI-SUCAT'                                                        
													WHEN a.TargetBRCode=22 THEN 'DCI-BICUTAN'                                                      
													WHEN a.TargetBRCode=23 THEN 'DCI-DASMARINAS'                                                   
													WHEN a.TargetBRCode=25 THEN 'DCI-Mall Of Asia'
													WHEN a.TargetBRCode=27 THEN 'DCI-MARILAO'     
                                                    WHEN a.TargetBRCode=29 THEN 'DCI-Alabang Festival Mall'    
													ELSE 'HO' END AS [DestBranch]
                                                ,a.Remarks
			                              FROM [ITEM_PickBranch] a
			                              LEFT JOIN MyBranchList b
			                              ON a.[BrCode]=b.[BrCode]
			                              LEFT JOIN Sup_ItemMaster c
			                            ON a.[Sup_ItemCode]=c.[Sup_ItemCode]
			                            LEFT JOIN Sup_Category d
			                            ON c.sup_CATEGORY=d.Sup_CategoryNum
			                              WHERE a.[vStat]=1 AND TransactionType=2 and TargetBRCode ='" + ddBranch.SelectedValue + "' ORDER BY b.BrName,d.Sup_CategoryName";
                    }

                    using (SqlCommand cmD = new SqlCommand(stR2Load, sqlConn))
                    {
                        sqlConn.Open();
                        cmD.CommandTimeout = 0;
                        //  cmD.Parameters.AddWithValue("@CategoryNum", ddCategory.SelectedValue);
                        //  cmD.Parameters.AddWithValue("@BrCode", Session["UserBranchCode"].ToString());
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

        protected void ddPRFNo_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddPRFNo.SelectedValue == "0")
            {
                loadUnpostedPRFEntry();
            }
            else
            {
                loadUnpostedPRFEntryPErNo();
            }
        }


        private void loadUnpostedPRFEntryPErNo()
        {

            try
            {
                using (SqlConnection sqlConn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["MyConnCSI"].ConnectionString))
                {
                    string stR = @"SELECT a.vRecNum,a.HeaderID, b.BrName
				                              ,a.[ReceiptNo]
				                              ,d.Sup_CategoryName
				                              ,a.[Sup_ItemCode]
				                              ,c.sup_DESCRIPTION
				                              ,a.[vQtyPicked]
				                              ,a.[vBatchNo]
				                              ,a.[vDateExpiry]
				                              ,a.vPickedBy
				                              ,a.vTrandate
                                                    ,CASE WHEN a.TargetBRCode=4 THEN 'DCI-SM NORTH'
													WHEN a.TargetBRCode=5 THEN 'DCI-STA MESA'
													WHEN a.TargetBRCode=6 THEN 'DCI-MEGAMALL'                                                     
													WHEN a.TargetBRCode=9 THEN 'DCI-CEBU'                                                    
													WHEN a.TargetBRCode=10 THEN 'DCI-ALABANG'
													WHEN a.TargetBRCode=11 THEN 'DCI-SOUTHMALL'                                                    
													WHEN a.TargetBRCode=12 THEN 'DCI-BACOOR'                                                       
													WHEN a.TargetBRCode=13 THEN 'DCI-FAIRVIEW'                                                     
													WHEN a.TargetBRCode=14 THEN 'DCI-THE LINK'                                             
													WHEN a.TargetBRCode=16 THEN 'DCI-MANILA'                                                       
													WHEN a.TargetBRCode=17 THEN 'DCI-PAMPANGA'                                                     
													WHEN a.TargetBRCode=18 THEN 'DCI-SUCAT'                                                        
													WHEN a.TargetBRCode=22 THEN 'DCI-BICUTAN'                                                      
													WHEN a.TargetBRCode=23 THEN 'DCI-DASMARINAS'                                                   
													WHEN a.TargetBRCode=25 THEN 'DCI-Mall Of Asia'
													WHEN a.TargetBRCode=27 THEN 'DCI-MARILAO'   
                                                    WHEN a.TargetBRCode=29 THEN 'DCI-Alabang Festival Mall'      
													ELSE 'HO' END AS [DestBranch]
                                                ,a.Remarks
			                              FROM [ITEM_PickBranch] a
			                              LEFT JOIN MyBranchList b
			                              ON a.[BrCode]=b.[BrCode]
			                              LEFT JOIN Sup_ItemMaster c
			                            ON a.[Sup_ItemCode]=c.[Sup_ItemCode]
			                            LEFT JOIN Sup_Category d
			                            ON c.sup_CATEGORY=d.Sup_CategoryNum
			                              WHERE a.[vStat]=1 AND TransactionType=2 AND a.TargetBRCode='" + ddBranch.SelectedValue + "' and a.[ReceiptNo]='" + ddPRFNo.SelectedItem.Text + "' ORDER BY b.BrName,d.Sup_CategoryName";


                    using (SqlCommand cmD = new SqlCommand(stR, sqlConn))
                    {
                        sqlConn.Open();
                        cmD.CommandTimeout = 0;
                        //  cmD.Parameters.AddWithValue("@CategoryNum", ddCategory.SelectedValue);
                        //  cmD.Parameters.AddWithValue("@BrCode", Session["UserBranchCode"].ToString());
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


        private void loadUnpostedPRFEntryPerBranch()
        {
            try
            {
                using (SqlConnection sqlConn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["MyConnCSI"].ConnectionString))
                {
                    string stR = @"SELECT a.vRecNum,a.HeaderID, b.BrName
				                              ,a.[ReceiptNo]
				                              ,d.Sup_CategoryName
				                              ,a.[Sup_ItemCode]
				                              ,c.sup_DESCRIPTION
				                              ,a.[vQtyPicked]
				                              ,a.[vBatchNo]
				                              ,a.[vDateExpiry]
				                              ,a.vPickedBy
				                              ,a.vTrandate
                                                ,CASE WHEN a.TargetBRCode=4 THEN 'DCI-SM NORTH'
													WHEN a.TargetBRCode=5 THEN 'DCI-STA MESA'
													WHEN a.TargetBRCode=6 THEN 'DCI-MEGAMALL'                                                     
													WHEN a.TargetBRCode=9 THEN 'DCI-CEBU'                                                    
													WHEN a.TargetBRCode=10 THEN 'DCI-ALABANG'
													WHEN a.TargetBRCode=11 THEN 'DCI-SOUTHMALL'                                                    
													WHEN a.TargetBRCode=12 THEN 'DCI-BACOOR'                                                       
													WHEN a.TargetBRCode=13 THEN 'DCI-FAIRVIEW'                                                     
													WHEN a.TargetBRCode=14 THEN 'DCI-THE LINK'                                             
													WHEN a.TargetBRCode=16 THEN 'DCI-MANILA'                                                       
													WHEN a.TargetBRCode=17 THEN 'DCI-PAMPANGA'                                                     
													WHEN a.TargetBRCode=18 THEN 'DCI-SUCAT'                                                        
													WHEN a.TargetBRCode=22 THEN 'DCI-BICUTAN'                                                      
													WHEN a.TargetBRCode=23 THEN 'DCI-DASMARINAS'                                                   
													WHEN a.TargetBRCode=25 THEN 'DCI-Mall Of Asia'
													WHEN a.TargetBRCode=27 THEN 'DCI-MARILAO'  
                                                    WHEN a.TargetBRCode=29 THEN 'DCI-Alabang Festival Mall'       
													ELSE 'HO' END AS [DestBranch]
                                              ,a.Remarks
			                              FROM [ITEM_PickBranch] a
			                              LEFT JOIN MyBranchList b
			                              ON a.[BrCode]=b.[BrCode]
			                              LEFT JOIN Sup_ItemMaster c
			                            ON a.[Sup_ItemCode]=c.[Sup_ItemCode]
			                            LEFT JOIN Sup_Category d
			                            ON c.sup_CATEGORY=d.Sup_CategoryNum
			                              WHERE a.[vStat]=1 AND TransactionType=2 and a.TargetBRCode ='" + ddBranch.SelectedValue + "' ORDER BY b.BrName,d.Sup_CategoryName";


                    using (SqlCommand cmD = new SqlCommand(stR, sqlConn))
                    {
                        sqlConn.Open();
                        cmD.CommandTimeout = 0;
                        //  cmD.Parameters.AddWithValue("@CategoryNum", ddCategory.SelectedValue);
                        //  cmD.Parameters.AddWithValue("@BrCode", Session["UserBranchCode"].ToString());
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
                            //CheckBox chk = (CheckBox)gvR.FindControl("ckStat");

                            using (SqlConnection sqlConn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["MyConnCSI"].ConnectionString))
                            {
                                string stR = @"dbo.PostUnpostedStockTransferEntry";

                                using (SqlCommand cmD = new SqlCommand(stR, sqlConn))
                                {
                                    sqlConn.Open();
                                    cmD.CommandTimeout = 0;
                                    cmD.CommandType = CommandType.StoredProcedure;
                                    cmD.Parameters.AddWithValue("@RecNum", gvR.Cells[0].Text);
                                    cmD.Parameters.AddWithValue("@HeaderID", gvR.Cells[1].Text);
                                    cmD.Parameters.AddWithValue("@PRFNo", gvR.Cells[3].Text);

                                    cmD.Parameters.AddWithValue("@Qty", gvR.Cells[7].Text);
                                    cmD.Parameters.AddWithValue("@PostedBy", Session["UserFullName"].ToString());

                                    //if (chk != null & chk.Checked)
                                    //{
                                    //    cmD.Parameters.AddWithValue("@cK", 2);
                                    //}
                                    //else
                                    //{
                                    //    cmD.Parameters.AddWithValue("@cK", 1);
                                    //}

                                    cmD.ExecuteNonQuery();
                                }
                            }

                        }
                    }

                    loadBranch();
                    //loadUnpostedPRFEntryPerBranch();
                    ddPRFNo.DataSource = null;
                    ddPRFNo.Items.Clear();
                    //loadPRFNo();
                    lblMsg.Text = "Stock Transfer Successfully approved.";
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

        protected void gvPicked_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            if (IsPageRefresh == true)
            {
                Response.Redirect(Request.Url.AbsoluteUri);
            }
            else
            {
                try
                {
                    if (ddBranch.SelectedValue == "0")
                    {
                        lblMsg.Text = "Please select branch  and PRF number before deleting.";
                        ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "Popup", "ShowSuccessMsg();", true);
                        return;
                    }
                    else
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


                        if (ddPRFNo.SelectedValue == "0")
                        {
                            loadUnpostedPRFEntry();
                        }
                        else
                        {
                            loadUnpostedPRFEntryPErNo();
                        }
                    }
                }
                catch (Exception y)
                {
                    lblMsg.Text = y.Message;
                    ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "Popup", "ShowSuccessMsg();", true);
                    return;
                }
            }
        }

        protected void gvPicked_PreRender(object sender, EventArgs e)
        {
            if (gvPicked.Rows.Count > 0)
            {
                gvPicked.UseAccessibleHeader = true;
                gvPicked.HeaderRow.TableSection = TableRowSection.TableHeader;
                gvPicked.FooterRow.TableSection = TableRowSection.TableFooter;
            }
        }

    }
}