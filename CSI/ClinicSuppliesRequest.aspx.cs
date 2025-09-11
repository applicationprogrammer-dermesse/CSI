using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
//using ClosedXML.Excel;

namespace CSI
{
    public partial class ClinicSuppliesRequest : System.Web.UI.Page
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
                    Session["MRFNo"] = null;
                    loadCategory();
                    lblBranch.Text = Session["UserBranchName"].ToString();

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
                string stR = @"Select Sup_CategoryNum,Sup_CategoryName from Sup_Category WHERE Sup_CategoryNum in ('1','2','3','4','5','6','9')";
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
            try
            {
                using (SqlConnection sqlConn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["MyConnCSI"].ConnectionString))
                {

                    string stR = @"dbo.LoadItemMasterBranchRequest";

                    using (SqlCommand cmD = new SqlCommand(stR, sqlConn))
                    {
                        sqlConn.Open();
                        cmD.CommandTimeout = 0;
                        cmD.CommandType = CommandType.StoredProcedure;
                        cmD.Parameters.AddWithValue("@Category", ddCategory.SelectedValue);
                        cmD.Parameters.AddWithValue("@BrCode", Session["UserBranchCode"].ToString());
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
            catch (Exception x)
            {

                lblMsg.Text = x.Message;
                ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "Popup", "ShowSuccessMsg();", true);
                return;
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


                ddItem.DataSource = null;
                ddItem.Items.Clear();

                txtBalance.Text = string.Empty;
                txtQty.Text = string.Empty;


                if (Convert.ToInt32(Session["UserBranchCode"]) >= 2 && Convert.ToInt32(Session["UserBranchCode"]) <= 8 | Convert.ToInt32(Session["UserBranchCode"]) >= 10 && Convert.ToInt32(Session["UserBranchCode"]) <= 35)
                {
                    if (ddRequestType.SelectedItem.Text == "Regular")
                    {
                        if (ddCategory.SelectedValue == "5" | ddCategory.SelectedValue == "6")
                        {
                            if (DateTime.Now.Day == 10 | DateTime.Now.Day == 11 | DateTime.Now.Day == 22 | DateTime.Now.Day == 23 | DateTime.Now.Day == 24 | DateTime.Now.Day == 25)
                            {
                                LoadBranchRequest();
                                loadItemSupplies();
                            }
                            else
                            {
                                lblMsg.Text = "This category does not allow to input/submit on this date.";
                                ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "Popup", "ShowSuccessMsg();", true);
                                return;
                            }
                        }
                        else if (ddCategory.SelectedValue == "2" | ddCategory.SelectedValue == "3" | ddCategory.SelectedValue == "4" | ddCategory.SelectedValue == "1")
                        {
                            if (DateTime.Now.Day == 22 | DateTime.Now.Day == 23 | DateTime.Now.Day == 24 | DateTime.Now.Day == 25)
                            {
                                LoadBranchRequest();
                                loadItemSupplies();
                            }
                            else
                            {
                                lblMsg.Text = "This category does not allow to input/submit on this date.";
                                ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "Popup", "ShowSuccessMsg();", true);
                                return;
                            }
                        }
                        else if (ddCategory.SelectedValue == "9")
                        {
                                LoadBranchRequest();
                                loadItemSupplies();
                        
                        }

                        //else
                        //{
                        //    lblMsg.Text = "Other Category";
                        //    ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "Popup", "ShowSuccessMsg();", true);
                        //    return;
                        //}
                    
                    }
                    else if (ddRequestType.SelectedItem.Text == "Emergency")
                    {
                        if (ddCategory.SelectedValue == "5" | ddCategory.SelectedValue == "6")
                        {
                            if (DateTime.Now.Day == 10 | DateTime.Now.Day == 11 | DateTime.Now.Day == 22 | DateTime.Now.Day == 23 | DateTime.Now.Day == 24 | DateTime.Now.Day == 25)
                            {
                                LoadBranchRequest();
                                loadItemSupplies();
                            }
                            else
                            {
                                lblMsg.Text = "This category does not allow to input/submit on this date.";
                                ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "Popup", "ShowSuccessMsg();", true);
                                return;
                            }
                        }
                        else if (ddCategory.SelectedValue == "2" | ddCategory.SelectedValue == "3" |  ddCategory.SelectedValue == "1")
                        {
                            if (DateTime.Now.Day == 22 | DateTime.Now.Day == 23 | DateTime.Now.Day == 24 | DateTime.Now.Day == 25)
                            {
                                LoadBranchRequest();
                                loadItemSupplies();
                            }
                            else
                            {
                                lblMsg.Text = "This category does not allow to input/submit on this date.";
                                ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "Popup", "ShowSuccessMsg();", true);
                                return;
                            }
                        }

