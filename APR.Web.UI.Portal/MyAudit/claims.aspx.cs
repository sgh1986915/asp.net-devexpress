using APR.Common.Objects.Database.Mapping.Tables.APRSQLCuster.APRPortal;
using APR.Utilities.Extensions.NullExtensionChecks;
using APR.Utilities.NonExtHelpers;
using APR.Web.UI.Portal.Code;
using APR.Web.UI.Portal.Code.UI.Controls;
using APR.Web.UI.Portal.Code.UI.ITemplates;
using BLToolkit.Mapping;
using DevExpress.Data;
using DevExpress.Data.Filtering;
using DevExpress.Web.ASPxEditors;
using DevExpress.Web.ASPxGridView;
using DevExpress.Web.ASPxGridView.Export;
using DevExpress.Web.Data;
using DevExpress.XtraPrinting;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI.WebControls;

namespace APR.Web.UI.Portal.MyAudit
{
    public partial class Claims : System.Web.UI.Page
    {
        #region Private Variables

        private ObjectDataSource _odcInvoice;
        private ObjectDataSource _odcClaims;
        private string _invNum;
        public string InvGridId = "aspxInvGridView";
        public string ClmGridId = "aspxClmGridView";
        private ObjectDataSource _odAuditName;

        public delegate void EventRowSelectionChanged(string invNum);

        //public event EventRowSelectionChanged EventRowSelectionChangedEvent;

        private static ASPxGridView _gvInvoices;
        private ASPxGridView _gvClaims;
        //private ASPxGridViewExporter _gvInvGridExporter;
        //private ASPxGridViewExporter _gvClmsGridExporter;
        private const string noFilter = "[" + InvoiceColumns.INVNUMBER + "] ='None'";
        //public string AuditName;

        #endregion Private Variables

        //public string AuditName = "Allegheny12";

        //public string AuditName = "Southwire11";

        #region Load Data From Database or other sources

        public MyAuditPortalSrvc.AprClientPortalClaimsTbl[] GetClaims()
        {
            MyAuditPortalSrvc.AprClientPortalClaimsTbl[] clntPortalclaims = null;
            //DataTable dtClms = null;

            try
            {
                string auditValue = GetAuditNameValue();
                using (var myAuditPortal = new MyAuditPortalSrvc
                                   .ServiceMyAuditPortalClient())
                {
                    clntPortalclaims = myAuditPortal
                            .GetAprAuditClaims(auditValue);

                    //List<AprClientPortalClaimsTbl> listClientPortalClms = clntPortalclaims as List<AprClientPortalClaimsTbl>;
                    //dtClms = Map.ListToDataTable(clntPortalclaims);
                    //drClms = dtClms.CreateDataReader();
                }
            }
            catch
            {
            }
            return clntPortalclaims;
        }

        public MyAuditPortalSrvc.AprClientPortalClaimsTbl[] GetClaims(string invNum)
        {
            MyAuditPortalSrvc.AprClientPortalClaimsTbl[] clntPortalclaims = null;
            //DataTable dtClms = null;
            
            try
            {
                string auditValue = GetAuditNameValue();

                using (var myAuditPortal = new MyAuditPortalSrvc.ServiceMyAuditPortalClient())
                {
                    clntPortalclaims = myAuditPortal.GetAprAuditClaims(auditValue);

                    //dtClms = Map.ListToDataTable(clntPortalclaims);
                    //drClms = dtClms.CreateDataReader();
                }
            }
            catch
            {
            }
            return clntPortalclaims;
        }

        public MyAuditPortalSrvc.AprClientPortalInvoicesTbl[] GetInvoices()
        {
            MyAuditPortalSrvc.AprClientPortalInvoicesTbl[] clientPortalInvoices = null;
            //DataTable dtInv = null;

            try
            {
                var auditValue = GetAuditNameValue();

                using (var myAuditPortal = new MyAuditPortalSrvc.ServiceMyAuditPortalClient())
                {
                    clientPortalInvoices = myAuditPortal.GetAprAuditInvoices(auditValue);
                    
                    //dtInv = Map.ListToDataTable(clientPortalInvoices);
                    //drInv = dtInv.CreateDataReader();
                }
            }
            catch
            {
            }
            return clientPortalInvoices;
        }

        public DataTable GetAuditNames()
        {
            DataTable dtAuditNames = null;
            try
            {
                // Get Current User Context
                var username = HttpContext.Current.User.Identity.Name;

                // Extract username if it contains an @ sign
                username = StrHelprs.ExtractUserName(username);

                using (var myAuditPortal = new MyAuditPortalSrvc.ServiceMyAuditPortalClient())
                {
                    var auditNames = myAuditPortal.GetUsersAuditNamesList("prosenberg");

                    //dtAuditNames = Map.ListToDataTable(auditNames);

                    //dtAuditNames.Columns[1].ColumnName = "AuditName";

                }
            }
            catch
            {
            }
            return dtAuditNames;
        }

        public string GetAuditNameValue()
        {
            string auditValue = "";
            if (hdnSelectedAudit.Value != "")
                auditValue = ASPxComboBoxAuditName.Items[Convert.ToInt32(hdnSelectedAudit.Value)].Text;
            else
                auditValue = ASPxComboBoxAuditName.Items.Count > 0 ? ASPxComboBoxAuditName.Items[0].Text : "";

            return auditValue;
        }

        public DataTable OpenAuditNameExcelFile()
        {
            //var c = (this.FindControl("ASPxComboBoxAuditName") as DevExpress.Web.ASPxEditors.ASPxComboBox).SelectedItem.Text;
            DataTable dataTable = new DataTable();
            string fileName = Server.MapPath("~/App_Data/aspxInvGridView.xls");
            string connectionString = string.Format("Provider=Microsoft.Jet.OLEDB.4.0; data source={0}; Extended Properties=Excel 8.0;", fileName);
            System.Data.OleDb.OleDbDataAdapter adapter = new System.Data.OleDb.OleDbDataAdapter("SELECT distinct AuditName FROM [Sheet$]", connectionString);
            adapter.Fill(dataTable);
            return dataTable;
        }

        public DataTable OpenClaimExcelFile()
        {
            DataTable dataTable = new DataTable();
            string fileName = Server.MapPath("~/App_Data/aspxClmGridView.xls");
            string connectionString = string.Format("Provider=Microsoft.Jet.OLEDB.4.0; data source={0}; Extended Properties=Excel 8.0;", fileName);
            System.Data.OleDb.OleDbDataAdapter adapter = new System.Data.OleDb.OleDbDataAdapter("SELECT * FROM [Sheet$]", connectionString);
            adapter.Fill(dataTable);

            //try
            //{
            //    return dataTable.Select("AuditName ='" + AuditName + "'").CopyToDataTable();
            //}
            //catch
            //{
            //    return dataTable;
            //}

            return dataTable;
        }

        public DataTable OpenInvExcelFile()
        {
            //var c = (this.FindControl("ASPxComboBoxAuditName") as DevExpress.Web.ASPxEditors.ASPxComboBox).SelectedItem.Text;

            string query = "SELECT * FROM [Sheet$] WHERE " + String.Format("{0} ='{1}'", AuditColumns.AUDITNAME, ASPxComboBoxAuditName.Text);


            DataTable dataTable = new DataTable();
            string fileName = Server.MapPath("~/App_Data/aspxInvGridView.xls");
            string connectionString = string.Format("Provider=Microsoft.Jet.OLEDB.4.0; data source={0}; Extended Properties=Excel 8.0;", fileName);
            System.Data.OleDb.OleDbDataAdapter adapter = new System.Data.OleDb.OleDbDataAdapter(query, connectionString);
            adapter.Fill(dataTable);

            return dataTable;
        }

