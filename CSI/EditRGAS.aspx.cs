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
    public partial class EditRGAS : System.Web.UI.Page
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


                    loadItemForSellingRGAS();
                    

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

        private void loadItemForSellingRGAS()
        {
            using (SqlConnection sqlConn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["MyConnCSI"].ConnectionString))
            {

                string stR = @"SELECT DISTINCT a.sup_ItemCode,a.sup_ItemCode + ' - ' + b.sup_DESCRIPTION as [sup_DESCRIPTION] 
                                FROM [ITEM_RecordsHeader] a
                                LEFT JOIN Sup_ItemMaster b
                                ON a.sup_ItemCode=b.sup_ItemCode
                                WHERE a.BrCode=1 and a.Sup_CategoryNum=7
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
            if (ddItem.SelectedValue == "0")
            {
                txtBalance.Text = string.Empty;
                txtBatchNo.Text = string.Empty;
                txtDateExpiry.Text = string.Empty;
                txtID.Text = string.Empty;
            }
            else
            {
                using (SqlConnection Conn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["MyConnCSI"].ConnectionString))
                {


                    string stRGetBal = @"dbo.GetItemBatches";
                    using (SqlCommand cmD = new SqlCommand(stRGetBal, Conn))
                    {
                        Conn.Open();
                        cmD.CommandType = CommandType.StoredProcedure;
                        cmD.Parameters.AddWithValue("@ItemCode", ddItem.SelectedValue);
                        cmD.Parameters.AddWithValue("@CategoryType", 7);
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
                            txtBalance.Text = string.Empty;
                            txtBatchNo.Text = string.Empty;
                            txtDateExpiry.Text = string.Empty;
                            txtID.Text = string.Empty;

                            lblMsg.Text = "No Available Balance!";
                            ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "Popup", "ShowSuccessMsg();", true);
                            return;
                        }
                    }
                }
            }
        }

        protected void btnShow_Click(object sender, EventArgs e)
        {
            using (SqlConnection Conn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["MyConnCSI"].ConnectionString))
            {


                string stRGetBal = @"dbo.GetItemBatches";
                using (SqlCommand cmD = new SqlCommand(stRGetBal, Conn))
                {
                    Conn.Open();
                    cmD.CommandType = CommandType.StoredProcedure;
                    cmD.Parameters.AddWithValue("@ItemCode", ddItem.SelectedValue);
                    cmD.Parameters.AddWithValue("@CategoryType", 7);
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
                    TextBox textBalance = (TextBox)row.FindControl("txtBalance");
                    
                    txtID.Text = row.Cells[0].Text;
                    txtBatchNo.Text = row.Cells[2].Text;
                    txtDateExpiry.Text = row.Cells[3].Text;
                    txtBalance.Text = textBalance.Text;

                }
                catch (Exception x)
                {
                    lblMsg.Text = x.Message;
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
                if (txtID.Text == string.Empty)
                {
                    lblMsg.Text = "Please select item batch to update date expiry";
                    ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "Popup", "ShowSuccessMsg();", true);
                    return;
                }
                else
                {
                    try
                    {
                         using (SqlConnection sqlConn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["MyConnCSI"].ConnectionString))
                            {
                                string stR = @"UPDATE [dbo].[ITEM_RecordsHeader]
                                                set [vDateExpiry] = @vDateExpiry where HeaderID=@ID";

                                using (SqlCommand cmD = new SqlCommand(stR, sqlConn))
                                {
                                    sqlConn.Open();
                                    cmD.CommandTimeout = 0;
                                    
                                    cmD.Parameters.AddWithValue("@ID",txtID.Text);
                                    cmD.Parameters.AddWithValue("@vDateExpiry", txtNewDateExpiry.Text);
                                    cmD.ExecuteNonQuery();
                                }
                            }

                         txtID.Text = string.Empty;
                         txtBatchNo.Text = string.Empty;
                         txtDateExpiry.Text = string.Empty;
                         txtBalance.Text = string.Empty;
                         txtNewDateExpiry.Text = string.Empty;

                         loadItemForSellingRGAS();
                         lblMsg.Text = "Item date expiry successfully updated.";
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