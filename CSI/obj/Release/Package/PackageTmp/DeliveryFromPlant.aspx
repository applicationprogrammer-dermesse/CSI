<%@ Page Title="CSI System | Delivery from Plant" Language="C#" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true" CodeBehind="DeliveryFromPlant.aspx.cs" Inherits="CSI.DeliveryFromPlant" %>
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
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
      <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
     <section class="content-header">
        <div class="container-fluid">
            <div class="row mb-1">
                <div class="col-sm-6">
                    <h5>Delivery from Plant</h5>
                </div>
                <div class="col-sm-6">
                    <ol class="breadcrumb float-sm-right">
                            <%--<li class="breadcrumb-item"><a href="Return.aspx" class="nav-link">Back to Customer List</a></li>--%>
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
                                            <div class="col-md-2 col-2 text-left">
                                               Branch :
                                            </div>
                                            <div class="col-sm-3">
                                                <div class="input-group">
                                                    <asp:Label ID="lblBranch" runat="server" Text=""></asp:Label>
                                                </div>
                                            </div>
                                            <div class="col-sm-6 text-right">
                                                <div class="input-group">
                                                    <asp:Label ID="Label1" runat="server" Text="Note : Ensure that items to post is for clinic use only not for selling(if not please uncheck)"></asp:Label>
                                                    &nbsp;&nbsp;
                                                </div>
                                            </div>
                                             <div class="col-sm-1 col-1 text-right">
                                                 <div class="input-group" >
                                                     <asp:Button ID="btnPost" runat="server" Text="P O S T" OnClientClick="ShowProgress()" CssClass="btn btn-sm btn-primary" OnClick="btnPost_Click"  />
                                                 </div>
                                             </div>
       
                                    </div>
                
                                    <div style="margin-bottom: 10px" class="input-group">
                                            <div class="col-md-2 col-2 text-left">
                                               Source :
                                            </div>
                                            <div class="col-sm-9">
                                                <div class="input-group">
                                                    <asp:Label ID="lblSource" runat="server" Text="DERMESSE, INC."></asp:Label>
                                                </div>
                                            </div>
                                    </div>

                               
                                <div style="margin-bottom: 10px" class="input-group">
                                            <div class="col-md-2 col-2 text-left">
                                                <asp:Label ID="Label3" runat="server" Text="DR Number" style="width:125px;"></asp:Label>
                                            </div>
                                             <div class="col-lg-3 col-3">
                                                 <div class="input-group" >
                                                     <asp:TextBox ID="txtDRNumber" runat="server" CssClass="form-control" AutoCompleteType="Disabled"></asp:TextBox>
                                                     <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="txtDRNumber"
                                                         ForeColor="Red" ValidationGroup="grpDownload" Display="Dynamic"
                                                         ErrorMessage="Please supply DR Number"></asp:RequiredFieldValidator>
                                                 </div>
                                             </div>
                                          <div class="col-lg-4 col-4">
                                          </div>
                                            
                                     </div>

                                 <div style="margin-bottom: 5px" class="input-group">
                                            <div class="col-md-2 col-2 text-left">
                                
                                            </div>
                                             <div class="col-lg-5 col-5">
                                                 <div class="input-group" >
                                                     <asp:Button ID="btnDownload" runat="server" Text="DOWNLOAD"  CssClass="btn btn-secondary"  ValidationGroup="grpDownload" OnClientClick="if(Page_ClientValidate()) ShowProgress()" OnClick="btnDownload_Click" />
                                                     &nbsp;&nbsp;&nbsp;
                                                     <asp:Button ID="btnClear" runat="server" Text="C L E A R" CssClass="btn btn-danger" OnClick="btnClear_Click" />
                                                     
               
                                                 </div>
                                             </div>
                                             
                                     </div>

                            </div>

                              <div class="card-body">
                                <div class="col-sm-12">
                                    <div class="col-sm-12 text-center">
                                        <asp:GridView ID="gvDeliveries" runat="server" Width="99%" Font-Size="Medium" class="table table-bordered table-hover" AutoGenerateColumns="false" DataKeyNames="ID">
                                    <Columns>
                                       <asp:TemplateField HeaderText="Select Item">
                                            <HeaderStyle HorizontalAlign="Center" />
                                            <ItemStyle HorizontalAlign="Center" Width="4%" />
                                            <ItemTemplate>
                                                <asp:CheckBox ID="ckStat" runat="server" Checked="true" />
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:BoundField DataField="ID" HeaderText="RecID" ReadOnly="True" ItemStyle-CssClass="hiddencol" HeaderStyle-CssClass="hiddencol">
                                            <HeaderStyle Width="3%" HorizontalAlign="Center" />
                                            <ItemStyle HorizontalAlign="Center" />
                                        </asp:BoundField>
                                        
                                          

                                        <asp:BoundField DataField="BrCode" HeaderText="BrCode" ReadOnly="True" ItemStyle-CssClass="hiddencol" HeaderStyle-CssClass="hiddencol">
                                            <HeaderStyle Width="3%" HorizontalAlign="Center" />
                                            <ItemStyle HorizontalAlign="Center" />
                                        </asp:BoundField>

                                        
                                        <asp:BoundField DataField="DR_Number" HeaderText="DR No." ReadOnly="True">
                                            <HeaderStyle Width="5%" HorizontalAlign="Center" />
                                            <ItemStyle HorizontalAlign="Center" />
                                        </asp:BoundField>

                                         <asp:BoundField DataField="MRFNo" HeaderText="MRF No." ReadOnly="True">
                                            <HeaderStyle Width="10%" HorizontalAlign="Center" />
                                            <ItemStyle HorizontalAlign="Center" />
                                        </asp:BoundField>
                    
                                        <asp:BoundField HeaderText="Itemcode" DataField="Sup_ItemCode"  ReadOnly="True" ItemStyle-Width="10%" HeaderStyle-VerticalAlign="Middle" HeaderStyle-HorizontalAlign="Center">
                                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                            <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                        </asp:BoundField>

                                        <asp:BoundField HeaderText="Description" DataField="sup_DESCRIPTION"  ReadOnly="True"  ItemStyle-Width="30%" HeaderStyle-VerticalAlign="Middle" HeaderStyle-HorizontalAlign="Center">
                                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                            <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                        </asp:BoundField>

                                      

                                           <asp:TemplateField  HeaderText="Qty" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="5%" ItemStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblQty" runat="server" Text='<%# Eval("vQty") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>

                                                 <asp:BoundField HeaderText="Batch No" DataField="vBatchNo"  ReadOnly="True"  ItemStyle-Width="10%" HeaderStyle-VerticalAlign="Middle" HeaderStyle-HorizontalAlign="Center">
                                                    <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                                    <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                                </asp:BoundField>

                                             <asp:BoundField HeaderText="Date Expiry" DataField="vDateExpiry" DataFormatString="{0:MM/dd/yyyy}"  ReadOnly="True"  ItemStyle-Width="5%" HeaderStyle-VerticalAlign="Middle" HeaderStyle-HorizontalAlign="Center">
                                                    <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                                    <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                                </asp:BoundField>

                                                   <asp:BoundField DataField="FGCode" HeaderText="FGCode" ReadOnly="True" ItemStyle-CssClass="hiddencol" HeaderStyle-CssClass="hiddencol">
                                                    <HeaderStyle Width="3%" HorizontalAlign="Center" />
                                                    <ItemStyle HorizontalAlign="Center" />
                                                </asp:BoundField>

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
                title: "Delivery from Plant",
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
