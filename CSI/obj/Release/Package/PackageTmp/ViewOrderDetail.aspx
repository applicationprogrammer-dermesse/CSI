<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true" CodeBehind="ViewOrderDetail.aspx.cs" Inherits="CSI.ViewOrderDetail" %>
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
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
 <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
         <section class="content-header">
        <div class="container-fluid">
            <div class="row mb-2">
                <div class="col-sm-6">
                    <h5>Online Order Detail</h5>
                </div>
                <div class="col-sm-6">
                    <ol class="breadcrumb float-sm-right">
                         
                        <li class="breadcrumb-item"><a href="ListOrderOnline.aspx" class="nav-link">Back to List</a></li>
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
                                     <div class="col-md-7 col-7">
                                         <div class="input-group" >
                                             <asp:Label ID="lblOrderDate" runat="server" Text="" ForeColor="Maroon"></asp:Label>

                                        </div>
                                     </div>
                                     <div class="col-md-3 col-3 text-right">
                                         
                                    </div>
                                     <div class="col-md-1 col-1 text-right">
                                         <div class="input-group" >
                                             <asp:Button ID="btnPost" runat="server" Text="P O S T"  CssClass="btn btn-sm btn-primary" ValidationGroup="grpPost" OnClick="btnPost_Click"/>

                                         </div>
                                     </div>

                                 </div>

                                <div style="margin-bottom: 10px" class="input-group">
                                     <asp:Label ID="Label2" runat="server" Text="Reference No." style="width:125px;"></asp:Label>
                                     <asp:Label ID="lblReferenceNo" runat="server" Text="" ForeColor="Maroon"></asp:Label>
                                 </div>

                                <div style="margin-bottom: 10px" class="input-group">
                                     <asp:Label ID="Label3" runat="server" Text="Source" style="width:125px;"></asp:Label>
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
                                     <asp:DropDownList ID="ddCourier" runat="server"  class="form-control" Style="width: 295px;" oninvalid="this.setCustomValidity('Please select item')" oninput="setCustomValidity('')" ></asp:DropDownList>
                                      <asp:RequiredFieldValidator ID="Req5" runat="server" ControlToValidate="ddCourier"  Display="Dynamic" InitialValue="0" ForeColor="Red" ValidationGroup="grpPost"
                                                                         ErrorMessage="Please select"></asp:RequiredFieldValidator>
                                     <asp:Label ID="Label14" runat="server" Text="Delivered By" ForeColor="Transparent" style="width:675px;"></asp:Label>
                                 </div>

                                <div style="margin-bottom: 20px" class="input-group">
                                     <asp:Label ID="Label15" runat="server" Text="Date Delivered" style="width:125px;"></asp:Label>
                                      <asp:TextBox ID="txtDateDelivered" runat="server" class="form-control txtDate" Width="105px"  onkeydown="return (event.keyCode!=13);" autocomplete="off" AutoCompleteType="Disabled"></asp:TextBox>
                                      <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtDateDelivered"
                                            Display="Dynamic" ForeColor="Red" ValidationGroup="grpPost"
                                             ErrorMessage="Required"></asp:RequiredFieldValidator>
                                     <asp:Label ID="Label16" runat="server" Text="Delivered By" ForeColor="Transparent" style="width:690px;"></asp:Label>
                                 </div>

                                 <div style="margin-bottom: 10px" class="input-group">
                                     <asp:Label ID="Label6" runat="server" Text="Category" ForeColor="Transparent" style="width:125px;"></asp:Label>
                                     <asp:Button ID="btnPrint" runat="server" Text="PRINT DR"  CssClass="btn btn-sm btn-success" OnClick="btnPrint_Click"/>
                                     
                                 </div>
                         </div>

                           <div class="card-body">

                            <asp:GridView ID="gvItems" CssClass="table table-bordered active" DataKeyNames="OrderID" AutoGenerateColumns="false" runat="server" >
                                                <Columns>
                                                    <asp:BoundField DataField="OrderID" HeaderText="ID" ReadOnly="true" ItemStyle-Width="3%" ItemStyle-HorizontalAlign="Center" ItemStyle-CssClass="hiddencol" HeaderStyle-CssClass="hiddencol"/>
                                                    <asp:BoundField DataField="RowNum" HeaderText="No" ReadOnly="true" ItemStyle-Width="5%" ItemStyle-HorizontalAlign="Center"/>
                                                    <asp:BoundField DataField="sup_ItemCode" HeaderText="Itemcode"  ReadOnly="true" ItemStyle-Width="10%" ItemStyle-HorizontalAlign="Center" />
                                                    <asp:BoundField DataField="sup_DESCRIPTION" HeaderText="Item Description"  ReadOnly="true" ItemStyle-Width="30%" ItemStyle-HorizontalAlign="Center" />
                                                    <asp:BoundField DataField="sup_UOM" HeaderText="UOM"  ReadOnly="true" ItemStyle-Width="5%" ItemStyle-HorizontalAlign="Center" />
                                                    <asp:BoundField DataField="vQtyPicked" HeaderText="Quantity" DataFormatString="{0:###0;(###0);0}"  ReadOnly="true" ItemStyle-Width="10%" ItemStyle-HorizontalAlign="Center" />
                                                    <asp:BoundField DataField="vUnitCost" HeaderText="vUnitCost" DataFormatString="{0:N2}"  ReadOnly="true" ItemStyle-Width="10%" ItemStyle-HorizontalAlign="Right" />
                                                    <asp:BoundField DataField="Amount" HeaderText="Amount" DataFormatString="{0:N2}"  ReadOnly="true" ItemStyle-Width="10%" ItemStyle-HorizontalAlign="Right" />
                                                </Columns>
                                        </asp:GridView>
                        </div>

                      </div>
                </div>
            </div>
        </div>
    </section>



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
                            window.location = '<%= ResolveUrl("~/ListOrderOnline.aspx") %>';
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
</asp:Content>

