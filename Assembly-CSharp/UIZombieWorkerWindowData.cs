using System;
using System.Collections.Generic;
using LazyBearTechnology;

// Token: 0x02000A7F RID: 2687
public class UIZombieWorkerWindowData : LazyWidgetDataBase
{
	// Token: 0x140000CC RID: 204
	// (add) Token: 0x06004937 RID: 18743 RVA: 0x0015A6DC File Offset: 0x001588DC
	// (remove) Token: 0x06004938 RID: 18744 RVA: 0x0015A710 File Offset: 0x00158910
	public static event Action OnBodyItemAddOrRemove;

	// Token: 0x17000B18 RID: 2840
	// (get) Token: 0x06004939 RID: 18745 RVA: 0x0015A743 File Offset: 0x00158943
	// (set) Token: 0x0600493A RID: 18746 RVA: 0x0015A74B File Offset: 0x0015894B
	public ZombieWgoData ZombieWgoData { get; private set; }

	// Token: 0x17000B19 RID: 2841
	// (get) Token: 0x0600493B RID: 18747 RVA: 0x0015A754 File Offset: 0x00158954
	// (set) Token: 0x0600493C RID: 18748 RVA: 0x0015A75C File Offset: 0x0015895C
	public BodyOrgansInventoryWidgetData BodyOrgansInventoryWidgetData { get; private set; }

	// Token: 0x17000B1A RID: 2842
	// (get) Token: 0x0600493D RID: 18749 RVA: 0x0015A765 File Offset: 0x00158965
	// (set) Token: 0x0600493E RID: 18750 RVA: 0x0015A76D File Offset: 0x0015896D
	public BodyPocketInventoryWidgetData BodyPocketInventoryWidgetData { get; private set; }

	// Token: 0x17000B1B RID: 2843
	// (get) Token: 0x0600493F RID: 18751 RVA: 0x0015A776 File Offset: 0x00158976
	// (set) Token: 0x06004940 RID: 18752 RVA: 0x0015A77E File Offset: 0x0015897E
	public ZombieEquipmentInventoryWidgetData ZombieEquipmentInventoryWidgetData { get; private set; }

	// Token: 0x17000B1C RID: 2844
	// (get) Token: 0x06004941 RID: 18753 RVA: 0x0015A787 File Offset: 0x00158987
	// (set) Token: 0x06004942 RID: 18754 RVA: 0x0015A78F File Offset: 0x0015898F
	public ZombieProgressionWidgetData ZombieProgressionWidgetData { get; private set; }

	// Token: 0x17000B1D RID: 2845
	// (get) Token: 0x06004943 RID: 18755 RVA: 0x0015A798 File Offset: 0x00158998
	// (set) Token: 0x06004944 RID: 18756 RVA: 0x0015A7A0 File Offset: 0x001589A0
	public UIZombieWorkerWindowData.ZombieState State { get; private set; }

	// Token: 0x06004945 RID: 18757 RVA: 0x0015A7A9 File Offset: 0x001589A9
	public UIZombieWorkerWindowData(ZombieWgoData zombieWgoData)
	{
		this.ZombieWgoData = zombieWgoData;
		this.State = UIZombieWorkerWindowData.ZombieState.Worker;
		this.CommonFill();
	}

	// Token: 0x06004946 RID: 18758 RVA: 0x0015A7C5 File Offset: 0x001589C5
	public UIZombieWorkerWindowData(DropData dropData)
	{
		this.ZombieWgoData = MainGame.ZombieSystemData.GetZombie(dropData.Item.UniqueId);
		this.State = UIZombieWorkerWindowData.ZombieState.Body;
		this.CommonFill();
	}

	// Token: 0x06004947 RID: 18759 RVA: 0x0015A7F8 File Offset: 0x001589F8
	private void CommonFill()
	{
		Inventory inventory = new Inventory(this.ZombieWgoData.ZombieItem);
		this.BodyOrgansInventoryWidgetData = new BodyOrgansInventoryWidgetData(true, this.ZombieWgoData, this.ZombieWgoData, inventory, null, null, null);
		this.BodyPocketInventoryWidgetData = new BodyPocketInventoryWidgetData(true, this.ZombieWgoData, this.ZombieWgoData, inventory, null, null, null);
		inventory.OnItemsAdd += delegate(List<Item> _)
		{
			Action onBodyItemAddOrRemove = UIZombieWorkerWindowData.OnBodyItemAddOrRemove;
			if (onBodyItemAddOrRemove == null)
			{
				return;
			}
			onBodyItemAddOrRemove();
		};
		inventory.OnItemsRemove += delegate(List<Item> _)
		{
			Action onBodyItemAddOrRemove2 = UIZombieWorkerWindowData.OnBodyItemAddOrRemove;
			if (onBodyItemAddOrRemove2 == null)
			{
				return;
			}
			onBodyItemAddOrRemove2();
		};
		this.ZombieEquipmentInventoryWidgetData = new ZombieEquipmentInventoryWidgetData(this.ZombieWgoData, inventory, null, null, false);
		this.ZombieProgressionWidgetData = new ZombieProgressionWidgetData(this.ZombieWgoData, null);
	}

	// Token: 0x02000A80 RID: 2688
	public enum ZombieState
	{
		// Token: 0x0400391E RID: 14622
		Body,
		// Token: 0x0400391F RID: 14623
		Worker
	}
}
