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
    public partial class ListAdjustment : System.Web.UI.Page
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

                    if (Session["UserBranchCode"].ToString() == "1")
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
                string stR = @"SELECT DISTINCT a.BrCode,b.BrName
			                          FROM [ITEM_PickBranch] a
			                          LEFT JOIN MyBranchList b
			                          ON a.[BrCode]=b.[BrCode] WHERE a.[vStat]=1 AND TransactionType=3 ORDER BY b.BrName";
                sqlConn.Open();
                using (SqlCommand cmD = new SqlCommand(stR, sqlConn))
                {

                    SqlDataReader dR = cmD.ExecuteReader();
                    ddBranch.Items.Clear();
                    ddBranch.DataSource = dR;
                    ddBranch.DataValueField = "BrCode";
                    ddBranch.DataTextField = "BrName";
                    ddBranch.DataBind();
                    if (Session["UserBranchCode"].ToString() == "1")
                    {
                        ddBranch.Items.Insert(0, new ListItem("Select Branch", "0"));
                        ddBranch.SelectedIndex = 0;
                        loadUnpostedAdjustmentEntry();
                    }
                    else
                    {
                        ddBranch.Items.Insert(0, new ListItem(Session["UserBranchName"].ToString(), Session["UserBranchCode"].ToString()));
                        ddBranch.SelectedIndex = 0;
                        loadAdjustmentNo();
                        loadUnpostedAdjustmentEntry();
                    }
                    

                }
            }
        }

        protected void ddBranch_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddBranch.SelectedValue=="0")
            {
                ddAdjustmentNo.DataSource = null;
                ddAdjustmentNo.Items.Clear();
            }
            else
            {
                loadUnpostedAdjustmentEntryPerBranch();
                loadAdjustmentNo();
            }
        }

        //public string stR; 
        private void loadAdjustmentNo()
        {
            using (SqlConnection sqlConn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["MyConnCSI"].ConnectionString))
            {
                
                string stR = @"SELECT DISTINCT TransactionNo
			                          FROM [ITEM_PickBranch] 
			                          WHERE [vStat]=1 AND TransactionType=3 and BrCode='" + ddBranch.SelectedValue + "' ORDER BY TransactionNo";
                sqlConn.Open();
                using (SqlCommand cmD = new SqlCommand(stR, sqlConn))
                {

                    SqlDataReader dR = cmD.ExecuteReader();
                    ddAdjustmentNo.Items.Clear();
                    ddAdjustmentNo.DataSource = dR;
                    ddAdjustmentNo.DataValueField = "TransactionNo";
                    ddAdjustmentNo.DataTextField = "TransactionNo";
                    ddAdjustmentNo.DataBind();
                    ddAdjustmentNo.Items.Insert(0, new ListItem("Select Adjustment No.", "0"));
                    ddAdjustmentNo.SelectedIndex = 0;

                }
            }
        }


        public string stR2Load;
        private void loadUnpostedAdjustmentEntry()
        {
           
            try
            {
                using (SqlConnection sqlConn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["MyConnCSI"].ConnectionString))
                {
                    if (Session["UserBranchCode"].ToString() == "1")
                    {
                        stR2Load = @"SELECT a.vRecNum,a.HeaderID, b.BrName
				                              ,a.[TransactionNo]
				                              ,d.Sup_CategoryName
				                              ,a.[Sup_ItemCode]
				                              ,c.sup_DESCRIPTION
				                              ,a.[vQtyPicked]
				                              ,a.[vBatchNo]
				                              ,a.[vDateExpiry]
				                              ,a.vPickedBy
				                              ,a.vTrandate
                                              ,a.Remarks
			                              FROM [ITEM_PickBranch] a
			                              LEFT JOIN MyBranchList b
			                              ON a.[BrCode]=b.[BrCode]
			                              LEFT JOIN Sup_ItemMaster c
			                            ON a.[Sup_ItemCode]=c.[Sup_ItemCode]
			                            LEFT JOIN Sup_Category d
			                            ON c.sup_CATEGORY=d.Sup_CategoryNum
			                              WHERE a.[vStat]=1 AND TransactionType=3
			                              ORDER BY b.BrName,d.Sup_CategoryName";
                    }
                    else
                    {
                        stR2Load = @"SELECT a.vRecNum,a.HeaderID, b.BrName
				                              ,a.[TransactionNo]
				                              ,d.Sup_CategoryName
				                              ,a.[Sup_ItemCode]
				                              ,c.sup_DESCRIPTION
				                              ,a.[vQtyPicked]
				                              ,a.[vBatchNo]
				                              ,a.[vDateExpiry]
				                              ,a.vPickedBy
				                              ,a.vTrandate
                                               ,a.Remarks
			                              FROM [ITEM_PickBranch] a
			                              LEFT JOIN MyBranchList b
			                              ON a.[BrCode]=b.[BrCode]
			                              LEFT JOIN Sup_ItemMaster c
			                            ON a.[Sup_ItemCode]=c.[Sup_ItemCode]
			                            LEFT JOIN Sup_Category d
			                            ON c.sup_CATEGORY=d.Sup_CategoryNum
			                              WHERE a.[vStat]=1 AND TransactionType=3 AND a.BrCode='" + ddBranch.SelectedValue + "' ORDER BY b.BrName,d.Sup_CategoryName";
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

        protected void ddAdjustmentNo_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddAdjustmentNo.SelectedValue=="0")
            {
                loadUnpostedAdjustmentEntry();
            }
            else
            {
                loadUnpostedAdjustmentEntryPErNo();
            }
        }


        private void loadUnpostedAdjustmentEntryPErNo()
        {

            try
            {
                using (SqlConnection sqlConn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["MyConnCSI"].ConnectionString))
                {
                    string stR = @"SELECT a.vRecNum,a.HeaderID, b.BrName
				                              ,a.[TransactionNo]
				                              ,d.Sup_CategoryName
				                              ,a.[Sup_ItemCode]
				                              ,c.sup_DESCRIPTION
				                              ,a.[vQtyPicked]
				                              ,a.[vBatchNo]
				                              ,a.[vDateExpiry]
				                              ,a.vPickedBy
				                              ,a.vTrandate
                                              ,a.Remarks
			                              FROM [ITEM_PickBranch] a
			                              LEFT JOIN MyBranchList b
			                              ON a.[BrCode]=b.[BrCode]
			                              LEFT JOIN Sup_ItemMaster c
			                            ON a.[Sup_ItemCode]=c.[Sup_ItemCode]
			                            LEFT JOIN Sup_Category d
			                            ON c.sup_CATEGORY=d.Sup_CategoryNum
			                              WHERE a.[vStat]=1 AND TransactionType=3 AND a.BrCode='" + ddBranch.SelectedValue + "' and a.[TransactionNo]='" + ddAdjustmentNo.SelectedItem.Text + "' ORDER BY b.BrName,d.Sup_CategoryName";


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


        private void loadUnpostedAdjustmentEntryPerBranch()
        {
            try
            {
                using (SqlConnection sqlConn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["MyConnCSI"].ConnectionString))
                {
                    string stR = @"SELECT a.vRecNum,a.HeaderID, b.BrName
				                              ,a.[TransactionNo]
				                              ,d.Sup_CategoryName
				                              ,a.[Sup_ItemCode]
				                              ,c.sup_DESCRIPTION
				                              ,a.[vQtyPicked]
				                              ,a.[vBatchNo]
				                              ,a.[vDateExpiry]
				                              ,a.vPickedBy
				                              ,a.vTrandate
                                              ,a.Remarks
			                              FROM [ITEM_PickBranch] a
			                              LEFT JOIN MyBranchList b
			                              ON a.[BrCode]=b.[BrCode]
			                              LEFT JOIN Sup_ItemMaster c
			                            ON a.[Sup_ItemCode]=c.[Sup_ItemCode]
			                            LEFT JOIN Sup_Category d
			                            ON c.sup_CATEGORY=d.Sup_CategoryNum
			                              WHERE a.[vStat]=1 AND TransactionType=3 AND a.BrCode='" + ddBranch.SelectedValue + "' ORDER BY b.BrName,d.Sup_CategoryName";


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
                            using (SqlConnection sqlConn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["MyConnCSI"].ConnectionString))
                            {
                                string stR = @"dbo.PostUnpostedAdjustmentEntry";

                                using (SqlCommand cmD = new SqlCommand(stR, sqlConn))
                                {
                                    sqlConn.Open();
                                    cmD.CommandTimeout = 0;
                                    cmD.CommandType = CommandType.StoredProcedure;
                                    cmD.Parameters.AddWithValue("@RecNum", gvR.Cells[0].Text);
                                    cmD.Parameters.AddWithValue("@HeaderID", gvR.Cells[1].Text);
                                    cmD.Parameters.AddWithValue("@Qty", gvR.Cells[7].Text);

                                    cmD.ExecuteNonQuery();
                                }
                            }

                        }
                    }

                    //loadBranch();
                    loadUnpostedAdjustmentEntryPerBranch();
                    loadAdjustmentNo();
                    lblMsg.Text = "Adjustment Successfully Posted.";
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