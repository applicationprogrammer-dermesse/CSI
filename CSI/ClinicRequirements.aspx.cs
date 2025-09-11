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
    public partial class ClinicRequirements : System.Web.UI.Page
    {

        public bool IsPageRefresh = false;
        protected void Page_Load(object sender, EventArgs e)
        {
            //Session["UserFullName"] = "Rea";
            //Session["BrCode"] = "1";
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

                  
                    loadRequestDetail();

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

        private void loadRequestDetail()
        {
            using (SqlConnection sqlConn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["MyConnCSI"].ConnectionString))
            {
                string stR = @"dbo.ClinicRequirements";
                using (SqlCommand cmD = new SqlCommand(stR, sqlConn))
                {
                    sqlConn.Open();
                    cmD.CommandTimeout = 0;
                    cmD.CommandType = CommandType.StoredProcedure;
                    cmD.Parameters.AddWithValue("@BrCode", Session["UserBranchCode"].ToString());

                    DataTable dT = new DataTable();
                    SqlDataAdapter dA = new SqlDataAdapter(cmD);
                    dA.Fill(dT);

                    gvRequirements.DataSource = dT;
                    gvRequirements.DataBind();

                }
            }
        }

        protected void gvRequirements_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            GridViewRow gvr = (GridViewRow)(((LinkButton)e.CommandSource).NamingContainer);
            int RowIndex = gvr.RowIndex;

            using (SqlConnection sqlConn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["MyConnCSI"].ConnectionString))
            {
                string stR = @"dbo.ClinicRequirementsDetails";
                using (SqlCommand cmD = new SqlCommand(stR, sqlConn))
                {
                    sqlConn.Open();
                    cmD.CommandTimeout = 0;
                    cmD.CommandType = CommandType.StoredProcedure;
                    cmD.Parameters.AddWithValue("@BrCode", gvr.Cells[0].Text);
                    cmD.Parameters.AddWithValue("@ItemCode", gvr.Cells[3].Text);

                    DataTable dT = new DataTable();
                    SqlDataAdapter dA = new SqlDataAdapter(cmD);
                    dA.Fill(dT);

                    gvReceiptNo.DataSource = dT;
                    gvReceiptNo.DataBind();

                }
            }

            ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "Popup", "ShowGridReceiptNo();", true);
            return;
        }

        protected void gvRequirements_PreRender(object sender, EventArgs e)
        {
            if (gvRequirements.Rows.Count > 0)
            {
                gvRequirements.UseAccessibleHeader = true;
                gvRequirements.HeaderRow.TableSection = TableRowSection.TableHeader;
                gvRequirements.FooterRow.TableSection = TableRowSection.TableFooter;
            }
        }


        protected void gvReceiptNo_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (IsPageRefresh == true)
            {
                Response.Redirect(Request.Url.AbsoluteUri);
            }
            else
            {

                if (e.CommandName == "linkSelect")
                {

                    GridViewRow gvr = (GridViewRow)(((LinkButton)e.CommandSource).NamingContainer);
                    int RowIndex = gvr.RowIndex;

                    using (SqlConnection sqlConn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["MyConnCSI"].ConnectionString))
                    {
                        string stR = @"dbo.SaveAcknowledgeSupplies";
                        using (SqlCommand cmD = new SqlCommand(stR, sqlConn))
                        {
                            sqlConn.Open();
                            cmD.CommandTimeout = 0;
                            cmD.CommandType = CommandType.StoredProcedure;
                            cmD.Parameters.AddWithValue("@RecID", gvr.Cells[0].Text);
							cmD.Parameters.AddWithValue("@BrCode", gvr.Cells[1].Text);
							cmD.Parameters.AddWithValue("@CustomerName", gvr.Cells[4].Text);
							cmD.Parameters.AddWithValue("@ReceiptNo", gvr.Cells[5].Text);
                            cmD.Parameters.AddWithValue("@ServiceAvailed", gvr.Cells[6].Text);
							cmD.Parameters.AddWithValue("@vQty", gvr.Cells[7].Text);
							cmD.Parameters.AddWithValue("@TotalSession", gvr.Cells[8].Text);
							cmD.Parameters.AddWithValue("@sup_ItemCode", gvr.Cells[9].Text);
							cmD.Parameters.AddWithValue("@cQty", Convert.ToDecimal(gvr.Cells[11].Text));
							cmD.Parameters.AddWithValue("@cUOM", gvr.Cells[12].Text);
                            cmD.Parameters.AddWithValue("@AcknowledgeBy", Session["UserFullName"].ToString());
                            cmD.Parameters.AddWithValue("@DateAcknowledge", DateTime.Now);
                            cmD.ExecuteNonQuery();

                        }
                    }


                    loadRequestDetail();
                }

            }
        }




    }
}