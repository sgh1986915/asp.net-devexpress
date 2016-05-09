using DevExpress.Web.ASPxGridView;
using System.Web.UI;

namespace APR.Web.UI.Portal.Code.UI.ITemplates
{
    public class CellDataItemTemplate : ITemplate
    {
        public void InstantiateIn(System.Web.UI.Control container)
        {
            var gridContainer = (GridViewDataItemTemplateContainer)container;
        }
    }
}
