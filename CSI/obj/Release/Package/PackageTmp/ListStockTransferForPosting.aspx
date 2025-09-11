<%@ Page Title="Stock Transfer for Approval" Language="C#" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true" CodeBehind="ListStockTransferForPosting.aspx.cs" Inherits="CSI.ListStockTransferForPosting" %>
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
    </style>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
     <section class="content-header">
        <div class="container-fluid">
            <div class="row mb-2">
                <div class="col-sm-6">
                    <h5>List of Stock Transfer for Approval</h5>
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
                                        <div class="col-md-2 col-2 text-left">
                                            <asp:Label ID="Label3" runat="server" Text="Branch" style="width:125px;"></asp:Label>
                                        </div>
                                         <div class="col-lg-5 col-5">
                                             <div class="input-group" >
                                                 <asp:DropDownList ID="ddBranch" runat="server" CssClass="form-control" AutoPostBack="true" OnSelectedIndexChanged="ddBranch_SelectedIndexChanged" ></asp:DropDownList>
                                                 <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="ddBranch" InitialValue="0" ForeColor="Red" ValidationGroup="grpSave"
                                                     ErrorMessage="Please select branch"></asp:RequiredFieldValidator>
                                             </div>
                                         </div>
                                      <div class="col-lg-4 col-4">
                                      </div>
                                        <div class="col-lg-1 col-1 text-right">
                                             <div class="input-group" >
                                                 <asp:Button ID="btnPost" runat="server" Text="P O S T" CssClass="btn btn-primary" ValidationGroup="grpSave" OnClientClick="if(Page_ClientValidate()) ShowProgress()" OnClick="btnPost_Click"/>
                                             </div>
                                         </div>
                                 </div>
                                <div style="margin-bottom: 5px" class="input-group">
                                        <div class="col-md-2 col-2 text-left">
                                            <asp:Label ID="Label1" runat="server" Text="PRF No" style="width:125px;"></asp:Label>
                                        </div>
                                         <div class="col-lg-5 col-5">
                                             <div class="input-group" >
                                                 <asp:DropDownList ID="ddPRFNo" runat="server" CssClass="form-control" AutoPostBack="true" OnSelectedIndexChanged="ddPRFNo_SelectedIndexChanged" ></asp:DropDownList>
                                                 <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="ddPRFNo" InitialValue="0" ForeColor="Red" ValidationGroup="grpSave"
                                                     ErrorMessage="Please select PRF No"></asp:RequiredFieldValidator>
                                             </div>
                                         </div>
                                 </div>

                                


                          </div>


                             <div class="card-body">
                                <div class="col-sm-12">
                                    

                                        <asp:GridView ID="gvPicked" ClientIDMode="Static"  CssClass="table table-bordered active active" DataKeyNames="vRecNum" AutoGenerateColumns="false" runat="server" OnRowDeleting="gvPicked_RowDeleting" OnPreRender="gvPicked_PreRender" OnRowDataBound="gvPicked_RowDataBound">
                                    <Columns>
                                        <asp:BoundField DataField="vRecNum" HeaderText="ID" ItemStyle-Width="3%" ItemStyle-HorizontalAlign="Center" ItemStyle-CssClass="hiddencol" HeaderStyle-CssClass="hiddencol"/>
                                        <asp:BoundField DataField="HeaderID" HeaderText="HeaderID" ItemStyle-Width="3%" ItemStyle-HorizontalAlign="Center" ItemStyle-CssClass="hiddencol" HeaderStyle-CssClass="hiddencol"/>
                                        <asp:BoundField DataField="BrName" HeaderText="From Branch" ItemStyle-Width="10%" ItemStyle-HorizontalAlign="Center" />
                                        <asp:BoundField DataField="ReceiptNo" HeaderText="PRF No." ItemStyle-Width="5%" ItemStyle-HorizontalAlign="Center" />
                                        <asp:BoundField DataField="Sup_CategoryName" HeaderText="Category" ItemStyle-Width="10%" ItemStyle-HorizontalAlign="Center" />
                                        
                                        <asp:BoundField DataField="Sup_ItemCode" HeaderText="Item Code" ItemStyle-Width="5%" ItemStyle-HorizontalAlign="Center" />
                                        <asp:BoundField DataField="sup_DESCRIPTION" HeaderText="Item Description" ItemStyle-Width="20%" ItemStyle-HorizontalAlign="Center" />
                                        <asp:BoundField DataField="vQtyPicked" HeaderText="Qty" ItemStyle-Width="5%" DataFormatString="{0:###0;-###0;0}" ItemStyle-HorizontalAlign="Center" />
                                        <asp:BoundField HeaderText="Batch No." DataField="vBatchNo"  HtmlEncode="false" ItemStyle-Width="8%">
                                               <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                                <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                            </asp:BoundField>
                                              <asp:BoundField DataField="vDateExpiry" DataFormatString="{0:MM/dd/yyyy}" HeaderText="Date Expiry" ReadOnly="True">
                                                <HeaderStyle Width="8%" HorizontalAlign="Center" />
                                                <ItemStyle HorizontalAlign="Center"  />
                                            </asp:BoundField>
                            
                                          <asp:BoundField DataField="vTrandate" HeaderText="Date Encoded" ReadOnly="True">
                                                <HeaderStyle Width="10%" HorizontalAlign="Center" />
                                                <ItemStyle HorizontalAlign="Center"  />
                                            </asp:BoundField>

                                        <asp:BoundField DataField="vPickedBy" HeaderText="Encoded By" ReadOnly="True">
                                                <HeaderStyle Width="5%" HorizontalAlign="Center" />
                                                <ItemStyle HorizontalAlign="Center"  />
                                            </asp:BoundField>
                                        <asp:BoundField DataField="DestBranch" HeaderText="Transfer To" ItemStyle-Width="10%" ItemStyle-HorizontalAlign="Center" />
                                           <asp:BoundField DataField="Remarks" HeaderText="Reason of Return" ItemStyle-Width="10%" ItemStyle-HorizontalAlign="Center" />

                                         <%--<asp:TemplateField HeaderText="RGAS Items">
                                            <HeaderStyle HorizontalAlign="Center" />
                                            <ItemStyle HorizontalAlign="Center" Width="4%" />
                                            <ItemTemplate>
                                                <asp:CheckBox ID="ckStat" runat="server" Checked="false" />
                                            </ItemTemplate>
                                        </asp:TemplateField>--%>

                                               <asp:TemplateField>
                                                    <ItemStyle HorizontalAlign="Center" Width="5%" />
                                                    <ItemTemplate>
                                                        <asp:LinkButton ID="btnDeletePRF" runat="server" CommandName="Delete" Text="" class="btn btn-sm btn-danger"><i class="fa fa-trash"></i></asp:LinkButton>
                                                    </ItemTemplate>

                                                </asp:TemplateField>
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
                        <%--<img src="images/ajax-loader.gif" alt=""  />--%>
                        <div class="loader"></div>
                         <br />
                         <asp:Label ID="Label6" runat="server" Text="Please wait" ForeColor="Red"></asp:Label>
                         
                    </div>

      <!-- jQuery -->
    <script src="plugins/jquery/jquery.min.js"></script>
    <!-- Bootstrap 4 -->
    <script src="plugins/bootstrap/js/bootstrap.bundle.min.js"></script>


     <script>
         $(function () {
             $("#<%= gvPicked.ClientID%>").DataTable({
                 "paging": true,
                 "lengthChange": true,
                 "searching": true,
                 "ordering": false,
                 "aLengthMenu": [[-1, 10, 25, 50], ["All", 10, 25, 50]],
                 "iDisplayLength": -1,
                 "bSortable": false,
                 "aTargets": [0],
                 "buttons": ["excel", "pdf", "print", "colvis"],
                 "info": true,
                 "autoWidth": false,
                 "responsive": true,
             }).buttons().container().appendTo('#gvPicked_wrapper .col-md-6:eq(0)');;
         });
    </script>

<script type="text/javascript">
    function ShowSuccessMsg() {
        $(function () {
            $("#messageDiv").dialog({
                title: "Posting of PRF",
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
