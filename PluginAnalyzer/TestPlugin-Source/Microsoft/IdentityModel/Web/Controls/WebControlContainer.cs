using System;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Microsoft.IdentityModel.Web.Controls
{
	internal abstract class WebControlContainer<TWebControl> : WebControl where TWebControl : WebControl
	{
		[SupportsEventValidation]
		private class LayoutHelperTable : Table
		{
			private class LayoutTableCell : TableCell
			{
				protected override void AddedControl(Control control, int index)
				{
					if (control.Page == null)
					{
						control.Page = Page;
					}
				}

				protected override void RemovedControl(Control control)
				{
				}
			}

			public TableCell this[int row, int column] => Rows[row].Cells[column];

			public LayoutHelperTable(int rows, int columns, Page page)
			{
				if (rows <= 0)
				{
					throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentOutOfRange("rows");
				}
				if (columns <= 0)
				{
					throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentOutOfRange("columns");
				}
				if (page != null)
				{
					Page = page;
				}
				for (int i = 0; i < rows; i++)
				{
					TableRow tableRow = new TableRow();
					Rows.Add(tableRow);
					for (int j = 0; j < columns; j++)
					{
						TableCell cell = new LayoutTableCell();
						tableRow.Cells.Add(cell);
					}
				}
			}
		}

		internal const string DesignerRegionAttributeName = "_designerRegion";

		private const string _templateDesignerRegion = "0";

		private bool _renderDesignerRegion;

		private TWebControl _owner;

		private bool _ownerDesignMode;

		private Table _layoutTable;

		private Table _borderTable;

		internal Table BorderTable
		{
			get
			{
				return _borderTable;
			}
			set
			{
				_borderTable = value;
			}
		}

		protected abstract bool ConvertingToTemplate { get; }

		internal Table LayoutTable
		{
			get
			{
				return _layoutTable;
			}
			set
			{
				_layoutTable = value;
			}
		}

		internal TWebControl Owner => _owner;

		internal bool RenderDesignerRegion
		{
			get
			{
				if (_ownerDesignMode)
				{
					return _renderDesignerRegion;
				}
				return false;
			}
			set
			{
				_renderDesignerRegion = value;
			}
		}

		private bool UsingDefaultTemplate => BorderTable != null;

		public WebControlContainer(TWebControl owner, bool ownerDesignMode)
		{
			_owner = owner;
			_ownerDesignMode = ownerDesignMode;
		}

		public sealed override void Focus()
		{
			throw DiagnosticUtil.ExceptionUtil.ThrowHelperError(new NotSupportedException(SR.GetString("ID5014", GetType().Name)));
		}

		protected override void Render(HtmlTextWriter writer)
		{
			if (UsingDefaultTemplate)
			{
				if (!ConvertingToTemplate)
				{
					BorderTable.CopyBaseAttributes(this);
					if (base.ControlStyleCreated)
					{
						ControlUtil.CopyBorderStyles(BorderTable, base.ControlStyle);
						ControlUtil.CopyStyleToInnerControl(LayoutTable, base.ControlStyle);
					}
				}
				LayoutTable.Height = Height;
				LayoutTable.Width = Width;
				RenderContents(writer);
			}
			else
			{
				RenderContentsInUnitTable(writer);
			}
		}

		private void RenderContentsInUnitTable(HtmlTextWriter writer)
		{
			LayoutHelperTable layoutHelperTable = new LayoutHelperTable(1, 1, Page);
			if (RenderDesignerRegion)
			{
				layoutHelperTable[0, 0].Attributes["_designerRegion"] = "0";
			}
			else
			{
				foreach (Control control in Controls)
				{
					layoutHelperTable[0, 0].Controls.Add(control);
				}
			}
			string iD = Parent.ID;
			if (iD != null && iD.Length != 0)
			{
				layoutHelperTable.ID = Parent.ClientID;
			}
			layoutHelperTable.CopyBaseAttributes(this);
			layoutHelperTable.ApplyStyle(base.ControlStyle);
			layoutHelperTable.CellPadding = 0;
			layoutHelperTable.CellSpacing = 0;
			layoutHelperTable.RenderControl(writer);
		}
	}
}
