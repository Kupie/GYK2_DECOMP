using System;
using System.Collections.Generic;
using LazyBearTechnology;
using UnityEngine;

// Token: 0x020006E2 RID: 1762
public class BuildManager : LazySingleton<BuildManager>
{
	// Token: 0x1700073F RID: 1855
	// (get) Token: 0x06002E96 RID: 11926 RVA: 0x000DF0E2 File Offset: 0x000DD2E2
	public BuildController BuildController
	{
		get
		{
			return this.buildController;
		}
	}

	// Token: 0x17000740 RID: 1856
	// (get) Token: 0x06002E97 RID: 11927 RVA: 0x000DF0EA File Offset: 0x000DD2EA
	// (set) Token: 0x06002E98 RID: 11928 RVA: 0x000DF0F2 File Offset: 0x000DD2F2
	public WorldZone WorldZone
	{
		get
		{
			return this.worldZone;
		}
		set
		{
			this.worldZone = value;
		}
	}

	// Token: 0x06002E99 RID: 11929 RVA: 0x000DF0FC File Offset: 0x000DD2FC
	public bool TryEnable(Wgo builder, Func<List<Inventory>> getAdditionalInventories = null)
	{
		if (!builder.TryGetNearestBuilderWorldZone(out this.worldZone))
		{
			Debug.LogError("Can not enable build mode, builder not in any world zones");
			return false;
		}
		if (!this.FormBuildData(builder))
		{
			Debug.LogError("Can not enable build mode, can not form build data");
			return false;
		}
		this.getAdditionalInventories = getAdditionalInventories;
		this.RefreshAdditionalInventories();
		this.OpenBuildingWindow(builder, this.additionalInventories);
		return true;
	}

	// Token: 0x06002E9A RID: 11930 RVA: 0x000DF154 File Offset: 0x000DD354
	public void EnableBuildMode(Wgo currentBuildDesk, BuildData selectedBuildData, List<NeedItemData> selectedItems, List<Inventory> additionalInventories = null)
	{
		this.currentBuildDesk = currentBuildDesk;
		this.additionalInventories = additionalInventories;
		MultiInventory multiInventory = new MultiInventory(MainGame.PlayerController.PlayerData, true);
		if (additionalInventories != null)
		{
			for (int i = 0; i < additionalInventories.Count; i++)
			{
				multiInventory.Add(additionalInventories[i]);
			}
		}
		MainGame.PlayerController.SetControlTakenType(TakenControlType.ByBuilding, false);
		this.buildController.EnableBuildMode(selectedBuildData, this.worldZone, selectedItems, multiInventory);
	}

	// Token: 0x06002E9B RID: 11931 RVA: 0x000DF1C6 File Offset: 0x000DD3C6
	public void Disable()
	{
		MainGame.PlayerController.SetControlTakenType(TakenControlType.ByBuilding, true);
		this.FormBuildData(this.currentBuildDesk);
		this.RefreshAdditionalInventories();
		this.OpenBuildingWindow(this.currentBuildDesk, this.additionalInventories);
	}

	// Token: 0x06002E9C RID: 11932 RVA: 0x000DF1FC File Offset: 0x000DD3FC
	private bool FormBuildData(Wgo buildDesk)
	{
		if (this.worldZone == null)
		{
			Debug.LogError("Can not form BuildData, player not in any world zones");
			return false;
		}
		List<BuildData> list = new List<BuildData>();
		if (buildDesk.Id == "test_playground_builder")
		{
			using (List<BuildingDef>.Enumerator enumerator = GameBalance.Me.buildingDefs.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					BuildingDef buildingDef = enumerator.Current;
					BuildingDef.BuildingMode buildingMode = buildingDef.buildingMode;
					if (buildingMode != BuildingDef.BuildingMode.None && buildingMode != BuildingDef.BuildingMode.Remove)
					{
						list.Add(BuildData.GetDataForBuild(buildingDef));
					}
				}
				goto IL_0086;
			}
		}
		list = BuildingDef.GetBuildingsInBuilder(buildDesk);
		IL_0086:
		this.buildDataList = list;
		return true;
	}

	// Token: 0x06002E9D RID: 11933 RVA: 0x000DF2A8 File Offset: 0x000DD4A8
	private void OpenBuildingWindow(Wgo builder, List<Inventory> additionalInventories = null)
	{
		BuildManager.<>c__DisplayClass15_0 CS$<>8__locals1 = new BuildManager.<>c__DisplayClass15_0();
		CS$<>8__locals1.additionalInventories = additionalInventories;
		CS$<>8__locals1.builder = builder;
		CS$<>8__locals1.buildWindow = LazyUI.GetWindow<UIBuildingWindow>();
		UIBuildingWindowData uibuildingWindowData = new UIBuildingWindowData(CS$<>8__locals1.builder, MainGame.PlayerController.PlayerData, this.buildDataList, new Action<BuildData, List<NeedItemData>>(CS$<>8__locals1.<OpenBuildingWindow>g__OnBuildPressed|1), new Func<BuildData, List<NeedItemData>, bool>(CS$<>8__locals1.<OpenBuildingWindow>g__CanBuild|0), CS$<>8__locals1.additionalInventories);
		CS$<>8__locals1.buildWindow.Open(uibuildingWindowData);
	}

	// Token: 0x06002E9E RID: 11934 RVA: 0x000DF31A File Offset: 0x000DD51A
	private void RefreshAdditionalInventories()
	{
		Func<List<Inventory>> func = this.getAdditionalInventories;
		this.additionalInventories = ((func != null) ? func() : null);
	}

	// Token: 0x040025AB RID: 9643
	[SerializeField]
	private BuildController buildController;

	// Token: 0x040025AC RID: 9644
	private Wgo currentBuildDesk;

	// Token: 0x040025AD RID: 9645
	private List<BuildData> buildDataList;

	// Token: 0x040025AE RID: 9646
	private WorldZone worldZone;

	// Token: 0x040025AF RID: 9647
	private List<Inventory> additionalInventories;

	// Token: 0x040025B0 RID: 9648
	private Func<List<Inventory>> getAdditionalInventories;
}
