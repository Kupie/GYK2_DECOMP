using System;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

// Token: 0x02000997 RID: 2455
public class UICraftWidget : UIBaseCraftWidget<UICraftWidgetData>
{
	// Token: 0x06004175 RID: 16757 RVA: 0x00137A6F File Offset: 0x00135C6F
	public override void Init()
	{
		base.Init();
		base.AddToQueueButton.onClick.AddListener(new UnityAction(base.OnPress));
	}

	// Token: 0x06004176 RID: 16758 RVA: 0x00137A94 File Offset: 0x00135C94
	public override void Redraw()
	{
		base.Redraw();
		this.boostItemCell.gameObject.SetActive(false);
		this.description.text = this.data.CraftDefinition.Description;
		base.AddToQueueButton.gameObject.SetActive(!this.data.CraftDefinition.isAuto && !this.data.CraftDefinition.isMulticraftDisabled);
		CraftDef craftDefinition = this.data.CraftDefinition;
		AlchemyMixDef mixDef = craftDefinition as AlchemyMixDef;
		if (mixDef != null && mixDef.BoostCraft != null)
		{
			this.boostItemCell.gameObject.SetActive(true);
			this.boostItemCell.runesLabel.text = mixDef.BoostCraft.GetBoostRunesAsString();
			this.boostItemCell.uiItemCell.DrawCustom(mixDef.BoostCraft.GetCraftResultIcon(this.data.WgoData), 1, true, false);
			this.boostItemCell.uiItemCell.CustomTooltipShowAction = delegate(UIItemCell cell)
			{
				UITooltip.ShowAlchemyBoostInfo(cell.transform as RectTransform, mixDef.BoostCraft);
			};
		}
	}

	// Token: 0x06004177 RID: 16759 RVA: 0x00137BBC File Offset: 0x00135DBC
	public override void DeInit()
	{
		base.DeInit();
		base.AddToQueueButton.onClick.RemoveAllListeners();
	}

	// Token: 0x06004178 RID: 16760 RVA: 0x00002318 File Offset: 0x00000518
	protected override void TestDraw()
	{
	}

	// Token: 0x0400332B RID: 13099
	[SerializeField]
	private TextMeshProUGUI description;

	// Token: 0x0400332C RID: 13100
	[SerializeField]
	private UIMixItemCell boostItemCell;
}
