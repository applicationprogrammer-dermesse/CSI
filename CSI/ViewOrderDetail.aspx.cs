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
    public partial class ViewOrderDetail : System.Web.UI.Page
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
                    
                     lblOrderDate.Text = Session["OrderDate"].ToString();
                     lblReferenceNo.Text = Session["ReferenceNo"].ToString();
                     lblSource.Text = Session["Source"].ToString();
                     lblType.Text = Session["Type"].ToString();
                     lblCustomerName.Text = Session["CustomerName"].ToString();
                     lblCustomerAddress.Text = Session["Customer Address"].ToString();
                     lblContactNo.Text = Session["ContactNo"].ToString();
                     lblEmailAddress.Text = Session["EmailAddress"].ToString();

                    loadUnpostedOrderEntry();
                    loadCourier();

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

        private void loadCourier()
        {
            using (SqlConnection conN = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["MyConnCSI"].ConnectionString))
            {
                string stR = @"SELECT CourierName FROM Courier ORDER BY CourierID";
                using (SqlCommand cmD = new SqlCommand(stR, conN))
                {
                    conN.Open();
                    SqlDataReader dR = cmD.ExecuteReader();

                    ddCourier.Items.Clear();
                    ddCourier.DataSource = dR;
                    ddCourier.DataValueField = "CourierName";
                    ddCourier.DataTextField = "CourierName";
                    ddCourier.DataBind();
                    ddCourier.Items.Insert(0, new ListItem("Please select courier", "0"));
                    ddCourier.SelectedIndex = 0;
                }
            }
        }


        private void loadUnpostedOrderEntry()
        {
            using (SqlConnection sqlConn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["MyConnCSI"].ConnectionString))
            {

                string stR = @"dbo.LoadOnlineOrderDetail";

                using (SqlCommand cmD = new SqlCommand(stR, sqlConn))
                {
                    sqlConn.Open();
                    cmD.CommandTimeout = 0;
                    cmD.CommandType = CommandType.StoredProcedure;
                    cmD.Parameters.AddWithValue("@ReferenceNo",Session["ReferenceNo"].ToString());
                    cmD.Parameters.AddWithValue("@Source", Session["Source"].ToString());
                    DataTable dT = new DataTable();
                    SqlDataAdapter dA = new SqlDataAdapter(cmD);
                    dA.Fill(dT);

                    gvItems.DataSource = dT;
                    gvItems.DataBind();

                    if (dT.Rows.Count > 0)
                    {
                        lblMOP.Text = dT.Rows[0][8].ToString();
                        lblDeliveryInstruction.Text = dT.Rows[0][9].ToString();
                        lblShippingFee.Text = dT.Rows[0][10].ToString();
                    }

                }
            }
        }


        
        
        

        
        //here
        protected void btnPost_Click(object sender, EventArgs e)
        {

            foreach (GridViewRow gvR in gvItems.Rows)
            {
                if (gvR.RowType == DataControlRowType.DataRow)
                {
                    using (SqlConnection sqlConn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["MyConnCSI"].ConnectionString))
                    {
                        string stR = @"Update [ITEM_OnlineSold] set vStat=2,DateDelivered=@DateDelivered,
                                            DeliveredBy=@DeliveredBy
                                            where OrderID = @OrderID";

                        using (SqlCommand cmD = new SqlCommand(stR, sqlConn))
                        {
                            sqlConn.Open();
                            cmD.CommandTimeout = 0;
                            cmD.Parameters.AddWithValue("@OrderID", gvR.Cells[0].Text);
                            cmD.Parameters.AddWithValue("@DeliveredBy", ddCourier.SelectedItem.Text);
                            cmD.Parameters.AddWithValue("@DateDelivered", txtDateDelivered.Text);
                            //cmD.Parameters.AddWithValue("@DatePostedDelivered", DateTime.Now);
                            cmD.ExecuteNonQuery();
                        }
                    }

                }
            }

            lblMsgSuccessPosting.Text = "Successfully posted!";
            ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "Popup", "ShowMsgSuccessPosting();", true);
            return;

        }




        public string filenameOfFile;
        public string newFileName;
        public string xlsHeader;
        protected void btnPrint_Click(object sender, EventArgs e)
        {
            
            //try
            //{
                //if (txtReqDate.Text == "")
                //{
                //    txtReqDate.Focus();
                //    lblMsgWarning.Text = "Please supply required date and click SAVE button to update records.";
                //    ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "Popup", "ShowWarningMsg();", true);
                //    return;
                //}
                //else if (txtReceiptNo.Text == "")
                //{
                //    txtReceiptNo.Focus();
                //    lblMsgWarning.Text = "Please supply Receipt No. and click SAVE button to update records.";
                //    ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "Popup", "ShowWarningMsg();", true);
                //    return;
                //}
                //else if (ddCourier.SelectedValue == "0")
                //{
                //    ddCourier.Focus();
                //    lblMsgWarning.Text = "Please select courier and click SAVE button to update records.";
                //    ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "Popup", "ShowWarningMsg();", true);
                //    return;
                //}

                //else
                //{
                    string localPath = Server.MapPath("~/exlTMP/DRtemp.xlsx");
                    string newPath = Server.MapPath("~/exlDUMP/DRtemp.xlsx");
                    //newFileName = Server.MapPath("~/exlDUMP/DR_" + txtReceiptNo.Text + ".xlsx");
                    newFileName = Server.MapPath("~/exlDUMP/DR_12345.xlsx");

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



                        worksheet.Cell("D6").Value = lblCustomerName.Text;
                        worksheet.Cell("D7").Value = lblCustomerAddress.Text;
                        //worksheet.Cell("D8").Value = txtCity.Text + ", " + txtregion.Text;
                        worksheet.Cell("D10").Value = "'" + lblContactNo.Text;

                        //worksheet.Cell("J4").Value =  txtReqDate.Text;
                        //worksheet.Cell("J5").Value = txtBrPrefix.Text + "-" + txtReqDate.Text.Substring(8, 2) + "-" + txtReceiptNo.Text;
                        //worksheet.Cell("J8").Value = txtOrderNo.Text;

                        
                        

                        for (int i = 0; i < gvItems.Rows.Count; i++)
                        {
                            worksheet.Cell(i + 13, 1).Value = Server.HtmlDecode(gvItems.Rows[i].Cells[1].Text);
                            worksheet.Cell(i + 13, 2).Value = Server.HtmlDecode(gvItems.Rows[i].Cells[2].Text);
                            worksheet.Cell(i + 13, 4).Value = Server.HtmlDecode(gvItems.Rows[i].Cells[3].Text);
                            worksheet.Cell(i + 13, 8).Value = Server.HtmlDecode(gvItems.Rows[i].Cells[5].Text);
                            worksheet.Cell(i + 13, 9).Value = Server.HtmlDecode(gvItems.Rows[i].Cells[6].Text);
                            //worksheet.Cell(i + 12, 6).Value = Server.HtmlDecode(gvItems.Rows[i].Cells[5].Text);

                            //worksheet.Cell(i + 9, 17).Value = ((Label)gvItems.Rows[i].FindControl("lblQuarantine")).Text;

                        }

                        worksheet.Cell("D48").Value = Session["UserFullName"].ToString();
                        worksheet.Cell("J42").Value = lblShippingFee.Text;
                        worksheet.Cell("I48").Value = ddCourier.SelectedItem.Text;

                        
                        if (lblMOP.Text == "Credit Card/PAYMAYA")
                        {
                            worksheet.Cell("C42").Value = "X";
                            worksheet.Cell("C43").Value = "";
                            worksheet.Cell("C44").Value = "";
                            worksheet.Cell("C45").Value = "";
                            worksheet.Cell("C46").Value = "";
                        }

                        else if (lblMOP.Text == "Debit Card")
                        {
                            worksheet.Cell("C42").Value = "";
                            worksheet.Cell("C43").Value = "X";
                            worksheet.Cell("C44").Value = "";
                            worksheet.Cell("C45").Value = "";
                            worksheet.Cell("C46").Value = "";
                        }

                        else if (lblMOP.Text == "G Cash")
                        {
                            worksheet.Cell("C42").Value = "";
                            worksheet.Cell("C43").Value = "";
                            worksheet.Cell("C44").Value = "X";
                            worksheet.Cell("C45").Value = "";
                            worksheet.Cell("C46").Value = "";
                        }

                        else if (lblMOP.Text == "Cash")
                        {
                            worksheet.Cell("C42").Value = "";
                            worksheet.Cell("C43").Value = "";
                            worksheet.Cell("C44").Value = "";
                            worksheet.Cell("C45").Value = "X";
                            worksheet.Cell("C46").Value = "";
                        }

                        else if (lblMOP.Text == "Bank Transfer / Paid")
                        {
                            worksheet.Cell("C42").Value = "";
                            worksheet.Cell("C43").Value = "";
                            worksheet.Cell("C44").Value = "";
                            worksheet.Cell("C45").Value = "";
                            worksheet.Cell("C46").Value = "X";
                        }

                        int MaxRow = 13 + gvItems.Rows.Count;

                        worksheet.Cell(MaxRow, 4).Value = "************* Nothing Follows *************";
                        worksheet.Cell(MaxRow + 1, 4).Value = "   ONLINE ORDER CUSTOMER   ";

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
                //}
            //}
            //catch (Exception x)
            //{
            //    lblMsg.Text = x.Message;
            //    ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "Popup", "ShowSuccessMsg();", true);
            //    return;
            //}
        }
       


    }
}