        private void LoadAuditCombo()
        {
            //_odAuditName = new ObjectDataSource();
            //_odAuditName.TypeName = "APR.Web.UI.Portal.myaudit.claims";

            //_odAuditName.SelectMethod = "GetAuditNames";
            //_odAuditName.SelectMethod = "OpenAuditNameExcelFile";

            // APR
            //ASPxComboBoxAuditName.DataSource = GetAuditNames();

            // Fake Data
            ASPxComboBoxAuditName.DataSource = OpenAuditNameExcelFile();
            ASPxComboBoxAuditName.ValueField = AuditColumns.AUDITNAME;
            ASPxComboBoxAuditName.TextField = AuditColumns.AUDITNAME;
            ASPxComboBoxAuditName.DataBind();
            ASPxComboBoxAuditName.SelectedIndex = 1;

            if (hdnSelectedAudit.Value == "")
            {
                ASPxComboBoxAuditName.SelectedIndex = 1;
                hdnSelectedAudit.Value = "1";
            }
            else
                ASPxComboBoxAuditName.SelectedIndex = Convert.ToInt32(hdnSelectedAudit.Value);
        }

        private void GridViewLoad(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                var invoiceGrid = (ASPxGridView)sender;

                var colChk = new GridViewDataColumn() { Name = "RowSelection", Width = 35, DataItemTemplate = new GridFileAttachmentTemplate("RowSelection") };
                invoiceGrid.Columns.Add(colChk);

                var colAttachment = new GridViewDataColumn() { Name = InvoiceColumns.ATTACHMENTS, DataItemTemplate = new GridFileAttachmentTemplate(InvoiceColumns.ATTACHMENTS), Width = 70 };
                invoiceGrid.Columns.Add(colAttachment);
            }
        }

        private void BindGetInvoicesDataSource()
        {
            var value = ASPxComboBoxAuditName.SelectedItem;

            //_odcInvoice = new ObjectDataSource();
            //_odcInvoice.TypeName = "APR.Web.UI.Portal.myaudit.claims";

            //_odcInvoice.SelectMethod = "GetInvoices";
            //_odcInvoice.SelectMethod = "OpenInvExcelFile";

            // APR
           //_gvInvoices.DataSource = GetInvoices();

            // Fake Data
            _gvInvoices.DataSource = OpenInvExcelFile();

            _gvInvoices.DataBind();
        }

        private void BindGetClaimsDataSource()
        {
            //_odcClaims = new ObjectDataSource();
            //_odcClaims.TypeName = "APR.Web.UI.Portal.myaudit.claims";

            //_odcClaims.SelectMethod = "GetClaims";
            //_odcClaims.SelectMethod = "OpenClaimExcelFile";

            //var invNumSessionParm = new SessionParameter();
            //invNumSessionParm.Name = "InvNum";
            //invNumSessionParm.SessionField = "Inv Num";
            //invNumSessionParm.Type = TypeCode.String;

            //_odcClaims.SelectParameters.Add(invNumSessionParm);

            //_gvClaims.DataSource = _odcClaims;

            //var invNumSessionParm = new SessionParameter();
            //invNumSessionParm.Name = "InvNum";
            //invNumSessionParm.SessionField = "Inv Num";
            //invNumSessionParm.Type = TypeCode.String;

            //_odcClaims.SelectParameters.Add(invNumSessionParm);

            // APR
            //gvClaims.DataSource = GetClaims();

            // Fake Data
            _gvClaims.DataSource = OpenClaimExcelFile();

            //_gvClaims.DataBind();
        }

        #endregion Load Data From Database or other sources

        #region Invoice Grid

        private void CreateInvoiceColumns()
        {
            GridViewCommandColumn command = CreateCommandColumn();
            command.UpdateButton.Text = "";
            command.CancelButton.Text = "";
            _gvInvoices.Columns.Add(command);
            GridViewDataTextColumn InvNum = new GridViewDataTextColumn() { FieldName = InvoiceColumns.INVNUMBER,Caption="Inv Num" };
            _gvInvoices.Columns.Add(InvNum);
            GridViewDataDateColumn InvDate = new GridViewDataDateColumn() { FieldName = InvoiceColumns.INVDATE };
            _gvInvoices.Columns.Add(InvDate);
            GridViewDataTextColumn VentodID = new GridViewDataTextColumn() { FieldName = InvoiceColumns.VENDORID };
            _gvInvoices.Columns.Add(VentodID);
            GridViewDataTextColumn VentorName = new GridViewDataTextColumn() { FieldName = InvoiceColumns.VENDORNAME };
            _gvInvoices.Columns.Add(VentorName);
            GridViewDataTextColumn AttachMent = new GridViewDataTextColumn() { FieldName = InvoiceColumns.ATTACHMENTS, HeaderCaptionTemplate = new AttachmentHeaderCaptionTemplate() };
            _gvInvoices.Columns.Add(AttachMent);
            GridViewDataTextColumn RECOVERYAMT = new GridViewDataTextColumn() { FieldName = InvoiceColumns.RECOVERYAMT };
            RECOVERYAMT.CellStyle.HorizontalAlign = HorizontalAlign.Right;
            _gvInvoices.Columns.Add(RECOVERYAMT);
            GridViewDataTextColumn BILLEDAMT = new GridViewDataTextColumn() { FieldName = InvoiceColumns.BILLEDAMT };
            BILLEDAMT.CellStyle.HorizontalAlign = HorizontalAlign.Right;
            _gvInvoices.Columns.Add(BILLEDAMT);
            GridViewDataTextColumn BALANCE = new GridViewDataTextColumn() { FieldName = InvoiceColumns.BALANCE };
            BALANCE.CellStyle.HorizontalAlign = HorizontalAlign.Right;
            _gvInvoices.Columns.Add(BALANCE);
            GridViewDataTextColumn RCVDRF = new GridViewDataTextColumn() { FieldName = InvoiceColumns.RCVDRF };
            _gvInvoices.Columns.Add(RCVDRF);
            GridViewDataTextColumn INVCOMMENTS = new GridViewDataTextColumn() { FieldName = InvoiceColumns.INVCOMMENTS };
            _gvInvoices.Columns.Add(INVCOMMENTS);
            GridViewDataComboBoxColumn APPROVALSTATUS = new GridViewDataComboBoxColumn() { FieldName = InvoiceColumns.APPROVALSTATUS };
            APPROVALSTATUS.PropertiesComboBox.Items.Add(new ListEditItem("Blank", ""));
            APPROVALSTATUS.PropertiesComboBox.Items.Add(new ListEditItem("CR Entered-RF Pending", "CR Entered-RF Pending"));
            APPROVALSTATUS.PropertiesComboBox.Items.Add(new ListEditItem("Processed to Pay", "Processed to Pay"));
            APPROVALSTATUS.PropertiesComboBox.Items.Add(new ListEditItem("Dispute", "Dispute"));
            APPROVALSTATUS.PropertiesComboBox.Items.Add(new ListEditItem("Needs Adjustment", "Needs Adjustment"));
            APPROVALSTATUS.PropertiesComboBox.Items.Add(new ListEditItem("Needs Research", "Needs Research"));
            APPROVALSTATUS.PropertiesComboBox.Items.Add(new ListEditItem("Pending", "Pending"));
            _gvInvoices.Columns.Add(APPROVALSTATUS);
            GridViewDataDateColumn ECONBENEFITDATE = new GridViewDataDateColumn() { FieldName = InvoiceColumns.ECONBENEFITDATE,Width=100 };
            _gvInvoices.Columns.Add(ECONBENEFITDATE);
            GridViewDataTextColumn AFTERPAYID = new GridViewDataTextColumn() { FieldName = InvoiceColumns.AFTERPAYID };
            _gvInvoices.Columns.Add(AFTERPAYID);
            GridViewDataTextColumn PAYID = new GridViewDataTextColumn() { FieldName = InvoiceColumns.PAYID };
            _gvInvoices.Columns.Add(PAYID);
            GridViewDataTextColumn AUD1 = new GridViewDataTextColumn() { FieldName = InvoiceColumns.AUD1 };
            _gvInvoices.Columns.Add(AUD1);
            GridViewDataTextColumn AUD2 = new GridViewDataTextColumn() { FieldName = InvoiceColumns.AUD2 };
            _gvInvoices.Columns.Add(AUD2);
            GridViewDataTextColumn ACTIONOWNER = new GridViewDataTextColumn() { FieldName = InvoiceColumns.ACTIONOWNER };
            //ACTIONOWNER.PropertiesComboBox.Items.Add(new ListEditItem("Blank", ""));
            //ACTIONOWNER.PropertiesComboBox.Items.Add(new ListEditItem("Admin-Claims", "Admin-Claims"));
            //ACTIONOWNER.PropertiesComboBox.Items.Add(new ListEditItem("Client", "Client"));
            _gvInvoices.Columns.Add(ACTIONOWNER);
            GridViewDataDateColumn ACTIONDATE = new GridViewDataDateColumn() { FieldName = InvoiceColumns.ACTIONDATE };
            _gvInvoices.Columns.Add(ACTIONDATE);
            GridViewDataTextColumn ACTIONREQUIRED = new GridViewDataTextColumn() { FieldName = InvoiceColumns.ACTIONREQUIRED };
            //ACTIONREQUIRED.PropertiesComboBox.Items.Add(new ListEditItem("Blank", ""));
            //ACTIONREQUIRED.PropertiesComboBox.Items.Add(new ListEditItem("Adjust Invoice", "Adjust Invoice"));
            
            _gvInvoices.Columns.Add(ACTIONREQUIRED);
            GridViewDataTextColumn SERVICELINE = new GridViewDataTextColumn() { FieldName = InvoiceColumns.SERVICELINE };
            _gvInvoices.Columns.Add(SERVICELINE);
        }

