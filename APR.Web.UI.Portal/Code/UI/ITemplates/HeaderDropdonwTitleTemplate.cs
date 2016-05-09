
using DevExpress.Web.ASPxEditors;
using DevExpress.Web.ASPxGridView;
using DevExpress.Web.ASPxGridView.Export;
using System;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace APR.Web.UI.Portal.Code.UI.ITemplates
{
    public class HeaderDropdonwTitleTemplate : ITemplate, IDisposable
    {
        private ASPxGridViewExporter _gvGridExporter = new ASPxGridViewExporter();

        public void Dispose()
        {
            if (_gvGridExporter != null)
            {
                _gvGridExporter.Dispose();
                _gvGridExporter = null;
            }
        }

        public void InstantiateIn(System.Web.UI.Control container)
        {
            var gridContainer = (GridViewTitleTemplateContainer)container;

            var TittlePanelsection = new HtmlGenericControl("section");


            var H3 = new HtmlGenericControl("H3") { InnerHtml = gridContainer.Grid.SettingsText.Title };
            H3.Attributes.Add("class", "gvTittle");
            var ExportExcel = new LinkButton() { CssClass = "aprLinkButton", ClientIDMode = ClientIDMode.Static, ID = "aspxButtonExportCSV_" + new Random().Next(1000), Text = "Export Excel", ToolTip = "Export To Excel" };

            ExportExcel.Click += (s, e) =>
            {

                _gvGridExporter.WriteXlsToResponse();


            };
            var cmb = new ASPxComboBox() { ID = "titleCombo" };
            LoadAuditCombo(cmb);
            cmb.ClientInstanceName = "ASPxComboBoxAuditName";
            cmb.ClientSideEvents.SelectedIndexChanged = "OnIndexChange";
            gridContainer.Controls.Add(cmb);

            gridContainer.Controls.Add(H3);

            TittlePanelsection.Controls.Add(ExportExcel);

            gridContainer.Controls.Add(TittlePanelsection);

            _gvGridExporter = new ASPxGridViewExporter() { GridViewID = gridContainer.Grid.ID };
            gridContainer.Controls.Add(_gvGridExporter);
        }

        private void LoadAuditCombo(ASPxComboBox cmb)
        {

        }
    }
}
