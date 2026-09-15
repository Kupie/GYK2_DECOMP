using System;
using System.Collections.Generic;
using LazyBearTechnology;
using UnityEngine;

// Token: 0x0200084B RID: 2123
public class UIMixInfoWidget : LazyWidget<UIMixInfoWidgetData>
{
	// Token: 0x06003674 RID: 13940 RVA: 0x00108220 File Offset: 0x00106420
	public override void Redraw()
	{
		base.Redraw();
		if (this.boostItemCell == null)
		{
			this.boostItemCell = UIPrefabsPooler.Instance.GetElementFromPool<UITooltipMixItemCell>(this.cellsParent);
		}
		if (this.ingredients == null)
		{
			this.ingredients = new List<UITooltipMixItemCell>();
			for (int i = 0; i < 3; i++)
			{
				UITooltipMixItemCell elementFromPool = UIPrefabsPooler.Instance.GetElementFromPool<UITooltipMixItemCell>(this.cellsParent);
				this.ingredients.Add(elementFromPool);
			}
		}
		for (int j = 0; j < this.ingredients.Count; j++)
		{
			this.ingredients[j].gameObject.SetActive(false);
		}
		this.boostItemCell.gameObject.SetActive(false);
		for (int k = 0; k < this.data.MixDef.ingredients.Length; k++)
		{
			this.ingredients[k].gameObject.SetActive(true);
			ItemDef data = GameBalance.Me.GetData<ItemDef>(this.data.MixDef.ingredients[k]);
			this.ingredients[k].uiItemCell.Draw(new Item(data.id, 1), false, -1, false, 1, false, 0, true, false, false, ItemRelatedWidgetState.NotSet, false);
			this.ingredients[k].runesLabel.text = data.GetRunesAsString();
			this.ingredients[k].runesLabel.gameObject.SetActive(false);
		}
		if (this.data.MixDef.BoostCraft != null)
		{
			this.boostItemCell.gameObject.SetActive(true);
			this.boostItemCell.runesLabel.text = this.data.MixDef.BoostCraft.GetBoostRunesAsString();
			this.boostItemCell.uiItemCell.DrawCustom(this.data.MixDef.BoostCraft.GetCraftResultIcon(null), 1, false, false);
			this.boostItemCell.uiItemCell.CustomTooltipShowAction = delegate(UIItemCell cell)
			{
				UITooltip.ShowAlchemyBoostInfo(cell.transform as RectTransform, this.data.MixDef.BoostCraft);
			};
			this.boostItemCell.runesLabel.gameObject.SetActive(false);
		}
	}

	// Token: 0x06003675 RID: 13941 RVA: 0x00002318 File Offset: 0x00000518
	protected override void TestDraw()
	{
	}

	// Token: 0x04002B77 RID: 11127
	[SerializeField]
	private Transform cellsParent;

	// Token: 0x04002B78 RID: 11128
	private List<UITooltipMixItemCell> ingredients;

	// Token: 0x04002B79 RID: 11129
	private UITooltipMixItemCell boostItemCell;
}
