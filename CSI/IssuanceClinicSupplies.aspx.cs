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
    public partial class IssuanceClinicSupplies : System.Web.UI.Page
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
                    loadIssuanceNo();
                    loadAllBranch();
                    loadCategory();
                    LoadOrderPicked();

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


        private void loadAllBranch()
        {
            try
            {
                using (SqlConnection conN = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["MyConnCSI"].ConnectionString))
                {
                    string stR = @"dbo.LoadAllDepartments";
                    using (SqlCommand cmD = new SqlCommand(stR, conN))
                    {
                        conN.Open();
                        cmD.CommandTimeout = 0;
                        cmD.CommandType = CommandType.StoredProcedure;
                        SqlDataReader dR = cmD.ExecuteReader();

                        ddBranch.Items.Clear();
                        ddBranch.DataSource = dR;
                        ddBranch.DataValueField = "BrCode";
                        ddBranch.DataTextField = "BrName";
                        ddBranch.DataBind();
                        ddBranch.Items.Insert(0, new ListItem("Select branch", "0"));
                        ddBranch.SelectedIndex = 0;
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

        protected void btnShow_Click(object sender, EventArgs e)
        {

            ShowItemBatches();
        }

        private void ShowItemBatches()
        {
            try
            {
                using (SqlConnection Conn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["MyConnCSI"].ConnectionString))
                {


                    string stRGetBal = @"dbo.GetItemBatches";
                    using (SqlCommand cmD = new SqlCommand(stRGetBal, Conn))
                    {
                        Conn.Open();
                        cmD.CommandType = CommandType.StoredProcedure;
                        cmD.Parameters.AddWithValue("@ItemCode", ddItem.SelectedValue);
                        cmD.Parameters.AddWithValue("@CategoryType", 0);
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

        private void loadIssuanceNo()
        {
            try
            {
                using (SqlConnection conN = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["MyConnSMS"].ConnectionString))
                {
                    string stR = @"SELECT CONCAT('IssNo-',[BrCode],'-',YEAR([CurrentDate]),'-',RIGHT('00000'+CAST(IssuanceNo AS VARCHAR(8)),8)) FROM SystemMaster where [BrCode]='1'";
                    using (SqlCommand cmD = new SqlCommand(stR, conN))
                    {
                        conN.Open();
                        SqlDataReader dR = cmD.ExecuteReader();
                        while (dR.Read())
                        {
                            txtIssuanceNo.Text = dR[0].ToString();
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
        private void loadItemForIssuance()
        {
            try
            {
                using (SqlConnection sqlConn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["MyConnCSI"].ConnectionString))
                {

                    string stR = @"dbo.loadItemForIssuance";

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
            catch (Exception x)
            {
                lblMsg.Text = x.Message;
                ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "Popup", "ShowSuccessMsg();", true);
                return;
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
                    TextBox xtQtyPicked = (TextBox)row.FindControl("txtQtyPicked");



                    string connString = System.Configuration.ConfigurationManager.ConnectionStrings["MyConnCSI"].ConnectionString;
                    string sql = @"GetItemBatchesBalance";
                    using (SqlConnection conn = new SqlConnection(connString))
                    {
                        conn.Open(); using (SqlCommand cmD = new SqlCommand(sql, conn))
                        {
                            cmD.CommandTimeout = 0;
                            cmD.CommandType = CommandType.StoredProcedure;
                            cmD.Parameters.AddWithValue("@HeaderID", row.Cells[0].Text);
                            //cmD.Parameters.AddWithValue("@HeaderID", 9505);
                            SqlDataAdapter dA = new SqlDataAdapter(cmD);
                            DataTable dT = new DataTable();
                            dA.Fill(dT);

                            int theBalanceHeader = Convert.ToInt32(dT.Rows[0][5]);
                            int thePickQty = Convert.ToInt32(xtQtyPicked.Text);
                            if (theBalanceHeader >= thePickQty)
                            {
                                InsertIntoItemPickCS(e);
                            }

                            else
                            {
                                lblMsg.Text = "Insufficient balance.<br />  Remaining balance is = " + dT.Rows[0][5].ToString() + "<br />There was an update on the qty balance of the selected item batch.<br /> Please reload item batches selected";
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

        private void InsertIntoItemPickCS(GridViewUpdateEventArgs e)
        {
            GridViewRow row = gvItemBatch.Rows[e.RowIndex];
            TextBox xtQtyPicked = (TextBox)row.FindControl("txtQtyPicked");
            Label lblDateExpiry = (Label)row.FindControl("lblvDateExpiry");
            using (SqlConnection sqlConn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["MyConnCSI"].ConnectionString))
            {
                string stR = @"dbo.InsertIntoItemPickCS";

                using (SqlCommand cmD = new SqlCommand(stR, sqlConn))
                {
                    sqlConn.Open();
                    cmD.CommandTimeout = 0;
                    cmD.CommandType = CommandType.StoredProcedure;
                    cmD.Parameters.AddWithValue("@OrderID", 0);
                    cmD.Parameters.AddWithValue("@TransactionType", 12);
                    //cmD.Parameters.AddWithValue("@BrCode", ddBranch.SelectedValue);
                    //cmD.Parameters.AddWithValue("@Sup_ControlNo", txtIssuanceNo.Text);

                    cmD.Parameters.AddWithValue("@Sup_CategoryNum", ddCategory.SelectedValue);
                    cmD.Parameters.AddWithValue("@HeaderID", gvItemBatch.Rows[e.RowIndex].Cells[0].Text);
                    cmD.Parameters.AddWithValue("@Sup_ItemCode", ddItem.SelectedValue);
                    cmD.Parameters.AddWithValue("@Sup_NoOfItemReq", xtQtyPicked.Text);
                    cmD.Parameters.AddWithValue("@vQtyPicked", xtQtyPicked.Text);
                    cmD.Parameters.AddWithValue("@vBatchNo", gvItemBatch.Rows[e.RowIndex].Cells[2].Text);
                    cmD.Parameters.AddWithValue("@vDateExpiry", lblDateExpiry.Text);
                    cmD.Parameters.AddWithValue("@vStat", 0);
                    cmD.Parameters.AddWithValue("@vPickedBy", Session["UserFullName"].ToString());

                    cmD.ExecuteNonQuery();

                }
            }


            LoadOrderPicked();
        }


        private void LoadOrderPicked()
        {

            try
            {
            using (SqlConnection sqlConn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["MyConnCSI"].ConnectionString))
            {
                string stR = @"SELECT A.[vRecNum]
                                      ,A.[HeaderID]
                                      ,A.Sup_ControlNo
                                      ,A.[Sup_ItemCode]
	                                  ,b.sup_DESCRIPTION
                                      ,A.[vQtyPicked]
                                      ,A.[vBatchNo]
                                      ,A.[vDateExpiry]
                                  FROM [ITEM_Pick] A
                                  LEFT JOIN [Sup_ItemMaster] B
                                   ON A.[Sup_ItemCode]=B.Sup_ItemCode
                                WHERE A.[vPickedBy]=@vPickedBy
                                and TransactionType=12 and vStat=0 AND a.Sup_CategoryNum=@Category
                                ORDER BY A.[Sup_ItemCode]";

                using (SqlCommand cmD = new SqlCommand(stR, sqlConn))
                {
                    sqlConn.Open();
                    cmD.CommandTimeout = 0;
                    cmD.Parameters.AddWithValue("@vPickedBy", Session["UserFullName"].ToString());
                    cmD.Parameters.AddWithValue("@Category", ddCategory.SelectedValue);
                    SqlDataAdapter dA = new SqlDataAdapter(cmD);
                    DataTable dT = new DataTable();
                    dA.Fill(dT);
                    gvPicked.DataSource = dT;
                    gvPicked.DataBind();
                }
            }
            }
            catch (SqlException x)
            {
                lblMsg.Text = x.GetType().ToString();
                ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "Popup", "ShowSuccessMsg();", true);
                return;
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


                    LoadOrderPicked();
                }
                catch (Exception y)
                {
                    lblMsg.Text = y.Message;
                    ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "Popup", "ShowSuccessMsg();", true);
                    return;
                }
            }
        }


        protected void btnPrint_Click(object sender, EventArgs e)
        {
            if (IsPageRefresh == true)
            {
                Response.Redirect(Request.Url.AbsoluteUri);
            }
            else
            {
                Session["TheBranch"] = ddBranch.SelectedItem.Text;
                Session["TheNo"] = txtIssuanceNo.Text;
                Session["Category"] = ddCategory.SelectedValue;
                Session["TheType"] = "12";
                //Session["TheDeliveredBy"] = txtDeliveredBy.Text;
                //Session["TheDateDelivered"] = txtDateDeliverd.Text;

                Response.Write("<script>window.open ('PrintIssueSlip.aspx?iNo=" + txtIssuanceNo.Text + " ','_blank');</script>");
            }


        }

        //protected void btnPost_Click(object sender, EventArgs e)
        //{
        //    if (IsPageRefresh == true)
        //    {
        //        Response.Redirect(Request.Url.AbsoluteUri);
        //    }
        //    else
        //    {
        //        if (gvPicked.Rows.Count == 0)
        //        {
        //            lblMsg.Text = "No Record to Post";
        //            ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "Popup", "ShowSuccessMsg();", true);
        //            return;
        //        }
        //        else
        //        {


        //            foreach (GridViewRow gvR in gvPicked.Rows)
        //            {
        //                if (gvR.RowType == DataControlRowType.DataRow)
        //                {

        //                    //Label lblQuantity = (Label)gvR.FindControl("lblQuantity");

        //                    using (SqlConnection sqlConn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["MyConnCSI"].ConnectionString))
        //                    {
        //                        string stR = @"dbo.PostUnpostedIssuanceCS";

        //                        using (SqlCommand cmD = new SqlCommand(stR, sqlConn))
        //                        {
        //                            sqlConn.Open();
        //                            cmD.CommandTimeout = 0;
        //                            cmD.CommandType = CommandType.StoredProcedure;
                       

        //                            cmD.Parameters.AddWithValue("@RecNum", gvR.Cells[0].Text);
        //                            cmD.Parameters.AddWithValue("@BrCode", ddBranch.SelectedValue);
        //                            cmD.Parameters.AddWithValue("@Sup_ControlNo", txtIssuanceNo.Text);
        //                            cmD.Parameters.AddWithValue("@BrName", ddBranch.SelectedItem.Text);
        //                            cmD.Parameters.AddWithValue("@HeaderID", gvR.Cells[2].Text);
        //                            cmD.Parameters.AddWithValue("@Qty", gvR.Cells[5].Text);
        //                            cmD.Parameters.AddWithValue("@PostedDeliveredBranch", Session["UserFullName"].ToString());
        //                            cmD.Parameters.AddWithValue("@DatePostedBR", DateTime.Now);


        //                            cmD.Parameters.AddWithValue("@DRNumber", txtHardCopy.Text);
        //                            cmD.Parameters.AddWithValue("@DateDelivered",txtDateIssue.Text);
        //                            cmD.Parameters.AddWithValue("@DeliveredBy", txtDeliveredBy.Text.ToUpper());
        //                            cmD.Parameters.AddWithValue("@DatePostedDelivered", DateTime.Now);

        //                            cmD.ExecuteNonQuery();
        //                        }
        //                    }

        //                }


        //            }
        //            UpdateIssuanceNo();
        //            txtDateIssue.Text = string.Empty;
        //            txtDeliveredBy.Text = string.Empty;
        //            loadAllBranch();

        //            loadIssuanceNo();
        //            LoadOrderPicked();
        //            lblMsg.Text = "Successfully Posted";
        //            ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "Popup", "ShowSuccessMsg();", true);
        //            return;
        //        }
        //    }
        //}


        private void UpdateIssuanceNo()
        {
            using (SqlConnection conN = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["MyConnSMS"].ConnectionString))
            {


                string stR = @"UPDATE SystemMaster SET IssuanceNo=IssuanceNo + 1 WHERE BrCode=1";
                using (SqlCommand cmD = new SqlCommand(stR, conN))
                {
                    conN.Open();
                    cmD.CommandTimeout = 0;
                    cmD.ExecuteNonQuery();

                }
            }

        }

        protected void btnPostIssuane_Click(object sender, EventArgs e)
        {
            if (IsPageRefresh == true)
            {
                Response.Redirect(Request.Url.AbsoluteUri);
            }
            else
            {
                using (SqlConnection conN = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["MyConnCSI"].ConnectionString))
                {
                    string stR = @"SELECT Sup_ControlNo FROM ITEM_Pick WHERE Sup_ControlNo=@Sup_ControlNo";
                    using (SqlCommand cmD = new SqlCommand(stR, conN))
                    {
                        conN.Open();
                        cmD.Parameters.AddWithValue("@Sup_ControlNo", txtIssuanceNo.Text);
                        SqlDataReader dR = cmD.ExecuteReader();

                        if (dR.HasRows)
                        {

                            lblMsg.Text = "Issue slip number already used. Please click Refresh Button.";
                            ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "Popup", "ShowSuccessMsg();", true);
                            return;


                        }
                        else
                        {
                            PostIssuanceCS();
                            return;
                        }


                    }


                }
                
                
            }
        }

        private void PostIssuanceCS()
        {
            if (gvPicked.Rows.Count == 0)
            {
                lblMsg.Text = "No Record to Post";
                ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "Popup", "ShowSuccessMsg();", true);
                return;
            }
            else
            {


                foreach (GridViewRow gvR in gvPicked.Rows)
                {
                    if (gvR.RowType == DataControlRowType.DataRow)
                    {

                        //Label lblQuantity = (Label)gvR.FindControl("lblQuantity");

                        using (SqlConnection sqlConn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["MyConnCSI"].ConnectionString))
                        {
                            string stR = @"dbo.PostUnpostedIssuanceCS";

                            using (SqlCommand cmD = new SqlCommand(stR, sqlConn))
                            {
                                sqlConn.Open();
                                cmD.CommandTimeout = 0;
                                cmD.CommandType = CommandType.StoredProcedure;


                                cmD.Parameters.AddWithValue("@RecNum", gvR.Cells[0].Text);
                                cmD.Parameters.AddWithValue("@Sup_ControlNo", txtIssuanceNo.Text);
                                cmD.Parameters.AddWithValue("@BrCode", ddBranch.SelectedValue);
                                cmD.Parameters.AddWithValue("@BrName", ddBranch.SelectedItem.Text);
                                cmD.Parameters.AddWithValue("@HeaderID", gvR.Cells[1].Text);
                                cmD.Parameters.AddWithValue("@Qty", gvR.Cells[5].Text);
                                cmD.Parameters.AddWithValue("@PostedDeliveredBranch", Session["UserFullName"].ToString());
                                cmD.Parameters.AddWithValue("@DatePostedBR", DateTime.Now);
                                cmD.Parameters.AddWithValue("@DRNumber", txtHardCopy.Text);
                                cmD.Parameters.AddWithValue("@DateDelivered", txtDateIssue.Text);
                                cmD.Parameters.AddWithValue("@DeliveredBy", txtDeliveredBy.Text.ToUpper());
                                cmD.Parameters.AddWithValue("@DatePostedDelivered", DateTime.Now);

                                cmD.ExecuteNonQuery();
                            }
                        }

                    }


                }
                UpdateIssuanceNo();
                txtDateIssue.Text = string.Empty;
                txtDeliveredBy.Text = string.Empty;
                loadAllBranch();

                

                loadIssuanceNo();
                LoadOrderPicked();
                lblMsg.Text = "Successfully Posted";
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

            }
            else
            {
                loadItemForIssuance();
                LoadOrderPicked();
            }


        
        }

        private void loadCategory()
        {
            try
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
            catch (Exception x)
            {
                lblMsg.Text = x.Message;
                ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "Popup", "ShowSuccessMsg();", true);
                return;
            }
        }

        protected void ddItem_SelectedIndexChanged(object sender, EventArgs e)
        {
            ShowItemBatches();
        }

        protected void btnRefresh_Click(object sender, EventArgs e)
        {
            if (IsPageRefresh == true)
            {
                Response.Redirect(Request.Url.AbsoluteUri);
            }
            else
            {
                UpdateIssuanceNo();
                loadIssuanceNo();

            }
        }

    }
}