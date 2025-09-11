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
    public partial class ListOrderOnline : System.Web.UI.Page
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
                    loadUnpostedOrderEntry();

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


        private void loadUnpostedOrderEntry()
        {
            using (SqlConnection sqlConn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["MyConnCSI"].ConnectionString))
            {

                string stR = @"dbo.ListOrderOnlineForPrintDR";

                using (SqlCommand cmD = new SqlCommand(stR, sqlConn))
                {
                    sqlConn.Open();
                    cmD.CommandTimeout = 0;
                    cmD.CommandType = CommandType.StoredProcedure;
                    DataTable dT = new DataTable();
                    SqlDataAdapter dA = new SqlDataAdapter(cmD);
                    dA.Fill(dT);
                    
                    gvOrders.DataSource = dT;
                    gvOrders.DataBind();

                }
            }
        }

        
        protected void gvOrders_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "ViewOrderDetail")
            {
                GridViewRow gvr = (GridViewRow)(((LinkButton)e.CommandSource).NamingContainer);
                int RowIndex = gvr.RowIndex;

                Session["OrderDate"] = gvr.Cells[0].Text;
                Session["ReferenceNo"] = gvr.Cells[1].Text;
                Session["Source"] = gvr.Cells[2].Text;
                Session["Type"] = gvr.Cells[3].Text;
                Session["CustomerName"] = gvr.Cells[4].Text;
                Session["Customer Address"] = gvr.Cells[5].Text;
                Session["ContactNo"] = gvr.Cells[6].Text;
                Session["EmailAddress"] = gvr.Cells[7].Text;

                Response.Redirect("~/ViewOrderDetail.aspx?val=" + Session["ReferenceNo"].ToString());
                //Response.Redirect("~/TransferQCADReleased.aspx?val=" + Session["vRecNum"].ToString());



            }
        }



    }
}