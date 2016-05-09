<%@ Page Title="" Language="C#" MasterPageFile="~/AprMaster.Master" AutoEventWireup="true" CodeBehind="ResetPassword.aspx.cs" Inherits="APR.Web.UI.Portal.Account.ResetPassword" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="aprMain" runat="Server">
    <div class="grid_12">
        <div class="message">
            <textarea name="" cols="" rows=""></textarea>
        </div>
    </div>

    <div class="grid_7">
        <div class="user">
            <div class="user_hed">
                user's name
            </div>
            <div class="user_txt">
                <h2>Reset Password
                </h2>
                <p>
                    Use the form below to reset your password.
                </p>
                <p>
                    New passwords are required to be a minimum of <%= Membership.MinRequiredPasswordLength %> characters in length.
                </p>
            </div>
            <div class="user_form">
                <span class="failureNotification">
                    <asp:Label id="lblStatus" runat="server" />
                </span>
                <asp:ValidationSummary ID="ResetPasswordValidationSummary" runat="server" CssClass="failureNotification"
                    ValidationGroup="ResetPasswordValidationGroup" />
                <div class="accountInfo">
                    <fieldset class="ResetPassword">
                        <legend>Account Information</legend>
                        <p>
                            <asp:Label ID="lblUser" runat="server">User Name:</asp:Label>
                            <asp:TextBox ID="txtUserName" runat="server" ReadOnly="true" />
                        </p>
                        <p>
                            <asp:Label ID="NewPasswordLabel" runat="server" AssociatedControlID="NewPassword">New Password:</asp:Label>
                            <asp:TextBox ID="NewPassword" runat="server" CssClass="passwordEntry" TextMode="Password"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="NewPasswordRequired" runat="server" ControlToValidate="NewPassword"
                                CssClass="failureNotification" ErrorMessage="New Password is required." ToolTip="New Password is required."
                                ValidationGroup="ResetPasswordValidationGroup">*</asp:RequiredFieldValidator>
                        </p>
                        <p>
                            <asp:Label ID="ConfirmNewPasswordLabel" runat="server" AssociatedControlID="ConfirmNewPassword">Confirm New Password:</asp:Label>
                            <asp:TextBox ID="ConfirmNewPassword" runat="server" CssClass="passwordEntry" TextMode="Password"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="ConfirmNewPasswordRequired" runat="server" ControlToValidate="ConfirmNewPassword"
                                CssClass="failureNotification" Display="Dynamic" ErrorMessage="Confirm New Password is required."
                                ToolTip="Confirm New Password is required." ValidationGroup="ResetPasswordValidationGroup">*</asp:RequiredFieldValidator>
                            <asp:CompareValidator ID="NewPasswordCompare" runat="server" ControlToCompare="NewPassword" ControlToValidate="ConfirmNewPassword"
                                CssClass="failureNotification" Display="Dynamic" ErrorMessage="The Confirm New Password must match the New Password entry."
                                ValidationGroup="ResetPasswordValidationGroup">*</asp:CompareValidator>
                        </p>
                    </fieldset>
                    <p class="submitButton">
                        <asp:Button ID="CancelPushButton" runat="server" CausesValidation="False" CommandName="Cancel" Text="Cancel" />
                        <asp:Button ID="ResetPasswordPushButton" runat="server" OnClick="ResetPasswordPushButton_Click" CommandName="ResetPassword" Text="Change Password"
                            ValidationGroup="ResetPasswordValidationGroup" />
                    </p>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
