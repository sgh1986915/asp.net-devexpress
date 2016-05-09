<%@ Page Title="" Language="C#" MasterPageFile="~/AprMaster.Master" AutoEventWireup="true" ViewStateMode="Disabled" EnableViewState="false" CodeBehind="Login.aspx.cs" Inherits="APR.Web.UI.Portal.Account.Login" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">

<title>Login</title>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="aprMain" Runat="Server">

<%--<div class="grid_12">
        <div class="message">
        	<textarea name="" cols="" rows=""></textarea>
        </div>
  </div>--%>
  <div class="clear"></div>
  <div class="grid_7">
	<div class="user">
        <div class="user_hed">
            Login
        </div>
        <div class="user_txt">
         <p>  Please enter your username and password.</p>
        <%--<asp:HyperLink ID="RegisterHyperLink" runat="server" EnableViewState="false">Register</asp:HyperLink> if you don't have an account.--%>
        </div>
        
       <div class="user_form">
           <asp:Login ID="LoginUser" runat="server" EnableViewState="false" RenderOuterTable="false">
               <LayoutTemplate>
                   <span class="failureNotification">
                       <asp:Literal ID="FailureText" runat="server"></asp:Literal>
                   </span>
                   <asp:ValidationSummary ID="LoginUserValidationSummary" runat="server" CssClass="failureNotification"
                       ValidationGroup="LoginUserValidationGroup" />
                   <div class="accountInfo">
                       <fieldset class="login">
                           <legend>Account Information</legend>
                           <p>
                               <asp:Label ID="UserNameLabel" runat="server" AssociatedControlID="UserName">Username:</asp:Label>
                               <asp:TextBox ID="UserName" runat="server" CssClass="textEntry"></asp:TextBox>
                               <asp:RequiredFieldValidator ID="UserNameRequired" runat="server" ControlToValidate="UserName"
                                   CssClass="failureNotification" ErrorMessage="User Name is required." ToolTip="User Name is required."
                                   ValidationGroup="LoginUserValidationGroup">*</asp:RequiredFieldValidator>
                           </p>
                           <p>
                               <asp:Label ID="PasswordLabel" runat="server" AssociatedControlID="Password">Password:</asp:Label>
                               <asp:TextBox ID="Password" runat="server" CssClass="passwordEntry" TextMode="Password"></asp:TextBox>
                               <asp:RequiredFieldValidator ID="PasswordRequired" runat="server" ControlToValidate="Password"
                                   CssClass="failureNotification" ErrorMessage="Password is required." ToolTip="Password is required."
                                   ValidationGroup="LoginUserValidationGroup">*</asp:RequiredFieldValidator>
                           </p>
                           <p>
                               <asp:CheckBox ID="RememberMe" runat="server" />
                               <asp:Label ID="RememberMeLabel" runat="server" AssociatedControlID="RememberMe" CssClass="inline">Keep me logged in</asp:Label>
                           </p>
                       </fieldset>
                       <p class="submitButton">
                           <asp:Button ID="LoginButton" runat="server" CommandName="Login" Text="Log In" ValidationGroup="LoginUserValidationGroup" />
                       </p>
                   </div>
               </LayoutTemplate>
           </asp:Login>
        </div>
    </div>
  </div>
        <div class="grid_5">
        
    </div>
      <div class="clear"></div>
</asp:Content>