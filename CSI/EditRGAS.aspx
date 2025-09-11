<%@ Page Title="Edit RGAS" Language="C#" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true" CodeBehind="EditRGAS.aspx.cs" Inherits="CSI.EditRGAS" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
        <link href="plugins/jquery-ui/jquery-ui.css" rel="stylesheet" />
    <link rel="stylesheet" href="docsupport/prism.css" />
    <link rel="stylesheet" href="chosen.css" />


    <style type="text/css">
        .modal {
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
            0% {
                -webkit-transform: rotate(0deg);
            }

            100% {
                -webkit-transform: rotate(360deg);
            }
        }

        @keyframes spin {
            0% {
                transform: rotate(0deg);
            }

            100% {
                transform: rotate(360deg);
            }
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
                    <h5>RGAS Data Entry</h5>
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
                                    <asp:Label ID="Label11" runat="server" Text="Item Description" style="width:125px;"></asp:Label>
                                </div>
                                    <div class="col-lg-6 col-6">
                                        <div class="input-group">
                                        <asp:DropDownList ID="ddItem" runat="server"  CssClass="form-control chosen-select" required="required" AutoPostBack="True" OnSelectedIndexChanged="ddItem_SelectedIndexChanged"></asp:DropDownList>
                                                                        
                                        </div>
                                    </div>
                                    <div class="col-sm-4 col-4">
                                            <div class="input-group" >
                                                <asp:Button ID="btnShow" runat="server" Text="Show Batches"  ValidationGroup="grpShow" CssClass="btn btn-sm btn-secondary" OnClick="btnShow_Click"  />
                                                <asp:RequiredFieldValidator ID="Req5" runat="server" ControlToValidate="ddItem"  Display="Dynamic" InitialValue="0" ForeColor="Red" ValidationGroup="grpShow"
                                                    ErrorMessage="Please select item"></asp:RequiredFieldValidator>
                                        </div>
                                        </div>
                            </div>


                             <div style="margin-bottom: 5px" class="input-group">
                                    <div class="col-md-2 col-2 text-left">
                                        <asp:Label ID="Label2" runat="server" Text="ID" style="width:125px;"></asp:Label>
                                    </div>
                                     <div class="col-lg-1 col-1">
                                          <div class="input-group">
                                               <asp:TextBox ID="txtID" runat="server" class="form-control"  ReadOnly="true" onkeydown="return (event.keyCode!=13);" autocomplete="off" AutoCompleteType="Disabled"></asp:TextBox>
                                         </div>
                                     </div>
                                  <div class="col-lg-6 col-6">
                                          
                                  </div>
                             </div>

                            <div style="margin-bottom: 5px" class="input-group">
                                    <div class="col-md-2 col-2 text-left">
                                        <asp:Label ID="Label1" runat="server" Text="Balance" style="width:125px;"></asp:Label>
                                    </div>
                                     <div class="col-lg-2 col-2">
                                          <div class="input-group">
                                               <asp:TextBox ID="txtBalance" runat="server" class="form-control"  ReadOnly="true" onkeydown="return (event.keyCode!=13);" autocomplete="off" AutoCompleteType="Disabled"></asp:TextBox>
                                         </div>
                                     </div>
                                  <div class="col-lg-6 col-6">
                                          
                                  </div>
                             </div>

                            <div style="margin-bottom: 5px" class="input-group">
                                    <div class="col-md-2 col-2 text-left">
                                        <asp:Label ID="Label3" runat="server" Text="Batch No." style="width:125px;"></asp:Label>
                                    </div>
                                     <div class="col-lg-3 col-3">
                                          <div class="input-group">
                                               <asp:TextBox ID="txtBatchNo" runat="server" class="form-control"  ReadOnly="true" onkeydown="return (event.keyCode!=13);" autocomplete="off" AutoCompleteType="Disabled"></asp:TextBox>
                                         </div>
                                     </div>
                                  <div class="col-lg-6 col-6">
                                          
                                  </div>
                             </div>

                            <div style="margin-bottom: 10px" class="input-group">
                                    <div class="col-md-2 col-2 text-left">
                                        <asp:Label ID="Label4" runat="server" Text="Date Expiry" style="width:125px;"></asp:Label>
                                    </div>
                                     <div class="col-lg-2 col-2">
                                          <div class="input-group">
                                               <asp:TextBox ID="txtDateExpiry" runat="server" class="form-control" ReadOnly="true" onkeydown="return (event.keyCode!=13);" autocomplete="off" AutoCompleteType="Disabled"></asp:TextBox>
                                         </div>
                                     </div>
                                  <div class="col-lg-6 col-6">
                                          
                                  </div>
                             </div>

                            <div style="margin-bottom: 15px" class="input-group">
                                    <div class="col-md-2 col-2 text-left">
                                        <asp:Label ID="Label5" runat="server" Text="New Date Expiry" style="width:125px;"></asp:Label>
                                    </div>
                                     <div class="col-lg-2 col-2">
                                          <div class="input-group">
                                               <asp:TextBox ID="txtNewDateExpiry" runat="server" class="form-control txtExpiry" onkeydown="return (event.keyCode!=13);" autocomplete="off" AutoCompleteType="Disabled"></asp:TextBox>
                                         </div>
                                     </div>
                                  <div class="col-lg-6 col-6">
                                          <asp:RequiredFieldValidator ID="ReqDate" runat="server"
                                            ControlToValidate="txtNewDateExpiry" Display="Dynamic" ValidationGroup="grpRGAS" ForeColor="Red"
                                            ErrorMessage="Required"></asp:RequiredFieldValidator>
                                        <asp:RegularExpressionValidator ID="REVdate" runat="server" ControlToValidate="txtNewDateExpiry"
                                            ValidationExpression="^([1-9]|0[1-9]|1[0-2])[- / .]([1-9]|0[1-9]|1[0-9]|2[0-9]|3[0-1])[- / .](1[9][0-9][0-9]|2[0][0-9][0-9])$"
                                            ForeColor="Red"
                                            ErrorMessage="Invalid date format"
                                            ValidationGroup="grpRGAS"></asp:RegularExpressionValidator>
                                  </div>
                             </div>

                            <div style="margin-bottom: 15px" class="input-group">
                                    <div class="col-md-2 col-2 text-left">
                                        
                                    </div>
                                     <div class="col-lg-2 col-2">
                                          <div class="input-group">
                                               <asp:Button ID="btnPost" runat="server" Text="P O S T" CssClass="btn btn-primary" ValidationGroup="grpRGAS" OnClientClick="if(Page_ClientValidate()) ShowProgress()" OnClick="btnPost_Click"/>
                                         </div>
                                     </div>
                                  <div class="col-lg-6 col-6">
                                  </div>
                             </div>

                            
                            

                        </div>
                    </div>
                </div>
            </div>
        </div>
    </section>

<div class="loading" align="center" style="margin-top: 100px;">
        <br />
        <br />
        <%--<img src="images/ajax-loader.gif" alt=""  />--%>
        <div class="loader"></div>
        <br />
        <asp:Label ID="Label12" runat="server" Text="Please wait" ForeColor="Red"></asp:Label>

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
                title: "Edit RGAS",
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

<%--*******************************************************************************--%>
    <script type="text/javascript">
        function CloseGridItemBatch() {
            $(function () {
                $("#ShowItemBatch").dialog('close');
            });
        }
    </script>

    <script type="text/javascript">
        function ShowGridItemBatch() {
            $(function () {
                $("#ShowItemBatch").dialog({
                    title: "Item Batch",

                    create: function (event, ui) {
                        $(event.target).parent().css('position', 'fixed');
                    },

                    width: '650px',
                    buttons: {
                        Close: function () {
                            $(this).dialog('close');
                        }
                    },
                    modal: true

                });
                $("#ShowItemBatch").parent().appendTo($("form:first"));
            });
        }
    </script>
    

    <div id="ShowItemBatch" style="display:none;">
                <div style="overflow: auto; max-height: 400px;">
                <asp:GridView ID="gvItemBatch" runat="server" AutoGenerateColumns="false" OnRowUpdating="gvItemBatch_RowUpdating">
                    <Columns>
                         <asp:BoundField HeaderText="Rec No." DataField="HeaderID" ItemStyle-Width="95px">
                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                            <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                        </asp:BoundField>
                        
                        <asp:BoundField DataField="vRecDate" DataFormatString="{0:MM/dd/yyyy}" HeaderText="Date Received" ReadOnly="True">
                            <HeaderStyle Width="115px" HorizontalAlign="Center" />
                            <ItemStyle HorizontalAlign="Center"  />
                        </asp:BoundField>

                           <asp:BoundField HeaderText="Batch No." DataField="vBatchNo"  HtmlEncode="false" ItemStyle-Width="155px">
                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                            <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                        </asp:BoundField>

                          <asp:BoundField DataField="vDateExpiry" DataFormatString="{0:MM/dd/yyyy}" HeaderText="Date Expiry" ReadOnly="True">
                            <HeaderStyle Width="115px" HorizontalAlign="Center" />
                            <ItemStyle HorizontalAlign="Center"  />
                        </asp:BoundField>

         
                          <asp:TemplateField HeaderText="Balance" ItemStyle-Width="85px" HeaderStyle-HorizontalAlign="Center">
                                    <ItemTemplate>
                                        <asp:TextBox ID="txtBalance" runat="server" Width="85px" Text='<%# Bind("Balance","{0:###0;(###0);0}") %>' Enabled="false" BorderStyle="None" Style="text-align: center; background-color: transparent; border-width: 0px;"></asp:TextBox>
                                    </ItemTemplate>
                                    <HeaderStyle HorizontalAlign="Center" />
                                    <ItemStyle HorizontalAlign="Center" />
                                </asp:TemplateField>



                        <asp:TemplateField>
                            <ItemStyle HorizontalAlign="Center" Width="175px" />
                            <ItemTemplate>
                                <asp:Button ID="btnSelectItemID" runat="server" Text="Select Batch" CssClass="btn btn-success"  ValidationGroup='<%# "Grp-" + Container.DataItemIndex %>' CommandName="Update"/>
                        
                            </ItemTemplate>
                        </asp:TemplateField>

                

                    </Columns>
                </asp:GridView>
            </div>

    </div>

</asp:Content>
