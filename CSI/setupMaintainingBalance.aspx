<%@ Page Title="Set Maintaining Balance" MaintainScrollPositionOnPostback="true" Language="C#" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true" CodeBehind="setupMaintainingBalance.aspx.cs" Inherits="CSI.setupMaintainingBalance" %>
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
            <div class="row mb-1">
                <div class="col-sm-6">
                    <h5>Setup Maintaining Balance</h5>
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
                                               Category :
                                </div>
                                <div class="col-sm-3">
                                    <div class="input-group">
                                          <asp:DropDownList ID="ddCategory" runat="server" class="form-control" Style="width: 135px;  text-transform:uppercase;" AutoPostBack="True" OnSelectedIndexChanged="ddCategory_SelectedIndexChanged">
                                            </asp:DropDownList> 
                                        <%--<asp:RequiredFieldValidator ID="Req4" runat="server" ControlToValidate="ddCategory" Display="Dynamic" 
                                            InitialValue="0" ForeColor="Red" ValidationGroup="grpSave"
                                                 ErrorMessage="Please select category"></asp:RequiredFieldValidator>--%>
                                    </div>
                                </div>
                            </div>
                        </div>

                         <div class="card-body">
                                <div class="col-sm-12">
                                    <asp:GridView ID="gvItems" runat="server" ClientIDMode="Static" DataKeyNames="RecNum" EmptyDatatext="No Record Found" class="table table-bordered table-hover" AutoGenerateColumns="false" OnPreRender="gvItems_PreRender" OnRowCancelingEdit="gvItems_RowCancelingEdit" OnRowEditing="gvItems_RowEditing" OnRowUpdating="gvItems_RowUpdating" >
                                        <Columns>
                                            
                                            <asp:BoundField DataField="RecNum" HeaderText="RecNum" ReadOnly="true" ItemStyle-Width="3%" ItemStyle-HorizontalAlign="Center" ItemStyle-CssClass="hiddencol" HeaderStyle-CssClass="hiddencol"/>

                                     

                                            <asp:BoundField DataField="sup_ItemCode" HeaderText="Item Code" ItemStyle-Width="10%"  ReadOnly="true" HeaderStyle-HorizontalAlign="Center">
                                                <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                            </asp:BoundField>
                                            
                                            <asp:BoundField DataField="sup_DESCRIPTION" HeaderText="Item Description" ItemStyle-Width="30%"  ReadOnly="true" HeaderStyle-HorizontalAlign="Center">
                                                <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                            </asp:BoundField>

                                            <asp:BoundField DataField="sup_UOM" HeaderText="UOM" ItemStyle-Width="10%"  ReadOnly="true" HeaderStyle-HorizontalAlign="Center">
                                                <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                            </asp:BoundField>

                                        

                                             <asp:TemplateField HeaderText="Maintaining Balance" ItemStyle-Width="10%" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblMBal" runat="server" Text='<%#Eval("MaintainingBalance") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <EditItemTemplate>
                                                        <asp:TextBox ID="txtMBal" runat="server"  class="form-control decimalnumbers-only" Width="105px" Text='<%#Eval("MaintainingBalance") %>' BackColor="#ffcc99" Style="text-align: center;"></asp:TextBox>
                                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator171" runat="server" ErrorMessage="Required" 
                                                            ControlToValidate="txtMBal" ValidationGroup='<%# "Grp-" + Container.DataItemIndex %>'  ForeColor="Red" Display="Dynamic"></asp:RequiredFieldValidator>
                                                    </EditItemTemplate>
                                              </asp:TemplateField>

                                             <asp:TemplateField>
                                                <ItemStyle HorizontalAlign="Center" Width="10%" />
                                                <ItemTemplate>
                                                    <asp:LinkButton ID="lnkView" runat="server" CommandName="Edit" class="btn btn-sm btn-info" CommandArgument='<%#Eval("RecNum") %>'><i class="fa fa-edit"></i></asp:LinkButton>
                                                </ItemTemplate>
                                                    <EditItemTemplate>
                                                                <asp:Button ID="btn_Update" runat="server" Text="Update" CommandName="Update" ValidationGroup='<%# "Grp-" + Container.DataItemIndex %>'  class="btn btn-sm btn-primary"/>
                                                                <asp:Button ID="btn_Cancel" runat="server" Text="Cancel" CommandName="Cancel"  class="btn btn-sm btn-danger"/>
                                                            </EditItemTemplate>
                                            </asp:TemplateField>

                                        </Columns>
                                    </asp:GridView>
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
                         <div class="loader"></div>
                         <br />
                         <asp:Label ID="Label2" runat="server" Text="Please wait" ForeColor="Red"></asp:Label>
                    </div>

    <!-- jQuery -->
    <script src="plugins/jquery/jquery.min.js"></script>
    <!-- Bootstrap 4 -->
    <script src="plugins/bootstrap/js/bootstrap.bundle.min.js"></script>

     <script>
         $(function () {
             $("#<%= gvItems.ClientID%>").DataTable({
                 "paging": true,
                 "lengthChange": true,
                 "searching": true,
                 "ordering": false,
                 "aLengthMenu": [[-1, 25, 50], ["All", 25, 50]],
                 "iDisplayLength": -1,
                 "bSortable": false,
                 "aTargets": [0],
                 "buttons": ["excel", "pdf", "print"],
                 "info": true,
                 "autoWidth": false,
                 "responsive": true,
             }).buttons().container().appendTo('#gvItems_wrapper .col-md-6:eq(0)');;
         });
    </script>

    <!-- ################################################# END #################################################### -->

    <script type="text/javascript">
        function ShowSuccessMsg() {
            $(function () {
                $("#messageDiv").dialog({
                    title: "Maintaining Balance",
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
                <asp:Label Text="" ID="lblMsg" runat="server" />
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

