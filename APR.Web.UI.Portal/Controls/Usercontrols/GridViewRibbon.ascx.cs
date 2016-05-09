using DevExpress.Web.ASPxClasses;
using DevExpress.Web.ASPxNavBar;
using System;

namespace APR.Web.UI.Portal.Controls.Usercontrols
{
    public partial class GridViewRibbon : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
        }

        protected void Page_Init(object sender, EventArgs e)
        {
            CreateNavBar();
        }

        private void CreatePanel()
        {
            try
            {
            }
            catch (Exception e)
            {
            }
        }

        private void CreateNavBar()
        {
            try
            {
                gridRbbnNavBar.RenderMode = ControlRenderMode.Lightweight;

                var newGroup = new NavBarGroup();
                newGroup.Name = "grpPrint";
                newGroup.Text = "Print";
                newGroup.HeaderImage.Url = "~/images/printer.ico";

                var newItem = new NavBarItem();
                newItem.Name = "itmPrintAll";
                newItem.Text = "Print All Invoices";
                newGroup.Items.Add(newItem);

                newItem = new NavBarItem();
                newItem.Name = "itmPrintHighlighted";
                newItem.Text = "Print Highlighted Invoices";
                newGroup.Items.Add(newItem);

                gridRbbnNavBar.Groups.Add(newGroup);
            }
            catch (Exception e)
            {
            }
        }
    }
}
