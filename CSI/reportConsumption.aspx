<%@ Page Title="Report Consumption" Language="C#" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true" CodeBehind="reportConsumption.aspx.cs" Inherits="CSI.reportConsumption" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="plugins/jquery-ui/jquery-ui.css" rel="stylesheet" />
    <style type="text/css">

        div.dataTables_wrapper {
        /*width: 800px;*/
        margin: 0 auto;
    }


        .modal {
            position: fixed;
            top: 0;
            left: 0;
            background-color: black;
            z-index: 999;
            opacity: 0.8;
            filter: alpha(opacity=80);
            -moz-opacity: 0.8;
            min-height: 100%;
            width: 100%;
        }

        .loading {
            display: none;
            position: fixed;
            z-index: 999;
            height: 100%;
            width: 100%;
            top: 50px;
            left: 0;
            background-color: Black;
            filter: alpha(opacity=60);
            opacity: 0.6;
            -moz-opacity: 0.8;
        }

        .loader {
            border: 16px solid #f3f3f3;
            border-radius: 50%;
            border-top: 16px solid #3498db;
            width: 120px;
            height: 120px;
            -webkit-animation: spin 2s linear infinite; /* Safari */
            animation: spin 2s linear infinite;
        }

        /* Safari */
        @-webkit-keyframes spin {
            0% {
                -webkit-transform: rotate(0deg);
            }

            100% {
                -webkit-transform: rotate(360deg);
            }
        }

        @keyframes spin {
            0% {
                transform: rotate(0deg);
            }

            100% {
                transform: rotate(360deg);
            }
        }
    </style>


    <style type="text/css">
        table th {
            text-align: center;
            vertical-align: middle;
            background-color: #f2f2f2;
            font-size: 14px;
        }

        table tr {
            vertical-align: middle;
            font-size: 12px;
            text-align: center;
        }

        .hiddencol {
            display: none;
        }

        /*.table-hover tbody tr:hover td {
            background: aqua;
        }*/

        .table-hover thead tr:hover th, .table-hover tbody tr:hover td {
            background-color: #D1D119;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
    <section class="content-header">
        <div class="container-fluid">
            <div class="row mb-1">
                <div class="col-sm-6">
                    <h5>Monthly Consumption Report</h5>
                </div>
                <div class="col-sm-6">
                    <ol class="breadcrumb float-sm-right">
                    </ol>
                </div>
            </div>
        </div>
    </section>

    <section class="content">
        <div class="container-fluid">
            <div class="row">
                <div class="col-12">

                    <div class="card">
                        <div class="card-header">

                            <div style="margin-bottom: 5px" class="input-group">
                                 <div class="col-md-1 col-1 text-left">
                                               Category :
                                </div>
                                <div class="col-sm-3">
                                    <div class="input-group">
                                          <asp:DropDownList ID="ddCategory" runat="server" class="form-control" Style="width: 135px;  text-transform:uppercase;" >
                                            </asp:DropDownList> 
                                        &nbsp;&nbsp;
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" InitialValue="0" 
                                        ControlToValidate="ddCategory"  Display="Dynamic" ValidationGroup="grpCon" ForeColor="Red"
                                        ErrorMessage="Please select"></asp:RequiredFieldValidator>
                                    </div>
                                </div>
                            </div>

                            <div style="margin-bottom: 5px" class="input-group">
                                <div class="col-md-1 col-1 text-left">
                                    Year :
                                </div>
                                <div class="col-sm-1">
                                    <div class="input-group">
                                        <asp:DropDownList ID="ddYear" runat="server" class="form-control" Style="width: 135px;">
                                        </asp:DropDownList>

                                    </div>
                                </div>
                            </div>



                            <div style="margin-bottom: 15px" class="input-group">
                                <div class="col-md-1 col-1 text-left">
                                    Month :
                                </div>
                                <div class="col-sm-3">
                                    <div class="input-group">
                                        <asp:DropDownList ID="ddMonth" runat="server" class="form-control" Style="width: 135px; text-transform: uppercase;">
                                                <asp:ListItem Value="0" Text="Select Month" Selected="True"></asp:ListItem>
                                                <asp:ListItem Value="1" Text="January"></asp:ListItem>
                                                <asp:ListItem Value="2" Text="February"></asp:ListItem>
                                                <asp:ListItem Value="3" Text="March"></asp:ListItem>
                                                <asp:ListItem Value="4" Text="April"></asp:ListItem>
                                                <asp:ListItem Value="5" Text="May"></asp:ListItem>
                                                <asp:ListItem Value="6" Text="June"></asp:ListItem>
                                                <asp:ListItem Value="7" Text="July"></asp:ListItem>
                                                <asp:ListItem Value="8" Text="August"></asp:ListItem>
                                                <asp:ListItem Value="9" Text="September"></asp:ListItem>
                                                <asp:ListItem Value="10" Text="October"></asp:ListItem>
                                                <asp:ListItem Value="11" Text="November"></asp:ListItem>
                                                <asp:ListItem Value="12" Text="December"></asp:ListItem>
                                        </asp:DropDownList>
                                        &nbsp;&nbsp;
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator113" runat="server" InitialValue="0"
                                            ControlToValidate="ddMonth" Display="Dynamic" ValidationGroup="grpCon" ForeColor="Red"
                                            ErrorMessage="Please select"></asp:RequiredFieldValidator>
                                    </div>
                                </div>
                            </div>

                            
                             <div style="margin-bottom: 5px" class="input-group">
                                 <div class="col-md-1 col-1 text-left">
                                               
                                </div>
                                <div class="col-sm-6">
                                    <div class="input-group">
                                        <asp:Button ID="btnGen" runat="server" Text="Generate" ValidationGroup="grpCon"   CssClass="btn btn-sm btn-info" OnClick="btnGen_Click"/>
                                        <asp:Button ID="btnGenerate" Visible="false" runat="server" Text="Generate" ValidationGroup="grpCon"   CssClass="btn btn-sm btn-info" OnClick="btnGenerate_Click"/>   
                                    </div>
                                </div>
                            </div>
                        </div>
                        <%--OnClientClick="if(Page_ClientValidate()) ShowProgress()"--%>
                        <div class="card-body">
                            <div class="col-sm-12">
                                    <asp:GridView ID="gvConsumption" ClientIDMode="Static" class="table table-bordered table-hover" style="width:100%" EmptyDataText="No Record Found" AutoGenerateColumns="false" runat="server" OnPreRender="gvConsumption_PreRender">
                                            <Columns>
                                                <asp:BoundField DataField="sup_ItemCode" HeaderText="Item Code" ItemStyle-Width="5%" HeaderStyle-HorizontalAlign="Center">
                                                    <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                                </asp:BoundField>
                                                <asp:BoundField DataField="sup_DESCRIPTION" HeaderText="Item Description" ItemStyle-Width="20%" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                                    <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                                    <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                                </asp:BoundField>
                                                <asp:BoundField DataField="UNIT COST" HeaderText="UNIT COST" DataFormatString="{0:#,##0.##;(#,##0.##);0}" ItemStyle-Width="5%" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Right">
                                                    <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                                    <ItemStyle HorizontalAlign="Right"></ItemStyle>
                                                </asp:BoundField>

                                                <asp:BoundField DataField="NET OF VAT" HeaderText="NET OF VAT" DataFormatString="{0:#,##0.##;(#,##0.##);0}" ItemStyle-Width="10%" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Right">
                                                    <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                                    <ItemStyle HorizontalAlign="Right"></ItemStyle>
                                                </asp:BoundField>

                                                <%--branch--%>
                                                <asp:BoundField DataField="HOConsumption" HeaderText="HO<br />(Qty)" HtmlEncode="false" ItemStyle-Width="5%" HeaderStyle-HorizontalAlign="Center">
                                                    <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                                </asp:BoundField>

                                                <asp:BoundField DataField="HOamt" HeaderText="HO<br />(Amt)"  HtmlEncode="false" DataFormatString="{0:#,##0.##;(#,##0.##);0}" ItemStyle-Width="8%" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Right">
                                                    <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                                    <ItemStyle HorizontalAlign="Right"></ItemStyle>
                                                </asp:BoundField>

                                                <%--branch--%>
                                                <asp:BoundField DataField="SMCConsumption" HeaderText="North EDSA<br />(Qty)" HtmlEncode="false" DataFormatString="{0:N0}" ItemStyle-Width="5%" HeaderStyle-HorizontalAlign="Center">
                                                    <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                                </asp:BoundField>

                                                <asp:BoundField DataField="SMCamt" HeaderText="North EDSA<br />(Amt)" HtmlEncode="false" DataFormatString="{0:#,##0.##;(#,##0.##);0}" ItemStyle-Width="8%" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Right">
                                                    <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                                    <ItemStyle HorizontalAlign="Right"></ItemStyle>
                                                </asp:BoundField>
                                                
                                                <%--branch--%>
                                                <asp:BoundField DataField="SCPConsumption" HeaderText="Sta Mesa<br />(Qty)" HtmlEncode="false" DataFormatString="{0:N0}" ItemStyle-Width="5%" HeaderStyle-HorizontalAlign="Center">
                                                    <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                                </asp:BoundField>

                                                <asp:BoundField DataField="SCPamt" HeaderText="Sta Mesa<br />(Amt)" HtmlEncode="false" DataFormatString="{0:#,##0.##;(#,##0.##);0}" ItemStyle-Width="8%" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Right">
                                                    <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                                    <ItemStyle HorizontalAlign="Right"></ItemStyle>
                                                </asp:BoundField>

                                                <%--branch--%>
                                                <asp:BoundField DataField="MMLConsumption" HeaderText="Megamall<br />(Qty)" HtmlEncode="false" DataFormatString="{0:N0}" ItemStyle-Width="5%" HeaderStyle-HorizontalAlign="Center">
                                                    <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                                </asp:BoundField>

                                                <asp:BoundField DataField="MMLamt" HeaderText="Megamall<br />(Amt)" HtmlEncode="false" DataFormatString="{0:#,##0.##;(#,##0.##);0}" ItemStyle-Width="8%" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Right">
                                                    <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                                    <ItemStyle HorizontalAlign="Right"></ItemStyle>
                                                </asp:BoundField>

                                                <%--branch--%>
                                                <asp:BoundField DataField="SCBConsumption" HeaderText="Cebu<br />(Qty)" HtmlEncode="false" DataFormatString="{0:N0}" ItemStyle-Width="5%" HeaderStyle-HorizontalAlign="Center">
                                                    <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                                </asp:BoundField>

                                                <asp:BoundField DataField="SCBamt" HeaderText="Cebu<br />(Amt)" HtmlEncode="false" DataFormatString="{0:#,##0.##;(#,##0.##);0}" ItemStyle-Width="8%" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Right">
                                                    <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                                    <ItemStyle HorizontalAlign="Right"></ItemStyle>
                                                </asp:BoundField>

                                                <%--branch--%>
                                                   <%--branch--%>
                                           <asp:BoundField DataField="AFMConsumption" HeaderText="Festival Mall<br />(Qty)" HtmlEncode="false" DataFormatString="{0:N0}" ItemStyle-Width="5%" HeaderStyle-HorizontalAlign="Center">
                                                    <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                                </asp:BoundField>

                                                <asp:BoundField DataField="AFMamt" HeaderText="Festival Mall<br />(Amt)" HtmlEncode="false" DataFormatString="{0:#,##0.##;(#,##0.##);0}" ItemStyle-Width="8%" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Right">
                                                    <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                                    <ItemStyle HorizontalAlign="Right"></ItemStyle>
                                                </asp:BoundField>

                                                <%--branch--%>
                                                <asp:BoundField DataField="SMSConsumption" HeaderText="Southmall<br />(Qty)" HtmlEncode="false" DataFormatString="{0:N0}" ItemStyle-Width="5%" HeaderStyle-HorizontalAlign="Center">
                                                    <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                                </asp:BoundField>

                                                <asp:BoundField DataField="SMSamt" HeaderText="Southmall<br />(Amt)" HtmlEncode="false" DataFormatString="{0:#,##0.##;(#,##0.##);0}" ItemStyle-Width="8%" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Right">
                                                    <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                                    <ItemStyle HorizontalAlign="Right"></ItemStyle>
                                                </asp:BoundField>

                                                <%--branch--%>
                                                <asp:BoundField DataField="SMBConsumption" HeaderText="Bacoor<br />(Qty)" HtmlEncode="false" DataFormatString="{0:N0}" ItemStyle-Width="5%" HeaderStyle-HorizontalAlign="Center">
                                                    <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                                </asp:BoundField>

                                                <asp:BoundField DataField="SMBamt" HeaderText="Bacoor<br />(Amt)" HtmlEncode="false" DataFormatString="{0:#,##0.##;(#,##0.##);0}" ItemStyle-Width="8%" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Right">
                                                    <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                                    <ItemStyle HorizontalAlign="Right"></ItemStyle>
                                                </asp:BoundField>
                                                <%--branch--%>
                                                <asp:BoundField DataField="FVWConsumption" HeaderText="Fairview<br />(Qty)" HtmlEncode="false" DataFormatString="{0:N0}" ItemStyle-Width="5%" HeaderStyle-HorizontalAlign="Center">
                                                    <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                                </asp:BoundField>

                                                <asp:BoundField DataField="FVWamt" HeaderText="Fairview<br />(Amt)" HtmlEncode="false" DataFormatString="{0:#,##0.##;(#,##0.##);0}" ItemStyle-Width="8%" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Right">
                                                    <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                                    <ItemStyle HorizontalAlign="Right"></ItemStyle>
                                                </asp:BoundField>
                                                <%--branch--%>
                                                <asp:BoundField DataField="GSAConsumption" HeaderText="The Link<br />(Qty)" HtmlEncode="false" DataFormatString="{0:N0}" ItemStyle-Width="5%" HeaderStyle-HorizontalAlign="Center">
                                                    <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                                </asp:BoundField>

                                                <asp:BoundField DataField="GSAamt" HeaderText="The Link<br />(Amt)" HtmlEncode="false" DataFormatString="{0:#,##0.##;(#,##0.##);0}" ItemStyle-Width="8%" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Right">
                                                    <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                                    <ItemStyle HorizontalAlign="Right"></ItemStyle>
                                                </asp:BoundField>

                                                <%--branch--%>
                                                <asp:BoundField DataField="SMMConsumption" HeaderText="Manila<br />(Qty)" HtmlEncode="false" DataFormatString="{0:N0}" ItemStyle-Width="5%" HeaderStyle-HorizontalAlign="Center">
                                                    <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                                </asp:BoundField>

                                                <asp:BoundField DataField="SMMamt" HeaderText="Manila<br />(Amt)" HtmlEncode="false" DataFormatString="{0:#,##0.##;(#,##0.##);0}" ItemStyle-Width="8%" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Right">
                                                    <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                                    <ItemStyle HorizontalAlign="Right"></ItemStyle>
                                                </asp:BoundField>

                                                <%--branch--%>
                                                <asp:BoundField DataField="SMPConsumption" HeaderText="Pampanga<br />(Qty)" HtmlEncode="false" DataFormatString="{0:N0}" ItemStyle-Width="5%" HeaderStyle-HorizontalAlign="Center">
                                                    <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                                </asp:BoundField>

                                                <asp:BoundField DataField="SMPamt" HeaderText="Pampanga<br />(Amt)" HtmlEncode="false" DataFormatString="{0:#,##0.##;(#,##0.##);0}" ItemStyle-Width="8%" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Right">
                                                    <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                                    <ItemStyle HorizontalAlign="Right"></ItemStyle>
                                                </asp:BoundField>

                                                <%--branch--%>
                                                <asp:BoundField DataField="SCTConsumption" HeaderText="Sucat<br />(Qty)" HtmlEncode="false" DataFormatString="{0:N0}" ItemStyle-Width="5%" HeaderStyle-HorizontalAlign="Center">
                                                    <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                                </asp:BoundField>

                                                <asp:BoundField DataField="SCTamt" HeaderText="Sucat<br />(Amt)" HtmlEncode="false" DataFormatString="{0:#,##0.##;(#,##0.##);0}" ItemStyle-Width="8%" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Right">
                                                    <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                                    <ItemStyle HorizontalAlign="Right"></ItemStyle>
                                                </asp:BoundField>

                                                <%--branch--%>
                                                <asp:BoundField DataField="BCTConsumption" HeaderText="Bicutan<br />(Qty)" HtmlEncode="false" DataFormatString="{0:N0}" ItemStyle-Width="5%" HeaderStyle-HorizontalAlign="Center">
                                                    <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                                </asp:BoundField>

                                                <asp:BoundField DataField="BCTamt" HeaderText="Bicutan<br />(Amt)" HtmlEncode="false" DataFormatString="{0:#,##0.##;(#,##0.##);0}" ItemStyle-Width="8%" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Right">
                                                    <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                                    <ItemStyle HorizontalAlign="Right"></ItemStyle>
                                                </asp:BoundField>

                                                <%--branch--%>
                                                <asp:BoundField DataField="DASConsumption" HeaderText="Dasma<br />(Qty)" HtmlEncode="false" DataFormatString="{0:N0}" ItemStyle-Width="5%" HeaderStyle-HorizontalAlign="Center">
                                                    <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                                </asp:BoundField>

                                                <asp:BoundField DataField="DASamt" HeaderText="Dasma<br />(Amt)" HtmlEncode="false" DataFormatString="{0:#,##0.##;(#,##0.##);0}" ItemStyle-Width="8%" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Right">
                                                    <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                                    <ItemStyle HorizontalAlign="Right"></ItemStyle>
                                                </asp:BoundField>

                                                <%--branch--%>
                                                <asp:BoundField DataField="MOAConsumption" HeaderText="MOA<br />(Qty)" HtmlEncode="false" DataFormatString="{0:N0}" ItemStyle-Width="5%" HeaderStyle-HorizontalAlign="Center">
                                                    <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                                </asp:BoundField>

                                                <asp:BoundField DataField="MOAamt" HeaderText="MOA<br />(Amt)" HtmlEncode="false" DataFormatString="{0:#,##0.##;(#,##0.##);0}" ItemStyle-Width="8%" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Right">
                                                    <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                                    <ItemStyle HorizontalAlign="Right"></ItemStyle>
                                                </asp:BoundField>

                                                <%--branch--%>
                                                <asp:BoundField DataField="MARConsumption" HeaderText="Marilao<br />(Qty)" HtmlEncode="false" DataFormatString="{0:N0}" ItemStyle-Width="5%" HeaderStyle-HorizontalAlign="Center">
                                                    <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                                </asp:BoundField>

                                                <asp:BoundField DataField="MARamt" HeaderText="Marilao<br />(Amt)" HtmlEncode="false" DataFormatString="{0:#,##0.##;(#,##0.##);0}" ItemStyle-Width="8%" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Right">
                                                    <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                                    <ItemStyle HorizontalAlign="Right"></ItemStyle>
                                                </asp:BoundField>

                                         

                                            </Columns>
                                        </asp:GridView>
                            </div>
                        </div>

                    </div>
                </div>
            </div>
        </div>
    </section>


    <div class="loading" align="center" style="margin-top:100px;">
                        <br />
                        <br />
                         <div class="loader"></div>
                         <br />
                         <asp:Label ID="Label2" runat="server" Text="Please wait" ForeColor="Red"></asp:Label>
                    </div>

    <!-- jQuery -->
    <script src="plugins/jquery/jquery.min.js"></script>
    <!-- Bootstrap 4 -->
    <script src="plugins/bootstrap/js/bootstrap.bundle.min.js"></script>
<!-- ################################################# END #################################################### -->

    <script>
        $(function () {
            $("#<%= gvConsumption.ClientID%>").DataTable({
                "lengthChange": true,
                "searching": true,
                
                "scrollY": 400,
                "scrollX": true,
                "ordering": false,
                "aLengthMenu": [[-1], ["All"]],
                "iDisplayLength": -1,
                "bSortable": false,
                "aTargets": [0],
                "buttons": ["excel", "print", "colvis"],
                "info": true,
                //"autoWidth": false,
                "responsive": true,
            }).buttons().container().appendTo('#gvConsumption_wrapper .col-md-6:eq(0)');;
        });
    </script>


    <script type="text/javascript">
        function ShowSuccessMsg() {
            $(function () {
                $("#messageDiv").dialog({
                    title: "Consumption",
                    width: '335px',
                    buttons: {
                        Close: function () {
                            $(this).dialog('close');
                        }
                    },
                    modal: true
                });
            });
        }
    </script>

    <div id="messageDiv" style="display: none;">
        <asp:UpdatePanel ID="UpdatePanel3" runat="server">
            <ContentTemplate>
                <asp:Label Text="" ID="lblMsg" runat="server" />
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
    <!-- ################################################# END #################################################### -->

    <!-- ################################################# END #################################################### -->
    <script src="images/ForAjaxLoader.js"></script>


 <script type="text/javascript">
     function ShowProgress() {
         setTimeout(function () {
             var modal = $('<div />');
             modal.addClass("modal");
             $('body').append(modal);
             var loading = $(".loading");
             loading.show();
             var top = Math.max($(window).height() / 2 - loading[0].offsetHeight / 2, 0);
             var left = Math.max($(window).width() / 2 - loading[0].offsetWidth / 2, 0);
             loading.css({ top: top, left: left });
         }, 200);
     }

</script>
</asp:Content>
