using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using ClosedXML.Excel;
using System.Reflection;

namespace CSI
{
    public partial class ForPickItemBatch : System.Web.UI.Page
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
                    loadRequestDetail();



                    lblBranchCode.Text = Session["BrCode"].ToString();
                    lblBranch.Text = Session["BranchName"].ToString();
                    lblNo.Text = Session["No"].ToString();
                    lblDateSubmit.Text = Session["DateSubmit"].ToString();
                    lblCategory.Text = Session["Category"].ToString();
                    lblCategoryNum.Text = Session["CategoryNum"].ToString();
                    lblType.Text = Session["Type"].ToString();

                    loadRequestDetail();
                    LoadOrderPicked();

                    if (Session["UserBranchCode"].ToString() == "1")
                    {
                        btnPost.Enabled = true;
                    }
                    else
                    {
                        btnPost.Enabled = false;
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


        private void loadRequestDetail()
        {
            using (SqlConnection sqlConn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["MyConnCSI"].ConnectionString))
            {
                string stR = @"dbo.LoadApprovedDetail";
                using (SqlCommand cmD = new SqlCommand(stR, sqlConn))
                {
                    sqlConn.Open();
                    cmD.CommandTimeout = 0;
                    cmD.CommandType = CommandType.StoredProcedure;
                    cmD.Parameters.AddWithValue("@BrCode", Session["BrCode"].ToString());
                    cmD.Parameters.AddWithValue("@ControlNo", Session["No"].ToString());
                    cmD.Parameters.AddWithValue("@Sup_CategoryNum", lblCategoryNum.Text);

                    DataTable dT = new DataTable();
                    SqlDataAdapter dA = new SqlDataAdapter(cmD);
                    dA.Fill(dT);

                    gvForPick.DataSource = dT;
                    gvForPick.DataBind();

                }
            }
        }

        public string sqlRemoved;
        protected void gvForPick_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            try
            {
                string TheID = gvForPick.DataKeys[e.RowIndex].Value.ToString();

                string connString = System.Configuration.ConfigurationManager.ConnectionStrings["MyConnCSI"].ConnectionString;
                if (Session["UserBranchCode"].ToString() == "1")
                {
                    sqlRemoved = @"Update [BranchRequest] set Sup_Stat=4 ,Sup_ApprovedBy= 'Del by: " + Session["UserFullName"].ToString() + "' ,Sup_DateApproved = '" + DateTime.Now + "' where Sup_RequestID = @TheID";
                }
                else
                {
                    sqlRemoved = @"DELETE FROM [BranchRequest] where Sup_RequestID = @TheID";
                }
                using (SqlConnection conn = new SqlConnection(connString))
                {
                    conn.Open();

                    using (SqlCommand cmD = new SqlCommand(sqlRemoved, conn))
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

            loadRequestDetail();
        }

        protected void gvForPick_RowEditing(object sender, GridViewEditEventArgs e)
        {
            gvForPick.EditIndex = e.NewEditIndex;
            loadRequestDetail();
        }

        protected void gvForPick_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            gvForPick.EditIndex = -1;
            loadRequestDetail();
        }

