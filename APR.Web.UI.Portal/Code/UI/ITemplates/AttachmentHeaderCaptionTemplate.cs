using DevExpress.Web.ASPxEditors;
using DevExpress.Web.ASPxGridView;
using System.Web.UI;
using System.Web.UI.HtmlControls;

namespace APR.Web.UI.Portal.Code.UI.ITemplates
{
    public class AttachmentHeaderCaptionTemplate : ITemplate
    {
        public void InstantiateIn(System.Web.UI.Control container)
        {
            var gvHeaderContainer = (GridViewHeaderTemplateContainer)container;

            var tbl = new HtmlTable();

            tbl.Attributes.Add("width", "100%");
            var cell = new HtmlTableCell();
            var row = new HtmlTableRow() { ID = "captionRow1" };

            var chkAttach = new ASPxCheckBox() { ID = "chkAttach", ClientInstanceName = "chkAttach" };
            chkAttach.ClientSideEvents.CheckedChanged = "chkAttachCheckChanged";
            var cd =  gvHeaderContainer.Grid.AllColumns;
            cell.Controls.AddAt(0, chkAttach);
            var span = new HtmlGenericControl("span") { InnerText = "Attachment" };

            cell.Controls.AddAt(1, span);
            cell.Style.Add("text-align", "center");
            row.Controls.Add(cell);

            cell = new HtmlTableCell();
            cell.Style.Add("text-align", "center");
            cell.VAlign = "middle";
            var asave = new HtmlAnchor();
            asave.Attributes.Add("class", "saveicon");
            asave.InnerText = "Save";
            cell.Controls.Add(asave);
            row.Controls.Add(cell);
            cell = new HtmlTableCell();
            cell.Style.Add("text-align", "center");
            cell.VAlign = "middle";
            var aprint = new HtmlAnchor();
            aprint.Attributes.Add("class", "printicon");
            aprint.InnerText = "Print";
            cell.Controls.Add(aprint);
            row.Controls.Add(cell);
            tbl.Controls.Add(row);
            container.Controls.Add(tbl);
        }
    }
}
