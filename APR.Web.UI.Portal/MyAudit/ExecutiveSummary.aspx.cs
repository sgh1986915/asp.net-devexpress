using APR.Web.UI.Portal.Code;
using APR.Web.UI.Portal.Code.UI.ITemplates;
using DevExpress.Web.ASPxEditors;
using DevExpress.Web.ASPxGridView;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace APR.Web.UI.Portal.MyAudit
{
    public partial class ExecutiveSummary : System.Web.UI.Page
    {
        private ObjectDataSource _odAuditName;
        private ObjectDataSource _odcExecutiveSummery;
        private static string AuditName;
        public string executiveGrid = "aspxExecutiveSummeryGridView";
        private static ASPxGridView _gvExecutiveSummery;
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            ASPxComboBoxAuditName.SelectedIndexChanged += ASPxComboBoxAuditName_SelectedIndexChanged;
        }
        private void ASPxComboBoxAuditName_SelectedIndexChanged(object sender, EventArgs e)
        {
            AuditName = ((ASPxComboBox)sender).SelectedItem.Text;
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsCallback)
            {
                LoadAuditCombo();
                CreateExecutiveSummeryGrid();
            }
        }
        private void LoadAuditCombo()
        {
            _odAuditName = new ObjectDataSource();
            _odAuditName.TypeName = "APR.Web.UI.Portal.Myaudit.ExecutiveSummary";

            _odAuditName.SelectMethod = "OpenAuditNameExcelFile";

            ASPxComboBoxAuditName.DataSource = _odAuditName;
            ASPxComboBoxAuditName.ValueField = AuditColumns.AUDITNAME;
            ASPxComboBoxAuditName.TextField = AuditColumns.AUDITNAME;
            ASPxComboBoxAuditName.DataBind();
            ASPxComboBoxAuditName.SelectedItem = ASPxComboBoxAuditName.Items[0];
        }
        public DataTable OpenChartExcelFile()
        {
            var dataTable = new DataTable();
            var fileName = Server.MapPath("~/App_Data/aspxExecutiveSummeryGridView.xls");
            var connectionString = string.Format("Provider=Microsoft.Jet.OLEDB.4.0; data source={0}; Extended Properties=Excel 8.0;", fileName);
            var adapter = new System.Data.OleDb.OleDbDataAdapter("SELECT * FROM [Sheet$]", connectionString);
            adapter.Fill(dataTable);
            try
            {
                return dataTable.Select(string.Empty + AuditColumns.AUDITNAME + " ='" + AuditName + "'").CopyToDataTable();
            }
            catch
            {
                return dataTable;
            }
        }
        public DataTable OpenAuditNameExcelFile()
        {
            var dataTable = new DataTable();
            var fileName = Server.MapPath("~/App_Data/aspxInvGridView.xls");
            var connectionString = string.Format("Provider=Microsoft.Jet.OLEDB.4.0; data source={0}; Extended Properties=Excel 8.0;", fileName);
            var adapter = new System.Data.OleDb.OleDbDataAdapter("SELECT distinct AuditName FROM [Sheet$]", connectionString);
            adapter.Fill(dataTable);

            return dataTable;
        }
        private void CreateExecutiveSummeryChart()
        {
        }
        private void CreateExecutiveSummeryGrid()
        {
            try
            {
                _gvExecutiveSummery = new ASPxGridView();
                _gvExecutiveSummery.ID = "executiveGrid";
                _gvExecutiveSummery.ClientInstanceName = executiveGrid;
                _gvExecutiveSummery.SettingsText.Title = AuditColumns.EXECUTIVESUMMARY;
                _gvExecutiveSummery.KeyFieldName = AuditColumns.AUDITNAME;
                BindGetInvoicesDataSource();

                dvExecutiveGrid.Controls.Add(_gvExecutiveSummery);
            }
            catch
            {
            }
        }
        private void BindGetInvoicesDataSource()
        {
            _odcExecutiveSummery = new ObjectDataSource();
            _odcExecutiveSummery.TypeName = "APR.Web.UI.Portal.Myaudit.ExecutiveSummary";

            _odcExecutiveSummery.SelectMethod = "OpenChartExcelFile";

            _gvExecutiveSummery.DataSource = _odcExecutiveSummery.Select();

            _gvExecutiveSummery.DataBind();
        }
        protected void SetGridVwProperties(ASPxGridView grid)
        {
            try
            {
                grid.ClientIDMode = ClientIDMode.Static;
                grid.AutoGenerateColumns = true;

                grid.EnableCallBacks = true;
                grid.KeyboardSupport = true;
                grid.FilterEnabled = true;
                grid.EnableTheming = false;

                grid.SettingsLoadingPanel.Mode = GridViewLoadingPanelMode.ShowAsPopup;
                grid.SettingsLoadingPanel.ImagePosition = DevExpress.Web.ASPxClasses.ImagePosition.Left;
                grid.SettingsLoadingPanel.Text = string.Empty;

                grid.Styles.AlternatingRow.Enabled = DevExpress.Utils.DefaultBoolean.True;
                grid.Width = Unit.Percentage(100);

                grid.Settings.ColumnMinWidth = 40;
                grid.Settings.EnableFilterControlPopupMenuScrolling = true;

                grid.Settings.VerticalScrollBarMode = DevExpress.Web.ASPxClasses.ScrollBarMode.Visible;
                grid.Settings.VerticalScrollBarStyle = GridViewVerticalScrollBarStyle.Standard;
                grid.Settings.HorizontalScrollBarMode = DevExpress.Web.ASPxClasses.ScrollBarMode.Visible;
                grid.Settings.GridLines = GridLines.Horizontal;
                grid.Settings.ShowColumnHeaders = true;
                grid.Settings.ShowFooter = true;
                grid.Settings.ShowFilterBar = GridViewStatusBarMode.Visible;
                grid.Settings.ShowFilterRowMenu = true;
                grid.Settings.ShowFilterRow = true;
                grid.Settings.ShowGroupButtons = true;
                grid.Settings.ShowGroupedColumns = true;
                grid.Settings.ShowGroupFooter = GridViewGroupFooterMode.VisibleAlways;

                grid.Settings.ShowGroupPanel = true;

                grid.Settings.ShowHeaderFilterButton = true;
                grid.Settings.ShowTitlePanel = true;

                grid.SettingsBehavior.AllowDragDrop = true;
                grid.SettingsBehavior.AllowFocusedRow = true;

                grid.SettingsBehavior.AllowGroup = true;
                grid.SettingsBehavior.AllowSelectByRowClick = true;

                grid.SettingsBehavior.AllowSort = true;
                grid.SettingsBehavior.AllowClientEventsOnLoad = true;
                grid.SettingsBehavior.ColumnResizeMode = DevExpress.Web.
                          ASPxClasses.ColumnResizeMode.Control;
                grid.SettingsBehavior.EnableCustomizationWindow = true;
                grid.SettingsBehavior.EnableRowHotTrack = true;
                grid.SettingsBehavior.ProcessFocusedRowChangedOnServer = true;
                grid.SettingsBehavior.SortMode = DevExpress.XtraGrid
                                                    .ColumnSortMode.Value;

                grid.SettingsPager.Mode = GridViewPagerMode.ShowAllRecords;

                grid.SettingsEditing.Mode = GridViewEditingMode.Inline;

                grid.Templates.TitlePanel = new HeaderTitleTemplate();

                foreach (GridViewDataColumn column in grid.Columns)
                {
                    column.Settings.HeaderFilterMode = HeaderFilterMode.List;
                }
            }
            catch
            {
            }
        }
    }
}
