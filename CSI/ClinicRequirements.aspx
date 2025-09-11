<%@ Page Title="Clinic Requirements" MaintainScrollPositionOnPostback="true" Language="C#" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true" CodeBehind="ClinicRequirements.aspx.cs" Inherits="CSI.ClinicRequirements" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
     <link href="plugins/jquery-ui/jquery-ui.css" rel="stylesheet" />
    <style type="text/css">
        table th {
            text-align: center;
            vertical-align: middle;
            background-color: #f2f2f2;
            font-size: 12px;
        }

        table tr {
            vertical-align: middle;
            font-size: 10px;
            text-align: center;
        }

        .hiddencol {
            display: none;
        }
         
        .myTitleClass .ui-dialog-titlebar {
            /*background-color:magenta;*/
             background-image: linear-gradient(to bottom right, red, yellow);
             /*background: yellow;*/
  
  height: 20%;
  width: 100%;
          /*background:  red;*/
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
                            <h3 class="card-title">Clinic Supplies Requirements</h3>
                        </div>
                     <div class="card-body">

                            <%--start--%>
                            <asp:GridView ID="gvRequirements" runat="server" Width="100%" EmptyDataText="No Record Found." ClientIDMode="Static"  class="table table-bordered table-hover" DataKeyNames="sup_ItemCode" AutoGenerateColumns="false" OnPreRender="gvRequirements_PreRender" OnRowCommand="gvRequirements_RowCommand" >
                                <Columns>
                                    
                                    <asp:BoundField HeaderText="BrC" DataField="BrCode" ItemStyle-Width="3%"  ItemStyle-CssClass="hiddencol" HeaderStyle-CssClass="hiddencol">
                                        <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                        <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                    </asp:BoundField>

                                   <asp:BoundField DataField="Branch" HeaderText="Branch" ItemStyle-Width="15%" HeaderStyle-HorizontalAlign="Center">
                                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                    </asp:BoundField>

                                     <asp:BoundField DataField="Sup_CategoryName" HeaderText="Category" ItemStyle-Width="10%" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                        <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                    </asp:BoundField>

                                    <asp:BoundField DataField="sup_ItemCode" HeaderText="ItemCode" ItemStyle-Width="5%" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                        <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                    </asp:BoundField>

                                    <asp:BoundField DataField="ClinicRequirements" HeaderText="Item Description" ItemStyle-Width="20%" HeaderStyle-HorizontalAlign="Center">
                                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                        <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                    </asp:BoundField>

                                   

                                    <asp:BoundField DataField="cUOM" HeaderText="UOM" HtmlEncode="false" ItemStyle-Width="3%" HeaderStyle-HorizontalAlign="Center">
                                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                        <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                    </asp:BoundField>

                                    <asp:BoundField DataField="BRBalance" HeaderText="Branch<br />Balance" HtmlEncode="false" ItemStyle-Width="10%" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                                <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                                <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                            </asp:BoundField>

                                    <asp:BoundField DataField="TotalQtyRequired" HeaderText="Branch Qty<br />Requirements" HtmlEncode="false" ItemStyle-Width="10%" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                                <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                                <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                            </asp:BoundField>

                                    

                                    <asp:BoundField DataField="HOBal" HeaderText="Head Office<br />Balance" HtmlEncode="false" ItemStyle-Width="10%" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                                <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                                <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                            </asp:BoundField>
                                          
                               
                                    <asp:TemplateField HeaderText="View<br />Transaction">
                                        <ItemStyle HorizontalAlign="Center" Width="5%" />
                                        <ItemTemplate>
                                            <asp:LinkButton ID="lnkView" runat="server" CommandName="ViewDetail" class="btn btn-sm btn-info" CommandArgument='<%#Eval("sup_ItemCode") %>'><i class="fa fa-eye"></i></asp:LinkButton>
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
            $("#<%=gvRequirements.ClientID%>").DataTable({
                "lengthChange": false,
                "autoWidth": false,
                "ordering": false,
                "bSortable": false, "aTargets": [0],
                "buttons": ["excel", "pdf", "print"],
                "aLengthMenu": [[-1, 5, 10, 25, 50], ["All", 5, 10, 25, 50]],
                "iDisplayLength": 25,


                //"buttons": [
                //               {


                //                   extend: 'excelHtml5',
                //                   exportOptions: {
                //                       columns: [1, 2, 3, 4, 5, 6]
                //                   },
                //                   title: "For Approval",
                //                   message: "Supplies Request"


                //               },

                //                    {
                //                        extend: 'pdfHtml5',
                //                        exportOptions: {
                //                            columns: [1, 2, 3, 4, 5, 6]
                //                        },
                //                        title: "For Approval",
                //                        message: "Supplies Request"
                //                    }


                //],

                "responsive": true,
            }).buttons().container().appendTo('#gvRequirements_wrapper .col-md-6:eq(0)');;
        });
</script>
        
<!-- ################################################# END #################################################### -->


        
<!-- ################################################# END #################################################### -->

