using System;
using LazyBearTechnology;
using TMPro;
using UnityEngine;

// Token: 0x020008E8 RID: 2280
public class UIAlchemyFormulaWidget : LazyWidget<UIAlchemyFormulaWidgetData>
{
	// Token: 0x06003BAB RID: 15275 RVA: 0x0011D2CC File Offset: 0x0011B4CC
	public override void Redraw()
	{
		base.Redraw();
		ItemDef displayItemDef = this.data.GetDisplayItemDef();
		this.nameLabel.text = LLBase.L(displayItemDef.id);
		this.runesLabel.text = this.data.GetRunesAsString();
		this.itemCell.Draw(new Item(displayItemDef.id, 1), false, -1, false, 1, false, 0, true, false, false, ItemRelatedWidgetState.NotSet, false);
	}

	// Token: 0x06003BAC RID: 15276 RVA: 0x0011D33C File Offset: 0x0011B53C
	[LazyUITest]
	protected override void TestDraw()
	{
		this.Draw(new UIAlchemyFormulaWidgetData
		{
			AlchemyFormulaDef = GameBalance.Me.GetData<AlchemyFormulaDef>("heal_potion")
		});
	}

	// Token: 0x04002F06 RID: 12038
	[SerializeField]
	private UIItemCell itemCell;

	// Token: 0x04002F07 RID: 12039
	[SerializeField]
	private TextMeshProUGUI nameLabel;

	// Token: 0x04002F08 RID: 12040
	[SerializeField]
	private TextMeshProUGUI runesLabel;
}
