using DevExpress.Web.ASPxEditors;
using DevExpress.Web.ASPxGridView;
using System;
using System.Reflection;
using System.Web.UI;

namespace APR.Web.UI.Portal.Code.UI.ITemplates
{
    public class HeaderCheckBox : ITemplate
    {
        public void InstantiateIn(Control container)
        {
            var CheckAll = new ASPxCheckBox();
            var gridContainer = (GridViewHeaderTemplateContainer)container;

            CheckAll.ID = "chkAll";
            CheckAll.ClientInstanceName = "chkAll";
            CheckAll.CheckBoxStyle.CssClass = "checkboxes";
            CheckAll.Theme = "Red Wine";
            CheckAll.ClientSideEvents.CheckedChanged += "OnAllCheckedChanged";

            gridContainer.Controls.Add(CheckAll);
        }

        public string GetTextValue(Control control)
        {
            var controlType = control.GetType();

            var attr = controlType.GetCustomAttribute<ControlValuePropertyAttribute>();
            if (attr == null)
            {
                throw new InvalidOperationException("Control must be decorated with ControlValuePropertyAttribute");
            }
            var valueAttr = (ControlValuePropertyAttribute)attr;

            var propertyName = valueAttr.Name;

            var propertyInfo = controlType.GetProperty(propertyName);

            var val = propertyInfo.GetValue(control);
            if (val == null)
            {
                val = string.Empty;
            }
            return val.ToString();
        }
    }
}
