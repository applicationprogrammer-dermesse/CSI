using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using ClosedXML.Excel;
using System.IO;

namespace CSI
{
    public partial class PrintMultipleRequest : System.Web.UI.Page
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


                    lblBranchCode.Text = Session["BrCodeToPrint"].ToString();
                    lblBranch.Text = Session["BranchNameToPrint"].ToString();

                    LoadItemSel();
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


        


        private void LoadItemSel()
        {
            using (SqlConnection conN = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["MyConnCSI"].ConnectionString))
            {
                string stR = @"dbo.LoadMultiplePrinting";
                using (SqlCommand cmD = new SqlCommand(stR, conN))
                {
                    conN.Open();
                    cmD.CommandTimeout = 0;
                    cmD.CommandType = CommandType.StoredProcedure;
                    cmD.Parameters.AddWithValue("@BrCode", lblBranchCode.Text);
                    SqlDataReader dR = cmD.ExecuteReader();

                    selMultiple.Items.Clear();
                    selMultiple.DataSource = dR;
                    selMultiple.DataValueField = "Sup_ControlNo";
                    selMultiple.DataTextField = "Sup_ControlNo";
                    selMultiple.DataBind();
                    selMultiple.Items.Insert(0, new ListItem("", string.Empty));
                    selMultiple.SelectedIndex = 0;
                }
            }
        }

        public string strText;
        public string valueList;
        public string strgroupids;

        protected void btnLoadDetailToPrint_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < selMultiple.Items.Count; i++)
            {
                if (selMultiple.Items[i].Selected)
                {
                    strText += "," + selMultiple.Items[i].Text;
                    valueList += "'" + selMultiple.Items[i].Value + "',";

                    strgroupids = valueList.Remove(valueList.Length - 1);

                }

            }

            if (strgroupids.ToString() == "''")
            {
                selMultiple.Focus();
                lblMsg.Text = "Please select item";
                ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "Popup", "ShowSuccessMsg();", true);
                return;
            }
            else
            {
                genDetailedToPrint();
            }
            
        }

        private void genDetailedToPrint()
        {
            using (SqlConnection conN = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["MyConnCSI"].ConnectionString))
            {
                string stR1 = @"SELECT a.Sup_ControlNo, a.Sup_ItemCode + ' - ' + c.sup_DESCRIPTION as [ItemDesc]
                            ,a.vQtyPicked
                            ,c.sup_UOM
                            ,ltrim(rtrim(a.vBatchNo)) as vBatchNo
                            ,a.vDateExpiry 
			                              FROM [ITEM_Pick] a
			                              LEFT JOIN Sup_ItemMaster c
				                            ON a.[Sup_ItemCode]=c.[Sup_ItemCode]
			                              WHERE a.[vStat]=1 AND a.BrCode=@BrCode AND a.Sup_ControlNo in (" + strgroupids + ") " +
                             " ORDER BY a.Sup_ItemCode,c.sup_DESCRIPTION,a.Sup_ControlNo";
                using (SqlCommand cmD = new SqlCommand(stR1, conN))
                {
                    conN.Open();
                    cmD.CommandTimeout = 0;
                    cmD.Parameters.AddWithValue("@BrCode", lblBranchCode.Text);
                    SqlDataAdapter dA = new SqlDataAdapter(cmD);
                    DataTable dT = new DataTable();
                    dA.Fill(dT);

                    gvPrint.DataSource = dT;
                    gvPrint.DataBind();

                }
            }
        }


        public string filenameOfFile;
        public string newFileName;
        public string xlsHeader;
        protected void btnPrint_Click(object sender, EventArgs e)
        {
            string localPath = Server.MapPath("~/exlTMP/IStemp.xlsx");
            string newPath = Server.MapPath("~/exlDUMP/IStemp.xlsx");
            //newFileName = Server.MapPath("~/exlDUMP/DR_" + txtReceiptNo.Text + ".xlsx");
            newFileName = Server.MapPath("~/exlDUMP/IssueSlip.xlsx");

            File.Copy(localPath, newPath, overwrite: true);

            FileInfo fi = new FileInfo(newPath);
            if (fi.Exists)
            {
                if (File.Exists(newFileName))
                {
                    File.Delete(newFileName);
                }

                fi.MoveTo(newFileName);
                var workbook = new XLWorkbook(newFileName);
                var worksheet = workbook.Worksheet(1);



                worksheet.Cell("C2").Value = lblBranch.Text;




                for (int i = 0; i < gvPrint.Rows.Count; i++)
                {
                    //worksheet.Cell(i + 13, 1).Value = Server.HtmlDecode(gvPrint.Rows[i].Cells[1].Text);
                    worksheet.Cell(i + 13, 2).Value = Server.HtmlDecode(gvPrint.Rows[i].Cells[1].Text) + " " + Server.HtmlDecode(gvPrint.Rows[i].Cells[2].Text);
                    worksheet.Cell(i + 13, 6).Value = Server.HtmlDecode(gvPrint.Rows[i].Cells[3].Text);
                    worksheet.Cell(i + 13, 7).Value = Server.HtmlDecode(gvPrint.Rows[i].Cells[4].Text);
                    worksheet.Cell(i + 13, 8).Value = Server.HtmlDecode(gvPrint.Rows[i].Cells[5].Text);
                    worksheet.Cell(i + 13, 9).Value = ((Label)gvPrint.Rows[i].FindControl("lblvDateExpiry")).Text;  //Server.HtmlDecode(gvPrint.Rows[i].Cells[6].Text);
                }


                int MaxRow = 14 + gvPrint.Rows.Count;

                worksheet.Cell(MaxRow, 2).Value = "************* Nothing Follows *************";


                var fileName = Path.GetFileName(newFileName);

                Response.Clear();
                Response.Buffer = true;
                Response.Charset = "";
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.AddHeader("content-disposition", "inline; filename=" + fileName);
                using (MemoryStream MyMemoryStream = new MemoryStream())
                {
                    workbook.SaveAs(MyMemoryStream);
                    MyMemoryStream.WriteTo(Response.OutputStream);
                    Response.Flush();
                    Response.End();
                }

            }
        }

    }
}