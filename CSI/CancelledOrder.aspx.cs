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
    public partial class CancelledOrder : System.Web.UI.Page
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

                    gvOrdersCancelled.DataSource = null;
                    gvOrdersCancelled.DataBind();
                    gvOrdersCancelled.Visible = false;
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
                    ddCategory.Items.Insert(0, new ListItem("E-Commerce", "8"));
                    ddCategory.SelectedIndex = 0;
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

                    string stR = @"dbo.ListCancelledOrderOnline";

                    using (SqlCommand cmD = new SqlCommand(stR, sqlConn))
                    {
                        sqlConn.Open();
                        cmD.CommandTimeout = 0;
                        cmD.CommandType = CommandType.StoredProcedure;
                        DataTable dT = new DataTable();
                        SqlDataAdapter dA = new SqlDataAdapter(cmD);
                        dA.Fill(dT);

                        gvOrdersCancelled.DataSource = dT;
                        gvOrdersCancelled.DataBind();

                    }
                }
            }
        }


        protected void gvOrdersCancelled_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "ViewOrderDetailCancelled")
            {

                GridViewRow gvr = (GridViewRow)(((LinkButton)e.CommandSource).NamingContainer);
                int RowIndex = gvr.RowIndex;

                //Session["OrderDate"] = gvr.Cells[2].Text;
                //Session["ReferenceNo"] = gvr.Cells[3].Text;
                //Session["Source"] = gvr.Cells[1].Text;


                using (SqlConnection Conn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["MyConnCSI"].ConnectionString))
                {


                    string stRGetBal = @"SELECT a.[HeaderID]
                              ,a.[Sup_ItemCode]
	                          ,b.sup_DESCRIPTION
                              ,a.[vQtyPicked]
                              ,a.[vUnitCost]
                          FROM [ITEM_OnlineSold] a
                          LEFT JOIN Sup_ItemMaster b
                          ON a.[Sup_ItemCode] = b.[Sup_ItemCode]
                          where a.vStat=4 
                          and ReferenceNo=@ReferenceNo
                          ORDER BY b.sup_DESCRIPTION";
                    using (SqlCommand cmD = new SqlCommand(stRGetBal, Conn))
                    {
                        Conn.Open();
                        cmD.Parameters.AddWithValue("@ReferenceNo", gvr.Cells[3].Text);

                        DataTable dT = new DataTable();
                        SqlDataAdapter dA = new SqlDataAdapter(cmD);
                        dA.Fill(dT);

                        gvCancelledItem.DataSource = dT;
                        gvCancelledItem.DataBind();
                        ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "Popup", "ShowGridItemBatch();", true);
                        return;

                    }
                }


            }
        }

        protected void ddCategory_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddCategory.SelectedValue == "8")
            {
                loadUnpostedOrderEntryECommerce();
                gvOrdersCancelled.DataSource = null;
                gvOrdersCancelled.DataBind();
                gvOrdersCancelled.Visible = false;
                gvECommerceCancelled.Visible = true;
            }
            else
            {
                loadUnpostedOrderEntry();
                gvOrdersCancelled.Visible = true;
                gvECommerceCancelled.DataSource = null;
                gvECommerceCancelled.DataBind();
                gvECommerceCancelled.Visible = false;
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

                    string stR = @"dbo.ListCancelledOrderECommerce";

                    using (SqlCommand cmD = new SqlCommand(stR, sqlConn))
                    {
                        sqlConn.Open();
                        cmD.CommandTimeout = 0;
                        cmD.CommandType = CommandType.StoredProcedure;
                        DataTable dT = new DataTable();
                        SqlDataAdapter dA = new SqlDataAdapter(cmD);
                        dA.Fill(dT);

                        gvECommerceCancelled.DataSource = dT;
                        gvECommerceCancelled.DataBind();

                    }
                }
            }
        }

        protected void gvECommerceCancelled_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "ViewCancelledDetail")
            {
                GridViewRow gvr = (GridViewRow)(((LinkButton)e.CommandSource).NamingContainer);
                int RowIndex = gvr.RowIndex;

                //Session["OrderDate"] = gvr.Cells[2].Text;
                //Session["ReferenceNo"] = gvr.Cells[3].Text;
                //Session["Source"] = gvr.Cells[1].Text;


                using (SqlConnection Conn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["MyConnCSI"].ConnectionString))
                {


                    string stRGetBal = @"SELECT a.[HeaderID]
                              ,a.[Sup_ItemCode]
	                          ,b.sup_DESCRIPTION
                              ,a.[vQtyPicked]
                              ,a.[vUnitCost]
                          FROM [ITEM_OnlineSold] a
                          LEFT JOIN Sup_ItemMaster b
                          ON a.[Sup_ItemCode] = b.[Sup_ItemCode]
                          where a.vStat=4 
                          and ReferenceNo=@ReferenceNo
                          ORDER BY b.sup_DESCRIPTION";
                    using (SqlCommand cmD = new SqlCommand(stRGetBal, Conn))
                    {
                        Conn.Open();
                        cmD.Parameters.AddWithValue("@ReferenceNo", gvr.Cells[3].Text);

                        DataTable dT = new DataTable();
                        SqlDataAdapter dA = new SqlDataAdapter(cmD);
                        dA.Fill(dT);

                        gvCancelledItem.DataSource = dT;
                        gvCancelledItem.DataBind();
                            ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "Popup", "ShowGridItemBatch();", true);
                            return;
                
                    }
                }

                
            }
        }

        protected void gvECommerceCancelled_PreRender(object sender, EventArgs e)
        {
            if (gvECommerceCancelled.Rows.Count > 0)
            {
                gvECommerceCancelled.UseAccessibleHeader = true;
                gvECommerceCancelled.HeaderRow.TableSection = TableRowSection.TableHeader;
                gvECommerceCancelled.FooterRow.TableSection = TableRowSection.TableFooter;
            }
        }


    }
}