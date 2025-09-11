<%@ Page Title="Online Order List" Language="C#" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true" CodeBehind="ListOrderOnline.aspx.cs" Inherits="CSI.ListOrderOnline" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
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
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
     <section class="content-header">
        <div class="container-fluid">
            <div class="row mb-2">
                <div class="col-sm-6">
                    <h5>Online Order List(For Print DR)</h5>
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
                            </div>


                             <div class="card-body">
                                <div class="col-sm-12">
                                    <div class="col-sm-12 text-center">

                                         <asp:GridView ID="gvOrders" CssClass="table table-bordered active" AutoGenerateColumns="false" runat="server" OnRowCommand="gvOrders_RowCommand">
                                                <Columns>
                                                    <asp:BoundField DataField="OrderDate" HeaderText="Order Date" DataFormatString="{0:MM/dd/yyyy}"  ReadOnly="true" ItemStyle-Width="8%" ItemStyle-HorizontalAlign="Center" />
                                                    <asp:BoundField DataField="ReferenceNo" HeaderText="Reference No"  ReadOnly="true" ItemStyle-Width="10%" ItemStyle-HorizontalAlign="Center" />
                                                    <asp:BoundField DataField="Source" HeaderText="Source"  ReadOnly="true" ItemStyle-Width="8%" ItemStyle-HorizontalAlign="Center" />
                                                    <asp:BoundField DataField="Type" HeaderText="Type"  ReadOnly="true" ItemStyle-Width="8%" ItemStyle-HorizontalAlign="Center" />
                                                    <asp:BoundField DataField="CustomerName" HeaderText="Customer Name"  ReadOnly="true" ItemStyle-Width="15%" ItemStyle-HorizontalAlign="Center" />
                                                    <asp:BoundField DataField="Customer Address" HeaderText="Customer Address"  ReadOnly="true" ItemStyle-Width="30%" ItemStyle-HorizontalAlign="Center" />
                                                    <asp:BoundField DataField="ContactNo" HeaderText="Contact No"  ReadOnly="true" ItemStyle-Width="10%" ItemStyle-HorizontalAlign="Center" />
                                                    <asp:BoundField DataField="EmailAddress" HeaderText="Email Address"  ReadOnly="true" ItemStyle-Width="15%" ItemStyle-HorizontalAlign="Center" />

                                                    <asp:TemplateField>
                                                        <ItemStyle HorizontalAlign="Center" Width="5%" />
                                                        <ItemTemplate>
                                                            <asp:LinkButton ID="lnkView" runat="server" CommandName="ViewOrderDetail" class="btn btn-sm btn-info" ><i class="fa fa-eye"></i></asp:LinkButton>
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

    

      <!-- jQuery -->
    <script src="plugins/jquery/jquery.min.js"></script>
    <!-- Bootstrap 4 -->
    <script src="plugins/bootstrap/js/bootstrap.bundle.min.js"></script>


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

