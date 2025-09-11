<%@ Page Title="List - For Issuance" Language="C#" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true" CodeBehind="ListForIssuance.aspx.cs" Inherits="CSI.ListForIssuance" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
<link href="plugins/jquery-ui/jquery-ui.css" rel="stylesheet" />
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
    </style>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
        <section class="content-header">
        <div class="container-fluid">
            <div class="row mb-2">
                <div class="col-sm-6">
                    </div>
                <div class="col-sm-6">
                    <ol class="breadcrumb float-sm-right">
    
                     </ol>
                </div>
            </div>
        </div>
    </section>

    <!-- Main content -->
    <section class="content">
        <div class="container-fluid">
            <div class="row">
                <div class="col-12">
                    <div class="card">
                        <div class="card-header">
                            <h3 class="card-title">List of Request for Issuance</h3>
                        </div>
                     <div class="card-body">

                            <%--start--%>
                            <asp:GridView ID="gvForIssuance" runat="server" Width="100%" ClientIDMode="Static"  class="table table-bordered table-hover" DataKeyNames="Sup_ControlNo" AutoGenerateColumns="false" OnRowCommand="gvForIssuance_RowCommand" OnPreRender="gvForIssuance_PreRender">
                                <Columns>
                                    

                                     <asp:BoundField DataField="BrCode" HeaderText="BrCode" ItemStyle-Width="5%" HeaderStyle-HorizontalAlign="Center" ItemStyle-CssClass="hiddencol" HeaderStyle-CssClass="hiddencol">
                                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                    </asp:BoundField>

                                   <asp:BoundField DataField="BrName" HeaderText="Branch" ItemStyle-Width="15%" HeaderStyle-HorizontalAlign="Center">
                                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                    </asp:BoundField>

                                     <asp:BoundField DataField="Sup_ControlNo" HeaderText="No." ItemStyle-Width="10%" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                        <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                    </asp:BoundField>

                                      <asp:BoundField DataField="DatePosted" HeaderText="Date Posted" ItemStyle-Width="10%" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                        <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                    </asp:BoundField>

                                    <asp:BoundField DataField="Sup_CategoryName" HeaderText="Category" ItemStyle-Width="20%" HeaderStyle-HorizontalAlign="Center">
                                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                        <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                    </asp:BoundField>

                                    <asp:BoundField DataField="vPickedBy" HeaderText="Picked By" ItemStyle-Width="15%" HeaderStyle-HorizontalAlign="Center">
                                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                        <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                    </asp:BoundField>

                                    <asp:BoundField DataField="RequestType" HeaderText="Request Type" ItemStyle-Width="5%" HeaderStyle-HorizontalAlign="Center">
                                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                        <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                    </asp:BoundField>
                                    

                                    <asp:TemplateField>
                                        <ItemStyle HorizontalAlign="Center" Width="5%" />
                                        <ItemTemplate>
                                            <asp:LinkButton ID="lnkView" runat="server" CommandName="ViewDetail" class="btn btn-sm btn-info" CommandArgument='<%#Eval("Sup_ControlNo") %>'><i class="fa fa-edit"></i></asp:LinkButton>
                                        </ItemTemplate>
                                    </asp:TemplateField>


                                </Columns>
                            </asp:GridView>



                         

                            <%--end--%>
                        </div>
                        <!-- /.card-body -->
                    </div>
                    <!-- /.card -->
                </div>
                <!-- /.col -->
            </div>
            <!-- /.row -->
        </div>
        <!-- /.container-fluid -->
    </section>


      <!-- jQuery -->
    <script src="plugins/jquery/jquery.min.js"></script>
    <!-- Bootstrap 4 -->
    <script src="plugins/bootstrap/js/bootstrap.bundle.min.js"></script>



    <script>
        $(function () {
            $("#<%=gvForIssuance.ClientID%>").DataTable({
                "lengthChange": false,
                "autoWidth": false,
                "ordering": false,
                "bSortable": false, "aTargets": [0],
                "buttons": ["excel", "pdf", "print"],
                "aLengthMenu": [[5, 10, 25, 50, -1], [5, 10, 25, 50, "All"]],
                "iDisplayLength": 5,


                //"buttons": [
                //               {


                //                   extend: 'excelHtml5',
                //                   exportOptions: {
                //                       columns: [1, 2, 3, 4, 5, 6]
                //                   },
                //                   title: "For Issuance",
                //                   message: "Supplies Request"


                //               },

                //                    {
                //                        extend: 'pdfHtml5',
                //                        exportOptions: {
                //                            columns: [1, 2, 3, 4, 5, 6]
                //                        },
                //                        title: "For Issuance",
                //                        message: "Supplies Request"
                //                    }


                //],

                "responsive": true,
            }).buttons().container().appendTo('#gvForIssuance_wrapper .col-md-6:eq(0)');;
        });
</script>
        
<!-- ################################################# END #################################################### -->


        
<!-- ################################################# END #################################################### -->

<script type="text/javascript">
    function ShowSuccessMsg() {
        $(function () {
            $("#messageDiv").dialog({
                title: "For Issuance",
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

<div id="messageDiv" style="display:none;">
    <asp:UpdatePanel ID="UpdatePanel3" runat="server">
            <ContentTemplate>
                <asp:Label Text="" ID="lblMsg" runat="server"/>
            </ContentTemplate>
    </asp:UpdatePanel>
</div>
<!-- ################################################# END #################################################### -->

</asp:Content>

