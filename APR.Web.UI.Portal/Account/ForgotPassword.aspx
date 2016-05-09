<%@ Page Title="" Language="C#" MasterPageFile="~/AprMaster.Master" AutoEventWireup="true" CodeBehind="ForgotPassword.aspx.cs" Inherits="APR.Web.UI.Portal.Account.ForgotPassword" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="aprMain" runat="server">

<div class="grid_12">
        <div class="message">
        	<textarea name="" cols="" rows=""></textarea>
        </div>
  </div>
  
  <div class="grid_7">
	<div class="user">
        <div class="user_hed">
            Forgot Password
        </div>
        <div class="user_txt">
           <h2>
        Forgot Password
    </h2>
    <p>
        Please enter your email to receive instructions on how to reset your password.
    </p>
        </div>
        <div class="user_form">
            <span class="failureNotification">
                <asp:Label id="lblStatus" runat="server" />
            </span>
            <asp:ValidationSummary ID="ForgotPasswordValidationSummary" runat="server" CssClass="failureNotification" 
                 ValidationGroup="ForgotPasswordValidationGroup"/>
            
            <div class="accountInfo">
                <fieldset class="forgotPassword">
                    <legend></legend>
                    <p>
                        <asp:Label ID="EmailLabel" runat="server" AssociatedControlID="txtEmail" text="Email:" />
                        <asp:TextBox ID="txtEmail" runat="server" CssClass="emailEntry" />
                        <asp:RequiredFieldValidator ID="EmailRequired" runat="server" ControlToValidate="txtEmail" 
                             CssClass="failureNotification" ErrorMessage="Email is required." ToolTip="Email is required." 
                             ValidationGroup="ForgotPasswordValidationGroup" Text="*" />
                    </p>
                </fieldset>
                <p class="submitButton">
                    <asp:Button ID="btnSubmit" OnClick="btnSubmit_Click" runat="server" CommandName="ResetPassword" Text="Reset Password" 
                         ValidationGroup="ForgotPasswordValidationGroup"/>
                </p>
            </div>
        </div>
    </div>
  </div>  
</asp:Content>
