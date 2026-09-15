using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020003C6 RID: 966
[Serializable]
public class TownBuildingWgoComponent : IComponent
{
	// Token: 0x17000471 RID: 1137
	// (get) Token: 0x060019D7 RID: 6615 RVA: 0x00079929 File Offset: 0x00077B29
	// (set) Token: 0x060019D8 RID: 6616 RVA: 0x00079931 File Offset: 0x00077B31
	public TownBuildingSceneConfiguration SceneConfiguration
	{
		get
		{
			return this.sceneConfiguration;
		}
		set
		{
			this.sceneConfiguration = value;
		}
	}

	// Token: 0x17000472 RID: 1138
	// (get) Token: 0x060019D9 RID: 6617 RVA: 0x0007993A File Offset: 0x00077B3A
	// (set) Token: 0x060019DA RID: 6618 RVA: 0x00079942 File Offset: 0x00077B42
	public int TierIndex
	{
		get
		{
			return this.tierIndex;
		}
		set
		{
			this.tierIndex = value;
		}
	}

	// Token: 0x17000473 RID: 1139
	// (get) Token: 0x060019DB RID: 6619 RVA: 0x0007994B File Offset: 0x00077B4B
	// (set) Token: 0x060019DC RID: 6620 RVA: 0x00079953 File Offset: 0x00077B53
	public string TownBuildingId
	{
		get
		{
			return this.townBuildingId;
		}
		set
		{
			this.townBuildingId = value;
			this.isActive = !string.IsNullOrEmpty(value);
		}
	}

	// Token: 0x17000474 RID: 1140
	// (get) Token: 0x060019DD RID: 6621 RVA: 0x0007996B File Offset: 0x00077B6B
	public TownBuildingDef TownBuildingDef
	{
		get
		{
			return GameBalance.Me.GetData<TownBuildingDef>(this.townBuildingId);
		}
	}

	// Token: 0x17000475 RID: 1141
	// (get) Token: 0x060019DE RID: 6622 RVA: 0x0007997D File Offset: 0x00077B7D
	public bool HasLevelUp
	{
		get
		{
			return !string.IsNullOrEmpty(this.TownBuildingDef.lvlUpId);
		}
	}

	// Token: 0x17000476 RID: 1142
	// (get) Token: 0x060019DF RID: 6623 RVA: 0x00079992 File Offset: 0x00077B92
	public bool HasVendor
	{
		get
		{
			return !string.IsNullOrEmpty(this.TownBuildingDef.vendorId);
		}
	}

	// Token: 0x17000477 RID: 1143
	// (get) Token: 0x060019E0 RID: 6624 RVA: 0x000799A7 File Offset: 0x00077BA7
	public bool IsActive
	{
		get
		{
			return this.isActive;
		}
	}

	// Token: 0x060019E1 RID: 6625 RVA: 0x000799B0 File Offset: 0x00077BB0
	public List<TownBuildingDef> GetAvailableBuildings(WgoData wgoData)
	{
		List<TownBuildingDef> list = new List<TownBuildingDef>();
		foreach (TownBuildingDef townBuildingDef in GameBalance.Me.townBuildingDefs)
		{
			if ((!townBuildingDef.isNeedsUnlock || MainGame.Instance.GameSave.knowledgeSystem.unlockedTownBuildings.Contains(townBuildingDef.id)) && !MainGame.Instance.GameSave.knowledgeSystem.lockedTownBuildings.Contains(townBuildingDef.id) && townBuildingDef.craftsIn.Contains(wgoData.id))
			{
				list.Add(townBuildingDef);
			}
		}
		return list;
	}

	// Token: 0x0400191F RID: 6431
	[SerializeField]
	private TownBuildingSceneConfiguration sceneConfiguration;

	// Token: 0x04001920 RID: 6432
	[SerializeField]
	private int tierIndex;

	// Token: 0x04001921 RID: 6433
	[SerializeField]
	private string townBuildingId;

	// Token: 0x04001922 RID: 6434
	[SerializeField]
	private bool isActive;
}
