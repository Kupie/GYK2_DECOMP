using System;
using System.Collections.Generic;
using LazyBearTechnology;

// Token: 0x020009CC RID: 2508
public class UIPrefightSquadWidgetData : LazyWidgetDataBase
{
	// Token: 0x17000A1B RID: 2587
	// (get) Token: 0x060042CD RID: 17101 RVA: 0x0013D68E File Offset: 0x0013B88E
	// (set) Token: 0x060042CE RID: 17102 RVA: 0x0013D696 File Offset: 0x0013B896
	public WgoData WgoData { get; private set; }

	// Token: 0x17000A1C RID: 2588
	// (get) Token: 0x060042CF RID: 17103 RVA: 0x0013D69F File Offset: 0x0013B89F
	// (set) Token: 0x060042D0 RID: 17104 RVA: 0x0013D6A7 File Offset: 0x0013B8A7
	public int SquadPower { get; private set; }

	// Token: 0x17000A1D RID: 2589
	// (get) Token: 0x060042D1 RID: 17105 RVA: 0x0013D6B0 File Offset: 0x0013B8B0
	// (set) Token: 0x060042D2 RID: 17106 RVA: 0x0013D6B8 File Offset: 0x0013B8B8
	public List<ItemType> FightersWeapons { get; private set; }

	// Token: 0x17000A1E RID: 2590
	// (get) Token: 0x060042D3 RID: 17107 RVA: 0x0013D6C1 File Offset: 0x0013B8C1
	// (set) Token: 0x060042D4 RID: 17108 RVA: 0x0013D6C9 File Offset: 0x0013B8C9
	public List<ItemType> FightersArmors { get; private set; }

	// Token: 0x17000A1F RID: 2591
	// (get) Token: 0x060042D5 RID: 17109 RVA: 0x0013D6D2 File Offset: 0x0013B8D2
	// (set) Token: 0x060042D6 RID: 17110 RVA: 0x0013D6DA File Offset: 0x0013B8DA
	public Action<UIPrefightSquadWidget> OnPressedDefault { get; private set; }

	// Token: 0x17000A20 RID: 2592
	// (get) Token: 0x060042D7 RID: 17111 RVA: 0x0013D6E3 File Offset: 0x0013B8E3
	// (set) Token: 0x060042D8 RID: 17112 RVA: 0x0013D6EB File Offset: 0x0013B8EB
	public Action<UIPrefightSquadWidget> OnPressedTurnedOn { get; private set; }

	// Token: 0x17000A21 RID: 2593
	// (get) Token: 0x060042D9 RID: 17113 RVA: 0x0013D6F4 File Offset: 0x0013B8F4
	// (set) Token: 0x060042DA RID: 17114 RVA: 0x0013D6FC File Offset: 0x0013B8FC
	public bool IsMercenary { get; private set; }

	// Token: 0x17000A22 RID: 2594
	// (get) Token: 0x060042DB RID: 17115 RVA: 0x0013D705 File Offset: 0x0013B905
	// (set) Token: 0x060042DC RID: 17116 RVA: 0x0013D70D File Offset: 0x0013B90D
	public bool HasAnyFighterInSquad { get; private set; }

	// Token: 0x17000A23 RID: 2595
	// (get) Token: 0x060042DD RID: 17117 RVA: 0x0013D716 File Offset: 0x0013B916
	// (set) Token: 0x060042DE RID: 17118 RVA: 0x0013D71E File Offset: 0x0013B91E
	public bool IsTurnedOn { get; set; }

	// Token: 0x17000A24 RID: 2596
	// (get) Token: 0x060042DF RID: 17119 RVA: 0x0013D727 File Offset: 0x0013B927
	// (set) Token: 0x060042E0 RID: 17120 RVA: 0x0013D72F File Offset: 0x0013B92F
	public bool CanBeTurnedOn { get; set; }

