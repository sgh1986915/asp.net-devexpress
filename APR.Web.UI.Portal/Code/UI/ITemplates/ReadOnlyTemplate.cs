using DevExpress.Web.ASPxEditors;
using DevExpress.Web.ASPxGridView;
using System.Web.UI;

namespace APR.Web.UI.Portal.Code.UI.ITemplates
{
    public class ReadOnlyTemplate : ITemplate
    {
        public void InstantiateIn(Control _container)
        {
            var container = _container as GridViewEditItemTemplateContainer;

            var lbl = new ASPxLabel() { ID = "lbl" };

            container.Controls.Add(lbl);
            var txt = container.Text;
            lbl.Text = txt == "&nbsp;" ? string.Empty : txt;
            lbl.CssClass = "ReadonlyCell";
        }
    }
}
