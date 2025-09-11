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
    public partial class EcommerceFreeEntry : System.Web.UI.Page
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
                    if (Session["UserBranchCode"].ToString() == "50" | Session["UserBranchCode"].ToString() == "51" | Session["UserBranchCode"].ToString() == "55")
                    {

                        loadItemForSelling();
                        LoadOrderPicked();
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

        



        private void loadItemForSelling()
        {
            using (SqlConnection sqlConn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["MyConnCSI"].ConnectionString))
            {

                string stR = @"SELECT DISTINCT a.sup_ItemCode,a.sup_ItemCode + ' - ' + b.sup_DESCRIPTION as [sup_DESCRIPTION] 
                                FROM [ITEM_RecordsHeader] a
                                LEFT JOIN Sup_ItemMaster b
                                ON a.sup_ItemCode=b.sup_ItemCode
                                WHERE a.BrCode=@BrCode and a.Sup_CategoryNum=8
								ORDER BY sup_DESCRIPTION";

                using (SqlCommand cmD = new SqlCommand(stR, sqlConn))
                {
                    sqlConn.Open();
                    cmD.CommandTimeout = 0;
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

        protected void btnShow_Click(object sender, EventArgs e)
        {
            try
            {
                using (SqlConnection conN = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["MyConnCSI"].ConnectionString))
                {
                    string stR1 = @"SELECT DISTINCT ReferenceNo FROM ITEM_OnlineSold WHERE ReferenceNo=@RefNumber and vStat > 0";
                    using (SqlCommand cmD = new SqlCommand(stR1, conN))
                    {
                        conN.Open();
                        cmD.CommandTimeout = 0;
                        cmD.Parameters.AddWithValue("@RefNumber", txtOrderNo.Text.Trim());
                        SqlDataAdapter dA = new SqlDataAdapter(cmD);
                        DataTable dT = new DataTable();
                        dA.Fill(dT);
                        if (dT.Rows.Count > 0)
                        {
                            lblMsg.Text = "Reference Number already used!";
                            ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "Popup", "ShowSuccessMsg();", true);
                            return;
                        }
                        else
                        {
                            ShowItmBatches();
                        }

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

        private void ShowItmBatches()
        {
            using (SqlConnection Conn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["MyConnCSI"].ConnectionString))
            {


                string stRGetBal = @"dbo.GetItemBatchesOnline";
                using (SqlCommand cmD = new SqlCommand(stRGetBal, Conn))
                {
                    Conn.Open();
                    cmD.CommandType = CommandType.StoredProcedure;
                    cmD.Parameters.AddWithValue("@ItemCode", ddItem.SelectedValue);
                    cmD.Parameters.AddWithValue("@CategoryType", 8);
                    cmD.Parameters.AddWithValue("@BrCode", Session["UserBranchCode"].ToString());
                    DataTable dT = new DataTable();
                    SqlDataAdapter dA = new SqlDataAdapter(cmD);
                    dA.Fill(dT);

                    if (dT.Rows.Count > 0)
                    {
                        lblItemDesc.Text = ddItem.SelectedItem.Text;
                        gvItemBatch.DataSource = dT;
                        gvItemBatch.DataBind();
                        ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "Popup", "ShowGridItemBatch();", true);
                        return;
                    }
                    else
                    {
                        lblItemDesc.Text = string.Empty;
                        lblMsg.Text = "No Available Balance!";
                        ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "Popup", "ShowSuccessMsg();", true);
                        return;
                    }
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
                //try
                //{
                GridViewRow row = gvItemBatch.Rows[e.RowIndex];
                TextBox textQtyPicked = (TextBox)row.FindControl("txtQtyPicked");
                TextBox textSRP = (TextBox)row.FindControl("txtSRP");

                using (SqlConnection sqlConn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["MyConnCSI"].ConnectionString))
                {
                    string stR = @"dbo.InsertIntoItemSoldFree";

                    using (SqlCommand cmD = new SqlCommand(stR, sqlConn))
                    {
                        sqlConn.Open();
                        cmD.CommandTimeout = 0;
                        cmD.CommandType = CommandType.StoredProcedure;
                        cmD.Parameters.AddWithValue("@TransactionType", 5);
                        cmD.Parameters.AddWithValue("@Sup_CategoryNum", 8);
                        cmD.Parameters.AddWithValue("@HeaderID", gvItemBatch.Rows[e.RowIndex].Cells[0].Text);
                        cmD.Parameters.AddWithValue("@Sup_ItemCode", ddItem.SelectedValue);
                        cmD.Parameters.AddWithValue("@vQtyPicked", textQtyPicked.Text);
                        cmD.Parameters.AddWithValue("@vUnitCost", textSRP.Text);
                        cmD.Parameters.AddWithValue("@vPickedBy", Session["UserFullName"].ToString());
                        cmD.Parameters.AddWithValue("@Type", ddType.SelectedItem.Text);
                        cmD.ExecuteNonQuery();

                    }
                }



                LoadOrderPicked();



                //}
                //catch (Exception x)
                //{
                //    lblMsg.Text = x.Message;
                //    ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "Popup", "ShowSuccessMsg();", true);
                //    return;
                //}
            }
        }




        private void LoadOrderPicked()
        {
            if (IsPageRefresh == true)
            {
                Response.Redirect(Request.Url.AbsoluteUri);
            }
            else
            {
                //try
                //{
                using (SqlConnection sqlConn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["MyConnCSI"].ConnectionString))
                {
                    string stR = @"SELECT a.[OrderID]
                                      ,a.[HeaderID]
                                      ,a.[Sup_ItemCode]
	                                  ,b.sup_DESCRIPTION
	                                  ,c.vDateExpiry
	                                  ,c.vBatchNo
                                      ,a.[vQtyPicked]
                                      ,a.[vUnitCost]
                                      ,CASE WHEN ISNULL(a.[vUnitCost],0) * ISNULL(a.[vQtyPicked],0) < 0 THEN 0
									  ELSE ISNULL(a.[vUnitCost],0) * ISNULL(a.[vQtyPicked],0)  END AS [Amount]
                                  FROM [ITEM_OnlineSold] a
                                  LEFT JOIN Sup_ItemMaster b
                                  ON a.[Sup_ItemCode]=b.[Sup_ItemCode]
                                  LEFT JOIN ITEM_RecordsHeader c
                                  ON a.[HeaderID]=C.[HeaderID]
                                  WHERE a.[TransactionType]=5 and a.Sup_CategoryNum=8 AND a.vStat=0 AND a.Source='" + ddType.SelectedItem.Text + "' and vPickedBy=@PickedBy";

                    using (SqlCommand cmD = new SqlCommand(stR, sqlConn))
                    {
                        sqlConn.Open();
                        cmD.CommandTimeout = 0;
                        cmD.Parameters.AddWithValue("@PickedBy", Session["UserFullName"].ToString());

                        SqlDataAdapter dA = new SqlDataAdapter(cmD);
                        DataTable dT = new DataTable();
                        dA.Fill(dT);
                        gvPicked.DataSource = dT;
                        gvPicked.DataBind();
                    }
                }
                //}
                //catch (SqlException x)
                //{
                //    lblMsg.Text = x.GetType().ToString();
                //    ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "Popup", "ShowSuccessMsg();", true);
                //    return;
                //}
                //catch (Exception y)
                //{
                //    lblMsg.Text = y.GetType().ToString();
                //    ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "Popup", "ShowSuccessMsg();", true);
                //    return;
                //}
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
                    string sql = @"Delete from ITEM_OnlineSold  where OrderID='" + RecNo + "'";
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
                    lblMsg.Text = "Please generate item to post.";
                    ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "Popup", "ShowSuccessMsg();", true);
                    return;
                }
                else
                {
                    foreach (GridViewRow gvR in gvPicked.Rows)
                    {
                        if (gvR.RowType == DataControlRowType.DataRow)
                        {
                         
                            using (SqlConnection sqlConn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["MyConnCSI"].ConnectionString))
                            {
                                string stR = @"dbo.PostItemSoldECommerceFree";

                                using (SqlCommand cmD = new SqlCommand(stR, sqlConn))
                                {
                                    sqlConn.Open();
                                    cmD.CommandTimeout = 0;
                                    cmD.CommandType = CommandType.StoredProcedure;
                                    cmD.Parameters.AddWithValue("@OrderID", gvR.Cells[0].Text);
                                    cmD.Parameters.AddWithValue("@HeaderID", gvR.Cells[1].Text);
                                    cmD.Parameters.AddWithValue("@BrCode", Session["UserBranchCode"].ToString());
                                    cmD.Parameters.AddWithValue("@ReferenceNo", txtOrderNo.Text);
                                    cmD.Parameters.AddWithValue("@OrderDate", txtDate.Text);
                                    cmD.Parameters.AddWithValue("@Source",ddType.SelectedItem.Text);
                                    cmD.Parameters.AddWithValue("@vQtyOnline", gvR.Cells[6].Text);
                                    cmD.ExecuteNonQuery();
                                }
                            }

                        }
                    }

                    ClearForm();
                    LoadOrderPicked();
                    lblMsg.Text = "Successfully Posted";
                    ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "Popup", "ShowSuccessMsg();", true);
                    return;
                }
            }

        }


        private void ClearForm()
        {
            if (IsPageRefresh == true)
            {
                Response.Redirect(Request.Url.AbsoluteUri);
            }
            else
            {
                //txtAddress.Text = string.Empty;
                //txtContactNo.Text = string.Empty;
                //txtCustomerName.Text = string.Empty;
                txtDate.Text = string.Empty;
                //txtEmailAddress.Text = string.Empty;
                txtOrderNo.Text = string.Empty;
                //txtShippingFee.Text = string.Empty;
                //txtDeliveryInstruction.Text = string.Empty;

                
                loadItemForSelling();

                //ddCity.DataSource = null;
                //ddCity.Items.Clear();
            }



        }

        protected void ddItem_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                using (SqlConnection conN = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["MyConnCSI"].ConnectionString))
                {
                    string stR1 = @"SELECT DISTINCT ReferenceNo FROM ITEM_OnlineSold WHERE ReferenceNo=@RefNumber and vStat > 0";
                    using (SqlCommand cmD = new SqlCommand(stR1, conN))
                    {
                        conN.Open();
                        cmD.CommandTimeout = 0;
                        cmD.Parameters.AddWithValue("@RefNumber", txtOrderNo.Text.Trim());
                        SqlDataAdapter dA = new SqlDataAdapter(cmD);
                        DataTable dT = new DataTable();
                        dA.Fill(dT);
                        if (dT.Rows.Count > 0)
                        {
                            lblMsg.Text = "Reference Number already used!";
                            ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "Popup", "ShowSuccessMsg();", true);
                            return;
                        }
                        else
                        {
                            ShowItmBatches();
                        }

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

        protected void ddType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddType.SelectedValue == "0")
            {
                ddItem.DataSource = null;
                ddItem.Items.Clear();
            }
            else
            {
                LoadOrderPicked();
            }
        }


    }
}