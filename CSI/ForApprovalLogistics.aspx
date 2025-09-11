<%@ Page Title="For Approval - Logistics" Language="C#" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true" CodeBehind="ForApprovalLogistics.aspx.cs" Inherits="CSI.ForApprovalLogistics" %>
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
                    <h5>Request Detail</h5>
                </div>
                <div class="col-sm-6">
                    <ol class="breadcrumb float-sm-right">
                         
                        <li class="breadcrumb-item"><a href="ListOfForApprovalLogistics.aspx" class="nav-link">Back to List</a></li>
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

                                
                         </div>

                           <div class="card-body">

                            <asp:GridView ID="gvForApproval" runat="server" Width="100%"  class="table table-bordered table-hover" DataKeyNames="Sup_RequestID" AutoGenerateColumns="false" OnRowCancelingEdit="gvForApproval_RowCancelingEdit" OnRowDeleting="gvForApproval_RowDeleting" OnRowEditing="gvForApproval_RowEditing" OnRowUpdating="gvForApproval_RowUpdating">
                                <Columns>
                                    
                                    <asp:BoundField DataField="Sup_RequestID" HeaderText="No." ItemStyle-Width="5%" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"  ItemStyle-CssClass="hiddencol" HeaderStyle-CssClass="hiddencol">
                                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                        <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                    </asp:BoundField>

                                     

                                   <asp:BoundField DataField="Sup_ItemCode" HeaderText="Item Code" ItemStyle-Width="10%" ReadOnly="true" HeaderStyle-HorizontalAlign="Center">
                                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                    </asp:BoundField>

                                     

                                      <asp:BoundField DataField="sup_DESCRIPTION" HeaderText="Item Description" ItemStyle-Width="40%" ReadOnly="true" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                        <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                    </asp:BoundField>

                                    <asp:BoundField DataField="sup_UOM" HeaderText="U.O.M" ItemStyle-Width="5%" ReadOnly="true" HeaderStyle-HorizontalAlign="Center">
                                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                        <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                    </asp:BoundField>

                                    <asp:BoundField DataField="Sup_Balance" HeaderText="Balance<br/>(Branch/Dept.)"  HtmlEncode="false" ItemStyle-Width="10%" ReadOnly="true" HeaderStyle-HorizontalAlign="Center">
                                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                        <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                    </asp:BoundField>

                                   <asp:BoundField DataField="EndingBal" HeaderText="Balance<br/>(Logistics)" HtmlEncode="false" ItemStyle-Width="10%" ReadOnly="true" HeaderStyle-HorizontalAlign="Center">
                                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                        <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                    </asp:BoundField>
                                    
                                         <asp:TemplateField HeaderText="Quantity <br/>(Request)" ItemStyle-Width="10%" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblQuantity" runat="server" Text='<%#Eval("Sup_Qty","{0:###0;(###0);0}") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <EditItemTemplate>
                                                        <asp:TextBox ID="txtQuantity" runat="server"  class="form-control decimalnumbers-only" Text='<%#Eval("Sup_Qty","{0:###0;(###0);0}") %>' BackColor="#ffcc99" Style="text-align: center;"></asp:TextBox>
                                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator17" runat="server" ErrorMessage="Required" 
                                                            ControlToValidate="txtQuantity" ValidationGroup='<%# "Grp-" + Container.DataItemIndex %>'  ForeColor="Red" Display="Dynamic"></asp:RequiredFieldValidator>
                                                        <asp:RegularExpressionValidator ID="REV1" runat="server" ControlToValidate="txtQuantity" Display="Dynamic"
                                                            ErrorMessage="Only numeric allowed here!" ValidationExpression="^\d+(\.\d{1,4})?$"
                                                            ValidationGroup='<%# "Grp-" + Container.DataItemIndex %>'  ForeColor="Red"></asp:RegularExpressionValidator>
                                                    </EditItemTemplate>
                                              </asp:TemplateField>

                                    <asp:TemplateField>
                                        <ItemStyle HorizontalAlign="Center" Width="5%" />
                                        <ItemTemplate>
                                            <asp:LinkButton ID="lnkView" runat="server" CommandName="Edit" class="btn btn-sm btn-info" CommandArgument='<%#Eval("Sup_RequestID") %>'><i class="fa fa-edit"></i></asp:LinkButton>
                                        </ItemTemplate>
                                            <EditItemTemplate>
                                                        <asp:Button ID="btn_Update" runat="server" Text="Update" CommandName="Update" ValidationGroup='<%# "Grp-" + Container.DataItemIndex %>'  class="btn btn-sm btn-primary"/>
                                                        <asp:Button ID="btn_Cancel" runat="server" Text="Cancel" CommandName="Cancel"  class="btn btn-sm btn-warning"/>
                                                    </EditItemTemplate>
                                    </asp:TemplateField>

                                     <asp:TemplateField>
                                        <ItemStyle HorizontalAlign="Center" Width="5%" />
                                        <ItemTemplate>
                                            <asp:LinkButton ID="lnkDelete" runat="server" CommandName="Delete" class="btn btn-sm btn-danger" CommandArgument='<%#Eval("Sup_RequestID") %>'><i class="fa fa-trash"></i></asp:LinkButton>
                                        </ItemTemplate>
                                    </asp:TemplateField>

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
    <!-- ################################################# START #################################################### -->
    <script type="text/javascript">
        $(".decimalnumbers-only").keypress(function (e) {
            if (e.which == 46) {
                if ($(this).val().indexOf('.') != -1) {
                    return false;
                }
            }

            if (e.which != 8 && e.which != 0 && e.which != 46 && (e.which < 48 || e.which > 57)) {
                return false;
            }
        });


        var prm = Sys.WebForms.PageRequestManager.getInstance();
        if (prm != null) {
            prm.add_endRequest(function (sender, e) {
                if (sender._postBackSettings.panelsToUpdate != null) {
                    $(".decimalnumbers-only").keypress(function (e) {
                        if (e.which == 46) {
                            if ($(this).val().indexOf('.') != -1) {
                                return false;
                            }
                        }

                        if (e.which != 8 && e.which != 0 && e.which != 46 && (e.which < 48 || e.which > 57)) {
                            return false;
                        }
                    });
                }
            });
        };

    </script>
    <!-- ################################################# END #################################################### -->


        <!-- ################################################# END #################################################### -->

    <script type="text/javascript">
        function ShowMsgSuccessPosting() {
            $(function () {
                $("#messageSuccessPosting").dialog({
                    title: "Posting Approval",
                    width: '335px',
                    buttons: {
                        Close: function () {
                            window.location = '<%= ResolveUrl("~/ListOfForApprovalLogistics.aspx") %>';
                            $(this).dialog('close');
                        }
                    },
                    modal: true
                });
            });
        }
    </script>

    <div id="messageSuccessPosting" style="display: none;">
        <asp:UpdatePanel ID="UpdatePanel2" runat="server">
            <ContentTemplate>
                <asp:Label Text="" ID="lblMsgSuccessPosting" runat="server" />
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>

</asp:Content>

