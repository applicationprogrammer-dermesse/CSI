<%@ Page Title="Access Denied" Language="C#" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true" CodeBehind="AccessDenied.aspx.cs" Inherits="CSI.AccessDenied" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
     <div class="container-fluid">
            <div class="row mb-12" style="margin-top:100px;">
                <div class="col-sm-12 text-center">
                    <h1 style="color:red">ACCESS DENIED!</h1>
                    <br />
                    <br />
                    <h3>You are not authorized to view this page.</h3>
                </div>
                
            </div>
        </div>
</asp:Content>
