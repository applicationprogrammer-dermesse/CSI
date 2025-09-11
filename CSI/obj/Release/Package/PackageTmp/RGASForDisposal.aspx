<%@ Page Title="RGAS For Disposal" Language="C#" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true" CodeBehind="RGASForDisposal.aspx.cs" Inherits="CSI.RGASForDisposal" %>
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
                                    <asp:Label ID="Label3" runat="server" Text="Date" Style="width: 125px;"></asp:Label>
                                </div>
                                <div class="col-sm-2 col-2">
                                    <div class="input-group">
                                        <asp:TextBox ID="txtDate" runat="server" class="form-control txtDate" Width="105px" onkeydown="return (event.keyCode!=13);" autocomplete="off" AutoCompleteType="Disabled"></asp:TextBox>

                                    </div>
                                </div>
                                <div class="col-lg-7 col-7">
                                    <div class="input-group">
                                        <asp:RequiredFieldValidator ID="ReqDate" runat="server"
                                            ControlToValidate="txtDate" Display="Dynamic" ValidationGroup="grpRGAS" ForeColor="Red"
                                            ErrorMessage="Required"></asp:RequiredFieldValidator>
                                        <asp:RegularExpressionValidator ID="REVdate" runat="server" ControlToValidate="txtDate"
                                            ValidationExpression="^([1-9]|0[1-9]|1[0-2])[- / .]([1-9]|0[1-9]|1[0-9]|2[0-9]|3[0-1])[- / .](1[9][0-9][0-9]|2[0][0-9][0-9])$"
                                            ForeColor="Red"
                                            ErrorMessage="Invalid date format"
                                            ValidationGroup="grpRGAS"></asp:RegularExpressionValidator>
                                    </div>

                                </div>
                                <div class="col-lg-1 col-1 text-right">
                                    <div class="input-group">
                                        <asp:Button ID="btnPost" runat="server" Text="P O S T" CssClass="btn btn-primary" ValidationGroup="grpRGAS" OnClientClick="if(Page_ClientValidate()) ShowProgress()" OnClick="btnPost_Click" />
                                    </div>
                                </div>
                            </div>

               
                        
               


                            <div style="margin-bottom: 5px" class="input-group">
                                <div class="col-md-2 col-2 text-left">
                                    <asp:Label ID="Label11" runat="server" Text="Item Description" style="width:125px;"></asp:Label>
                                </div>
                                    <div class="col-lg-6 col-6">
                                        <div class="input-group">
                                        <asp:DropDownList ID="ddItem" runat="server"  CssClass="form-control chosen-select" required="required"></asp:DropDownList>
                                                                        
                                        </div>
                                    </div>
                                    <div class="col-sm-4 col-4">
                                            <div class="input-group" >
                                                <asp:Button ID="btnShow" runat="server" Text="Show Batches"  ValidationGroup="grpShow" CssClass="btn btn-sm btn-secondary" OnClick="btnShow_Click" />
                                                <asp:RequiredFieldValidator ID="Req5" runat="server" ControlToValidate="ddItem"  Display="Dynamic" InitialValue="0" ForeColor="Red" ValidationGroup="grpShow"
                                                    ErrorMessage="Please select item"></asp:RequiredFieldValidator>
                                        </div>
                                        </div>
                            </div>

                        </div>


                        <div class="card-body">
                            <div class="col-sm-12">
                                <asp:GridView ID="gvPicked" CssClass="table table-bordered active active" DataKeyNames="vRecnum" AutoGenerateColumns="false" runat="server" OnRowDeleting="gvPickedItems_RowDeleting">
                                                <Columns>
                                                    <asp:BoundField DataField="vRecnum" HeaderText="ID" ItemStyle-Width="3%" ItemStyle-HorizontalAlign="Center" ItemStyle-CssClass="hiddencol" HeaderStyle-CssClass="hiddencol"/>
                                                    <asp:BoundField DataField="HeaderID" HeaderText="HeaderID" ItemStyle-Width="3%" ItemStyle-HorizontalAlign="Center" ItemStyle-CssClass="hiddencol" HeaderStyle-CssClass="hiddencol"/>
                                                    <asp:BoundField DataField="Sup_ItemCode" HeaderText="Item Code" ItemStyle-Width="5%" ItemStyle-HorizontalAlign="Center" />
                                                    <asp:BoundField DataField="sup_DESCRIPTION" HeaderText="Item Description" ItemStyle-Width="40%" ItemStyle-HorizontalAlign="Center" />
                                                    
                                                    <asp:BoundField HeaderText="Batch No." DataField="vBatchNo"  HtmlEncode="false" ItemStyle-Width="10%">
                                                           <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                                            <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                                        </asp:BoundField>
                                                          <asp:BoundField DataField="vDateExpiry" DataFormatString="{0:MM/dd/yyyy}" HeaderText="Date Expiry" ReadOnly="True">
                                                            <HeaderStyle Width="10%" HorizontalAlign="Center" />
                                                            <ItemStyle HorizontalAlign="Center"  />
                                                        </asp:BoundField>
                            
                                                    <asp:BoundField DataField="vQtyPicked" HeaderText="Qty" ItemStyle-Width="5%" DataFormatString="{0:###0;(###0);0}" ItemStyle-HorizontalAlign="Center" />
                                                    
                                                    
                                                           <asp:TemplateField>
                                                                <ItemStyle HorizontalAlign="Center" Width="5%" />
                                                                <ItemTemplate>
                                                                    <asp:LinkButton ID="btnDeletePicked" runat="server" CommandName="Delete" Text="" class="btn btn-sm btn-danger"><i class="fa fa-trash"></i></asp:LinkButton>
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
                title: "RGAS Data Entry",
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

                    width: '1000px',
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

                        <asp:BoundField DataField="vSource"  HeaderText="Remarks" ReadOnly="True">
                            <HeaderStyle Width="205px" HorizontalAlign="Center" />
                            <ItemStyle HorizontalAlign="Center"  />
                        </asp:BoundField>

                        

                          <asp:TemplateField HeaderText="Balance" ItemStyle-Width="85px" HeaderStyle-HorizontalAlign="Center">
                                    <ItemTemplate>
                                        <asp:TextBox ID="txtBalance" runat="server" Width="85px" Text='<%# Bind("Balance","{0:###0;(###0);0}") %>' Enabled="false" BorderStyle="None" Style="text-align: center; background-color: transparent; border-width: 0px;"></asp:TextBox>
                                    </ItemTemplate>
                                    <HeaderStyle HorizontalAlign="Center" />
                                    <ItemStyle HorizontalAlign="Center" />
                                </asp:TemplateField>

                            <asp:TemplateField HeaderText="Quantity" ItemStyle-Width="50px" HeaderStyle-HorizontalAlign="Center">
                                    <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                    <ItemStyle HorizontalAlign="Center" />
                                    <ItemTemplate>
                                        <asp:TextBox ID="txtQtyPicked" runat="server" CssClass="form-control decimalnumbers-only" Width="50px" AutoCompleteType="Disabled" autocomplete="off"></asp:TextBox>
                                           <asp:RequiredFieldValidator ID="RequiredFieldValidator1" 
                                            runat="server" ControlToValidate="txtQtyPicked" 
                                            Display="Dynamic" ValidationGroup='<%# "Grp-" + Container.DataItemIndex %>' ForeColor="Red"
                                            ErrorMessage="Required"></asp:RequiredFieldValidator>
                                        <asp:CompareValidator ID="CompareValidator1" runat="server" ControlToCompare="txtBalance"
                                            ErrorMessage="Insufficient balance!"
                                            ControlToValidate="txtQtyPicked" Type="Double" Operator="LessThanEqual"
                                            Display="Dynamic" ValidationGroup='<%# "Grp-" + Container.DataItemIndex %>' ForeColor="Red"></asp:CompareValidator>
                                    </ItemTemplate>
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
