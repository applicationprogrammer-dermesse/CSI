<%@ Page Title="CSI System | Home Page" Language="C#" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true" CodeBehind="HomePage.aspx.cs" Inherits="CSI.HomePage" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <section class="content-header">
        <div class="container-fluid">
            <div class="row mb-2">
                <div class="col-sm-6">
                    <h3>Home Page</h3>
                </div>
                <div class="col-sm-6">
                    <ol class="breadcrumb float-sm-right">
                        <%--  <li class="breadcrumb-item"><a href="#">Home</a></li>
              <li class="breadcrumb-item active">Dashboard</li>--%>
                    </ol>
                </div>
            </div>
        </div>
        <!-- /.container-fluid -->
    </section>



    <section class="content">
        <div class="container-fluid">
            <!-- Small boxes (Stat box) ge badge-secondary bg-gradient-fuchsia-->
            <div class="row">

                <div class="col">
                    <div class="small-box badge-dark">
                        <a href="ListForApprovalBranchHead.aspx">
                            <div class="inner" style="margin-left: 5px;">
                                <h3>
                                    <asp:Label ID="lblForApprovalBrHead" runat="server" Text="0" style="color:white;"></asp:Label></h3>

                                <p style="color:white;">For BM/ASM Approval</p>
                            </div>

                            <div class="icon">
                                <i class="ion ion-bag"></i>
                            </div>
                        </a>
                        <a href="ListForApprovalBranchHead.aspx" class="small-box-footer">Go to list <i class="fas fa-arrow-circle-right"></i></a>
                    </div>
                </div>

                 <div class="col">
                    <div class="small-box bg-info">
                        <a href="ListOfForApproval.aspx">
                            <div class="inner" style="margin-left: 5px;">
                                <h3>
                                    <asp:Label ID="lblForApproval" runat="server" Text="0" style="color:white;"></asp:Label></h3>

                                <p style="color:white;">For Approval - Operations</p>
                            </div>

                            <div class="icon">
                                <i class="ion ion-bag"></i>
                            </div>
                        </a>
                        <a href="ListOfForApproval.aspx" class="small-box-footer">Go to list <i class="fas fa-arrow-circle-right"></i></a>
                    </div>
                </div>


                <div class="col">
                    <div class="small-box bg-gradient-fuchsia">
                        <a href="ListOfForApprovalLogistics.aspx">
                            <div class="inner" style="margin-left: 5px;">
                                <h3>
                                    <asp:Label ID="lblForApprovalLogistics" runat="server" Text="0" style="color:white;"></asp:Label></h3>

                                <p style="color:white;">Approved</p>
                            </div>

                            <div class="icon">
                                <i class="ion ion-bag"></i>
                            </div>
                        </a>
                        <a href="ListOfForApprovalLogistics.aspx" class="small-box-footer">Go to list <i class="fas fa-arrow-circle-right"></i></a>
                    </div>
                </div>


                <div class="col">
                    <!-- small box -->
                    <div class="small-box bg-success">
                        <a href="ListOfApprovedRequest.aspx">
                            <div class="inner" style="margin-left: 5px;">
                                <h3>
                                    <asp:Label ID="lblApproved" runat="server" Text="0"></asp:Label></h3>

                                <p>Pick Order</p>
                            </div>
                            <div class="icon">
                                <i class="ion ion-stats-bars"></i>
                            </div>
                        </a>
                        <a href="ListOfApprovedRequest.aspx" class="small-box-footer">Go to list <i class="fas fa-arrow-circle-right"></i></a>
                    </div>
                </div>
                <!-- ./col -->
                <div class="col">
                    <!-- small box -->
                    <div class="small-box bg-warning">
                        <a href="ListForIssuance.aspx">
                            <div class="inner" style="margin-left: 5px;">
                                <h3>
                                    <asp:Label ID="lblForIssuance" runat="server" Text="0"></asp:Label></h3>

                                <p>Pack Order</p>
                            </div>
                            <div class="icon">
                                <i class="ion ion-person-add"></i>
                            </div>
                        </a>
                        <a href="ListForIssuance.aspx" class="small-box-footer">Go to list <i class="fas fa-arrow-circle-right"></i></a>
                    </div>
                </div>
                <!-- ./col -->
                <div class="col">
                    <!-- small box -->
                    <div class="small-box bg-danger">
                        <a href="ListUnpostedIssuance.aspx">
                            <div class="inner" style="margin-left: 5px;">
                                <h3>
                                    <asp:Label ID="lblUnpostedIssuance" runat="server" Text="0"></asp:Label></h3>

                                <p>Unposted Issuance</p>
                            </div>
                            <div class="icon">
                                <i class="ion ion-pie-graph"></i>
                            </div>
                        </a>
                        <a href="ListUnpostedIssuance.aspx" class="small-box-footer">Go to list <i class="fas fa-arrow-circle-right"></i></a>
                    </div>
                </div>
                <!-- ./col -->
            </div>

            <div class="row">
                <div class="col-md-12">
                    <div class="col-sm-12 col-12">
                        <div class="small-box bg-gradient-maroon">
                        <a href="ClinicRequirements.aspx">
                            <div class="inner" style="margin-left: 5px;">
                                <asp:Label ID="lblClinicRequirements" runat="server" Text="" Font-Bold="true" Font-Size="XX-Large" ForeColor="White"></asp:Label>
                                &nbsp;&nbsp;&nbsp;
                                <asp:Label ID="lblC" runat="server" Text="DCI Branches Clinic Supplies Requirements (For Pre-Availed Services)" ForeColor="White"></asp:Label>

                                <%--<p>Clinic Supplies Requirements<br />(Pre-Availed Services)</p>--%>
                            </div>

                            <div class="icon">
                                <i class="ion ion-bag"></i>
                            </div>
                        </a>
                        <a href="ClinicRequirements.aspx" class="small-box-footer">Go to list <i class="fas fa-arrow-circle-right"></i></a>
                    </div>
                </div>
             </div>
            </div>
            <%--START--%>
            <div class="row">
            <div class="col-md-6">
                <div class="card">
                    <div class="card-header">
                        <h3 class="card-title">
                            <i class="ion ion-clipboard mr-1"></i>
                            To Do/Check List
                </h3>

                        <div class="card-tools">
                            <ul class="pagination pagination-sm">
                                <li class="page-item"><a href="#" class="page-link">&laquo;</a></li>
                                <li class="page-item"><a href="#" class="page-link">1</a></li>
                                <li class="page-item"><a href="#" class="page-link">2</a></li>
                                <li class="page-item"><a href="#" class="page-link">3</a></li>
                                <li class="page-item"><a href="#" class="page-link">&raquo;</a></li>
                            </ul>
                        </div>
                    </div>
                    <!-- /.card-header -->
                    <div class="card-body">

                        <ul class="todo-list" data-widget="todo-list">
                            <li>
                                <!-- drag handle -->
                                <span class="handle">
                                    <i class="fas fa-ellipsis-v"></i>
                                    <i class="fas fa-ellipsis-v"></i>
                                </span>
                                <!-- checkbox -->

                                <!-- todo text -->
                                <span class="text" style="width: 250px;">PRF for Posting</span>
                                <!-- Emphasis label -->
                                <small class="badge badge-primary" style="width: 85px;"><i class="far fa-file-alt"></i>
                                    &nbsp;&nbsp;&nbsp;
                       
                                    <a href="ListPRF.aspx" style="color:white;"><asp:Label ID="lblPRFForPosting" runat="server" Text=""></asp:Label>&nbsp;  items</a></small>
                                <!-- General tools such as edit or delete-->
                                <div class="tools">
                                    <a href="ListPRF.aspx"><i class="far fa-file-alt"></i></a>
                                </div>
                            </li>

                            <li>
                                <!-- drag handle -->
                                <span class="handle">
                                    <i class="fas fa-ellipsis-v"></i>
                                    <i class="fas fa-ellipsis-v"></i>
                                </span>
                                <!-- checkbox -->

                                <!-- todo text -->
                                <span class="text" style="width: 250px;">Stock Transfer for Approval</span>
                                <!-- Emphasis label <div class="small-box bg-gradient-maroon"> -->
                                <small class="badge badge-light bg-gradient-maroon" style="width: 85px;"><i class="far fa-file-alt"></i>
                                    &nbsp;&nbsp;&nbsp;
                       
                                    <a href="ListStockTransfer.aspx" style="color:white;"><asp:Label ID="lblStockTransfer" runat="server" Text=""></asp:Label>&nbsp;  items</a></small>
                                <!-- General tools such as edit or delete-->
                                <div class="tools">
                                    <a href="ListStockTransfer.aspx"><i class="far fa-file-alt"></i></a>
                                </div>
                            </li>


                            <li>
                                <!-- drag handle -->
                                <span class="handle">
                                    <i class="fas fa-ellipsis-v"></i>
                                    <i class="fas fa-ellipsis-v"></i>
                                </span>
                                <!-- checkbox -->

                                <!-- todo text -->
                                <span class="text" style="width: 250px;">Stock Transfer for Posting</span>
                                <!-- Emphasis label -->
                                <small class="badge badge-success" style="width: 85px;"><i class="far fa-file-alt"></i>
                                    &nbsp;&nbsp;&nbsp;
                       
                                    <a href="ListStockTransferForPosting.aspx" style="color:white;"><asp:Label ID="lblStockTransferForPosting" runat="server" Text=""></asp:Label>&nbsp;  items</a></small>
                                <!-- General tools such as edit or delete-->
                                <div class="tools">
                                    <a href="ListStockTransferForPosting.aspx"><i class="far fa-file-alt"></i></a>
                                </div>
                            </li>

                            <li >
                                <span class="handle">
                                    <i class="fas fa-ellipsis-v"></i>
                                    <i class="fas fa-ellipsis-v"></i>
                                </span>

                                <span class="text" style="width: 250px;">RR with PO for Posting</span>
                                <small class="badge badge-secondary" style="width: 85px;"><i class="fas fa-cart-arrow-down"></i>
                                    &nbsp;&nbsp;&nbsp;
                       
                                    <a href="ListRRwithPOforPosting.aspx" style="color:white;"><asp:Label ID="lblRRwithPOforPostingPO" runat="server" Text=""></asp:Label>&nbsp;  Items</a></small>
                                <div class="tools">
                                    <a href="ListRRwithPOforPosting.aspx"><i class="fas fa-cart-arrow-down"></i></a>
                                </div>
                            </li>

                            <li >
                                <span class="handle">
                                    <i class="fas fa-ellipsis-v"></i>
                                    <i class="fas fa-ellipsis-v"></i>
                                </span>

                                <span class="text" style="width: 250px;">RR without PO for Posting</span>
                                <small class="badge badge-secondary bg-gradient-fuchsia" style="width: 85px;"><i class="fas fa-cart-arrow-down"></i>
                                    &nbsp;&nbsp;&nbsp;
                       
                                    <a href="ListRRwithoutPOforPosting.aspx" style="color:white;"><asp:Label ID="blRRwithoutPOforPosting" runat="server" Text=""></asp:Label>&nbsp;  Items</a></small>
                                <div class="tools">
                                    <a href="ListRRwithoutPOforPosting.aspx"><i class="fas fa-cart-arrow-down"></i></a>
                                </div>
                            </li>

                            <li>
                                <span class="handle">
                                    <i class="fas fa-ellipsis-v"></i>
                                    <i class="fas fa-ellipsis-v"></i>
                                </span>

                                <span class="text" style="width: 250px;">Adjustment for Posting</span>
                                <small class="badge badge-warning" style="width: 85px;"><i class="fas fa-cut"></i>
                                    &nbsp;&nbsp;&nbsp;
                       
                                    <a href="ListAdjustment.aspx" style="color:white;"><asp:Label ID="lblAdjustmentForPosting" runat="server" Text=""></asp:Label>&nbsp; items</a></small>
                                <div class="tools">
                                    <a href="ListAdjustment.aspx"><i class="fas fa-cut"></i></a>
                                </div>
                            </li>

                            <li>
                                <span class="handle">
                                    <i class="fas fa-ellipsis-v"></i>
                                    <i class="fas fa-ellipsis-v"></i>
                                </span>

                                <span class="text" style="width: 250px;">Complimentary for Posting</span>
                                <small class="badge badge-secondary bg-gradient-lime" style="width: 85px;"><i class="fas fa-gift"></i>
                                    &nbsp;&nbsp;&nbsp;
                       
                                    <a href="ListComplimentary.aspx" style="color:white;"><asp:Label ID="lblCompliForPosting" runat="server" Text=""></asp:Label>&nbsp;  items</a></small>
                                <div class="tools">
                                    <a href="ListComplimentary.aspx"><i class="fas fa-gift"></i></a>
                                </div>
                            </li>

                            <li >
                                <span class="handle">
                                    <i class="fas fa-ellipsis-v"></i>
                                    <i class="fas fa-ellipsis-v"></i>
                                </span>

                                <span class="text" style="width: 250px;">Purchase Requisition</span>
                                <small class="badge badge-dark" style="width: 85px;"><i class="fas fa-pump-soap"></i>
                                    &nbsp;&nbsp;&nbsp;
                       
                                    <a href="ListPurchaseRequisition.aspx" style="color:white;"><asp:Label ID="lblPurchaseRequisition" runat="server" Text=""></asp:Label>&nbsp;  PR</a></small>
                                <div class="tools">
                                    <a href="ListPurchaseRequisition.aspx"><i class="fas fa-pump-soap"></i></a>
                                </div>
                            </li>

                             

                      
                            <li>
                                <span class="handle">
                                    <i class="fas fa-ellipsis-v"></i>
                                    <i class="fas fa-ellipsis-v"></i>
                                </span>

                                <span class="text" style="width: 250px;">Fall Below Maintaining Balance</span>
                                
                                <small class="badge badge-success bg-gradient-navy" style="width: 85px;"><i class="fas fa-balance-scale-right"></i>
                                    &nbsp;&nbsp;&nbsp;
                       
                                    <a href="ListFallBelow.aspx" style="color:white;"><asp:Label ID="lblFallBelow" runat="server" Text=""></asp:Label>&nbsp;  Items</a></small>
                                <div class="tools">
                                    <a href="ListFallBelow.aspx"><i class="fas fa-balance-scale-right"></i></a>
                                </div>
                            </li>

                            <li>
                                <span class="handle">
                                    <i class="fas fa-ellipsis-v"></i>
                                    <i class="fas fa-ellipsis-v"></i>
                                </span>

                                <span class="text" style="width: 250px;">Near Expiry Items</span>
                                
                                <small class="badge badge-info" style="width: 85px;"><i class="fas fa-battery-quarter"></i>
                                    &nbsp;&nbsp;&nbsp;
                       
                                    <a href="ListNearExpiryItems.aspx" style="color:white;"><asp:Label ID="lblNearExpiryItems" runat="server" ForeColor="White" Text=""></asp:Label>&nbsp;  Items</a></small>
                                <div class="tools">
                                    <a href="ListNearExpiryItems.aspx"><i class="fas fa-battery-quarter"></i></a>
                                </div>
                            </li>

                            <li>
                                <span class="handle">
                                    <i class="fas fa-ellipsis-v"></i>
                                    <i class="fas fa-ellipsis-v"></i>
                                </span>

                                <span class="text" style="width: 250px;">Expired Items</span>
                                
                                <small class="badge badge-danger" style="width: 85px;"><i class="fas fa-battery-empty"></i>
                                    &nbsp;&nbsp;&nbsp;
                       
                                    <a href="ListExpiredItems.aspx" style="color:white;"><asp:Label ID="lblExpired" runat="server" Text="" ForeColor="White"></asp:Label>&nbsp;  Items</a></small>
                                <div class="tools">
                                    <a href="ListExpiredItems.aspx"><i class="fas fa-battery-empty"></i></a>
                                </div>
                            </li>


                        </ul>
                    </div>

                </div>
                   </div>

                <div class="col-md-6">
                    <div class="card">
                        <div class="card-header">
                            <h3 class="card-title">E-Commerce Transaction Status</h3>
                        </div>
                        <div class="card-body">



                            <p></p>
                 
                             <a href="ECommerceEntry.aspx" class="btn btn-app" style="height:80px;">
                                <span class="badge bg-teal"></span>
                                <i class="fas fa-edit"></i>Encode Order<br />(E-Commerce)
                                
                             </a>

                            <a href="OnlineOrderDataEntry.aspx" class="btn btn-app" style="height:80px; display:none;">
                                <span class="badge bg-teal"></span>
                                <i class="fas fa-edit"></i>Encode Order<br />(In-House)
                             </a>

                            <a href="ListOrderOnline.aspx" class="btn btn-app" style="height:80px; display:none;" >
                                <span class="badge bg-teal"><asp:Label ID="lblOnlineOrder" runat="server" Text=""></asp:Label></span>
                                <i class="fas fa-inbox"></i>For print DR
                             </a>

                            <a href="ListInTransitOrder.aspx" class="btn btn-app" style="height:80px;">
                                <span class="badge bg-info"><asp:Label ID="lblIntransit" runat="server" Text=""></asp:Label></span>
                                <i class="fas fa-bicycle"></i>In Transit
                             </a>
                             <a href="ConfirmedDeliveredOrder.aspx" class="btn btn-app" style="height:80px;">
                                <span class="badge bg-success"></span>
                                <i class="fas fa-money-bill-alt"></i>Delivered
                             </a>
                            <a href="CancelledOrder.aspx" class="btn btn-app" style="height:80px;">
                                <span class="badge bg-danger"><asp:Label ID="lblCancelledOrder" runat="server" Text=""></asp:Label></span>
                                <span class="badge bg-success"></span>
                                <i class="far fa-thumbs-down"></i>Cancelled
                             </a>
                            <a href="EcommerceFreeEntry.aspx" class="btn btn-app" style="height:80px;">
                                <%--<span class="badge bg-danger"><asp:Label ID="Label1" runat="server" Text=""></asp:Label></span>--%>
                                <span class="badge bg-success"></span>
                                <i class="fas fa-gift"></i>Free <br />Complimentary
                             </a>
                            <br />
                              <br />
                              <br />
                              <br />
                            <br />
                            <br />
                        </div>
                    </div>

                </div>

           </div>
            <!-- /.card-body -->
            <div class="card-footer clearfix">
            </div>



            <%--END--%>
        </div>
    </section>
</asp:Content>
