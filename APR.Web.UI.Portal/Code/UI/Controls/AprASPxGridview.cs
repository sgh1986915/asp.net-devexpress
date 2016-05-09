using APR.Web.UI.Portal.Code.UI.ITemplates;
using DevExpress.Web.ASPxGridView;
using System.Web.UI.WebControls;

namespace APR.Web.UI.Portal.Code.UI.Controls
{
    public class AprASPxGridview : ASPxGridView
    {
        public AprASPxGridview()
        {
            SetGridVwProperties(this);
            Theme = "PlasticBlue";
        }

        protected void SetGridVwProperties(ASPxGridView grid)
        {
            try
            {
                grid.ClientIDMode = System.Web.UI.ClientIDMode.Static;

                grid.EnableCallBacks = true;
                grid.KeyboardSupport = true;
                grid.FilterEnabled = true;
                grid.EnableTheming = false;

                grid.SettingsLoadingPanel.Mode = GridViewLoadingPanelMode.ShowAsPopup;
                grid.SettingsLoadingPanel.ImagePosition = DevExpress.Web.ASPxClasses.ImagePosition.Left;

                grid.Styles.AlternatingRow.Enabled = DevExpress.Utils.DefaultBoolean.True;
                grid.Width = Unit.Percentage(100);

                grid.Settings.ColumnMinWidth = 40;
                grid.Settings.EnableFilterControlPopupMenuScrolling = true;

                grid.Settings.VerticalScrollBarMode = DevExpress.Web.ASPxClasses.ScrollBarMode.Visible;
                grid.Settings.VerticalScrollBarStyle = GridViewVerticalScrollBarStyle.Virtual;
                grid.Settings.HorizontalScrollBarMode = DevExpress.Web.ASPxClasses.ScrollBarMode.Visible;
                grid.Settings.GridLines = GridLines.Both;
                grid.Settings.ShowColumnHeaders = true;
                grid.Settings.ShowFooter = true;
                grid.Settings.ShowFilterBar = GridViewStatusBarMode.Visible;
                grid.Settings.ShowFilterRowMenu = true;
                grid.Settings.ShowFilterRow = true;
                grid.Settings.ShowGroupButtons = false;
                grid.Settings.ShowGroupedColumns = false;
                grid.Settings.ShowGroupFooter = GridViewGroupFooterMode.VisibleIfExpanded;
                grid.SettingsText.CommandCancel = string.Empty;
                grid.SettingsText.CommandUpdate = string.Empty;
                grid.Settings.ShowGroupPanel = true;

                grid.Settings.ShowHeaderFilterButton = true;
                grid.Settings.ShowTitlePanel = true;

                grid.SettingsBehavior.AllowDragDrop = true;
                //grid.SettingsBehavior.AllowFocusedRow = true;

                grid.SettingsBehavior.AllowGroup = true;
                grid.SettingsBehavior.AllowSelectByRowClick = false;

                grid.SettingsBehavior.AllowSort = true;
                grid.SettingsBehavior.AllowClientEventsOnLoad = true;
                grid.SettingsBehavior.ColumnResizeMode = DevExpress.Web.
                          ASPxClasses.ColumnResizeMode.Control;
                grid.SettingsBehavior.EnableCustomizationWindow = false;
                grid.SettingsBehavior.EnableRowHotTrack = true;
                //grid.SettingsBehavior.ProcessFocusedRowChangedOnServer = true;
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