        private void CreateInvoicesGridView()
        {
            try
            {
                _gvInvoices = new AprASPxGridview() { ID = InvGridId, ClientInstanceName = InvGridId };//new CustomGridview(InvGridId,"Inv Num");
                CreateInvoiceColumns();
                _gvInvoices.KeyFieldName = InvoiceColumns.INVNUMBER;

                // Set the Events for the Grid

                #region Server side and Client Side Events

                //_gvInvoices.ClientSideEvents.Init = "function(s,e) {chkAll.SetChecked(true)}";
                _gvInvoices.AfterPerformCallback += _gvInvoices_AfterPerformCallback;
                _gvInvoices.CustomCallback += _gvInvoices_CustomCallback;

                //_gvInvoices.ClientSideEvents.RowClick = "function(s,e) {s.SelectRowOnPage(e.visibleIndex,!s.IsRowSelectedOnPage(e.visibleIndex));gvClaims.PerformCallback();}";
                _gvInvoices.ClientSideEvents.SelectionChanged = "OnGridSelectionChanged";
                _gvInvoices.SettingsText.Title = AuditColumns.INVOICES;
                _gvInvoices.FocusedRowChanged += GvInvoicesFocusedRowChanged;
                _gvInvoices.CustomColumnSort += GvInvoices_CustomColumnSort;
                _gvInvoices.CustomColumnGroup += GvInvoices_CustomColumnGroup;
                _gvInvoices.CustomGroupDisplayText += GvInvoices_CustomGroupDisplayText;
                _gvInvoices.HtmlRowCreated += GvInvoices_HtmlRowCreated;
                _gvInvoices.HtmlDataCellPrepared += gridInvoice_HtmlDataCellPrepared;
               // _gvInvoices.RowUpdated += 
                _gvInvoices.CellEditorInitialize += _gvInvoices_CellEditorInitialize;

                //_gvInvoices.Load += new EventHandler(GridViewLoad);
                _gvInvoices.ClientSideEvents.EndCallback = "OnEndCallback";
                _gvInvoices.ClientSideEvents.Init = "OnInit";
                _gvInvoices.ClientSideEvents.BeginCallback = "OnInvoiceBeginCallBack";
                _gvInvoices.ClientSideEvents.ColumnSorting = "GetSortedColumnIndex";
                _gvInvoices.RowInserted += _gvInvoices_RowInserted;
                _gvInvoices.RowUpdated += _gvInvoices_RowUpdated;

                //_gvInvoices.ClientSideEvents.RowClick = string.Format("function (s, e) {{ var cb = eval('cbPage_' + e.visibleIndex); cb.SetChecked(true); }}");

                #endregion Server side and Client Side Events

                // Add the Grid to the page
                dvInvoiceGrid.Controls.Add(_gvInvoices);

                // Bind the Data

                // SetGridVwProperties(_gvInvoices);
                _gvInvoices.SettingsBehavior.AllowSelectByRowClick = false;

                //_gvInvoices.SettingsBehavior.AllowFocusedRow = false;
                //rpnlInvGrid.Controls.Add(_gvInvoices);

                //_gvInvoices.Columns.Insert(0, CreateCommandColumn());

                // BindGetInvoicesDataSource();
                _gvInvoices.Settings.UseFixedTableLayout = true;

                //_gvInvoices.Settings.HorizontalScrollBarMode = DevExpress.Web.ASPxClasses.ScrollBarMode.Visible;

                //_gvInvoices.CustomColumnSort += GVInvoicesColumnSort;

                #region Setting Fixed Column

                var invColumn = _gvInvoices.Columns[InvoiceColumns.INVNUMBER];
                invColumn.IfNotNull(x => x.FixedStyle = GridViewColumnFixedStyle.Left);
                invColumn.IfNotNull(x => x.Width = Unit.Pixel(70));
                var invDt = _gvInvoices.Columns[InvoiceColumns.INVDATE];
                invDt.IfNotNull(x => x.FixedStyle = GridViewColumnFixedStyle.Left);
                invDt.IfNotNull(x => x.Width = Unit.Pixel(70));
                var vendorId = _gvInvoices.Columns[InvoiceColumns.VENDORID];
                vendorId.IfNotNull(x => x.FixedStyle = GridViewColumnFixedStyle.Left);
                vendorId.IfNotNull(x => x.Width = Unit.Pixel(70));

                var vendorName = _gvInvoices.Columns[InvoiceColumns.VENDORNAME];
                vendorName.IfNotNull(x => x.FixedStyle = GridViewColumnFixedStyle.Left);
                vendorName.IfNotNull(x => x.Width = Unit.Pixel(150));

                var audInit1 = _gvInvoices.Columns[InvoiceColumns.AUD1];
                audInit1.IfNotNull(x => x.Width = Unit.Pixel(100));
                var audInit2 = _gvInvoices.Columns[InvoiceColumns.AUD2];
                audInit2.IfNotNull(x => x.Width = Unit.Pixel(100));

                var econ = _gvInvoices.Columns[InvoiceColumns.ECONBENEFITDATE];
                econ.IfNotNull(x => x.Width = Unit.Pixel(60));
                var actionDate = _gvInvoices.Columns[InvoiceColumns.ACTIONDATE];
                actionDate.IfNotNull(x => x.Width = Unit.Pixel(70));

                var billedAmt = _gvInvoices.Columns[InvoiceColumns.BILLEDAMT];
                billedAmt.IfNotNull(x => x.Width = Unit.Pixel(65));
                var balance = _gvInvoices.Columns[InvoiceColumns.BALANCE];
                balance.IfNotNull(x => x.Width = Unit.Pixel(65));

                #endregion Setting Fixed Column

                #region Format Columns

                var claimAmtTotal = _gvInvoices.Columns[InvoiceColumns.RECOVERYAMT]
                                                         as GridViewDataTextColumn;
                claimAmtTotal.IfNotNull(x => x.PropertiesTextEdit.DisplayFormatString
                                                                          = "N2");
                var invoiceAmt = _gvInvoices.Columns[InvoiceColumns.BILLEDAMT]
                                                    as GridViewDataTextColumn;
                invoiceAmt.IfNotNull(x => x.PropertiesTextEdit.DisplayFormatString
                                                                       = "N2");
                var invoiceBalance = _gvInvoices.Columns[InvoiceColumns.BALANCE]
                                                      as GridViewDataTextColumn;
                invoiceBalance.IfNotNull(x => x.PropertiesTextEdit.DisplayFormatString
                                                                           = "N2");

                #endregion Format Columns

                var attachmntColumn = _gvInvoices.Columns[InvoiceColumns.ATTACHMENTS]
                                                             as GridViewDataColumn;
                attachmntColumn.IfNotNull(x => x.Width = 170);
                attachmntColumn.IfNotNull(x => x.DataItemTemplate = new UploaderTemplate { AuditName = ASPxComboBoxAuditName.Text });
                _gvInvoices.CustomJSProperties += _gvInvoices_CustomJSProperties;
            }
            catch (Exception)
            {
            }
        }

