using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x020008FA RID: 2298
public class ZombieEquipmentInventoryWidget : InventoryWidgetBase<ZombieEquipmentInventoryWidgetData>
{
	// Token: 0x1700090F RID: 2319
	// (get) Token: 0x06003C18 RID: 15384 RVA: 0x0011F02E File Offset: 0x0011D22E
	private ZombieEquipmentInventoryWidgetData Data
	{
		get
		{
			return this.data as ZombieEquipmentInventoryWidgetData;
		}
	}

	// Token: 0x06003C19 RID: 15385 RVA: 0x0011F03C File Offset: 0x0011D23C
	public override void Redraw()
	{
		base.Redraw();
		this.SubscribeToInventoryEvents();
		this.armorCell.DrawEmptyInteractable();
		this.collarCell.DrawEmptyInteractable();
		this.handCell.DrawEmptyInteractable();
		int num = 0;
		int num2 = 0;
		if (!this.Data.ZombieWgoData.Armor.IsEmpty)
		{
			this.armorCell.Draw(this.Data.ZombieWgoData.Armor, ItemRelatedWidgetState.NotSet, true);
			num = this.Data.ZombieWgoData.ArmorValue;
		}
		if (!this.Data.ZombieWgoData.Collar.IsEmpty)
		{
			this.collarCell.Draw(this.Data.ZombieWgoData.Collar, ItemRelatedWidgetState.NotSet, true);
			this.collarCell.UIItemCell.OnItemCellPress = new Action<UIItemCell>(this.Data.OnCollarCellPressed);
		}
		if (!this.Data.ZombieWgoData.Hand.IsEmpty)
		{
			this.handCell.Draw(this.Data.ZombieWgoData.Hand, ItemRelatedWidgetState.NotSet);
			num2 = this.Data.ZombieWgoData.AttackValue;
		}
		this.handCell.UIItemCell.OnItemCellPress = new Action<UIItemCell>(this.Data.OnInstrumentCellPressed);
		this.handCell.UIItemCell.OnItemCellPress2 = new Action<UIItemCell>(this.Data.OnInstrumentCellPressed2);
		this.armorCell.UIItemCell.OnItemCellPress = new Action<UIItemCell>(this.Data.OnArmorCellPressed);
		this.armorCell.UIItemCell.OnItemCellPress2 = new Action<UIItemCell>(this.Data.OnArmorCellPressed2);
		this.armorLabel.text = string.Format("{0}{1}", "equip_icon_armor".FontIcon(), num);
		this.handLabel.text = string.Format("{0}{1}", "equip_icon_sword".FontIcon(), num2);
	}

	// Token: 0x06003C1A RID: 15386 RVA: 0x0011F226 File Offset: 0x0011D426
	protected override void ClearCallbacks()
	{
		this.UnsubscribeFromInventoryEvents();
		base.ClearCallbacks();
	}

	// Token: 0x06003C1B RID: 15387 RVA: 0x0011F234 File Offset: 0x0011D434
	public override void UpdateItemRelatedWidgetStateForCells()
	{
		this.armorCell.UIItemCell.SetWidgetState(this.data.ItemRelatedWidgetState);
		this.handCell.UIItemCell.SetWidgetState(this.data.ItemRelatedWidgetState);
	}

	// Token: 0x06003C1C RID: 15388 RVA: 0x00002318 File Offset: 0x00000518
	protected override void TestDraw()
	{
	}

	// Token: 0x04002F4B RID: 12107
	[SerializeField]
	private TextMeshProUGUI armorLabel;

	// Token: 0x04002F4C RID: 12108
	[SerializeField]
	private TextMeshProUGUI handLabel;

	// Token: 0x04002F4D RID: 12109
	[SerializeField]
	private UIFixedTypeItemCell collarCell;

	// Token: 0x04002F4E RID: 12110
	[SerializeField]
	private UIFixedTypeItemCell armorCell;

	// Token: 0x04002F4F RID: 12111
	[SerializeField]
	private UIGroupsItemCell handCell;
}
