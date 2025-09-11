<%@ Page Title="Add Item" MaintainScrollPositionOnPostback="true" Language="C#" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true" CodeBehind="setupAddItem.aspx.cs" Inherits="CSI.setupAddItem" %>
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
                    <h5>Add Item</h5>
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
                                               Category :
                                </div>
                                <div class="col-sm-3">
                                    <div class="input-group">
                                          <asp:DropDownList ID="ddCategory" runat="server" class="form-control" Style="width: 135px;  text-transform:uppercase;" AutoPostBack="True" OnSelectedIndexChanged="ddCategory_SelectedIndexChanged" >
                                            </asp:DropDownList> 
                                        <asp:RequiredFieldValidator ID="Req4" runat="server" ControlToValidate="ddCategory" Display="Dynamic" 
                                            InitialValue="0" ForeColor="Red" ValidationGroup="grpSave"
                                                 ErrorMessage="Please select category"></asp:RequiredFieldValidator>
                                    </div>
                                </div>
                            </div>

                            <div style="margin-bottom: 5px" class="input-group">
                                 <div class="col-md-2 col-2 text-left">
                                               Last Itemcode Used :
                                </div>
                                <div class="col-sm-3">
                                    <div class="input-group">
                                        <asp:Label ID="lblLastItemcodeUsed" runat="server" Text=""></asp:Label>
                                    </div>
                                </div>
                            </div>
                       
                            <div style="margin-bottom: 5px" class="input-group">
                                 <div class="col-md-2 col-2 text-left">
                                               Itemcode:
                                </div>
                                <div class="col-sm-3">
                                    <div class="input-group">
                                        <asp:TextBox ID="txtItemCode" CssClass="form-control" style="text-transform:uppercase;" runat="server"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtItemCode" Display="Dynamic" 
                                            ForeColor="Red" ValidationGroup="grpSave"
                                                 ErrorMessage="Please supply"></asp:RequiredFieldValidator>
                                    </div>
                                </div>
                            </div>

                            <div style="margin-bottom: 5px" class="input-group">
                                 <div class="col-md-2 col-2 text-left">
                                        Item Description       
                                </div>
                                <div class="col-sm-6">
                                    <div class="input-group">
                                        <asp:TextBox ID="txtItemDescription" CssClass="form-control" MaxLength="250"  runat="server"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="txtItemDescription" Display="Dynamic" 
                                            ForeColor="Red" ValidationGroup="grpSave"
                                                 ErrorMessage="Please supply"></asp:RequiredFieldValidator>
                                    </div>
                                </div>
                            </div>

                           <div style="margin-bottom: 5px" class="input-group">
                                 <div class="col-md-2 col-2 text-left">
                                               Unit of Measure:
                                </div>
                                <div class="col-sm-2">
                                    <div class="input-group">
                                        <asp:TextBox ID="txtUOM" CssClass="form-control" MaxLength="10" style="text-transform:uppercase;" runat="server"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="txtUOM" Display="Dynamic" 
                                            ForeColor="Red" ValidationGroup="grpSave"
                                                 ErrorMessage="Please supply"></asp:RequiredFieldValidator>
                                    </div>
                                </div>
                            </div>

                            <div style="margin-bottom: 5px" class="input-group">
                                 <div class="col-md-2 col-2 text-left">
                                              
                                </div>
                                <div class="col-sm-2">
                                    <div class="input-group">
                                        <asp:Button ID="btnSave" runat="server" Text="S A V E" ValidationGroup="grpSave" OnClientClick="if(Page_ClientValidate()) ShowProgress()" CssClass="btn btn-sm btn-info" OnClick="btnSave_Click"/>   
                                    </div>
                                </div>
                            </div>

                        </div>

                         <div class="card-body">
                                <div class="col-sm-12">
                                    <%--START LOGISTICS DETAILED--%>
                                    <asp:GridView ID="gvItems" runat="server" ClientIDMode="Static" DataKeyNames="sup_ID" EmptyDatatext="No Record Found" class="table table-bordered table-hover" AutoGenerateColumns="false" OnPreRender="gvItems_PreRender" OnRowCancelingEdit="gvItems_RowCancelingEdit" OnRowEditing="gvItems_RowEditing" OnRowUpdating="gvItems_RowUpdating" OnRowDataBound="gvItems_RowDataBound">
                                        <Columns>
                                            
                                            <asp:BoundField DataField="sup_ID" HeaderText="ID" ReadOnly="true" ItemStyle-Width="3%" ItemStyle-HorizontalAlign="Center" ItemStyle-CssClass="hiddencol" HeaderStyle-CssClass="hiddencol"/>
                                            <asp:BoundField DataField="sup_CATEGORY" HeaderText="cID" ReadOnly="true" ItemStyle-Width="3%" ItemStyle-HorizontalAlign="Center" ItemStyle-CssClass="hiddencol" HeaderStyle-CssClass="hiddencol"/>
                                                   
                                             
                                            <asp:BoundField DataField="Sup_CategoryName" HeaderText="Category" ItemStyle-Width="10%" ReadOnly="true" HeaderStyle-HorizontalAlign="Center">
                                                <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                            </asp:BoundField>

                                     

                                            <asp:BoundField DataField="sup_ItemCode" HeaderText="Item Code" ItemStyle-Width="8%"  ReadOnly="true" HeaderStyle-HorizontalAlign="Center">
                                                <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                            </asp:BoundField>
                                            
                                            <asp:TemplateField HeaderText="Item Description" ItemStyle-Width="30%" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblsup_DESCRIPTION" runat="server" Text='<%#Eval("sup_DESCRIPTION") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <EditItemTemplate>
                                                        <asp:TextBox ID="txtsup_DESCRIPTION" runat="server"  class="form-control" Text='<%#Eval("sup_DESCRIPTION") %>' BackColor="#ffcc99" Style="text-align: left;"></asp:TextBox>
                                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator17" runat="server" ErrorMessage="Required" 
                                                            ControlToValidate="txtsup_DESCRIPTION" ValidationGroup='<%# "Grp-" + Container.DataItemIndex %>'  ForeColor="Red" Display="Dynamic"></asp:RequiredFieldValidator>
                                                  
                                                    </EditItemTemplate>
                                              </asp:TemplateField>

                                        

                                             <asp:TemplateField HeaderText="UOM" ItemStyle-Width="7%" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblsup_UOM" runat="server" Text='<%#Eval("sup_UOM") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <EditItemTemplate>
                                                        <asp:TextBox ID="txtsup_UOM" runat="server"  class="form-control" Width="95px" Text='<%#Eval("sup_UOM") %>' BackColor="#ffcc99" Style="text-align: center;"></asp:TextBox>
                                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator171" runat="server" ErrorMessage="Required" 
                                                            ControlToValidate="txtsup_UOM" ValidationGroup='<%# "Grp-" + Container.DataItemIndex %>'  ForeColor="Red" Display="Dynamic"></asp:RequiredFieldValidator>
                                                    </EditItemTemplate>
                                              </asp:TemplateField>

                                              <asp:TemplateField HeaderText="Unit Cost" ItemStyle-Width="10%" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Right">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblsup_UnitCost" runat="server" Text='<%#Eval("sup_UnitCost") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <EditItemTemplate>
                                                        <asp:TextBox ID="txtsup_UnitCost" runat="server"  class="form-control decimalnumbers-only" Width="125px" Text='<%#Eval("sup_UnitCost") %>' BackColor="#ffcc99" Style="text-align: right;"></asp:TextBox>
                                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator172" runat="server" ErrorMessage="Required" 
                                                            ControlToValidate="txtsup_UnitCost" ValidationGroup='<%# "Grp-" + Container.DataItemIndex %>'  ForeColor="Red" Display="Dynamic"></asp:RequiredFieldValidator>
                                                        <asp:RegularExpressionValidator ID="REV1" runat="server" ControlToValidate="txtsup_UnitCost" Display="Dynamic"
                                                            ErrorMessage="Only numeric allowed here!" ValidationExpression="^\d+(\.\d{1,4})?$"
                                                            ValidationGroup='<%# "Grp-" + Container.DataItemIndex %>'  ForeColor="Red"></asp:RegularExpressionValidator>
                                                    </EditItemTemplate>
                                              </asp:TemplateField>
                                            
                                            <asp:TemplateField HeaderText="Purchasing Code" ItemStyle-Width="10%" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblsup_PurchasingCode" runat="server" Text='<%#Eval("sup_PurchasingCode") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <EditItemTemplate>
                                                        <asp:TextBox ID="txtsup_PurchasingCode" runat="server"  class="form-control decimalnumbers-only" Width="125px" Text='<%#Eval("sup_PurchasingCode") %>' BackColor="#ffcc99" Style="text-align: center;"></asp:TextBox>
                                                    </EditItemTemplate>
                                              </asp:TemplateField>

                                            

                                            <asp:BoundField DataField="sup_CreatedDate" HeaderText="Date Created" HtmlEncode="false" ReadOnly="true" DataFormatString="{0:MM/dd/yyyy}" ItemStyle-Width="8%" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                                <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                                <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                            </asp:BoundField>


                                            <asp:TemplateField HeaderText="Status" ItemStyle-Width="10%" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lbliStat" runat="server" Text='<%#Eval("iStat") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <EditItemTemplate>
                                                        <asp:DropDownList ID="ddStatus" runat="server" class="form-control"  Width="125px">
                                                
                                                            </asp:DropDownList> 
                                                    </EditItemTemplate>
                                              </asp:TemplateField>

                                             <asp:TemplateField>
                                                        <ItemStyle HorizontalAlign="Center" Width="5%" />
                                                        <ItemTemplate>
                                                            <asp:LinkButton ID="lnkView" runat="server" CommandName="Edit" class="btn btn-sm btn-info" CommandArgument='<%#Eval("sup_ID") %>'><i class="fa fa-edit"></i></asp:LinkButton>
                                                        </ItemTemplate>
                                                            <EditItemTemplate>
                                                                        <asp:Button ID="btn_Update" runat="server" Text="Update" CommandName="Update" ValidationGroup='<%# "Grp-" + Container.DataItemIndex %>'  class="btn btn-sm btn-primary"/>
                                                                        <asp:Button ID="btn_Cancel" runat="server" Text="Cancel" CommandName="Cancel"  class="btn btn-sm btn-warning"/>
                                                                    </EditItemTemplate>
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
                         <div class="loader"></div>
                         <br />
                         <asp:Label ID="Label2" runat="server" Text="Please wait" ForeColor="Red"></asp:Label>
                    </div>

    <!-- jQuery -->
    <script src="plugins/jquery/jquery.min.js"></script>
    <!-- Bootstrap 4 -->
    <script src="plugins/bootstrap/js/bootstrap.bundle.min.js"></script>

     <script>
         $(function () {
             $("#<%= gvItems.ClientID%>").DataTable({
                 "paging": true,
                 "lengthChange": true,
                 "searching": true,
                 "ordering": false,
                 "aLengthMenu": [[-1, 25, 50], ["All", 25, 50]],
                 "iDisplayLength": -1,
                 "bSortable": false,
                 "aTargets": [0],
                 "buttons": ["excel", "pdf", "print"],
                 "info": true,
                 "autoWidth": false,
                 "responsive": true,
             }).buttons().container().appendTo('#gvItems_wrapper .col-md-6:eq(0)');;
         });
    </script>

    <!-- ################################################# END #################################################### -->

    <script type="text/javascript">
        function ShowSuccessMsg() {
            $(function () {
                $("#messageDiv").dialog({
                    title: "Add Items",
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
