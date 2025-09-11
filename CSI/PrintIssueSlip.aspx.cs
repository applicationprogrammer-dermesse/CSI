using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using System.Data;
using System.Data.SqlClient;

namespace CSI
{
    public partial class PrintIssueSlip : System.Web.UI.Page
    {

        public string theBranchToPrint;
        public string theIssueSlipNo;
        public string theUser;
        public string theCategory;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                theBranchToPrint = Session["TheBranch"].ToString();
                theIssueSlipNo = Session["TheNo"].ToString();
                theUser = Session["UserFullName"].ToString();
                theCategory = Session["Category"].ToString();
                loadIssueSlip();
                
                
                
            }

            
        }

        public string strToPrint;

        private void loadIssueSlip()
        {
            using (SqlConnection conN = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["MyConnCSI"].ConnectionString))
            {

                string conStr = System.Configuration.ConfigurationManager.ConnectionStrings["MyConnCSI"].ConnectionString;
//                string stR = @"SELECT a.[Sup_ItemCode]
//                          ,b.sup_DESCRIPTION
//                          ,a.[vQtyPicked]
//                          ,a.[vBatchNo]
//                          ,a.[vDateExpiry]
//	                      ,a.[BrName]
//                      FROM [ITEM_Pick] a
//                      LEFT JOIN Sup_ItemMaster b
//                      ON a.Sup_ItemCode=b.Sup_ItemCode
//                      where a.Sup_ControlNo='" + theIssueSlipNo.ToString() + "' order by a.OrderID";

                if (Session["TheType"].ToString() == "12")
                {
                    strToPrint = @"SELECT a.[Sup_ItemCode]
                          ,b.sup_DESCRIPTION
                          ,a.[vQtyPicked]
                          ,a.[vBatchNo]
                          ,a.[vDateExpiry]
	                      ,a.[BrName]
                      FROM [ITEM_Pick] a
                      LEFT JOIN Sup_ItemMaster b
                      ON a.Sup_ItemCode=b.Sup_ItemCode
					  where vStat=0 and TransactionType=12
					  and Sup_CategoryNum='" + theCategory.ToString() + "' and vPickedBy='" + theUser.ToString() + "'";
                }
                else
                {
                    strToPrint = @"SELECT a.[Sup_ItemCode]
                          ,b.sup_DESCRIPTION
                          ,a.[vQtyPicked]
                          ,a.[vBatchNo]
                          ,a.[vDateExpiry]
	                      ,a.[BrName]
                      FROM [ITEM_Pick] a
                      LEFT JOIN Sup_ItemMaster b
                      ON a.Sup_ItemCode=b.Sup_ItemCode
					  where vStat=0 and TransactionType=11
					  and Sup_CategoryNum='" + theCategory.ToString() + "' and vPickedBy='" + theUser.ToString() + "'";
                }

                using (SqlCommand cmD = new SqlCommand(strToPrint, conN))
                {
                    conN.Open();
                    SqlDataAdapter dA = new SqlDataAdapter(cmD);
                    DataSet dS = new DataSet();
                    dA.Fill(dS);

                    SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder(conStr);

                    //ConnectionInfo crConnectionInfo = new ConnectionInfo();
                    //crConnectionInfo.ServerName = builder["Data Source"].ToString();
                    //crConnectionInfo.DatabaseName = builder["Initial Catalog"].ToString();
                    //crConnectionInfo.UserID = builder["User Id"].ToString();
                    //crConnectionInfo.Password = builder["Password"].ToString();



                    string ServerName = builder["Data Source"].ToString();
                    string DatabaseName = builder["Initial Catalog"].ToString();
                    string UserID = builder["User ID"].ToString();
                    string Password = builder["Password"].ToString();

                    ReportDocument crp = new ReportDocument();




                    crp.Load(Server.MapPath("~/CryFile/IssueSlip.rpt"));
                    crp.SetDatabaseLogon(UserID, Password, ServerName, DatabaseName);
                    //crp.SetDatabaseLogon("sa", "citadmin", "192.168.5.85", "SMSTEST1");
                    crp.DataSourceConnections[0].SetConnection(ServerName, DatabaseName, UserID, Password);

                    crp.SetDataSource(dS.Tables["table"]);



                    //crp.SetParameterValue("TheBranch", theBranchToPrint.ToString().TrimEnd());
                    crp.SetParameterValue("TheBranch", theBranchToPrint.ToString());
                    //crp.SetParameterValue("theDate", theDate.ToString());
                    //crp.SetParameterValue("theDelivered", theDelBy.ToString());
                    //crp.SetParameterValue("theIssueNo", theprint.ToString());
                    //crp.SetParameterValue("theIssuanceNo", theprint.ToString());

                    cryIssueSlip.ReportSource = crp;

                    //TableLogOnInfo crTableLogoninfo = new TableLogOnInfo();

                    //foreach (CrystalDecisions.CrystalReports.Engine.Table CrTable in crp.Database.Tables)
                    //{
                    //    crTableLogoninfo = CrTable.LogOnInfo;
                    //    crTableLogoninfo.ConnectionInfo = crConnectionInfo;
                    //    CrTable.ApplyLogOnInfo(crTableLogoninfo);
                    //}

                  

                    crp.ExportToHttpResponse(ExportFormatType.PortableDocFormat, Response, false, "IssueSlip");

                    crp.Close();
                    crp.Dispose();
                    GC.Collect();
                }
            }
        }


    }
}