                        else if (ddCategory.SelectedValue == "4" | ddCategory.SelectedValue == "9")
                        {
                            
                                LoadBranchRequest();
                                loadItemSupplies();
                            
                        }

                   
                    }
                    else
                    {
                        LoadBranchRequest();
                        loadItemSupplies();
                    }
                
                }
                else
                {
                    LoadBranchRequest();
                    loadItemSupplies();
                }
            }
            
        }

        protected void ddItem_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddItem.SelectedItem.Text == "Please Select Item")
            {
                txtBalance.Text = string.Empty;
                txtQty.Text = string.Empty;
            }
            else
            {
                if (Convert.ToInt32(Session["UserBranchCode"].ToString()) == 1 | Convert.ToInt32(Session["UserBranchCode"].ToString()) >= 30)
                {
                    txtBalance.Text = "0";
                }
                else
                {
                    if (ddCategory.SelectedValue == "6" | ddCategory.SelectedValue == "9")
                    {
                        loadRemainingBalanceInSMS();
                    }
                    else
                    {
                        loadRemainingBalance();
                    }
                    
                }
            }
        }

        private void loadRemainingBalanceInSMS()
        {
            using (SqlConnection conN = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["MyConnSMS"].ConnectionString))
            {
                string stR = @"dbo.GetItemInfo";
                using (SqlCommand cmD = new SqlCommand(stR, conN))
                {
                    conN.Open();
                    cmD.CommandTimeout = 0;
                    cmD.CommandType = CommandType.StoredProcedure;
                    cmD.Parameters.AddWithValue("@vFGCode", ddItem.SelectedValue);
                    cmD.Parameters.AddWithValue("@BrCode", Session["UserBranchCode"].ToString());
                    DataTable dT = new DataTable();
                    SqlDataAdapter dA = new SqlDataAdapter(cmD);

                    dA.Fill(dT);
                    if (dT.Rows.Count > 0)
                    {

                        txtBalance.Text = dT.Rows[0]["EndBal"].ToString();
                    }
                    else
                    {
                        txtBalance.Text = "0";

                    }
                }
            }

            
        }

        private void loadRemainingBalance()
        {

            using (SqlConnection sqlConn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["MyConnCSI"].ConnectionString))
            {
                string stR = @"dbo.LoadRemainingBalanceBranch";
                sqlConn.Open();
                using (SqlCommand cmD = new SqlCommand(stR, sqlConn))
                {

                    cmD.CommandTimeout = 0;
                    cmD.CommandType = CommandType.StoredProcedure;
                    cmD.Parameters.AddWithValue("@BrCode", Session["UserBranchCode"].ToString());
                    cmD.Parameters.AddWithValue("@Sup_ItemCode", ddItem.SelectedValue);
                    DataTable dT = new DataTable();
                    SqlDataAdapter dA = new SqlDataAdapter(cmD);

                    dA.Fill(dT);
                    if (dT.Rows.Count > 0)
                    {

                        txtBalance.Text = dT.Rows[0]["EndBal"].ToString();
                    }
                    else
                    {
                        txtBalance.Text = "0";

                    }

                }

            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (IsPageRefresh == true)
            {
                Response.Redirect(Request.Url.AbsoluteUri);
            }
            else
            {
                try
                {
                    if (Convert.ToInt32(Session["UserBranchCode"]) >= 2 && Convert.ToInt32(Session["UserBranchCode"]) <= 8 | Convert.ToInt32(Session["UserBranchCode"]) >= 10 && Convert.ToInt32(Session["UserBranchCode"]) <= 35)
                    {
                        if (ddCategory.SelectedValue == "9")
                        {
                            if (ddRequestType.SelectedItem.Text == "Emergency")
                            {
                                if (ddItem.SelectedValue == "NSAAVC01" | ddItem.SelectedValue == "FNU31400" | ddItem.SelectedValue == "GLU00796" | ddItem.SelectedValue == "FNU31070"  | ddItem.SelectedValue == "CLN228" | ddItem.SelectedValue == "NADI0937" | ddItem.SelectedValue == "FNP70050" | ddItem.SelectedValue == "MICO0001" | ddItem.SelectedValue == "HYD00001" | ddItem.SelectedValue == "GLG24002" | ddItem.SelectedValue == "GLP24001" | ddItem.SelectedValue == "PSO00717" | ddItem.SelectedValue == "PSO0563" | ddItem.SelectedValue == "PRED0001" | ddItem.SelectedValue == "PRED0907")
                                {
                                    SaveItemRequest();
                                }
                                else
                                {
                                    if (DateTime.Now.Day == 10 | DateTime.Now.Day == 11 | DateTime.Now.Day == 22 | DateTime.Now.Day == 23 | DateTime.Now.Day == 24 | DateTime.Now.Day == 25)
                                    {
                                        SaveItemRequest();
                                    }
                                    else
                                    {
                                        lblMsg.Text = "This item does not allow to input/save on this date.";
                                        ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "Popup", "ShowSuccessMsg();", true);
                                        return;
                                    }
                                }
                            }
                            else
                            {
                                if (DateTime.Now.Day == 10 | DateTime.Now.Day == 11 | DateTime.Now.Day == 22 | DateTime.Now.Day == 23 | DateTime.Now.Day == 24 | DateTime.Now.Day == 25)
                                {
                                    SaveItemRequest();
                                }
                                else
                                {
                                    lblMsg.Text = "This item does not allow to input/save on this date.";
                                    ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "Popup", "ShowSuccessMsg();", true);
                                    return;

                                }
                            }
                        }
                        else
                        {

                            SaveItemRequest();
                        }
                    }
                    else
                    {
                        SaveItemRequest();
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

        private void SaveItemRequest()
        {
            using (SqlConnection sqlConn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["MyConnCSI"].ConnectionString))
            {
                string stR = @"dbo.SaveRequest";
                sqlConn.Open();
                using (SqlCommand cmD = new SqlCommand(stR, sqlConn))
                {
                    cmD.CommandTimeout = 0;
                    cmD.CommandType = CommandType.StoredProcedure;
                    cmD.Parameters.AddWithValue("@BrCode", Session["UserBranchCode"].ToString());
                    cmD.Parameters.AddWithValue("@Sup_CategoryNum", ddCategory.SelectedValue);
                    cmD.Parameters.AddWithValue("@Sup_ItemCode", ddItem.SelectedValue);
                    cmD.Parameters.AddWithValue("@Sup_Balance", txtBalance.Text);
                    cmD.Parameters.AddWithValue("@Sup_Qty", txtQty.Text);
                    cmD.Parameters.AddWithValue("@Sup_EncodedBy", Session["UserFullName"].ToString());
                    if (ddCategory.SelectedValue == "6")
                    {
                        cmD.Parameters.AddWithValue("@IsSelling", 1);
                    }
                    else if (ddCategory.SelectedValue == "9")
                    {
                        cmD.Parameters.AddWithValue("@IsSelling", 1);
                    }
                    else
                    {
                        cmD.Parameters.AddWithValue("@IsSelling", 0);
                    }
                    cmD.Parameters.AddWithValue("@RequiredDate", txtDateRequired.Text);
                    cmD.Parameters.AddWithValue("@RequestType", ddRequestType.SelectedValue);
                    cmD.Parameters.AddWithValue("@ReasonOfEmergency", txtReason.Text);
                    cmD.ExecuteNonQuery();
                }



            }

            loadItemSupplies();
            txtBalance.Text = string.Empty;
            txtQty.Text = string.Empty;
            LoadBranchRequest();
        }


        private void LoadBranchRequest()
        {
            try
            {
                using (SqlConnection sqlConn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["MyConnCSI"].ConnectionString))
                {
                    string stR = @"dbo.LoadBranchREquest";
                    using (SqlCommand cmD = new SqlCommand(stR, sqlConn))
                    {
                        sqlConn.Open();
                        cmD.CommandTimeout = 0;
                        cmD.CommandType = CommandType.StoredProcedure;
                        cmD.Parameters.AddWithValue("@BrCode", Session["UserBranchCode"].ToString());
                        cmD.Parameters.AddWithValue("@Category", ddCategory.SelectedValue);
                        cmD.Parameters.AddWithValue("@RType", ddRequestType.SelectedItem.Text);
                        DataTable dT = new DataTable();
                        SqlDataAdapter dA = new SqlDataAdapter(cmD);
                        dA.Fill(dT);

                        gvRequest.DataSource = dT;
                        gvRequest.DataBind();

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

        protected void gvRequest_PreRender(object sender, EventArgs e)
        {
            if (gvRequest.Rows.Count > 0)
            {
                gvRequest.UseAccessibleHeader = true;
                gvRequest.HeaderRow.TableSection = TableRowSection.TableHeader;
                gvRequest.FooterRow.TableSection = TableRowSection.TableFooter;
            }
        }

        protected void gvRequest_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            string RecNo = gvRequest.DataKeys[e.RowIndex].Value.ToString();
            lblConfirmRecID.Text = RecNo.ToString();
            lblItem.Text = gvRequest.Rows[e.RowIndex].Cells[2].Text;
            lblMsgConfirm.Text = "Are you sure you want to delete these item? ";
            ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "Popup", "ShowConfirmMsg();", true);
            return;
            //DeleteRequestItem(e);
        }
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
                        

                        string connString = System.Configuration.ConfigurationManager.ConnectionStrings["MyConnCSI"].ConnectionString;

                        string sql = @"Delete from [BranchRequest] where Sup_RequestID = @TheID";
                        using (SqlConnection conn = new SqlConnection(connString))
                        {
                            conn.Open();

                            using (SqlCommand cmD = new SqlCommand(sql, conn))
                            {
                                cmD.CommandTimeout = 0;
                                cmD.Parameters.AddWithValue("@TheID", lblConfirmRecID.Text);
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
                    LoadBranchRequest();
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
                //try
                //{
                    if (gvRequest.Rows.Count==0)
                    {
                        lblMsg.Text = "No input to submit";
                        ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "Popup", "ShowSuccessMsg();", true);
                        return;
                    }
                    else if (txtDateRequired.Text == string.Empty)
                    {
                        lblMsg.Text = "Please supply required date.";
                        ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "Popup", "ShowSuccessMsg();", true);
                        return;
                    }
                    else
                    {
                        //SubmitRequest();
                        //return;

                        if (ddCategory.SelectedValue == "5" | ddCategory.SelectedValue == "6")
                        {
                            if (Convert.ToInt32(Session["UserBranchCode"].ToString()) >= 50 | Convert.ToInt32(Session["UserBranchCode"].ToString()) == 1)
                            {
                                SubmitMRF_NoneBR();
                                return;
                            }
                            else
                            {
                                SubmitRequest();
                                return;
                            }
                        }
                        else
                        {
                            SubmitRequest();
                            return;
                        }
                    }
                //}
                //catch (Exception x)
                //{

                //    lblMsg.Text = x.Message;
                //    ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "Popup", "ShowSuccessMsg();", true);
                //    return;
                //}
            }
        }

        private void SubmitRequest()
        {
            try
            {
                getLastControlNo();

                foreach (GridViewRow gvR in gvRequest.Rows)
                {
                    if (gvR.RowType == DataControlRowType.DataRow)
                    {
                        using (SqlConnection sqlConn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["MyConnCSI"].ConnectionString))
                        {
                            string stR = @"dbo.SubmitRequest";
                            using (SqlCommand cmD = new SqlCommand(stR, sqlConn))
                            {
                                sqlConn.Open();
                                cmD.CommandTimeout = 0;
                                cmD.CommandType = CommandType.StoredProcedure;
                                cmD.Parameters.AddWithValue("@ID", gvR.Cells[0].Text);
                                cmD.Parameters.AddWithValue("@Sup_ControlNo", theRequestControlNo);
                                cmD.Parameters.AddWithValue("@Sup_EncodedBy", Session["UserFullName"].ToString());
                                cmD.Parameters.AddWithValue("@BrCode", Session["UserBranchCode"].ToString());
                                cmD.ExecuteNonQuery();
                            }
                        }

                    }
                }

                LoadBranchRequest();
                UpdateControlNo();
                lblMsg.Text = "Request successfully submitted.";
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

        public string theRequestControlNo;
        public int theControlNo;
        private void getLastControlNo()
        {
            try
            {
                using (SqlConnection sqlConn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["MyConnCSI"].ConnectionString))
                {
                    string stR = @"SELECT ControlNo FROM sup_RequestControlNo WHERE Brcode='" + Session["UserBranchCode"].ToString() + "'";

                    sqlConn.Open();
                    using (SqlCommand cmD = new SqlCommand(stR, sqlConn))
                    {
                        SqlDataReader dR = cmD.ExecuteReader();
                        cmD.CommandTimeout = 0;
                        while (dR.Read())
                        {
                            theControlNo = Convert.ToInt32(dR[0].ToString());
                            string sNum = theControlNo.ToString("00000");
                            theRequestControlNo = DateTime.Now.Year + "-" + Session["UserBranchCode"].ToString() + "-" + sNum;
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


        private void UpdateControlNo()
        {
            try
            {
                using (SqlConnection sqlConn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["MyConnCSI"].ConnectionString))
                {
                    string stR = @"dbo.UpdateControlNo";
                    using (SqlCommand cmD = new SqlCommand(stR, sqlConn))
                    {
                        sqlConn.Open();
                        cmD.CommandTimeout = 0;
                        cmD.CommandType = CommandType.StoredProcedure;
                        cmD.Parameters.AddWithValue("@BrCode", Session["UserBranchCode"].ToString());
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
        }


        public string theMrfNo;
        private void loadMRFNo()
        {
            try
            {
                using (SqlConnection conN = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["MyConnCSI"].ConnectionString))
                {
                    string stR = @"SELECT CAST([BrCode] AS VARCHAR(3)) + '-' + RIGHT('000000'+CAST(MRFNo AS VARCHAR(5)),5)  FROM MyBranchList where [BrName]='" + lblBranch.Text + "'";
                    using (SqlCommand cmD = new SqlCommand(stR, conN))
                    {
                        conN.Open();
                        SqlDataReader dR = cmD.ExecuteReader();
                        while (dR.Read())
                        {
                            Session["MRFNo"] = "MRF-" + dR[0].ToString();
                            theMrfNo = "MRF-" + dR[0].ToString();
                            //txtOrderNo.Text = dR[0].ToString();
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


        //private void SubmitMRF()
        //{
        //    if (IsPageRefresh == true)
        //    {
        //        Response.Redirect(Request.Url.AbsoluteUri);
        //    }
        //    else
        //    {
        //        //try
        //        //{
        //            loadMRFNo();

        //            foreach (GridViewRow row in gvRequest.Rows)
        //            {

        //                using (SqlConnection sqlConn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["MyConnCSI"].ConnectionString))
        //                {

        //                    string stRUpdateItemPack = @"dbo.SubmitMRF";
        //                    using (SqlCommand cmD = new SqlCommand(stRUpdateItemPack, sqlConn))
        //                    {
        //                        sqlConn.Open();
        //                        cmD.CommandTimeout = 0;
        //                        cmD.CommandType = CommandType.StoredProcedure;
        //                        //cmD.Parameters.AddWithValue("@OrderNumber",Session["MRFNo"].ToString());
        //                        cmD.Parameters.AddWithValue("@OrderNumber", theMrfNo.ToString());
                                
        //                        cmD.Parameters.AddWithValue("@Com_Company", "Dermclinic, Inc.");
        //                        cmD.Parameters.AddWithValue("@vCustCode", "");
        //                        cmD.Parameters.AddWithValue("@vCustName", lblBranch.Text);
        //                        cmD.Parameters.AddWithValue("@vReqDate", txtDateRequired.Text); //TO ADD
        //                        cmD.Parameters.AddWithValue("@vDPIcode", row.Cells[1].Text);
        //                        cmD.Parameters.AddWithValue("@vQty", row.Cells[5].Text);
        //                        cmD.Parameters.AddWithValue("@vOrigOrder", row.Cells[5].Text);
        //                        cmD.Parameters.AddWithValue("@vTrandate", DateTime.Now);
        //                        cmD.Parameters.AddWithValue("@vEncodedBy", Session["UserID"].ToString());
        //                        cmD.Parameters.AddWithValue("@vStatus", 0);
        //                        cmD.Parameters.AddWithValue("@Sup_RequestID", row.Cells[0].Text);
        //                        cmD.ExecuteNonQuery();

        //                    }
        //                }
        //            }




        //            using (SqlConnection sqlConn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["MyConnCSI"].ConnectionString))
        //            {

        //                string stRUpdateItemPack = @"UPDATE MyBranchList SET MRFNo = MRFNo + 1 WHERE [BrName] = @vCustName";
        //                using (SqlCommand cmD = new SqlCommand(stRUpdateItemPack, sqlConn))
        //                {
        //                    sqlConn.Open();
        //                    cmD.CommandTimeout = 0;
        //                    //cmD.CommandType = CommandType.StoredProcedure;
        //                    cmD.Parameters.AddWithValue("@Com_Company", "Dermclinic, Inc.");
        //                    cmD.Parameters.AddWithValue("@vCustName", lblBranch.Text);
        //                    cmD.ExecuteNonQuery();

        //                }
        //            }

        //            LoadBranchRequest();
        //            lblMsg.Text = "Request succesfully send to for approval of BM/ASM.";
        //            ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "Popup", "ShowSuccessMsg();", true);
        //            return;
        //        //}

        //        //catch (Exception x)
        //        //{
        //        //    lblMsg.Text = x.Message;
        //        //    ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "Popup", "ShowSuccessMsg();", true);
        //        //    return;
        //        //}

        //    }
        //}

        protected void ddRequestType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddRequestType.SelectedItem.Text == "Emergency")
            {
                txtReason.ReadOnly = false;
                RequiredReason.Enabled = true;
            }
            else
            {
                txtReason.ReadOnly = true;
                txtReason.Text = string.Empty;
                RequiredReason.Enabled = false;
            }

            loadCategory();

            //if (ddRequestType.SelectedValue != "0" & ddCategory.SelectedItem.Text != "Select Category")
            //{
            //                       LoadBranchRequest();
                    
            // }
        }



        private void SubmitMRF_NoneBR()
        {
            if (IsPageRefresh == true)
            {
                Response.Redirect(Request.Url.AbsoluteUri);
            }
            else
            {
                //try
                //{
                loadMRFNo();

                foreach (GridViewRow row in gvRequest.Rows)
                {

                    //Label lblQuantity = (Label)row.FindControl("lblQuantity");

                    using (SqlConnection sqlConn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["MyConnCSI"].ConnectionString))
                    {

                        string stRUpdateItemPack = @"dbo.SubmitMRF";
                        using (SqlCommand cmD = new SqlCommand(stRUpdateItemPack, sqlConn))
                        {
                            sqlConn.Open();
                            cmD.CommandTimeout = 0;
                            cmD.CommandType = CommandType.StoredProcedure;
                            cmD.Parameters.AddWithValue("@OrderNumber", theMrfNo.ToString());

                            cmD.Parameters.AddWithValue("@Com_Company", "Dermclinic, Inc.");
                            cmD.Parameters.AddWithValue("@vCustCode", "");
                            cmD.Parameters.AddWithValue("@vCustName", lblBranch.Text);
                            cmD.Parameters.AddWithValue("@vReqDate", txtDateRequired.Text); //TO ADD
                            cmD.Parameters.AddWithValue("@vDPIcode", row.Cells[1].Text);
                            cmD.Parameters.AddWithValue("@vQty", row.Cells[5].Text);
                            cmD.Parameters.AddWithValue("@vOrigOrder", row.Cells[5].Text);
                            cmD.Parameters.AddWithValue("@vTrandate", DateTime.Now);
                            cmD.Parameters.AddWithValue("@vEncodedBy", Session["UserID"].ToString());
                            cmD.Parameters.AddWithValue("@vStatus", 0);
                            cmD.Parameters.AddWithValue("@Sup_RequestID", row.Cells[0].Text);
                            cmD.Parameters.AddWithValue("@Category", ddCategory.SelectedItem.Text);
                            cmD.ExecuteNonQuery();

                        }
                    }
                }




                using (SqlConnection sqlConn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["MyConnCSI"].ConnectionString))
                {

                    string stRUpdateItemPack = @"UPDATE MyBranchList SET MRFNo = MRFNo + 1 WHERE [BrName] = @vCustName";
                    using (SqlCommand cmD = new SqlCommand(stRUpdateItemPack, sqlConn))
                    {
                        sqlConn.Open();
                        cmD.CommandTimeout = 0;
                        //cmD.CommandType = CommandType.StoredProcedure;
                        cmD.Parameters.AddWithValue("@Com_Company", "Dermclinic, Inc.");
                        cmD.Parameters.AddWithValue("@vCustName", lblBranch.Text);
                        cmD.ExecuteNonQuery();

                    }
                }

                LoadBranchRequest();


                lblMsg.Text = "Request succesfully send to ICS for Approval";
                ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "Popup", "ShowSuccessMsg();", true);
                return;

                //}

                //catch (Exception x)
                //{
                //    lblMsg.Text = x.Message;
                //    ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "Popup", "ShowSuccessMsg();", true);
                //    return;
                //}

            }
        }

        

    }

}