        private void _gvInvoices_RowUpdated(object sender, ASPxDataUpdatedEventArgs e)
        {
            ASPxGridView gridView = (ASPxGridView)sender;
            for (int i = 0; i < gridView.Columns.Count; i++)
                if (gridView.Columns[i] is GridViewDataColumn)
                {
                  ////var column=  e.NewValues[Session["InvColumn"].ToString()]
                    ((GridViewDataColumn)gridView.Columns[i]).EditItemTemplate = null;
                    Session["InvColumn"] = null;
                }

            //For Single Cell Edit Event
            var NewValue = e.NewValues[0];
            //BindGetInvoicesDataSource();
            e.ExceptionHandled = true;
        }

        private void _gvInvoices_RowInserted(object sender, ASPxDataInsertedEventArgs e)
        {
            //For Single Cell Edit Event
            var NewValue = e.NewValues[0];
            BindGetInvoicesDataSource();
            e.ExceptionHandled = true;
        }

        protected void gridInvoice_HtmlDataCellPrepared(object sender,
                                 ASPxGridViewTableDataCellEventArgs e)
        {
            var columnupdatable = new List<string>();
            columnupdatable.Add(InvoiceColumns.RCVDRF);
            columnupdatable.Add(InvoiceColumns.INVCOMMENTS);
            columnupdatable.Add(InvoiceColumns.APPROVALSTATUS);
            columnupdatable.Add(InvoiceColumns.AFTERPAYID);
            columnupdatable.Add(InvoiceColumns.PAYID);
            columnupdatable.Add(InvoiceColumns.ACTIONOWNER);
            columnupdatable.Add(InvoiceColumns.ACTIONDATE);
            columnupdatable.Add(InvoiceColumns.ACTIONREQUIRED);
            if (columnupdatable.Contains(e.DataColumn.FieldName))
            {
                e.Cell.Attributes.Add("ondblclick", String.Format("onInvoiceCellClick({0}, '{1}')", e.VisibleIndex, e.DataColumn.FieldName));
                e.Cell.BackColor = System.Drawing.Color.White;
                e.Cell.ForeColor = System.Drawing.Color.Black;

                // e.Cell.CssClass = "EditableCellCustomize";
                e.Cell.ToolTip = "Click to Edit";
            }
        }

        private void _gvInvoices_CustomJSProperties(object sender,
                        ASPxGridViewClientJSPropertiesEventArgs e)
        {
            var grid = sender as ASPxGridView;
            int start = grid.IfNotNull(x => x.VisibleStartIndex);
            //int end = grid.IfNotNull(x => x.VisibleStartIndex + grid.SettingsPager.PageSize);
            int end = grid.IfNotNull(x => x.VisibleStartIndex + grid.VisibleRowCount);
            int selectNumbers = 0;
            if (!grid.IsNull())
            {
                end = (end > grid.VisibleRowCount ? grid.VisibleRowCount : end);
            }

            //object[] chkDataAttach = new object[end - start], titles
            //= new object[end - start];

            for (var i = start; i < end; i++)
            {
                //chkDataAttach[i - start] = grid.GetRow(i);
                //grid.GetRowValues(i, "title_id");

                if (grid.IfNotNull(x => x.Selection.IsRowSelected(i)))
                {
                    selectNumbers++;
                }
                e.Properties["cpSelectedRowsOnPage"] = selectNumbers;
                e.Properties["cpVisibleRowCount"] = grid.IfNotNull(x => x.VisibleRowCount);
            }

            // e.Properties["cpchkDataAttach"] = chkDataAttach;
        }

        private void CreateReadOnlyTemplate(ASPxGridView grid)
        {
            var columnEditable = new List<string>();
            columnEditable.Add(InvoiceColumns.RCVDRF);
            columnEditable.Add(InvoiceColumns.INVCOMMENTS);
            columnEditable.Add(InvoiceColumns.APPROVALSTATUS);
            columnEditable.Add(InvoiceColumns.ECONBENEFITDATE);
            columnEditable.Add(InvoiceColumns.PAYID);
            var template = new ReadOnlyTemplate();

            foreach (var gridcolumn in grid.Columns)
            {
                var col = (gridcolumn as GridViewDataColumn);
                if (!columnEditable.Contains(col.IfNotNull(x => x.FieldName)))
                {
                   col.IfNotNull(x => x.ReadOnly=true//EditItemTemplate = template
                       );
                }
            }

            //(grid.Columns["UnitPrice"] as GridViewDataColumn).EditItemTemplate
            //= template;
            //(grid.Columns["UnitsInStock"] as GridViewDataColumn).EditItemTemplate
            //= template;
            //(grid.Columns["UnitsOnOrder"] as GridViewDataColumn).EditItemTemplate
            //= template;
        }

        private void _gvInvoices_CellEditorInitialize(object sender, ASPxGridViewEditorEventArgs e)
        {
            e.Editor.Enabled = !e.Column.ReadOnly;
            if (e.Editor.Enabled)
            {
                var type = e.Editor.GetType();
                if (type.FullName == "DevExpress.Web.ASPxEditors.ASPxTextBox")
                {
                    ((ASPxTextBox)e.Editor).ClientSideEvents.KeyPress = "function(s,e) {OnInvoiceEditorKeyPress(s, e);}";
                }
                else
                    if (type.FullName == "DevExpress.Web.ASPxEditors.ASPxDateEdit")
                    {
                        ((ASPxDateEdit)e.Editor).ClientSideEvents.KeyPress = "function(s,e) {OnInvoiceEditorKeyPress(s, e);}";
                    }
            }
        }

        private void _gvInvoices_CustomCallback(object sender,
                        ASPxGridViewCustomCallbackEventArgs e)
        {
            if (e.Parameters.Contains("OnChange"))
            {
                BindGetInvoicesDataSource();
                BindGetClaimsDataSource();
                //_gvInvoices.Selection.SelectAll();
            }
            else
                if (e.Parameters.Contains('|'))
                {
                    var gridView = (ASPxGridView)sender;

                    // gridView.UpdateEdit();
                    var data = e.Parameters.Split(new char[] { '|' });
                    gridView.FocusedRowIndex = Convert.ToInt32(data[0]);
                    for (var i = 0; i < gridView.Columns.Count; i++)
                    {
                        if (i == 0)
                        {
                            // comColumn is never used, why is it here?
                            var comColumn = (GridViewCommandColumn)gridView.Columns[i];
                        }
                        else
                        {
                            var column = (GridViewDataColumn)gridView.Columns[i];
                            if (column != null)
                                if (column.FieldName != data[1])
                                    column.ReadOnly=true;// = new ReadOnlyTemplate();
                                else
                                {
                                    Session["InvColumn"] = column.FieldName;
                                    column.EditItemTemplate = null;
                                }
                        }
                    }
                    gridView.StartEdit(Convert.ToInt32(data[0]));
                }
        }

        private void _gvInvoices_AfterPerformCallback(object sender,
                        ASPxGridViewAfterPerformCallbackEventArgs e)
        {
            var callBack = e.CallbackName;
            if (callBack == "STARTEDIT")
            {
                //_gvInvoices.StartEdit(e.
            }
        }

