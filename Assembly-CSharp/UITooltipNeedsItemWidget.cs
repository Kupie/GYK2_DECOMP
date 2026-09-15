using System;
using System.Collections.Generic;
using LazyBearTechnology;
using UnityEngine;

// Token: 0x02000850 RID: 2128
public class UITooltipNeedsItemWidget : LazyWidget<UITooltipNeedsItemWidgetData>
{
	// Token: 0x0600368D RID: 13965 RVA: 0x001089F4 File Offset: 0x00106BF4
	public override void Redraw()
	{
		base.Redraw();
		this.HideCells();
		foreach (UICraftItemCellData uicraftItemCellData in this.data.CraftItemCellsData)
		{
			UITooltipCraftItemCell elementFromPool = UIPrefabsPooler.Instance.GetElementFromPool<UITooltipCraftItemCell>(this.cellsParent);
			elementFromPool.Draw(uicraftItemCellData, null, true, this.data.DrawAsNeedItem);
			elementFromPool.GamepadNavigationItem.group = 1;
			this.items.Add(elementFromPool);
		}
	}

	// Token: 0x0600368E RID: 13966 RVA: 0x00108A90 File Offset: 0x00106C90
	public override void Hide()
	{
		this.HideCells();
		base.Hide();
	}

	// Token: 0x0600368F RID: 13967 RVA: 0x00108AA0 File Offset: 0x00106CA0
	private void HideCells()
	{
		foreach (UITooltipCraftItemCell uitooltipCraftItemCell in this.items)
		{
			UIPrefabsPooler.Instance.ReleaseElementToPool<UITooltipCraftItemCell>(uitooltipCraftItemCell);
		}
		this.items.Clear();
	}

	// Token: 0x06003690 RID: 13968 RVA: 0x00002318 File Offset: 0x00000518
	protected override void TestDraw()
	{
	}

	// Token: 0x04002B8D RID: 11149
	[SerializeField]
	private Transform cellsParent;

	// Token: 0x04002B8E RID: 11150
	private List<UITooltipCraftItemCell> items = new List<UITooltipCraftItemCell>();
}
