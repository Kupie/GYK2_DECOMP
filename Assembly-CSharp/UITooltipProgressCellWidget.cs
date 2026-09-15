using System;
using LazyBearTechnology;
using TMPro;
using UnityEngine;

// Token: 0x02000852 RID: 2130
public class UITooltipProgressCellWidget : LazyWidget<UITooltipProgressCellWidgetData>
{
	// Token: 0x0600369E RID: 13982 RVA: 0x00108D30 File Offset: 0x00106F30
	public override void Redraw()
	{
		base.Redraw();
		if (this.data.ItemDefBonus != null)
		{
			this.uiItemCell.Draw(new Item(this.data.ItemDefBonus.id, 1), false, -1, false, 1, false, 0, true, false, false, ItemRelatedWidgetState.NotSet, false);
			this.textLabel.text = LLBase.L(this.data.ItemDefBonus.id);
			this.uiItemCell.gameObject.SetActive(true);
			this.textLabel.gameObject.SetActive(true);
			return;
		}
		this.uiItemCell.gameObject.SetActive(false);
		this.textLabel.gameObject.SetActive(true);
		this.textLabel.text = LLBase.L(this.data.PerkDefBonus.id);
	}

	// Token: 0x0600369F RID: 13983 RVA: 0x00002318 File Offset: 0x00000518
	protected override void TestDraw()
	{
	}

	// Token: 0x04002B95 RID: 11157
	[SerializeField]
	private UIItemCell uiItemCell;

	// Token: 0x04002B96 RID: 11158
	[SerializeField]
	private TextMeshProUGUI textLabel;
}
