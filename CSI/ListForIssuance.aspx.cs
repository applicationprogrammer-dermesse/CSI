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
    public partial class ListForIssuance : System.Web.UI.Page
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
                    listForIssuance();


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

        private void listForIssuance()
        {
            using (SqlConnection sqlConn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["MyConnCSI"].ConnectionString))
            {
                string stR = @"dbo.ListForIssuance";
                using (SqlCommand cmD = new SqlCommand(stR, sqlConn))
                {
                    sqlConn.Open();
                    cmD.CommandTimeout = 0;
                    cmD.CommandType = CommandType.StoredProcedure;
                    cmD.Parameters.AddWithValue("@BrCode", Session["UserBranchCode"].ToString());
                    DataTable dT = new DataTable();
                    SqlDataAdapter dA = new SqlDataAdapter(cmD);
                    dA.Fill(dT);

                    gvForIssuance.DataSource = dT;
                    gvForIssuance.DataBind();

                }
            }
        }

        protected void gvForIssuance_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "ViewDetail")
            {
                GridViewRow gvr = (GridViewRow)(((LinkButton)e.CommandSource).NamingContainer);
                int RowIndex = gvr.RowIndex;

                Session["BrCode"] = gvr.Cells[0].Text;
                Session["BranchName"] = gvr.Cells[1].Text;
                Session["No"] = gvr.Cells[2].Text;
                Session["DatePosted"] = gvr.Cells[3].Text;
                Session["Category"] = gvr.Cells[4].Text;
                Session["Type"] = gvr.Cells[6].Text;

                Response.Redirect("~/ForIssuance.aspx?val=" + Session["BrCode"].ToString() + " - " + Session["No"].ToString());
                //Response.Redirect("~/TransferQCADReleased.aspx?val=" + Session["vRecNum"].ToString());



            }
        }

        protected void gvForIssuance_PreRender(object sender, EventArgs e)
        {
            if (gvForIssuance.Rows.Count > 0)
            {
                gvForIssuance.UseAccessibleHeader = true;
                gvForIssuance.HeaderRow.TableSection = TableRowSection.TableHeader;
                gvForIssuance.FooterRow.TableSection = TableRowSection.TableFooter;
            }
        }
    }
}