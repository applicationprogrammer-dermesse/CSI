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
    public partial class DeliveryFromPlant : System.Web.UI.Page
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
                    lblBranch.Text = Session["UserBranchName"].ToString();
                    LoadUnpostedDrDetail();
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

        protected void btnDownload_Click(object sender, EventArgs e)
        {
            //CheckIfDRExists();
            downloadDRDetail();
            
        }

        //private void CheckIfDRExists()
        //{
        //    using (SqlConnection conN = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["MyConnCSI"].ConnectionString))
        //    {
        //        string stR1 = @"SELECT DISTINCT DR_Number FROM DeliveryFromPlant WHERE DR_Number=@DR_Number";
        //        using (SqlCommand cmD = new SqlCommand(stR1, conN))
        //        {
        //            conN.Open();
        //            cmD.CommandTimeout = 0;
        //            cmD.Parameters.AddWithValue("@DR_Number", txtDRNumber.Text.Trim());
        //            SqlDataAdapter dA = new SqlDataAdapter(cmD);
        //            DataTable dT = new DataTable();
        //            dA.Fill(dT);
        //            if (dT.Rows.Count > 0)
        //            {
        //                lblMsg.Text = "DR Number already exists in download record!";
        //                ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "Popup", "ShowSuccessMsg();", true);
        //                return;
        //            }
        //            else
        //            {
        //                downloadDRDetail();
        //            }

        //        }
        //    }
        //}

        private void downloadDRDetail()
        {
            if (IsPageRefresh == true)
            {
                Response.Redirect(Request.Url.AbsoluteUri);
            }
            else
            {
                try
                {
                    using (SqlConnection sqlConn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["MyConnCSI"].ConnectionString))
                    {
                        string stR = @"dbo.DownloadDRfromPlant";
                        using (SqlCommand cmD = new SqlCommand(stR, sqlConn))
                        {
                            sqlConn.Open();
                            cmD.CommandTimeout = 0;
                            cmD.CommandType = CommandType.StoredProcedure;
                            cmD.Parameters.AddWithValue("@BrCode", Session["UserBranchCode"].ToString());
                            cmD.Parameters.AddWithValue("@DR_Number", txtDRNumber.Text);
                            cmD.ExecuteNonQuery();
                        }
                    }

                    LoadDrDetail();
                }
                catch (Exception x)
                {
                    lblMsg.Text = x.Message;
                    ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "Popup", "ShowSuccessMsg();", true);
                    return;
                }
            }
        }



        private void LoadDrDetail()
        {
            try
            {
                using (SqlConnection sqlConn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["MyConnCSI"].ConnectionString))
                {
                    string stR = @"SELECT a.[ID]
                          ,a.[BrCode]
                          ,a.[DR_Number]
                          ,a.[MRFNo]
                          ,a.[Sup_ItemCode]
						  ,a.[sup_DESCRIPTION]
                          ,a.[vQty]
                          ,a.[vBatchNo]
                          ,a.[vDateExpiry]
						  ,ISNULL(b.sup_ItemCode,0) AS [FGCode]
                      FROM [DeliveryFromPlant] a 
					  LEFT JOIN Sup_ItemMaster b
					  ON a.Sup_ItemCode=b.sup_ItemCode
					  WHERE a.[BrCode]=@BrCode AND a.vStat=0 ";
                    using (SqlCommand cmD = new SqlCommand(stR, sqlConn))
                    {
                        sqlConn.Open();
                        cmD.CommandTimeout = 0;
                        cmD.Parameters.AddWithValue("@BrCode", Session["UserBranchCode"].ToString());
                        //cmD.Parameters.AddWithValue("@DR_Number", txtDRNumber.Text);
                        DataTable dT = new DataTable();
                        SqlDataAdapter dA = new SqlDataAdapter(cmD);
                        dA.Fill(dT);

                        //gvDeliveries.DataSource = dT;
                        //gvDeliveries.DataBind();

                        if (dT.Rows.Count > 0)
                        {
                            gvDeliveries.DataSource = dT;
                            gvDeliveries.DataBind();
                        }
                        else
                        {
                            lblMsg.Text = "DR number does not exists or DR already Posted";
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


        private void LoadUnpostedDrDetail()
        {
            try
            {
                using (SqlConnection sqlConn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["MyConnCSI"].ConnectionString))
                {
                    string stR = @"SELECT a.[ID]
                          ,a.[BrCode]
                          ,a.[DR_Number]
                          ,a.[MRFNo]
                          ,a.[Sup_ItemCode]
						  ,a.[sup_DESCRIPTION]
                          ,a.[vQty]
                          ,a.[vBatchNo]
                          ,a.[vDateExpiry]
						  ,ISNULL(b.sup_ItemCode,0) AS [FGCode]
                      FROM [DeliveryFromPlant] a 
					  LEFT JOIN Sup_ItemMaster b
					  ON a.Sup_ItemCode=b.sup_ItemCode
					  WHERE a.[BrCode]=@BrCode AND a.vStat=0 ";
                    using (SqlCommand cmD = new SqlCommand(stR, sqlConn))
                    {
                        sqlConn.Open();
                        cmD.CommandTimeout = 0;
                        cmD.Parameters.AddWithValue("@BrCode", Session["UserBranchCode"].ToString());
                        //cmD.Parameters.AddWithValue("@DR_Number", txtDRNumber.Text);
                        DataTable dT = new DataTable();
                        SqlDataAdapter dA = new SqlDataAdapter(cmD);
                        dA.Fill(dT);

                        gvDeliveries.DataSource = dT;
                        gvDeliveries.DataBind();


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

        protected void btnClear_Click(object sender, EventArgs e)
        {
            ClearDelivery();
        }

        private void ClearDelivery()
        {
            if (IsPageRefresh == true)
            {
                Response.Redirect(Request.Url.AbsoluteUri);
            }
            else
            {
                

                    foreach (GridViewRow gvR in gvDeliveries.Rows)
                    {
                        if (gvR.RowType == DataControlRowType.DataRow)
                        {
                        
                                string Rec = gvDeliveries.DataKeys[gvR.RowIndex].Value.ToString();
                                using (SqlConnection sqlConn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["MyConnCSI"].ConnectionString))
                                {
                                    string stR = "Delete from [DeliveryFromPlant] where ID=@ID";

                                    sqlConn.Open();

                                    using (SqlCommand cmD = new SqlCommand(stR, sqlConn))
                                    {
                                        cmD.CommandTimeout = 0;
                                         cmD.Parameters.AddWithValue("@ID", gvR.Cells[1].Text);

                                        cmD.ExecuteNonQuery();

                                    }
                                }
                        
                        }
                    }


                    gvDeliveries.DataSource = null;
                    gvDeliveries.DataBind();
                
            }
        }

        protected void btnPost_Click(object sender, EventArgs e)
        {
            if (gvDeliveries.Rows.Count==0)
            {
                lblMsg.Text = "Please download DR to Post.";
                ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "Popup", "ShowSuccessMsg();", true);
                return;
            }
            else
            {
                PostDelivery();
            }
        }

        public string strSRS;
        private void PostDelivery()
        {
            if (IsPageRefresh == true)
            {
                Response.Redirect(Request.Url.AbsoluteUri);
            }
            else
            {
                foreach (GridViewRow gvrow in gvDeliveries.Rows)
                {
                    CheckBox chk = (CheckBox)gvrow.FindControl("ckStat");
                    if (chk != null & chk.Checked)
                    {
                        strSRS += "'" + gvDeliveries.DataKeys[gvrow.RowIndex].Value.ToString() + "',";
                    }

                }

                if (strSRS != null)
                {

                    foreach (GridViewRow gvR in gvDeliveries.Rows)
                    {
                        if (gvR.RowType == DataControlRowType.DataRow)
                        {
                            Label lbQty = gvR.Cells[8].FindControl("lblQty") as Label;
                            CheckBox chk = (CheckBox)gvR.FindControl("ckStat");
                            
                                string Rec = gvDeliveries.DataKeys[gvR.RowIndex].Value.ToString();
                                using (SqlConnection sqlConn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["MyConnCSI"].ConnectionString))
                                {
                                    string stR = "dbo.PostUnpostedDeliveryFromPlantBranch";

                                    sqlConn.Open();

                                    using (SqlCommand cmD = new SqlCommand(stR, sqlConn))
                                    {
                                        cmD.CommandTimeout = 0;
                                        cmD.CommandType = CommandType.StoredProcedure;

                                        cmD.Parameters.AddWithValue("@ID", gvR.Cells[1].Text);
                                        cmD.Parameters.AddWithValue("@FGCode", gvR.Cells[10].Text);
                                        cmD.Parameters.AddWithValue("@PostedBy", Session["UserFullName"].ToString());
                                        if (chk != null & chk.Checked)
                                        {
                                            cmD.Parameters.AddWithValue("@Opt", 1);
                                        }
                                        else
                                        {
                                            cmD.Parameters.AddWithValue("@Opt", 0);
                                        }
                                        cmD.ExecuteNonQuery();

                                    }
                                }
                            
                        }
                    }

                    txtDRNumber.Text = string.Empty;
                    gvDeliveries.DataSource = null;
                    gvDeliveries.DataBind();
                    lblMsg.Text = "Delivery succesfully posted.";
                    ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "Popup", "ShowSuccessMsg();", true);
                    return;
                }
                else
                {

                    lblMsg.Text = "Please check at least one item to post.";
                    ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "Popup", "ShowSuccessMsg();", true);
                    return;
                }
            }
        }


    }
}