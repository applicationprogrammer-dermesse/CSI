<%@ Page Title="In-Transit Order Posting" MaintainScrollPositionOnPostback="true" Language="C#" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true" CodeBehind="ViewInTransitOrderDetail.aspx.cs" Inherits="CSI.ViewInTransitOrderDetail" %>
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
        <div class="container-fluid">
            <div class="row">
                <div class="col-12">

                         <div class="card">
                            <div class="card-header">
                                 <div style="margin-bottom: 10px" class="input-group">
                                    

                                    <div class="col-sm-1 col-1 text-left">
                                        <asp:Label ID="Label4" runat="server" Text="Order Date " style="width:125px;"></asp:Label>
                                        </div>
                                     <div class="col-md-5 col-5">
                                         <div class="input-group" >
                                             <asp:Label ID="lblOrderDate" runat="server" Text="" ForeColor="Maroon"></asp:Label>

                                        </div>
                                     </div>
                                     <div class="col-md-5 col-5 text-right">
                                         <div class="input-group" >
                                                    
                                                <asp:Label ID="Label14" runat="server" Text="Confirmed Delivered Date&nbsp;&nbsp;&nbsp; " style="width:225px;"></asp:Label>
                                             <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtPostedDateDelivered"
                                                        Display="Dynamic" ForeColor="Red" ValidationGroup="grpPost"
                                                        ErrorMessage="Required"></asp:RequiredFieldValidator>
                                                <asp:TextBox ID="txtPostedDateDelivered" runat="server" class="form-control txtDate" Width="105px"  onkeydown="return (event.keyCode!=13);" autocomplete="off" AutoCompleteType="Disabled"></asp:TextBox>
                                        </div>
                                    </div>
                                     <div class="col-md-1 col-1 text-right">
                                         <div class="input-group" >
                                             <asp:Button ID="btnPost" runat="server" Text="P O S T"  CssClass="btn btn-sm btn-primary" ValidationGroup="grpPost" OnClientClick="if(Page_ClientValidate()) ShowProgress()" OnClick="btnPost_Click"/>

                                         </div>
                                     </div>

                                 </div>

                                <div style="margin-bottom: 10px" class="input-group">
                                     <asp:Label ID="Label2" runat="server" Text="Reference No." style="width:125px;"></asp:Label>
                                     <asp:Label ID="lblReferenceNo" runat="server" Text="" ForeColor="Maroon"></asp:Label>
                                 </div>

                                <div style="margin-bottom: 10px" class="input-group">
                                     <asp:Label ID="Label3" runat="server" Text="Platform" style="width:125px;"></asp:Label>
                                     <asp:Label ID="lblSource" runat="server" Text="" ForeColor="Maroon"></asp:Label>
                                 </div>

                                 <div style="margin-bottom: 15px" class="input-group">
                                     <asp:Label ID="Label1" runat="server" Text="Type" style="width:125px;"></asp:Label>
                                     <asp:Label ID="lblType" runat="server" Text="" ForeColor="Maroon"></asp:Label>
                                 </div>

                                <div style="margin-bottom: 10px" class="input-group">
                                     <asp:Label ID="Label5" runat="server" Text="Name" style="width:125px;"></asp:Label>
                                     <asp:Label ID="lblCustomerName" runat="server" Text="" ForeColor="Maroon"></asp:Label>
                                 </div>
                                
                                <div style="margin-bottom: 10px" class="input-group">
                                     <asp:Label ID="Label7" runat="server" Text="Address" style="width:125px;"></asp:Label>
                                     <asp:Label ID="lblCustomerAddress" runat="server" Text="" ForeColor="Maroon"></asp:Label>
                                 </div>

                                <div style="margin-bottom: 10px" class="input-group">
                                     <asp:Label ID="Label9" runat="server" Text="Contact No." style="width:125px;"></asp:Label>
                                     <asp:Label ID="lblContactNo" runat="server" Text="" ForeColor="Maroon"></asp:Label>
                                 </div>
               
                                <div style="margin-bottom: 10px" class="input-group">
                                     <asp:Label ID="Label8" runat="server" Text="Email" style="width:125px;"></asp:Label>
                                     <asp:Label ID="lblEmailAddress" runat="server" Text="" ForeColor="Maroon"></asp:Label>
                                 </div>

                                 <div style="margin-bottom: 10px" class="input-group">
                                     <asp:Label ID="Label10" runat="server" Text="M.O.P" style="width:125px;"></asp:Label>
                                     <asp:Label ID="lblMOP" runat="server" Text="" ForeColor="Maroon"></asp:Label>
                                 </div>

                                <div style="margin-bottom: 10px" class="input-group">
                                     <asp:Label ID="Label11" runat="server" Text="Del Instruction" style="width:125px;"></asp:Label>
                                     <asp:Label ID="lblDeliveryInstruction" runat="server" Text="" ForeColor="Maroon"></asp:Label>
                                 </div>

                                <div style="margin-bottom: 10px" class="input-group">
                                     <asp:Label ID="Label13" runat="server" Text="Shipping Fee" style="width:125px;"></asp:Label>
                                     <asp:Label ID="lblShippingFee" runat="server" Text="" ForeColor="Maroon"></asp:Label>
                                 </div>

                                <div style="margin-bottom: 10px" class="input-group">
                                     <asp:Label ID="Label12" runat="server" Text="Delivered By" style="width:125px;"></asp:Label>
                                     <asp:Label ID="lblDeliveredBy" runat="server" Text="" ForeColor="Maroon"></asp:Label>
                                 </div>

                                <div style="margin-bottom: 20px" class="input-group">
                                     <asp:Label ID="Label15" runat="server" Text="Date Delivered" style="width:125px;"></asp:Label>
                                      <asp:Label ID="lblDateDelivered" runat="server" Text="" ForeColor="Maroon"></asp:Label>
                                 </div>

                                 
                         </div>

                           <div class="card-body">

                            <asp:GridView ID="gvItems" CssClass="table table-bordered active" DataKeyNames="OrderID" AutoGenerateColumns="false" runat="server" OnRowCancelingEdit="gvItems_RowCancelingEdit" OnRowEditing="gvItems_RowEditing" OnRowUpdating="gvItems_RowUpdating" >
                                                <Columns>
                                                    <asp:BoundField DataField="OrderID" HeaderText="ID" ReadOnly="true" ItemStyle-Width="3%" ItemStyle-HorizontalAlign="Center" ItemStyle-CssClass="hiddencol" HeaderStyle-CssClass="hiddencol"/>
                                                    <asp:BoundField DataField="HeaderID" HeaderText="hID" ReadOnly="true" ItemStyle-Width="3%" ItemStyle-HorizontalAlign="Center" ItemStyle-CssClass="hiddencol" HeaderStyle-CssClass="hiddencol"/>
                                                    <asp:BoundField DataField="RowNum" HeaderText="No" ReadOnly="true" ItemStyle-Width="5%" ItemStyle-HorizontalAlign="Center"/>
                                                    <asp:BoundField DataField="sup_ItemCode" HeaderText="Itemcode"  ReadOnly="true" ItemStyle-Width="10%" ItemStyle-HorizontalAlign="Center" />
                                                    <asp:BoundField DataField="sup_DESCRIPTION" HeaderText="Item Description"  ReadOnly="true" ItemStyle-Width="30%" ItemStyle-HorizontalAlign="Center" />
                                                    <asp:BoundField DataField="sup_UOM" HeaderText="UOM"  ReadOnly="true" ItemStyle-Width="5%" ItemStyle-HorizontalAlign="Center" />
                                                    <%--<asp:BoundField DataField="vQtyPicked" HeaderText="Quantity" DataFormatString="{0:###0;(###0);0}"  ReadOnly="true" ItemStyle-Width="10%" ItemStyle-HorizontalAlign="Center" />--%>
                                                    <asp:BoundField DataField="vUnitCost" HeaderText="SRP" DataFormatString="{0:N2}"  ReadOnly="true" ItemStyle-Width="10%" ItemStyle-HorizontalAlign="Right" />
                                                    <asp:BoundField DataField="Amount" HeaderText="Amount" DataFormatString="{0:N2}"  ReadOnly="true" ItemStyle-Width="10%" ItemStyle-HorizontalAlign="Right" />
                                                           
                                                    
                                                    
                                            <asp:TemplateField HeaderText="Quantity" ItemStyle-Width="10%" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblQuantity" runat="server" Text='<%#Eval("vQtyPicked","{0:###0;(###0);0}") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <EditItemTemplate>
                                                        <asp:TextBox ID="txtQuantity" runat="server"  class="form-control decimalnumbers-only" Text='<%#Eval("vQtyPicked","{0:###0;(###0);0}") %>' BackColor="#ffcc99" Style="text-align: center;"></asp:TextBox>
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
                                                            <asp:LinkButton ID="lnkView" runat="server" CommandName="Edit" class="btn btn-sm btn-info" CommandArgument='<%#Eval("OrderID") %>'><i class="fa fa-edit"></i></asp:LinkButton>
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
                    title: "Online Order",
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
</asp:Content>

