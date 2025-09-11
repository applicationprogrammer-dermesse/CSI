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
    public partial class MasterPage : System.Web.UI.MasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["UserBranchName"] == null)
            {
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "redirect script",
                "alert('You been idle for a long period of time, Need to Sign in again!'); location.href='Login.aspx';", true);
            }
            else
            {
                if (!IsPostBack)
                {
                    lblBranch.Text = Session["UserBranchName"].ToString();
                    MenuOption();
                }
            }
        }

        private void MenuOption()
        {
            if (Session["UserBranchCode"].ToString() == "1")
            {
                if (Session["UserType"].ToString() == "1" | Session["UserType"].ToString() == "2")
                {
                    trnBR.Visible = false;
                    setupBR.Visible = false;
                    RRHO.Visible = true;
                    setupHO.Visible = true;
                    //RequestID.Visible = false;
                }
                else
                {
                    trnBR.Visible = false;
                    setupBR.Visible = false;
                    RRHO.Visible = true;
                    setupHO.Visible = false;
                }
                loadNotificationHO();
            }
 
            else
            {
                trnHO.Visible = false;
                setupHO.Visible = false;
                RRHO.Visible = false;
                loadNotification();
                if (Session["UserBranchCode"].ToString() == "50" | Session["UserBranchCode"].ToString() == "51")
                {
                    DFPMktg.Visible = true;
                    DFPBranch.Visible = false;
                }
                else
                {
                    DFPMktg.Visible = false;
                    DFPBranch.Visible = true;
                }
                 
            }
        }


        private void loadNotification()
        {
            using (SqlConnection sqlConn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["MyConnCSI"].ConnectionString))
            {
                string stR = @"SELECT c.Sup_CategoryName
                            ,COUNT(a.[Sup_ItemCode]) AS [NoOfITems]
                       FROM [ITEM_PickBranch] a LEFT JOIN  ITEM_RecordsHeader b
                      ON A.HeaderID=b.HeaderID
                      left join Sup_Category c
                      ON B.Sup_CategoryNum=c.Sup_CategoryNum
                      WHERE a.BrCode=@BrCode AND a.TransactionType=1 AND a.vStat=0
                      and B.Sup_CategoryNum in (1,2,3,4,5)
                      GROUP BY a.[BrCode],C.Sup_CategoryName";
                using (SqlCommand cmD = new SqlCommand(stR, sqlConn))
                {
                    sqlConn.Open();
                    cmD.CommandTimeout = 0;
                    cmD.Parameters.AddWithValue("@BrCode", Session["UserBranchCode"].ToString());

                    DataTable dT = new DataTable();
                    SqlDataAdapter dA = new SqlDataAdapter(cmD);
                    dA.Fill(dT);

                    if (dT.Rows.Count > 0)
                    {
                        lblHeader.Text = "Pending Stock Used Data Entry";
                        lblNotif.Text = dT.Rows.Count.ToString();
                        gvNotif.DataSource = dT;
                        gvNotif.DataBind();
                    }
                    else
                    {
                        lblHeader.Text = "No Pending Transaction";
                        lblNotif.Text = "0";
                        gvNotif.DataSource = null;
                        gvNotif.DataBind();
                    }
                }
            }
        }



        private void loadNotificationHO()
        {
            using (SqlConnection sqlConn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["MyConnCSI"].ConnectionString))
            {
                string stR = @"select c.Sup_CategoryName
                            ,COUNT(a.[Sup_ItemCode]) AS [NoOfITems]
                      FROM [ITEM_Pick] a LEFT JOIN Sup_ItemMaster b
                      ON a.Sup_ItemCode=b.sup_ItemCode
                      left join Sup_Category c
                      ON b.sup_CATEGORY=c.Sup_CategoryNum
                      WHERE  a.TransactionType=12 AND a.vStat=0 AND vPickedBy=@PickedBy
					  GROUP BY C.Sup_CategoryName";
                using (SqlCommand cmD = new SqlCommand(stR, sqlConn))
                {
                    sqlConn.Open();
                    cmD.CommandTimeout = 0;
                    cmD.Parameters.AddWithValue("@BrCode", Session["UserBranchCode"].ToString());
                    cmD.Parameters.AddWithValue("@PickedBy", Session["UserFullName"].ToString());

                    DataTable dT = new DataTable();
                    SqlDataAdapter dA = new SqlDataAdapter(cmD);
                    dA.Fill(dT);

                    if (dT.Rows.Count > 0)
                    {
                        lblHeader.Text = "You have pending Issuance-Clinic Supplies";
                        lblNotif.Text = dT.Rows.Count.ToString();
                        gvNotif.DataSource = dT;
                        gvNotif.DataBind();
                    }
                    else
                    {
                        lblHeader.Text = "No Pending Transaction";
                        lblNotif.Text = "0";
                        gvNotif.DataSource = null;
                        gvNotif.DataBind();
                    }
                }
            }
        }

    }
}