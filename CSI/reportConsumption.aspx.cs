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
    public partial class reportConsumption : System.Web.UI.Page
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

            int lessYear = 5;

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

        public string filenameOfFile;
        public string newFileName;
        protected void btnGenerate_Click(object sender, EventArgs e)
        {
            using (SqlConnection sqlConn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["MyConnCSI"].ConnectionString))
            {
                string stRConsumption = @"dbo.reportComsumption";
                sqlConn.Open();
                using (SqlCommand cmD = new SqlCommand(stRConsumption, sqlConn))
                {
                    cmD.CommandType = CommandType.StoredProcedure;
                    cmD.Parameters.AddWithValue("@Category", ddCategory.SelectedValue);
                    cmD.Parameters.AddWithValue("@Year", ddYear.SelectedItem.Text.TrimEnd());
                    cmD.Parameters.AddWithValue("@Month", ddMonth.SelectedValue);

                    SqlDataAdapter dA = new SqlDataAdapter(cmD);
                    DataSet ds = new DataSet();
                    DataTable dT = new DataTable();
                    dA.Fill(ds);
                    dT = ds.Tables[0];

                    string localPath = Server.MapPath("~/exlTMP/rptConsumption.xlsx");
                    string newPath = Server.MapPath("~/exlDUMP/rptConsumption.xlsx");
                    newFileName = Server.MapPath("~/exlDUMP/Consumption.xlsx");


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


                        worksheet.Cell("A2").Value = ddCategory.SelectedItem.Text;
                        worksheet.Cell("A3").Value = "FOR THE MONTH OF " + ddMonth.SelectedItem.Text + "  YEAR " + ddYear.SelectedItem.Text;


                        foreach (DataRow row in ds.Tables[0].Rows)
                        {
                            for (int i = 0; i < dT.Rows.Count; i++)
                            {
                                worksheet.Cell(i + 8, 1).Value = Server.HtmlDecode(dT.Rows[i]["sup_ItemCode"].ToString());
                                worksheet.Cell(i + 8, 2).Value = Server.HtmlDecode(dT.Rows[i]["sup_DESCRIPTION"].ToString());
                                worksheet.Cell(i + 8, 3).Value = Server.HtmlDecode(dT.Rows[i]["UNIT COST"].ToString());
                                worksheet.Cell(i + 8, 4).Value = Server.HtmlDecode(dT.Rows[i]["NET OF VAT"].ToString());


                                worksheet.Cell(i + 8, 5).Value = Server.HtmlDecode(dT.Rows[i]["HOConsumption"].ToString());
                                worksheet.Cell(i + 8, 6).Value = Server.HtmlDecode(dT.Rows[i]["HOamt"].ToString());
                                //4
                                worksheet.Cell(i + 8, 7).Value = Server.HtmlDecode(dT.Rows[i]["SMCConsumption"].ToString());
                                worksheet.Cell(i + 8, 8).Value = Server.HtmlDecode(dT.Rows[i]["SMCamt"].ToString());
                                //5
                                worksheet.Cell(i + 8, 9).Value = Server.HtmlDecode(dT.Rows[i]["SCPConsumption"].ToString());
                                worksheet.Cell(i + 8, 10).Value = Server.HtmlDecode(dT.Rows[i]["SCPamt"].ToString());
                                //6
                                worksheet.Cell(i + 8, 11).Value = Server.HtmlDecode(dT.Rows[i]["MMLConsumption"].ToString());
                                worksheet.Cell(i + 8, 12).Value = Server.HtmlDecode(dT.Rows[i]["MMLamt"].ToString());
                                //9
                                worksheet.Cell(i + 8, 13).Value = Server.HtmlDecode(dT.Rows[i]["SCBConsumption"].ToString());
                                worksheet.Cell(i + 8, 14).Value = Server.HtmlDecode(dT.Rows[i]["SCBamt"].ToString());
                                //10
                                worksheet.Cell(i + 8, 15).Value = Server.HtmlDecode(dT.Rows[i]["AFMConsumption"].ToString());
                                worksheet.Cell(i + 8, 16).Value = Server.HtmlDecode(dT.Rows[i]["AFMamt"].ToString());
                                //11
                                worksheet.Cell(i + 8, 17).Value = Server.HtmlDecode(dT.Rows[i]["SMSConsumption"].ToString());
                                worksheet.Cell(i + 8, 18).Value = Server.HtmlDecode(dT.Rows[i]["SMSamt"].ToString());
                                //12
                                worksheet.Cell(i + 8, 19).Value = Server.HtmlDecode(dT.Rows[i]["SMBConsumption"].ToString());
                                worksheet.Cell(i + 8, 20).Value = Server.HtmlDecode(dT.Rows[i]["SMBamt"].ToString());
                                //13
                                worksheet.Cell(i + 8, 21).Value = Server.HtmlDecode(dT.Rows[i]["FVWConsumption"].ToString());
                                worksheet.Cell(i + 8, 22).Value = Server.HtmlDecode(dT.Rows[i]["FVWamt"].ToString());
                                //14
                                worksheet.Cell(i + 8, 23).Value = Server.HtmlDecode(dT.Rows[i]["GSAConsumption"].ToString());
                                worksheet.Cell(i + 8, 24).Value = Server.HtmlDecode(dT.Rows[i]["GSAamt"].ToString());
                                //16
                                worksheet.Cell(i + 8, 25).Value = Server.HtmlDecode(dT.Rows[i]["SMMConsumption"].ToString());
                                worksheet.Cell(i + 8, 26).Value = Server.HtmlDecode(dT.Rows[i]["SMMamt"].ToString());
                                //17
                                worksheet.Cell(i + 8, 27).Value = Server.HtmlDecode(dT.Rows[i]["SMPConsumption"].ToString());
                                worksheet.Cell(i + 8, 28).Value = Server.HtmlDecode(dT.Rows[i]["SMPamt"].ToString());
                                //18
                                worksheet.Cell(i + 8, 29).Value = Server.HtmlDecode(dT.Rows[i]["SCTConsumption"].ToString());
                                worksheet.Cell(i + 8, 30).Value = Server.HtmlDecode(dT.Rows[i]["SCTamt"].ToString());
                                //22
                                worksheet.Cell(i + 8, 31).Value = Server.HtmlDecode(dT.Rows[i]["BCTConsumption"].ToString());
                                worksheet.Cell(i + 8, 32).Value = Server.HtmlDecode(dT.Rows[i]["BCTamt"].ToString());
                                //23
                                worksheet.Cell(i + 8, 33).Value = Server.HtmlDecode(dT.Rows[i]["DASConsumption"].ToString());
                                worksheet.Cell(i + 8, 34).Value = Server.HtmlDecode(dT.Rows[i]["DASamt"].ToString());
                                //25
                                worksheet.Cell(i + 8, 35).Value = Server.HtmlDecode(dT.Rows[i]["MOAConsumption"].ToString());
                                worksheet.Cell(i + 8, 36).Value = Server.HtmlDecode(dT.Rows[i]["MOAamt"].ToString());
                                //27
                                worksheet.Cell(i + 8, 37).Value = Server.HtmlDecode(dT.Rows[i]["MARConsumption"].ToString());
                                worksheet.Cell(i + 8, 38).Value = Server.HtmlDecode(dT.Rows[i]["MARamt"].ToString());


                                worksheet.Cell(i + 8, 1).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                                worksheet.Cell(i + 8, 2).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                                worksheet.Cell(i + 8, 3).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                                worksheet.Cell(i + 8, 4).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                                worksheet.Cell(i + 8, 5).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                                worksheet.Cell(i + 8, 6).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                                worksheet.Cell(i + 8, 7).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                                worksheet.Cell(i + 8, 8).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                                worksheet.Cell(i + 8, 9).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                                worksheet.Cell(i + 8, 10).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                                worksheet.Cell(i + 8, 11).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                                worksheet.Cell(i + 8, 12).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                                worksheet.Cell(i + 8, 13).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                                worksheet.Cell(i + 8, 14).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                                worksheet.Cell(i + 8, 15).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                                worksheet.Cell(i + 8, 16).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                                worksheet.Cell(i + 8, 17).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                                worksheet.Cell(i + 8, 18).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                                worksheet.Cell(i + 8, 19).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                                worksheet.Cell(i + 8, 20).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;

                                worksheet.Cell(i + 8, 21).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                                worksheet.Cell(i + 8, 22).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                                worksheet.Cell(i + 8, 23).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                                worksheet.Cell(i + 8, 24).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                                worksheet.Cell(i + 8, 25).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                                worksheet.Cell(i + 8, 26).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                                worksheet.Cell(i + 8, 27).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                                worksheet.Cell(i + 8, 28).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                                worksheet.Cell(i + 8, 29).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                                worksheet.Cell(i + 8, 30).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;

                                worksheet.Cell(i + 8, 31).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                                worksheet.Cell(i + 8, 32).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                                worksheet.Cell(i + 8, 33).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                                worksheet.Cell(i + 8, 34).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                                worksheet.Cell(i + 8, 35).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                                worksheet.Cell(i + 8, 36).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                                worksheet.Cell(i + 8, 37).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                                worksheet.Cell(i + 8, 38).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;

                                worksheet.Cell(i + 8, 1).Style.Font.FontSize = 9;
                                worksheet.Cell(i + 8, 2).Style.Font.FontSize = 9;
                                worksheet.Cell(i + 8, 3).Style.Font.FontSize = 9;
                                worksheet.Cell(i + 8, 4).Style.Font.FontSize = 9;
                                worksheet.Cell(i + 8, 5).Style.Font.FontSize = 9;
                                worksheet.Cell(i + 8, 6).Style.Font.FontSize = 9;
                                worksheet.Cell(i + 8, 7).Style.Font.FontSize = 9;
                                worksheet.Cell(i + 8, 8).Style.Font.FontSize = 9;
                                worksheet.Cell(i + 8, 9).Style.Font.FontSize = 9;
                                worksheet.Cell(i + 8, 10).Style.Font.FontSize = 9;
                                worksheet.Cell(i + 8, 11).Style.Font.FontSize = 9;

                                worksheet.Cell(i + 8, 12).Style.Font.FontSize = 9;
                                worksheet.Cell(i + 8, 13).Style.Font.FontSize = 9;
                                worksheet.Cell(i + 8, 14).Style.Font.FontSize = 9;
                                worksheet.Cell(i + 8, 15).Style.Font.FontSize = 9;
                                worksheet.Cell(i + 8, 16).Style.Font.FontSize = 9;
                                worksheet.Cell(i + 8, 17).Style.Font.FontSize = 9;
                                worksheet.Cell(i + 8, 18).Style.Font.FontSize = 9;
                                worksheet.Cell(i + 8, 19).Style.Font.FontSize = 9;
                                worksheet.Cell(i + 8, 20).Style.Font.FontSize = 9;

                                worksheet.Cell(i + 8, 21).Style.Font.FontSize = 9;
                                worksheet.Cell(i + 8, 22).Style.Font.FontSize = 9;
                                worksheet.Cell(i + 8, 23).Style.Font.FontSize = 9;
                                worksheet.Cell(i + 8, 24).Style.Font.FontSize = 9;
                                worksheet.Cell(i + 8, 25).Style.Font.FontSize = 9;
                                worksheet.Cell(i + 8, 26).Style.Font.FontSize = 9;
                                worksheet.Cell(i + 8, 27).Style.Font.FontSize = 9;
                                worksheet.Cell(i + 8, 28).Style.Font.FontSize = 9;
                                worksheet.Cell(i + 8, 29).Style.Font.FontSize = 9;
                                worksheet.Cell(i + 8, 30).Style.Font.FontSize = 9;

                                worksheet.Cell(i + 8, 31).Style.Font.FontSize = 9;
                                worksheet.Cell(i + 8, 32).Style.Font.FontSize = 9;
                                worksheet.Cell(i + 8, 33).Style.Font.FontSize = 9;
                                worksheet.Cell(i + 8, 34).Style.Font.FontSize = 9;
                                worksheet.Cell(i + 8, 35).Style.Font.FontSize = 9;
                                worksheet.Cell(i + 8, 36).Style.Font.FontSize = 9;
                                worksheet.Cell(i + 8, 37).Style.Font.FontSize = 9;
                                worksheet.Cell(i + 8, 38).Style.Font.FontSize = 9;


                            }
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
            }
        }

        protected void btnGen_Click(object sender, EventArgs e)
        {
            using (SqlConnection sqlConn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["MyConnCSI"].ConnectionString))
            {
                string stR = @"dbo.reportConsumption";
                using (SqlCommand cmD = new SqlCommand(stR, sqlConn))
                {
                    sqlConn.Open();
                    cmD.CommandTimeout = 0;
                    cmD.CommandType = CommandType.StoredProcedure;
                    cmD.Parameters.AddWithValue("@Category", ddCategory.SelectedValue);
                    cmD.Parameters.AddWithValue("@Year", ddYear.SelectedItem.Text);
                    cmD.Parameters.AddWithValue("@Month",ddMonth.SelectedValue);
                


                    DataTable dT = new DataTable();
                    SqlDataAdapter dA = new SqlDataAdapter(cmD);
                    dA.Fill(dT);

                    gvConsumption.DataSource = dT;
                    gvConsumption.DataBind();

                }
            }
        }

        protected void gvConsumption_PreRender(object sender, EventArgs e)
        {
            if (gvConsumption.Rows.Count > 0)
            {
                gvConsumption.UseAccessibleHeader = true;
                gvConsumption.HeaderRow.TableSection = TableRowSection.TableHeader;
                gvConsumption.FooterRow.TableSection = TableRowSection.TableFooter;
            }
        }



    }
}