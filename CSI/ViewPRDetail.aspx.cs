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
    public partial class ViewPRDetail : System.Web.UI.Page
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
                    lblBranch.Text = Session["BrName"].ToString();
                    lblPRNo.Text = Session["PRNo"].ToString();
                    lblREquiredDate.Text = Session["RequiredDate"].ToString();
                    lblEncodedBy.Text = Session["EncodedBy"].ToString().ToUpper();

                    loadSuppliers();
                    loadUnpostedPREntry();
        

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

        private void loadSuppliers()
        {
            using (SqlConnection sqlConn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["MyConnPUR"].ConnectionString))
            {

                string stR = @"SELECT DISTINCT SuppCode,SuppName 
                                FROM [SUPPLIER]
                                WHERE SuppName IS NOT NULL AND SuppName<>''
								ORDER BY SuppName";

                using (SqlCommand cmD = new SqlCommand(stR, sqlConn))
                {
                    sqlConn.Open();
                    cmD.CommandTimeout = 0;
                    DataTable dT = new DataTable();
                    SqlDataAdapter dA = new SqlDataAdapter(cmD);
                    dA.Fill(dT);
                    ddSupplier.Items.Clear();
                    ddSupplier.DataSource = dT;
                    ddSupplier.DataTextField = "SuppName";
                    ddSupplier.DataValueField = "SuppCode";
                    ddSupplier.DataBind();
                    ddSupplier.Items.Insert(0, new ListItem("Please select supplier", "0"));
                    ddSupplier.SelectedIndex = 0;
                }
            }
        }

        private void loadUnpostedPREntry()
        {
            using (SqlConnection sqlConn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["MyConnCSI"].ConnectionString))
            {

                string stR = @"SELECT  a.[RecNum]
                                ,ROW_NUMBER() OVER (ORDER BY c.[sup_DESCRIPTION]) AS [RowNum]
                                ,a.[sup_ItemCode]
                                  ,c.[sup_DESCRIPTION]
								  ,c.sup_UOM
                                  ,a.[vQtyBalance]
                                  ,a.[vQtyRequest]
                              FROM [ITEM_PREntry] a 
							  LEFT JOIN MyBranchList b
							  ON a.BrCode=b.BrCode
							  Left join Sup_ItemMaster c
							  ON a.sup_ItemCode=c.sup_ItemCode
							  WHERE a.[vStat]=1 AND a.PRNo='" + Session["PRNo"].ToString() + "'";

                using (SqlCommand cmD = new SqlCommand(stR, sqlConn))
                {
                    sqlConn.Open();
                    cmD.CommandTimeout = 0;
                    //cmD.CommandType = CommandType.StoredProcedure;
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
            gvItems.EditIndex = e.NewEditIndex;
            loadUnpostedPREntry();
        }

        protected void gvItems_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            gvItems.EditIndex = -1;
            loadUnpostedPREntry();
        }

        protected void gvItems_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            try
            {
                string TheID = gvItems.DataKeys[e.RowIndex].Value.ToString();

                string connString = System.Configuration.ConfigurationManager.ConnectionStrings["MyConnCSI"].ConnectionString;

                string sql = @"DELETE FROM ITEM_PREntry where RecNum = @TheID";
                using (SqlConnection conn = new SqlConnection(connString))
                {
                    conn.Open();

                    using (SqlCommand cmD = new SqlCommand(sql, conn))
                    {
                        cmD.CommandTimeout = 0;
                        cmD.Parameters.AddWithValue("@TheID", TheID.ToString());
                        cmD.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception x)
            {

                lblMsg.Text = x.Message;
                ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "Popup", "ShowSuccessMsg();", true);
                return;
            }

            loadUnpostedPREntry();
        }

        protected void gvItems_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            try
            {
                string TheRecNum = gvItems.DataKeys[e.RowIndex].Value.ToString();

                GridViewRow row = gvItems.Rows[e.RowIndex];
                TextBox txtrQuantity = (TextBox)row.FindControl("txtvQuantity");

                string connString = System.Configuration.ConfigurationManager.ConnectionStrings["MyConnCSI"].ConnectionString;

                string sql = @"Update ITEM_PREntry  set vQtyRequest=@vQty
                            where RecNum = @RecNum";
                using (SqlConnection conn = new SqlConnection(connString))
                {
                    conn.Open();

                    using (SqlCommand cmD = new SqlCommand(sql, conn))
                    {
                        cmD.CommandTimeout = 0;
                        cmD.Parameters.AddWithValue("@RecNum", TheRecNum.ToString());
                        cmD.Parameters.AddWithValue("@vQty", txtrQuantity.Text);
                        cmD.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception x)
            {

                lblMsg.Text = x.Message;
                ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "Popup", "ShowSuccessMsg();", true);
                return;
            }

            gvItems.EditIndex = -1;
            loadUnpostedPREntry();
        }


        public string filenameOfFile;
        public string newFileName;
        public string xlsHeader;
        protected void btnPrint_Click(object sender, EventArgs e)
        {
            string localPath = Server.MapPath("~/exlTMP/PRFormNew.xlsx");
            string newPath = Server.MapPath("~/exlDUMP/PRFormNew.xlsx");
            newFileName = Server.MapPath("~/exlDUMP/PRFormExcel.xlsx");

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


                worksheet.Cell("F11").Value = lblBranch.Text;
                worksheet.Cell("Q4").Value = lblPRNo.Text;
                worksheet.Cell("Z10").Value = lblREquiredDate.Text;
                worksheet.Cell("G14").Value = ddSupplier.SelectedItem.Text;
                worksheet.Cell("G15").Value =lblContactPerson.Text;
                worksheet.Cell("G16").Value = lblContactNo.Text;
                
                worksheet.Cell("F9").Value = lblEncodedBy.Text.ToUpper().TrimEnd();

                for (int i = 0; i < gvItems.Rows.Count; i++)
                {
                    

                    worksheet.Cell(i + 20, 1).Value = Server.HtmlDecode(gvItems.Rows[i].Cells[1].Text);
                    worksheet.Cell(i + 20, 2).Value = Server.HtmlDecode(gvItems.Rows[i].Cells[2].Text);
                    worksheet.Cell(i + 20, 4).Value = ((Label)gvItems.Rows[i].FindControl("lblQuantity")).Text;

                    worksheet.Cell(i + 20, 7).Value = Server.HtmlDecode(gvItems.Rows[i].Cells[4].Text);
                    worksheet.Cell(i + 20, 8).Value = Server.HtmlDecode(gvItems.Rows[i].Cells[3].Text);
                    worksheet.Cell(i + 20, 17).Value = Server.HtmlDecode(gvItems.Rows[i].Cells[5].Text);

                    //worksheet.Cell(i + 6, 1).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                    //worksheet.Cell(i + 6, 3).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                    //worksheet.Cell(i + 6, 4).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
    
                    

                }



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

        protected void btnPost_Click(object sender, EventArgs e)
        {

            foreach (GridViewRow gvR in gvItems.Rows)
            {
                if (gvR.RowType == DataControlRowType.DataRow)
                {
                    using (SqlConnection sqlConn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["MyConnCSI"].ConnectionString))
                    {
                        string stR = @"Update [ITEM_PREntry] set vStat=2,PostedBy=@PostedBy,
                                            PostedDate=@PostedDate
                                            where RecNum = @RecNum";

                        using (SqlCommand cmD = new SqlCommand(stR, sqlConn))
                        {
                            sqlConn.Open();
                            cmD.CommandTimeout = 0;
                            cmD.Parameters.AddWithValue("@RecNum", gvR.Cells[0].Text);
                            cmD.Parameters.AddWithValue("@PostedBy", Session["UserFullName"].ToString());
                            cmD.Parameters.AddWithValue("@PostedDate", DateTime.Now);
                            cmD.ExecuteNonQuery();
                        }
                    }

                }
            }

            lblMsgSuccessPosting.Text = lblBranch.Text + " - " + lblPRNo.Text + " Successfully posted!";
            ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "Popup", "ShowMsgSuccessPosting();", true);
            return;

        }

        protected void ddSupplier_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddSupplier.SelectedValue == "0")
            {
                lblContactNo.Text = string.Empty;
                lblContactPerson.Text = string.Empty;
            }
            else
            {
                LoadSupplierInfo();
            }
        }


        private void LoadSupplierInfo()
        {
            using (SqlConnection sqlConn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["MyConnPUR"].ConnectionString))
            {

                string stR = @"SELECT DISTINCT ContactPerson,TelNo
                                FROM [SUPPLIER]
                                WHERE SuppCode='" + ddSupplier.SelectedValue + "'";

                using (SqlCommand cmD = new SqlCommand(stR, sqlConn))
                {
                    sqlConn.Open();
                    cmD.CommandTimeout = 0;
                    DataTable dT = new DataTable();
                    SqlDataAdapter dA = new SqlDataAdapter(cmD);
                    dA.Fill(dT);


                    lblContactPerson.Text = dT.Rows[0][0].ToString();
                    lblContactNo.Text = dT.Rows[0][1].ToString();
                }
            }
        }


    }
}