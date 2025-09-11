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
    public partial class ConfirmedDeliveredOrder : System.Web.UI.Page
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
                    loadECommercePlatForm();
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

        public string stRCategory;
        private void loadCategory()
        {
            using (SqlConnection conN = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["MyConnCSI"].ConnectionString))
            {
                
                    stRCategory = @"SELECT [Sup_CategoryNum]
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


        protected void gvOnline_PreRender(object sender, EventArgs e)
        {
            if (gvOnline.Rows.Count > 0)
            {
                gvOnline.UseAccessibleHeader = true;
                gvOnline.HeaderRow.TableSection = TableRowSection.TableHeader;
                gvOnline.FooterRow.TableSection = TableRowSection.TableFooter;
            }
        }

        protected void btnGenerate_Click(object sender, EventArgs e)
        {
            loadSalesOnline();
        }


        private void loadSalesOnline()
        {
            using (SqlConnection sqlConn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["MyConnCSI"].ConnectionString))
            {
                string stR = @"dbo.ReportConfirmedDelivered";
                using (SqlCommand cmD = new SqlCommand(stR, sqlConn))
                {
                    sqlConn.Open();
                    cmD.CommandTimeout = 0;
                    cmD.CommandType = CommandType.StoredProcedure;
                    
                    cmD.Parameters.AddWithValue("@dFROM", txtDateFrom.Text);
                    cmD.Parameters.AddWithValue("@dTO", txtDateTo.Text);
                    cmD.Parameters.AddWithValue("@Category", ddCategory.SelectedValue);
                    if (ddPlatform.SelectedValue=="0")
                    {
                        cmD.Parameters.AddWithValue("@Option", 0);
                    }
                    else
                    {
                        cmD.Parameters.AddWithValue("@Option", 1);
                    }

                    cmD.Parameters.AddWithValue("@Source", ddPlatform.SelectedItem.Text);
                   
                    DataTable dT = new DataTable();
                    SqlDataAdapter dA = new SqlDataAdapter(cmD);
                    dA.Fill(dT);

                    gvOnline.DataSource = dT;
                    gvOnline.DataBind();

                }
            }
        }

        protected void ddCategory_SelectedIndexChanged(object sender, EventArgs e)
        {
            gvOnline.DataSource = null;
            gvOnline.DataBind();
            if (ddCategory.SelectedValue=="8")
            {
                loadECommercePlatForm();
            }
            else if (ddCategory.SelectedValue == "6")
            {
                loadOnlinePlatForm();
            }
        }




        private void loadECommercePlatForm()
        {
            using (SqlConnection conN = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["MyConnCSI"].ConnectionString))
            {
                string stR = @"SELECT ID,SourceType FROM tblSource WHERE SourceCategory=1 AND vStat=1 ORDER BY ID";
                using (SqlCommand cmD = new SqlCommand(stR, conN))
                {
                    conN.Open();
                    SqlDataReader dR = cmD.ExecuteReader();

                    ddPlatform.Items.Clear();
                    ddPlatform.DataSource = dR;
                    ddPlatform.DataValueField = "ID";
                    ddPlatform.DataTextField = "SourceType";
                    ddPlatform.DataBind();
                    ddPlatform.Items.Insert(0, new ListItem("All", "0"));
                    ddPlatform.Items.Insert(3, new ListItem("Free", "Free"));
                    ddPlatform.Items.Insert(4, new ListItem("Complimentary", "Complimentary"));
                    ddPlatform.SelectedIndex = 0;
                }
            }
        }


        private void loadOnlinePlatForm()
        {
            using (SqlConnection conN = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["MyConnCSI"].ConnectionString))
            {
                string stR = @"SELECT ID,SourceType FROM tblSource WHERE SourceCategory=2 AND vStat=1 ORDER BY ID";
                using (SqlCommand cmD = new SqlCommand(stR, conN))
                {
                    conN.Open();
                    SqlDataReader dR = cmD.ExecuteReader();

                    ddPlatform.Items.Clear();
                    ddPlatform.DataSource = dR;
                    ddPlatform.DataValueField = "ID";
                    ddPlatform.DataTextField = "SourceType";
                    ddPlatform.DataBind();
                    ddPlatform.Items.Insert(0, new ListItem("All", "0"));
                    ddPlatform.SelectedIndex = 0;
                }
            }
        }

    }
}