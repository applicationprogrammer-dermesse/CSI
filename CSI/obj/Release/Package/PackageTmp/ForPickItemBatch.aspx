<%@ Page Title="CSI System | Pick Batch" MaintainScrollPositionOnPostback="true" Language="C#" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true" CodeBehind="ForPickItemBatch.aspx.cs" Inherits="CSI.ForPickItemBatch" %>

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
                    <h5>Request Detail for Pick Item Batch</h5>
                </div>
                <div class="col-sm-6">
                    <ol class="breadcrumb float-sm-right">

                        <li class="breadcrumb-item"><a href="ListOfApprovedRequest.aspx" class="nav-link">Back to List</a></li>
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
                                    <asp:Label ID="Label4" runat="server" Text="Branch " Style="width: 125px;"></asp:Label>
                                </div>
                                <div class="col-md-10 col-10">
                                    <div class="input-group">
                                        <asp:Label ID="lblBranchCode" runat="server" Text=""></asp:Label>
                                        &nbsp;&nbsp;-&nbsp;&nbsp;
                                            
                                        <asp:Label ID="lblBranch" runat="server" Text=""></asp:Label>
                                    </div>
                                </div>

                                <div class="col-md-1 col-1 text-right">
                                    <div class="input-group">
                                        
                                        <asp:LinkButton ID="LinkPost" runat="server" CssClass="btn btn-sm btn-primary" OnClick="LinkPost_Click">PICKED</asp:LinkButton>
                                        <asp:Button ID="btnPost" runat="server" Text="P O S T" CssClass="btn btn-sm btn-primary" OnClick="btnPost_Click" Visible="false" />
                                    </div>
                                </div>

                            </div>

                            <div style="margin-bottom: 10px" class="input-group">
                                <asp:Label ID="Label2" runat="server" Text="No." Style="width: 125px;"></asp:Label>
                                <asp:Label ID="lblNo" runat="server" Text=""></asp:Label>
                            </div>

                            <div style="margin-bottom: 10px" class="input-group">
                                <asp:Label ID="Label3" runat="server" Text="Date Submit" Style="width: 125px;"></asp:Label>
                                <asp:Label ID="lblDateSubmit" runat="server" Text=""></asp:Label>
                            </div>

                            <div style="margin-bottom: 10px" class="input-group">
                                <asp:Label ID="Label5" runat="server" Text="Category" Style="width: 125px;"></asp:Label>
                                <asp:Label ID="lblCategory" runat="server" Text=""></asp:Label>
                                <asp:Label ID="lblCategoryNum" runat="server" Text=""></asp:Label>
                            </div>

                            <div style="margin-bottom: 15px" class="input-group">
                                     <asp:Label ID="Label11" runat="server" Text="Type" style="width:125px;"></asp:Label>
                                     <asp:Label ID="lblType" runat="server" Text=""></asp:Label>
                                 </div>

                            <div style="margin-bottom: 10px" class="input-group">
                                <asp:Label ID="Label8" runat="server" Text="" Style="width: 125px;"></asp:Label>
                                <asp:Button ID="btnExcel" runat="server" Text="Export to Excel" CssClass="btn btn-sm btn-success" OnClick="btnExcel_Click"/>
                            </div>
                        </div>

                        <div class="card-body">

                            <asp:GridView ID="gvForPick" runat="server" Width="100%" class="table table-bordered table-hover" DataKeyNames="Sup_RequestID" AutoGenerateColumns="false" OnRowCancelingEdit="gvForPick_RowCancelingEdit" OnRowDeleting="gvForPick_RowDeleting" OnRowEditing="gvForPick_RowEditing" OnRowUpdating="gvForPick_RowUpdating" OnRowCommand="gvForPick_RowCommand">
                                <Columns>

                                    <asp:BoundField DataField="Sup_RequestID" HeaderText="No." ItemStyle-Width="5%" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" ItemStyle-CssClass="hiddencol" HeaderStyle-CssClass="hiddencol">
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

                                    <asp:BoundField DataField="Sup_Balance" HeaderText="Balance<br/>(Branch)"  HtmlEncode="false" ItemStyle-Width="10%" ReadOnly="true" HeaderStyle-HorizontalAlign="Center">
                                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                        <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                    </asp:BoundField>

                                    <asp:BoundField DataField="EndingBal" HeaderText="Balance<br/>(Logistics)" HtmlEncode="false" ItemStyle-Width="10%" ReadOnly="true" HeaderStyle-HorizontalAlign="Center">
                                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                        <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                    </asp:BoundField>
                                    


                                    <asp:TemplateField HeaderText="Quantity <br/> Request" ItemStyle-Width="10%" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                        <ItemTemplate>
                                            <asp:Label ID="lblQuantity" runat="server" Text='<%#Eval("Sup_Qty","{0:###0;(###0);0}") %>'></asp:Label>
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <asp:TextBox ID="txtQuantity" runat="server" class="form-control" Text='<%#Eval("Sup_Qty","{0:###0;(###0);0}") %>' BackColor="#ffcc99" Style="text-align: center;"></asp:TextBox>
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator117" runat="server" ErrorMessage="Required"
                                                ControlToValidate="txtQuantity" ValidationGroup='<%# "Grp2-" + Container.DataItemIndex %>' ForeColor="Red" Display="Dynamic"></asp:RequiredFieldValidator>
                                            <asp:RegularExpressionValidator ID="REV111" runat="server" ControlToValidate="txtQuantity" Display="Dynamic"
                                                ErrorMessage="Only numeric allowed here!" ValidationExpression="^\d+(\.\d{1,4})?$"
                                                ValidationGroup='<%# "Grp2-" + Container.DataItemIndex %>' ForeColor="Red"></asp:RegularExpressionValidator>
                                        </EditItemTemplate>
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="Select Item Batch">
                                        <ItemStyle HorizontalAlign="Center" Width="5%" />
                                        <ItemTemplate>
                                            <asp:LinkButton ID="lnkPick" runat="server" CommandName="SelectBatch" ToolTip="Select Batch" class="btn btn-sm btn-secondary"> <i class="far fa-file-alt"></i></asp:LinkButton>
                                        </ItemTemplate>
                                    </asp:TemplateField>


                                    <asp:TemplateField HeaderText="Edit Qty <br/>Request">
                                        <ItemStyle HorizontalAlign="Center" Width="5%"  />
                                        <ItemTemplate>
                                            <asp:LinkButton ID="lnkView" runat="server" CommandName="Edit" ToolTip="Edit Item" class="btn btn-sm btn-info" CommandArgument='<%#Eval("Sup_RequestID") %>'><i class="fa fa-edit"></i></asp:LinkButton>
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <%--<asp:Button ID="btn_Update" runat="server" Text="Update" CommandName="Update" ValidationGroup='<%# "Grp2-" + Container.DataItemIndex %>' class="btn btn-sm btn-primary" />--%>
                                            <asp:LinkButton ID="lnk_Update" runat="server" CommandName="Update" Text="Update" class="btn btn-sm btn-success" ValidationGroup='<%# "Grp2-" + Container.DataItemIndex %>' CommandArgument='<%#Eval("Sup_RequestID") %>'></asp:LinkButton>
                                            <%--<asp:Button ID="btn_Cancel" runat="server" Text="Cancel" CommandName="Cancel" class="btn btn-sm btn-warning" />--%>
                                            <asp:LinkButton ID="LinkCancel" runat="server" CommandName="Cancel" Text="Cancel" class="btn btn-sm btn-warning"></asp:LinkButton>
                                        </EditItemTemplate>
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="Change Item">
                                        <ItemStyle HorizontalAlign="Center" Width="5%" />
                                        <ItemTemplate>
                                            <asp:LinkButton ID="lnkChangeItem" runat="server" CommandName="ChangeItem" ToolTip="ChangeItem" class="btn btn-sm btn-warning" CommandArgument='<%#Eval("Sup_RequestID") %>'><i class="fas fa-archive"></i></asp:LinkButton>
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="Remove">
                                        <ItemStyle HorizontalAlign="Center" Width="5%" />
                                        <ItemTemplate>
                                            <asp:LinkButton ID="lnkDelete" runat="server" CommandName="Delete" ToolTip="Delete Item" class="btn btn-sm btn-danger" CommandArgument='<%#Eval("Sup_RequestID") %>'><i class="fa fa-trash"></i></asp:LinkButton>
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                    

                                </Columns>
                            </asp:GridView>

                            <br />

                            <div class="row" style="margin-bottom:15px;">
            <div class="col-sm-12">
                <div class="col-sm-12 text-center">
                     <asp:Label ID="Label1" runat="server" Visible="false" Text="Item Picked List"></asp:Label>
                </div>
            </div>

            <div class="col-sm-12">
                <div class="col-sm-12 text-center">
                   
                               <asp:GridView ID="gvPicked" CssClass="table table-bordered active active" DataKeyNames="vRecNum" AutoGenerateColumns="false" runat="server" OnRowDeleting="gvPickedItems_RowDeleting">
                                    <Columns>
                                        <asp:BoundField DataField="vRecNum" HeaderText="ID" ItemStyle-Width="3%" ItemStyle-HorizontalAlign="Center" ItemStyle-CssClass="hiddencol" HeaderStyle-CssClass="hiddencol"/>
                                        <asp:BoundField DataField="OrderID" HeaderText="OrderID" ItemStyle-Width="3%" ItemStyle-HorizontalAlign="Center" ItemStyle-CssClass="hiddencol" HeaderStyle-CssClass="hiddencol"/>
                                        <asp:BoundField DataField="Sup_ItemCode" HeaderText="Item Code" ItemStyle-Width="5%" ItemStyle-HorizontalAlign="Center" />
                                        <asp:BoundField DataField="sup_DESCRIPTION" HeaderText="Item Description" ItemStyle-Width="20%" ItemStyle-HorizontalAlign="Center" />
                                        <asp:BoundField DataField="vQtyPicked" HeaderText="Qty" ItemStyle-Width="5%" DataFormatString="{0:###0;(###0);0}" ItemStyle-HorizontalAlign="Center" />
                                        <asp:BoundField HeaderText="Batch No." DataField="vBatchNo"  HtmlEncode="false" ItemStyle-Width="10%">
                                               <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                                <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                            </asp:BoundField>
                                              <asp:BoundField DataField="vDateExpiry" DataFormatString="{0:MM/dd/yyyy}" HeaderText="Date Expiry" ReadOnly="True">
                                                <HeaderStyle Width="10%" HorizontalAlign="Center" />
                                                <ItemStyle HorizontalAlign="Center"  />
                                            </asp:BoundField>
                            
                                               <asp:TemplateField>
                                                    <ItemStyle HorizontalAlign="Center" Width="5%" />
                                                    <ItemTemplate>
                                                        <asp:LinkButton ID="btnDeletePicked" runat="server" CommandName="Delete" Text="" class="btn btn-sm btn-danger"><i class="fa fa-trash"></i></asp:LinkButton>
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
            </div>
        </div>
    </section>


    <%--<script src="external/jquery/jquery.js"></script>
    <script src="jquery-ui.js"></script>--%>
    <script src="plugins/jquery/jquery.js"></script>
    <script src="plugins/jquery-ui/jquery-ui.js"></script>
    <%--<script src="plugins/jquery/jquery.min.js"></script>
    <!-- Bootstrap 4 -->
