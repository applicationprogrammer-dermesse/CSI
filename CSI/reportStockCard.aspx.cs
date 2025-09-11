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
    public partial class reportStockCard : System.Web.UI.Page
    {
        public bool IsPageRefresh = false;
        public int theExpiration;
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

                    // Set default date range to last 30 days
                    txtDateFrom.Text = DateTime.Now.AddDays(-30).ToString("MM/dd/yyyy");
                    txtDateTo.Text = DateTime.Now.ToString("MM/dd/yyyy");

                    if (Session["UserBranchCode"].ToString() == "1")
                    {
                        loadBranch();
                    }
                    else
                    {
                        loadPerBranch();
                    }

                    loadCategory();

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


        private void loadPerBranch()
        {
            using (SqlConnection conN = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["MyConnCSI"].ConnectionString))
            {
                string stR = @"SELECT BrCode,BrName FROM MyBranchList Where BrCode='" + Session["UserBranchCode"].ToString() + "'  ORDER BY BrCode";
                using (SqlCommand cmD = new SqlCommand(stR, conN))
                {
                    conN.Open();
                    SqlDataReader dR = cmD.ExecuteReader();

                    ddBranch.Items.Clear();
                    ddBranch.DataSource = dR;
                    ddBranch.DataValueField = "BrCode";
                    ddBranch.DataTextField = "BrName";
                    ddBranch.DataBind();
                    //ddBranch.Items.Insert(0, new ListItem("All Branches", "0"));
                    //ddBranch.SelectedIndex = 0;
                }
            }
        }

        private void loadBranch()
        {
            using (SqlConnection conN = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["MyConnCSI"].ConnectionString))
            {
                string stR = @"dbo.LoadBranches";
                using (SqlCommand cmD = new SqlCommand(stR, conN))
                {
                    conN.Open();
                    cmD.CommandTimeout = 0;
                    cmD.CommandType = CommandType.StoredProcedure;
                    SqlDataReader dR = cmD.ExecuteReader();

                    ddBranch.Items.Clear();
                    ddBranch.DataSource = dR;
                    ddBranch.DataValueField = "BrCode";
                    ddBranch.DataTextField = "BrName";
                    ddBranch.DataBind();
                    //ddBranch.Items.Insert(0, new ListItem("DCI-Logistics", "0"));
                    ddBranch.SelectedIndex = 0;
                }
            }
        }
        private void loadCategory()
        {
            using (SqlConnection conN = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["MyConnCSI"].ConnectionString))
            {
                string stR = @"SELECT [Sup_CategoryNum]
                              ,[Sup_CategoryName]
                          FROM [Sup_Category] order by [Sup_CategoryName]";
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
                    ddCategory.Items.Insert(0, new ListItem("All Categories", "-1")); // Changed from "0" to "-1"
                    ddCategory.Items.Insert(1, new ListItem("Select Category", "0")); // Keep select option but make it different
                    ddCategory.SelectedIndex = 1; // Select "Select Category" by default
                }
            }
        }

        protected void btnGenerate_Click(object sender, EventArgs e)
        {
            // Validate date range
            DateTime dateFrom, dateTo;

            if (!DateTime.TryParse(txtDateFrom.Text, out dateFrom))
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert",
                    "alert('Please enter a valid From Date');", true);
                return;
            }

            if (!DateTime.TryParse(txtDateTo.Text, out dateTo))
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert",
                    "alert('Please enter a valid To Date');", true);
                return;
            }

            if (dateFrom > dateTo)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert",
                    "alert('From Date cannot be greater than To Date');", true);
                return;
            }

            // Validate item selection when a specific category is chosen
            if (ddCategory.SelectedValue != "-1" && ddItem.SelectedValue == "0")
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert",
                    "alert('Please select an item');", true);
                return;
            }

            if (ddBranch.SelectedValue == "1")
            {
                genStockCardLogistics();
            }
            else
            {
                genStockCardBranches();
            }
        }


        private void genStockCardBranches()
        {
            using (SqlConnection sqlConn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["MyConnCSI"].ConnectionString))
            {
                string stR = @"dbo.ReportItemMovementBranches";
                using (SqlCommand cmD = new SqlCommand(stR, sqlConn))
                {
                    sqlConn.Open();
                    cmD.CommandTimeout = 0;
                    cmD.CommandType = CommandType.StoredProcedure;
                    cmD.Parameters.AddWithValue("@BrCode", ddBranch.SelectedValue);
                    cmD.Parameters.AddWithValue("@ItemCode", ddItem.SelectedValue == "0" && ddCategory.SelectedValue == "-1"
                        ? (object)DBNull.Value : ddItem.SelectedValue);
                    cmD.Parameters.AddWithValue("@CategoryNum", ddCategory.SelectedValue == "-1"
                        ? (object)DBNull.Value : ddCategory.SelectedValue);
                    cmD.Parameters.AddWithValue("@DateFrom", DateTime.Parse(txtDateFrom.Text));
                    cmD.Parameters.AddWithValue("@DateTo", DateTime.Parse(txtDateTo.Text));
                    DataTable dT = new DataTable();
                    SqlDataAdapter dA = new SqlDataAdapter(cmD);
                    dA.Fill(dT);

                    gvStockCard.DataSource = dT;
                    gvStockCard.DataBind();
                }
            }
        }

        private void genStockCardLogistics()
        {
            using (SqlConnection sqlConn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["MyConnCSI"].ConnectionString))
            {
                string stR = @"dbo.ReportItemMovement";
                using (SqlCommand cmD = new SqlCommand(stR, sqlConn))
                {
                    sqlConn.Open();
                    cmD.CommandTimeout = 0;
                    cmD.CommandType = CommandType.StoredProcedure;
                    cmD.Parameters.AddWithValue("@ItemCode", ddItem.SelectedValue == "0" && ddCategory.SelectedValue == "-1"
                        ? (object)DBNull.Value : ddItem.SelectedValue);
                    cmD.Parameters.AddWithValue("@CategoryNum", ddCategory.SelectedValue == "-1"
                        ? (object)DBNull.Value : ddCategory.SelectedValue);
                    cmD.Parameters.AddWithValue("@DateFrom", DateTime.Parse(txtDateFrom.Text));
                    cmD.Parameters.AddWithValue("@DateTo", DateTime.Parse(txtDateTo.Text));
                    DataTable dT = new DataTable();
                    SqlDataAdapter dA = new SqlDataAdapter(cmD);
                    dA.Fill(dT);

                    gvStockCard.DataSource = dT;
                    gvStockCard.DataBind();
                }
            }
        }

        protected void gvStockCard_PreRender(object sender, EventArgs e)
        {
            if (gvStockCard.Rows.Count > 0)
            {
                gvStockCard.UseAccessibleHeader = true;
                gvStockCard.HeaderRow.TableSection = TableRowSection.TableHeader;
                gvStockCard.FooterRow.TableSection = TableRowSection.TableFooter;
            }
        }

        protected void ddItem_SelectedIndexChanged(object sender, EventArgs e)
        {
            gvStockCard.DataSource = null;
            gvStockCard.DataBind();
        }


        private void loadItemSupplies()
        {
            using (SqlConnection sqlConn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["MyConnCSI"].ConnectionString))
            {
                string whereClause = ddCategory.SelectedValue == "-1"
                    ? "WHERE b.sup_DESCRIPTION IS NOT NULL"
                    : "WHERE b.sup_DESCRIPTION IS NOT NULL and a.Sup_CategoryNum='" + ddCategory.SelectedValue + "'";

                string stR = @"SELECT DISTINCT a.sup_ItemCode,a.sup_ItemCode + ' - ' + b.sup_DESCRIPTION as [sup_DESCRIPTION] 
                        FROM [ITEM_RecordsHeader] a
                        LEFT JOIN Sup_ItemMaster b
                        ON a.sup_ItemCode=b.sup_ItemCode
                        " + whereClause + " ORDER BY sup_DESCRIPTION";

                using (SqlCommand cmD = new SqlCommand(stR, sqlConn))
                {
                    sqlConn.Open();
                    cmD.CommandTimeout = 0;
                    DataTable dT = new DataTable();
                    SqlDataAdapter dA = new SqlDataAdapter(cmD);
                    dA.Fill(dT);
                    ddItem.Items.Clear();
                    ddItem.DataSource = dT;
                    ddItem.DataTextField = "sup_DESCRIPTION";
                    ddItem.DataValueField = "sup_ItemCode";
                    ddItem.DataBind();

                    if (ddCategory.SelectedValue == "-1")
                    {
                        ddItem.Items.Insert(0, new ListItem("All Items", "0"));
                    }
                    else
                    {
                        ddItem.Items.Insert(0, new ListItem("Please select item", "0"));
                    }
                    ddItem.SelectedIndex = 0;
                }
            }
        }

        protected void ddCategory_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddCategory.SelectedValue == "0")
            {
                ddItem.DataSource = null;
                ddItem.Items.Clear();
            }
            else
            {
                loadItemSupplies();
            }
            gvStockCard.DataSource = null;
            gvStockCard.DataBind();

        }
    }
}