        protected void gvForPick_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            try
            {
                string TheID = gvForPick.DataKeys[e.RowIndex].Value.ToString();

                GridViewRow row = gvForPick.Rows[e.RowIndex];
                TextBox txtQuantity = (TextBox)row.FindControl("txtQuantity");

                string connString = System.Configuration.ConfigurationManager.ConnectionStrings["MyConnCSI"].ConnectionString;

                string sql = @"Update [BranchRequest] set Sup_Qty=@Sup_Qty where Sup_RequestID = @TheID";
                using (SqlConnection conn = new SqlConnection(connString))
                {
                    conn.Open();

                    using (SqlCommand cmD = new SqlCommand(sql, conn))
                    {
                        cmD.CommandTimeout = 0;
                        cmD.Parameters.AddWithValue("@TheID", TheID.ToString());
                        cmD.Parameters.AddWithValue("@Sup_Qty", txtQuantity.Text);
                        cmD.ExecuteNonQuery();
                    }
                }

                gvForPick.EditIndex = -1;
                loadRequestDetail();

                

            }
            catch (Exception x)
            {

                lblMsg.Text = x.Message;
                ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "Popup", "ShowSuccessMsg();", true);
                return;
            }

            gvForPick.EditIndex = -1;
            loadRequestDetail();
        }

        protected void btnPost_Click(object sender, EventArgs e)
        {
            checkIfQtyMatch();
            
        }


        private void checkIfQtyMatch()
        {
            if (IsPageRefresh == true)
            {
                Response.Redirect(Request.Url.AbsoluteUri);
            }
            else
            {
                using (SqlConnection conN = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["MyConnCSI"].ConnectionString))
                {
                    string stR1 = @"dbo.CheckIfQtyMatchApprovedStatus";
                    using (SqlCommand cmD = new SqlCommand(stR1, conN))
                    {
                        conN.Open();
                        cmD.CommandTimeout = 0;
                        cmD.CommandType = CommandType.StoredProcedure;
                        cmD.Parameters.AddWithValue("@Sup_ControlNo", lblNo.Text);
                        cmD.Parameters.AddWithValue("@BrCode", lblBranchCode.Text);
                        SqlDataAdapter dA = new SqlDataAdapter(cmD);
                        DataTable dT = new DataTable();
                        dA.Fill(dT);
                        gvItemQtyMatch.DataSource = dT;
                        gvItemQtyMatch.DataBind();
                        
                        if (gvItemQtyMatch.Rows.Count > 0)
                        {

                            ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "Popup", "ShowGridItemQtyMatch();", true);
                            return;

                        }

                        else
                        {
                            PostPickedItem();
                        }

                    }
                }
            }
        }

        private void PostPickedItem()
        {
            if (IsPageRefresh == true)
            {
                Response.Redirect(Request.Url.AbsoluteUri);
            }
            else
            {
                try
                {

                    foreach (GridViewRow gvR in gvForPick.Rows)
                    {
                        if (gvR.RowType == DataControlRowType.DataRow)
                        {
                            using (SqlConnection sqlConn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["MyConnCSI"].ConnectionString))
                            {
                                string stR = @"dbo.SendToForIssuance";

                                using (SqlCommand cmD = new SqlCommand(stR, sqlConn))
                                {
                                    sqlConn.Open();
                                    cmD.CommandTimeout = 0;
                                    cmD.CommandType = CommandType.StoredProcedure;
                                    cmD.Parameters.AddWithValue("@TheID", gvR.Cells[0].Text);
                                    cmD.ExecuteNonQuery();
                                }
                            }

                        }
                    }

                    lblMsgSuccessPosting.Text = lblBranch.Text + " - " + lblNo.Text + " Successfully Picked!";
                    ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "Popup", "ShowMsgSuccessPosting();", true);
                    return;

                    
                }
                catch (Exception x)
                {

                    lblMsg.Text = x.Message;
                    ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "Popup", "ShowSuccessMsg();", true);
                    return;
                }
            }
        }

        protected void gvForPick_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "SelectBatch")
            {
                if (Convert.ToInt32(Session["UserBranchCode"]) == 1)
                {
                    GridViewRow gvr = (GridViewRow)(((LinkButton)e.CommandSource).NamingContainer);
                    Label lblQuantity = (Label)gvr.FindControl("lblQuantity");
                    int RowIndex = gvr.RowIndex;

                    string theOrderID = gvr.Cells[0].Text;
                    string theItemCode = gvr.Cells[1].Text;
                    string theQtyToPick = lblQuantity.Text;
                    string theItemDesc = gvr.Cells[2].Text;
                    GetItemBatch(theItemCode, theOrderID, theQtyToPick, theItemDesc);
                }
                else
                {
                    lblMsg.Text = "You are not authorized to select item batches.";
                    ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "Popup", "ShowSuccessMsg();", true);
                    return;
                }
            }

            else if (e.CommandName == "ChangeItem")
            {
                if (Convert.ToInt32(Session["UserBranchCode"]) == 1)
                {
                    GridViewRow gvr = (GridViewRow)(((LinkButton)e.CommandSource).NamingContainer);
                    //Label lblQuantity = (Label)gvr.FindControl("lblQuantity");
                    int RowIndex = gvr.RowIndex;

                    string theOrderID = gvr.Cells[0].Text;
                    string theItemCode = gvr.Cells[1].Text;
                    //string theQtyToPick = lblQuantity.Text;
                    string theItemDesc = gvr.Cells[2].Text;
                    GetItemList(theItemCode, theOrderID, theItemDesc);
                }
                else
                {
                    lblMsg.Text = "You are not authorized to change item.";
                    ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "Popup", "ShowSuccessMsg();", true);
                    return;
                }
            }

        }

        private void GetItemBatch(string theItemCode, String theOrderID, string theQtyToPick, string theItemDesc)
        {
            if (IsPageRefresh == true)
            {
                Response.Redirect(Request.Url.AbsoluteUri);
            }
            else
            {
                try
                {
                    lblOrderID.Text = theOrderID.ToString();
                    lblQtyToPick.Text = theQtyToPick.ToString();
                    lblItemCodeToPick.Text = theItemCode.ToString();
                    lblItemDescToPick.Text = theItemDesc.ToString();
                    using (SqlConnection Conn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["MyConnCSI"].ConnectionString))
                    {


                        string stRGetBal = @"dbo.GetItemBatches";
                        using (SqlCommand cmD = new SqlCommand(stRGetBal, Conn))
                        {
                            Conn.Open();
                            cmD.CommandType = CommandType.StoredProcedure;
                            cmD.Parameters.AddWithValue("@ItemCode", theItemCode);
                            cmD.Parameters.AddWithValue("@CategoryType",lblCategoryNum.Text);
                            DataTable dT = new DataTable();
                            SqlDataAdapter dA = new SqlDataAdapter(cmD);
                            dA.Fill(dT);

                            if (dT.Rows.Count > 0)
                            {
                                gvItemBatch.DataSource = dT;
                                gvItemBatch.DataBind();
                                ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "Popup", "ShowGridItemBatch();", true);
                                return;
                            }
                            else
                            {
                                lblMsg.Text = "No Available Balance!";
                                ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "Popup", "ShowSuccessMsg();", true);
                                return;
                            }
                        }
                    }
                }
                catch (Exception x)
                {
                    lblMsg.Text = x.Message;
                    ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "Popup", "ShowSuccessMsg();", true);
                    return;
                }
            }
        }

        

        private void AssignBatchNumber(GridViewUpdateEventArgs e)
        {
            if (IsPageRefresh == true)
            {
                Response.Redirect(Request.Url.AbsoluteUri);
            }
            else
            {
                //try
                //{
                    GridViewRow row = gvItemBatch.Rows[e.RowIndex];
                    TextBox xtQtyPicked = (TextBox)row.FindControl("txtQtyPicked");

                    using (SqlConnection sqlConn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["MyConnCSI"].ConnectionString))
                    {
                        string stR = @"dbo.InsertIntoItemPick";

                        using (SqlCommand cmD = new SqlCommand(stR, sqlConn))
                        {
                            sqlConn.Open();
                            cmD.CommandTimeout = 0;
                            cmD.CommandType = CommandType.StoredProcedure;
                            cmD.Parameters.AddWithValue("@OrderID", lblOrderID.Text);
                            cmD.Parameters.AddWithValue("@BrCode",lblBranchCode.Text);
                            cmD.Parameters.AddWithValue("@BrName", lblBranch.Text);
                            cmD.Parameters.AddWithValue("@Sup_ControlNo", lblNo.Text);
                            cmD.Parameters.AddWithValue("@HeaderID", gvItemBatch.Rows[e.RowIndex].Cells[0].Text);
                            cmD.Parameters.AddWithValue("@Sup_ItemCode", lblItemCodeToPick.Text);
                            cmD.Parameters.AddWithValue("@Sup_NoOfItemReq", lblQtyToPick.Text);
                            cmD.Parameters.AddWithValue("@vQtyPicked", xtQtyPicked.Text);
                            cmD.Parameters.AddWithValue("@vBatchNo", gvItemBatch.Rows[e.RowIndex].Cells[2].Text);
                            cmD.Parameters.AddWithValue("@vDateExpiry", gvItemBatch.Rows[e.RowIndex].Cells[3].Text);
                            cmD.Parameters.AddWithValue("@vStat", 0);
                            cmD.Parameters.AddWithValue("@vPickedBy", Session["UserFullName"].ToString());

                            cmD.ExecuteNonQuery();

                        }
                    }


                    loadRequestDetail();
                    LoadOrderPicked();
                //}

                //catch (Exception x)
                //{
                //    lblMsg.Text = x.GetType().ToString();
                //    ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "Popup", "ShowSuccessMsg();", true);
                //    return;
                //}

            }
        }

        //protected void gvItemBatch_RowUpdating(object sender, GridViewUpdateEventArgs e)
        //{
        //    if (IsPageRefresh == true)
        //    {
        //        Response.Redirect(Request.Url.AbsoluteUri);
        //    }
        //    else
        //    {
        //        try
        //        {
        //            GridViewRow row = gvItemBatch.Rows[e.RowIndex];
        //            TextBox xt2QtyPicked = (TextBox)row.FindControl("txtQtyPicked");

        //            AssignBatchNumber(e);

                    
        //        }
        //        catch (Exception x)
        //        {
        //            lblMsg.Text = x.Message;
        //            ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "Popup", "ShowSuccessMsg();", true);
        //            return;
        //        }
        //    }
        //}




        private void LoadOrderPicked()
        {
            Label3.Visible = true;
            try
            {
                using (SqlConnection sqlConn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["MyConnCSI"].ConnectionString))
                {
                    string stR = @"SELECT A.[vRecNum]
 	                                  ,A.OrderID
                                      ,A.[HeaderID]
                                      ,A.[Sup_ControlNo]
                                      ,A.[Sup_ItemCode]
	                                  ,b.sup_DESCRIPTION
                                      ,A.[vQtyPicked]
                                      ,A.[vBatchNo]
                                      ,A.[vDateExpiry]
                                  FROM [ITEM_Pick] A
                                  LEFT JOIN [Sup_ItemMaster] B
                                   ON A.[Sup_ItemCode]=B.Sup_ItemCode
                                WHERE A.[Sup_ControlNo]=@ControlNo and A.BrCode=@BrCode ORDER BY A.[Sup_ItemCode]";

                    using (SqlCommand cmD = new SqlCommand(stR, sqlConn))
                    {
                        sqlConn.Open();
                        cmD.CommandTimeout = 0;
                        cmD.Parameters.AddWithValue("@ControlNo", lblNo.Text);
                        cmD.Parameters.AddWithValue("@BrCode", lblBranchCode.Text);
                        SqlDataAdapter dA = new SqlDataAdapter(cmD);
                        DataTable dT = new DataTable();
                        dA.Fill(dT);
                        gvPicked.DataSource = dT;
                        gvPicked.DataBind();
                    }
                }
            }
            catch (SqlException x)
            {
                lblMsg.Text = x.GetType().ToString();
                ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "Popup", "ShowSuccessMsg();", true);
                return;
            }
            catch (Exception y)
            {
                lblMsg.Text = y.GetType().ToString();
                ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "Popup", "ShowSuccessMsg();", true);
                return;
            }

        }

        protected void gvPickedItems_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            if (IsPageRefresh == true)
            {
                Response.Redirect(Request.Url.AbsoluteUri);
            }
            else
            {
                try
                {
                    string RecNo = gvPicked.DataKeys[e.RowIndex].Value.ToString();
                    string connString = System.Configuration.ConfigurationManager.ConnectionStrings["MyConnCSI"].ConnectionString;
                    string sql = @"Delete from ITEM_Pick  where vRecNum='" + RecNo + "'";
                    using (SqlConnection conn = new SqlConnection(connString))
                    {
                        conn.Open();

                        using (SqlCommand cmd = new SqlCommand(sql, conn))
                        {
                            cmd.CommandTimeout = 0;
                            cmd.ExecuteNonQuery();
                        }
                    }

                    LoadOrderPicked();
                }
                catch (Exception y)
                {
                    lblMsg.Text = y.Message;
                    ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "Popup", "ShowSuccessMsg();", true);
                    return;
                }
            }
        }

        public string filenameOfFile;
        public string newFileName;
        protected void btnExcel_Click(object sender, EventArgs e)
        {
            try
            {
                string localPath = Server.MapPath("~/exlTMP/rptApproved.xlsx");
                string newPath = Server.MapPath("~/exlDUMP/rptApproved.xlsx");
                newFileName = Server.MapPath("~/exlDUMP/" + lblNo.Text + ".xlsx");


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


                    //worksheet.Cell("A3").Value = Session["Option"].ToString();

                    worksheet.Cell("A1").Value = lblBranch.Text;
                    worksheet.Cell("A2").Value = lblNo.Text;
                    worksheet.Cell("A3").Value = "Date Submit : " + lblDateSubmit.Text;
                    worksheet.Cell("A4").Value = lblCategory.Text;

                    for (int i = 0; i < gvForPick.Rows.Count; i++)
                    {
                        
                        worksheet.Cell(i + 7, 1).Value = Server.HtmlDecode(gvForPick.Rows[i].Cells[1].Text);
                        worksheet.Cell(i + 7, 2).Value = Server.HtmlDecode(gvForPick.Rows[i].Cells[2].Text);
                        worksheet.Cell(i + 7, 3).Value = Server.HtmlDecode(gvForPick.Rows[i].Cells[3].Text);
                        worksheet.Cell(i + 7, 4).Value = Server.HtmlDecode(gvForPick.Rows[i].Cells[4].Text);
                        worksheet.Cell(i + 7, 5).Value = Server.HtmlDecode(gvForPick.Rows[i].Cells[5].Text);
                        worksheet.Cell(i + 7, 6).Value = ((Label)gvForPick.Rows[i].FindControl("lblQuantity")).Text;



                        worksheet.Cell(i + 7, 1).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                        worksheet.Cell(i + 7, 2).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                        worksheet.Cell(i + 7, 3).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                        worksheet.Cell(i + 7, 4).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                        worksheet.Cell(i + 7, 5).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                        worksheet.Cell(i + 7, 6).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;


                        //worksheet.Cell(i + 7, 1).Style.Font.FontSize = 13;
                        //worksheet.Cell(i + 7, 2).Style.Font.FontSize = 13;
                        //worksheet.Cell(i + 7, 3).Style.Font.FontSize = 13;
                        //worksheet.Cell(i + 7, 4).Style.Font.FontSize = 13;
                        //worksheet.Cell(i + 7, 5).Style.Font.FontSize = 13;
                        //worksheet.Cell(i + 7, 6).Style.Font.FontSize = 13;
                        //worksheet.Cell(i + 7, 7).Style.Font.FontSize = 13;




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
            catch (Exception x)
            {
                lblMsg.Text = x.Message;
                ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "Popup", "ShowSuccessMsg();", true);
                return;
            }
        }

        protected void btnUpdateItem_Click(object sender, EventArgs e)
        {
            if (IsPageRefresh == true)
            {
                Response.Redirect(Request.Url.AbsoluteUri);
            }
            else
            {
                using (SqlConnection sqlConn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["MyConnCSI"].ConnectionString))
                {
                    string stR = @"UPDATE [BranchRequest] SET Sup_ItemCode=@Sup_ItemCode where Sup_RequestID=@Sup_RequestID";

                    using (SqlCommand cmD = new SqlCommand(stR, sqlConn))
                    {
                        sqlConn.Open();
                        cmD.CommandTimeout = 0;
                        //cmD.CommandType = CommandType.StoredProcedure;
                        cmD.Parameters.AddWithValue("@Sup_RequestID", lblOrderIDToChange.Text);
                        cmD.Parameters.AddWithValue("@Sup_ItemCode", ddItem.SelectedValue);
                        cmD.ExecuteNonQuery();
                    }
                }

                loadRequestDetail();
            }

        }


        private void GetItemList(string theItemCodeToChange, String theOrderIDToChange, string theItemDescToChange)
        {
            if (IsPageRefresh == true)
            {
                Response.Redirect(Request.Url.AbsoluteUri);
            }
            else
            {
                try
                {
                    lblOrderIDToChange.Text = theOrderIDToChange.ToString();
                    lblItemCodeToChange.Text = theItemCodeToChange.ToString();
                    lblItemDescToChange.Text = theItemDescToChange.ToString();
                    using (SqlConnection Conn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["MyConnCSI"].ConnectionString))
                    {


                        string stRGetBal = @"dbo.LoadItemMasterListReplacement";
                        using (SqlCommand cmD = new SqlCommand(stRGetBal, Conn))
                        {
                            Conn.Open();
                            cmD.CommandType = CommandType.StoredProcedure;
                            cmD.Parameters.AddWithValue("@CategoryName", lblCategory.Text);
                            DataTable dT = new DataTable();
                            SqlDataAdapter dA = new SqlDataAdapter(cmD);
                            dA.Fill(dT);

                            ddItem.Items.Clear();
                            ddItem.DataSource = dT;
                            ddItem.DataTextField = "sup_DESCRIPTION";
                            ddItem.DataValueField = "sup_ItemCode";
                            ddItem.DataBind();
                            ddItem.Items.Insert(0, new ListItem("Please select item", "0"));
                            ddItem.SelectedIndex = 0;

                            ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "Popup", "ShowGridItemToChange();", true);

                        }
                    }
                }
                catch (Exception x)
                {
                    lblMsg.Text = x.Message;
                    ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "Popup", "ShowSuccessMsg();", true);
                    return;
                }
            }
        }

        //protected void gvItemBatch_RowUpdating(object sender, GridViewUpdateEventArgs e)
        //{
        //    GridViewRow row = gvItemBatch.Rows[e.RowIndex];
        //    TextBox xt2QtyPicked = (TextBox)row.FindControl("txtQtyPicked");

        //    AssignBatchNumber(e);
        //}

        protected void gvItemBatch_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (IsPageRefresh == true)
            {
                Response.Redirect(Request.Url.AbsoluteUri);
            }
            else
            {

                if (e.CommandName == "linkSelectBatch")
                {
                    GridViewRow gvr = (GridViewRow)(((LinkButton)e.CommandSource).NamingContainer);
                    TextBox txt2QtyPicked = (TextBox)gvr.FindControl("txtQtyPicked");

                    string connString = System.Configuration.ConfigurationManager.ConnectionStrings["MyConnCSI"].ConnectionString;
                    string sql = @"GetItemBatchesBalance";
                    using (SqlConnection conn = new SqlConnection(connString))
                    {
                        conn.Open(); using (SqlCommand cmD = new SqlCommand(sql, conn))
                        {
                            cmD.CommandTimeout = 0;
                            cmD.CommandType = CommandType.StoredProcedure;
                            cmD.Parameters.AddWithValue("@HeaderID", gvr.Cells[0].Text);
                            //cmD.Parameters.AddWithValue("@HeaderID", 9505);
                            SqlDataAdapter dA = new SqlDataAdapter(cmD);
                            DataTable dT = new DataTable();
                            dA.Fill(dT);

                            int theBalanceHeader = Convert.ToInt32(dT.Rows[0][5]);
                            int thePickQty = Convert.ToInt32(txt2QtyPicked.Text);
                            if (theBalanceHeader >= thePickQty)
                            {
                                InsertSelectedItemBatch(e);
                            }

                            else
                            {
                                lblMsg.Text = "Insufficient balance.<br />  Remaining balance is = " + dT.Rows[0][5].ToString() + "<br />There was an update on the qty balance of the selected item batch.<br /> Please reload item batches selected";
                                ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "Popup", "ShowSuccessMsg();", true);
                                return;
                            }
                        }
                    }
                    

                }

            }
        }

        private void InsertSelectedItemBatch(GridViewCommandEventArgs e)
        {
            GridViewRow gvr = (GridViewRow)(((LinkButton)e.CommandSource).NamingContainer);
            TextBox txt2QtyPicked = (TextBox)gvr.FindControl("txtQtyPicked");

            using (SqlConnection sqlConn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["MyConnCSI"].ConnectionString))
            {
                string stR = @"dbo.InsertIntoItemPick";

                using (SqlCommand cmD = new SqlCommand(stR, sqlConn))
                {
                    sqlConn.Open();
                    cmD.CommandTimeout = 0;
                    cmD.CommandType = CommandType.StoredProcedure;
                    cmD.Parameters.AddWithValue("@OrderID", lblOrderID.Text);
                    cmD.Parameters.AddWithValue("@BrCode", lblBranchCode.Text);
                    cmD.Parameters.AddWithValue("@BrName", lblBranch.Text);
                    cmD.Parameters.AddWithValue("@Sup_ControlNo", lblNo.Text);
                    cmD.Parameters.AddWithValue("@HeaderID", gvr.Cells[0].Text);
                    cmD.Parameters.AddWithValue("@Sup_ItemCode", lblItemCodeToPick.Text);
                    cmD.Parameters.AddWithValue("@Sup_NoOfItemReq", lblQtyToPick.Text);
                    cmD.Parameters.AddWithValue("@vQtyPicked", txt2QtyPicked.Text);
                    cmD.Parameters.AddWithValue("@vBatchNo", gvr.Cells[2].Text);
                    cmD.Parameters.AddWithValue("@vDateExpiry", gvr.Cells[3].Text);
                    cmD.Parameters.AddWithValue("@vStat", 0);
                    if (lblCategory.Text == "DPI Items(For Selling)" | lblCategory.Text == "DCI Items(For Selling)")
                    {
                        cmD.Parameters.AddWithValue("@IsSelling", 1);
                    }
                    else
                    {
                        cmD.Parameters.AddWithValue("@IsSelling", 0);
                    }

                    cmD.Parameters.AddWithValue("@vPickedBy", Session["UserFullName"].ToString());
                    cmD.Parameters.AddWithValue("@RequestType", Server.HtmlDecode(lblType.Text));

                    cmD.Parameters.AddWithValue("@Sup_CategoryNum", lblCategoryNum.Text);
                    cmD.ExecuteNonQuery();

                }
            }


            loadRequestDetail();
            LoadOrderPicked();
        }

        protected void LinkPost_Click(object sender, EventArgs e)
        {
            checkIfQtyMatch();
        }
        
            
            
        
        
    }
}