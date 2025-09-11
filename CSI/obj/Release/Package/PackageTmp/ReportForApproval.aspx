<%@ Page Title="Report For Approval" Language="C#" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true" CodeBehind="ReportForApproval.aspx.cs" Inherits="CSI.ReportForApproval" %>
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
            <div class="row mb-2">
                <div class="col-sm-6">
                    <h5>List of Request for Approval</h5>
                </div>
                <div class="col-sm-6">
                    <ol class="breadcrumb float-sm-right">
                         
                        <%--<li class="breadcrumb-item"><a href="ListOfForApproval.aspx" class="nav-link">Back to List</a></li>--%>
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
                           <%-- <div class="card-header">
                                 <div style="margin-bottom: 10px" class="input-group">
                                    

                                    <div class="col-sm-1 col-1 text-left">
                                        <asp:Label ID="Label4" runat="server" Text="Branch " style="width:125px;"></asp:Label>
                                        </div>
                                     <div class="col-md-10 col-10">
                                         <div class="input-group" >
                                            <asp:Label ID="lblBranchCode" runat="server" Text=""></asp:Label>
                                             &nbsp;&nbsp;-&nbsp;&nbsp;
                                             <asp:Label ID="lblBranch" runat="server" Text=""></asp:Label>
                                        </div>
                                     </div>

                                     <div class="col-md-1 col-1 text-right">
                                         <div class="input-group" >
                                             <asp:Button ID="btnPost" runat="server" Text="APPROVE"  CssClass="btn btn-sm btn-primary" OnClick="btnPost_Click" />
                                         </div>
                                     </div>

                                 </div>

                                <div style="margin-bottom: 10px" class="input-group">
                                     <asp:Label ID="Label2" runat="server" Text="No." style="width:125px;"></asp:Label>
                                     <asp:Label ID="lblNo" runat="server" Text=""></asp:Label>
                                 </div>

                                <div style="margin-bottom: 10px" class="input-group">
                                     <asp:Label ID="Label3" runat="server" Text="Date Submit" style="width:125px;"></asp:Label>
                                     <asp:Label ID="lblDateSubmit" runat="server" Text=""></asp:Label>
                                 </div>

                                <div style="margin-bottom: 10px" class="input-group">
                                     <asp:Label ID="Label5" runat="server" Text="Category" style="width:125px;"></asp:Label>
                                     <asp:Label ID="lblCategory" runat="server" Text=""></asp:Label>
                                 </div>

                                <div style="margin-bottom: 10px" class="input-group">
                                     <asp:Label ID="Label1" runat="server" Text="Type" style="width:125px;"></asp:Label>
                                     <asp:Label ID="lblType" runat="server" Text=""></asp:Label>
                                 </div>

                                
                         </div>--%>

                           <div class="card-body">

                            <asp:GridView ID="gvForApproval" runat="server" Width="100%"  ClientIDMode="Static"  class="table table-bordered table-hover" AutoGenerateColumns="false" OnPreRender="gvForApproval_PreRender">
                                <Columns>
                                    
                                    <asp:BoundField DataField="RequestType" HeaderText="Request Type" ItemStyle-Width="5%" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"  >
                                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                        <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                    </asp:BoundField>

                                    <asp:BoundField DataField="BrName" HeaderText="Branch" ItemStyle-Width="10%" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"  >
                                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                        <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                    </asp:BoundField>

                                    <asp:BoundField DataField="Sup_ControlNo" HeaderText="Request No" ItemStyle-Width="10%" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"  >
                                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                        <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                    </asp:BoundField>

                                      <asp:BoundField DataField="DateSubmit" HeaderText="Date Submit" DataFormatString="{0:MM/dd/yyyy}" ItemStyle-Width="10%" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"  >
                                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                        <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                    </asp:BoundField>
                                    
                                    <asp:BoundField DataField="Sup_CategoryName" HeaderText="Category" ItemStyle-Width="10%" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"  >
                                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                        <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                     </asp:BoundField>

                                   <asp:BoundField DataField="Sup_ItemCode" HeaderText="Item Code" ItemStyle-Width="10%" ReadOnly="true" HeaderStyle-HorizontalAlign="Center">
                                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                    </asp:BoundField>

                                     

                                      <asp:BoundField DataField="sup_DESCRIPTION" HeaderText="Item Description" ItemStyle-Width="30%" ReadOnly="true" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                        <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                    </asp:BoundField>

                               

                                    <asp:BoundField DataField="Branch Balance" HeaderText="Balance<br/>(Branch/Dept.)"  HtmlEncode="false" ItemStyle-Width="10%" ReadOnly="true" HeaderStyle-HorizontalAlign="Center">
                                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                        <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                    </asp:BoundField>

                                   <asp:BoundField DataField="HOBalance" HeaderText="Balance<br/>(Logistics)" HtmlEncode="false" ItemStyle-Width="10%" ReadOnly="true" HeaderStyle-HorizontalAlign="Center">
                                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                        <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                    </asp:BoundField>
                                    

                                   <asp:BoundField DataField="Sup_Qty" HeaderText="Quantity<br/>Request" HtmlEncode="false" ItemStyle-Width="10%" ReadOnly="true" HeaderStyle-HorizontalAlign="Center">
                                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                        <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                    </asp:BoundField>
                                         
                                     <asp:BoundField DataField="Sup_EncodedBy" HeaderText="Encoded By" HtmlEncode="false" ItemStyle-Width="15%" ReadOnly="true" HeaderStyle-HorizontalAlign="Center">
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



<script src="plugins/jquery/jquery.min.js"></script>
    <!-- Bootstrap 4 -->
<script src="plugins/bootstrap/js/bootstrap.bundle.min.js"></script>

<!-- ################################################# END #################################################### -->

    <script>
        $(function () {
            $("#<%= gvForApproval.ClientID%>").DataTable({
                "paging": true,
                "lengthChange": true,
                "searching": true,
                "ordering": true,
                "aLengthMenu": [[10, 25, 50, -1], [10, 25, 50, "All"]],
                "iDisplayLength": 10,
                "bSortable": true,
                "aTargets": [0,1,2,3],
                "buttons": ["excel", "pdf", "colvis"],
                "info": true,
                "autoWidth": false,
                "responsive": true,
            }).buttons().container().appendTo('#gvForApproval_wrapper .col-md-6:eq(0)');;
        });
    </script>


<script type="text/javascript">
    function ShowSuccessMsg() {
        $(function () {
            $("#messageDiv").dialog({
                title: "For Approval",
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
