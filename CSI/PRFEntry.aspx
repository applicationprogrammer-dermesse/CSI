<%@ Page Title="PRF Entry" Language="C#" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true" CodeBehind="PRFEntry.aspx.cs" Inherits="CSI.PRFEntry" %>
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
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
     <section class="content-header">
        <div class="container-fluid">
            <div class="row mb-2">
                <div class="col-sm-6">
                    <h5>PRF/Stock Transfer Data Entry</h5>
                </div>
                <div class="col-sm-6">
                    <ol class="breadcrumb float-sm-right">
                            <%--<li class="breadcrumb-item"><a href="Return.aspx" class="nav-link">Back to Customer List</a></li>--%>
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
                                               Branch/Department(where to transfer)  :
                                </div>
                                <div class="col-lg-5 col-5">
                                    <div class="input-group">
                                        <asp:DropDownList ID="ddBranch" runat="server" class="form-control" Style="width: 135px;  text-transform:uppercase;" >
                                        </asp:DropDownList>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator6" runat="server" ControlToValidate="ddBranch" InitialValue="0" ForeColor="Red" ValidationGroup="grpSave" Display="Dynamic"
                                                 ErrorMessage="Please select branch/department where to transfer"></asp:RequiredFieldValidator>
                                                    
                                    </div>
                                </div>
                            </div>

                               
                              <div style="margin-bottom: 5px" class="input-group">
                                    <div class="col-md-2 col-2 text-left">
                                        <asp:Label ID="Label3" runat="server" Text="Category" style="width:125px;"></asp:Label>
                                    </div>
                                     <div class="col-lg-5 col-5">
                                         <div class="input-group" >
                                             <asp:DropDownList ID="ddCategory" runat="server" CssClass="form-control" AutoPostBack="true" OnSelectedIndexChanged="ddCategory_SelectedIndexChanged"></asp:DropDownList>
                                             <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="ddCategory" InitialValue="0" ForeColor="Red" ValidationGroup="grpSave" Display="Dynamic"
                                                 ErrorMessage="Please select category"></asp:RequiredFieldValidator>
                                         </div>
                                     </div>
                                  <div class="col-lg-4 col-4">
                                  </div>
                                    <div class="col-lg-1 col-1 text-right">
                                         <div class="input-group" >
                                             <asp:Button ID="btnPost" runat="server" Text="SUBMIT" CssClass="btn btn-primary" OnClick="btnPost_Click" />
                                         </div>
                                     </div>
                             </div>

                             <div style="margin-bottom: 5px" class="input-group">
                                    <div class="col-md-2 col-2 text-left">
                                        <asp:Label ID="Label4" runat="server" Text="Item Description" style="width:125px;"></asp:Label>   
                                    </div>
                                     <div class="col-sm-6 col-6">
                                             <asp:DropDownList ID="ddItem" runat="server"  CssClass="form-control chosen-select" required="required" AutoPostBack="true" OnSelectedIndexChanged="ddItem_SelectedIndexChanged"></asp:DropDownList>
                                     </div>
                                    <div class="col-sm-4 col-4">
                                         <div class="input-group" >
                                             <asp:Button ID="btnDhow" runat="server" Text="Show Batches"  ValidationGroup="grpShow" CssClass="btn btn-sm btn-secondary" OnClick="btnDhow_Click" />
                                                          <asp:RequiredFieldValidator ID="Req4" runat="server" ControlToValidate="ddItem" Display="Dynamic" InitialValue="0" ForeColor="Red" ValidationGroup="grpSave"
                                                 ErrorMessage="Please select item"></asp:RequiredFieldValidator>
                                                  <asp:RequiredFieldValidator ID="Req5" runat="server" ControlToValidate="ddItem"  Display="Dynamic" InitialValue="0" ForeColor="Red" ValidationGroup="grpShow"
                                                 ErrorMessage="Please select item"></asp:RequiredFieldValidator>
                                        </div>
                                     </div>

                                    <%--<div class="col-sm-2 col-2">
                                         <div class="input-group" >
                                              
                                            </div>
                                     </div>--%>
                             </div>

                                <div style="margin-bottom: 5px" class="input-group">
                                    <div class="col-md-2 col-2 text-left">
                                        <asp:Label ID="Label1" runat="server" Text="Batch Number" style="width:125px;"></asp:Label>   
                                    </div>
                                     <div class="col-lg-2 col-2">
                                         <div class="input-group" >
                                             <asp:TextBox ID="txtBatchNumber" runat="server" CssClass="form-control" Width="155px" ReadOnly="true"></asp:TextBox>
                                          
                                         </div>
                                     </div>
                                    <div class="col-md-1 col-1">
                                           <asp:TextBox ID="txtID" runat="server" CssClass="form-control" Width="85px" Visible="false" ReadOnly="true"></asp:TextBox>
                                    </div>
                                    <div class="col-lg-1 col-1">
                                           <asp:Label ID="Label2" runat="server" Text="Date Expiry" style="width:125px;"></asp:Label>   
                                    </div>
                                    <div class="col-lg-2 col-2">
                                         <div class="input-group" >
                                             <asp:TextBox ID="txtDateExpiry" runat="server" CssClass="form-control" Width="115px" ReadOnly="true"></asp:TextBox>
                                         </div>
                                     </div>
                             </div>

                                

                              <div style="margin-bottom: 5px" class="input-group">
                                    <div class="col-md-2 col-2 text-left">
                                        <asp:Label ID="Label5" runat="server" Text="Balance" style="width:125px;"></asp:Label>   
                                    </div>
                                     <div class="col-lg-1 col-1">
                                         <div class="input-group" >
                                             <asp:TextBox ID="txtBalance" runat="server" CssClass="form-control" Width="105px" ReadOnly="true"></asp:TextBox>
                                                
                                         </div>
                                     </div>
                                  <div class="col-lg-2 col-2 text-left">
                                      <asp:CompareValidator ID="cvQty" runat="server"
                                                ErrorMessage="Insufficient balance!" ValidationGroup="grpSave" Display="Dynamic"
                                                ControlToCompare="txtBalance" ControlToValidate="txtQty" Type="Double" Operator="LessThanEqual"
                                                ForeColor="Red"></asp:CompareValidator>
                                  </div>
                             </div>

                                <div style="margin-bottom: 5px" class="input-group">
                                    <div class="col-md-2 col-2 text-left">
                                        <asp:Label ID="Label7" runat="server" Text="Transaction Date" style="width:125px;"></asp:Label>   
                                    </div>
                                     <div class="col-lg-3 col-3">
                                         <div class="input-group" >
                                             <asp:TextBox ID="txtTransactionDate" runat="server" CssClass="form-control txtDate" Width="105px" ValidationGroup="grpSave"  AutoCompleteType="Disabled"></asp:TextBox>
                                             <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="txtTransactionDate" ForeColor="Red" ValidationGroup="grpSave" Display="Dynamic"
                                                 ErrorMessage="Required Field"></asp:RequiredFieldValidator>
                                              <asp:RegularExpressionValidator ID="REVdate" runat="server" ControlToValidate="txtTransactionDate"
                                                ValidationExpression="^([1-9]|0[1-9]|1[0-2])[- / .]([1-9]|0[1-9]|1[0-9]|2[0-9]|3[0-1])[- / .](1[9][0-9][0-9]|2[0][0-9][0-9])$"
                                                ForeColor="Red" Display="Dynamic"
                                                ErrorMessage="Invalid date format"
                                                ValidationGroup="grpSave"></asp:RegularExpressionValidator>

                                         </div>
                                     </div>
                             </div>

                                <div style="margin-bottom: 5px" class="input-group">
                                    <div class="col-md-2 col-2 text-left">
                                        <asp:Label ID="Label8" runat="server" Text="PRF Number" style="width:125px;"></asp:Label>   
                                    </div>
                                     <div class="col-lg-3 col-3">
                                         <div class="input-group" >
                                             <asp:TextBox ID="txtPRFNumber" runat="server" CssClass="form-control" Width="105px" ValidationGroup="grpSave"  AutoCompleteType="Disabled"></asp:TextBox>
                                             <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ControlToValidate="txtPRFNumber" ForeColor="Red" ValidationGroup="grpSave" Display="Dynamic"
                                                 ErrorMessage="Required Field"></asp:RequiredFieldValidator>
                                              

                                         </div>
                                     </div>
                             </div>

                                <div style="margin-bottom: 5px" class="input-group">
                                    <div class="col-md-2 col-2 text-left">
                                        <asp:Label ID="Label9" runat="server" Text="Reason of Return" style="width:125px;"></asp:Label>   
                                    </div>
                                     <div class="col-lg-7 col-7">
                                         <div class="input-group" >
                                             <asp:TextBox ID="txtReason" runat="server" CssClass="form-control" MaxLength="150" Width="105px" ValidationGroup="grpSave"  AutoCompleteType="Disabled"></asp:TextBox>
                                             <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" ControlToValidate="txtReason" ForeColor="Red" ValidationGroup="grpSave" Display="Dynamic"
                                                 ErrorMessage="Required Field"></asp:RequiredFieldValidator>
                                              

                                         </div>
                                     </div>
                             </div>
                                

                             <div style="margin-bottom: 5px" class="input-group">
                                    <div class="col-md-2 col-2 text-left">
                                        <asp:Label ID="Label6" runat="server" Text="Quantity" style="width:125px;"></asp:Label>   
                                    </div>
                                     <div class="col-lg-5 col-5">
                                         <div class="input-group" >
                                             <asp:TextBox ID="txtQty" runat="server" CssClass="form-control decimalnumbers-only" Width="75px" ValidationGroup="grpSave"  AutoCompleteType="Disabled"></asp:TextBox>
                                             <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtQty" ForeColor="Red" ValidationGroup="grpSave" Display="Dynamic"
                                                 ErrorMessage="Required Field"></asp:RequiredFieldValidator>
                                           
                                         </div>
                                     </div>
                             </div>
                            <div style="margin-bottom: 15px" class="input-group">
                                    <div class="col-md-2 col-2 text-left">
                                        
                                    </div>
                                     <div class="col-lg-5 col-5">
                                         <div class="input-group" >
                                             <asp:Button ID="btnSave" runat="server" Text="S A V E"  ValidationGroup="grpSave" CssClass="btn btn-info" OnClick="btnSave_Click" />
                                         </div>
                                     </div>
                             </div>

                        </div>

                            <div class="card-body">
                                <div class="col-sm-12">
                <div class="col-sm-12 text-center">
                   
                               <asp:GridView ID="gvPicked" CssClass="table table-bordered active active" DataKeyNames="vRecNum" AutoGenerateColumns="false" runat="server" OnRowDeleting="gvPickedItems_RowDeleting">
                                    <Columns>
                                        <asp:BoundField DataField="vRecNum" HeaderText="ID" ItemStyle-Width="3%" ItemStyle-HorizontalAlign="Center" ItemStyle-CssClass="hiddencol" HeaderStyle-CssClass="hiddencol"/>
                                        <asp:BoundField DataField="HeaderID" HeaderText="HeaderID" ItemStyle-Width="3%" ItemStyle-HorizontalAlign="Center" ItemStyle-CssClass="hiddencol" HeaderStyle-CssClass="hiddencol"/>
                                        <asp:BoundField DataField="ReceiptNo" HeaderText="PRF No." ItemStyle-Width="14%" ItemStyle-HorizontalAlign="Center" />
                                        <asp:BoundField DataField="Sup_ItemCode" HeaderText="Item Code" ItemStyle-Width="5%" ItemStyle-HorizontalAlign="Center" />
                                        <asp:BoundField DataField="sup_DESCRIPTION" HeaderText="Item Description" ItemStyle-Width="20%" ItemStyle-HorizontalAlign="Center" />
                                        <asp:BoundField DataField="vQtyPicked" HeaderText="Qty" ItemStyle-Width="5%" DataFormatString="{0:###0;(###0);0}" ItemStyle-HorizontalAlign="Center" />
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

                                        <asp:BoundField DataField="Remarks" HeaderText="Reason of Return" ItemStyle-Width="10%" ItemStyle-HorizontalAlign="Center" />

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
                title: "Data Entry - PRF",
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
    

    <div id="ShowItemBatch" style="display:none;">
                <div style="overflow: auto; max-height: 400px;">

                <asp:GridView ID="gvItemBatch" runat="server" AutoGenerateColumns="false" OnRowUpdating="gvItemBatch_RowUpdating">
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

                        <asp:BoundField DataField="Balance"  HeaderText="Balance" ReadOnly="True">
                            <HeaderStyle Width="205px" HorizontalAlign="Center" />
                            <ItemStyle HorizontalAlign="Center"  />
                        </asp:BoundField>

                          <%--<asp:TemplateField HeaderText="Balance" ItemStyle-Width="85px" HeaderStyle-HorizontalAlign="Center">
                                    <ItemTemplate>
                                        <asp:TextBox ID="txtBalance" runat="server" Width="85px" Text='<%# Bind("Balance","{0:###0;(###0);0}") %>' Enabled="false" BorderStyle="None" Style="text-align: center; background-color: transparent; border-width: 0px;"></asp:TextBox>
                                    </ItemTemplate>
                                    <HeaderStyle HorizontalAlign="Center" />
                                    <ItemStyle HorizontalAlign="Center" />
                                </asp:TemplateField>--%>

                        <asp:TemplateField>
                            <ItemStyle HorizontalAlign="Center" Width="175px" />
                            <ItemTemplate>
                                <asp:Button ID="btnSelectItemID" runat="server" Text="Select Batch" CssClass="btn btn-success"  ValidationGroup='<%# "Grp-" + Container.DataItemIndex %>' CommandName="Update"/>
                        
                            </ItemTemplate>
                        </asp:TemplateField>

                

                    </Columns>
                </asp:GridView>
            </div>

    </div>






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

</asp:Content>
