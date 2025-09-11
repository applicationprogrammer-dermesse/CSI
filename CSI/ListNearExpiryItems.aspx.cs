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
    public partial class ListNearExpiryItems : System.Web.UI.Page
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

                    if (Convert.ToInt32(Session["UserBranchCode"].ToString()) > 1)
                    {

                        loadPerBranch();
                        LoadNearExpiryPerBranch();
                        
                    }
                    else
                    {
                        loadBranch();
                        LoadNearExpiryLogistics();

                    }



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


        private void loadBranch()
        {
            using (SqlConnection conN = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["MyConnCSI"].ConnectionString))
            {
                string stR = @"SELECT BrCode,BrName FROM MyBranchList where BrCode in (1,4,5,6,9,10,11,12,13,14,16,17,18,22,23,25,27) ORDER BY BrCode";
                using (SqlCommand cmD = new SqlCommand(stR, conN))
                {
                    conN.Open();
                    cmD.CommandTimeout = 0;
                    //cmD.CommandType = CommandType.StoredProcedure;
                    SqlDataReader dR = cmD.ExecuteReader();

                    ddBranch.Items.Clear();
                    ddBranch.DataSource = dR;
                    ddBranch.DataValueField = "BrCode";
                    ddBranch.DataTextField = "BrName";
                    ddBranch.DataBind();
                    ddBranch.Items.Insert(0, new ListItem("DCI-Logistics", "1"));
                    ddBranch.SelectedIndex = 0;
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


        private void LoadNearExpiryLogistics()
        {
            using (SqlConnection sqlConn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["MyConnCSI"].ConnectionString))
            {
                string stR = @"dbo.NearExpiryLogistics";
                using (SqlCommand cmD = new SqlCommand(stR, sqlConn))
                {
                    sqlConn.Open();
                    cmD.CommandTimeout = 0;
                    cmD.CommandType = CommandType.StoredProcedure;
                    cmD.Parameters.AddWithValue("@BrCode", ddBranch.SelectedValue);

                    if (Convert.ToInt32(ddBranch.SelectedValue) > 2 & Convert.ToInt32(ddBranch.SelectedValue) < 28)
                    {
                        cmD.Parameters.AddWithValue("@Option", 1);
                    }
                    else if (Convert.ToInt32(ddBranch.SelectedValue) == 1)
                    {
                        cmD.Parameters.AddWithValue("@Option", 0);
                    }
                    else
                    {
                        cmD.Parameters.AddWithValue("@Option", 2);
                    }

                    DataTable dT = new DataTable();
                    SqlDataAdapter dA = new SqlDataAdapter(cmD);
                    dA.Fill(dT);

                   gvNearEx.DataSource = dT;
                   gvNearEx.DataBind();

                }
            }
        }
        protected void gvNearEx_PreRender(object sender, EventArgs e)
        {
            if (gvNearEx.Rows.Count > 0)
            {
                gvNearEx.UseAccessibleHeader = true;
                gvNearEx.HeaderRow.TableSection = TableRowSection.TableHeader;
                gvNearEx.FooterRow.TableSection = TableRowSection.TableFooter;
            }
        }

        protected void ddBranch_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (Convert.ToInt32(ddBranch.SelectedValue) == 1)
            {
                LoadNearExpiryLogistics();
            }
            else
            {
                LoadNearExpiryPerBranch();
            }
        }


        private void LoadNearExpiryPerBranch()
        {
            using (SqlConnection sqlConn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["MyConnCSI"].ConnectionString))
            {
                string stR = @"dbo.NearExpiryPerBranch";
                using (SqlCommand cmD = new SqlCommand(stR, sqlConn))
                {
                    sqlConn.Open();
                    cmD.CommandTimeout = 0;
                    cmD.CommandType = CommandType.StoredProcedure;
                    cmD.Parameters.AddWithValue("@BrCode", ddBranch.SelectedValue);

                    if (Convert.ToInt32(ddBranch.SelectedValue) > 2 & Convert.ToInt32(ddBranch.SelectedValue) < 28)
                    {
                        cmD.Parameters.AddWithValue("@Option", 1);
                    }
                    else
                    {
                        cmD.Parameters.AddWithValue("@Option", 2);
                    }


                    DataTable dT = new DataTable();
                    SqlDataAdapter dA = new SqlDataAdapter(cmD);
                    dA.Fill(dT);

                    gvNearEx.DataSource = dT;
                    gvNearEx.DataBind();

                }
            }
        }
    }
}