<%@ Page Title="Issue Slip Printing" Language="C#" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true" CodeBehind="PrintMultipleRequest.aspx.cs" Inherits="CSI.PrintMultipleRequest" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="plugins/jquery-ui/jquery-ui.css" rel="stylesheet" />
    <link rel="stylesheet" href="docsupport/prism.css" />
    <link rel="stylesheet" href="chosen.css" />

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
                    <h5>Print Multiple Request in One Issue Slip</h5>
                </div>
                <div class="col-sm-6">
                    <ol class="breadcrumb float-sm-right">
                         
                        <li class="breadcrumb-item"><a href="ListForIssuance.aspx" class="nav-link">Back to List</a></li>
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
                                 <div style="margin-bottom: 10px" class="input-group">
                                    <div class="col-sm-2 col-2 text-left">
                                        <asp:Label ID="Label4" runat="server" Text="Branch Code" style="width:125px;"></asp:Label>
                                        </div>
                                     <div class="col-md-9 col-9">
                                         <div class="input-group" >
                                            <asp:Label ID="lblBranchCode" runat="server" Text=""></asp:Label>
                                        </div>
                                     </div>
                                     <div class="col-sm-1 col-1 text-right">
                                        <asp:Button ID="btnPrint" runat="server" Text="PRINT" CssClass="btn btn-success btn-sm" OnClick="btnPrint_Click"/>
                                     </div>
                                </div>

                                <div style="margin-bottom: 10px" class="input-group">
                                    <div class="col-sm-2 col-2 text-left">
                                        <asp:Label ID="Label1" runat="server" Text="Branch Name" style="width:125px;"></asp:Label>
                                        </div>
                                     <div class="col-md-7 col-7">
                                         <div class="input-group" >
                                                <asp:Label ID="lblBranch" runat="server" Text=""></asp:Label>
                                        </div>
                                     </div>
                                </div>

                                

                                <div style="margin-bottom: 10px" class="input-group">
                                    <div class="col-sm-2 col-2 text-left">
                                        <asp:Label ID="Label3" runat="server" Text="Request No." style="width:125px;"></asp:Label>
                                        </div>
                                     <div class="col-md-7 col-7">
                                         <div class="input-group" >
                                                       <select id="selMultiple" runat="server" data-placeholder="Choose Control No..." class="chosen-select" multiple="true" style="width:650px">
                                                        </select>
                                       
                                        </div>
                                     </div>
                                </div>

                                <div style="margin-bottom: 10px" class="input-group">
                                       <div class="col-sm-2 col-2 text-left">
                                        
                                        </div>
                                     <div class="col-md-7 col-7">
                                         <div class="input-group" >
                                              <asp:Button ID="btnLoadDetailToPrint" runat="server" Text="L O A D" CssClass="btn btn-info" OnClick="btnLoadDetailToPrint_Click"/>
                                                Click "LOAD" button if you add or removed Request No in the selected Request No list 
                                        </div>
                                     </div>
                                </div>
                                      
                                      
                            </div>

                             <div class="card-body">
                                 <asp:GridView ID="gvPrint" runat="server" Width="100%" class="table table-bordered table-hover"  AutoGenerateColumns="false" >
                                     <Columns>
                                         
                                         <asp:TemplateField HeaderText="No"  ItemStyle-Width="5%">   
                                             <ItemTemplate>
                                                     <%# Container.DataItemIndex + 1 %>   
                                             </ItemTemplate>
                                         </asp:TemplateField>

                                         <asp:BoundField DataField="Sup_ControlNo" HeaderText="Control No" ItemStyle-Width="10%" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                             <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                             <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                         </asp:BoundField>
                                         
                                         <asp:BoundField DataField="ItemDesc" HeaderText="Item Description" ItemStyle-Width="30%" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" ReadOnly="true">
                                             <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                             <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                         </asp:BoundField>

                                         <asp:BoundField DataField="vQtyPicked" HeaderText="Quantity" HtmlEncode="false" ItemStyle-Width="8%" ReadOnly="true" HeaderStyle-HorizontalAlign="Center">
                                             <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                             <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                         </asp:BoundField>

                                         <asp:BoundField DataField="sup_UOM" HeaderText="UOM" HtmlEncode="false" ItemStyle-Width="5%" ReadOnly="true" HeaderStyle-HorizontalAlign="Center">
                                             <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                             <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                         </asp:BoundField>


                                         <asp:BoundField DataField="vBatchNo" HeaderText="Batch No" ItemStyle-Width="10%" ReadOnly="true" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                             <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                             <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                         </asp:BoundField>

                                         <asp:TemplateField HeaderText="Date Expiry">
                                            <HeaderStyle HorizontalAlign="Center" Width="5%" />
                                            <ItemStyle HorizontalAlign="Center" />
                                            <ItemTemplate>
                                                       <asp:Label ID="lblvDateExpiry" runat="server" Text='<%#  Convert.ToString(Eval("vDateExpiry", "{0:MM/dd/yyyy}")).Equals("01/01/1900")?"":Eval("vDateExpiry", "{0:MM/dd/yyyy}")%>'></asp:Label>
                                             </ItemTemplate>
                                        </asp:TemplateField>

                                         <%--<asp:BoundField DataField="vDateExpiry" HeaderText="Date Expiry" DataFormatString="{0:MM/dd/yyyy}" ItemStyle-Width="10%" ReadOnly="true" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                             <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                             <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                         </asp:BoundField>--%>
                                     </Columns>
                                 </asp:GridView>
                             </div>
                         </div>
                </div>
            </div>
        </div>
    </section>

<script src="docsupport/jquery-3.2.1.min.js" type="text/javascript"></script>
    <script src="chosen.jquery.js" type="text/javascript"></script>
    <script src="docsupport/prism.js" type="text/javascript" charset="utf-8"></script>
    <script src="docsupport/init.js" type="text/javascript" charset="utf-8"></script>

<!-- ################################################# END #################################################### -->

    
        
<!-- ################################################# END #################################################### -->

<script type="text/javascript">
    function ShowSuccessMsg() {
        $(function () {
            $("#messageDiv").dialog({
                title: "Printing",
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

<!-- ################################################# START #################################################### -->


</asp:Content>