        private void GvInvoices_HtmlRowCreated(object sender,
                             ASPxGridViewTableRowEventArgs e)
        {
            var invoiceGrid = (ASPxGridView)sender;

            if (e.RowType == GridViewRowType.Data)
            {
                e.Row.Height = 8;
                invoiceGrid.JSProperties["cpPageIndex" + e.VisibleIndex] = e.Row.Height;
            }
            else
            {
                if (e.RowType == GridViewRowType.InlineEdit)
                {
                    string fieldName = Session["InvColumn"] == null ? "" : Session["InvColumn"].ToString();

                    //Exclude Command Column
                    for (int i = 0; i < e.Row.Cells.Count - 1; i++)
                    {
                        //Exclude Command Column
                        if (i != 0)
                        {
                            GridViewDataColumn column = ((GridViewDataColumn)((ASPxGridView)sender).VisibleColumns[i]);
                            if (column.FieldName != fieldName)
                                e.Row.Cells[i].Attributes.Add("ondblclick", String.Format("onInvoiceCellClick({0}, '{1}')", e.VisibleIndex, column.FieldName));
                        }
                    }
                }
            }
        }

        protected void GvInvoicesFocusedRowChanged(object sender, EventArgs e)
        {
            if (IsPostBack)
            {
                //var invoiceGrid = sender as ASPxGridView;
                var grid = sender as ASPxGridView;

                #region Setting Column Filter Session

                //
                // This region takes care of column filter
                // setting to the child session
                //
                var sortCols = grid.IfNotNull(g => g.GetSortedColumns());
                IList<GridViewDataColumn> lstCols = sortCols;

                foreach (var col in lstCols)
                {
                    Session["SortedColumns"] = string.Format("{0}|{1}",
                        col.FieldName, col.SortOrder);
                }

                #endregion Setting Column Filter Session

                #region Setting Row Filter Session

                //
                // This region takes care of row filter
                // setting to the child session
                //
                _invNum = grid.IfNotNull(g => Convert.ToString(g.GetRowValues
                    (g.FocusedRowIndex, _gvInvoices.KeyFieldName)));
                _gvInvoices.JSProperties["cpIsUpdated"] = true;

                Session[InvoiceColumns.INVNUMBER] = _invNum;

                #endregion Setting Row Filter Session

                #region Attachment and Print settings

                //var clmn = new GridViewDataColumn();
                //clmn = (GridViewDataColumn)grid.Columns["Attachments"];
                //clmn.Name = "Attachments";
                //clmn.DataItemTemplate = new GridFileAttachmentTemplate("Attachments");
                //clmn.Width = 70;

                //clmn = (GridViewDataColumn)grid.Columns["RowSelection"];
                //clmn.Name = "RowSelection";
                //clmn.DataItemTemplate = new GridFileAttachmentTemplate("RowSelection");
                //clmn.Width = 35;

                #endregion Attachment and Print settings
            }
        }

        private void GvInvoices_CustomColumnSort(object sender,
                                   CustomColumnSortEventArgs e)
        {
            CompareColumnValues(e);
        }

        private void GvInvoices_CustomColumnGroup(object sender,
                                    CustomColumnSortEventArgs e)
        {
            CompareColumnValues(e);
        }

        private void CompareColumnValues(CustomColumnSortEventArgs e)
        {
            var aVal = e.Value1;
            if (aVal is int || aVal is double || aVal is decimal)
            {
                var res = 0;

                var x = Math.Floor(Convert.ToDouble(e.Value1) / 1000);
                var y = Math.Floor(Convert.ToDouble(e.Value2) / 1000);
                res = Comparer.Default.Compare(x, y);
                if (res < 0)
                    res = -1;
                else
                    if (res > 0)
                        res = 1;

                e.Result = res;
                e.Handled = true;
            }
        }

        private void AddSummary(string colName)
        {
            foreach (ASPxSummaryItem sum in _gvInvoices.TotalSummary)
            {
                if (sum.FieldName == colName)
                    return;
            }

            var totalSummary = new ASPxSummaryItem() { FieldName = colName, ShowInColumn = colName, SummaryType = colName.ToLower().Contains("id") ? SummaryItemType.Count : SummaryItemType.Sum, DisplayFormat = colName.ToLower().Contains("id") ? string.Empty : "c" };
            _gvInvoices.TotalSummary.Add(totalSummary);
        }

        protected void GvInvoices_CustomGroupDisplayText(object sender,
                              ASPxGridViewColumnDisplayTextEventArgs e)
        {
            var aVal = e.Value;
            if (aVal is int || aVal is double || aVal is decimal)
            {
                double d = Math.Floor(Convert.ToDouble(e.Value) / 1000);
                string displayText = string.Format("{0:c} - {1:c} ", d * 1000,
                                                              (d + 1) * 1000);
                e.DisplayText = displayText;

                AddSummary(e.Column.FieldName.ToString(CultureInfo.InvariantCulture));
            }
        }

        #endregion Invoice Grid

        #region Claims Grid
        /// <summary>
        /// Create Claims Grid Model
        /// </summary>
        private void CreateClaimColumn()
        {
            //We can set the visible index of the column from here
            var INVNUMBER = new GridViewDataTextColumn { FieldName = APR.Web.UI.Portal.Code.ClaimsColumns.INVNUMBER, FixedStyle = GridViewColumnFixedStyle.Left, Width = Unit.Pixel(65),VisibleIndex=0 ,ReadOnly=true};
            _gvClaims.Columns.Add(INVNUMBER);
            var INVDATE = new GridViewDataDateColumn { FieldName = APR.Web.UI.Portal.Code.ClaimsColumns.INVDATE, FixedStyle = GridViewColumnFixedStyle.Left, Width = 65, ReadOnly = true };
            _gvClaims.Columns.Add(INVDATE);
            var CLAIMID = new GridViewDataTextColumn { FieldName = APR.Web.UI.Portal.Code.ClaimsColumns.CLAIMID, FixedStyle = GridViewColumnFixedStyle.Left, Width = Unit.Pixel(70), VisibleIndex = 1, ReadOnly = true };
            _gvClaims.Columns.Add(CLAIMID);
            var REFERENCEDATE = new GridViewDataDateColumn { FieldName = APR.Web.UI.Portal.Code.ClaimsColumns.REFERENCEDATE, FixedStyle = GridViewColumnFixedStyle.Left, Width = 65 };
            _gvClaims.Columns.Add(REFERENCEDATE);
            var RERENCENUM = new GridViewDataTextColumn { FieldName = APR.Web.UI.Portal.Code.ClaimsColumns.RERENCENUM, Width = Unit.Pixel(70), ReadOnly = true };
            _gvClaims.Columns.Add(RERENCENUM);
            var CLAIMAMOUNT = new GridViewDataTextColumn { FieldName = APR.Web.UI.Portal.Code.ClaimsColumns.CLAIMAMOUNT, Width = Unit.Pixel(70), ReadOnly = true };
            CLAIMAMOUNT.PropertiesTextEdit.DisplayFormatString = "N2";
            CLAIMAMOUNT.CellStyle.HorizontalAlign = HorizontalAlign.Right;
            _gvClaims.Columns.Add(CLAIMAMOUNT);
            var BILLEDAMOUNT = new GridViewDataTextColumn { FieldName = APR.Web.UI.Portal.Code.ClaimsColumns.BILLEDAMOUNT, Width = Unit.Pixel(70), ReadOnly = true };
            BILLEDAMOUNT.PropertiesTextEdit.DisplayFormatString = "N2";
            BILLEDAMOUNT.CellStyle.HorizontalAlign = HorizontalAlign.Right;
            _gvClaims.Columns.Add(BILLEDAMOUNT);
            var CLAIMDESC = new GridViewDataTextColumn { FieldName = APR.Web.UI.Portal.Code.ClaimsColumns.CLAIMDESC, Width = Unit.Pixel(120), ReadOnly = true };
            _gvClaims.Columns.Add(CLAIMDESC);
            var CRTYPE = new GridViewDataTextColumn { FieldName = APR.Web.UI.Portal.Code.ClaimsColumns.CRTYPE, Width = Unit.Pixel(70), ReadOnly = true };
            _gvClaims.Columns.Add(CRTYPE);
            var ROOTCAUSE = new GridViewDataTextColumn { FieldName = APR.Web.UI.Portal.Code.ClaimsColumns.ROOTCAUSE, Width = Unit.Pixel(70), ReadOnly = true };
            _gvClaims.Columns.Add(ROOTCAUSE);
            var SYSTEMORIGIN = new GridViewDataTextColumn { FieldName = APR.Web.UI.Portal.Code.ClaimsColumns.SYSTEMORIGIN, Width = Unit.Pixel(70), ReadOnly = true };
            _gvClaims.Columns.Add(SYSTEMORIGIN);
            var USERDEF1 = new GridViewDataTextColumn { FieldName = APR.Web.UI.Portal.Code.ClaimsColumns.USERDEF1, Width = Unit.Pixel(70), ReadOnly = true };
            _gvClaims.Columns.Add(USERDEF1);
            var USERDEF2 = new GridViewDataTextColumn { FieldName = APR.Web.UI.Portal.Code.ClaimsColumns.USERDEF2, Width = Unit.Pixel(70), ReadOnly = true };
            _gvClaims.Columns.Add(USERDEF2);
            var CLAIMAPPROVAL = new GridViewDataComboBoxColumn { FieldName = APR.Web.UI.Portal.Code.ClaimsColumns.CLAIMAPPROVAL, Width = Unit.Pixel(70) };
            CLAIMAPPROVAL.PropertiesComboBox.Items.Add(new ListEditItem("Approved", "Approved"));
            CLAIMAPPROVAL.PropertiesComboBox.Items.Add(new ListEditItem("Disputed", "Disputed"));
           
            _gvClaims.Columns.Add(CLAIMAPPROVAL);
            var CLAIMCOMMENTS = new GridViewDataTextColumn { FieldName = APR.Web.UI.Portal.Code.ClaimsColumns.CLAIMCOMMENTS, Width = Unit.Pixel(70), ReadOnly = true };
            _gvClaims.Columns.Add(CLAIMCOMMENTS);
            var VENDORNAME = new GridViewDataTextColumn { FieldName = APR.Web.UI.Portal.Code.ClaimsColumns.VENDORNAME, Width = Unit.Pixel(70), ReadOnly = true };
            _gvClaims.Columns.Add(VENDORNAME);
            var VENDORID = new GridViewDataTextColumn { FieldName = APR.Web.UI.Portal.Code.ClaimsColumns.VENDORID, Width = Unit.Pixel(70), ReadOnly = true };
            _gvClaims.Columns.Add(VENDORID);

        }

