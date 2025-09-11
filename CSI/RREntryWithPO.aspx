<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true" CodeBehind="RREntryWithPO.aspx.cs" Inherits="CSI.RREntryWithPO" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
     <link href="plugins/jquery-ui/jquery-ui.css" rel="stylesheet" />
    <link rel="stylesheet" href="docsupport/prism.css" />
    <link rel="stylesheet" href="chosen.css" />

       

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
                    <h5>RR with PO</h5>
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
                                        <asp:Label ID="Label1" runat="server" Text="RR Number" style="width:125px;"></asp:Label>   
                                    </div>
                                     <div class="col-lg-2 col-2">
                                             <asp:TextBox ID="txtRRNumber" runat="server" CssClass="form-control" Width="175px"  AutoCompleteType="Disabled"></asp:TextBox>
                                       </div>
                                        <div class="col-lg-2 col-2">
                                             <asp:Button ID="btnLoadDetail" runat="server" Text="L O A D" CssClass="btn btn-info"  ValidationGroup="grpLoad" OnClick="btnLoadDetail_Click" />
                                             &nbsp;
                                             <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ControlToValidate="txtRRNumber" 
                                                 ForeColor="Red" ValidationGroup="grpLoad" Display="Dynamic"
                                                 ErrorMessage="Required Field"></asp:RequiredFieldValidator>
                                           
                                         </div>
                                     
                                </div>
                                
                                <div style="margin-bottom: 5px" class="input-group">
                                    <div class="col-md-2 col-2 text-left">
                                        <asp:Label ID="Label2" runat="server" Text="Supplier"></asp:Label>   
                                    </div>
                                     <div class="col-lg-8 col-8">
                                         <div class="input-group" >
                                           <asp:Label ID="lblSupplier" runat="server" Text=""></asp:Label>   
                                         </div>
                                     </div>
                                </div>

                                <div style="margin-bottom: 5px" class="input-group">
                                    <div class="col-md-2 col-2 text-left">
                                        <asp:Label ID="Label3" runat="server" Text="PO Number" style="width:125px;"></asp:Label>   
                                    </div>
                                     <div class="col-lg-3 col-3">
                                         <div class="input-group" >
                                           <asp:Label ID="lblPONumber" runat="server" Text=""></asp:Label>   
                                         </div>
                                     </div>
                                </div>

                                <div style="margin-bottom: 5px" class="input-group">
                                    <div class="col-md-2 col-2 text-left">
                                        <asp:Label ID="Label4" runat="server" Text="RR Date" style="width:125px;"></asp:Label>   
                                    </div>
                                     <div class="col-lg-3 col-3">
                                         <div class="input-group" >
                                           <asp:Label ID="lblRRDate" runat="server" Text=""></asp:Label>   
                                         </div>
                                     </div>
                                </div>

                            </div>


                             <div class="card-body">

                            <asp:GridView ID="gvRR" runat="server" Width="100%"  class="table table-bordered table-hover" DataKeyNames="RRdetailID" AutoGenerateColumns="false" OnRowCancelingEdit="gvRR_RowCancelingEdit" OnRowCommand="gvRR_RowCommand" OnRowEditing="gvRR_RowEditing" OnRowUpdating="gvRR_RowUpdating" OnRowDataBound="gvRR_RowDataBound">
                                <Columns>
                                     <%--ItemStyle-CssClass="hiddencol" HeaderStyle-CssClass="hiddencol"--%>
                                    <asp:BoundField DataField="RRdetailID" HeaderText="ID" ItemStyle-Width="3%" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" ItemStyle-CssClass="hiddencol" HeaderStyle-CssClass="hiddencol">
                                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                        <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                    </asp:BoundField>

                           

                                    <asp:BoundField DataField="RRNo" HeaderText="RR No" ItemStyle-Width="5%" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" ReadOnly="true">
                                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                        <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                    </asp:BoundField> 

                                   <asp:BoundField DataField="RR_ItemCode" HeaderText="Purchasing </br>Item Code" HtmlEncode="false" ItemStyle-Width="8%" ReadOnly="true" HeaderStyle-HorizontalAlign="Center">
                                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                       <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                    </asp:BoundField>

                                     

                                      <asp:BoundField DataField="RR_ItemDescpt" HeaderText="Item Description" ItemStyle-Width="15%" ReadOnly="true" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                        <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                    </asp:BoundField>

                                  
                                    
                                         <asp:TemplateField HeaderText="Qty" ItemStyle-Width="5%" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblQuantity" runat="server" Text='<%#Eval("RR_QuantityDeliverd","{0:###0;(###0);0}") %>'></asp:Label>
                                                    </ItemTemplate>
                                         </asp:TemplateField>

                                        <asp:TemplateField HeaderText="SRP" ItemStyle-Width="7%" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Right">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblPO_UOP" runat="server" Text='<%#Eval("PO_UOP","{0:#,###0.#0}") %>'></asp:Label>
                                                    </ItemTemplate>
                                         </asp:TemplateField>

                                    <asp:TemplateField HeaderText="Total Amount" ItemStyle-Width="4%" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Right">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblRR_Amount" runat="server" Text='<%#Eval("RR_Amount","{0:#,###0.#0}") %>'></asp:Label>
                                                    </ItemTemplate>
                                         </asp:TemplateField>

                                    
						              <asp:TemplateField HeaderText="Batch No." ItemStyle-Width="5%" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="txtBatchNo" runat="server" CssClass="form-control" Width="105px"></asp:TextBox>
                                                    </ItemTemplate>
                                         </asp:TemplateField>

                                     <asp:TemplateField HeaderText="Date Expiry" ItemStyle-Width="4%" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                                    <ItemTemplate>
                                                            <asp:TextBox ID="txtDateExpiry" runat="server" CssClass="form-control txtExpiry" Width="105px" AutoCompleteType="Disabled"  autocomplete="off"></asp:TextBox>
                                                           <asp:RegularExpressionValidator ID="REVdate" runat="server" ControlToValidate="txtDateExpiry"
                                                                ValidationExpression="^([1-9]|0[1-9]|1[0-2])[- / .]([1-9]|0[1-9]|1[0-9]|2[0-9]|3[0-1])[- / .](1[9][0-9][0-9]|2[0][0-9][0-9])$"
                                                                ForeColor="Red" Display="Dynamic"
                                                                ErrorMessage="Invalid date format"
                                                                ValidationGroup='<%# "Grp-" + Container.DataItemIndex %>'></asp:RegularExpressionValidator>
                                                    </ItemTemplate>
                                         </asp:TemplateField>



                                    <asp:TemplateField HeaderText="DCI logistics Code" ItemStyle-Width="5%" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblCode" runat="server" Text='<%#Eval("ItemCode") %>'></asp:Label>
                                                    </ItemTemplate>
                                                <EditItemTemplate>
                                                        <asp:DropDownList ID="ddItem" runat="server" Width="275px" CssClass="form-control chosen-select"></asp:DropDownList>
                                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator17" runat="server" ErrorMessage="Required" InitialValue="0" 
                                                            ControlToValidate="ddItem" ValidationGroup='<%# "Grp2-" + Container.DataItemIndex %>'  ForeColor="Red" Display="Dynamic"></asp:RequiredFieldValidator>
                                                        
                                                    </EditItemTemplate>

                                        </asp:TemplateField>

                                    <asp:TemplateField HeaderText="Unit Cost" ItemStyle-Width="5%" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="txtUnitCost" runat="server" CssClass="form-control  decimalnumbers-only" Text='' Width="85px" AutoCompleteType="Disabled"  autocomplete="off"></asp:TextBox>
                                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1887" runat="server" ErrorMessage="Required"  
                                                            ControlToValidate="txtUnitCost" ValidationGroup='<%# "Grp2-" + Container.DataItemIndex %>'  ForeColor="Red" Display="Dynamic"></asp:RequiredFieldValidator>
                                                    </ItemTemplate>
                                         </asp:TemplateField>

                                    
                                    <asp:TemplateField HeaderText="Actual <br />Quantity<br/>Received" ItemStyle-Width="5%" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="txtActualQty" runat="server" CssClass="form-control  decimalnumbers-only" Text='<%#Eval("RR_QuantityDeliverd","{0:###0;(###0);0}") %>' Width="95px" AutoCompleteType="Disabled"  autocomplete="off"></asp:TextBox>
                                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator187" runat="server" ErrorMessage="Required"  
                                                            ControlToValidate="txtActualQty" ValidationGroup='<%# "Grp2-" + Container.DataItemIndex %>'  ForeColor="Red" Display="Dynamic"></asp:RequiredFieldValidator>
                                                    </ItemTemplate>
                                         </asp:TemplateField>


                                    <asp:TemplateField>
                                        <ItemStyle HorizontalAlign="Center" Width="5%" />
                                        <ItemTemplate>
                                            <asp:LinkButton ID="lnkView" runat="server" CommandName="Edit" class="btn btn-sm btn-info"><i class="fa fa-edit"></i></asp:LinkButton>
                                        </ItemTemplate>
                                            <EditItemTemplate>
                                                        <asp:Button ID="btn_Update" runat="server" Text="Update" CommandName="Update" ValidationGroup='<%# "Grp-" + Container.DataItemIndex %>'  class="btn btn-sm btn-primary"/>
                                                        <asp:Button ID="btn_Cancel" runat="server" Text="Cancel" CommandName="Cancel"  class="btn btn-sm btn-warning"/>
                                                    </EditItemTemplate>
                                    </asp:TemplateField>

                                     <asp:TemplateField>
                                        <ItemStyle HorizontalAlign="Center" Width="5%" />
                                        <ItemTemplate>
                                            <asp:LinkButton ID="lnkPOST" runat="server" CommandName="Post" Text="SUBMIT" ValidationGroup='<%# "Grp2-" + Container.DataItemIndex %>' OnClientClick="if(Page_ClientValidate()) ShowProgress()"  class="btn btn-sm btn-success"></asp:LinkButton>
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

          <div class="loading" align="center" style="margin-top:100px;">
                        <br />
                        <br />
                        <%--<img src="images/ajax-loader.gif" alt=""  />--%>
                                <div class="loader"></div>
                         <br />
                         <asp:Label ID="Label9" runat="server" Text="Please wait" ForeColor="Red"></asp:Label>
                         
                    </div>

          <!-- jQuery -->
    <script src="plugins/jquery/jquery.min.js"></script>
    <!-- Bootstrap 4 -->
    <script src="plugins/bootstrap/js/bootstrap.bundle.min.js"></script>


     <script src="docsupport/jquery-3.2.1.min.js" type="text/javascript"></script>
    <script src="chosen.jquery.js" type="text/javascript"></script>
    <script src="docsupport/prism.js" type="text/javascript" charset="utf-8"></script>
    <script src="docsupport/init.js" type="text/javascript" charset="utf-8"></script>

<!-- ################################################# END #################################################### -->

<script type="text/javascript">
    function ShowSuccessMsg() {
        $(function () {
            $("#messageDiv").dialog({
                title: "RR Entry WITH po",
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

        <script type="text/javascript">
            $(document).ready(function () {
                $('.txtExpiry').datepicker({
                    //dateFormat: "MM/dd/yyyy",
                    minDate: 0,
                    changeMonth: true,
                    changeYear: true,
                    yearRange: "-0:+5"
                });

            });
</script>


  <%--       <script>
             $(function () {
                 $("#<%=gvRR.ClientID%>").DataTable({
                 "responsive": true,
                 "lengthChange": false,
                 "autoWidth": false,
                 "ordering": false,
                 "bSortable": false, "aTargets": [0],
                 "fnRowCallback": function (nRow, aData, iDisplayIndex, iDisplayIndexFull) {
                     if (aData[1] == "Assign Code") {
                         $('td', nRow).css('background-color', '#ff6666');


                     }
                     //else  {
                     //    $('td', nRow).css('background-color', '#fff099');

                     //}
                 },
                 "iDisplayLength": -1,

                 "responsive": true,
             });
             });
    </script>--%>

    <!-- ################################################# END #################################################### -->

<script type="text/javascript">
    function ShowConfirmMsg() {
        $(function () {
            $("#messageConfirm").dialog({
                title: "Post PO",
                width: '520px',
                buttons: {
                    Close: function () {
                        $(this).dialog('close');
                    }
                },
                modal: true
            });
            $("#messageConfirm").parent().appendTo($("form:first"));
        });
    }
    </script>

    <div id="messageConfirm" style="display: none;">
        <%--<asp:UpdatePanel ID="UpdatePanel4" runat="server">
            <ContentTemplate>--%>
              <div style="display:none;">
                <asp:Label Text="" ID="lblConfirmDetailID" runat="server" ForeColor="Maroon" />
                <br />
                <asp:Label Text="" ID="lblOrigQuantity" runat="server" ForeColor="Maroon" />
                <br />
                <asp:Label Text="" ID="lblConfirmRRNo" runat="server" ForeColor="Maroon" />
                <br />
                <asp:Label Text="" ID="lblConfirmPurchasingCode" runat="server" ForeColor="Maroon" />
                <br />
                <asp:Label Text="" ID="lblConfirmItemCode" runat="server" ForeColor="Maroon" />
                <br />
                <asp:Label Text="" ID="lblConfirmUnitCost" runat="server" ForeColor="Maroon" />
                <br />
                 <asp:Label Text="" ID="lblRetailPrice" runat="server" ForeColor="Maroon" />
                <br />
                <asp:Label Text="" ID="lblConfirmQtyReceived" runat="server" ForeColor="Maroon" />
                <br />
                <asp:Label Text="" ID="lblConfirmTotalAmount" runat="server" ForeColor="Maroon" />
                <br />
                <asp:Label Text="" ID="lblConfirmBatchNo" runat="server" ForeColor="Maroon" />
                <br />
                <asp:Label Text="" ID="lblConfirmateExpiry" runat="server" ForeColor="Maroon" />
               </div>

                <asp:Label Text="" ID="lblItem" runat="server" ForeColor="Maroon" />
               
                <asp:Label Text="" ID="lblMsgConfirm" runat="server" />
                &nbsp;&nbsp;&nbsp;
                <asp:Button ID="btnPostPO" runat="server" Text="YES/Proceed" CssClass="btn btn-sm btn-success" OnClientClick="ShowProgress()"  OnClick="btnPostPO_Click" />
            <%--</ContentTemplate>
        </asp:UpdatePanel>--%>
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
    <script type="text/javascript">
        function tabE(obj, e) {
            var f = (typeof event != 'undefined') ? window.event : e; // IE : Moz 
            if (e.keyCode == 13) {
                var ele = document.forms[0].elements;
                for (var i = 0; i < ele.length; i++) {
                    var q = (i == ele.length - 1) ? 0 : i + 1; // if last element : if any other 
                    if (obj == ele[i]) { ele[q].focus(); break }
                }
                return false;
            }
        }

        function disableautocompletion(id) {
            var passwordControl = document.getElementById(id);
            passwordControl.setAttribute("autocomplete", "off");
        }

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