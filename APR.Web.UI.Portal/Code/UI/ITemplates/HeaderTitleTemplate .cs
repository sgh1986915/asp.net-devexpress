using DevExpress.Web.ASPxGridView;
using DevExpress.Web.ASPxGridView.Export;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System;

namespace APR.Web.UI.Portal.Code.UI.ITemplates
{
    public class HeaderTitleTemplate : ITemplate, IDisposable
    {
        public LinkButton ExportExcel = new LinkButton();

        public void Dispose()
        {
            if (ExportExcel != null)
            {
                ExportExcel.Dispose();
                ExportExcel = null;
            }
        }
        public void InstantiateIn(System.Web.UI.Control container)
        {
            var gridContainer = (GridViewTitleTemplateContainer)container;
            var TittlePanelsection = new HtmlGenericControl("section");
            var H3 = new HtmlGenericControl("H3") { InnerHtml = gridContainer.Grid.SettingsText.Title };
            H3.Attributes.Add("class", "gvTittle");
            ExportExcel.CssClass = "aprLinkButton";
            ExportExcel.ID = "aspxButtonExportXLs";
            ExportExcel.Text = "Export Excel";
            ExportExcel.ToolTip = "Export To Excel";


            gridContainer.Controls.Add(H3);

            var _gvGridExporter = new ASPxGridViewExporter() { ID = "gvGridExporter", GridViewID = gridContainer.Grid.ID };
            ExportExcel.Click += (s, e) => _gvGridExporter.WriteXlsToResponse();
            TittlePanelsection.Controls.Add(ExportExcel);
            gridContainer.Controls.Add(_gvGridExporter);
            gridContainer.Controls.Add(TittlePanelsection);
        }
    }
}
