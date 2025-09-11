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
    public partial class ExpiredToRGAS : System.Web.UI.Page
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

                        loadAllExpiredItems();



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


        private void loadCategory()
        {
            using (SqlConnection conN = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["MyConnCSI"].ConnectionString))
            {
                string stR = @"SELECT [Sup_CategoryNum]
                                      ,[Sup_CategoryName]
                                  FROM [Sup_Category] WHERE [Sup_CategoryNum] IN (1,2,3,4,9) order by [Sup_CategoryName]";
                using (SqlCommand cmD = new SqlCommand(stR, conN))
                {
                    conN.Open();
                    cmD.CommandTimeout = 0;
                    SqlDataReader dR = cmD.ExecuteReader();

                    ddCategory.Items.Clear();
                    ddCategory.DataSource = dR;
                    ddCategory.DataValueField = "Sup_CategoryNum";
                    ddCategory.DataTextField = "Sup_CategoryName";
                    ddCategory.DataBind();
                    ddCategory.Items.Insert(0, new ListItem("All Category", "0"));
                    ddCategory.SelectedIndex = 0;
                }
            }
        }

        private void loadAllExpiredItems()
        {
            using (SqlConnection sqlConn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["MyConnCSI"].ConnectionString))
            {
                string stR = @"dbo.loadExpiredToRGAS ";
                using (SqlCommand cmD = new SqlCommand(stR, sqlConn))
                {
                    sqlConn.Open();
                    cmD.CommandTimeout = 0;
                    cmD.CommandType = CommandType.StoredProcedure;
                    cmD.Parameters.AddWithValue("@Sup_CategoryNum", ddCategory.SelectedValue);
                    DataTable dT = new DataTable();
                    SqlDataAdapter dA = new SqlDataAdapter(cmD);
                    dA.Fill(dT);

                    gvInventoryDetailedHO.DataSource = dT;
                    gvInventoryDetailedHO.DataBind();

                }
            }
        }

        protected void gvInventoryDetailedHO_PreRender(object sender, EventArgs e)
        {
            if (gvInventoryDetailedHO.Rows.Count > 0)
            {
                gvInventoryDetailedHO.UseAccessibleHeader = true;
                gvInventoryDetailedHO.HeaderRow.TableSection = TableRowSection.TableHeader;
                gvInventoryDetailedHO.FooterRow.TableSection = TableRowSection.TableFooter;
            }
        }

        protected void gvInventoryDetailedHO_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "Post")
            {
                try
                {
                    GridViewRow gvr = (GridViewRow)(((LinkButton)e.CommandSource).NamingContainer);
                    string connString = System.Configuration.ConfigurationManager.ConnectionStrings["MyConnCSI"].ConnectionString;

                    string sql = @"dbo.PostExpiredToRGAS";
                    using (SqlConnection conn = new SqlConnection(connString))
                    {
                        conn.Open();
                        //string test = "-" + gvr.Cells[6].Text;
                        using (SqlCommand cmD = new SqlCommand(sql, conn))
                        {
                            cmD.CommandTimeout = 0;
                            cmD.CommandType = CommandType.StoredProcedure;
                            cmD.Parameters.AddWithValue("@HeaderID", gvr.Cells[0].Text);
                            cmD.Parameters.AddWithValue("@Qty", "-" + gvr.Cells[6].Text);
                            cmD.ExecuteNonQuery();
                        }
                    }


                    loadAllExpiredItems();

                    lblMsg.Text = "Successfully transferred to RGAS";
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