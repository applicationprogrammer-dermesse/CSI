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
    public partial class setupAddItem : System.Web.UI.Page
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

                    loadCategory();
                    ListAllItems();

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
                                  FROM [Sup_Category] where Sup_CategoryNum in (1,2,3,4,5,6,9)  order by [Sup_CategoryName]";
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


        private void ListAllItems()
        {
            using (SqlConnection sqlConn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["MyConnCSI"].ConnectionString))
            {
                string stR = @"dbo.LoadItemMasterList";
                using (SqlCommand cmD = new SqlCommand(stR, sqlConn))
                {
                    sqlConn.Open();
                    cmD.CommandTimeout = 0;
                    cmD.CommandType = CommandType.StoredProcedure;
                    cmD.Parameters.AddWithValue("@Category", ddCategory.SelectedValue);
                    DataTable dT = new DataTable();
                    SqlDataAdapter dA = new SqlDataAdapter(cmD);
                    dA.Fill(dT);

                    gvItems.DataSource = dT;
                    gvItems.DataBind();

                }
            }
        }

        protected void gvItems_RowEditing(object sender, GridViewEditEventArgs e)
        {
            Label lblStat = (Label)gvItems.Rows[e.NewEditIndex].FindControl("lbliStat");
            gvItems.EditIndex = e.NewEditIndex;
            theStat = lblStat.Text;
            ListAllItems();
        }

        protected void gvItems_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            gvItems.EditIndex = -1;
            ListAllItems();
        }

        protected void gvItems_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            try
            {
                string TheID = gvItems.DataKeys[e.RowIndex].Value.ToString();

                GridViewRow row = gvItems.Rows[e.RowIndex];
                TextBox txtsup_DESCRIPTION = (TextBox)row.FindControl("txtsup_DESCRIPTION");
                TextBox txtsup_UOM = (TextBox)row.FindControl("txtsup_UOM");
                TextBox txtsup_UnitCost = (TextBox)row.FindControl("txtsup_UnitCost");
                TextBox txtsup_PurchasingCode = (TextBox)row.FindControl("txtsup_PurchasingCode");

                DropDownList ddStatus = gvItems.Rows[e.RowIndex].FindControl("ddStatus") as DropDownList;

                string connString = System.Configuration.ConfigurationManager.ConnectionStrings["MyConnCSI"].ConnectionString;

                string sql = @"dbo.UpdateItem";
                using (SqlConnection conn = new SqlConnection(connString))
                {
                    conn.Open();

                    using (SqlCommand cmD = new SqlCommand(sql, conn))
                    {
                        cmD.CommandTimeout = 0;
                        cmD.CommandType = CommandType.StoredProcedure;
                        cmD.Parameters.AddWithValue("@sup_ID", TheID.ToString());
                        cmD.Parameters.AddWithValue("@sup_DESCRIPTION",txtsup_DESCRIPTION.Text);
                        cmD.Parameters.AddWithValue("@sup_UOM",txtsup_UOM.Text.TrimEnd());
                        cmD.Parameters.AddWithValue("@sup_UnitCost",txtsup_UnitCost.Text);
                        cmD.Parameters.AddWithValue("@sup_CreatedBy", Session["UserFullName"].ToString());
                        cmD.Parameters.AddWithValue("@sup_PurchasingCode", txtsup_PurchasingCode.Text);
                        if (ddStatus.SelectedValue == "Active")
                        {
                            cmD.Parameters.AddWithValue("@Status", "1");
                        }
                        else
                        {
                            cmD.Parameters.AddWithValue("@Status", "0");
                        }

                        cmD.ExecuteNonQuery();
                    }
                }
                
                gvItems.EditIndex = -1;
                ListAllItems();
                lblMsg.Text = "Item successfully updated.";
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

        protected void gvItems_PreRender(object sender, EventArgs e)
        {
            if (gvItems.Rows.Count > 0)
            {
                gvItems.UseAccessibleHeader = true;
                gvItems.HeaderRow.TableSection = TableRowSection.TableHeader;
                gvItems.FooterRow.TableSection = TableRowSection.TableFooter;
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (IsPageRefresh == true)
            {
                Response.Redirect(Request.Url.AbsoluteUri);
            }
            else
            {
                if (ddCategory.SelectedItem.Text == "All Category")
                {
                    ddCategory.Focus();
                    lblMsg.Text = "Please select Category!";
                    ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "Popup", "ShowSuccessMsg();", true);
                    return;


                }
                else if (txtItemDescription.Text == string.Empty)
                {
                    txtItemDescription.Focus();
                    lblMsg.Text = "Please supply item description.";
                    ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "Popup", "ShowSuccessMsg();", true);
                    return;

                }
                else if (txtUOM.Text == string.Empty)
                {
                    txtUOM.Focus();
                    lblMsg.Text = "Please supply Unit of Measure!";
                    ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "Popup", "ShowSuccessMsg();", true);
                    return;
                }
                else
                {

                    SaveItem();



                }
            }
        }


        private void SaveItem()
        {
            if (IsPageRefresh == true)
            {
                Response.Redirect(Request.Url.AbsoluteUri);
            }
            else
            {
                //decimal theUnitCost = Convert.ToDecimal(TxtUnitCost.Text);

                using (SqlConnection sqlConn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["MyConnCSI"].ConnectionString))
                {

                    string stR = @"dbo.AddNewItem";
                    using (SqlCommand cmD = new SqlCommand(stR, sqlConn))
                    {
                        sqlConn.Open();
                        cmD.CommandTimeout = 0;
                        cmD.CommandType = CommandType.StoredProcedure;
                        cmD.Parameters.AddWithValue("@sup_ItemCode", txtItemCode.Text.ToUpper().Trim());
                        cmD.Parameters.AddWithValue("@sup_DESCRIPTION", txtItemDescription.Text.Trim());
                        cmD.Parameters.AddWithValue("@sup_CATEGORY", ddCategory.SelectedValue);
                        cmD.Parameters.AddWithValue("@sup_UOM", txtUOM.Text.TrimEnd());
                        cmD.Parameters.AddWithValue("@sup_UnitCost", 0);
                        cmD.Parameters.AddWithValue("@sup_CreatedBy", Session["UserFullName"].ToString());
                        cmD.Parameters.AddWithValue("@ResultValue", SqlDbType.Int).Direction = ParameterDirection.Output;
                        cmD.ExecuteNonQuery();
                        int Result = Convert.ToInt32(cmD.Parameters["@ResultValue"].Value);

                        if (Result == 99)
                        {
                            lblMsg.Text = "itemcode already exists!";
                            ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "Popup", "ShowSuccessMsg();", true);
                            return;


                        }
                        else
                        {
                            lblLastItemcodeUsed.Text = string.Empty;
                            txtItemCode.Text = string.Empty;
                            txtItemDescription.Text = string.Empty;
                            txtUOM.Text = string.Empty;

                            
                            ListAllItems();
                            getLastItemCode();

                            lblMsg.Text = "New Item successfully added!";
                            ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "Popup", "ShowSuccessMsg();", true);
                            return;

                        }

                    }
                }
            }

        }

        private void getLastItemCode()
        {
            using (SqlConnection sqlConn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["MyConnCSI"].ConnectionString))
            {

                string stR = @"SELECT max(sup_ItemCode)
                                  FROM Sup_ItemMaster
                                  where sup_CATEGORY = '" + ddCategory.SelectedValue  + "'";

                using (SqlCommand cmD = new SqlCommand(stR, sqlConn))
                {
                    sqlConn.Open();
                    SqlDataReader dR = cmD.ExecuteReader();
                    while (dR.Read())
                    {
                        lblLastItemcodeUsed.Text = dR[0].ToString();
                    }
                }

            }
        }

        protected void ddCategory_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddCategory.SelectedValue != "0")
            {
                getLastItemCode();
            }
            else
            {
                lblLastItemcodeUsed.Text = string.Empty;
            }
            ListAllItems();
        }

        public string theStat;
        protected void gvItems_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow && gvItems.EditIndex == e.Row.RowIndex)
            {
                

                DropDownList ddStat = (DropDownList)e.Row.FindControl("ddStatus");

                if (theStat.ToString() == "Active")
                {

                    ddStat.Items.Insert(0, "Active");
                    ddStat.Items.Insert(1, "Inactive");
                }
                else
                {
                    ddStat.Items.Insert(0, "Inactive");
                    ddStat.Items.Insert(1, "Active");
                }
            }

        }

    }
}