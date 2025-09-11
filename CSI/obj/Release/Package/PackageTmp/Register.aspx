<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Register.aspx.cs" Inherits="CSI.Register" %>

<!DOCTYPE html>
<html lang="en">
<head>
  <meta charset="utf-8">
  <meta name="viewport" content="width=device-width, initial-scale=1">
  <title>CSI System | Registration</title>

  <!-- Google Font: Source Sans Pro -->
<%--  <link rel="stylesheet" href="https://fonts.googleapis.com/css?family=Source+Sans+Pro:300,400,400i,700&display=fallback">--%>
  <!-- Font Awesome -->
  <link rel="stylesheet" href="plugins/fontawesome-free/css/all.min.css">
  <!-- icheck bootstrap -->
  <link rel="stylesheet" href="plugins/icheck-bootstrap/icheck-bootstrap.min.css">
  <!-- Theme style -->
  <link rel="stylesheet" href="dist/css/adminlte.min.css">

   <link href="plugins/jquery-ui/jquery-ui.css" rel="stylesheet" />
        <%--<link rel="stylesheet" href="//code.jquery.com/ui/1.12.1/themes/smoothness/jquery-ui.css">--%>
</head>
<body class="hold-transition register-page">
<form id="form1" runat="server">
<div class="register-box">
  <div class="card card-outline card-primary">
    <div class="card-header text-center">
      <a href="#" class="h4"><b>DCI Clinic Supplies</b><br />Inventory System</a>
    </div>
    <div class="card-body">
      <p class="login-box-msg">Register a new account</p>

      <form action="index.html" method="post">
      <%--  <div class="input-group mb-3" style="visibility:hidden;">
          <input type="password" class="form-control" placeholder="Password" style="visibility:hidden;">
          <div class="input-group-append">
            <div class="input-group-text">
              <span class="fas fa-lock"></span>
            </div>
          </div>
        </div>

        <div class="input-group mb-3" style="visibility:hidden;">
          <input type="password" class="form-control" placeholder="Password">
          <div class="input-group-append">
            <div class="input-group-text">
              <span class="fas fa-lock"></span>
            </div>
          </div>
        </div>--%>


        <div class="input-group mb-3">
          <input type="text" runat="server" id="inpEmpNo" class="form-control" placeholder="Employee No.">
          <div class="input-group-append">
            <div class="input-group-text">
              <span class="fas fa-id-badge"></span>
            </div>
          </div>
        </div>

        <div class="input-group mb-3">
          <input type="text" runat="server" id="inpEmpName" class="form-control" placeholder="Full name">
          <div class="input-group-append">
            <div class="input-group-text">
              <span class="fas fa-user"></span>
            </div>
          </div>
        </div>
        
        <div class="input-group mb-3">
          <input type="password" runat="server"  id="inpPassword" class="form-control" placeholder="Password">
          <div class="input-group-append">
            <div class="input-group-text">
              <span class="fas fa-lock"></span>
            </div>
          </div>
        </div>
        
        <div class="input-group mb-3">
         <asp:DropDownList ID="ddDepartment" runat="server"  class="form-control"></asp:DropDownList>
          <div class="input-group-append">
            <div class="input-group-text">
              <span class="fas fa-lock"></span>
            </div>
          </div>
        </div>

        <div class="row">
          <div class="col-8">
            
          </div>
          <!-- /.col -->
          <div class="col-4">
             <asp:Button ID="btnRegister" runat="server" class="btn btn-primary btn-block" Text="Register" OnClick="btnRegister_Click" />
          </div>
          <!-- /.col -->
        </div>
      </form>

      

      <a href="Login.aspx" class="text-center">Go to Log in page</a>
    </div>
    <!-- /.form-box -->
  </div><!-- /.card -->
</div>
<!-- /.register-box -->

<!-- jQuery -->
<script src="plugins/jquery/jquery.min.js"></script>
<!-- Bootstrap 4 -->
<script src="plugins/bootstrap/js/bootstrap.bundle.min.js"></script>
<!-- AdminLTE App -->
<script src="dist/js/adminlte.min.js"></script>
    <script src="LoginTheme/jquery-ui.js"></script>
<%--<script src="//code.jquery.com/ui/1.12.1/jquery-ui.js"></script>--%>
    <!-- ################################################# END #################################################### -->
    <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
<script type="text/javascript">
    function ShowSuccessMsg() {
        $(function () {
            $("#messageDiv").dialog({
                title: "Register",
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
</form>
</body>
</html>
