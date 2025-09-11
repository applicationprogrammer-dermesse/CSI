<%@ Page Title="PR Detail" Language="C#" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true" CodeBehind="ViewPRDetail.aspx.cs" Inherits="CSI.ViewPRDetail" %>
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
                    <h5>PR Detail</h5>
                </div>
                <div class="col-sm-6">
                    <ol class="breadcrumb float-sm-right">
                         
                        <li class="breadcrumb-item"><a href="ListPurchaseRequisition.aspx" class="nav-link">Back to List</a></li>
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
                                        <asp:Label ID="Label4" runat="server" Text="Department " style="width:125px;"></asp:Label>
                                        </div>
                                     <div class="col-md-7 col-7">
                                         <div class="input-group" >
                                             <asp:Label ID="lblBranch" runat="server" Text=""></asp:Label>

                                        </div>
                                     </div>
                                     <div class="col-md-3 col-3 text-right">
                                         
                                    </div>
                                     <div class="col-md-1 col-1 text-right">
                                         <div class="input-group" >
                                             <asp:Button ID="btnPost" runat="server" Text="P O S T"  CssClass="btn btn-sm btn-primary" ValidationGroup="grpPost" OnClick="btnPost_Click"/>
                                              <%--<button type="button" onserverclick="btnPost_Click" class="btn btn-sm btn-primary btn-block"><i class="fa fa-bell"></i> SUBMIT</button>--%>
                                         </div>
                                     </div>

                                 </div>

                                <div style="margin-bottom: 10px" class="input-group">
                                     <asp:Label ID="Label2" runat="server" Text="PR No." style="width:125px;"></asp:Label>
                                     <asp:Label ID="lblPRNo" runat="server" Text=""></asp:Label>
                                 </div>

                                <div style="margin-bottom: 10px" class="input-group">
                                     <asp:Label ID="Label3" runat="server" Text="Required Date" style="width:125px;"></asp:Label>
                                     <asp:Label ID="lblREquiredDate" runat="server" Text=""></asp:Label>
                                 </div>

                                 <div style="margin-bottom: 15px" class="input-group">
                                     <asp:Label ID="Label1" runat="server" Text="Encoded By" style="width:125px;"></asp:Label>
                                     <asp:Label ID="lblEncodedBy" runat="server" Text=""></asp:Label>
                                 </div>

                                <div style="margin-bottom: 10px" class="input-group">
                                     <asp:Label ID="Label5" runat="server" Text="Supplier" style="width:125px;"></asp:Label>
                                     <asp:DropDownList ID="ddSupplier" runat="server" Width="350px" CssClass="form-control chosen-select" AutoPostBack="True" OnSelectedIndexChanged="ddSupplier_SelectedIndexChanged" ></asp:DropDownList>
                                 </div>
                                
                                <div style="margin-bottom: 10px" class="input-group">
                                     <asp:Label ID="Label7" runat="server" Text="Contact Person" style="width:125px;"></asp:Label>
                                     <asp:Label ID="lblContactPerson" runat="server" Text=""></asp:Label>
                                 </div>

                                <div style="margin-bottom: 10px" class="input-group">
                                     <asp:Label ID="Label9" runat="server" Text="Contact No." style="width:125px;"></asp:Label>
                                     <asp:Label ID="lblContactNo" runat="server" Text=""></asp:Label>
                                 </div>
               


                                 <div style="margin-bottom: 10px" class="input-group">
                                     <asp:Label ID="Label6" runat="server" Text="Category" ForeColor="Transparent" style="width:125px;"></asp:Label>
                                     <asp:Button ID="btnPrint" runat="server" Text="EXCEL"  CssClass="btn btn-sm btn-success" OnClick="btnPrint_Click"/>
                                     
                                 </div>
                         </div>

                           <div class="card-body">

                            <asp:GridView ID="gvItems" CssClass="table table-bordered active" DataKeyNames="RecNum" AutoGenerateColumns="false" runat="server" OnRowCancelingEdit="gvItems_RowCancelingEdit" OnRowDeleting="gvItems_RowDeleting" OnRowEditing="gvItems_RowEditing" OnRowUpdating="gvItems_RowUpdating" >
                                                <Columns>
                                                    <asp:BoundField DataField="RecNum" HeaderText="ID" ReadOnly="true" ItemStyle-Width="3%" ItemStyle-HorizontalAlign="Center" ItemStyle-CssClass="hiddencol" HeaderStyle-CssClass="hiddencol"/>
                                                    <asp:BoundField DataField="RowNum" HeaderText="No" ReadOnly="true" ItemStyle-Width="5%" ItemStyle-HorizontalAlign="Center"/>
                                                    <asp:BoundField DataField="sup_ItemCode" HeaderText="Itemcode"  ReadOnly="true" ItemStyle-Width="10%" ItemStyle-HorizontalAlign="Center" />
                                                    <asp:BoundField DataField="sup_DESCRIPTION" HeaderText="Item Description"  ReadOnly="true" ItemStyle-Width="30%" ItemStyle-HorizontalAlign="Center" />
                                                    
                                                    <asp:BoundField DataField="sup_UOM" HeaderText="UOM"  ReadOnly="true" ItemStyle-Width="5%" ItemStyle-HorizontalAlign="Center" />
                                                    <asp:BoundField DataField="vQtyBalance" HeaderText="Stock On Hand" DataFormatString="{0:###0;(###0);0}"  ReadOnly="true" ItemStyle-Width="10%" ItemStyle-HorizontalAlign="Center" />

                                                  <asp:TemplateField HeaderText="Order Quantity" ItemStyle-Width="10%" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblQuantity" runat="server" Text='<%#Eval("vQtyRequest","{0:###0;(###0);0}") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <EditItemTemplate>
                                                        <asp:TextBox ID="txtvQuantity" runat="server"  class="form-control decimalnumbers-only" Text='<%#Eval("vQtyRequest","{0:###0}") %>' BackColor="#ffcc99" Style="text-align: center;"></asp:TextBox>
                                                        <asp:RequiredFieldValidator ID="Requi7" runat="server" ErrorMessage="Required" 
                                                            ControlToValidate="txtvQuantity" ValidationGroup='<%# "Grp-" + Container.DataItemIndex %>'  ForeColor="Red" Display="Dynamic"></asp:RequiredFieldValidator>
                                                    </EditItemTemplate>
                                                </asp:TemplateField>

                                                

                                                <asp:TemplateField>
                                                        <ItemStyle HorizontalAlign="Center" Width="5%" />
                                                        <ItemTemplate>
                                                            <asp:LinkButton ID="lnkView" runat="server" CommandName="Edit" class="btn btn-sm btn-info" CommandArgument='<%#Eval("RecNum") %>'><i class="fa fa-edit"></i></asp:LinkButton>
                                                        </ItemTemplate>
                                                            <EditItemTemplate>
                                                                        <asp:Button ID="btn_Update" runat="server" Text="Update" CommandName="Update" ValidationGroup='<%# "Grp-" + Container.DataItemIndex %>'  class="btn btn-sm btn-primary"/>
                                                                        <asp:Button ID="btn_Cancel" runat="server" Text="Cancel" CommandName="Cancel"  class="btn btn-sm btn-warning"/>
                                                                    </EditItemTemplate>
                                                    </asp:TemplateField>

                                                     <asp:TemplateField>
                                                        <ItemStyle HorizontalAlign="Center" Width="5%" />
                                                        <ItemTemplate>
                                                            <asp:LinkButton ID="lnkDelete" runat="server" CommandName="Delete" class="btn btn-sm btn-danger" CommandArgument='<%#Eval("RecNum") %>'><i class="fa fa-trash"></i></asp:LinkButton>
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



<!-- ################################################# END #################################################### -->
    <script src="docsupport/jquery-3.2.1.min.js" type="text/javascript"></script>
    <script src="chosen.jquery.js" type="text/javascript"></script>
    <script src="docsupport/prism.js" type="text/javascript" charset="utf-8"></script>
    <script src="docsupport/init.js" type="text/javascript" charset="utf-8"></script>


<!-- ################################################# END #################################################### -->

<script type="text/javascript">
    function ShowSuccessMsg() {
        $(function () {
            $("#messageDiv").dialog({
                title: "PR Entry",
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
                    title: "Purchase Request",
                    width: '335px',
                    buttons: {
                        Close: function () {
                            window.location = '<%= ResolveUrl("~/ListPurchaseRequisition.aspx") %>';
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


</asp:Content>