<script src="plugins/bootstrap/js/bootstrap.bundle.min.js"></script>--%>








    <script type="text/javascript">
        function ShowSuccessMsg() {
            $(function () {
                $("#messageDiv").dialog({
                    title: "For Pick Item Batch",
                    width: '4   35px',
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
    function CloseGridItemQtyMatch() {
        $(function () {
            $("#ShowItemQtyMatch").dialog('close');
        });
    }
    </script>

    <script type="text/javascript">
        function ShowGridItemQtyMatch() {
            $(function () {
                $("#ShowItemQtyMatch").dialog({
                    title: "Item Qty Request and Pick dont match",
                    //position: ['center', 20],

                    width: '700px',
                    buttons: {
                        Close: function () {
                            $(this).dialog('close');
                        }
                    },
                    modal: true

                });
                $("#ShowItemQtyMatch").parent().appendTo($("form:first"));
            });
        }
    </script>

    <div id="ShowItemQtyMatch" style="display: none;">
        <div style="overflow:auto; max-height:400px;">
               <asp:Label Text="Quantity Order and Quantity Pick do not match" ID="Label6" runat="server"/>
               <br />
                <asp:GridView ID="gvItemQtyMatch" runat="server" AutoGenerateColumns="False">
                    <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                    <Columns>
                        <asp:BoundField HeaderText="Item Code" DataField="Sup_ItemCode" ItemStyle-Width="105px">
                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                            <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                        </asp:BoundField>
                        
                        <asp:BoundField HeaderText="Item Description" DataField="sup_DESCRIPTION" ItemStyle-Width="295px">
                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                            <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                        </asp:BoundField>

                        <asp:BoundField DataField="QtyOrder" DataFormatString="{0:###0;(###0);0}" HeaderText="Qty Order" ReadOnly="True">
                            <HeaderStyle Width="165px" HorizontalAlign="Center" />
                            <ItemStyle HorizontalAlign="Center"  />
                        </asp:BoundField>

                          <asp:BoundField DataField="QtyPicked" DataFormatString="{0:###0;(###0);0}" HeaderText="QtyPicked" ReadOnly="True">
                            <HeaderStyle Width="165px" HorizontalAlign="Center" />
                            <ItemStyle HorizontalAlign="Center"  />
                        </asp:BoundField>

                        <asp:BoundField DataField="Difference" DataFormatString="{0:###0;(###0);0}" HeaderText="Difference" ReadOnly="True">
                            <HeaderStyle Width="165px" HorizontalAlign="Center" />
                            <ItemStyle HorizontalAlign="Center"  />
                        </asp:BoundField>
                
                    </Columns>
                </asp:GridView>
        </div>
    
    </div>
  <!-- ################################################# END #################################################### -->

    <!-- ################################################# END #################################################### -->

    <script type="text/javascript">
        function ShowMsgSuccessPosting() {
            $(function () {
                $("#messageSuccessPosting").dialog({
                    title: "Posting - Approved Request",
                    width: '335px',
                    buttons: {
                        Close: function () {
                            window.location = '<%= ResolveUrl("~/ListOfApprovedRequest.aspx") %>';
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








<%--*******************************************************************************--%>
    <script type="text/javascript">
        function CloseGridItemToChange() {
            $(function () {
                $("#ShowItemToChange").dialog('close');
            });
        }
    </script>

    <script type="text/javascript">
        function ShowGridItemToChange() {
            $(function () {
                $("#ShowItemToChange").dialog({
                    title: "Item Description",

                    create: function (event, ui) {
                        $(event.target).parent().css('position', 'fixed');
                    },

                    width: '700px',
                    buttons: {
                        Close: function () {
                            $(this).dialog('close');
                        }
                    },
                    modal: true

                });
                $("#ShowItemToChange").parent().appendTo($("form:first"));
            });
        }
    </script>
    

    <div id="ShowItemToChange" style="display:none;">
                <div style="overflow: auto; max-height: 400px;">
                    <asp:Label ID="Label10" runat="server" Text="Item to Change "></asp:Label>
                    <asp:Label ID="lblItemCodeToChange" runat="server" Text=""></asp:Label>
                    <asp:Label ID="Label9" runat="server" Text=" - "></asp:Label>
                    <asp:Label ID="lblItemDescToChange" runat="server" Text=""></asp:Label>
                    
                    <br />
                    <%----%>
                    <asp:Label ID="lblOrderIDToChange" runat="server" Text="" ForeColor="Transparent"></asp:Label>
                    <br />
                    <asp:DropDownList ID="ddItem" runat="server"  CssClass="form-control chosen-select" required="required"></asp:DropDownList>
                    <asp:RequiredFieldValidator ID="RequiredItem" runat="server" ForeColor="Red" Display="Dynamic" InitialValue="0"
                                                      ValidationGroup="grpUpdate" ControlToValidate="ddItem"
                                                          ErrorMessage="Please select item"></asp:RequiredFieldValidator>
                    <br />
                    <asp:Button ID="btnUpdateItem" runat="server" Text="UPDATE" CssClass="btn btn-sm btn-primary" ValidationGroup="grpUpdate"  OnClick="btnUpdateItem_Click"/>                    


            </div>

    </div>








    <!-- ################################################# END #################################################### -->


    <%--*******************************************************************************--%>
    <script type="text/javascript">
        function CloseGridItemBatch() {
            $(function () {
                $("#ShowItemBatch").dialog('close');
            });
        }
    </script>

    <script type="text/javascript">
        function ShowGridItemBatch() {
            $(function () {
                $("#ShowItemBatch").dialog({
                    title: "Item Batch",

                    create: function (event, ui) {
                        $(event.target).parent().css('position', 'fixed');
                    },

                    width: '1000px',
                    buttons: {
                        Close: function () {
                            $(this).dialog('close');
                        }
                    },
                    modal: true

                });
                $("#ShowItemBatch").parent().appendTo($("form:first"));
            });
        }
    </script>
    
     <%--style="display:none;"--%>
    <div id="ShowItemBatch" style="display:none;">
                <div style="overflow: auto; max-height: 400px;">
                    Qty To Pick :
                    <asp:Label ID="lblQtyToPick" runat="server" Text=""></asp:Label>
                    <br />
                    <asp:Label ID="lblItemCodeToPick" runat="server" Text=""></asp:Label>
                    <asp:Label ID="Label7" runat="server" Text=" - "></asp:Label>
                    <asp:Label ID="lblItemDescToPick" runat="server" Text=""></asp:Label>
                    
                    <br />
                    <%----%>
                    <asp:Label ID="lblOrderID" runat="server" Text="" ForeColor="Transparent"></asp:Label>
                    <br />
                    


                <asp:GridView ID="gvItemBatch" runat="server" DataKeyNames="HeaderID" AutoGenerateColumns="false" OnRowCommand="gvItemBatch_RowCommand" >
                    <Columns>
                         <asp:BoundField HeaderText="Rec No." DataField="HeaderID" ItemStyle-Width="95px">
                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                            <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                        </asp:BoundField>
                        
                        <asp:BoundField DataField="vRecDate" DataFormatString="{0:MM/dd/yyyy}" HeaderText="Date Received" ReadOnly="True">
                            <HeaderStyle Width="115px" HorizontalAlign="Center" />
                            <ItemStyle HorizontalAlign="Center"  />
                        </asp:BoundField>

                           <asp:BoundField HeaderText="Batch No." DataField="vBatchNo"  HtmlEncode="false" ItemStyle-Width="155px">
                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                            <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                        </asp:BoundField>

                          <asp:BoundField DataField="vDateExpiry" DataFormatString="{0:MM/dd/yyyy}" HeaderText="Date Expiry" ReadOnly="True">
                            <HeaderStyle Width="115px" HorizontalAlign="Center" />
                            <ItemStyle HorizontalAlign="Center"  />
                        </asp:BoundField>

                        <asp:BoundField DataField="vSource"  HeaderText="Remarks" ReadOnly="True">
                            <HeaderStyle Width="205px" HorizontalAlign="Center" />
                            <ItemStyle HorizontalAlign="Center"  />
                        </asp:BoundField>

                        <asp:BoundField DataField="Category"  HeaderText="Category" ReadOnly="True">
                            <HeaderStyle Width="205px" HorizontalAlign="Center" />
                            <ItemStyle HorizontalAlign="Center"  />
                        </asp:BoundField>

                          <asp:TemplateField HeaderText="Balance" ItemStyle-Width="85px" HeaderStyle-HorizontalAlign="Center">
                                    <ItemTemplate>
                                        <asp:TextBox ID="txtBalance" runat="server" Width="85px" Text='<%# Bind("Balance","{0:###0;(###0);0}") %>' Enabled="false" BorderStyle="None" Style="text-align: center; background-color: transparent; border-width: 0px;"></asp:TextBox>
                                    </ItemTemplate>
                                    <HeaderStyle HorizontalAlign="Center" />
                                    <ItemStyle HorizontalAlign="Center" />
                                </asp:TemplateField>

                            <asp:TemplateField HeaderText="QTY to Pick" ItemStyle-Width="70px" HeaderStyle-HorizontalAlign="Center">
                                    <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                    <ItemStyle HorizontalAlign="Center" />
                                    <ItemTemplate>
                                        <asp:TextBox ID="txtQtyPicked" runat="server" CssClass="form-control decimalnumbers-only" Width="65px" AutoCompleteType="Disabled" autocomplete="off"></asp:TextBox>
                                           <asp:RequiredFieldValidator ID="RequiredFieldValidator1" 
                                            runat="server" ControlToValidate="txtQtyPicked" 
                                            Display="Dynamic" ValidationGroup='<%# "Grp1-" + Container.DataItemIndex %>' ForeColor="Red"
                                            ErrorMessage="Required"></asp:RequiredFieldValidator>
                                        <asp:CompareValidator ID="CompareValidator1" runat="server" ControlToCompare="txtBalance"
                                            ErrorMessage="Insufficient balance!"
                                            ControlToValidate="txtQtyPicked" Type="Double" Operator="LessThanEqual"
                                            Display="Dynamic" ValidationGroup='<%# "Grp1-" + Container.DataItemIndex %>' ForeColor="Red"></asp:CompareValidator>
                                    </ItemTemplate>
                             </asp:TemplateField>


                  <%--      <asp:TemplateField>
                            <ItemStyle HorizontalAlign="Center" Width="175px" />
                            <ItemTemplate>
                                <asp:Button ID="btnSelectItemID" runat="server" Text="Select Batch" CssClass="btn btn-success"  ValidationGroup='<%# "Grp1-" + Container.DataItemIndex %>' CommandName="Update"/>
                        
                            </ItemTemplate>
                        </asp:TemplateField>--%>

                        
                    <asp:TemplateField HeaderText="Select Batch">
                        <ItemStyle HorizontalAlign="Center" Width="10%" />
                        <ItemTemplate>
                            <asp:LinkButton ID="lnkSelectBatch" runat="server" CommandName="linkSelectBatch" Text="Select" class="btn btn-sm btn-success" ValidationGroup='<%# "Grp1-" + Container.DataItemIndex %>' CommandArgument='<%#Eval("HeaderID") %>'></asp:LinkButton>
                        </ItemTemplate>
                    </asp:TemplateField>                

                    </Columns>
                </asp:GridView>
            </div>

    </div>








    <!-- ################################################# END #################################################### -->


</asp:Content>