<script type="text/javascript">
    function ShowSuccessMsg() {
        $(function () {
            $("#messageDiv").dialog({
                title: "Clinic Requirements",
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


<%--*******************************************************************************--%>
    <script type="text/javascript">
        function CloseGridReceiptNo() {
            $(function () {
                $("#ShowReceiptNo").dialog('close');
            });
        }
    </script>

    <script type="text/javascript">
        function ShowGridReceiptNo() {
            $(function () {
                $("#ShowReceiptNo").dialog({
                    title: "Transaction Record",
                    dialogClass: 'myTitleClass',
                    create: function (event, ui) {
                        $(event.target).parent().css('position', 'fixed');
                    },

                    width: '1100px',
                    buttons: {
                        Close: function () {
                            $(this).dialog('close');
                        }
                    },
                    modal: true

                });
                $("#ShowReceiptNo").parent().appendTo($("form:first"));
            });
        }
    </script>
    
     <%--style="display:none;"--%>
    <div id="ShowReceiptNo" style="display:none;">
                <div style="overflow: auto; max-height: 400px;">
                    <asp:Label ID="lblQtyToPick" runat="server" Text=""></asp:Label>
                    <br />
                
                    


                <asp:GridView ID="gvReceiptNo" runat="server" class="table table-bordered table-hover" DataKeyNames="RecID" AutoGenerateColumns="false" OnRowCommand="gvReceiptNo_RowCommand" >
                    <Columns>
                         <asp:BoundField HeaderText="Rec No." DataField="RecID" ItemStyle-Width="3%"  ItemStyle-CssClass="hiddencol" HeaderStyle-CssClass="hiddencol">
                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                            <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                        </asp:BoundField>

                         <asp:BoundField HeaderText="BrC" DataField="BrCode" ItemStyle-Width="3%"  ItemStyle-CssClass="hiddencol" HeaderStyle-CssClass="hiddencol">
                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                            <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                        </asp:BoundField>

                        
                        <asp:BoundField DataField="Branch" HeaderText="Branch" ReadOnly="True">
                            <HeaderStyle Width="10%" HorizontalAlign="Center" />
                            <ItemStyle HorizontalAlign="Center"  />
                        </asp:BoundField>

                           <asp:BoundField DataField="SalesDate" DataFormatString="{0:MM/dd/yyyy}" HeaderText="Date" ReadOnly="True">
                            <HeaderStyle Width="10%" HorizontalAlign="Center" />
                            <ItemStyle HorizontalAlign="Center"  />
                        </asp:BoundField>


                           <asp:BoundField HeaderText="Customer Name" DataField="CustomerName"  HtmlEncode="false" ItemStyle-Width="15%">
                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                            <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                        </asp:BoundField>

                       

                        <asp:BoundField DataField="ReceiptNo"  HeaderText="Receipt No" ReadOnly="True">
                            <HeaderStyle Width="5%" HorizontalAlign="Center" />
                            <ItemStyle HorizontalAlign="Center"  />
                        </asp:BoundField>

                        <asp:BoundField DataField="ItemDescription"  HeaderText="Service Availed" ReadOnly="True">
                            <HeaderStyle Width="15%" HorizontalAlign="Center" />
                            <ItemStyle HorizontalAlign="Center"  />
                        </asp:BoundField>

                     
                        <asp:BoundField DataField="vQty"  HeaderText="Qty" ReadOnly="True">
                            <HeaderStyle Width="5%" HorizontalAlign="Center" />
                            <ItemStyle HorizontalAlign="Center"  />
                        </asp:BoundField>

                        <asp:BoundField DataField="TotalSession"  HeaderText="Total<br />Session"  HtmlEncode="false" ReadOnly="True">
                            <HeaderStyle Width="5%" HorizontalAlign="Center" />
                            <ItemStyle HorizontalAlign="Center"  />
                        </asp:BoundField>

                          <asp:BoundField DataField="sup_ItemCode"  HeaderText="ItemCode" ReadOnly="True">
                            <HeaderStyle Width="5%" HorizontalAlign="Center" />
                            <ItemStyle HorizontalAlign="Center"  />
                        </asp:BoundField>

                          <asp:BoundField DataField="ClinicRequirements"  HeaderText="Clinic Supplies" ReadOnly="True">
                            <HeaderStyle Width="15%" HorizontalAlign="Center" />
                            <ItemStyle HorizontalAlign="Center"  />
                        </asp:BoundField>

                          <asp:BoundField DataField="cQty"  HeaderText="Quantity<br />Requirements" HtmlEncode="false" ReadOnly="True">
                            <HeaderStyle Width="5%" HorizontalAlign="Center" />
                            <ItemStyle HorizontalAlign="Center"  />
                        </asp:BoundField>

          
                         <asp:BoundField DataField="cUOM"  HeaderText="UOM" ReadOnly="True">
                            <HeaderStyle Width="5%" HorizontalAlign="Center" />
                            <ItemStyle HorizontalAlign="Center"  />
                        </asp:BoundField>
                        
                    <asp:TemplateField HeaderText="Action">
                        <ItemStyle HorizontalAlign="Center" Width="10%" />
                        <ItemTemplate>
                            <asp:LinkButton ID="lnkSelect" runat="server" CommandName="linkSelect" Text="Select" class="btn btn-sm btn-primary"  CommandArgument='<%#Eval("RecID") %>'><i class="fa fa-bookmark"></i></asp:LinkButton>
                        </ItemTemplate>
                    </asp:TemplateField>                

                    </Columns>
                </asp:GridView>
            </div>

    </div>








    <!-- ################################################# END #################################################### -->


</asp:Content>