	// Token: 0x17000A25 RID: 2597
	// (get) Token: 0x060042E1 RID: 17121 RVA: 0x0013D738 File Offset: 0x0013B938
	// (set) Token: 0x060042E2 RID: 17122 RVA: 0x0013D740 File Offset: 0x0013B940
	public Action OnRedraw { get; set; }

	// Token: 0x060042E3 RID: 17123 RVA: 0x0013D74C File Offset: 0x0013B94C
	public UIPrefightSquadWidgetData()
	{
		this.WgoData = null;
		this.SquadPower = 0;
		this.FightersWeapons = new List<ItemType>();
		this.FightersArmors = new List<ItemType>();
		this.OnPressedDefault = null;
		this.OnPressedTurnedOn = null;
		this.IsTurnedOn = false;
		this.CanBeTurnedOn = false;
	}

	// Token: 0x060042E4 RID: 17124 RVA: 0x0013D7A0 File Offset: 0x0013B9A0
	public UIPrefightSquadWidgetData(WgoData wgoData, bool isMercenary, bool isTurnedOn, bool canBeTurnedOn, Action<UIPrefightSquadWidget> onPressedDefault, Action<UIPrefightSquadWidget> onPressedTurnedOn)
	{
		this.WgoData = wgoData;
		this.OnPressedDefault = onPressedDefault;
		this.OnPressedTurnedOn = onPressedTurnedOn;
		this.IsTurnedOn = isTurnedOn;
		this.IsMercenary = isMercenary;
		this.CanBeTurnedOn = canBeTurnedOn;
		this.FightersWeapons = new List<ItemType>();
		this.FightersArmors = new List<ItemType>();
		for (int i = 0; i < this.WgoData.MainWgoPartData.DockPointsCount; i++)
		{
			DockPointData dockPointData = this.WgoData.MainWgoPartData.DockPointDataList[i];
			if (dockPointData.IsOccupied)
			{
				bool flag = false;
				bool flag2 = false;
				if (isMercenary)
				{
					WgoData wgoData2 = MainGame.WorldData.GetWgoData(dockPointData.OccupiedBy);
					this.SquadPower += (int)wgoData2.Quality;
					Item itemByType = wgoData2.Inventory.GetItemByType(ItemType.Pike);
					Item itemByType2 = wgoData2.Inventory.GetItemByType(ItemType.Bow);
					if (!itemByType.IsEmpty)
					{
						this.FightersWeapons.Add(ItemType.Pike);
						flag2 = true;
					}
					else if (!itemByType2.IsEmpty)
					{
						this.FightersWeapons.Add(ItemType.Bow);
						flag2 = true;
					}
					else
					{
						this.FightersWeapons.Add(ItemType.None);
					}
					Item itemByType3 = wgoData2.Inventory.GetItemByType(ItemType.BodyArmor);
					if (!itemByType3.IsEmpty)
					{
						this.FightersArmors.Add(itemByType3.Definition.type);
						flag = true;
					}
					else
					{
						this.FightersArmors.Add(ItemType.None);
					}
				}
				else
				{
					ZombieWgoData zombie = MainGame.ZombieSystemData.GetZombie(dockPointData.OccupiedBy);
					Item hand = zombie.Hand;
					Item armor = zombie.Armor;
					if ((!hand.IsEmpty && hand.Definition.type == ItemType.Pike) || hand.Definition.type == ItemType.Bow)
					{
						flag2 = true;
					}
					if (!armor.IsEmpty && armor.Definition.type == ItemType.BodyArmor)
					{
						flag = true;
					}
					if (flag && flag2)
					{
						this.SquadPower += hand.Definition.quality + armor.Definition.quality;
						this.FightersWeapons.Add(hand.Definition.type);
						this.FightersArmors.Add(armor.Definition.type);
					}
					else
					{
						this.FightersWeapons.Add(ItemType.None);
						this.FightersArmors.Add(ItemType.None);
					}
				}
				if (flag2 && flag)
				{
					this.HasAnyFighterInSquad = true;
				}
			}
		}
		if (isMercenary)
		{
			this.IsTurnedOn = true;
			this.HasAnyFighterInSquad = true;
		}
	}
}
