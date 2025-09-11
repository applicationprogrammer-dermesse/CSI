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
    public partial class HomePage : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Session["UserFullName"] == null)
                {
                    ScriptManager.RegisterStartupScript(Page, Page.GetType(), "redirect script",
                    "alert('You been idle for a long period of time, Need to Sign in again!'); location.href='Login.aspx';", true);
                }
                else
                {
                    CountForApprovalBrHead();
                    CountForApproval();
                    CountForApprovalLogistics();
                    CountApprovedRequest();
                    CountForIssuance();
                    CountUnpostedIssuance();
                    CountUnpostedAdjustment();
                    CountUnpostedPRF();

                    CountUnpostedCompli();

                    CountPR();
                    CountOrderForPrintDR();
                    CountInTransitOrder();
                    CountCancelledOrder();

                    CountFallBelow();

                    CountNearExpiry();

                    CountExpired();

                    CountUnpostedStockTransfer();
                    CountStockTransferforPosting();

                    CountRRforPostingwithPO();
                    CountRRforPostingwithoutPO();


                  //  CountClinicRequirements();
                }
            }
        }

        private void CountClinicRequirements()
        {
            using (SqlConnection sqlConn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["MyConnCSI"].ConnectionString))
            {
                string stR = @"dbo.ClinicRequirements";
                using (SqlCommand cmD = new SqlCommand(stR, sqlConn))
                {
                    sqlConn.Open();
                    cmD.CommandTimeout = 0;
                    cmD.CommandType = CommandType.StoredProcedure;
                    cmD.Parameters.AddWithValue("@BrCode", Session["UserBranchCode"].ToString());
                    DataTable dT = new DataTable();
                    SqlDataAdapter dA = new SqlDataAdapter(cmD);
                    dA.Fill(dT);

                    if (dT.Rows.Count > 0)
                    {
                       lblClinicRequirements.Text = dT.Rows.Count.ToString();
                    }
                    else
                    {
                        lblClinicRequirements.Text = "0";
                    }

                }
            }
        }

        private void CountForApprovalBrHead()
        {
            using (SqlConnection sqlConn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["MyConnCSI"].ConnectionString))
            {
                string stR = @"dbo.ListForApprovalBrHead";
                using (SqlCommand cmD = new SqlCommand(stR, sqlConn))
                {
                    sqlConn.Open();
                    cmD.CommandTimeout = 0;
                    cmD.CommandType = CommandType.StoredProcedure;
                    cmD.Parameters.AddWithValue("@BrCode", Session["UserBranchCode"].ToString());
                    cmD.Parameters.AddWithValue("@AreaApprover", Session["AreaApprover"].ToString());
                    DataTable dT = new DataTable();
                    SqlDataAdapter dA = new SqlDataAdapter(cmD);
                    dA.Fill(dT);

                    if (dT.Rows.Count > 0)
                    {
                        lblForApprovalBrHead.Text = dT.Rows.Count.ToString();
                    }
                    else
                    {
                        lblForApprovalBrHead.Text = "0";
                    }

                }
            }
        }

        private void CountForApproval()
        {
            using (SqlConnection sqlConn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["MyConnCSI"].ConnectionString))
            {
                string stR = @"dbo.ListForApproval";
                using (SqlCommand cmD = new SqlCommand(stR, sqlConn))
                {
                    sqlConn.Open();
                    cmD.CommandTimeout = 0;
                    cmD.CommandType = CommandType.StoredProcedure;
                    cmD.Parameters.AddWithValue("@BrCode", Session["UserBranchCode"].ToString());
                    DataTable dT = new DataTable();
                    SqlDataAdapter dA = new SqlDataAdapter(cmD);
                    dA.Fill(dT);

                    if (dT.Rows.Count > 0)
                    {
                       lblForApproval.Text = dT.Rows.Count.ToString();
                    }
                    else
                    {
                        lblForApproval.Text = "0";
                    }

                }
            }
        }

        private void CountForApprovalLogistics()
        {
            using (SqlConnection sqlConn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["MyConnCSI"].ConnectionString))
            {
                string stR = @"dbo.ListForApprovalLogistics";
                using (SqlCommand cmD = new SqlCommand(stR, sqlConn))
                {
                    sqlConn.Open();
                    cmD.CommandTimeout = 0;
                    cmD.CommandType = CommandType.StoredProcedure;
                    cmD.Parameters.AddWithValue("@BrCode", Session["UserBranchCode"].ToString());
                    DataTable dT = new DataTable();
                    SqlDataAdapter dA = new SqlDataAdapter(cmD);
                    dA.Fill(dT);

                    if (dT.Rows.Count > 0)
                    {
                        lblForApprovalLogistics.Text = dT.Rows.Count.ToString();
                    }
                    else
                    {
                        lblForApprovalLogistics.Text = "0";
                    }

                }
            }
        }
        private void CountApprovedRequest()
        {
            using (SqlConnection sqlConn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["MyConnCSI"].ConnectionString))
            {
                string stR = @"dbo.ListApprovedRequest";
                using (SqlCommand cmD = new SqlCommand(stR, sqlConn))
                {
                    sqlConn.Open();
                    cmD.CommandTimeout = 0;
                    cmD.CommandType = CommandType.StoredProcedure;
                    cmD.Parameters.AddWithValue("@BrCode", Session["UserBranchCode"].ToString());
                    DataTable dT = new DataTable();
                    SqlDataAdapter dA = new SqlDataAdapter(cmD);
                    dA.Fill(dT);

                    if (dT.Rows.Count > 0)
                    {
                        lblApproved.Text = dT.Rows.Count.ToString();
                    }
                    else
                    {
                        lblApproved.Text = "0";
                    }

                }
            }
        }


        private void CountForIssuance()
        {
            using (SqlConnection sqlConn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["MyConnCSI"].ConnectionString))
            {
                string stR = @"dbo.ListForIssuance";
                using (SqlCommand cmD = new SqlCommand(stR, sqlConn))
                {
                    sqlConn.Open();
                    cmD.CommandTimeout = 0;
                    cmD.CommandType = CommandType.StoredProcedure;
                    cmD.Parameters.AddWithValue("@BrCode", Session["UserBranchCode"].ToString());
                    DataTable dT = new DataTable();
                    SqlDataAdapter dA = new SqlDataAdapter(cmD);
                    dA.Fill(dT);

                    if (dT.Rows.Count > 0)
                    {
                        lblForIssuance.Text = dT.Rows.Count.ToString();
                    }
                    else
                    {
                        lblForIssuance.Text = "0";
                    }

                }
            }
        }


        private void CountUnpostedIssuance()
        {
            using (SqlConnection sqlConn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["MyConnCSI"].ConnectionString))
            {
                string stR = @"dbo.ListUnpostedIssuance";
                using (SqlCommand cmD = new SqlCommand(stR, sqlConn))
                {
                    sqlConn.Open();
                    cmD.CommandTimeout = 0;
                    cmD.CommandType = CommandType.StoredProcedure;
                    cmD.Parameters.AddWithValue("@BrCode", Session["UserBranchCode"].ToString());
                    DataTable dT = new DataTable();
                    SqlDataAdapter dA = new SqlDataAdapter(cmD);
                    dA.Fill(dT);

                    if (dT.Rows.Count > 0)
                    {
                        lblUnpostedIssuance.Text = dT.Rows.Count.ToString();
                    }
                    else
                    {
                        lblUnpostedIssuance.Text = "0";
                    }

                }
            }
        }


        private void CountUnpostedAdjustment()
        {
            using (SqlConnection sqlConn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["MyConnCSI"].ConnectionString))
            {
                string stR = @"dbo.ListUnpostedAdjustment";
                using (SqlCommand cmD = new SqlCommand(stR, sqlConn))
                {
                    sqlConn.Open();
                    cmD.CommandTimeout = 0;
                    cmD.CommandType = CommandType.StoredProcedure;
                    cmD.Parameters.AddWithValue("@BrCode", Session["UserBranchCode"].ToString());
                    DataTable dT = new DataTable();
                    SqlDataAdapter dA = new SqlDataAdapter(cmD);
                    dA.Fill(dT);

                    if (dT.Rows.Count > 0)
                    {
                        lblAdjustmentForPosting.Text = dT.Rows.Count.ToString();
                    }
                    else
                    {
                        lblAdjustmentForPosting.Text = "0";
                    }

                }
            }
        }

        private void CountUnpostedStockTransfer()
        {
            using (SqlConnection sqlConn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["MyConnCSI"].ConnectionString))
            {
                string stR = @"dbo.ListUnpostedTransfer";
                using (SqlCommand cmD = new SqlCommand(stR, sqlConn))
                {
                    sqlConn.Open();
                    cmD.CommandTimeout = 0;
                    cmD.CommandType = CommandType.StoredProcedure;
                    cmD.Parameters.AddWithValue("@BrCode", Session["UserBranchCode"].ToString());
                    DataTable dT = new DataTable();
                    SqlDataAdapter dA = new SqlDataAdapter(cmD);
                    dA.Fill(dT);

                    if (dT.Rows.Count > 0)
                    {
                        lblStockTransfer.Text = dT.Rows.Count.ToString();
                    }
                    else
                    {
                        lblStockTransfer.Text = "0";
                    }

                }
            }
        }

        private void CountStockTransferforPosting()
        {
            using (SqlConnection sqlConn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["MyConnCSI"].ConnectionString))
            {
                string stR = @"dbo.LisStockTransferforPosting";
                using (SqlCommand cmD = new SqlCommand(stR, sqlConn))
                {
                    sqlConn.Open();
                    cmD.CommandTimeout = 0;
                    cmD.CommandType = CommandType.StoredProcedure;
                    cmD.Parameters.AddWithValue("@BrCode", Session["UserBranchCode"].ToString());
                    DataTable dT = new DataTable();
                    SqlDataAdapter dA = new SqlDataAdapter(cmD);
                    dA.Fill(dT);

                    if (dT.Rows.Count > 0)
                    {
                        lblStockTransferForPosting.Text = dT.Rows.Count.ToString();
                    }
                    else
                    {
                        lblStockTransferForPosting.Text = "0";
                    }

                }
            }
        }

        private void CountUnpostedPRF()
        {
            using (SqlConnection sqlConn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["MyConnCSI"].ConnectionString))
            {
                string stR = @"dbo.ListUnpostedPRF";
                using (SqlCommand cmD = new SqlCommand(stR, sqlConn))
                {
                    sqlConn.Open();
                    cmD.CommandTimeout = 0;
                    cmD.CommandType = CommandType.StoredProcedure;
                    cmD.Parameters.AddWithValue("@BrCode", Session["UserBranchCode"].ToString());
                    DataTable dT = new DataTable();
                    SqlDataAdapter dA = new SqlDataAdapter(cmD);
                    dA.Fill(dT);

                    if (dT.Rows.Count > 0)
                    {
                        lblPRFForPosting.Text = dT.Rows.Count.ToString();
                    }
                    else
                    {
                        lblPRFForPosting.Text = "0";
                    }

                }
            }
        }

        private void CountUnpostedCompli()
        {
            using (SqlConnection sqlConn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["MyConnCSI"].ConnectionString))
            {
                string stR = @"dbo.ListUnpostedCompli";
                using (SqlCommand cmD = new SqlCommand(stR, sqlConn))
                {
                    sqlConn.Open();
                    cmD.CommandTimeout = 0;
                    cmD.CommandType = CommandType.StoredProcedure;
                    cmD.Parameters.AddWithValue("@BrCode", Session["UserBranchCode"].ToString());
                    DataTable dT = new DataTable();
                    SqlDataAdapter dA = new SqlDataAdapter(cmD);
                    dA.Fill(dT);

                    if (dT.Rows.Count > 0)
                    {
                        lblCompliForPosting.Text = dT.Rows.Count.ToString();
                    }
                    else
                    {
                        lblCompliForPosting.Text = "0";
                    }

                }
            }
        }


        private void CountPR()
        {
            using (SqlConnection sqlConn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["MyConnCSI"].ConnectionString))
            {
                string stR = @"dbo.ListPR";
                using (SqlCommand cmD = new SqlCommand(stR, sqlConn))
                {
                    sqlConn.Open();
                    cmD.CommandTimeout = 0;
                    cmD.CommandType = CommandType.StoredProcedure;
                    //cmD.Parameters.AddWithValue("@BrCode", Session["UserBranchCode"].ToString());
                    DataTable dT = new DataTable();
                    SqlDataAdapter dA = new SqlDataAdapter(cmD);
                    dA.Fill(dT);

                    if (dT.Rows.Count > 0)
                    {
                        lblPurchaseRequisition.Text = dT.Rows.Count.ToString();
                    }
                    else
                    {
                        lblPurchaseRequisition.Text = "0";
                    }

                }
            }
        }



        private void CountOrderForPrintDR()
        {
            using (SqlConnection sqlConn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["MyConnCSI"].ConnectionString))
            {
                string stR = @"dbo.ListOrderOnlineForPrintDR";
                using (SqlCommand cmD = new SqlCommand(stR, sqlConn))
                {
                    sqlConn.Open();
                    cmD.CommandTimeout = 0;
                    cmD.CommandType = CommandType.StoredProcedure;
                    //cmD.Parameters.AddWithValue("@BrCode", Session["UserBranchCode"].ToString());
                    DataTable dT = new DataTable();
                    SqlDataAdapter dA = new SqlDataAdapter(cmD);
                    dA.Fill(dT);

                    if (dT.Rows.Count > 0)
                    {
                        lblOnlineOrder.Text = dT.Rows.Count.ToString();
                    }
                    else
                    {
                        lblOnlineOrder.Text = "0";
                    }

                }
            }
        }

        private void CountCancelledOrder()
        {
            using (SqlConnection sqlConn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["MyConnCSI"].ConnectionString))
            {
                string stR = @"dbo.CountCancelledOnlineTransaction";
                using (SqlCommand cmD = new SqlCommand(stR, sqlConn))
                {
                    sqlConn.Open();
                    cmD.CommandTimeout = 0;
                    cmD.CommandType = CommandType.StoredProcedure;
                    //cmD.Parameters.AddWithValue("@BrCode", Session["UserBranchCode"].ToString());
                    DataTable dT = new DataTable();
                    SqlDataAdapter dA = new SqlDataAdapter(cmD);
                    dA.Fill(dT);

                    if (dT.Rows.Count > 0)
                    {
                        lblCancelledOrder.Text = dT.Rows.Count.ToString();
                    }
                    else
                    {
                        lblCancelledOrder.Text = "0";
                    }

                }
            }
        }
        private void CountInTransitOrder()
        {
            using (SqlConnection sqlConn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["MyConnCSI"].ConnectionString))
            {
                string stR = @"dbo.CountOnlineTransaction";
                using (SqlCommand cmD = new SqlCommand(stR, sqlConn))
                {
                    sqlConn.Open();
                    cmD.CommandTimeout = 0;
                    cmD.CommandType = CommandType.StoredProcedure;
                    //cmD.Parameters.AddWithValue("@BrCode", Session["UserBranchCode"].ToString());
                    DataTable dT = new DataTable();
                    SqlDataAdapter dA = new SqlDataAdapter(cmD);
                    dA.Fill(dT);

                    if (dT.Rows.Count > 0)
                    {
                        lblIntransit.Text = dT.Rows.Count.ToString();
                    }
                    else
                    {
                        lblIntransit.Text = "0";
                    }

                }
            }
        }


        private void CountFallBelow()
        {
            using (SqlConnection sqlConn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["MyConnCSI"].ConnectionString))
            {
                string stR = @"dbo.rptBelowMaintainingBalance";
                using (SqlCommand cmD = new SqlCommand(stR, sqlConn))
                {
                    sqlConn.Open();
                    cmD.CommandTimeout = 0;
                    cmD.CommandType = CommandType.StoredProcedure;
                    cmD.Parameters.AddWithValue("@BrCode", Session["UserBranchCode"].ToString());
                    DataTable dT = new DataTable();
                    SqlDataAdapter dA = new SqlDataAdapter(cmD);
                    dA.Fill(dT);

                    if (dT.Rows.Count > 0)
                    {
                        lblFallBelow.Text = dT.Rows.Count.ToString();
                    }
                    else
                    {
                        lblFallBelow.Text = "0";
                    }

                }
            }
        }



        private string stRNearEx;
        private void CountNearExpiry()
        {
            using (SqlConnection sqlConn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["MyConnCSI"].ConnectionString))
            {
                if (Session["UserBranchCode"].ToString() == "1")
                {
                    stRNearEx = @"dbo.NearExpiryLogistics";
                }
                else
                {
                    stRNearEx = @"dbo.NearExpiryPerBranch";
                }
                
                using (SqlCommand cmD = new SqlCommand(stRNearEx, sqlConn))
                {
                    sqlConn.Open();
                    cmD.CommandTimeout = 0;
                    cmD.CommandType = CommandType.StoredProcedure;
                    cmD.Parameters.AddWithValue("@BrCode", Session["UserBranchCode"].ToString());
                    if (Convert.ToInt32(Session["UserBranchCode"].ToString()) > 2 & Convert.ToInt32(Session["UserBranchCode"].ToString()) < 28)
                    {
                        cmD.Parameters.AddWithValue("@Option", 1);
                    }
                    else if (Convert.ToInt32(Session["UserBranchCode"].ToString()) == 1)
                    {
                        cmD.Parameters.AddWithValue("@Option", 0);
                    }
                    else
                    {
                        cmD.Parameters.AddWithValue("@Option", 2);
                    }
                    DataTable dT = new DataTable();
                    SqlDataAdapter dA = new SqlDataAdapter(cmD);
                    dA.Fill(dT);

                    if (dT.Rows.Count > 0)
                    {
                        if (Convert.ToInt32(Session["UserBranchCode"].ToString()) < 52)
                        {
                            lblNearExpiryItems.Text = dT.Rows.Count.ToString();
                        }
                        else
                        {
                            lblNearExpiryItems.Text = "0";
                        }
                    }
                    else
                    {
                        lblNearExpiryItems.Text = "0";
                    }

                }
            }

        }

        public string stRExpired;
        private void CountExpired()
        {
            using (SqlConnection sqlConn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["MyConnCSI"].ConnectionString))
            {
                if (Session["UserBranchCode"].ToString() == "1")
                {
                    stRExpired = @"dbo.ExpiredLogistics";
                }
                else
                {
                    stRExpired = @"dbo.ExpiredPerBranch";
                }

                using (SqlCommand cmD = new SqlCommand(stRExpired, sqlConn))
                {
                    sqlConn.Open();
                    cmD.CommandTimeout = 0;
                    cmD.CommandType = CommandType.StoredProcedure;
                    cmD.Parameters.AddWithValue("@BrCode", Session["UserBranchCode"].ToString());
                    if (Convert.ToInt32(Session["UserBranchCode"].ToString()) > 2 & Convert.ToInt32(Session["UserBranchCode"].ToString()) < 28)
                    {
                        cmD.Parameters.AddWithValue("@Option", 1);
                    }
                    else if (Convert.ToInt32(Session["UserBranchCode"].ToString()) == 1)
                    {
                        cmD.Parameters.AddWithValue("@Option", 0);
                    }
                    else
                    {
                        cmD.Parameters.AddWithValue("@Option", 2);
                    }
                    DataTable dT = new DataTable();
                    SqlDataAdapter dA = new SqlDataAdapter(cmD);
                    dA.Fill(dT);

                    if (dT.Rows.Count > 0)
                    {
                        if (Convert.ToInt32(Session["UserBranchCode"].ToString()) < 52)
                        {
                            lblExpired.Text = dT.Rows.Count.ToString();
                        }
                        else
                        {
                            lblExpired.Text = "0";
                        }
                    }
                    else
                    {
                        lblExpired.Text = "0";
                    }

                }
            }
        }


        private void CountRRforPostingwithPO()
        {
            using (SqlConnection sqlConn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["MyConnCSI"].ConnectionString))
            {
                string stR = @"SELECT A.[RecNum] 
                              ,A.[RRDetailID]
                              ,A.[RRNo]
                              ,A.[RRDate]
                              ,A.[RRSupplier]
                              ,A.[sup_ItemCode]
	                          ,B.sup_DESCRIPTION
                              ,A.[sup_UnitCost]
                              ,A.POQuantity
                              ,A.[sup_RetailCost]
                              ,A.[vQtyReceived]
                              ,A.[TotalAmount]
                              ,A.[vBatchNo]
                              ,A.[vDateExpiry]
                              ,A.[EncodedBy]
                          FROM [ITEM_RREntryWithPO] A
                          LEFT JOIN Sup_ItemMaster B
                          ON A.sup_ItemCode = B.sup_ItemCode
                          WHERE A.[vStat]=0";
                using (SqlCommand cmD = new SqlCommand(stR, sqlConn))
                {
                    sqlConn.Open();
                    cmD.CommandTimeout = 0;
                    //cmD.CommandType = CommandType.StoredProcedure;
                    //cmD.Parameters.AddWithValue("@BrCode", Session["UserBranchCode"].ToString());
                    DataTable dT = new DataTable();
                    SqlDataAdapter dA = new SqlDataAdapter(cmD);
                    dA.Fill(dT);

                    if (dT.Rows.Count > 0)
                    {
                        lblRRwithPOforPostingPO.Text = dT.Rows.Count.ToString();
                    }
                    else
                    {
                        lblRRwithPOforPostingPO.Text = "0";
                    }

                }
            }
        }


        private void CountRRforPostingwithoutPO()
        {
            using (SqlConnection sqlConn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["MyConnCSI"].ConnectionString))
            {
                string stR = @"SELECT A.[RecNum] 
                              ,A.[RRNo]
                              ,A.[RRDate]
                              ,A.[RRSupplier]
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
                using (SqlCommand cmD = new SqlCommand(stR, sqlConn))
                {
                    sqlConn.Open();
                    cmD.CommandTimeout = 0;
                    //cmD.CommandType = CommandType.StoredProcedure;
                    //cmD.Parameters.AddWithValue("@BrCode", Session["UserBranchCode"].ToString());
                    DataTable dT = new DataTable();
                    SqlDataAdapter dA = new SqlDataAdapter(cmD);
                    dA.Fill(dT);

                    if (dT.Rows.Count > 0)
                    {
                        blRRwithoutPOforPosting.Text = dT.Rows.Count.ToString();
                    }
                    else
                    {
                        blRRwithoutPOforPosting.Text = "0";
                    }

                }
            }
        }

    }
}