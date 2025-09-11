<%@ Page Title="CSI System | Encode Request" MaintainScrollPositionOnPostback="true" Language="C#" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true" CodeBehind="ClinicSuppliesRequest.aspx.cs" Inherits="CSI.ClinicSuppliesRequest" %>
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
                    <h5>Clinic Supplies Request Data Entry</h5>
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
                                        <asp:Label ID="Label1" runat="server" Text="To " style="width:125px;"></asp:Label>
                                         </div>
                                     <div class="col-md-9 col-9">
                                         <div class="input-group" >
                                            <asp:Label ID="lblCompany" runat="server" Text="DCI - LOGISTICS"></asp:Label>
                                        </div>
                                     </div>

                                     <div class="col-md-1 col-1 text-right">
                                         <div class="input-group" >
                                             <asp:Button ID="btnPost" runat="server" Text="SUBMIT"  CssClass="btn btn-sm btn-primary" ValidationGroup="GRPp" OnClientClick="ShowProgress()" OnClick="btnPost_Click"/>
                                         </div>
                                     </div>

                                 </div>

                                <div style="margin-bottom: 10px" class="input-group">
                                    <div class="col-md-2 col-2 text-left">
                                        <asp:Label ID="Label2" runat="server" Text="Branch" style="width:125px;"></asp:Label>
                                    </div>
                                     <div class="col-lg-3 col-3">
                                         <div class="input-group" >
                                            <asp:Label ID="lblBranch" runat="server" Text=""></asp:Label>
                                        </div>
                                     </div>
                                 </div>

                                <div style="margin-bottom: 10px" class="input-group">
                                    <div class="col-md-2 col-2 text-left">
                                        <asp:Label ID="Label7" runat="server" Text="Type of Request" style="width:125px;"></asp:Label>
                                    </div>
                                     <div class="col-lg-9 col-9">
                                         <div class="input-group" >
                                             <asp:DropDownList ID="ddRequestType" runat="server" CssClass="form-control" AutoPostBack="True" OnSelectedIndexChanged="ddRequestType_SelectedIndexChanged">
                                                 <asp:ListItem Value="0" Text="Select Type" Selected="True"></asp:ListItem>
                                                 <asp:ListItem Value="Emergency" Text="Emergency"></asp:ListItem>
                                                 <asp:ListItem Value="Regular" Text="Regular"></asp:ListItem>
                                             </asp:DropDownList>
                                             Note : Please separate Emergency and Regular Request per transaction.
                                             <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ControlToValidate="ddRequestType" InitialValue="0" ForeColor="Red" ValidationGroup="grpSave"
                                                 ErrorMessage="Please select type"></asp:RequiredFieldValidator>
                                         </div>
                                     </div>
                             </div>


                                <div style="margin-bottom: 10px" class="input-group">
                                    <div class="col-md-2 col-2 text-left">
                                        <asp:Label ID="Label9" runat="server" Text="Reason of Emergency Request" style="width:125px;"></asp:Label>
                                    </div>
                                     <div class="col-lg-5 col-5">
                                         <div class="input-group" >
                                             <asp:TextBox ID="txtReason" runat="server" CssClass="form-control" ReadOnly="true"></asp:TextBox>
                                             <asp:RequiredFieldValidator ID="RequiredReason" runat="server" ControlToValidate="txtReason" ForeColor="Red" ValidationGroup="grpSave"
                                                 ErrorMessage="Required if Emergency"></asp:RequiredFieldValidator>
                                         </div>
                                     </div>
                             </div>

                                <div style="margin-bottom: 10px" class="input-group">
                                    <div class="col-md-2 col-2 text-left">
                                        <asp:Label ID="Label8" runat="server" Text="Date Required" style="width:125px;"></asp:Label>
                                    </div>
                                     <div class="col-lg-3 col-3">
                                         <div class="input-group" >
                                             <asp:TextBox ID="txtDateRequired" runat="server" CssClass="form-control txtDate"></asp:TextBox>
                                             <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" ControlToValidate="txtDateRequired" ForeColor="Red" ValidationGroup="grpSave"
                                                 ErrorMessage="Please supply"></asp:RequiredFieldValidator>
                                         </div>
                                     </div>
                             </div>

                             
                              <div style="margin-bottom: 10px" class="input-group">
                                    <div class="col-md-2 col-2 text-left">
                                        <asp:Label ID="Label3" runat="server" Text="Category" style="width:125px;"></asp:Label>
                                    </div>
                                     <div class="col-lg-5 col-5">
                                         <div class="input-group" >
                                             <asp:DropDownList ID="ddCategory" runat="server" CssClass="form-control" AutoPostBack="true" OnSelectedIndexChanged="ddCategory_SelectedIndexChanged"></asp:DropDownList>
                                             <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="ddCategory" InitialValue="0" ForeColor="Red" ValidationGroup="grpSave"
                                                 ErrorMessage="Please select category"></asp:RequiredFieldValidator>
                                         </div>
                                     </div>
                             </div>

                             <div style="margin-bottom: 10px" class="input-group">
                                    <div class="col-md-2 col-2 text-left">
                                        <asp:Label ID="Label4" runat="server" Text="Item Description" style="width:125px;"></asp:Label>   
                                    </div>
                                     <div class="col-lg-7 col-7">
                                         <div class="input-group" >
                                             <asp:DropDownList ID="ddItem" runat="server"  CssClass="form-control chosen-select" AutoPostBack="true" OnSelectedIndexChanged="ddItem_SelectedIndexChanged"></asp:DropDownList>
                                             <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="ddItem" InitialValue="0" ForeColor="Red" ValidationGroup="grpSave"
                                                 ErrorMessage="Please select item"></asp:RequiredFieldValidator>
                                         </div>
                                     </div>
                             </div>

                              <div style="margin-bottom: 10px" class="input-group">
                                    <div class="col-md-2 col-2 text-left">
                                        <asp:Label ID="Label5" runat="server" Text="Balance" style="width:125px;"></asp:Label>   
                                    </div>
                                     <div class="col-lg-1 col-1">
                                         <div class="input-group" >
                                             <asp:TextBox ID="txtBalance" runat="server" CssClass="form-control" Width="105px" ReadOnly="true"></asp:TextBox>
                                         </div>
                                     </div>
                             </div>

                             <div style="margin-bottom: 10px" class="input-group">
                                    <div class="col-md-2 col-2 text-left">
                                        <asp:Label ID="Label6" runat="server" Text="No. Of Request" style="width:125px;"></asp:Label>   
                                    </div>
                                     <div class="col-lg-2 col-2">
                                         <div class="input-group" >
                                             <asp:TextBox ID="txtQty" runat="server" CssClass="form-control decimalnumbers-only" Width="80px" ValidationGroup="grpSave"  AutoCompleteType="Disabled"></asp:TextBox>
                                             <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtQty" ForeColor="Red" ValidationGroup="grpSave"
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
                                <asp:GridView ID="gvRequest" runat="server" Width="100%"  ClientIDMode="Static" class="table table-bordered table-hover" DataKeyNames="Sup_RequestID" AutoGenerateColumns="false" EmptyDataText="No Record Found" OnPreRender="gvRequest_PreRender" OnRowDeleting="gvRequest_RowDeleting" >
                                        <Columns>

                                            <asp:BoundField DataField="Sup_RequestID" HeaderText="ID" ItemStyle-Width="3%" HeaderStyle-HorizontalAlign="Center" HeaderStyle-CssClass="hiddencol" ItemStyle-CssClass="hiddencol">
                                                <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                            </asp:BoundField>
                                           <%--    <asp:TemplateField HeaderStyle-CssClass="hiddencol" ItemStyle-CssClass="hiddencol">
                                                    <ItemStyle HorizontalAlign="Center" Width="5%" />
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblRequestID" runat="server" Text='<%#Eval("Sup_RequestID")%>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>--%>


                                            <asp:BoundField DataField="Sup_ItemCode" HeaderText="Itemcode" ItemStyle-Width="5%" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                                <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                                <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                            </asp:BoundField>

                                            <asp:BoundField DataField="sup_DESCRIPTION" HeaderText="Item Description" ItemStyle-Width="35%" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                                <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                                <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                            </asp:BoundField>

                                              <asp:BoundField DataField="Sup_CategoryName" HeaderText="Category" ItemStyle-Width="10%" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                                <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                                <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                            </asp:BoundField>

                                              <asp:BoundField DataField="Sup_Balance" HeaderText="Balance" DataFormatString="{0:#,##0.##;(#,##0.##);0}" ItemStyle-Width="5%" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                                <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                                <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                            </asp:BoundField>

                                            <asp:BoundField DataField="Sup_Qty" HeaderText="Qty Request" DataFormatString="{0:#,##0.##;(#,##0.##);0}" ItemStyle-Width="10%" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                                <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                                <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                            </asp:BoundField>

                                              <asp:BoundField DataField="sup_UOM" HeaderText="U.O.M" ItemStyle-Width="5%" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                                <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                                <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                            </asp:BoundField>

                                            <asp:BoundField DataField="Sup_EncodedBy" HeaderText="Encoded By" ItemStyle-Width="10%" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                                <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                                <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                            </asp:BoundField>

                                           <asp:BoundField DataField="RequestType" HeaderText="Request Type" ItemStyle-Width="10%" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                                <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                                <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                            </asp:BoundField>

                                                 <asp:BoundField DataField="ReasonOfEmergency" HeaderText="Reason Of Emergency" ItemStyle-Width="20%" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                                <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                                <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                            </asp:BoundField>

                                               <asp:TemplateField>
                                                    <ItemStyle HorizontalAlign="Center" Width="5%" />
                                                    <ItemTemplate>
                                                        <asp:LinkButton ID="lnkView" runat="server" CommandName="Delete" class="btn btn-sm btn-danger" CommandArgument='<%#Eval("Sup_RequestID") %>'><i class="fa fa-trash"></i></asp:LinkButton>
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

     <div class="loading" align="center" style="margin-top: 100px;">
        <br />
        <br />
        <%--<img src="images/ajax-loader.gif" alt=""  />--%>
        <div class="loader"></div>
        <br />
        <asp:Label ID="Label12" runat="server" Text="Please wait" ForeColor="Red"></asp:Label>

    </div>


    <script src="docsupport/jquery-3.2.1.min.js" type="text/javascript"></script>
    <script src="chosen.jquery.js" type="text/javascript"></script>
    <script src="docsupport/prism.js" type="text/javascript" charset="utf-8"></script>
    <script src="docsupport/init.js" type="text/javascript" charset="utf-8"></script>

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
            //yearRange: "-1:+1"
        });

    });
