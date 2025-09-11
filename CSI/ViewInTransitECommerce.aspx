<%@ Page Title="E-Commerce Detail" MaintainScrollPositionOnPostback="true" Language="C#" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true" CodeBehind="ViewInTransitECommerce.aspx.cs" Inherits="CSI.ViewInTransitECommerce" %>
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
        .loading {
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
                    <h5>Online Order Detail(In Transit)</h5>
                </div>
                <div class="col-sm-6">
                    <ol class="breadcrumb float-sm-right">
                         
                        <li class="breadcrumb-item"><a href="ListInTransitOrder.aspx" class="nav-link">Back to List</a></li>
                     </ol>
                </div>
            </div>
        </div>
    </section>


    <section class="content">
        <div class="row">
                  <div class="col-md-4">
                         <div class="card">
                            <div class="card-header">
                                    <h3 class="card-title">Order Detail</h3>
                              </div>
                             <div class="card-body">

                                 <div style="margin-bottom: 10px" class="input-group">
            
                                            <asp:Label ID="Label4" runat="server" Text="Order Date " style="width:125px;"></asp:Label>
                                             <asp:Label ID="lblOrderDate" runat="server" Text="" ForeColor="Maroon"></asp:Label>

                                        </div>

                                 <div style="margin-bottom: 10px" class="input-group">
                                     <asp:Label ID="Label3" runat="server" Text="Source" style="width:125px;"></asp:Label>
                                     <asp:Label ID="lblSource" runat="server" Text="" ForeColor="Maroon"></asp:Label>
                                 </div>

                                <div style="margin-bottom: 10px" class="input-group">
                                     <asp:Label ID="Label2" runat="server" Text="Reference No." style="width:125px;"></asp:Label>
                                     <asp:Label ID="lblReferenceNo" runat="server" Text="" ForeColor="Maroon"></asp:Label>
                                 </div>

            
                            </div>
                         </div>
                     </div>

                     <div class="col-md-4">
                         <div class="card">
                             <div class="card-header">
                                    <h3 class="card-title">Confirmed Delivered</h3>
                              </div>
                             <div class="card-body">
                                  <div style="margin-bottom: 30px" class="input-group">
                                        <asp:Label ID="Label1" runat="server" Text="Date" style="width:65px;"></asp:Label>
                                        <asp:TextBox ID="txtPostedDateDelivered" runat="server" class="form-control txtDate" Width="105px"  onkeydown="return (event.keyCode!=13);" autocomplete="off" AutoCompleteType="Disabled"></asp:TextBox>
                                   </div>
                                    <asp:Button ID="btnPost" runat="server" Text="P O S T"  CssClass="btn btn-sm btn-primary" Visible="false" ValidationGroup="grpPost" OnClientClick="if(Page_ClientValidate()) ShowProgress()" OnClick="btnPost_Click"/>
                                       <asp:LinkButton ID="lnkPost" runat="server" CssClass="btn btn-sm btn-primary" Text="P O S T" OnClick="lnkPost_Click" ValidationGroup="grpPost" OnClientClick="if(Page_ClientValidate()) ShowProgress()"></asp:LinkButton>
                                          <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtPostedDateDelivered"
                                                        Display="Dynamic" ForeColor="Red" ValidationGroup="grpPost"
                                                        ErrorMessage="Please supply date confirmed delivered"></asp:RequiredFieldValidator>
                                    <asp:RegularExpressionValidator ID="REVdate" runat="server" ControlToValidate="txtPostedDateDelivered"
                                                ValidationExpression="^([1-9]|0[1-9]|1[0-2])[- / .]([1-9]|0[1-9]|1[0-9]|2[0-9]|3[0-1])[- / .](1[9][0-9][0-9]|2[0][0-9][0-9])$"
                                                ForeColor="Red" Display="Dynamic"
                                                ErrorMessage="Invalid date format"
                                                ValidationGroup="grpPost"></asp:RegularExpressionValidator>
                                    
                             </div>
                           </div>
                       </div>
                     <div class="col-md-4">
                         <div class="card">
                             <div class="card-header">
                                    <h3 class="card-title">Cancelled</h3>
                              </div>
                             <div class="card-body">
                                   <div style="margin-bottom: 30px" class="input-group">
                                     <asp:Label ID="Label5" runat="server" Text="Date" style="width:65px;"></asp:Label>
                                    <asp:TextBox ID="txtCancelledDate" runat="server" class="form-control txtDate" Width="105px"  onkeydown="return (event.keyCode!=13);" autocomplete="off" AutoCompleteType="Disabled"></asp:TextBox>
                                    </div>
                                    <asp:Button ID="btnCancelled" runat="server" Visible="false" Text="CANCEL"  CssClass="btn btn-sm btn-danger" ValidationGroup="grpCancelled" OnClientClick="if(Page_ClientValidate()) ShowProgress()" OnClick="btnCancelled_Click" />
                                     <asp:LinkButton ID="lnkCancelled" runat="server" CssClass="btn btn-sm btn-danger" Text="CANCEL" ValidationGroup="grpCancelled" OnClientClick="if(Page_ClientValidate()) ShowProgress()" OnClick="lnkCancelled_Click"></asp:LinkButton>
                                          <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="txtCancelledDate"
                                                        Display="Dynamic" ForeColor="Red" ValidationGroup="grpCancelled"
                                                        ErrorMessage="Please supply date cancelled"></asp:RequiredFieldValidator>
                                        <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ControlToValidate="txtCancelledDate"
                                                ValidationExpression="^([1-9]|0[1-9]|1[0-2])[- / .]([1-9]|0[1-9]|1[0-9]|2[0-9]|3[0-1])[- / .](1[9][0-9][0-9]|2[0][0-9][0-9])$"
                                                ForeColor="Red" Display="Dynamic"
                                                ErrorMessage="Invalid date format"
                                                ValidationGroup="grpCancelled"></asp:RegularExpressionValidator>
                                    
                             </div>
                           </div>
                       </div>
                
           </div>

                  <div class="card-body">
                       <div class="col-sm-12">
                            <div style="margin-bottom: 5px" class="input-group">
                                    <div class="col-md-2 col-2 text-right">
                                        <asp:Label ID="Label11" runat="server" Text="Add More Item" Style="width: 125px;"></asp:Label>
                                    </div>
                                    <div class="col-lg-6 col-6">
                                        <div class="input-group">
                                            <asp:DropDownList ID="ddITemToAdd" runat="server" CssClass="form-control chosen-select" required="required" AutoPostBack="True" OnSelectedIndexChanged="ddITemToAdd_SelectedIndexChanged"></asp:DropDownList>

                                        </div>
                                    </div>
                                    <div class="col-sm-4 col-4">
                                        <div class="input-group">
                                            <%--<asp:Button ID="btnShow" runat="server" Text="Show Batches" ValidationGroup="grpShow" CssClass="btn btn-sm btn-secondary" OnClick="btnShow_Click" />
                                            <asp:RequiredFieldValidator ID="Req5" runat="server" ControlToValidate="ddItem" Display="Dynamic" InitialValue="0" ForeColor="Red" ValidationGroup="grpShow"
                                                ErrorMessage="Please select item"></asp:RequiredFieldValidator>--%>
                                        </div>
                                    </div>
                                </div>
                         </div>


                            <asp:GridView ID="gvItems" CssClass="table table-bordered active" DataKeyNames="OrderID" AutoGenerateColumns="false" runat="server" OnRowCancelingEdit="gvItems_RowCancelingEdit" OnRowEditing="gvItems_RowEditing" OnRowUpdating="gvItems_RowUpdating" OnRowCommand="gvItems_RowCommand" OnRowDeleting="gvItems_RowDeleting" >
                                                <Columns>
                                                    <asp:BoundField DataField="OrderID" HeaderText="ID" ReadOnly="true" ItemStyle-Width="3%" ItemStyle-HorizontalAlign="Center" ItemStyle-CssClass="hiddencol" HeaderStyle-CssClass="hiddencol"/>
                                                    <asp:BoundField DataField="HeaderID" HeaderText="hID" ReadOnly="true" ItemStyle-Width="3%" ItemStyle-HorizontalAlign="Center" ItemStyle-CssClass="hiddencol" HeaderStyle-CssClass="hiddencol"/>
                                                    <asp:BoundField DataField="RowNum" HeaderText="No" ReadOnly="true" ItemStyle-Width="5%" ItemStyle-HorizontalAlign="Center"/>
                                                    <asp:BoundField DataField="sup_ItemCode" HeaderText="Itemcode"  ReadOnly="true" ItemStyle-Width="10%" ItemStyle-HorizontalAlign="Center" />
                                                    <asp:BoundField DataField="sup_DESCRIPTION" HeaderText="Item Description"  ReadOnly="true" ItemStyle-Width="30%" ItemStyle-HorizontalAlign="Center" />
                                                    <asp:BoundField DataField="sup_UOM" HeaderText="UOM"  ReadOnly="true" ItemStyle-Width="5%" ItemStyle-HorizontalAlign="Center" />
                                                    <%--<asp:BoundField DataField="vQtyPicked" HeaderText="Quantity" DataFormatString="{0:###0;(###0);0}"  ReadOnly="true" ItemStyle-Width="10%" ItemStyle-HorizontalAlign="Center" />--%>
                                                    <%--<asp:BoundField DataField="vUnitCost" HeaderText="SRP" DataFormatString="{0:N2}"  ReadOnly="true" ItemStyle-Width="10%" ItemStyle-HorizontalAlign="Right" />--%>

                                                    <asp:TemplateField HeaderText="SRP" ItemStyle-Width="10%" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Right">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblSRP" runat="server" Text='<%#Eval("vUnitCost","{0:N2}") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <EditItemTemplate>
                                                        <asp:TextBox ID="txtSRP" runat="server"  class="form-control decimalnumbers-only" Text='<%#Eval("vUnitCost","{0:N2}") %>' BackColor="#ffcc99" Style="text-align: center;"></asp:TextBox>
                                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1117" runat="server" ErrorMessage="Required" 
                                                            ControlToValidate="txtSRP" ValidationGroup='<%# "Grp-" + Container.DataItemIndex %>'  ForeColor="Red" Display="Dynamic"></asp:RequiredFieldValidator>
                                                        <asp:RegularExpressionValidator ID="REV111" runat="server" ControlToValidate="txtSRP" Display="Dynamic"
                                                            ErrorMessage="Only numeric allowed here!" ValidationExpression="^\d+(\.\d{1,4})?$"
                                                            ValidationGroup='<%# "Grp-" + Container.DataItemIndex %>'  ForeColor="Red"></asp:RegularExpressionValidator>
                                                    </EditItemTemplate>
                                              </asp:TemplateField>

                                                    

                                                  <asp:TemplateField HeaderText="Quantity" ItemStyle-Width="10%" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblQuantity" runat="server" Text='<%#Eval("vQtyPicked","{0:###0;(###0);0}") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <EditItemTemplate>
                                                        <asp:Label ID="lblOrigQuantity" runat="server" Text='<%#Eval("vQtyPicked","{0:###0;(###0);0}") %>'></asp:Label>
                                                        <asp:TextBox ID="txtQuantity" runat="server"  class="form-control decimalnumbers-only" Text='<%#Eval("vQtyPicked","{0:###0;(###0);0}") %>' BackColor="#ffcc99" Style="text-align: center;"></asp:TextBox>
                                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator17" runat="server" ErrorMessage="Required" 
                                                            ControlToValidate="txtQuantity" ValidationGroup='<%# "Grp-" + Container.DataItemIndex %>'  ForeColor="Red" Display="Dynamic"></asp:RequiredFieldValidator>
                                                        <asp:RegularExpressionValidator ID="REV1" runat="server" ControlToValidate="txtQuantity" Display="Dynamic"
                                                            ErrorMessage="Only numeric allowed here!" ValidationExpression="^\d+(\.\d{1,4})?$"
                                                            ValidationGroup='<%# "Grp-" + Container.DataItemIndex %>'  ForeColor="Red"></asp:RegularExpressionValidator>
                                                    </EditItemTemplate>
                                              </asp:TemplateField>

                                                    <asp:BoundField DataField="Amount" HeaderText="Total Amount" DataFormatString="{0:N2}"  ReadOnly="true" ItemStyle-Width="10%" ItemStyle-HorizontalAlign="Right" />

                                                    <asp:TemplateField  HeaderText="Edit">
                                                        <ItemStyle HorizontalAlign="Center" Width="5%" />
                                                        <ItemTemplate>
                                                            <asp:LinkButton ID="lnkView" runat="server" CommandName="Edit" class="btn btn-sm btn-info" CommandArgument='<%#Eval("OrderID") %>'><i class="fa fa-edit"></i></asp:LinkButton>
                                                        </ItemTemplate>
                                                            <EditItemTemplate>
                                                                        <%--<asp:Button ID="btn_Update" runat="server" Text="Update" CommandName="Update" ValidationGroup='<%# "Grp-" + Container.DataItemIndex %>'  class="btn btn-sm btn-primary"/>--%>
                                                                        <%--<asp:Button ID="btn_Cancel" runat="server" Text="Cancel" CommandName="Cancel"  class="btn btn-sm btn-warning"/>--%>
                                                                            <asp:LinkButton ID="lnkUpdate" runat="server" CommandName="Update" class="btn btn-sm btn-primary" ValidationGroup='<%# "Grp-" + Container.DataItemIndex %>' CommandArgument='<%#Eval("OrderID") %>'>UPDATE</asp:LinkButton>
                                                                            <asp:LinkButton ID="lnkCancel" runat="server" CommandName="Cancel" class="btn btn-sm btn-warning" CommandArgument='<%#Eval("OrderID") %>'>CANCEL</asp:LinkButton>
                                                                            <asp:LinkButton ID="lnkDelete" runat="server" CommandName="Delete" class="btn btn-sm btn-danger" CommandArgument='<%#Eval("OrderID") %>'>DELETE</asp:LinkButton>
                                                                    </EditItemTemplate>
                                                    </asp:TemplateField>

                                                    <asp:TemplateField HeaderText="Change Item">
                                                        <ItemStyle HorizontalAlign="Center" Width="5%" />
                                                        <ItemTemplate>
                                                            <asp:LinkButton ID="lnkChangeItem" runat="server" CommandName="ChangeItem" ToolTip="ChangeItem" class="btn btn-sm btn-warning" CommandArgument='<%#Eval("OrderID") %>'><i class="fas fa-archive"></i></asp:LinkButton>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                </Columns>
                                        </asp:GridView>
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



<!-- ################################################# END #################################################### -->

      <!-- jQuery -->
    <script src="plugins/jquery/jquery.min.js"></script>
    <!-- Bootstrap 4 -->
    <script src="plugins/bootstrap/js/bootstrap.bundle.min.js"></script>

<!-- ################################################# END #################################################### -->

<script type="text/javascript">
    function ShowSuccessMsg() {
        $(function () {
            $("#messageDiv").dialog({
                title: "Online Order",
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
                    title: "E-Commerce",
                    width: '335px',
                    buttons: {
                        Close: function () {
                            window.location = '<%= ResolveUrl("~/ListInTransitOrder.aspx") %>';
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
            maxDate: new Date,
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
    <!-- ################################################# END #################################################### -->


<%--*******************************************************************************--%>
    <script type="text/javascript">
        function CloseGridItemToChange() {
            $(function (event, ui) {
                //location.reload();
                //e.preventDefault();
                $("#ShowItemToChange").dialog('close');
                
            });
        }
    </script>

    <script type="text/javascript">
        function ShowGridItemToChange() {
            $(function () {
                $("#ddItem").click(function (event) {
                    event.preventDefault();
                });

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
                    //close: function(event, ui) {
                    //location.reload();
                });
                $("#ShowItemToChange").parent().appendTo($("form:first"));
            });
        }
    </script>
    

    <div id="ShowItemToChange" style="display: none;">
        <asp:UpdatePanel ID="UpdatePanel1" runat="server" ChildrenAsTriggers="true">
            <ContentTemplate>
                <div style="overflow: auto; max-height: 400px;">
                    <asp:Label ID="Label10" runat="server" Text="Item to Change "></asp:Label>
                    <asp:Label ID="lblItemCodeToChange" runat="server" Text=""></asp:Label>
                    <asp:Label ID="Label9" runat="server" Text=" - "></asp:Label>
                    <asp:Label ID="lblItemDescToChange" runat="server" Text=""></asp:Label>

                    <br />
                    <%--ForeColor="Transparent"--%>
                    <asp:Label ID="lblOrderIDToChange" runat="server" Text="" ForeColor="Transparent"></asp:Label>
                    <br />
                    <asp:DropDownList ID="ddItem" runat="server" CssClass="form-control chosen-select" required="required" AutoPostBack="true" OnSelectedIndexChanged="ddItem_SelectedIndexChanged"></asp:DropDownList>
                    <br />

                    <asp:GridView ID="gvItemBatch" runat="server" AutoGenerateColumns="false" OnRowUpdating="gvItemBatch_RowUpdating" OnRowDataBound="gvItemBatch_RowDataBound" >
                        <Columns>
                            <asp:BoundField HeaderText="Rec No." DataField="HeaderID" ItemStyle-Width="95px">
                                <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                            </asp:BoundField>

                            <asp:BoundField DataField="vRecDate" DataFormatString="{0:MM/dd/yyyy}" HeaderText="Date Received" ReadOnly="True">
                                <HeaderStyle Width="115px" HorizontalAlign="Center" />
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:BoundField>

                            <asp:BoundField HeaderText="Batch No." DataField="vBatchNo" HtmlEncode="false" ItemStyle-Width="155px">
                                <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                            </asp:BoundField>

                            <asp:BoundField DataField="vDateExpiry" DataFormatString="{0:MM/dd/yyyy}" HeaderText="Date Expiry" ReadOnly="True">
                                <HeaderStyle Width="115px" HorizontalAlign="Center" />
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:BoundField>

                            <asp:BoundField DataField="vSource" HeaderText="Remarks" ReadOnly="True">
                                <HeaderStyle Width="205px" HorizontalAlign="Center" />
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:BoundField>



                            <asp:TemplateField HeaderText="Balance" ItemStyle-Width="85px" HeaderStyle-HorizontalAlign="Center">
                                <ItemTemplate>
                                    <asp:TextBox ID="txtBalance" runat="server" Width="85px" Text='<%# Bind("Balance","{0:###0;(###0);0}") %>' Enabled="false" BorderStyle="None" Style="text-align: center; background-color: transparent; border-width: 0px;"></asp:TextBox>
                                </ItemTemplate>
                                <HeaderStyle HorizontalAlign="Center" />
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="SRP" ItemStyle-Width="70px" HeaderStyle-HorizontalAlign="Center">
                                <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                <ItemStyle HorizontalAlign="Right" />
                                <ItemTemplate>
                                    <asp:TextBox ID="txtSRP" runat="server" Text='<%#Eval("sup_UnitCost","{0:N2}") %>' CssClass="form-control decimalnumbers-only" Width="95px" AutoCompleteType="Disabled" autocomplete="off" Style="text-align: right;"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator109"
                                        runat="server" ControlToValidate="txtSRP"
                                        Display="Dynamic" ValidationGroup='<%# "Grp-" + Container.DataItemIndex %>' ForeColor="Red"
                                        ErrorMessage="Required"></asp:RequiredFieldValidator>
                                </ItemTemplate>
                            </asp:TemplateField>


                            <asp:TemplateField>
                                <ItemStyle HorizontalAlign="Center" Width="175px" />
                                <ItemTemplate>
                            
                                    <%--<asp:Button ID="btnsELECT" runat="server" CommandName="Update" Text="Select Batch" OnClientClick = "CallButtonEvent()" />--%>
                                    <asp:LinkButton ID="lnkSelect" runat="server" CommandName="Update" Text="Select Batch" CssClass="btn btn-success"  CommandArgument='<%# Eval("HeaderID") %>' OnClientClick="return CloseGridItemToChange()" />

                                </ItemTemplate>
                            </asp:TemplateField>


                        </Columns>
                    </asp:GridView>

                    <br />

                    
                </div>
                </ContentTemplate>
        </asp:UpdatePanel>  
          
        <%--OnClientClick = "CallButtonEvent()"--%>
        <asp:Button ID="btnPostBack" runat="server" Text="CHANGE" OnClick="btnPostBack_Click" Visible="false" />
    </div>



<%--   <script type="text/javascript">
       function CallButtonEvent() {
           document.getElementById("<%=btnPostBack.ClientID %>").click();
    }
</script>--%>




    <!-- ################################################# END #################################################### -->

    <%--*******************************************************************************--%>
    <script type="text/javascript">
        function CloseGridItemBatchToAddd() {
            $(function () {
                $("#ShowItemBatchToAddd").dialog('close');
            });
        }
    </script>

    <script type="text/javascript">
        function ShowGridItemBatchToAddd() {
            $(function () {
                $("#ShowItemBatchToAddd").dialog({
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
                $("#ShowItemBatchToAddd").parent().appendTo($("form:first"));
            });
        }
    </script>


    <div id="ShowItemBatchToAddd" style="display: none;">
        <div style="overflow: auto; max-height: 400px;">
            <asp:Label ID="lblItemDesc" runat="server" Text=""></asp:Label>
            <br />
            <asp:GridView ID="gvItemBatchToAddd" runat="server" AutoGenerateColumns="false" OnRowUpdating="gvItemBatchToAddd_RowUpdating">
                <Columns>
                    <asp:BoundField HeaderText="Rec No." DataField="HeaderID" ItemStyle-Width="95px">
                        <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                        <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                    </asp:BoundField>

                    <asp:BoundField DataField="vRecDate" DataFormatString="{0:MM/dd/yyyy}" HeaderText="Date Received" ReadOnly="True">
                        <HeaderStyle Width="115px" HorizontalAlign="Center" />
                        <ItemStyle HorizontalAlign="Center" />
                    </asp:BoundField>

                    <asp:BoundField HeaderText="Batch No." DataField="vBatchNo" HtmlEncode="false" ItemStyle-Width="155px">
                        <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                        <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                    </asp:BoundField>

                    <asp:BoundField DataField="vDateExpiry" DataFormatString="{0:MM/dd/yyyy}" HeaderText="Date Expiry" ReadOnly="True">
                        <HeaderStyle Width="115px" HorizontalAlign="Center" />
                        <ItemStyle HorizontalAlign="Center" />
                    </asp:BoundField>

                    <asp:BoundField DataField="vSource" HeaderText="Remarks" ReadOnly="True">
                        <HeaderStyle Width="205px" HorizontalAlign="Center" />
                        <ItemStyle HorizontalAlign="Center" />
                    </asp:BoundField>



                    <asp:TemplateField HeaderText="Balance" ItemStyle-Width="85px" HeaderStyle-HorizontalAlign="Center">
                        <ItemTemplate>
                            <asp:TextBox ID="txtBalance" runat="server" Width="85px" Text='<%# Bind("Balance","{0:###0;(###0);0}") %>' Enabled="false" BorderStyle="None" Style="text-align: center; background-color: transparent; border-width: 0px;"></asp:TextBox>
                        </ItemTemplate>
                        <HeaderStyle HorizontalAlign="Center" />
                        <ItemStyle HorizontalAlign="Center" />
                    </asp:TemplateField>

                    <asp:TemplateField HeaderText="Quantity" ItemStyle-Width="50px" HeaderStyle-HorizontalAlign="Center">
                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                        <ItemStyle HorizontalAlign="Center" />
                        <ItemTemplate>
                            <asp:TextBox ID="txtQtyPickedToAdd" runat="server" CssClass="form-control decimalnumbers-only" Width="50px" AutoCompleteType="Disabled" autocomplete="off"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator9"
                                runat="server" ControlToValidate="txtQtyPickedToAdd"
                                Display="Dynamic" ValidationGroup='<%# "Grp-" + Container.DataItemIndex %>' ForeColor="Red"
                                ErrorMessage="Required"></asp:RequiredFieldValidator>
                            <asp:CompareValidator ID="CompareValidator1" runat="server" ControlToCompare="txtBalance"
                                ErrorMessage="Insufficient balance!"
                                ControlToValidate="txtQtyPickedToAdd" Type="Double" Operator="LessThanEqual"
                                Display="Dynamic" ValidationGroup='<%# "Grp-" + Container.DataItemIndex %>' ForeColor="Red"></asp:CompareValidator>
                        </ItemTemplate>
                    </asp:TemplateField>

                    <asp:TemplateField HeaderText="SRP" ItemStyle-Width="70px" HeaderStyle-HorizontalAlign="Center">
                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                        <ItemStyle HorizontalAlign="Right" />
                        <ItemTemplate>
                            <asp:TextBox ID="txtSRPToAdd" runat="server" Text='<%#Eval("sup_UnitCost","{0:N2}") %>' CssClass="form-control decimalnumbers-only" Width="95px" AutoCompleteType="Disabled" autocomplete="off" style="text-align:right;"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator109"
                                runat="server" ControlToValidate="txtSRPToAdd"
                                Display="Dynamic" ValidationGroup='<%# "Grp-" + Container.DataItemIndex %>' ForeColor="Red"
                                ErrorMessage="Required"></asp:RequiredFieldValidator>
                        </ItemTemplate>
                    </asp:TemplateField>


                    <asp:TemplateField>
                        <ItemStyle HorizontalAlign="Center" Width="175px" />
                        <ItemTemplate>
                            <%--<asp:Button ID="btnSelectItemID" runat="server" Text="Select Batch" CssClass="btn btn-success" ValidationGroup='<%# "Grp-" + Container.DataItemIndex %>' CommandName="Update" />--%>
                            <asp:LinkButton ID="lnkAddedITem" runat="server" CommandName="Update" class="btn btn-sm btn-info" ValidationGroup='<%# "Grp-" + Container.DataItemIndex %>' CommandArgument='<%#Eval("HeaderID") %>'><i class="fa fa-plus"></i>ADD</asp:LinkButton>
                        </ItemTemplate>
                    </asp:TemplateField>



                </Columns>
            </asp:GridView>
        </div>

    </div>

    <!-- ################################################# START #################################################### -->

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

</asp:Content>

