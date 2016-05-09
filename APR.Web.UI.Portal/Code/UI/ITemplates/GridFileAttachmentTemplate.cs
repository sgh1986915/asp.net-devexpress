using DevExpress.Web.ASPxEditors;
using DevExpress.Web.ASPxGridView;
using DevExpress.Web.ASPxUploadControl;
using System.Drawing;
using System.Web.UI;

namespace APR.Web.UI.Portal.Code.UI.ITemplates
{
    public class GridFileAttachmentTemplate : ITemplate
    {
        private readonly string _dataItemValue;

        public GridFileAttachmentTemplate(string dataItemValue)
        {
            _dataItemValue = dataItemValue;
        }

        public void InstantiateIn(Control container)
        {
            int index;

            switch (_dataItemValue)
            {
                case "Attachments":

                    var attachment = new ASPxUploadControl();
                    index = (container as GridViewDataItemTemplateContainer).VisibleIndex;

                    attachment.ID = "attachment" + index.ToString();
                    attachment.ClientInstanceName = "attachment_" + index.ToString();
                    attachment.FileUploadMode = UploadControlFileUploadMode.OnPageLoad;
                    attachment.Width = 40;
                    attachment.ShowUploadButton = false;

                    attachment.BrowseButton.Image.Url = "~/images/mail_attachment.ico";
                    attachment.BrowseButton.Text = "";

                    attachment.BrowseButtonStyle.Border.BorderStyle = System.Web.UI.WebControls.BorderStyle.None;
                    attachment.BrowseButtonStyle.Border.BorderColor = Color.Transparent;
                    attachment.BrowseButtonStyle.BackColor = Color.Transparent;

                    attachment.TextBoxStyle.Border.BorderColor = Color.Transparent;
                    attachment.TextBoxStyle.Border.BorderStyle = System.Web.UI.WebControls.BorderStyle.None;
                    attachment.TextBoxStyle.BackColor = Color.Transparent;

                    container.Controls.Add(attachment);
                    break;

                case "RowSelection":
                    var chkBox = new ASPxCheckBox();
                    index = (container as GridViewDataItemTemplateContainer).VisibleIndex;
                    chkBox.ID = "cbPage" + index.ToString();
                    chkBox.ClientInstanceName = "cbPage_" + index.ToString();
                    container.Controls.Add(chkBox);

                    break;

                default:
                    break;
            }
        }
    }
}
