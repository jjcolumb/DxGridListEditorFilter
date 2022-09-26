using DevExpress.Blazor;
using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Actions;
using DevExpress.ExpressApp.Blazor.Components.Models;
using DevExpress.ExpressApp.Blazor.Components.Models.Renderers;
using DevExpress.ExpressApp.Blazor.Editors;
using DevExpress.ExpressApp.Blazor.Editors.Models;
using DevExpress.ExpressApp.Editors;
using DevExpress.ExpressApp.Layout;
using DevExpress.ExpressApp.Model.NodeGenerators;
using DevExpress.ExpressApp.SystemModule;
using DevExpress.ExpressApp.Templates;
using DevExpress.ExpressApp.Utils;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.Validation;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TestDxGridListEditorFilter.Module.BusinessObjects;

namespace TestDxGridListEditorFilter.Blazor.Server.Controllers
{
    // For more typical usage scenarios, be sure to check out https://documentation.devexpress.com/eXpressAppFramework/clsDevExpressExpressAppViewControllertopic.aspx.
    public partial class CustomizeDxGridEditorController : ViewController<ListView>
    {
        // Use CodeRush to create Controllers and Actions with a few keystrokes.
        // https://docs.devexpress.com/CodeRushForRoslyn/403133/
        public CustomizeDxGridEditorController()
        {
            InitializeComponent();
            // Target required Views (via the TargetXXX properties) and create their Actions.
        }
        protected override void OnActivated()
        {
            base.OnActivated();
            // Perform various tasks depending on the target View.
        }
        protected override void OnViewControlsCreated()
        {
            base.OnViewControlsCreated();
            // Access and customize the target View control.
            if (View.Editor is DxGridListEditor gridListEditor)
            {
                IDxGridAdapter dataGridAdapter = gridListEditor.GetGridAdapter();
                foreach (DxGridDataColumnModel item in dataGridAdapter.GridDataColumnModels)
                {
                    switch (item.FieldName)
                    {
                        case "Supervisor.Name":
                            item.FilterRowCellTemplate = CreateComboboxControlCore<Employee>();
                            break;
                    }
                }
            }
        }

        private RenderFragment<GridDataColumnFilterRowCellTemplateContext> CreateComboboxControlCore<T>()
        {
            DxComboBoxModel<DataItem<object>, object> componentModel = new DxComboBoxModel<DataItem<object>, object>();
            componentModel.Data = GetEmployees();
            componentModel.ClearButtonDisplayMode = DataEditorClearButtonDisplayMode.Auto;
            componentModel.ValueFieldName = "Value";
            componentModel.TextFieldName = "Text";
            return delegate (GridDataColumnFilterRowCellTemplateContext context)
            {
                componentModel.Value = context.FilterRowValue;
                componentModel.ValueChanged = EventCallback.Factory.Create<object>((object)this, (Action<object>)delegate (object val)
                {
                    context.FilterRowValue = val;
                });
                return componentModel.GetComponentContent();
            };
        }

        private IEnumerable<DataItem<object>> GetEmployees()
        {
            List<DataItem<object>> list = new List<DataItem<object>>();
            var employees = ObjectSpace.GetObjects<Employee>();
            foreach (var item in employees)
            {
                list.Add(new DataItem<object>(item.Name, item.Name));
            }

            return list;
        }

        protected override void OnDeactivated()
        {
            // Unsubscribe from previously subscribed events and release other references and resources.
            base.OnDeactivated();
        }
    }
}
