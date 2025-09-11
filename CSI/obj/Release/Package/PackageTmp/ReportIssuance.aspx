<%@ Page Title="Report - Issuance" Language="C#" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true" CodeBehind="ReportIssuance.aspx.cs" Inherits="CSI.ReportIssuance" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
<link href="plugins/jquery-ui/jquery-ui.css" rel="stylesheet" />
<style type="text/css">
    .modal
    {
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
    .loading
    {
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
          0% { -webkit-transform: rotate(0deg); }
          100% { -webkit-transform: rotate(360deg); }
        }

        @keyframes spin {
          0% { transform: rotate(0deg); }
          100% { transform: rotate(360deg); }
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
                    <h5>Issuance Report</h5>
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
                                               Branch :
                                </div>
                                <div class="col-sm-3">
                                    <div class="input-group">
                                        <asp:DropDownList ID="ddBranch" runat="server" class="form-control" Style="width: 135px;  text-transform:uppercase;" >
                                              
                                        </asp:DropDownList>
                                                    
                                    </div>
                                </div>
                            </div>

                            <div style="margin-bottom: 5px" class="input-group">
                            <div class="col-md-1 col-1 text-left">
                                <asp:Label ID="Label1" runat="server" Text="Date From" Style="width: 125px;"></asp:Label>
                            </div>
                            <div class="col-sm-2 col-2">
                                <asp:TextBox ID="txtDateFrom" runat="server" CssClass="form-control txtDate" Width="105px" AutoCompleteType="Disabled"></asp:TextBox>

                            </div>
                            <div class="col-sm-4 col-4">
                                <div class="input-group">
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" Display="Dynamic"
                                        ControlToValidate="txtDateFrom" ForeColor="Red" ValidationGroup="grpIssuance"
                                        ErrorMessage="Required Field"></asp:RequiredFieldValidator>
                                    <asp:RegularExpressionValidator ID="REVdate" runat="server" ControlToValidate="txtDateFrom"
                                        ValidationExpression="^([1-9]|0[1-9]|1[0-2])[- / .]([1-9]|0[1-9]|1[0-9]|2[0-9]|3[0-1])[- / .](1[9][0-9][0-9]|2[0][0-9][0-9])$"
                                        ForeColor="Red" Display="Dynamic"
                                        ErrorMessage="Invalid date format"
                                        ValidationGroup="grpIssuance"></asp:RegularExpressionValidator>
                                    <asp:CompareValidator ID="CompareValidator1" runat="server" Style="color: Red;"
                                        ValidationGroup="grpIssuance" Display="Dynamic" ControlToCompare="txtDateFrom"
                                        ControlToValidate="txtDateTo" Type="Date" Operator="GreaterThanEqual"
                                        ErrorMessage="Invalid date range"></asp:CompareValidator>
                                </div>
                            </div>
                        </div>

                        <div style="margin-bottom: 5px" class="input-group">
                            <div class="col-md-1 col-1 text-left">
                                <asp:Label ID="Label2" runat="server" Text="Date To" Style="width: 125px;"></asp:Label>
                            </div>
                            <div class="col-sm-2 col-2">
                                <asp:TextBox ID="txtDateTo" runat="server" CssClass="form-control txtDate" Width="105px" AutoCompleteType="Disabled"></asp:TextBox>
                            </div>
                            <div class="col-sm-4 col-4">
                                <div class="input-group">
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtDateTo" ForeColor="Red" Display="Dynamic"
                                        ValidationGroup="grpIssuance" ErrorMessage="Required Field"></asp:RequiredFieldValidator>
                                    <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ControlToValidate="txtDateTo"
                                        ValidationExpression="^([1-9]|0[1-9]|1[0-2])[- / .]([1-9]|0[1-9]|1[0-9]|2[0-9]|3[0-1])[- / .](1[9][0-9][0-9]|2[0][0-9][0-9])$"
                                        ForeColor="Red" Display="Dynamic"
                                        ErrorMessage="Invalid date format"
                                        ValidationGroup="grpIssuance"></asp:RegularExpressionValidator>
                                </div>
                            </div>
                        </div>

                        <div style="margin-bottom: 5px" class="input-group">
                            <div class="col-md-1 col-1 text-left">
                            </div>
                            <div class="col-sm-2 col-2">
                                <asp:Button ID="btnGenerate" runat="server" Text="GENERATE" CssClass="btn btn-sm btn-info" ValidationGroup="grpIssuance" OnClick="btnGenerate_Click"  />
                            </div>
                            <div class="col-sm-4 col-4">
                                <div class="input-group">
                                </div>
                            </div>
                        </div>

                    </div>

                     <div class="card-body">
                             <asp:GridView ID="gvIssuance" runat="server" ClientIDMode="Static" class="table table-bordered table-hover" AutoGenerateColumns="false" OnPreRender="gvIssuance_PreRender" >
                        <Columns>
                            
                            <asp:BoundField DataField="BrName" HeaderText="Branch/Dept." ItemStyle-Width="10%" HeaderStyle-HorizontalAlign="Center">
                                <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                            </asp:BoundField>
                            
                            <asp:BoundField DataField="Sup_ControlNo" HeaderText="Issuance No." ItemStyle-Width="10%" HeaderStyle-HorizontalAlign="Center">
                                <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                            </asp:BoundField>

                            

                            <asp:BoundField DataField="DRNumber" HeaderText="Issue <br />Slip No." HtmlEncode="false" ItemStyle-Width="5%" HeaderStyle-HorizontalAlign="Center">
                                <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                            </asp:BoundField>

                            <asp:BoundField DataField="Sup_CategoryName" HeaderText="Category" ItemStyle-Width="5%" HeaderStyle-HorizontalAlign="Center">
                                <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                            </asp:BoundField>

                            <asp:BoundField DataField="sup_ItemCode" HeaderText="Item Code" ItemStyle-Width="10%" HeaderStyle-HorizontalAlign="Center">
                                <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                            </asp:BoundField>
                            <asp:BoundField DataField="sup_DESCRIPTION" HeaderText="Item Description" ItemStyle-Width="20%" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                <ItemStyle HorizontalAlign="Center"></ItemStyle>
                            </asp:BoundField>

                            <asp:BoundField DataField="vQtyPicked" HeaderText="Quantity" DataFormatString="{0:#,##0.##;(#,##0.##);0}" ItemStyle-Width="5%" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                <ItemStyle HorizontalAlign="Center"></ItemStyle>
                            </asp:BoundField>

                              <asp:BoundField DataField="SRP" HeaderText="SRP" DataFormatString="{0:#,##0.##;(#,##0.##);0}" ItemStyle-Width="5%" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                <ItemStyle HorizontalAlign="Right"></ItemStyle>
                            </asp:BoundField>

                            <asp:BoundField DataField="Cost" HeaderText="Cost" DataFormatString="{0:#,##0.##;(#,##0.##);0}" ItemStyle-Width="5%" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                <ItemStyle HorizontalAlign="Right"></ItemStyle>
                            </asp:BoundField>

                             <asp:BoundField DataField="vBatchNo" HeaderText="Batch No" ItemStyle-Width="5%" HeaderStyle-HorizontalAlign="Center">
                                <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                            </asp:BoundField>

                            <asp:TemplateField HeaderText="Date Expiry">
                                            <HeaderStyle HorizontalAlign="Center" Width="5%" />
                                            <ItemStyle HorizontalAlign="Center" />
                                            <ItemTemplate>
                                                       <asp:Label ID="lblvDateExpiry" runat="server" Text='<%#  Convert.ToString(Eval("vDateExpiry", "{0:MM/dd/yyyy}")).Equals("01/01/1900")?"":Eval("vDateExpiry", "{0:MM/dd/yyyy}")%>'></asp:Label>
                                             </ItemTemplate>
                                        </asp:TemplateField>

      
                            <asp:BoundField DataField="DateDelivered" HeaderText="Date Delivered" DataFormatString="{0:MM/dd/yyyy}" ItemStyle-Width="10%" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                <ItemStyle HorizontalAlign="Center"></ItemStyle>
                            </asp:BoundField>
      
                            <asp:BoundField DataField="vPickedBy" HeaderText="Posted By" ItemStyle-Width="10%" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                <ItemStyle HorizontalAlign="Center"></ItemStyle>
                            </asp:BoundField>

                            <asp:BoundField DataField="DatePostedDelivered" HeaderText="Date Posted" DataFormatString="{0:MM/dd/yyyy}" ItemStyle-Width="10%" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                <ItemStyle HorizontalAlign="Center"></ItemStyle>
                            </asp:BoundField>

                            
      
                            
                         </Columns>
                       </asp:GridView>
                     </div>
                </div>
            </div>
        </div>
    </div>
</section>

   <!-- ################################################# END #################################################### -->
 <div class="loading" align="center" style="margin-top:100px;">
                        <br />
                        <br />
                         <div class="loader"></div>
                         <br />
                         <asp:Label ID="Label3" runat="server" Text="Please wait" ForeColor="Red"></asp:Label>
                    </div>

    <!-- jQuery -->
    <script src="plugins/jquery/jquery.min.js"></script>
    <!-- Bootstrap 4 -->
    <script src="plugins/bootstrap/js/bootstrap.bundle.min.js"></script>

    <script>
        $(function () {
            $("#<%= gvIssuance.ClientID%>").DataTable({
                "paging": true,
                "lengthChange": true,
                "searching": true,
                "ordering": false,
                "aLengthMenu": [[10, 25, 50, -1], [10, 25, 50, "All"]],
                "iDisplayLength": 10,
                "bSortable": false,
                "aTargets": [0],
                "buttons": ["excel", "pdf", "print", "colvis"],
                "info": true,
                "autoWidth": false,
                "responsive": true,
            }).buttons().container().appendTo('#gvIssuance_wrapper .col-md-6:eq(0)');;
        });
    </script>

    <script type="text/javascript">
        function ShowSuccessMsg() {
            $(function () {
                $("#messageDiv").dialog({
                    title: "Issuance",
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
    <script type="text/javascript">
        $(document).ready(function () {
            $('.txtDate').datepicker({
                //dateFormat: "MM/dd/yyyy",
                maxDate: new Date,
                minDate: new Date(2007, 6, 12),
                changeMonth: true,
                changeYear: true,
                yearRange: "-1:+0"
            });

        });
    </script>


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


