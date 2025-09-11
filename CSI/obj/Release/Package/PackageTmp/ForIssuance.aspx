<%@ Page Title="CSI System | Issuance" MaintainScrollPositionOnPostback="true" Language="C#" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true" CodeBehind="ForIssuance.aspx.cs" Inherits="CSI.ForIssuance" %>
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
                    <h5>Issuance Detail</h5>
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
                                    

                                    <div class="col-sm-1 col-1 text-left">
                                        <asp:Label ID="Label4" runat="server" Text="Branch " style="width:125px;"></asp:Label>
                                        </div>
                                     <div class="col-md-7 col-7">
                                         <div class="input-group" >
                                            <asp:Label ID="lblBranchCode" runat="server" Text=""></asp:Label>
                                             &nbsp;&nbsp;-&nbsp;&nbsp;
                                             <asp:Label ID="lblBranch" runat="server" Text=""></asp:Label>

                                        </div>
                                     </div>
                                     <div class="col-md-3 col-3 text-right">
                                         <div class="input-group" >
                                             <asp:Label ID="Label1" runat="server" Text="Printed Issue Slip No. " ></asp:Label>
                                             &nbsp;&nbsp;&nbsp;
                                             <asp:TextBox ID="txtDRNumber" runat="server" CssClass="form-control"  AutoCompleteType="Disabled" onfocus="disableautocompletion(this.id);"></asp:TextBox>
                                          </div>
                                    </div>
                                     <div class="col-md-1 col-1 text-right">
                                         <div class="input-group" >
                                             <asp:Button ID="btnPost" runat="server" Text="SUBMIT"  CssClass="btn btn-sm btn-primary" OnClick="btnPost_Click" ValidationGroup="grpPost"/>
                                              <%--<button type="button" onserverclick="btnPost_Click" class="btn btn-sm btn-primary btn-block"><i class="fa fa-bell"></i> SUBMIT</button>--%>
                                         </div>
                                     </div>

                                 </div>

                                <div style="margin-bottom: 10px" class="input-group">
                                     <asp:Label ID="Label2" runat="server" Text="No." style="width:125px;"></asp:Label>
                                     <asp:Label ID="lblNo" runat="server" Text=""></asp:Label>
                                 </div>

                                <div style="margin-bottom: 10px" class="input-group">
                                     <asp:Label ID="Label3" runat="server" Text="Date Submit" style="width:125px;"></asp:Label>
                                     <asp:Label ID="lblDatePosted" runat="server" Text=""></asp:Label>
                                 </div>

                                <div style="margin-bottom: 10px" class="input-group">
                                     <asp:Label ID="Label5" runat="server" Text="Category" style="width:125px;"></asp:Label>
                                     <asp:Label ID="lblCategory" runat="server" Text=""></asp:Label>
                                 </div>

                                <div style="margin-bottom: 15px" class="input-group">
                                     <asp:Label ID="Label13" runat="server" Text="Type" style="width:125px;"></asp:Label>
                                     <asp:Label ID="lblType" runat="server" Text=""></asp:Label>
                                 </div>

                                <div style="margin-bottom: 10px" class="input-group">
                                     <asp:Label ID="Label7" runat="server" Text="Date Delivered" style="width:125px;"></asp:Label>
                                     <asp:TextBox ID="txtDateDeliverd" runat="server" CssClass="form-control txtDate"  AutoCompleteType="Disabled" onfocus="disableautocompletion(this.id);"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ForeColor="Red" Display="Dynamic"
                                          ValidationGroup="grpPost" ControlToValidate="txtDateDeliverd"
                                              ErrorMessage="Required Field"></asp:RequiredFieldValidator>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ForeColor="Red" Display="Dynamic"
                                          ValidationGroup="grpPrint" ControlToValidate="txtDateDeliverd"
                                              ErrorMessage="Required Field"></asp:RequiredFieldValidator>
                                    <asp:Label ID="Label9" runat="server" Text="Date Delivered" ForeColor="Transparent" style="width:900px;"></asp:Label>
                                 </div>

                                <div style="margin-bottom: 15px" class="input-group">
                                     <asp:Label ID="Label8" runat="server" Text="Delivered By" style="width:125px;"></asp:Label>
                                     <asp:TextBox ID="txtDeliveredBy" runat="server" CssClass="form-control" style="text-transform:uppercase;"></asp:TextBox>
                                     <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ForeColor="Red" Display="Dynamic"
                                          ValidationGroup="grpPost" ControlToValidate="txtDeliveredBy"
                                              ErrorMessage="Required Field"></asp:RequiredFieldValidator>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ForeColor="Red" Display="Dynamic"
                                          ValidationGroup="grpPrint" ControlToValidate="txtDeliveredBy"
                                              ErrorMessage="Required Field"></asp:RequiredFieldValidator>
                                     <asp:Label ID="Label10" runat="server" Text="Date Delivered" ForeColor="Transparent" style="width:800px;"></asp:Label>
                                 </div>

                                 <div style="margin-bottom: 10px" class="input-group">
                                     <asp:Label ID="Label6" runat="server" Text="Category" ForeColor="Transparent" style="width:125px;"></asp:Label>
                                     <asp:Button ID="btnPrint" runat="server" Text="PRINT"  CssClass="btn btn-sm btn-secondary" ValidationGroup="grpPrint" OnClick="btnPrint_Click"/>
                                     &nbsp;&nbsp;&nbsp;
                                     <asp:Button ID="PrintMultiple" runat="server" Text="PRINT MULTIPLE REQUEST"  CssClass="btn btn-sm btn-secondary" OnClick="PrintMultiple_Click"/>
                                     
                                 </div>
                         </div>

                           <div class="card-body">

                            <asp:GridView ID="gvForIssuance" runat="server" Width="100%"  class="table table-bordered table-hover" DataKeyNames="vRecNum" AutoGenerateColumns="false" OnRowCancelingEdit="gvForIssuance_RowCancelingEdit" OnRowDeleting="gvForIssuance_RowDeleting" OnRowEditing="gvForIssuance_RowEditing" OnRowUpdating="gvForIssuance_RowUpdating" OnRowCommand="gvForIssuance_RowCommand">
                                <Columns>
                                    
                                    <asp:BoundField DataField="OrderID" HeaderText="OrderID" ItemStyle-Width="3%" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"  ItemStyle-CssClass="hiddencol" HeaderStyle-CssClass="hiddencol">
                                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                        <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                    </asp:BoundField>

                                    <asp:BoundField DataField="vRecNum" HeaderText="No" ItemStyle-Width="3%" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"  ItemStyle-CssClass="hiddencol" HeaderStyle-CssClass="hiddencol">
                                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                        <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                    </asp:BoundField>
                                    
                                    <asp:BoundField DataField="HeaderID" HeaderText="HeaderID" ItemStyle-Width="3%" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"  ItemStyle-CssClass="hiddencol" HeaderStyle-CssClass="hiddencol">
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

                                    <asp:BoundField DataField="vBatchNo" HeaderText="Batch No" ItemStyle-Width="10%" ReadOnly="true" HeaderStyle-HorizontalAlign="Center">
                                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                        <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                    </asp:BoundField>

                                    <asp:BoundField DataField="vDateExpiry" HeaderText="Date Expiry" DataFormatString="{0:MM/dd/yyyy}" ItemStyle-Width="10%" ReadOnly="true" HeaderStyle-HorizontalAlign="Center">
                                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                        <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                    </asp:BoundField>

                                   
                                    
                                         <asp:TemplateField HeaderText="Quantity" ItemStyle-Width="10%" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblQuantity" runat="server" Text='<%#Eval("vQtyPicked","{0:###0;(###0);0}") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <EditItemTemplate>
                                                        <asp:TextBox ID="txtQuantity" runat="server"  class="form-control" Text='<%#Eval("vQtyPicked","{0:###0;(###0);0}") %>' BackColor="#ffcc99" Style="text-align: center;"></asp:TextBox>
                                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator17" runat="server" ErrorMessage="Required" 
                                                            ControlToValidate="txtQuantity" ValidationGroup='<%# "Grp-" + Container.DataItemIndex %>'  ForeColor="Red" Display="Dynamic"></asp:RequiredFieldValidator>
                                                        <asp:RegularExpressionValidator ID="REV1" runat="server" ControlToValidate="txtQuantity" Display="Dynamic"
                                                            ErrorMessage="Only numeric allowed here!" ValidationExpression="^\d+(\.\d{1,4})?$"
                                                            ValidationGroup='<%# "Grp-" + Container.DataItemIndex %>'  ForeColor="Red"></asp:RegularExpressionValidator>
                                                    </EditItemTemplate>
                                              </asp:TemplateField>

                                    <asp:TemplateField  HeaderText="Edit Qty">
                                        <ItemStyle HorizontalAlign="Center" Width="5%" />
                                        <ItemTemplate>
                                            <asp:LinkButton ID="lnkView" runat="server" CommandName="Edit" class="btn btn-sm btn-info" CommandArgument='<%#Eval("vRecNum") %>'><i class="fa fa-edit"></i></asp:LinkButton>
                                        </ItemTemplate>
                                            <EditItemTemplate>
                                                        <asp:Button ID="btn_Update" runat="server" Text="Update" CommandName="Update" ValidationGroup='<%# "Grp-" + Container.DataItemIndex %>'  class="btn btn-sm btn-primary"/>
                                                        <asp:Button ID="btn_Cancel" runat="server" Text="Cancel" CommandName="Cancel"  class="btn btn-sm btn-warning"/>
                                                    </EditItemTemplate>
                                    </asp:TemplateField>


                                    <asp:TemplateField HeaderText="Change Batch/Expiry">
                                        <ItemStyle HorizontalAlign="Center" Width="5%" />
                                        <ItemTemplate>
                                            <asp:LinkButton ID="lnkChangeItem" runat="server" CommandName="ChangeItem" ToolTip="ChangeItem" class="btn btn-sm btn-warning" CommandArgument='<%#Eval("vRecNum") %>'><i class="fas fa-archive"></i></asp:LinkButton>
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                     <asp:TemplateField HeaderText="Remove">
                                        <ItemStyle HorizontalAlign="Center" Width="5%" />
                                        <ItemTemplate>
                                            <asp:LinkButton ID="lnkDelete" runat="server" CommandName="Delete" class="btn btn-sm btn-danger" CommandArgument='<%#Eval("vRecNum") %>'><i class="fa fa-trash"></i></asp:LinkButton>
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


        <!-- ################################################# END #################################################### -->

    <script type="text/javascript">
        function ShowMsgSuccessPosting() {
            $(function () {
                $("#messageSuccessPosting").dialog({
                    title: "Posting Issuance",
                    width: '335px',
                    buttons: {
                        Close: function () {
                            window.location = '<%= ResolveUrl("~/ListForIssuance.aspx") %>';
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


    
<!-- ################################################# END #################################################### -->
<script type="text/javascript">
    $(document).ready(function () {
        $('.txtDate').datepicker({
            //dateFormat: "MM/dd/yyyy",
            minDate: new Date(2007, 6, 12),
            changeMonth: true,
            changeYear: true,
            yearRange: "-1:+0"
        });

    });
</script>

     <script type="text/javascript">
    function disableautocompletion(id) {
              var passwordControl = document.getElementById(id);
              passwordControl.setAttribute("autocomplete", "off");
          }

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
                    title: "Change Item Batch/Expiry",

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
                    Qty Picked :
                    <asp:Label ID="lblQtyToPick" runat="server" Text=""></asp:Label>
                    <br />
                    <asp:Label ID="lblItemCodeToPick" runat="server" Text=""></asp:Label>
                    <asp:Label ID="Label11" runat="server" Text=" - "></asp:Label>
                    <asp:Label ID="lblItemDescToPick" runat="server" Text=""></asp:Label>
                    
                    <br />
                    <%----%>
                    <asp:Label ID="lblOrderID" runat="server" Text="" ForeColor="Transparent"></asp:Label>
                    <asp:Label ID="lblHeaderID" runat="server" Text="" ForeColor="Transparent"></asp:Label>
                    <br />
                    

                <asp:Label ID="Label12" runat="server" Text="Please ensure that balance of batch to be selected is Greater or Equal to Qty Picked to avoid negative balance." ForeColor="Maroon"></asp:Label>
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

                        <asp:BoundField DataField="vSource"  HeaderText="Remarks" ReadOnly="True">
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

                       <%--     <asp:TemplateField HeaderText="QTY to Pick" ItemStyle-Width="70px" HeaderStyle-HorizontalAlign="Center">
                                    <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                    <ItemStyle HorizontalAlign="Center" />
                                    <ItemTemplate>
                                        <asp:TextBox ID="txtQtyPicked" runat="server" CssClass="form-control decimalnumbers-only" Width="65px" AutoCompleteType="Disabled" autocomplete="off"></asp:TextBox>
                                           <asp:RequiredFieldValidator ID="RequiredFieldValidator1" 
                                            runat="server" ControlToValidate="txtQtyPicked" 
                                            Display="Dynamic" ValidationGroup='<%# "Grp-" + Container.DataItemIndex %>' ForeColor="Red"
                                            ErrorMessage="Required"></asp:RequiredFieldValidator>
                                        <asp:CompareValidator ID="CompareValidator1" runat="server" ControlToCompare="txtBalance"
                                            ErrorMessage="Insufficient balance!"
                                            ControlToValidate="txtQtyPicked" Type="Double" Operator="LessThanEqual"
                                            Display="Dynamic" ValidationGroup='<%# "Grp-" + Container.DataItemIndex %>' ForeColor="Red"></asp:CompareValidator>
                                    </ItemTemplate>
                             </asp:TemplateField>--%>


                        <asp:TemplateField>
                            <ItemStyle HorizontalAlign="Center" Width="175px" />
                            <ItemTemplate>
                                <asp:Button ID="btnSelectItemID" runat="server" Text="Select Batch" CssClass="btn btn-success"  CommandName="Update"/>
                        
                            </ItemTemplate>
                        </asp:TemplateField>

                

                    </Columns>
                </asp:GridView>
            </div>

    </div>








    <!-- ################################################# END #################################################### -->
</asp:Content>