</script>

    <script>
        $(function () {
            $("#<%=gvRequest.ClientID%>").DataTable({
                "responsive": true,
                "lengthChange": false,
                "autoWidth": false,
                "ordering": false,
                "bSortable": false, "aTargets": [0],
                //"buttons": ["excel", "pdf", "print"],
                "iDisplayLength": -1,

                "buttons": [
                               {
                

                                   extend: 'excelHtml5',
                                   exportOptions: {
                                    columns: [1, 2, 3, 4, 5, 6 ]
                                   },
                                   title:  document.getElementById('<%= lblBranch.ClientID %>').innerHTML,
                                   message: "Supplies Request"


                                },

                                    {
                                        extend: 'pdfHtml5',
                                        exportOptions: {
                                            columns: [1, 2, 3, 4, 5, 6]
                                        },
                                       title:  document.getElementById('<%= lblBranch.ClientID %>').innerHTML,
                                       message: "Supplies Request"
                                    }


                ],

                "responsive": true,
            }).buttons().container().appendTo('#gvRequest_wrapper .col-md-6:eq(0)');;
        });
</script>
        
<!-- ################################################# END #################################################### -->

<script type="text/javascript">
    function ShowSuccessMsg() {
        $(function () {
            $("#messageDiv").dialog({
                title: "Encoding of Request",
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

    <%--*******************************************************************************--%>
<script type="text/javascript">
    function ShowConfirmMsg() {
        $(function () {
            $("#messageConfirm").dialog({
                title: "Delete Item",
                width: '420px',
                buttons: {
                    Close: function () {
                        $(this).dialog('close');
                    }
                },
                modal: true
            });
            $("#messageConfirm").parent().appendTo($("form:first"));
        });
    }
    </script>

    <div id="messageConfirm" style="display: none;">
        <%--<asp:UpdatePanel ID="UpdatePanel4" runat="server">
            <ContentTemplate>--%>
              <div style="display:none;">
                <asp:Label Text="" ID="lblConfirmRecID" runat="server" ForeColor="Maroon" />
               
               </div>

                <asp:Label Text="" ID="lblItem" runat="server" ForeColor="Maroon" />
                <br />
                <asp:Label Text="" ID="lblMsgConfirm" runat="server" />
                &nbsp;&nbsp;&nbsp;
                <asp:Button ID="btnDeleteItem" runat="server" Text="YES/Proceed" CssClass="btn btn-sm btn-success" OnClientClick="ShowProgress()"  OnClick="btnDeleteItem_Click" />
            <%--</ContentTemplate>
        </asp:UpdatePanel>--%>
    </div>

<%--*******************************************************************************--%>

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




    <!-- ################################################# END #################################################### -->

</asp:Content>
