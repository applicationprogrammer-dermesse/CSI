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
    public partial class RGASForDisposal : System.Web.UI.Page
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


                    loadItemForDisposalRGAS();
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

        private void loadItemForDisposalRGAS()
        {
            using (SqlConnection sqlConn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["MyConnCSI"].ConnectionString))
            {

                string stR = @"SELECT DISTINCT a.sup_ItemCode,a.sup_ItemCode + ' - ' + b.sup_DESCRIPTION as [sup_DESCRIPTION] 
                                FROM [ITEM_RGASHeader] a
                                LEFT JOIN Sup_ItemMaster b
                                ON a.sup_ItemCode=b.sup_ItemCode
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


        protected void btnShow_Click(object sender, EventArgs e)
        {

            using (SqlConnection Conn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["MyConnCSI"].ConnectionString))
            {


                string stRGetBal = @"dbo.GetItemBatchesRGAS";
                using (SqlCommand cmD = new SqlCommand(stRGetBal, Conn))
                {
                    Conn.Open();
                    cmD.CommandType = CommandType.StoredProcedure;
                    cmD.Parameters.AddWithValue("@ItemCode", ddItem.SelectedValue);
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
                    TextBox textQtyPicked = (TextBox)row.FindControl("txtQtyPicked");

                    using (SqlConnection sqlConn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["MyConnCSI"].ConnectionString))
                    {
                        string stR = @"dbo.InsertIntoItemPickRGAS";

                        using (SqlCommand cmD = new SqlCommand(stR, sqlConn))
                        {
                            sqlConn.Open();
                            cmD.CommandTimeout = 0;
                            cmD.CommandType = CommandType.StoredProcedure;
                            cmD.Parameters.AddWithValue("@HeaderID", gvItemBatch.Rows[e.RowIndex].Cells[0].Text);
                            cmD.Parameters.AddWithValue("@Sup_ItemCode", ddItem.SelectedValue);
                            cmD.Parameters.AddWithValue("@vBatchNo", gvItemBatch.Rows[e.RowIndex].Cells[2].Text.TrimEnd());
                            cmD.Parameters.AddWithValue("@vDateExpiry", gvItemBatch.Rows[e.RowIndex].Cells[3].Text);
                            cmD.Parameters.AddWithValue("@vQtyPicked", textQtyPicked.Text);
                            cmD.Parameters.AddWithValue("@vPickedBy", Session["UserFullName"].ToString());
                            cmD.ExecuteNonQuery();

                        }
                    }

                    LoadOrderPicked();





                }
                catch (Exception x)
                {
                    lblMsg.Text = x.Message;
                    ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "Popup", "ShowSuccessMsg();", true);
                    return;
                }
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
                try
                {
                    using (SqlConnection sqlConn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["MyConnCSI"].ConnectionString))
                    {
                        string stR = @"SELECT a.vRecnum,a.[HeaderID]
                                      ,a.[Sup_ItemCode]
	                                  ,b.sup_DESCRIPTION
	                                  ,c.vDateExpiry
	                                  ,c.vBatchNo
                                      ,a.[vQtyPicked]
                                  FROM [ITEM_PickRGAS] a
                                  LEFT JOIN Sup_ItemMaster b
                                  ON a.[Sup_ItemCode]=b.[Sup_ItemCode]
                                  LEFT JOIN ITEM_PickRGAS c
                                  ON a.[HeaderID]=C.[HeaderID]
                                  WHERE a.vStat=0 
                                  and a.vPickedBy=@PickedBy";

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
                    string sql = @"Delete from ITEM_PickRGAS  where vRecnum='" + RecNo + "'";
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
                                string stR = @"dbo.PostItemRGASDisposal";

                                using (SqlCommand cmD = new SqlCommand(stR, sqlConn))
                                {
                                    sqlConn.Open();
                                    cmD.CommandTimeout = 0;
                                    cmD.CommandType = CommandType.StoredProcedure;
                                    cmD.Parameters.AddWithValue("@vRecnum", gvR.Cells[0].Text);
                                    cmD.Parameters.AddWithValue("@PostedDate", txtDate.Text);
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

                txtDate.Text = string.Empty;


            }



        }




    }
}