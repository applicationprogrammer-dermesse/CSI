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
    public partial class PREntry : System.Web.UI.Page
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
                    loadUnpostedPREntry();
                    loadPRNo();
         
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


        private void loadPRNo()
        {
            using (SqlConnection conN = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["MyConnCSI"].ConnectionString))
            {
                string stR = @"SELECT SUBSTRING(CAST(YEAR(GETDATE()) AS VARCHAR(4)),3,2) +  '-' + RIGHT('00000'+CAST(PRNo AS VARCHAR(5)),5)  FROM SystemMaster where [BrCode]='" + Session["UserBranchCode"].ToString() + "'";
                using (SqlCommand cmD = new SqlCommand(stR, conN))
                {
                    conN.Open();
                    SqlDataReader dR = cmD.ExecuteReader();
                    while (dR.Read())
                    {
                        txtPRNumber.Text = dR[0].ToString();
                    }

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
                                WHERE b.sup_DESCRIPTION IS NOT NULL
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

        protected void ddItem_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddItem.SelectedValue != "0")
            {
                getItemBalance();
            }
            else
            {
                txtBalance.Text = string.Empty;
            }
        }


        private void getItemBalance()
        {
            using (SqlConnection sqlConn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["MyConnCSI"].ConnectionString))
            {

                string stR = @"dbo.GetItemBalance";

                using (SqlCommand cmD = new SqlCommand(stR, sqlConn))
                {
                    sqlConn.Open();
                    cmD.CommandTimeout = 0;
                    cmD.CommandType = CommandType.StoredProcedure;
                    cmD.Parameters.AddWithValue("@ItemCode", ddItem.SelectedValue);
                    DataTable dT = new DataTable();
                    SqlDataAdapter dA = new SqlDataAdapter(cmD);
                    dA.Fill(dT);

                    if (dT.Rows.Count == 0)
                    {
                        txtBalance.Text = "0";
                    }
                    else
                    {
                        txtBalance.Text = dT.Rows[0][1].ToString();
                    }

                }
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                using (SqlConnection sqlConn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["MyConnCSI"].ConnectionString))
                {

                    string stR = @"dbo.InsertIntoITEM_PREntry";

                    using (SqlCommand cmD = new SqlCommand(stR, sqlConn))
                    {
                        sqlConn.Open();
                        cmD.CommandTimeout = 0;
                        cmD.CommandType = CommandType.StoredProcedure;
                        cmD.Parameters.AddWithValue("@RequiredDate",txtDateRequired.Text);
                        cmD.Parameters.AddWithValue("@BrCode", Session["UserBranchCode"].ToString());
						cmD.Parameters.AddWithValue("@sup_ItemCode",ddItem.SelectedValue);
						cmD.Parameters.AddWithValue("@sup_DESCRIPTION",ddItem.SelectedItem.Text);
						cmD.Parameters.AddWithValue("@vQtyBalance",txtBalance.Text);
						cmD.Parameters.AddWithValue("@vQtyRequest",txtQty.Text);
						cmD.Parameters.AddWithValue("@EncodedBy",Session["UserFullName"].ToString());
                        cmD.ExecuteNonQuery();


                    }
                }


                txtBalance.Text = string.Empty;                
                txtQty.Text = string.Empty;
                loadItemSupplies();
                loadUnpostedPREntry();
            }
            catch (Exception x)
            {
                lblMsg.Text = x.Message;
                ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "Popup", "ShowSuccessMsg();", true);
                return;
            }

            
        }


        private void loadUnpostedPREntry()
        {
            using (SqlConnection sqlConn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["MyConnCSI"].ConnectionString))
            {

                string stR = @"SELECT a.[RecNum],a.RequiredDate,b.BrName
                                  ,a.[sup_ItemCode]
                                  ,a.[sup_DESCRIPTION]
                                  ,a.[vQtyBalance]
                                  ,a.[vQtyRequest]
								  ,a.EncodedBy
                              FROM [ITEM_PREntry] a 
							  LEFT JOIN MyBranchList b
							  ON a.BrCode=b.BrCode
							  WHERE a.[vStat]=0 AND a.BrCode='" + Session["UserBranchCode"].ToString() + "'";

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

        protected void gvItems_RowEditing(object sender, GridViewEditEventArgs e)
        {
            gvItems.EditIndex = e.NewEditIndex;
            loadUnpostedPREntry();
        }

        protected void gvItems_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            gvItems.EditIndex = -1;
            loadUnpostedPREntry();
        }

        protected void gvItems_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            try
            {
                string TheID = gvItems.DataKeys[e.RowIndex].Value.ToString();

                string connString = System.Configuration.ConfigurationManager.ConnectionStrings["MyConnCSI"].ConnectionString;

                string sql = @"DELETE FROM ITEM_PREntry where RecNum = @TheID";
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

            loadUnpostedPREntry();
        }

        protected void gvItems_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            try
            {
                string TheRecNum = gvItems.DataKeys[e.RowIndex].Value.ToString();

                GridViewRow row = gvItems.Rows[e.RowIndex];
                TextBox txtrQuantity = (TextBox)row.FindControl("txtvQuantity");
                
                string connString = System.Configuration.ConfigurationManager.ConnectionStrings["MyConnCSI"].ConnectionString;

                string sql = @"Update ITEM_PREntry  set vQtyRequest=@vQty
                            where RecNum = @RecNum";
                using (SqlConnection conn = new SqlConnection(connString))
                {
                    conn.Open();

                    using (SqlCommand cmD = new SqlCommand(sql, conn))
                    {
                        cmD.CommandTimeout = 0;
                        cmD.Parameters.AddWithValue("@RecNum", TheRecNum.ToString());
                        cmD.Parameters.AddWithValue("@vQty", txtrQuantity.Text);
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
            loadUnpostedPREntry();
        }

        protected void btnRefresh_Click(object sender, EventArgs e)
        {

            UpdatePRNumber();
        }

        private void UpdatePRNumber()
        {
            using (SqlConnection conN = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["MyConnCSI"].ConnectionString))
            {


                string stR = @"UPDATE SystemMaster SET PRNo=PRNo + 1 WHERE BrCode=@BrCode";
                using (SqlCommand cmD = new SqlCommand(stR, conN))
                {
                    conN.Open();
                    SqlTransaction myTrans = conN.BeginTransaction();
                    cmD.Transaction = myTrans;
                    try
                    {
                        cmD.CommandTimeout = 0;
                        cmD.Parameters.AddWithValue("@BrCode", Session["UserBranchCode"].ToString());
                        cmD.ExecuteNonQuery();
                        myTrans.Commit();
                    }
                    catch (Exception x)
                    {
                        myTrans.Rollback();
                        lblMsg.Text = x.Message;
                        ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "Popup", "ShowSuccessMsg();", true);
                        return;
                    }

                }
            }

            loadPRNo();
        }

        protected void btnPost_Click(object sender, EventArgs e)
        {
            if (IsPageRefresh == true)
            {
                Response.Redirect(Request.Url.AbsoluteUri);
            }
            else
            {
                CheckIfPRNoExists();
            }
        }

        private void CheckIfPRNoExists()
        {
            if (IsPageRefresh == true)
            {
                Response.Redirect(Request.Url.AbsoluteUri);
            }
            else
            {
                if (Session["UserFullName"] == null)
                {
                    ScriptManager.RegisterStartupScript(Page, Page.GetType(), "redirect script",
                    "alert('You been idle for a long period of time, Need to Sign in again!'); location.href='Login.aspx';", true);
                }
                else
                {
                    using (SqlConnection conN = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["MyConnCSI"].ConnectionString))
                    {
                        string stR = @"SELECT PRNo FROM ITEM_PREntry WHERE PRNo=@PRNo and vStat=1";
                        using (SqlCommand cmD = new SqlCommand(stR, conN))
                        {
                            conN.Open();
                            cmD.Parameters.AddWithValue("@PRNo", txtPRNumber.Text);
                            SqlDataReader dR = cmD.ExecuteReader();

                            if (dR.HasRows)
                            {
                                btnRefresh.Focus();
                                lblMsg.Text = "PR number " + txtPRNumber.Text + " already used. Please click refresh button.";
                                ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "Popup", "ShowSuccessMsg();", true);
                                return;


                            }
                            else
                            {
                                PosttUnpostedEntry();

                            }


                        }


                    }
                }
            }

 
        }



        private void PosttUnpostedEntry()
        {
            if (IsPageRefresh == true)
            {
                Response.Redirect(Request.Url.AbsoluteUri);
            }
            else
            {
                try
                {
                    foreach (GridViewRow row in gvItems.Rows)
                    {
                        using (SqlConnection conN = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["MyConnCSI"].ConnectionString))
                        {
                            string stR = @"UPDATE ITEM_PREntry set vStat=1, PRNo=@PRNo where RecNum=@RecNum";
                            using (SqlCommand cmD = new SqlCommand(stR, conN))
                            {
                                conN.Open();
                                cmD.CommandTimeout = 0;
                                cmD.Parameters.AddWithValue("@RecNum", row.Cells[0].Text);
                                cmD.Parameters.AddWithValue("@PRNo", txtPRNumber.Text);
                                cmD.ExecuteNonQuery();



                            }
                        }
                    }

                    loadUnpostedPREntry();
                    UpdatePRNumber();
                    lblMsg.Text = "Succesfully Posted";
                    ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "Popup", "ShowSuccessMsg();", true);
                    return;
                }
                catch(Exception x)
                {
                    lblMsg.Text = x.Message;
                    ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "Popup", "ShowSuccessMsg();", true);
                    return;
                }

            }
        }




    }
}