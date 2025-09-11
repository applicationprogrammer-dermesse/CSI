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
    public partial class TransferItem : System.Web.UI.Page
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

                    loadCategorySource();

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


        private void loadCategorySource()
        {
            using (SqlConnection conN = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["MyConnCSI"].ConnectionString))
            {
                string stR = @"dbo.loadCategorySource";
                using (SqlCommand cmD = new SqlCommand(stR, conN))
                {
                    conN.Open();
                    cmD.CommandTimeout = 0;
                    cmD.CommandType = CommandType.StoredProcedure;
                    SqlDataReader dR = cmD.ExecuteReader();

                    ddCategorySource.Items.Clear();
                    ddCategorySource.DataSource = dR;
                    ddCategorySource.DataValueField = "Sup_CategoryNum";
                    ddCategorySource.DataTextField = "Sup_CategoryName";
                    ddCategorySource.DataBind();
                    ddCategorySource.Items.Insert(0, new ListItem("Select Category", "0"));
                    ddCategorySource.SelectedIndex = 0;
                }
            }
        }


        private void loadCategoryTarget()
        {
            using (SqlConnection conN = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["MyConnCSI"].ConnectionString))
            {
                string stR = @"SELECT [Sup_CategoryNum]
                                      ,[Sup_CategoryName]
                                  FROM [Sup_Category] where [Sup_CategoryNum] not in ('" + ddCategorySource.SelectedValue + "','8','2','3') order by [Sup_CategoryName]";
                using (SqlCommand cmD = new SqlCommand(stR, conN))
                {
                    conN.Open();
                    cmD.CommandTimeout = 0;
                    SqlDataReader dR = cmD.ExecuteReader();

                    ddCategoryTarget.Items.Clear();
                    ddCategoryTarget.DataSource = dR;
                    ddCategoryTarget.DataValueField = "Sup_CategoryNum";
                    ddCategoryTarget.DataTextField = "Sup_CategoryName";
                    ddCategoryTarget.DataBind();
                    ddCategoryTarget.Items.Insert(0, new ListItem("Select Category", "0"));
                    ddCategoryTarget.SelectedIndex = 0;
                }
            }
        }

        protected void ddCategorySource_SelectedIndexChanged(object sender, EventArgs e)
        {
            loadCategoryTarget();
            if (ddCategorySource.SelectedValue == "0")
            {
                ddItem.DataSource = null;
                ddItem.Items.Clear();
            }

            else
            {
                loadItemToTransafer();
            }
        }

        private void loadItemToTransafer()
        {
            using (SqlConnection sqlConn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["MyConnCSI"].ConnectionString))
            {

                string stR = @"dbo.loadItemToTransaferToAnotherCategory";

                using (SqlCommand cmD = new SqlCommand(stR, sqlConn))
                {
                    sqlConn.Open();
                    cmD.CommandTimeout = 0;
                    cmD.CommandType = CommandType.StoredProcedure;
                    cmD.Parameters.AddWithValue("@Category", ddCategorySource.SelectedValue);
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
            ShowItemBatches();
        }


        private void ShowItemBatches()
        {
            using (SqlConnection Conn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["MyConnCSI"].ConnectionString))
            {


                string stRGetBal = @"dbo.GetItemBatchesForTransfer";
                using (SqlCommand cmD = new SqlCommand(stRGetBal, Conn))
                {
                    Conn.Open();
                    cmD.CommandType = CommandType.StoredProcedure;
                    cmD.Parameters.AddWithValue("@ItemCode", ddItem.SelectedValue);
                    cmD.Parameters.AddWithValue("@CategoryType", ddCategorySource.SelectedValue);
                    DataTable dT = new DataTable();
                    SqlDataAdapter dA = new SqlDataAdapter(cmD);
                    dA.Fill(dT);

                    if (dT.Rows.Count > 0)
                    {
                        lblItemtoTransfer.Text = ddItem.SelectedItem.Text;
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
                TextBox xtQtyPicked = (TextBox)row.FindControl("txtQtyPicked");
                Label lblDateExpiry = (Label)row.FindControl("lblvDateExpiry");


                int myNegInt = System.Math.Abs(Convert.ToInt32(xtQtyPicked.Text)) * (-1);

                using (SqlConnection sqlConn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["MyConnCSI"].ConnectionString))
                {
                    string stR = @"dbo.PostTransferEntry";

                    using (SqlCommand cmD = new SqlCommand(stR, sqlConn))
                    {
                        sqlConn.Open();
                        cmD.CommandTimeout = 0;
                        cmD.CommandType = CommandType.StoredProcedure;
                        

                        cmD.Parameters.AddWithValue("@HeaderID",row.Cells[0].Text);
									cmD.Parameters.AddWithValue("@Sup_ItemCode",ddItem.SelectedValue);
                                    cmD.Parameters.AddWithValue("@vQtyPicked", myNegInt);
                                    cmD.Parameters.AddWithValue("@vQty", xtQtyPicked.Text);
									cmD.Parameters.AddWithValue("@vBatchNo",Server.HtmlDecode(row.Cells[2].Text));
									cmD.Parameters.AddWithValue("@vDateExpiry",lblDateExpiry.Text);
									cmD.Parameters.AddWithValue("@vPickedBy",Session["UserFullName"].ToString());
                                    cmD.Parameters.AddWithValue("@sup_CATEGORY", ddCategoryTarget.SelectedValue);

                        cmD.ExecuteNonQuery();

                    }
                }

                lblMsg.Text = "Item successfully transfer to " + ddCategoryTarget.SelectedItem.Text;
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