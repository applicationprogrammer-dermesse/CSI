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
    public partial class ListOfForApprovalLogistics : System.Web.UI.Page
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
                    listForApproval();


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

        private void listForApproval()
        {
            using (SqlConnection sqlConn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["MyConnCSI"].ConnectionString))
            {
                string stR = @"dbo.ListForApprovalLogistics";
                using (SqlCommand cmD = new SqlCommand(stR, sqlConn))
                {
                    sqlConn.Open();
                    cmD.CommandTimeout = 0;
                    cmD.CommandType = CommandType.StoredProcedure;
                    cmD.Parameters.AddWithValue("@BrCode", Session["UserBranchCode"].ToString());
                    DataTable dT = new DataTable();
                    SqlDataAdapter dA = new SqlDataAdapter(cmD);
                    dA.Fill(dT);

                    gvForApproval.DataSource = dT;
                    gvForApproval.DataBind();

                }
            }
        }

        protected void gvForApproval_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "ViewDetail")
            {
                GridViewRow gvr = (GridViewRow)(((LinkButton)e.CommandSource).NamingContainer);
                int RowIndex = gvr.RowIndex;

                Session["BrCode"] = gvr.Cells[0].Text;
                Session["BranchName"] = gvr.Cells[1].Text;
                Session["No"] = gvr.Cells[2].Text;
                Session["DateSubmit"] = gvr.Cells[3].Text;
                Session["Category"] = gvr.Cells[4].Text;
                Session["Type"] = gvr.Cells[6].Text;

                Response.Redirect("~/ForApprovalLogistics.aspx?val=" + Session["BrCode"].ToString() + " - " + Session["No"].ToString());
                //Response.Redirect("~/TransferQCADReleased.aspx?val=" + Session["vRecNum"].ToString());



            }
        }

        protected void gvForApproval_PreRender(object sender, EventArgs e)
        {
            if (gvForApproval.Rows.Count > 0)
            {
                gvForApproval.UseAccessibleHeader = true;
                gvForApproval.HeaderRow.TableSection = TableRowSection.TableHeader;
                gvForApproval.FooterRow.TableSection = TableRowSection.TableFooter;
            }
        }





    }
}