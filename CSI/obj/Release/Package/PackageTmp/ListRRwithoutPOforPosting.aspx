<%@ Page Title="RR for Posting" Language="C#" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true" CodeBehind="ListRRwithoutPOforPosting.aspx.cs" Inherits="CSI.ListRRwithoutPOforPosting" %>
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
    <section class="content">
        <div class="container-fluid">
            <div class="row">
                <div class="col-12">
                    
                         <div class="card">
                            <div class="card-header">
                                 <h5>List of RR for Posting</h5>
                            </div>

                             <div class="card-body">

                            <asp:GridView ID="gvRR" runat="server" Width="100%"  class="table table-bordered table-hover" DataKeyNames="RecNum" AutoGenerateColumns="false" OnRowCancelingEdit="gvRR_RowCancelingEdit" OnRowEditing="gvRR_RowEditing" OnRowUpdating="gvRR_RowUpdating" OnRowCommand="gvRR_RowCommand" OnRowDeleting="gvRR_RowDeleting">
                                <Columns>
                                     <%--ItemStyle-CssClass="hiddencol" HeaderStyle-CssClass="hiddencol"--%>
                                    <asp:BoundField DataField="RecNum" HeaderText="RecNum" ItemStyle-Width="3%" HeaderStyle-HorizontalAlign="Center" ReadOnly="true" ItemStyle-HorizontalAlign="Center" ItemStyle-CssClass="hiddencol" HeaderStyle-CssClass="hiddencol">
                                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                        <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                    </asp:BoundField>

                           
                                    <asp:BoundField DataField="RRNo" HeaderText="RR No" ItemStyle-Width="8%" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" ReadOnly="true">
                                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                        <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                    </asp:BoundField> 

                                   <asp:BoundField DataField="sup_ItemCode" HeaderText="Item Code" HtmlEncode="false" ItemStyle-Width="8%" ReadOnly="true" HeaderStyle-HorizontalAlign="Center">
                                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                       <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                    </asp:BoundField>

                                     

                                      <asp:BoundField DataField="sup_DESCRIPTION" HeaderText="Item Description" ItemStyle-Width="20%" ReadOnly="true" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                        <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                    </asp:BoundField>

                                      <asp:TemplateField HeaderText="RR Price" ItemStyle-Width="7%" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Right">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblsup_UnitCost" runat="server" Text='<%#Eval("sup_UnitCost","{0:N2}") %>'></asp:Label>
                                                    </ItemTemplate>
                                         </asp:TemplateField>

                                    
                                    <asp:TemplateField HeaderText="RR Quantity" ItemStyle-Width="5%" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblvQtyReceived" runat="server" Text='<%#Eval("vQtyReceived","{0:N0}") %>'></asp:Label>
                                                    </ItemTemplate>
                                         </asp:TemplateField>
                                         

                                        <asp:TemplateField HeaderText="Edited Price" ItemStyle-Width="7%" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Right">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblsup_RetailCost" runat="server" Text='<%#Eval("sup_RetailCost","{0:N2}") %>'></asp:Label>
                                                    </ItemTemplate>
                                             <EditItemTemplate>
                                                      <asp:TextBox ID="txtsup_RetailCost" runat="server" CssClass="form-control  decimalnumbers-only" Text='<%#Eval("sup_RetailCost","{0:N2}") %>' Width="95px" AutoCompleteType="Disabled"  autocomplete="off"></asp:TextBox>
                                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1827" runat="server" ErrorMessage="Required"  
                                                            ControlToValidate="txtsup_RetailCost" ValidationGroup='<%# "Grp2-" + Container.DataItemIndex %>'  ForeColor="Red" Display="Dynamic"></asp:RequiredFieldValidator>
                                                 
                                                    </EditItemTemplate>
                                         </asp:TemplateField>

                                    <asp:TemplateField HeaderText="Edited <br />Quantity" ItemStyle-Width="5%" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblRetailQuantity" runat="server" Text='<%#Eval("RetailQuantity") %>'></asp:Label>
                                                    </ItemTemplate>
                                        <EditItemTemplate>
                                                      <asp:TextBox ID="txtRetailQuantity" runat="server" CssClass="form-control decimalnumbers-only" Text='<%#Eval("RetailQuantity") %>' Width="95px" AutoCompleteType="Disabled"  autocomplete="off"></asp:TextBox>
                                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator17827" runat="server" ErrorMessage="Required"  
                                                            ControlToValidate="txtRetailQuantity" ValidationGroup='<%# "Grp2-" + Container.DataItemIndex %>'  ForeColor="Red" Display="Dynamic"></asp:RequiredFieldValidator>
                                                 
                                                    </EditItemTemplate>
                                         </asp:TemplateField>

                                  <%--  <asp:TemplateField HeaderText="Total Amount" ItemStyle-Width="7%" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Right">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblTotalAmount" runat="server" Text='<%#Eval("TotalAmount","{0:#,###0.#0}") %>'></asp:Label>
                                                    </ItemTemplate>
                                         </asp:TemplateField>--%>

                                    <asp:TemplateField HeaderText="Total Amount" ItemStyle-Width="5%" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Right">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblTotalAmount" runat="server" Text='<%#Eval("TotalAmount") %>'></asp:Label>
                                                    </ItemTemplate>
                                        <EditItemTemplate>
                                                      <asp:TextBox ID="txtTotalAmount" runat="server" CssClass="form-control decimalnumbers-only" Text='<%#Eval("TotalAmount") %>' Width="95px" AutoCompleteType="Disabled"  autocomplete="off"></asp:TextBox>
                                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator178227" runat="server" ErrorMessage="Required"  
                                                            ControlToValidate="txtTotalAmount" ValidationGroup='<%# "Grp2-" + Container.DataItemIndex %>'  ForeColor="Red" Display="Dynamic"></asp:RequiredFieldValidator>
                                                 
                                                    </EditItemTemplate>
                                         </asp:TemplateField>


                                    
						              <asp:TemplateField HeaderText="Batch No." ItemStyle-Width="7%" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblvBatchNo" runat="server" Text='<%#Eval("vBatchNo") %>'></asp:Label>
                                                    </ItemTemplate>
                                         </asp:TemplateField>

                                     <asp:TemplateField HeaderText="Date Expiry" ItemStyle-Width="7%" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                                    <ItemTemplate>
                                                            <asp:Label ID="lblvDateExpiry" runat="server" Text='<%#Eval("vDateExpiry","{0:MM/dd/yyyy}") %>'></asp:Label>
                                                    </ItemTemplate>
                                         </asp:TemplateField>



                             <%--       <asp:TemplateField HeaderText="DCI logistics Code" ItemStyle-Width="10%" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblCode" runat="server" Text='<%#Eval("ItemCode") %>'></asp:Label>
                                                    </ItemTemplate>
                                                <EditItemTemplate>
                                                        <asp:DropDownList ID="ddItem" runat="server" Width="275px" CssClass="form-control chosen-select"></asp:DropDownList>
                                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator17" runat="server" ErrorMessage="Required" InitialValue="0" 
                                                            ControlToValidate="ddItem" ValidationGroup='<%# "Grp2-" + Container.DataItemIndex %>'  ForeColor="Red" Display="Dynamic"></asp:RequiredFieldValidator>
                                                        
                                                    </EditItemTemplate>

                                        </asp:TemplateField>--%>


                                  


                                    <asp:TemplateField>
                                        <ItemStyle HorizontalAlign="Center" Width="5%" />
                                        <ItemTemplate>
                                            <asp:LinkButton ID="lnkView" runat="server" CommandName="Edit" class="btn btn-sm btn-info"><i class="fa fa-edit"></i></asp:LinkButton>
                                        </ItemTemplate>
                                            <EditItemTemplate>
                                                        <asp:Button ID="btn_Update" runat="server" Text="Update" CommandName="Update" ValidationGroup='<%# "Grp-" + Container.DataItemIndex %>'  class="btn btn-sm btn-primary"/>
                                                        <asp:Button ID="btn_Cancel" runat="server" Text="Cancel" CommandName="Cancel"  class="btn btn-sm btn-warning"/>
                                                        <asp:Button ID="btn_Delete" runat="server" Text="Delete" CommandName="Delete"  class="btn btn-sm btn-danger"/>
                                                    </EditItemTemplate>
                                    </asp:TemplateField>

                                     <asp:TemplateField>
                                        <ItemStyle HorizontalAlign="Center" Width="5%" />
                                        <ItemTemplate>
                                            <asp:LinkButton ID="lnkPOST" runat="server" CommandName="Post" Text="POST" ValidationGroup='<%# "Grp2-" + Container.DataItemIndex %>' OnClientClick="if(Page_ClientValidate()) ShowProgress()"  class="btn btn-sm btn-success"></asp:LinkButton>
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
                title: "RR Entry Posting",
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
