<%@ Page Title="CSI System | Stock Card" Language="C#" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true" CodeBehind="reportStockCard.aspx.cs" Inherits="CSI.reportStockCard" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="plugins/jquery-ui/jquery-ui.css" rel="stylesheet" />
      <link rel="stylesheet" href="docsupport/prism.css" />
    <link rel="stylesheet" href="chosen.css" />

    <style type="text/css">
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
                    <h5>Stock Card</h5>
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
                                        <asp:DropDownList ID="ddBranch" runat="server" class="form-control" Style="width: 135px; text-transform: uppercase;">
                                        </asp:DropDownList>
                                

                                    </div>
                                </div>
                            </div>



                            <div style="margin-bottom: 5px" class="input-group">
                                <div class="col-md-1 col-1 text-left">
                                    Category :
                                </div>
                                <div class="col-sm-3">
                                    <div class="input-group">
                                        <asp:DropDownList ID="ddCategory" runat="server" class="form-control" Style="width: 135px; text-transform: uppercase;" AutoPostBack="True" OnSelectedIndexChanged="ddCategory_SelectedIndexChanged">
                                        </asp:DropDownList>
                                        &nbsp;&nbsp;
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator113" runat="server" InitialValue="0"
                                            ControlToValidate="ddCategory" Display="Dynamic" ValidationGroup="grpInv" ForeColor="Red"
                                            ErrorMessage="Please select"></asp:RequiredFieldValidator>
                                    </div>
                                </div>
                            </div>

                            <div style="margin-bottom: 10px" class="input-group">
                                <div class="col-md-1 col-1 text-left">
                                    Item Name :
                                </div>
                                <div class="col-sm-5">
                                    <div class="input-group">
                                        <asp:DropDownList ID="ddItem" runat="server" class="form-control chosen-select" AutoPostBack="True" OnSelectedIndexChanged="ddItem_SelectedIndexChanged">
                                        </asp:DropDownList>
                                        &nbsp;&nbsp;
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" InitialValue="0"
                                            ControlToValidate="ddItem" Display="Dynamic" ValidationGroup="grpInv" ForeColor="Red"
                                            ErrorMessage="Please select item"></asp:RequiredFieldValidator>
                                    </div>
                                </div>
                            </div>




                            <div style="margin-bottom: 5px" class="input-group">
                                <div class="col-md-1 col-1 text-left">
                                </div>
                                <div class="col-sm-6">
                                    <div class="input-group">

                                        <asp:Button ID="btnGenerate" runat="server" Text="Generate" ValidationGroup="grpInv" OnClientClick="if(Page_ClientValidate()) ShowProgress()" CssClass="btn btn-sm btn-info" OnClick="btnGenerate_Click" />
                                    </div>
                                </div>
                            </div>


                        </div>




                        <div class="card-body">
                            <div class="col-sm-12">
                                <%--START LOGISTICS DETAILED--%>
                                <asp:GridView ID="gvStockCard" runat="server" ClientIDMode="Static" class="table table-bordered table-hover" AutoGenerateColumns="false" OnPreRender="gvStockCard_PreRender">
                                    <Columns>
                                        <asp:TemplateField HeaderText="Transaction Date">
                                            <HeaderStyle HorizontalAlign="Center" Width="12%" />
                                            <ItemStyle HorizontalAlign="Center" />
                                            <ItemTemplate>
                                                <asp:Label ID="lblTransactionDate" runat="server" Text='<%#  Convert.ToString(Eval("TransactionDate")).Equals("1/1/1900 12:00:00 AM")?"":Eval("TransactionDate")%>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField> 

                                        <asp:BoundField DataField="RRNo" HeaderText="Receiving Report No." ItemStyle-Width="10%" HeaderStyle-HorizontalAlign="Center">
                                            <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                        </asp:BoundField>


                                        <asp:BoundField DataField="sup_ItemCode" HeaderText="Item Code" ItemStyle-Width="10%" HeaderStyle-HorizontalAlign="Center">
                                            <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                        </asp:BoundField>


                                        <asp:BoundField DataField="Sup_DESCRIPTION" HeaderText="Item Description" ItemStyle-Width="10%" HeaderStyle-HorizontalAlign="Center">
                                            <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                        </asp:BoundField>
                                        

                                        <asp:BoundField DataField="vBatchNo" HeaderText="Batch No" ItemStyle-Width="10%" HeaderStyle-HorizontalAlign="Center">
                                            <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                        </asp:BoundField>


                                        <asp:TemplateField HeaderText="Date Expiry">
                                            <HeaderStyle HorizontalAlign="Center" Width="5%" />
                                            <ItemStyle HorizontalAlign="Center" />
                                            <ItemTemplate>
                                                <asp:Label ID="lblvDateExpiry" runat="server" Text='<%#  Convert.ToString(Eval("vDateExpiry", "{0:MM/dd/yyyy}")).Equals("01/01/1900")?"":Eval("vDateExpiry", "{0:MM/dd/yyyy}")%>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        
                                        <asp:BoundField DataField="Transaction" HeaderText="Transaction Status" ItemStyle-Width="10%" HeaderStyle-HorizontalAlign="Center">
                                            <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                        </asp:BoundField>

                                        <asp:BoundField DataField="BrName" HeaderText="Branch/Dept." ItemStyle-Width="10%" HeaderStyle-HorizontalAlign="Center">
                                            <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                        </asp:BoundField>


                                        <asp:BoundField DataField="RequestNo" HeaderText="Computer Issue Slip No" ItemStyle-Width="10%" HeaderStyle-HorizontalAlign="Center">
                                            <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                        </asp:BoundField>


                                        <asp:BoundField DataField="DRNumber" HeaderText="Request No" ItemStyle-Width="10%" HeaderStyle-HorizontalAlign="Center">
                                            <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                        </asp:BoundField>

                                        <asp:BoundField DataField="Qty" HeaderText="Incoming" DataFormatString="{0:#,##0.##;(#,##0.##);0}" ItemStyle-Width="5%" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                            <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                            <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                        </asp:BoundField>

                                        <asp:BoundField DataField="QtyOut" HeaderText="Outgoing" DataFormatString="{0:#,##0.##;(#,##0.##);0}" ItemStyle-Width="5%" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                            <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                            <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                        </asp:BoundField>

                                        <%--<asp:BoundField DataField="Qty" HeaderText="Quantity" DataFormatString="{0:#,##0.##;(#,##0.##);0}" ItemStyle-Width="5%" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                            <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                            <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                        </asp:BoundField>--%>



                                        <asp:BoundField DataField="EndingBal" HeaderText="Ending Balance"  DataFormatString="{0:#,##0.##;(#,##0.##);0}" ItemStyle-Width="15%" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                            <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                            <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                        </asp:BoundField>



                                    </Columns>
                                </asp:GridView>
                                <%--END LOGISTICS DETAILED--%>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </section>


    <div class="loading" align="center" style="margin-top: 100px;">
        <br />
        <br />
        <div class="loader"></div>
        <br />
        <asp:Label ID="Label2" runat="server" Text="Please wait" ForeColor="Red"></asp:Label>
    </div>

    <!-- jQuery -->
    <%--<script src="plugins/jquery/jquery.min.js"></script>--%>
    <!-- Bootstrap 4 -->
    <%--<script src="plugins/bootstrap/js/bootstrap.bundle.min.js"></script>--%>

     <script src="docsupport/jquery-3.2.1.min.js" type="text/javascript"></script>
    <script src="chosen.jquery.js" type="text/javascript"></script>
    <script src="docsupport/prism.js" type="text/javascript" charset="utf-8"></script>
    <script src="docsupport/init.js" type="text/javascript" charset="utf-8"></script>

    <script>
        $(function () {
            $("#<%= gvStockCard.ClientID%>").DataTable({
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
            }).buttons().container().appendTo('#gvStockCard_wrapper .col-md-6:eq(0)');;
        });
    </script>



    <!-- ################################################# END #################################################### -->

    <script type="text/javascript">
        function ShowSuccessMsg() {
            $(function () {
                $("#messageDiv").dialog({
                    title: "Stock Card",
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