        private void CreateClaimsGridView()
        {
            try
            {
                _gvClaims = new AprASPxGridview() { ID = ClmGridId, ClientInstanceName = "gvClaims", KeyFieldName = APR.Web.UI.Portal.Code.ClaimsColumns.CLAIMID };
                CreateClaimColumn();
                _gvClaims.HtmlRowCreated += GvClaims_HtmlRowCreated;
                //_gvClaims.CustomButtonCallback += GvClaimsCustomButtonCallback;
                _gvClaims.HtmlDataCellPrepared += grid_HtmlDataCellPrepared;
                _gvClaims.CellEditorInitialize += _gvClaims_CellEditorInitialize;
                _gvClaims.ProcessColumnAutoFilter += _gvClaims_ProcessColumnAutoFilter;
                _gvClaims.RowUpdated += grid_RowUpdated;
                _gvClaims.CustomCallback += gvClaims_CustomCallback;
                _gvClaims.SettingsText.Title = AuditColumns.CLAIMS;

                //SetGridVwProperties(_gvClaims);
                var headerTitleTemplate = new HeaderTitleTemplate();

                //headerTitleTemplate.ExportExcel.Attributes.Add("onClick", "ExportExcel_Click();return false;");
                //headerTitleTemplate.ExportExcel.Click += (s, e) => PrintingHandle(ClmGridId);
                _gvClaims.Templates.TitlePanel = headerTitleTemplate;// new HeaderTitleTemplateDetail(_gvClaims);
                _gvClaims.ClientSideEvents.EndCallback = "LoadScripts";

                // Add Grid to div
                dvClaimsGrid.Controls.Add(_gvClaims);

                // Bind data
                BindGetClaimsDataSource();

                #region Setting Fixed Column

                //var invColumn = _gvClaims.Columns[ClaimsColumns.INVNUMBER];
                //invColumn.IfNotNull(x => x.FixedStyle = GridViewColumnFixedStyle.Left);
                //invColumn.IfNotNull(x => x.Width = Unit.Pixel(65));
                //var vendorName = _gvClaims.Columns[ClaimsColumns.VENDORNAME];
                //vendorName.IfNotNull(x => x.Width = Unit.Pixel(100));
                //var claimId = _gvClaims.Columns[ClaimsColumns.CLAIMID];
                //claimId.IfNotNull(x => x.Width = Unit.Pixel(70));
                //var claimDescription = _gvClaims.Columns[ClaimsColumns.CLAIMDESC];
                //claimDescription.IfNotNull(x => x.Width = Unit.Pixel(150));
                //var vendorId = _gvClaims.Columns[ClaimsColumns.VENDORID];
                //vendorId.IfNotNull(x => x.Width = Unit.Pixel(62));
                //var crType = _gvClaims.Columns[ClaimsColumns.CRTYPE];
                //crType.IfNotNull(x => x.Width = Unit.Pixel(62));
                //crType.IfNotNull(x => x.CellStyle.HorizontalAlign = HorizontalAlign.Center);
                //var referenceTyp = _gvClaims.Columns[ClaimsColumns.RERENCENUM];
                //referenceTyp.IfNotNull(x => x.Width = Unit.Pixel(60));
                //var billAmount = _gvClaims.Columns[ClaimsColumns.BILLEDAMOUNT];
                //billAmount.IfNotNull(x => x.Width = Unit.Pixel(60));
                //billAmount.IfNotNull(x => x.CellStyle.HorizontalAlign = HorizontalAlign.Right);

                #endregion Setting Fixed Column

                #region Format Column

                var claimAmt = _gvClaims.Columns[APR.Web.UI.Portal.Code.ClaimsColumns.CLAIMAMOUNT] as GridViewDataTextColumn;
                claimAmt.IfNotNull(x => x.PropertiesTextEdit.DisplayFormatString = "N2");
                var billedAmt = _gvClaims.Columns[APR.Web.UI.Portal.Code.ClaimsColumns.BILLEDAMOUNT] as GridViewDataTextColumn;
                billedAmt.IfNotNull(x => x.PropertiesTextEdit.DisplayFormatString = "N2");

                #endregion Format Column
            }
            catch
            {
            }
        }

        private void _gvClaims_ProcessColumnAutoFilter(object sender, ASPxGridViewAutoFilterEventArgs e)
        {
            if (e.Value == "|")
                return;
            if (e.Column.FieldName != APR.Web.UI.Portal.Code.ClaimsColumns.INVNUMBER)
                return;
            if (e.Kind != GridViewAutoFilterEventKind.CreateCriteria)
                return;

            //var c = e.Value;
            e.Criteria = (new OperandProperty(APR.Web.UI.Portal.Code.ClaimsColumns.INVNUMBER) == e.Value);
        }

        private void _gvClaims_CellEditorInitialize(object sender, ASPxGridViewEditorEventArgs e)
        {
            e.Editor.Enabled = !e.Column.ReadOnly;
            if (e.Editor.Enabled)
            {
                var type = e.Editor.GetType();
                if (type.FullName == "DevExpress.Web.ASPxEditors.ASPxTextBox")
                {
                    ((ASPxTextBox)e.Editor).ClientSideEvents.KeyPress = "function(s,e) {OnEditorKeyPress(s, e);}";
                }
                else
                    if (type.FullName == "DevExpress.Web.ASPxEditors.ASPxDateEdit")
                    {
                        ((ASPxDateEdit)e.Editor).ClientSideEvents.KeyPress = "function(s,e) {OnEditorKeyPress(s, e);}";
                    }
            }
        }

        private const int TextMaxLength = 50;

