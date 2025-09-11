<%@ Page Title="Transfer Item" Language="C#" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true" CodeBehind="TransferItem.aspx.cs" Inherits="CSI.TransferItem" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
<link href="plugins/jquery-ui/jquery-ui.css" rel="stylesheet" />
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

  <link rel="stylesheet" href="docsupport/prism.css" />
    <link rel="stylesheet" href="chosen.css" />
    <style type="text/css">
        table th {
            text-align: center;
            vertical-align: middle;
            background-color: #f2f2f2;
            font-size: 12px;
        }

        table tr {
            vertical-align: middle;
            font-size: 12px;
        }

        .hiddencol {
            display: none;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<asp:scriptmanager runat="server"></asp:scriptmanager>
<section class="content-header">
        <div class="container-fluid">
            <div class="row mb-1">
                <div class="col-sm-6">
                    <h5>Transfer Inventory per Category</h5>
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
                                                   Source Category
                                    </div>
                                        <div class="col-sm-3">
                                            <div class="input-group">
                                                <asp:DropDownList ID="ddCategorySource" CssClass="form-control"  Width="205px" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddCategorySource_SelectedIndexChanged" ></asp:DropDownList>
                                                    
                                            </div>
                                        </div>
                                    </div>          

                                  <div style="margin-bottom: 5px" class="input-group">
                                         <div class="col-md-2 col-2 text-left">
                                                       Target Category
                                        </div>
                                            <div class="col-sm-5">
                                                <div class="input-group">
                                                    <asp:DropDownList ID="ddCategoryTarget" CssClass="form-control"  Width="205px" runat="server" AutoPostBack="True" ></asp:DropDownList>
                                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="ddCategoryTarget"  Display="Dynamic" InitialValue="0" ForeColor="Red" ValidationGroup="grpShow"
                                                                ErrorMessage="Please select category"></asp:RequiredFieldValidator>
                                                </div>
                                            </div>
                                    </div>          
                                    
                                        <div style="margin-bottom: 5px" class="input-group">
                                            <div class="col-md-2 col-2 text-left">
                                                <asp:Label ID="Label11" runat="server" Text="Item Description" style="width:125px;"></asp:Label>
                                            </div>
                                                <div class="col-lg-6 col-6">
                                                    <div class="input-group">
                                                    <asp:DropDownList ID="ddItem" runat="server"  CssClass="form-control chosen-select" required="required" ></asp:DropDownList>
                                                                        
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
                title: "Issuance for selling",
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
            //maxDate: new Date,
            minDate: new Date(2007, 6, 12),
            changeMonth: true,
            changeYear: true,
            yearRange: "-1:+0"
        });

    });
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
                    <asp:Label ID="lblItemtoTransfer" runat="server" Text=""></asp:Label>
                 <br />
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

                        
                        <asp:TemplateField HeaderText="Date Expiry">
                                            <HeaderStyle HorizontalAlign="Center" Width="5%" />
                                            <ItemStyle HorizontalAlign="Center" />
                                            <ItemTemplate>
                                                       <asp:Label ID="lblvDateExpiry" runat="server" Text='<%#  Convert.ToString(Eval("vDateExpiry", "{0:MM/dd/yyyy}")).Equals("01/01/1900")?"":Eval("vDateExpiry", "{0:MM/dd/yyyy}")%>'></asp:Label>
                                             </ItemTemplate>
                        </asp:TemplateField>

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


                        <asp:TemplateField HeaderText="Action">
                            <ItemStyle HorizontalAlign="Center" Width="175px" />
                            <ItemTemplate>
                                <asp:Button ID="btnSelectItemID" runat="server" Text="Transfer" CssClass="btn btn-success"  ValidationGroup='<%# "Grp-" + Container.DataItemIndex %>' CommandName="Update"/>
                        
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
