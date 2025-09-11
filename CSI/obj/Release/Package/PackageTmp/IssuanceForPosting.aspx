<%@ Page Title="Issuance For Posting" MaintainScrollPositionOnPostback="true" Language="C#" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true" CodeBehind="IssuanceForPosting.aspx.cs" Inherits="CSI.IssuanceForPosting" %>
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
    .loading
    {
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
                    <h5>Issuance for Posting</h5>
                </div>
                <div class="col-sm-6">
                    <ol class="breadcrumb float-sm-right">
                         
                        <li class="breadcrumb-item"><a href="ListUnpostedIssuance.aspx" class="nav-link">Back to List</a></li>
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
                                     <div class="col-md-10 col-10">
                                         <div class="input-group" >
                                            <asp:Label ID="lblBranchCode" runat="server" Text=""></asp:Label>
                                             &nbsp;&nbsp;-&nbsp;&nbsp;
                                             <asp:Label ID="lblBranch" runat="server" Text=""></asp:Label>

                                        </div>
                                     </div>
                                     
                                     <div class="col-md-1 col-1 text-right">
                                         <div class="input-group" >
                                             <asp:Button ID="btnPost" runat="server" Text="P O S T"  CssClass="btn btn-sm btn-primary" OnClientClick="ShowProgress()" OnClick="btnPost_Click"/>
                                              <%--<button type="button" onserverclick="btnPost_Click" class="btn btn-sm btn-primary btn-block"><i class="fa fa-bell"></i> SUBMIT</button>--%>
                                         </div>
                                     </div>

                                 </div>

                                <div style="margin-bottom: 10px" class="input-group">
                                     <asp:Label ID="Label2" runat="server" Text="No." style="width:125px;"></asp:Label>
                                     <asp:Label ID="lblNo" runat="server" Text=""></asp:Label>
                                 </div>

                                <div style="margin-bottom: 10px" class="input-group">
                                     <asp:Label ID="Label3" runat="server" Text="Date Posted" style="width:125px;"></asp:Label>
                                     <asp:Label ID="lblDatePosted" runat="server" Text=""></asp:Label>
                                 </div>

                                <div style="margin-bottom: 10px" class="input-group">
                                     <asp:Label ID="Label5" runat="server" Text="Category" style="width:125px;"></asp:Label>
                                     <asp:Label ID="lblCategory" runat="server" Text=""></asp:Label>
                                 </div>

                                <div style="margin-bottom: 15px" class="input-group">
                                     <asp:Label ID="Label8" runat="server" Text="Delivered By" style="width:125px;"></asp:Label>
                                      <asp:Label ID="lblDeliveredBy" runat="server" Text="" style="width:125px;"></asp:Label>
                                     
                                 </div>

                                 <div style="margin-bottom: 10px" class="input-group">
                                     <asp:Label ID="Label1" runat="server" Text="Issue Slip No." style="width:125px;"></asp:Label>
                                      <asp:Label ID="lblIssueSlipNo" runat="server" Text="" style="width:125px;"></asp:Label>
                                     
                                 </div>
                         </div>

                           <div class="card-body">

                            <asp:GridView ID="gvForIssuance" runat="server" Width="100%"  class="table table-bordered table-hover" DataKeyNames="vRecNum" AutoGenerateColumns="false" OnRowCancelingEdit="gvForIssuance_RowCancelingEdit" OnRowDeleting="gvForIssuance_RowDeleting" OnRowEditing="gvForIssuance_RowEditing" OnRowUpdating="gvForIssuance_RowUpdating" OnRowDataBound="gvForIssuance_RowDataBound">
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

                                    <asp:TemplateField>
                                        <ItemStyle HorizontalAlign="Center" Width="5%" />
                                        <ItemTemplate>
                                            <asp:LinkButton ID="lnkView" runat="server" CommandName="Edit" class="btn btn-sm btn-info" CommandArgument='<%#Eval("vRecNum") %>'><i class="fa fa-edit"></i></asp:LinkButton>
                                        </ItemTemplate>
                                            <EditItemTemplate>
                                                        <asp:Button ID="btn_Update" runat="server" Text="Update" CommandName="Update" ValidationGroup='<%# "Grp-" + Container.DataItemIndex %>'  class="btn btn-sm btn-primary"/>
                                                        <asp:Button ID="btn_Cancel" runat="server" Text="Cancel" CommandName="Cancel"  class="btn btn-sm btn-warning"/>
                                                    </EditItemTemplate>
                                    </asp:TemplateField>

                                     <asp:TemplateField>
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

       <div class="loading" align="center" style="margin-top:100px;">
                        <br />
                        <br />
                        <%--<img src="images/ajax-loader.gif" alt=""  />--%>
                         <div class="loader"></div>
                         <br />
                         <asp:Label ID="Label6" runat="server" Text="Please wait" ForeColor="Red"></asp:Label>
                         
                    </div>



<script src="plugins/jquery/jquery.min.js"></script>
    <!-- Bootstrap 4 -->
<script src="plugins/bootstrap/js/bootstrap.bundle.min.js"></script>

<!-- ################################################# END #################################################### -->

<script type="text/javascript">
    function ShowSuccessMsg() {
        $(function () {
            $("#messageDiv").dialog({
                title: "Issuance",
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

<div id="messageDiv" style="display: none;">

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
                            window.location = '<%= ResolveUrl("~/ListUnpostedIssuance.aspx") %>';
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


</asp:Content>
