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
    public partial class ViewInTransitOrderDetail : System.Web.UI.Page
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

                    lblOrderDate.Text = Session["OrderDate"].ToString();
                    lblReferenceNo.Text = Session["ReferenceNo"].ToString();
                    lblSource.Text = Session["Source"].ToString();
                    lblType.Text = Session["Type"].ToString();
                    lblCustomerName.Text = Session["CustomerName"].ToString();
                    lblCustomerAddress.Text = Session["Customer Address"].ToString();
                    lblContactNo.Text = Session["ContactNo"].ToString();
                    lblEmailAddress.Text = Session["EmailAddress"].ToString();
                    lblDateDelivered.Text = Session["DateDelivered"].ToString();
                    lblDeliveredBy.Text = Session["DeliveredBy"].ToString();

                    loadInTransitOrderEntry();
                    

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


        private void loadInTransitOrderEntry()
        {
            using (SqlConnection sqlConn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["MyConnCSI"].ConnectionString))
            {

                string stR = @"dbo.LoadInTransitOrderDetail";

                using (SqlCommand cmD = new SqlCommand(stR, sqlConn))
                {
                    sqlConn.Open();
                    cmD.CommandTimeout = 0;
                    cmD.CommandType = CommandType.StoredProcedure;
                    cmD.Parameters.AddWithValue("@ReferenceNo", Session["ReferenceNo"].ToString());
                    cmD.Parameters.AddWithValue("@Source", Session["Source"].ToString());
                    DataTable dT = new DataTable();
                    SqlDataAdapter dA = new SqlDataAdapter(cmD);
                    dA.Fill(dT);

                    gvItems.DataSource = dT;
                    gvItems.DataBind();

                    if (dT.Rows.Count > 0)
                    {
                        lblMOP.Text = dT.Rows[0][9].ToString();
                        lblDeliveryInstruction.Text = dT.Rows[0][10].ToString();
                        lblShippingFee.Text = dT.Rows[0][11].ToString();
                    }

                }
            }
        }







        //here
        protected void btnPost_Click(object sender, EventArgs e)
        {
            if (IsPageRefresh == true)
            {
                Response.Redirect(Request.Url.AbsoluteUri);
            }
            else
            {
                foreach (GridViewRow gvR in gvItems.Rows)
                {
                    if (gvR.RowType == DataControlRowType.DataRow)
                    {
                        Label lblQuantity = (Label)gvR.FindControl("lblQuantity");

                        using (SqlConnection sqlConn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["MyConnCSI"].ConnectionString))
                        {
                            string stR = @"dbo.PostInTransitOrder";

                            using (SqlCommand cmD = new SqlCommand(stR, sqlConn))
                            {
                                sqlConn.Open();
                                cmD.CommandTimeout = 0;
                                cmD.CommandType = CommandType.StoredProcedure;
                                cmD.Parameters.AddWithValue("@OrderID", gvR.Cells[0].Text);
                                cmD.Parameters.AddWithValue("@HeaderID", gvR.Cells[1].Text);
                                cmD.Parameters.AddWithValue("@DatePostedDelivered", txtPostedDateDelivered.Text);
                                cmD.Parameters.AddWithValue("@vQtyOnline", lblQuantity.Text);
                                cmD.ExecuteNonQuery();
                            }
                        }

                    }
                }

                lblMsgSuccessPosting.Text = "Successfully posted!";
                ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "Popup", "ShowMsgSuccessPosting();", true);
                return;
            }
        }

        protected void gvItems_RowEditing(object sender, GridViewEditEventArgs e)
        {
            gvItems.EditIndex = e.NewEditIndex;
            loadInTransitOrderEntry();
        }

        protected void gvItems_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            gvItems.EditIndex = -1;
            loadInTransitOrderEntry();
        }

        protected void gvItems_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            try
            {
                string TheID = gvItems.DataKeys[e.RowIndex].Value.ToString();

                GridViewRow row = gvItems.Rows[e.RowIndex];
                TextBox txtQuantity = (TextBox)row.FindControl("txtQuantity");

                string connString = System.Configuration.ConfigurationManager.ConnectionStrings["MyConnCSI"].ConnectionString;

                string sql = @"Update [ITEM_OnlineSold] set vQtyPicked=@vQtyPicked where OrderID = @OrderID";
                using (SqlConnection conn = new SqlConnection(connString))
                {
                    conn.Open();

                    using (SqlCommand cmD = new SqlCommand(sql, conn))
                    {
                        cmD.CommandTimeout = 0;
                        cmD.Parameters.AddWithValue("@OrderID", TheID.ToString());
                        cmD.Parameters.AddWithValue("@vQtyPicked", txtQuantity.Text);
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
            loadInTransitOrderEntry();
        }





    }
}