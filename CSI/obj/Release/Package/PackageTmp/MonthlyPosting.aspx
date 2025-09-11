<%@ Page Title="Monthly Posting" MaintainScrollPositionOnPostback="true" Language="C#" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true" CodeBehind="MonthlyPosting.aspx.cs" Inherits="CSI.MonthlyPosting" %>
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
                    <h5>Monthly Posting</h5>
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
                                 <div class="col-md-1 col-1 text-left">
                                               Category :
                                </div>
                                <div class="col-sm-3">
                                    <div class="input-group">
                                          <asp:DropDownList ID="ddCategory" runat="server" class="form-control" Style="width: 135px;  text-transform:uppercase;" AutoPostBack="true" OnSelectedIndexChanged="ddCategory_SelectedIndexChanged" >
                                            </asp:DropDownList> 
                                        &nbsp;&nbsp;
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" InitialValue="0" 
                                        ControlToValidate="ddCategory"  Display="Dynamic" ValidationGroup="grpCon" ForeColor="Red"
                                        ErrorMessage="Please select"></asp:RequiredFieldValidator>
                                    </div>
                                </div>
                            </div>

                            <div style="margin-bottom: 5px" class="input-group">
                                <div class="col-md-1 col-1 text-left">
                                    Year :
                                </div>
                                <div class="col-sm-1">
                                    <div class="input-group">
                                        <asp:DropDownList ID="ddYear" runat="server" class="form-control" Style="width: 135px;">
                                        </asp:DropDownList>

                                    </div>
                                </div>
                            </div>



                            <div style="margin-bottom: 15px" class="input-group">
                                <div class="col-md-1 col-1 text-left">
                                    Month :
                                </div>
                                <div class="col-sm-3">
                                    <div class="input-group">
                                        <asp:DropDownList ID="ddMonth" runat="server" class="form-control" Style="width: 135px; text-transform: uppercase;">
                                                <asp:ListItem Value="0" Text="Select Month" Selected="True"></asp:ListItem>
                                                <asp:ListItem Value="1" Text="January"></asp:ListItem>
                                                <asp:ListItem Value="2" Text="February"></asp:ListItem>
                                                <asp:ListItem Value="3" Text="March"></asp:ListItem>
                                                <asp:ListItem Value="4" Text="April"></asp:ListItem>
                                                <asp:ListItem Value="5" Text="May"></asp:ListItem>
                                                <asp:ListItem Value="6" Text="June"></asp:ListItem>
                                                <asp:ListItem Value="7" Text="July"></asp:ListItem>
                                                <asp:ListItem Value="8" Text="August"></asp:ListItem>
                                                <asp:ListItem Value="9" Text="September"></asp:ListItem>
                                                <asp:ListItem Value="10" Text="October"></asp:ListItem>
                                                <asp:ListItem Value="11" Text="November"></asp:ListItem>
                                                <asp:ListItem Value="12" Text="December"></asp:ListItem>
                                        </asp:DropDownList>
                                        &nbsp;&nbsp;
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator113" runat="server" InitialValue="0"
                                            ControlToValidate="ddMonth" Display="Dynamic" ValidationGroup="grpCon" ForeColor="Red"
                                            ErrorMessage="Please select"></asp:RequiredFieldValidator>
                                    </div>
                                </div>
                            </div>

                            
                             <div style="margin-bottom: 5px" class="input-group">
                                 <div class="col-md-1 col-1 text-left">
                                               
                                </div>
                                <div class="col-sm-11">
                                    <div class="input-group">
                                        <asp:Button ID="btnPost" runat="server" Text="P O S T" ValidationGroup="grpCon"   CssClass="btn btn-sm btn-info" OnClick="btnPost_Click"/>   
                                        &nbsp;&nbsp;&nbsp;
                                        <asp:Label ID="lblNote" runat="server" Text="" ForeColor="Red" Visible="false"></asp:Label>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <%--OnClientClick="if(Page_ClientValidate()) ShowProgress()"--%>
                        <div class="card-body">
                            <div class="col-sm-6 text-center">
                                    Last Monthly Posting Process Per Category
                                    <asp:GridView ID="gvMP" runat="server" ClientIDMode="Static" class="table table-bordered table-hover" AutoGenerateColumns="false">
                                            <Columns>
                            
                                                <asp:BoundField DataField="Sup_CategoryName" HeaderText="Category" ItemStyle-Width="25%" ItemStyle-HorizontalAlign="Center"/>

                                                <asp:BoundField DataField="YearPost" HeaderText="Year" ItemStyle-Width="10%" HeaderStyle-HorizontalAlign="Center">
                                                    <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                                </asp:BoundField>
                            
                                                <asp:BoundField DataField="LastMonth" HeaderText="Month" ItemStyle-Width="15%" HeaderStyle-HorizontalAlign="Center">
                                                    <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                                </asp:BoundField>
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
<!-- ################################################# END #################################################### -->

    <script type="text/javascript">
        function ShowSuccessMsg() {
            $(function () {
                $("#messageDiv").dialog({
                    title: "Monthly Posting",
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
