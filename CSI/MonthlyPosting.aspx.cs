using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.IO;

namespace CSI
{
    public partial class MonthlyPosting : System.Web.UI.Page
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
                    loadYear();

                    


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


        private void loadYear()
        {
            int CurrYear = System.DateTime.Now.Year;
            ddYear.Items.Insert(0, "" + CurrYear + "");

            int lessYear = 2;

            for (int y = 1; y <= lessYear; y++)
            {
                ddYear.Items.Add((CurrYear - y).ToString());
            }

        }


        private void loadCategory()
        {
            using (SqlConnection conN = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["MyConnCSI"].ConnectionString))
            {
                string stR = @"dbo.LoadCategoryConsumption";
                using (SqlCommand cmD = new SqlCommand(stR, conN))
                {
                    conN.Open();
                    cmD.CommandTimeout = 0;
                    cmD.CommandType = CommandType.StoredProcedure;
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


        private void loadLastMonthlyPost()
        {
            using (SqlConnection sqlConn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["MyConnCSI"].ConnectionString))
            {
                string stR = @"dbo.LoadLastMonthlyPostingBranch";
                using (SqlCommand cmD = new SqlCommand(stR, sqlConn))
                {
                    sqlConn.Open();
                    cmD.CommandTimeout = 0;
                    cmD.CommandType = CommandType.StoredProcedure;
                    cmD.Parameters.AddWithValue("@BrCode", Session["UserBranchCode"].ToString());
                    cmD.Parameters.AddWithValue("@Sup_CategoryNum", ddCategory.SelectedValue);
                    DataTable dT = new DataTable();
                    SqlDataAdapter dA = new SqlDataAdapter(cmD);
                    dA.Fill(dT);

                    gvMP.DataSource = dT;
                    gvMP.DataBind();

                }
            }
        }

        protected void btnPost_Click(object sender, EventArgs e)
        {
            using (SqlConnection conN = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["MyConnCSI"].ConnectionString))
            {
                string stR = @"SELECT * FROM [dbo].[ITEM_RecordsHeaderMonthly]  WHERE BrCode=@BrCode AND [iYear] = @Yr AND iMonth=@PrevMo AND Sup_CategoryNum=@Category";
                using (SqlCommand cmD = new SqlCommand(stR, conN))
                {
                    conN.Open();
                    cmD.CommandTimeout = 0;

                    cmD.Parameters.AddWithValue("@BrCode", Session["UserBranchCode"].ToString());
                    
                    if (ddMonth.SelectedValue == "1")
                    {
                        
                        cmD.Parameters.AddWithValue("@Yr", Convert.ToInt32(ddYear.SelectedItem.Text) - 1);
                        cmD.Parameters.AddWithValue("@PrevMo", "12");
                    }
                    else
                    {
                        cmD.Parameters.AddWithValue("@PrevMo",Convert.ToInt32(ddMonth.SelectedValue) - 1);
                        cmD.Parameters.AddWithValue("@Yr", ddYear.SelectedItem.Text);
                    }
                    cmD.Parameters.AddWithValue("@Category", ddCategory.SelectedValue);

                    SqlDataReader dR = cmD.ExecuteReader();
                    if (dR.HasRows)
                    {
                        ProcessMonthlyPosting();
                    }
                    else
                    {
                        lblMsg.Text = "Previous Month not yet Posted\n\n" + ddCategory.SelectedItem.Text;
                        ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "Popup", "ShowSuccessMsg();", true);
                        return;
                    }
                }
            }

            
        }

        private void ProcessMonthlyPosting()
        {
            if (IsPageRefresh == true)
            {
                Response.Redirect(Request.Url.AbsoluteUri);
            }
            else
            {
                using (SqlConnection conN = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["MyConnCSI"].ConnectionString))
                {
                    string stR = @"dbo.ProcessMonthlyPostingBranch";
                    using (SqlCommand cmD = new SqlCommand(stR, conN))
                    {
                        conN.Open();
                        cmD.CommandType = CommandType.StoredProcedure;
                        cmD.CommandTimeout = 0;

                        cmD.Parameters.AddWithValue("@BrCode", Session["UserBranchCode"].ToString());
                        cmD.Parameters.AddWithValue("@Yr", ddYear.SelectedItem.Text);
                        cmD.Parameters.AddWithValue("@Mo", ddMonth.SelectedValue);
                        cmD.Parameters.AddWithValue("@Category", ddCategory.SelectedValue);
                        cmD.Parameters.AddWithValue("@ProcessBy", Session["UserFullName"].ToString());
                        cmD.Parameters.AddWithValue("@ResultValue", SqlDbType.Int).Direction = ParameterDirection.Output;
                        cmD.ExecuteNonQuery();
                        int Result = Convert.ToInt32(cmD.Parameters["@ResultValue"].Value);

                        if (Result == 99)
                        {
                            lblMsg.Text = "Already Posted\n\n" + ddCategory.SelectedItem.Text + " for the month of " + ddMonth.SelectedItem.Text + "  Year " + ddYear.SelectedItem.Text;
                            ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "Popup", "ShowSuccessMsg();", true);
                            return;
                        }

                        else
                        {
                            loadLastMonthlyPost();
                            lblMsg.Text = "Successfully Posted\n\n" + ddCategory.SelectedItem.Text + " for the month of " + ddMonth.SelectedItem.Text + "  Year " + ddYear.SelectedItem.Text;
                            ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "Popup", "ShowSuccessMsg();", true);
                            return;
                        }
                    }
                }
            }
        }

        protected void ddCategory_SelectedIndexChanged(object sender, EventArgs e)
        {
            using (SqlConnection sqlConn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["MyConnCSI"].ConnectionString))
            {
//                string stR = @"SELECT b.sup_CATEGORY, c.Sup_CategoryName
//                                                    ,a.[Sup_ItemCode]
//                                              FROM [ITEM_PickBranch] a LEFT JOIN Sup_ItemMaster b
//                                              ON a.Sup_ItemCode=b.sup_ItemCode
//                                              left join Sup_Category c
//                                              ON b.sup_CATEGORY=c.Sup_CategoryNum
//                                              WHERE a.BrCode=@BrCode AND a.TransactionType=1 AND a.vStat=0 and b.sup_CATEGORY=@sup_CATEGORY";

                string stR = @"SELECT d.Sup_CategoryNum, c.Sup_CategoryName
                                                    ,a.[Sup_ItemCode]
                                              FROM [ITEM_PickBranch] a LEFT JOIN Sup_ItemMaster b
                                              ON a.Sup_ItemCode=b.sup_ItemCode
											  LEFT JOIN ITEM_RecordsHeader d
											  ON a.HeaderID = d.HeaderID
                                              left join Sup_Category c
                                              ON d.Sup_CategoryNum=c.Sup_CategoryNum
                                              WHERE a.BrCode=@BrCode AND a.TransactionType=1 AND a.vStat=0 and d.Sup_CategoryNum=@sup_CATEGORY";
                using (SqlCommand cmD = new SqlCommand(stR, sqlConn))
                {
                    sqlConn.Open();
                    cmD.CommandTimeout = 0;
                    cmD.Parameters.AddWithValue("@BrCode", Session["UserBranchCode"].ToString());
                    cmD.Parameters.AddWithValue("@sup_CATEGORY", ddCategory.SelectedValue);
                    DataTable dT = new DataTable();
                    SqlDataAdapter dA = new SqlDataAdapter(cmD);
                    dA.Fill(dT);

                    if (dT.Rows.Count > 0)
                    {
                        lblNote.Text = "You cannot process Monthly Posting of " + ddCategory.SelectedItem.Text + " because of pending transaction.  Please go to Stock Used Data Entry to finish transaction.";
                        lblNote.Visible = true;
                        btnPost.Enabled = false;

                    }
                    else
                    {
                        lblNote.Text=string.Empty;
                        lblNote.Visible = false;
                        btnPost.Enabled = true;
                    }
                }
            }

            loadLastMonthlyPost();
        }


    }
}