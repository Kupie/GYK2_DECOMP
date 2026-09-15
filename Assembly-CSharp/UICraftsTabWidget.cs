using System;
using System.Collections.Generic;
using System.Linq;
using LazyBearTechnology;
using UnityEngine;

// Token: 0x02000994 RID: 2452
public class UICraftsTabWidget : LazyWidget<UICraftsTabWidgetData>
{
	// Token: 0x0600415E RID: 16734 RVA: 0x00137643 File Offset: 0x00135843
	public override void Redraw()
	{
		base.Redraw();
		this.DrawCraftsCells();
	}

	// Token: 0x0600415F RID: 16735 RVA: 0x00137651 File Offset: 0x00135851
	public override void Hide()
	{
		base.Hide();
		this.HideCraftsCells();
	}

	// Token: 0x06004160 RID: 16736 RVA: 0x00137660 File Offset: 0x00135860
	private void DrawCraftsCells()
	{
		UICraftWindow craftWindow = LazyUI.GetWindow<UICraftWindow>();
		string text = this.data.Crafts[0].tabId;
		bool flag = this.data.Crafts.Any((CraftDef x) => x.craftsIn.Contains(craftWindow.Data.AssignedWgo.Data.id));
		if (string.IsNullOrEmpty(text))
		{
			text = craftWindow.Data.AssignedWgo.Data.Definition.id;
		}
		this.cellWidgets = new List<UICraftPreviewItemCell>();
		bool flag2 = craftWindow.Data.ExtensionCrafts.Exists((KeyValuePair<string, List<CraftDef>> x) => x.Key == this.data.TabId);
		if (this.data.WgoData.id == "alchemy_workbench" || flag2)
		{
			text = this.data.TabId;
		}
		if (flag2 || !flag)
		{
			string text2 = text;
			bool flag3 = false;
			BuildingDef buildingDef;
			if (flag2 && this.data.WgoData.id != "alchemy_workbench" && GameBalance.Me.buildableWgos.TryGetValue(this.data.TabId, out buildingDef))
			{
				text = buildingDef.BuildResultIcon;
				text2 = buildingDef.wgoId;
				flag3 = true;
			}
			UICraftPreviewItemCell craftWidget = craftWindow.GetCraftWidget(this.craftsParent.transform, flag2 ? new UICraftPreviewItemCellData(text, this.data.TabId, this.data.WgoData.WorkbenchExtensionsCrafts.ContainsKey(text2), flag3, this.data.IsGravePartRemove, this.GetTabBackgroundIcon()) : new UICraftPreviewItemCellData(null, null, null, null, text, this.data.IsGravePartRemove, this.GetTabBackgroundIcon()));
			craftWidget.transform.SetParent(this.craftsParent.transform);
			this.cellWidgets.Add(craftWidget);
		}
		foreach (CraftDef craftDef in this.data.Crafts)
		{
			UICraftPreviewItemCellData uicraftPreviewItemCellData = new UICraftPreviewItemCellData(craftDef, this.data.WgoData, this.data.OnCraftToQueueAdded, this.data.OnCraftStarted, "", this.data.IsGravePartRemove, null);
			UICraftPreviewItemCell craftWidget2 = craftWindow.GetCraftWidget(this.craftsParent.transform, uicraftPreviewItemCellData);
			craftWidget2.transform.SetParent(this.craftsParent.transform);
			this.cellWidgets.Add(craftWidget2);
		}
	}

	// Token: 0x06004161 RID: 16737 RVA: 0x001378F0 File Offset: 0x00135AF0
	private void HideCraftsCells()
	{
		UICraftWindow window = LazyUI.GetWindow<UICraftWindow>();
		foreach (UICraftPreviewItemCell uicraftPreviewItemCell in this.cellWidgets)
		{
			window.ReleaseCraftWidget(uicraftPreviewItemCell);
		}
		this.cellWidgets.Clear();
	}

	// Token: 0x06004162 RID: 16738 RVA: 0x00137954 File Offset: 0x00135B54
	private Sprite GetTabBackgroundIcon()
	{
		if (this.data.WgoData != null && this.data.WgoData.Definition.conveyorType != ConveyorElementType.None)
		{
			return this.conveyorTabBgIcon;
		}
		return this.defaultTabBgIcon;
	}

	// Token: 0x06004163 RID: 16739 RVA: 0x00002318 File Offset: 0x00000518
	protected override void TestDraw()
	{
	}

	// Token: 0x0400331F RID: 13087
	[SerializeField]
	private Transform craftsParent;

	// Token: 0x04003320 RID: 13088
	[Space]
	[SerializeField]
	private Sprite defaultTabBgIcon;

	// Token: 0x04003321 RID: 13089
	[SerializeField]
	private Sprite conveyorTabBgIcon;

	// Token: 0x04003322 RID: 13090
	private List<UICraftPreviewItemCell> cellWidgets;
}
