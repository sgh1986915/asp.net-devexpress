using DevExpress.Web.ASPxEditors;
using DevExpress.Web.ASPxGridView;
using System;
using System.IO;
using System.Linq;
using System.Web.Script.Serialization;
using System.Web.UI;
using System.Web.UI.HtmlControls;

namespace APR.Web.UI.Portal.Code.UI.ITemplates
{
    public class UploaderTemplate : ITemplate
    {
        private readonly string serverPath = System.Configuration.ConfigurationManager.AppSettings["fileUploadPath"].ToString();
        //String CallbackArgumentFormat = "function (s, e) {{ cb.PerformCallback(\"{0}|{1}|\" + {2}); }}"; // key | field | value
        const String CallbackArgumentFormat = "function (s, e) {{ cb.PerformCallback(\"{0}\"); }}"; // key 
        private string auditName = string.Empty;

        public string AuditName
        {
            get
            {
                return auditName;
            }
            set
            {
                auditName = value;
            }
        }

        private GridViewDataItemTemplateContainer GetGridDataContainer(System.Web.UI.Control container)
        {
            return (GridViewDataItemTemplateContainer)container;
        }
        public void InstantiateIn(System.Web.UI.Control container)
        {

            var invNum = GetGridDataContainer(container).KeyValue.ToString();

            var a = new HtmlAnchor();
            var field = new string[] { "Attachments" };
            var items = GetGridDataContainer(container).Grid.GetRowValues(GetGridDataContainer(container).ItemIndex, "Attachments");

            var folder = String.Format("{0}\\{1}\\Portal\\Claims\\", serverPath, AuditName);
            if (Directory.Exists(folder))
            {
                var files = Directory.GetFiles(folder).ToList();
                ;

                files = files.Where(x => x.Contains(invNum)).ToList();
                if (files.Count != 0)
                {
                    a.Attributes.Add("class", "paperClick");
                }
                a.Attributes.Add("onClick", String.Format("CreateAlltachmentControl({0});", new JavaScriptSerializer().Serialize(files)));
                a.InnerHtml = files.Count + " Files";
            }
            else
            {
                a.Attributes.Add("class", "nopaperClick");
                a.Attributes.Add("onClick", "CreateAlltachmentControl('')");
                a.InnerHtml = "0 Files";
            }

            var chkAttach = new HtmlInputCheckBox() { ID = "chkDataAttach" };
            chkAttach.Attributes.Add("Class", "attachmentCheck");
            //chkAttach.Load += (s, e) =>
            //{
            //    var cb = s as ASPxCheckBox;
            //    var chkcontainer = cb.NamingContainer as GridViewDataItemTemplateContainer;
            //    cb.ClientInstanceName = String.Format("cbCheck{0}", chkcontainer.VisibleIndex);
            //};
            chkAttach.Attributes.Add("onchange", String.Format("if(this.checked)cb.PerformCallback(\"{0}|add\"); if(!this.checked) cb.PerformCallback(\"{0}|remove\")",
            GetGridDataContainer(container).KeyValue));
            //chkAttach.ClientSideEvents.CheckedChanged += String.Format(CallbackArgumentFormat,
            //GetGridDataContainer(container).KeyValue,
            //GetGridDataContainer(container).Column.FieldName,
            //"s.GetValue()");
            GetGridDataContainer(container).Controls.AddAt(0, chkAttach);
            GetGridDataContainer(container).Controls.AddAt(1, a);
        }
    }
}
