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
    public partial class ListInTransitOrder : System.Web.UI.Page
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
                    loadCategory();
                    loadUnpostedOrderEntryECommerce();

                    gvOrders.DataSource = null;
                    gvOrders.DataBind();
                    gvOrders.Visible = false;
                    //loadUnpostedOrderEntry();

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

        //public string stRCategory;
        private void loadCategory()
        {
            using (SqlConnection conN = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["MyConnCSI"].ConnectionString))
            {

                string stRCategory = @"SELECT [Sup_CategoryNum]
                                      ,[Sup_CategoryName]
                                  FROM [Sup_Category] where Sup_CategoryNum in (8) order by [Sup_CategoryName]";


                using (SqlCommand cmD = new SqlCommand(stRCategory, conN))
                {
                    conN.Open();
                    cmD.CommandTimeout = 0;
                    SqlDataReader dR = cmD.ExecuteReader();

                    ddCategory.Items.Clear();
                    ddCategory.DataSource = dR;
                    ddCategory.DataValueField = "Sup_CategoryNum";
                    ddCategory.DataTextField = "Sup_CategoryName";
                    ddCategory.DataBind();
                    //ddCategory.Items.Insert(0, new ListItem("Select Category", "0"));
                    //ddCategory.SelectedIndex = 0;
                }
            }
        }

        private void loadUnpostedOrderEntry()
        {
            if (IsPageRefresh == true)
            {
                Response.Redirect(Request.Url.AbsoluteUri);
            }
            else
            {
                using (SqlConnection sqlConn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["MyConnCSI"].ConnectionString))
                {

                    string stR = @"dbo.ListInTransitOrder";

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
        }


        protected void gvOrders_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "ViewInTransitOrderDetail")
            {
                GridViewRow gvr = (GridViewRow)(((LinkButton)e.CommandSource).NamingContainer);
                int RowIndex = gvr.RowIndex;

                Session["OrderDate"] = gvr.Cells[1].Text;
                Session["ReferenceNo"] = gvr.Cells[2].Text;
                Session["Source"] = gvr.Cells[3].Text;
                Session["Type"] = gvr.Cells[4].Text;
                Session["CustomerName"] = gvr.Cells[5].Text;
                Session["Customer Address"] = gvr.Cells[6].Text;
                Session["ContactNo"] = gvr.Cells[7].Text;
                Session["EmailAddress"] = gvr.Cells[8].Text;
                Session["DateDelivered"] = gvr.Cells[9].Text;
                Session["DeliveredBy"] = gvr.Cells[10].Text;

                //if (gvr.Cells[0].Text == "In-House")
                //{
                    Response.Redirect("~/ViewInTransitOrderDetail.aspx?val=" + Session["ReferenceNo"].ToString());
                //}
                //else
                //{
                //    Response.Redirect("~/ViewInTransitECommerce.aspx?val=" + Session["ReferenceNo"].ToString());
                //}
                //Response.Redirect("~/TransferQCADReleased.aspx?val=" + Session["vRecNum"].ToString());



            }
        }

        protected void ddCategory_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddCategory.SelectedValue=="8")
            {
                loadUnpostedOrderEntryECommerce();
                gvOrders.DataSource = null;
                gvOrders.DataBind();
                gvOrders.Visible = false;
                gvECommerce.Visible = true;
            }
            else
            {
                loadUnpostedOrderEntry();
                gvOrders.Visible = true;
                gvECommerce.DataSource = null;
                gvECommerce.DataBind();
                gvECommerce.Visible = false;
            }
        }


        private void loadUnpostedOrderEntryECommerce()
        {
            if (IsPageRefresh == true)
            {
                Response.Redirect(Request.Url.AbsoluteUri);
            }
            else
            {
                using (SqlConnection sqlConn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["MyConnCSI"].ConnectionString))
                {

                    string stR = @"dbo.ListInTransitOrderECommerce";

                    using (SqlCommand cmD = new SqlCommand(stR, sqlConn))
                    {
                        sqlConn.Open();
                        cmD.CommandTimeout = 0;
                        cmD.CommandType = CommandType.StoredProcedure;
                        DataTable dT = new DataTable();
                        SqlDataAdapter dA = new SqlDataAdapter(cmD);
                        dA.Fill(dT);

                        gvECommerce.DataSource = dT;
                        gvECommerce.DataBind();

                    }
                }
            }
        }

        protected void gvECommerce_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "ViewInTransitOrderDetailECom")
            {
                GridViewRow gvr = (GridViewRow)(((LinkButton)e.CommandSource).NamingContainer);
                int RowIndex = gvr.RowIndex;

                Session["OrderDate"] = gvr.Cells[2].Text;
                Session["ReferenceNo"] = gvr.Cells[3].Text;
                Session["Source"] = gvr.Cells[1].Text;
                //Session["Type"] = gvr.Cells[4].Text;
                //Session["CustomerName"] = gvr.Cells[5].Text;
                //Session["Customer Address"] = gvr.Cells[6].Text;
                //Session["ContactNo"] = gvr.Cells[7].Text;
                //Session["EmailAddress"] = gvr.Cells[8].Text;
                //Session["DateDelivered"] = gvr.Cells[9].Text;
                //Session["DeliveredBy"] = gvr.Cells[10].Text;

                Response.Redirect("~/ViewInTransitECommerce.aspx?val=" + Session["ReferenceNo"].ToString());
              
                //Response.Redirect("~/TransferQCADReleased.aspx?val=" + Session["vRecNum"].ToString());



            }
        }

        protected void gvECommerce_PreRender(object sender, EventArgs e)
        {
            if (gvECommerce.Rows.Count > 0)
            {
                gvECommerce.UseAccessibleHeader = true;
                gvECommerce.HeaderRow.TableSection = TableRowSection.TableHeader;
                gvECommerce.FooterRow.TableSection = TableRowSection.TableFooter;
            }
        }


    }
}