        protected void grid_HtmlDataCellPrepared(object sender, ASPxGridViewTableDataCellEventArgs e)
        {
            if (e.DataColumn.FieldName == APR.Web.UI.Portal.Code.ClaimsColumns.CLAIMDESC)
            {
                if (e.CellValue != null)
                {
                    e.Cell.ToolTip = e.CellValue.ToString();
                }
            }
            var columnupdatable = new List<string>();
            columnupdatable.Add(APR.Web.UI.Portal.Code.ClaimsColumns.CLAIMAPPROVAL);
            columnupdatable.Add(APR.Web.UI.Portal.Code.ClaimsColumns.CLAIMCOMMENTS);
            //var fieldName = Session["Claimscolumn"] == null ? "" : Session["Claimscolumn"].ToString()
            if (columnupdatable.Contains(e.DataColumn.FieldName))
            {
                e.Cell.Attributes.Add("ondblclick", String.Format("onClaimsCellClick({0}, '{1}')", e.VisibleIndex, e.DataColumn.FieldName));
                e.Cell.BackColor = System.Drawing.Color.White;
                e.Cell.ForeColor = System.Drawing.Color.Black;

                // e.Cell.CssClass = "EditableCellCustomize";
            }
        }

        private void GvClaims_HtmlRowCreated(object sender, ASPxGridViewTableRowEventArgs e)
        {
            var claimsGrid = (ASPxGridView)sender;

            if (e.RowType == GridViewRowType.Data)
            {
                e.Row.Height = 8;
                claimsGrid.JSProperties["cpPageIndex" + e.VisibleIndex] = e.Row.Height;
            }

            if (e.RowType == GridViewRowType.InlineEdit)
            {
                var fieldName = Session["Claimscolumn"] == null ? "" : Session["Claimscolumn"].ToString();
                for (var i = 0; i < e.Row.Cells.Count - 1; i++)
                {
                    //Horizontal Scroling ExtraCell
                    var column = ((GridViewDataColumn)((ASPxGridView)sender).VisibleColumns[i]);

                    if (column.FieldName != fieldName)
                        e.Row.Cells[i].Attributes.Add("ondblclick", String.Format("onClaimsCellClick({0}, '{1}')", e.VisibleIndex, column.FieldName));
                }
            }
        }

        protected void grid_RowUpdated(object sender, ASPxDataUpdatedEventArgs e)
        {
            var gridView = (ASPxGridView)sender;
            for (var i = 0; i < gridView.Columns.Count; i++)
            {
                if (gridView.Columns[i] is GridViewDataColumn)
                {
                    ((GridViewDataColumn)gridView.Columns[i]).EditItemTemplate = null;
                    Session["column"] = null;
                }
            }
            e.ExceptionHandled = true;
        }

        protected void SetGridVwProperties(ASPxGridView grid)
        {
            try
            {
                // List in ABC order

                #region Grid Properties Section

                grid.ClientIDMode = System.Web.UI.ClientIDMode.Static;

                //grid.AutoGenerateColumns = true;

                //grid.CssFilePath = "~/css/master.css";
                grid.EnableCallBacks = true;
                grid.KeyboardSupport = true;
                grid.FilterEnabled = true;
                grid.EnableTheming = false;

                //grid.PreviewFieldName = "Claim Description";

                #region Setting Loading Panel

                grid.SettingsLoadingPanel.Mode = GridViewLoadingPanelMode.ShowAsPopup;
                grid.SettingsLoadingPanel.ImagePosition = DevExpress.Web.ASPxClasses.ImagePosition.Left;
                grid.SettingsLoadingPanel.Text = "";

                #endregion Setting Loading Panel

                //grid.Settings.ShowPreview = true;
                grid.Styles.AlternatingRow.Enabled = DevExpress.Utils.DefaultBoolean.True;
                grid.Width = Unit.Percentage(100);

                #endregion Grid Properties Section

                //grid.Theme = "Aqua";
                // List in ABC order

                #region Settings Properties Section

                grid.Settings.ColumnMinWidth = 40;
                grid.Settings.EnableFilterControlPopupMenuScrolling = true;

                // grid.Settings.VerticalScrollableHeight = 300;
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

                // Allows columns to be grouped via panel above columns
                // to be dragged and dropped
                grid.Settings.ShowGroupPanel = true;

                // Filter button represents
                // the windows style drop down filter on the column
                grid.Settings.ShowHeaderFilterButton = true;
                grid.Settings.ShowTitlePanel = true;

                #endregion Settings Properties Section

                // List in ABC order

                #region SettingsBehavior Properties Section

                grid.SettingsBehavior.AllowDragDrop = true;
                grid.SettingsBehavior.AllowFocusedRow = true;

                grid.SettingsBehavior.AllowGroup = true;
                grid.SettingsBehavior.AllowSelectByRowClick = false;

                //grid.SettingsBehavior.AllowSelectSingleRowOnly = true;
                grid.SettingsBehavior.AllowSort = true;
                grid.SettingsBehavior.AllowClientEventsOnLoad = true;
                grid.SettingsBehavior.ColumnResizeMode = DevExpress.Web.
                          ASPxClasses.ColumnResizeMode.Control;
                grid.SettingsBehavior.EnableCustomizationWindow = true;
                grid.SettingsBehavior.EnableRowHotTrack = true;
                grid.SettingsBehavior.ProcessFocusedRowChangedOnServer = true;
                grid.SettingsBehavior.SortMode = DevExpress.XtraGrid
                                                    .ColumnSortMode.Value;

                #endregion SettingsBehavior Properties Section

                #region SettingsText Properties Section

                #endregion SettingsText Properties Section

                // List in ABC order

                #region SettingsPager Properties Section

                // Turns off paging
                // Turns off paging
                grid.SettingsPager.Mode = GridViewPagerMode.ShowAllRecords;

                //grid.Settings.VerticalScrollableHeight = 400;

                #endregion SettingsPager Properties Section

                #region SettingsPager Properties Section

                // Turns off paging
                //grid.SettingsPager.Mode = GridViewPagerMode.ShowAllRecords;
                //grid.SettingsPager.RenderMode = DevExpress.Web.ASPxClasses.ControlRenderMode.Lightweight;

                #endregion SettingsPager Properties Section

                #region Setting Editing

                grid.SettingsEditing.Mode = GridViewEditingMode.Inline;

                #endregion Setting Editing

                grid.Templates.TitlePanel = new HeaderTitleTemplate();

                #region Set Filter Mode

                foreach (GridViewDataColumn column in grid.Columns)
                {
                    column.Settings.HeaderFilterMode = HeaderFilterMode.List;
                }

                #endregion Set Filter Mode
            }
            catch
            {
            }
        }

    

