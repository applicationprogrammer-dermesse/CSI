<%@ Page Title="Purchase Requisition Entry" Language="C#" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true" CodeBehind="PREntry.aspx.cs" Inherits="CSI.PREntry" %>
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
                    <h5>Purchase Requisition Entry</h5>
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
                                        <asp:Label ID="Label2" runat="server" Text="PR Number" style="width:125px;"></asp:Label>   
                                    </div>
                                     <div class="col-lg-3 col-3">
                                         <div class="input-group" >
                                             <asp:TextBox ID="txtPRNumber" runat="server" CssClass="form-control" Width="105px" ReadOnly="true" ></asp:TextBox>
                                             &nbsp;
                                             <asp:Button ID="btnRefresh" runat="server" Text="Refresh" CssClass="btn btn-default" OnClick="btnRefresh_Click"/>
                                         </div>
                                     </div>
                                     <div class="col-lg-6 col-6">
                                     </div>
                                    <div class="col-lg-1 col-1 text-right">
                                         <div class="input-group" >
                                             <asp:Button ID="btnPost" runat="server" Text="P O S T" CssClass="btn btn-primary"  OnClientClick="ShowProgress()" OnClick="btnPost_Click"/>
                                         </div>
                                     </div>

                             </div>
                                  

                            <div style="margin-bottom: 5px" class="input-group">
                                    <div class="col-md-2 col-2 text-left">
                                        <asp:Label ID="Label7" runat="server" Text="Date Required" style="width:125px;"></asp:Label>   
                                    </div>
                                     <div class="col-lg-3 col-3">
                                         <div class="input-group" >
                                             <asp:TextBox ID="txtDateRequired" runat="server" CssClass="form-control txtDateAll" Width="105px" ValidationGroup="grpSave" AutoCompleteType="Disabled"></asp:TextBox>
                                             <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="txtDateRequired" ForeColor="Red" ValidationGroup="grpSave"
                                             Display="Dynamic" ErrorMessage="Required Field"></asp:RequiredFieldValidator>
                                                    <asp:RegularExpressionValidator ID="REVdate" runat="server" ControlToValidate="txtDateRequired"
                                                ValidationExpression="^([1-9]|0[1-9]|1[0-2])[- / .]([1-9]|0[1-9]|1[0-9]|2[0-9]|3[0-1])[- / .](1[9][0-9][0-9]|2[0][0-9][0-9])$"
                                                ForeColor="Red" Display="Dynamic"
                                                ErrorMessage="Invalid date format"
                                                ValidationGroup="grpSave"></asp:RegularExpressionValidator>
                                         </div>
                                     </div>
                                     <div class="col-lg-6 col-6">
                                     </div>
                                    <div class="col-lg-1 col-1 text-right">
                                         <div class="input-group" >
                                             
                                         </div>
                                     </div>

                             </div>

                             <div style="margin-bottom: 5px" class="input-group">
                                    <div class="col-md-2 col-2 text-left">
                                        <asp:Label ID="Label4" runat="server" Text="Item Description" style="width:125px;"></asp:Label>   
                                    </div>
                                     <div class="col-sm-6 col-6">
                                             <asp:DropDownList ID="ddItem" runat="server"  CssClass="form-control chosen-select" required="required" AutoPostBack="True" OnSelectedIndexChanged="ddItem_SelectedIndexChanged"></asp:DropDownList>
                                     </div>
                                    <div class="col-sm-3 col-3">
                                         <div class="input-group" >
                                             <asp:RequiredFieldValidator ID="Req4" runat="server" ControlToValidate="ddItem" Display="Dynamic" InitialValue="0" ForeColor="Red" ValidationGroup="grpSave"
                                                 ErrorMessage="Please select item"></asp:RequiredFieldValidator>
                                        </div>
                                     </div>

                                    

                                 </div>
                            
                               <div style="margin-bottom: 10px" class="input-group">
                                    <div class="col-md-2 col-2 text-left">
                                        <asp:Label ID="Label1" runat="server" Text="Balance" style="width:125px;"></asp:Label>   
                                    </div>
                                     <div class="col-lg-2 col-2">
                                         <div class="input-group" >
                                             <asp:TextBox ID="txtBalance" runat="server" CssClass="form-control decimalnumbers-only" Width="75px"  AutoCompleteType="Disabled" ReadOnly="true"></asp:TextBox>
                                         </div>
                                     </div>
                             </div>


                             
                              <div style="margin-bottom: 10px" class="input-group">
                                    <div class="col-md-2 col-2 text-left">
                                        <asp:Label ID="Label6" runat="server" Text="Quantity" style="width:125px;"></asp:Label>   
                                    </div>
                                     <div class="col-lg-2 col-2">
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

                                        <asp:GridView ID="gvItems" CssClass="table table-bordered active" DataKeyNames="RecNum" AutoGenerateColumns="false" runat="server" OnRowCancelingEdit="gvItems_RowCancelingEdit" OnRowDeleting="gvItems_RowDeleting" OnRowEditing="gvItems_RowEditing" OnRowUpdating="gvItems_RowUpdating" >
                                                <Columns>
                                                    <asp:BoundField DataField="RecNum" HeaderText="ID" ReadOnly="true" ItemStyle-Width="3%" ItemStyle-HorizontalAlign="Center" ItemStyle-CssClass="hiddencol" HeaderStyle-CssClass="hiddencol"/>
                                                    <asp:BoundField DataField="BrName" HeaderText="Branch/Department"  ReadOnly="true" ItemStyle-Width="15%" ItemStyle-HorizontalAlign="Center" />
                                                    <asp:BoundField DataField="RequiredDate" HeaderText="Required Date" DataFormatString="{0:MM/dd/yyyy}"  ReadOnly="true" ItemStyle-Width="10%" ItemStyle-HorizontalAlign="Center" />
                                                    <asp:BoundField DataField="sup_DESCRIPTION" HeaderText="Item Description"  ReadOnly="true" ItemStyle-Width="30%" ItemStyle-HorizontalAlign="Center" />
                                                    <asp:BoundField DataField="vQtyBalance" HeaderText="Balance" DataFormatString="{0:###0;(###0);0}"  ReadOnly="true" ItemStyle-Width="10%" ItemStyle-HorizontalAlign="Center" />
                                                  <asp:TemplateField HeaderText="Quantity" ItemStyle-Width="10%" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblQuantity" runat="server" Text='<%#Eval("vQtyRequest","{0:###0;(###0);0}") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <EditItemTemplate>
                                                        <asp:TextBox ID="txtvQuantity" runat="server"  class="form-control decimalnumbers-only" Text='<%#Eval("vQtyRequest","{0:###0}") %>' BackColor="#ffcc99" Style="text-align: center;"></asp:TextBox>
                                                        <asp:RequiredFieldValidator ID="Requi7" runat="server" ErrorMessage="Required" 
                                                            ControlToValidate="txtvQuantity" ValidationGroup='<%# "Grp-" + Container.DataItemIndex %>'  ForeColor="Red" Display="Dynamic"></asp:RequiredFieldValidator>
                                                    </EditItemTemplate>
                                                </asp:TemplateField>

                                                <asp:BoundField DataField="EncodedBy" HeaderText="Encoded By"  ReadOnly="true" ItemStyle-Width="15%" ItemStyle-HorizontalAlign="Center" />

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
        $('.txtDateAll').datepicker({
            //dateFormat: "MM/dd/yyyy",
            //maxDate: new Date,
            //minDate: new Date(2007, 6, 12),
            minDate: 0,
            changeMonth: true,
            changeYear: true,
            yearRange: "-1:+1"
        });

    });
</script>

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

    <script type="text/javascript">
        function disableautocompletion(id) {
            var passwordControl = document.getElementById(id);
            passwordControl.setAttribute("autocomplete", "off");
        }
    </script>

</asp:Content>
