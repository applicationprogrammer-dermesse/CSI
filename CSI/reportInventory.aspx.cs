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
    public partial class reportInventory : System.Web.UI.Page
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

                    //if (Session["UserBranchCode"].ToString() == "1")
                    //{
                    //    loadBranch();
                    //    loadCategory();
                    
                    //}
                    //else
                    //{
                    
                    //    loadPerBranch();
                    //    loadCategoryBranch();
                    
                    //}

                    //if (Convert.ToInt32(Session["UserBranchCode"].ToString()) > 1 & Convert.ToInt32(Session["UserBranchCode"].ToString()) < 30)
                    if (Convert.ToInt32(Session["UserBranchCode"].ToString()) > 1)
                    {
                        
                        loadPerBranch();
                        loadCategoryBranch();
                    }
                    else
                    {
                        loadBranch();
                        loadCategory();
                     

                    }

                    EnableDisabelRadios();
                    
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

        protected void gvInventoryDetailedHO_PreRender(object sender, EventArgs e)
        {
            if (gvInventoryDetailedHO.Rows.Count > 0)
            {
                gvInventoryDetailedHO.UseAccessibleHeader = true;
                gvInventoryDetailedHO.HeaderRow.TableSection = TableRowSection.TableHeader;
                gvInventoryDetailedHO.FooterRow.TableSection = TableRowSection.TableFooter;
            }
        }


        
        private void LoadInventoryDetailedLogistics()
        {
            if (rbAll.Checked==true)
            {
                theExpiration = 1;
            }
            else if (rbMoreThan1Year.Checked == true)
            {
                theExpiration = 2;
            }

            else if (rbLessThan1Year.Checked == true)
            {
                theExpiration = 3;
            }
            else if (rbLess6months.Checked == true)
            {
                theExpiration = 4;
            }
            else if (rbExpired.Checked == true)
            {
                theExpiration = 5;
            }

            using (SqlConnection sqlConn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["MyConnCSI"].ConnectionString))
            {
                string stR = @"dbo.rptInventoryDetailedLogistics";
                using (SqlCommand cmD = new SqlCommand(stR, sqlConn))
                {
                    sqlConn.Open();
                    cmD.CommandTimeout = 0;
                    cmD.CommandType = CommandType.StoredProcedure;
                    cmD.Parameters.AddWithValue("@BrCode", ddBranch.SelectedValue);
                    cmD.Parameters.AddWithValue("@Sup_CategoryNum", ddCategory.SelectedValue );
                    cmD.Parameters.AddWithValue("@theExpiration", theExpiration);
                    DataTable dT = new DataTable();
                    SqlDataAdapter dA = new SqlDataAdapter(cmD);
                    dA.Fill(dT);

                   gvInventoryDetailedHO.DataSource = dT;
                   gvInventoryDetailedHO.DataBind();

                }
            }
        }

        private void LoadInventorySummaryLogistics()
        {
            using (SqlConnection sqlConn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["MyConnCSI"].ConnectionString))
            {
                string stR = @"dbo.rptInventorySummaryLogistics";
                using (SqlCommand cmD = new SqlCommand(stR, sqlConn))
                {
                    sqlConn.Open();
                    cmD.CommandTimeout = 0;
                    cmD.CommandType = CommandType.StoredProcedure;
                    cmD.Parameters.AddWithValue("@BrCode", ddBranch.SelectedValue);
                    cmD.Parameters.AddWithValue("@Sup_CategoryNum", ddCategory.SelectedValue);
                    DataTable dT = new DataTable();
                    SqlDataAdapter dA = new SqlDataAdapter(cmD);
                    dA.Fill(dT);

                    gvInventorySummaryHO.DataSource = dT;
                    gvInventorySummaryHO.DataBind();

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
                    ddBranch.Items.Insert(0, new ListItem("All Branches", "0"));
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
                    ddCategory.Items.Insert(0, new ListItem("Select Category", "0"));
                    ddCategory.SelectedIndex = 0;
                }
            }
        }

        public string stRCategory;
        private void loadCategoryBranch()
        {
            using (SqlConnection conN = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["MyConnCSI"].ConnectionString))
            {
                if (Convert.ToInt32(Session["UserBranchCode"].ToString()) > 1 & Convert.ToInt32(Session["UserBranchCode"].ToString()) < 30)
                {
                    stRCategory = @"SELECT [Sup_CategoryNum]
                                      ,[Sup_CategoryName]
                                  FROM [Sup_Category] where Sup_CategoryNum in (1,2,3,4,5) order by [Sup_CategoryName]";
                }
                else
                {
                    stRCategory = @"SELECT [Sup_CategoryNum]
                                      ,[Sup_CategoryName]
                                  FROM [Sup_Category] where Sup_CategoryNum in (8) order by [Sup_CategoryName]";
                }

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
                    ddCategory.Items.Insert(0, new ListItem("Select Category", "0"));
                    ddCategory.SelectedIndex = 0;
                }
            }
        }

        protected void btnGenerate_Click(object sender, EventArgs e)
        {
            if (ddBranch.SelectedValue =="1")
            {
                if (ddOption.SelectedItem.Text=="Detailed")
                {
                    gvInventoryDetailedBranches.DataSource=null;
                    gvInventoryDetailedBranches.DataBind();

                    gvInventorySummaryHO.DataSource = null;
                    gvInventorySummaryHO.DataBind();

                    gvInventorySummaryBranches.DataSource = null;
                    gvInventorySummaryBranches.DataBind();

                    LoadInventoryDetailedLogistics();
                }
                else
                {
                    gvInventoryDetailedBranches.DataSource=null;
                    gvInventoryDetailedBranches.DataBind();

                    gvInventoryDetailedHO.DataSource=null;
                    gvInventoryDetailedHO.DataBind();

                    gvInventorySummaryBranches.DataSource = null;
                    gvInventorySummaryBranches.DataBind();

                    LoadInventorySummaryLogistics();
                }
            }
            else
            {
                if (ddOption.SelectedItem.Text == "Detailed")
                {
                    gvInventoryDetailedHO.DataSource = null;
                    gvInventoryDetailedHO.DataBind();

                    gvInventorySummaryHO.DataSource = null;
                    gvInventorySummaryHO.DataBind();

                    gvInventorySummaryBranches.DataSource = null;
                    gvInventorySummaryBranches.DataBind();

                    LoadInventoryDetailedBranches();
                }
                else
                {
                    gvInventoryDetailedHO.DataSource = null;
                    gvInventoryDetailedHO.DataBind();

                    gvInventorySummaryHO.DataSource = null;
                    gvInventorySummaryHO.DataBind();

                    gvInventoryDetailedBranches.DataSource = null;
                    gvInventoryDetailedBranches.DataBind();

                    LoadInventorySummaryBranches();
                }
            }
        }


        private void LoadInventoryDetailedBranches()
        {
            if (rbAll.Checked == true)
            {
                theExpiration = 1;
            }
            else if (rbMoreThan1Year.Checked == true)
            {
                theExpiration = 2;
            }

            else if (rbLessThan1Year.Checked == true)
            {
                theExpiration = 3;
            }
            else if (rbLess6months.Checked == true)
            {
                theExpiration = 4;
            }
            else if (rbExpired.Checked == true)
            {
                theExpiration = 5;
            }

            using (SqlConnection sqlConn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["MyConnCSI"].ConnectionString))
            {
                string stR = @"dbo.rptInventoryDetailedBranches";
                using (SqlCommand cmD = new SqlCommand(stR, sqlConn))
                {
                    sqlConn.Open();
                    cmD.CommandTimeout = 0;
                    cmD.CommandType = CommandType.StoredProcedure;
                    cmD.Parameters.AddWithValue("@BrCode", ddBranch.SelectedValue);
                    if (ddBranch.SelectedValue== "0")
                    {
                        cmD.Parameters.AddWithValue("@Opt", 2);
                    }
                    else
                    {
                        cmD.Parameters.AddWithValue("@Opt", 1);
                    }
                    cmD.Parameters.AddWithValue("@Sup_CategoryNum", ddCategory.SelectedValue);
                    cmD.Parameters.AddWithValue("@theExpiration", theExpiration);
                    DataTable dT = new DataTable();
                    SqlDataAdapter dA = new SqlDataAdapter(cmD);
                    dA.Fill(dT);

                    gvInventoryDetailedBranches.DataSource = dT;
                    gvInventoryDetailedBranches.DataBind();

                }
            }
        }

        private void LoadInventorySummaryBranches()
        {
            using (SqlConnection sqlConn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["MyConnCSI"].ConnectionString))
            {
                string stR = @"dbo.rptInventorySummaryBranches";
                using (SqlCommand cmD = new SqlCommand(stR, sqlConn))
                {
                    sqlConn.Open();
                    cmD.CommandTimeout = 0;
                    cmD.CommandType = CommandType.StoredProcedure;
                    cmD.Parameters.AddWithValue("@BrCode", ddBranch.SelectedValue);
                    if (ddBranch.SelectedValue == "0")
                    {
                        cmD.Parameters.AddWithValue("@Opt", 2);
                    }
                    else
                    {
                        cmD.Parameters.AddWithValue("@Opt", 1);
                    }
                    cmD.Parameters.AddWithValue("@Sup_CategoryNum", ddCategory.SelectedValue);
                    DataTable dT = new DataTable();
                    SqlDataAdapter dA = new SqlDataAdapter(cmD);
                    dA.Fill(dT);

                    gvInventorySummaryBranches.DataSource = dT;
                    gvInventorySummaryBranches.DataBind();

                }
            }
        }
        protected void gvInventoryDetailedBranches_PreRender(object sender, EventArgs e)
        {
            if (gvInventoryDetailedBranches.Rows.Count > 0)
            {
                gvInventoryDetailedBranches.UseAccessibleHeader = true;
                gvInventoryDetailedBranches.HeaderRow.TableSection = TableRowSection.TableHeader;
                gvInventoryDetailedBranches.FooterRow.TableSection = TableRowSection.TableFooter;
            }
        }

        protected void gvInventorySummaryHO_PreRender(object sender, EventArgs e)
        {
            if (gvInventorySummaryHO.Rows.Count > 0)
            {
                gvInventorySummaryHO.UseAccessibleHeader = true;
                gvInventorySummaryHO.HeaderRow.TableSection = TableRowSection.TableHeader;
                gvInventorySummaryHO.FooterRow.TableSection = TableRowSection.TableFooter;
            }
        }

        protected void gvInventorySummaryBranches_PreRender(object sender, EventArgs e)
        {
            if (gvInventorySummaryBranches.Rows.Count > 0)
            {
                gvInventorySummaryBranches.UseAccessibleHeader = true;
                gvInventorySummaryBranches.HeaderRow.TableSection = TableRowSection.TableHeader;
                gvInventorySummaryBranches.FooterRow.TableSection = TableRowSection.TableFooter;
            }
        }


        private void EnableDisabelRadios()
        {
            if (ddOption.SelectedItem.Text=="Summary")
            {
                rbAll.Enabled = false;
                rbLess6months.Enabled = false;
                rbLessThan1Year.Enabled = false;
                rbMoreThan1Year.Enabled = false;
                rbExpired.Enabled = false;
            }
            else
            {
                rbAll.Enabled = true;
                rbLess6months.Enabled = true;
                rbLessThan1Year.Enabled = true;
                rbMoreThan1Year.Enabled = true;
                rbExpired.Enabled = true;
            }
        }

        protected void ddOption_SelectedIndexChanged(object sender, EventArgs e)
        {
            EnableDisabelRadios();
        }


    }
}