        protected void gvClaims_CustomCallback(object sender, ASPxGridViewCustomCallbackEventArgs e)
        {
               var childGrid = sender as ASPxGridView;



               if (e.Parameters.Contains('|'))
               {
                   var gridView = (ASPxGridView)sender;

                   // gridView.UpdateEdit();
                   var data = e.Parameters.Split(new char[] { '|' });
                   gridView.FocusedRowIndex = Convert.ToInt32(data[0]);
                   for (var i = 0; i < gridView.Columns.Count; i++)
                   {
                       //if (i == 0)
                       //{
                       //    // comColumn is never used, why is it here?
                       //    var comColumn = (GridViewCommandColumn)gridView.Columns[i];
                       //}
                       //else
                       //{
                       var column = (GridViewDataColumn)gridView.Columns[i];
                       if (column != null)
                           if (column.FieldName != data[1])
                               column.ReadOnly = true;//EditItemTemplate = new ReadOnlyTemplate();
                           else
                           {
                               Session["InvColumn"] = column.FieldName;
                               column.EditItemTemplate = null;
                           }
                       //}
                   }
                   gridView.StartEdit(Convert.ToInt32(data[0]));
               }
               else
               {


                   #region Filtering

                   #region Getting Invoices Column filters

                   //
                   // Getting the colum filter volums from parent(Invoices)
                   // and setting the child grid (columns
                   //
                   string strSort = Session["SortedColumns"].IfNotNull(sessnSrtClms =>
                                                             sessnSrtClms.ToString());
                   if (!strSort.IsNullOrBlank())
                   {
                       string[] sortArray = strSort.Split('|');
                       ColumnSortOrder sortOrder = ColumnSortOrder.Ascending.ToString() == sortArray[1]
                       ? ColumnSortOrder.Ascending : ColumnSortOrder.Descending;

                       if (!childGrid.IsNull() && sortArray[0] == InvoiceColumns.RECOVERYAMT)
                       {
                           childGrid.ClearSort();
                           childGrid.SortBy(childGrid.Columns[APR.Web.UI.Portal.Code.ClaimsColumns.CLAIMAMOUNT], sortOrder);
                       }
                       Session["SortedColumns"] = null;
                   }

                   #endregion Getting Invoices Column filters

                   #endregion Filtering

                   //
                   // Getting the row filter volums from parent(Invoices)
                   // and setting the child grid (columns
                   //

                   #region Getting Invoices row filters

                   if (!childGrid.IsNull())
                   {
                       // Filter your list code below
                       string[] fields = { InvoiceColumns.INVNUMBER };
                       var filterExpression = new StringBuilder();

                       var filterValues = _gvInvoices.GetSelectedFieldValues(fields).ToList();
                       if (filterValues.Count != 0)
                       {
                           for (int i = 0; i < filterValues.Count; i++)
                           {
                               var filtInvNum = filterValues[i];
                               if (!filtInvNum.IsNull())
                               {
                                   filterExpression.Append(i == 0 ? string.Format("[" + InvoiceColumns.INVNUMBER + "] ='{0}'",
                                   filtInvNum) : string.Format(" Or [" + InvoiceColumns.INVNUMBER + "] ='{0}'", filtInvNum));
                               }
                           }
                           childGrid.IfNotNull(x => x.FilterExpression = filterExpression.ToString());
                           if (!childGrid.IsNull())
                           {
                               childGrid.Selection.SelectAll();
                           }
                       }
                       else
                       {
                           childGrid.IfNotNull(x => x.FilterExpression = noFilter);
                       }
                   }

                   #endregion Getting Invoices row filters
               }
        }

        protected void gridClaims_RowUpdated(object sender, ASPxDataUpdatedEventArgs e)
        {
            var gridView = (ASPxGridView)sender;
            for (var i = 0; i < gridView.Columns.Count; i++)
            {
                if (gridView.Columns[i] is GridViewDataColumn)
                {
                    ((GridViewDataColumn)gridView.Columns[i]).EditItemTemplate = null;
                    Session["column"] = null;
                }
            }
            e.ExceptionHandled = true;
        }

        #endregion Claims Grid

        #region Page Events And Utility

        protected void btnInvGripExpr_Click(object sender, EventArgs e)
        {
            //_gvInvGridExporter.WriteXlsToResponse(true);
        }

        protected void btnClmGripExpr_Click(object sender, EventArgs e)
        {
            //_gvClmsGridExporter.WriteXlsToResponse();
        }

        private GridViewCommandColumn CreateCommandColumn()
        {
            var lVisibleIndex = 0;
            var col = new GridViewCommandColumn() { FixedStyle = GridViewColumnFixedStyle.Left,ShowSelectCheckbox = true };
            col.UpdateButton.Visible = false;
            col.CancelButton.Visible = false;
            //col.EditButton.Visible = true;
            col.VisibleIndex = lVisibleIndex++;
            col.Caption = "";
            col.HeaderTemplate = new HeaderCheckBox();
            col.Width = 40;
            return col;
        }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);

            //CreateInvoicesGridView();
            //CreateClaimsGridView();

            ASPxComboBoxAuditName.SelectedIndexChanged += ASPxComboBoxAuditName_SelectedIndexChanged;
        }

        private void ASPxComboBoxAuditName_SelectedIndexChanged(object sender, EventArgs e)
        {
            CreateInvoicesGridView();
        }

        //protected void Page_Init(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        //BindGetInvoicesDataSource();
        //        LoadAuditCombo();
        //        CreateInvoicesGridView();
        //        CreateClaimsGridView();

        //        // CreateInvoicesGridView();

        //        //BindGetClaimsDataSource();
        //    }
        //    catch
        //    {
        //    }
        //}

        protected void Page_Load(object sender, EventArgs e)
        {
            LoadAuditCombo();
            CreateInvoicesGridView();
            CreateClaimsGridView();

            if (!Page.IsPostBack)
            {
                BindGetInvoicesDataSource();
                _gvInvoices.Selection.SelectAll();
            }

            if (!_gvClaims.IsNull())
            {
                if (_gvClaims.IsEditing && Session["column"] != null)
                {
                    string fieldName = Convert.ToString(Session["column"]);
                    for (int i = 0; i < _gvClaims.Columns.Count; i++)
                    {
                        var column = (GridViewDataColumn)_gvClaims.Columns[i];
                        if (column != null && column.FieldName != fieldName)
                        {
                            column.ReadOnly = true;//EditItemTemplate = new ReadOnlyTemplate();
                        }
                    }
                }
            }

            if (!IsPostBack)
            {
                foreach (GridViewColumn col in _gvClaims.Columns)
                {
                    var c = col as GridViewDataColumn;
                    if ((c != null)
                        && !(c.Grid.GetRowValues(0, c.FieldName) is int))
                    {
                        c.Settings.AutoFilterCondition = AutoFilterCondition.Like;
                    }
                }
            }
        }

        #endregion Page Events And Utility

        private void PrintingHandle(string gridId)
        {
            //var ps = new PrintingSystem();

            //    var header = new Link();

            //header.CreateDetailArea +=
            //new CreateAreaEventHandler(header_CreateDetailArea);

            ASPxGridViewExporter1.GridViewID = gridId;
            ASPxGridViewExporter1.WriteXlsToResponse();

            //var link1 = new DevExpress.Web.ASPxGridView.Export.Helper
            //                    .GridViewLink(ASPxGridViewExporter1);

            //var compositeLink = new CompositeLink(ps);
            //compositeLink.Links.AddRange(new object[] { header, link1 });

            //compositeLink.CreateDocument();
            //using (var stream = new MemoryStream())
            //{
            //    //compositeLink.PrintingSystem.ExportToPdf(stream);
            //    //compositeLink.PrintingSystem.ExportToPdf(stream);
            //    compositeLink.PrintingSystem.ExportToXls(stream);
            //    WriteToResponse("filename", true, "Xls", stream);
            //}
            //ps.Dispose();
        }

        private void WriteToResponse(string fileName, bool saveAsFile, string fileFormat, MemoryStream stream)
        {
            if (Page == null || Page.Response == null)
                return;
            string disposition = saveAsFile ? "attachment" : "inline";
            Page.Response.Clear();
            Page.Response.Buffer = false;
            Page.Response.AppendHeader("Content-Type", string.Format
                                   ("application/{0}", fileFormat));
            Page.Response.AppendHeader("Content-Transfer-Encoding", "binary");
            Page.Response.AppendHeader("Content-Disposition",
                string.Format("{0}; filename={1}.{2}", disposition, fileName, fileFormat));
            Page.Response.BinaryWrite(stream.GetBuffer());
            Page.Response.End();
        }
        protected void cb_Callback(object source, DevExpress.Web.ASPxCallback.CallbackEventArgs e)
        {
            //if (e.Parameter == "checkAll")
            //{
            //    var rows = _gvInvoices.VisibleRowCount;
            //   var AttachmentColumns= _gvInvoices.Columns[InvoiceColumns.ATTACHMENTS]as GridViewDataColumn;
               
            //}
            //else
            //{
                if (Session["InvoiceNO"] != null)
                {
                    var Values = e.Parameter.Split('|');
                    if(Values[1]=="add")
                    (Session["InvoiceNO"] as List<string>).Add(Values[0]);
                    else (Session["InvoiceNO"] as List<string>).Remove(Values[0]);
                    
                }
                else
                {
                    var InvoiceNO = new List<string>();
                    InvoiceNO.Add(e.Parameter);
                    Session["InvoiceNO"] = InvoiceNO;
                }
            //}
        }
    }
}